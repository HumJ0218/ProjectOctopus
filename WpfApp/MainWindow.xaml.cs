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
        private readonly static string[][] serviceProgram = new[] {
            new[]{  "./WebApplication.exe","./"},
            new[]{ "../../../../WebApplication/bin/x64/Debug/netcoreapp3.1/WebApplication.exe","../../../../WebApplication"},
        };

        private ScriptRunner sr = null;

        public MainWindow()
        {
            Task.Run(delegate ()
            {
                var pf = serviceProgram.Select(m => new
                {
                    fi = new FileInfo(m[0]),
                    di = new DirectoryInfo(m[1]),
                }).First(m => m.fi.Exists && m.di.Exists);
                var p = new Process
                {
                    StartInfo = new ProcessStartInfo(pf.fi.FullName, "httpPort=65000")
                    {
                        FileName = pf.fi.FullName,
                        Arguments = "httpPort=65000",
                        CreateNoWindow = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        WorkingDirectory = pf.di.FullName,
                    },
                };
                p.OutputDataReceived += P_OutputDataReceived;
                p.ErrorDataReceived += P_ErrorDataReceived;
                p.Start();

                p.WaitForExit();
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
            Console.Error.WriteLine(e.Data);
        }

        private void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
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

        private void cwb_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                Task.Run(delegate ()
                {
                    BrowserLoadPage("http://localhost:65000");

                    var kahc = new HttpClient();
                    while (true)
                    {
                        try
                        {
                            kahc.GetAsync("http://localhost:65000/KeepAlive").Wait(1000);
                        }
                        catch { }
                        Thread.Sleep(1000);
                    }
                });
            }
        }

        private void cwb_AddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(url is null))
            {
                url.Text = e.NewValue.ToString();
            }
        }

        private void cwb_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {

        }

        private void cwb_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            Dispatcher.Invoke(delegate ()
            {
                url.Text = e.Url;
                url.Visibility = Visibility.Collapsed;
            });
        }

        private void cwb_LoadError(object sender, LoadErrorEventArgs e)
        {

        }

        private void cwb_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {

        }

        private void cwb_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {

        }

        private void cwb_StatusMessage(object sender, StatusMessageEventArgs e)
        {

        }

        private void cwb_JavascriptMessageReceived(object sender, JavascriptMessageReceivedEventArgs e)
        {

        }

        private void cwb_TitleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Title = e.NewValue.ToString();
        }

        private void cwb_VirtualKeyboardRequested(object sender, CefSharp.Wpf.VirtualKeyboardRequestedEventArgs e)
        {

        }

        private void cwb_Paint(object sender, CefSharp.Wpf.PaintEventArgs e)
        {

        }
    }
}