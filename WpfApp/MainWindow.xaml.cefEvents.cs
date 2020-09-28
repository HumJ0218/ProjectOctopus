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
        private void cwb_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                Task.Run(() =>
                {
                    BrowserLoadPage($"http://localhost:{httpPort}");

                    var kahc = new HttpClient();
                    while (true)
                    {
                        try
                        {
                            kahc.GetAsync($"http://localhost:{httpPort}/KeepAlive").Wait(1000);
                        }
                        catch (TaskCanceledException ex)
                        {
                            break;
                        }
                        catch (Exception ex)
                        {
                            ;
                        }

                        Thread.Sleep(1000);
                    }
                });

                Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            Dispatcher.Invoke(delegate ()
                            {
                                Title = cwb.Title ?? "";

                                if (!url.IsVisible)
                                {
                                    url.Text = cwb.GetBrowser()?.MainFrame?.Url ?? "";
                                }
                            });
                        }
                        catch (TaskCanceledException ex)
                        {
                            break;
                        }
                        catch (Exception ex)
                        {
                            ;
                        }

                        Thread.Sleep(100);
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

        private void cwb_VirtualKeyboardRequested(object sender, VirtualKeyboardRequestedEventArgs e)
        {

        }

        private void cwb_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}