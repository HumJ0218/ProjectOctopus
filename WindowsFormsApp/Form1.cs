using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        ChromiumWebBrowser chromiumWebBrowser;
        Stack<string> BackUrls;
        Stack<string> FowardUrls;

        public Form1()
        {
            InitializeComponent();

            CreateChromiumWebBrowser();
        }

        private void CreateChromiumWebBrowser()
        {
            BackUrls = new Stack<string>();
            FowardUrls = new Stack<string>();

            chromiumWebBrowser = new ChromiumWebBrowser("https://www.baidu.com/")
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, toolStrip.Height, 0, 0)
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
        }

        private void ChromiumWebBrowser_LoadError(object sender, LoadErrorEventArgs e)
        {
            ConsoleLogger.Debug(nameof(ChromiumWebBrowser_LoadError) + " : " + new
            {
                e.ErrorCode,
                e.ErrorText,
                e.FailedUrl,
            });
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
        }

        private void ChromiumWebBrowser_JavascriptMessageReceived(object sender, JavascriptMessageReceivedEventArgs e)
        {
            ConsoleLogger.Debug(nameof(ChromiumWebBrowser_JavascriptMessageReceived) + " : " + new
            {
                e.Message,
            });
        }

        private void ChromiumWebBrowser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            ConsoleLogger.Debug(nameof(ChromiumWebBrowser_IsBrowserInitializedChanged) + " : " + new
            {
            });
        }

        #endregion

    }
}
