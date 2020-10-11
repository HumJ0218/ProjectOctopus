using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        private ChromiumWebBrowser chromiumWebBrowser;
        private Stack<string> BackUrls;
        private Stack<string> FowardUrls;
        private Dictionary<Guid, ICrawlerPlugin> CrawlerPlugins;

        public Form1()
        {
            InitializeComponent();

            CreateChromiumWebBrowser(out Action AfterInitializeComponent);
            AfterInitializeComponent();

            LoadPlugins("./plugins");
        }

        private void LoadPlugins(string pluginsPath)
        {
            List<ICrawlerPlugin> plugins = new List<ICrawlerPlugin>();

            Directory.CreateDirectory(pluginsPath);
            foreach (string file in Directory.GetFiles(pluginsPath, "*.dll"))
            {
                try
                {
                    Assembly asm = Assembly.LoadFrom(file);
                    IEnumerable<Type> types = asm.GetTypes().Where(t => t.GetInterfaces().Any(i => i.Name == nameof(ICrawlerPlugin)));

                    foreach (Type type in types)
                    {
                        try
                        {
                            ICrawlerPlugin instance = type.GetConstructor(new Type[0]).Invoke(new object[0]) as ICrawlerPlugin;
                            instance.Initialize(chromiumWebBrowser);
                            chromiumWebBrowser.JavascriptObjectRepository.Register(instance.JsObjectName, instance, false);

                            plugins.Add(instance);
                        }
                        catch (Exception ex)
                        {
                            ConsoleLogger.Error(ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Error(ex);
                }
            }

            try
            {
                CrawlerPlugins = plugins.ToDictionary(p => p.ID, p => p);
            }
            catch (Exception ex)
            {
                CrawlerPlugins = new Dictionary<Guid, ICrawlerPlugin>();

                ConsoleLogger.Error(ex);
            }
        }

        private void CreateChromiumWebBrowser(out Action AfterInitializeComponent)
        {
            CefSettings cefSettings = new CefSettings();
            try
            {
                cefSettings = JsonConvert.DeserializeObject<CefSettings>(File.ReadAllText("./cefSettings.json"));
                cefSettings.BrowserSubprocessPath = Path.GetFullPath(cefSettings.BrowserSubprocessPath);
            }
            catch (Exception)
            {
                ;
            }

            if (!Cef.Initialize(cefSettings))
            {
                if (Environment.GetCommandLineArgs().Contains("--type=renderer"))
                {
                    Environment.Exit(0);
                }
            }

            BackUrls = new Stack<string>();
            FowardUrls = new Stack<string>();

            chromiumWebBrowser = new ChromiumWebBrowser("https://www.baidu.com/")
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, toolStrip.Height, 0, 0),

            };

            chromiumWebBrowser.TitleChanged += ChromiumWebBrowser_TitleChanged;
            chromiumWebBrowser.LoadError += ChromiumWebBrowser_LoadError;
            chromiumWebBrowser.AddressChanged += ChromiumWebBrowser_AddressChanged;
            chromiumWebBrowser.StatusMessage += ChromiumWebBrowser_StatusMessage;
            chromiumWebBrowser.ConsoleMessage += ChromiumWebBrowser_ConsoleMessage;
            chromiumWebBrowser.LoadingStateChanged += ChromiumWebBrowser_LoadingStateChanged;
            chromiumWebBrowser.FrameLoadEnd += ChromiumWebBrowser_FrameLoadEnd;
            chromiumWebBrowser.FrameLoadStart += ChromiumWebBrowser_FrameLoadStart;
            chromiumWebBrowser.JavascriptMessageReceived += ChromiumWebBrowser_JavascriptMessageReceived;
            chromiumWebBrowser.IsBrowserInitializedChanged += ChromiumWebBrowser_IsBrowserInitializedChanged;

            panel.Controls.Add(chromiumWebBrowser);

            AfterInitializeComponent = new Action(() =>
             {
                 //chromiumWebBrowser.Language = XmlLanguage.GetLanguage("zh-cn");

                 try
                 {
                     JToken bs = JToken.Parse(File.ReadAllText("./browserSettings.json"));
                     Type ibs = typeof(IBrowserSettings);

                     foreach (PropertyInfo p in ibs.GetProperties().Where(p => p.CanWrite))
                     {
                         try
                         {
                             object v = bs[p.Name]?.ToObject(p.PropertyType);
                             if (v == null)
                             {
                                 ;
                             }
                             else
                             {
                                 p.SetValue(chromiumWebBrowser.BrowserSettings, v);
                             }
                         }
                         catch (Exception)
                         {
                             ;
                         }
                     }
                 }
                 catch (Exception)
                 {
                     ;
                 }
             });
        }

        #region 窗体事件

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (ToolStripItem i in statusStrip.Items)
            {
                if (i is ToolStripStatusLabel)
                {
                    (i as ToolStripStatusLabel).Text = "";
                }
                else
                {
                    i.Visible = false;
                }
            }
        }

        private void toolStripButton_back_Click(object sender, EventArgs e)
        {
            ConsoleLogger.Debug(nameof(toolStripButton_back_Click) + " : " + new
            {
            });

            chromiumWebBrowser?.GetBrowser().GoBack();
        }

        private void toolStripButton_forward_Click(object sender, EventArgs e)
        {
            ConsoleLogger.Debug(nameof(toolStripButton_forward_Click) + " : " + new
            {
            });

            chromiumWebBrowser?.GetBrowser().GoForward();
        }

        private void toolStripButton_reload_Click(object sender, EventArgs e)
        {
            ConsoleLogger.Debug(nameof(toolStripButton_reload_Click) + " : " + new
            {
            });

            chromiumWebBrowser?.GetBrowser().Reload(true);
        }
        private void toolStripTextBox_cefAddress_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Enter:
                    {
                        Invoke(new Action(() =>
                        {
                            chromiumWebBrowser.Load((sender as ToolStripTextBox).Text);
                        }));

                        break;
                    }
            }
        }

        #endregion

        #region CefSharp 事件

        private void ChromiumWebBrowser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            ConsoleLogger.Debug(nameof(ChromiumWebBrowser_TitleChanged) + " : " + new
            {
                e.Title,
            });

            Invoke(new Action(() =>
            {
                Text = e.Title;
            }));

            foreach (ICrawlerPlugin plugin in CrawlerPlugins.Values)
            {
                try
                {
                    plugin.ChromiumWebBrowser_TitleChanged(sender, e);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Warn($"{plugin.Title}.{nameof(plugin.ChromiumWebBrowser_TitleChanged)} : {ex}");
                }
            }
        }

        private void ChromiumWebBrowser_LoadError(object sender, LoadErrorEventArgs e)
        {
            ConsoleLogger.Error(nameof(ChromiumWebBrowser_LoadError) + " : " + new
            {
                e.ErrorCode,
                e.ErrorText,
                e.FailedUrl,
            });

            foreach (ICrawlerPlugin plugin in CrawlerPlugins.Values)
            {
                try
                {
                    plugin.ChromiumWebBrowser_LoadError(sender, e);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Warn($"{plugin.Title}.{nameof(plugin.ChromiumWebBrowser_LoadError)} : {ex}");
                }
            }
        }

        private void ChromiumWebBrowser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            ConsoleLogger.Debug(nameof(ChromiumWebBrowser_AddressChanged) + " : " + new
            {
                e.Address,
            });

            BackUrls.Push(e.Address);

            Invoke(new Action(() =>
            {
                toolStripTextBox_cefAddress.Text = e.Address;
            }));

            foreach (ICrawlerPlugin plugin in CrawlerPlugins.Values)
            {
                try
                {
                    plugin.ChromiumWebBrowser_AddressChanged(sender, e);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Warn($"{plugin.Title}.{nameof(plugin.ChromiumWebBrowser_AddressChanged)} : {ex}");
                }
            }
        }

        private void ChromiumWebBrowser_StatusMessage(object sender, StatusMessageEventArgs e)
        {
            ConsoleLogger.Debug(nameof(ChromiumWebBrowser_StatusMessage) + " : " + new
            {
                e.Value,
            });

            Invoke(new Action(() =>
            {
                toolStripStatusLabel_message.Text = e.Value;
            }));

            foreach (ICrawlerPlugin plugin in CrawlerPlugins.Values)
            {
                try
                {
                    plugin.ChromiumWebBrowser_StatusMessage(sender, e);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Warn($"{plugin.Title}.{nameof(plugin.ChromiumWebBrowser_StatusMessage)} : {ex}");
                }
            }
        }

        private void ChromiumWebBrowser_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            switch (e.Level)
            {
                case LogSeverity.Default:
                    {
                        ConsoleLogger.InfoBrighter(nameof(ChromiumWebBrowser_ConsoleMessage) + " : " + new
                        {
                            e.Level,
                            e.Line,
                            e.Message,
                            e.Source,
                        });
                        break;
                    }
                case LogSeverity.Disable:
                    {
                        ConsoleLogger.DebugBrighter(nameof(ChromiumWebBrowser_ConsoleMessage) + " : " + new
                        {
                            e.Level,
                            e.Line,
                            e.Message,
                            e.Source,
                        });
                        break;
                    }
                case LogSeverity.Error:
                    {
                        ConsoleLogger.ErrorBrighter(nameof(ChromiumWebBrowser_ConsoleMessage) + " : " + new
                        {
                            e.Level,
                            e.Line,
                            e.Message,
                            e.Source,
                        });
                        break;
                    }
                case LogSeverity.Fatal:
                    {
                        ConsoleLogger.FatalBrighter(nameof(ChromiumWebBrowser_ConsoleMessage) + " : " + new
                        {
                            e.Level,
                            e.Line,
                            e.Message,
                            e.Source,
                        });
                        break;
                    }
                case LogSeverity.Info:
                    {
                        ConsoleLogger.InfoBrighter(nameof(ChromiumWebBrowser_ConsoleMessage) + " : " + new
                        {
                            e.Level,
                            e.Line,
                            e.Message,
                            e.Source,
                        });
                        break;
                    }
                case LogSeverity.Verbose:
                    {
                        ConsoleLogger.Debug(nameof(ChromiumWebBrowser_ConsoleMessage) + " : " + new
                        {
                            e.Level,
                            e.Line,
                            e.Message,
                            e.Source,
                        });
                        break;
                    }
                case LogSeverity.Warning:
                    {
                        ConsoleLogger.WarnBrighter(nameof(ChromiumWebBrowser_ConsoleMessage) + " : " + new
                        {
                            e.Level,
                            e.Line,
                            e.Message,
                            e.Source,
                        });
                        break;
                    }
            }

            foreach (ICrawlerPlugin plugin in CrawlerPlugins.Values)
            {
                try
                {
                    plugin.ChromiumWebBrowser_ConsoleMessage(sender, e);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Warn($"{plugin.Title}.{nameof(plugin.ChromiumWebBrowser_ConsoleMessage)} : {ex}");
                }
            }
        }

        private void ChromiumWebBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            ConsoleLogger.Debug(nameof(ChromiumWebBrowser_LoadingStateChanged) + " : " + new
            {
                e.CanGoBack,
                e.CanGoForward,
                e.CanReload,
                e.IsLoading,
            });

            Invoke(new Action(() =>
            {
                toolStripButton_back.Enabled = e.CanGoBack;
                toolStripButton_forward.Enabled = e.CanGoForward;
                toolStripButton_reload.Enabled = e.CanReload;

                toolStripProgressBar_loading.Visible = e.IsLoading;
                toolStripStatusLabel_loading.Visible = e.IsLoading;
            }));

            foreach (ICrawlerPlugin plugin in CrawlerPlugins.Values)
            {
                try
                {
                    plugin.ChromiumWebBrowser_LoadingStateChanged(sender, e);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Warn($"{plugin.Title}.{nameof(plugin.ChromiumWebBrowser_LoadingStateChanged)} : {ex}");
                }
            }
        }

        private void ChromiumWebBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            ConsoleLogger.Debug(nameof(ChromiumWebBrowser_FrameLoadEnd) + " : " + new
            {
                e.HttpStatusCode,
                e.Url,
            });

            Invoke(new Action(() =>
            {
                toolStripStatusLabel_loading.Text = $"{e.HttpStatusCode} {e.Url}";
            }));

            foreach (ICrawlerPlugin plugin in CrawlerPlugins.Values)
            {
                Task.Run(() =>
                {
                    try
                    {
                        chromiumWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync($"CefSharp.BindObjectAsync('{plugin.JsObjectName}')");
                        Thread.Sleep(100);
                        plugin.ChromiumWebBrowser_FrameLoadEnd(sender, e);
                    }
                    catch (Exception ex)
                    {
                        ConsoleLogger.Warn($"{plugin.Title}.{nameof(plugin.ChromiumWebBrowser_FrameLoadEnd)} : {ex}");
                    }
                });
            }
        }

        private void ChromiumWebBrowser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            ConsoleLogger.Debug(nameof(ChromiumWebBrowser_FrameLoadStart) + " : " + new
            {
                e.TransitionType,
                e.Url,
            });

            Invoke(new Action(() =>
            {
                toolStripStatusLabel_loading.Text = $"{e.TransitionType} {e.Url}";
            }));

            foreach (ICrawlerPlugin plugin in CrawlerPlugins.Values)
            {
                try
                {
                    plugin.ChromiumWebBrowser_FrameLoadStart(sender, e);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Warn($"{plugin.Title}.{nameof(plugin.ChromiumWebBrowser_FrameLoadStart)} : {ex}");
                }
            }
        }

        private void ChromiumWebBrowser_JavascriptMessageReceived(object sender, JavascriptMessageReceivedEventArgs e)
        {
            ConsoleLogger.Debug(nameof(ChromiumWebBrowser_JavascriptMessageReceived) + " : " + new
            {
                e.Message,
            });

            foreach (ICrawlerPlugin plugin in CrawlerPlugins.Values)
            {
                try
                {
                    plugin.ChromiumWebBrowser_JavascriptMessageReceived(sender, e);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Warn($"{plugin.Title}.{nameof(plugin.ChromiumWebBrowser_JavascriptMessageReceived)} : {ex}");
                }
            }
        }

        private void ChromiumWebBrowser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            ConsoleLogger.Debug(nameof(ChromiumWebBrowser_IsBrowserInitializedChanged) + " : " + new
            {
                chromiumWebBrowser.IsBrowserInitialized
            });

            Invoke(new Action(() =>
            {
                toolStripButton_devTools.Enabled = chromiumWebBrowser.IsBrowserInitialized;

            }));

            foreach (ICrawlerPlugin plugin in CrawlerPlugins.Values)
            {
                try
                {
                    plugin.ChromiumWebBrowser_IsBrowserInitializedChanged(sender, e);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Warn($"{plugin.Title}.{nameof(plugin.ChromiumWebBrowser_IsBrowserInitializedChanged)} : {ex}");
                }
            }
        }

        #endregion

        private void toolStripButton_devTools_Click(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                chromiumWebBrowser.ShowDevTools();
            }));
        }
    }
}