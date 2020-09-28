using CefSharp;
using CefSharp.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                var fi = new FileInfo("./WebApplication.dll");
                var di = new DirectoryInfo("./");

                var si = new ProcessStartInfo()
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

                var p = new Process
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

            var cefSettings = new CefSettings();
            try
            {
                cefSettings = JsonConvert.DeserializeObject<CefSettings>(File.ReadAllText("./cefSettings.json"));
                cefSettings.BrowserSubprocessPath = System.IO.Path.GetFullPath(cefSettings.BrowserSubprocessPath);
            }
            catch (Exception ex)
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
                var bs = JToken.Parse(File.ReadAllText("./browserSettings.json"));
                var ibs = typeof(IBrowserSettings);

                foreach (var p in ibs.GetProperties().Where(p=>p.CanWrite)) {
                    try
                    {
                        var v = bs[p.Name]?.ToObject(p.PropertyType);
                        if (v == null)
                        {
                            ;
                        }
                        else {
                            p.SetValue(cwb.BrowserSettings, v);
                        }
                    }
                    catch (Exception ex)
                    {
                        ;
                    }
                }
            }
            catch (Exception ex)
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
            catch (Exception ex)
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