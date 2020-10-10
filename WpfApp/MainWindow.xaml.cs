using CefSharp;
using CefSharp.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace WpfApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private static int httpPort = 0;
        private ScriptRunner sr = null;

        public MainWindow()
        {
            httpPort = new Random().Next(5000) + 60000;

            Task.Run(delegate ()
            {
                FileInfo fi = new FileInfo("./WebApplication.dll");
                DirectoryInfo di = new DirectoryInfo("./");

                ProcessStartInfo si = new ProcessStartInfo()
                {
                    FileName = "dotnet.exe",
                    Arguments = fi.FullName,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    WorkingDirectory = di.FullName,
                };
                si.Environment.Add("ASPNETCORE_URLS", $"http://localhost:{httpPort}");

#if DEBUG
                fi = new FileInfo("../../../../WebApplication/bin/x64/Debug/netcoreapp3.1/WebApplication.dll");
                di = new DirectoryInfo("../../../../WebApplication");

                si.Arguments = fi.FullName;
                si.WorkingDirectory = di.FullName;
                si.Environment.Add("ASPNETCORE_ENVIRONMENT", "Development");
#endif

                Process p = new Process
                {
                    StartInfo = si,
                };
                p.OutputDataReceived += P_OutputDataReceived;
                p.ErrorDataReceived += P_ErrorDataReceived;

                Debug.WriteLine($"{nameof(ProcessStartInfo)} = {JsonConvert.SerializeObject(si)}");
                Debug.WriteLine($"p.Start()");
                p.Start();

                Debug.WriteLine($"p.WaitForExit()");
                p.WaitForExit();

                Debug.WriteLine($"Environment.Exit(-1)");
                Environment.Exit(-1);
            });

            CefSettings cefSettings = new CefSettings();
            try
            {
                cefSettings = JsonConvert.DeserializeObject<CefSettings>(File.ReadAllText("./cefSettings.json"));
                cefSettings.BrowserSubprocessPath = System.IO.Path.GetFullPath(cefSettings.BrowserSubprocessPath);
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

            InitializeComponent();
            cwb.Language = XmlLanguage.GetLanguage("zh-cn");

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
                            p.SetValue(cwb.BrowserSettings, v);
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
        }

        private void P_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine(e.Data);
        }

        private void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine(e.Data);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    {
                        ToggleAddressBarVisible();
                        break;
                    }
                case Key.F5:
                    {
                        BrowserReload(true);
                        break;
                    }
                case Key.F12:
                    {
                        BrowserShowDevTools();
                        ScriptRunnerShowWindow();
                        break;
                    }
            }
        }

        private void ScriptRunnerShowWindow()
        {
            if (sr is null)
            {
                sr = ScriptRunner.Run((s, ea) =>
                {
                    sr = null;
                });
            }
        }

        private void url_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    {
                        BrowserLoadPage((sender as TextBox).Text);
                        break;
                    }
            }
        }

        private void BrowserLoadPage(string address)
        {
            try
            {
                cwb.Load(address);
            }
            catch (Exception)
            {
                ;
            }
        }

        private void ToggleAddressBarVisible()
        {
            url.Visibility = url.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BrowserReload(bool ignoreCache)
        {
            cwb.Reload(ignoreCache);
        }

        private void BrowserShowDevTools()
        {
            cwb.ShowDevTools();
        }
    }
}