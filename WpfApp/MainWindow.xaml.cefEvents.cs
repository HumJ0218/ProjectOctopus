using CefSharp;
using CefSharp.Wpf;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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

                    HttpClient kahc = new HttpClient();
                    while (true)
                    {
                        try
                        {
                            kahc.GetAsync($"http://localhost:{httpPort}/KeepAlive").Wait(1000);
                        }
                        catch (TaskCanceledException)
                        {
                            break;
                        }
                        catch (Exception)
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
                        catch (TaskCanceledException)
                        {
                            break;
                        }
                        catch (Exception)
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