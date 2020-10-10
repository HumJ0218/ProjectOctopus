using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CrawlerPlugin.JsRunner
{
    public class JsRunner : ICrawlerPlugin
    {
        private dynamic ChromiumWebBrowser;
        private readonly Dictionary<Regex, string> Scripts;
        private Action<string, string> ExecuteJavaScriptAsync;
        private string Cookie;

        public JsRunner()
        {
            Directory.CreateDirectory("./JsRunner");

            File.WriteAllText("./JsRunner/sample.js", "JsRunner.logCookie(document.cookie)");
            if (!File.Exists("./JsRunner/scripts.txt"))
            {
                File.WriteAllText("./JsRunner/scripts.txt", ".*\tsample.js");
            }

            Scripts = File.ReadAllLines("./JsRunner/scripts.txt")
                .Select(m => m.Split('\t').Select(n => n.Trim()).ToArray())
                .Where(m => m.Length == 2)
                .ToDictionary(m => new Regex(m[0]), m => m[1]);

            Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} {nameof(JsRunner)}] {Scripts.Count} 条规则已读取");
        }

        public Guid ID { get; } = Guid.Parse("38ED6E6D-9800-4435-9AA5-7B1F4627BE7E");

        public string JsObjectName { get; } = nameof(JsRunner);

        public string Title { get; } = nameof(JsRunner);

        public string Version { get; } = "0.0.0.0";

        public string Description { get; } = nameof(JsRunner);

        public string Author { get; } = "HumJ";

        public Uri Link { get; } = new Uri("https://humj.ink");

        public void Initialize(object chromiumWebBrowser)
        {
            ChromiumWebBrowser = chromiumWebBrowser;
        }

        public void LogCookie(string cookie)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} {nameof(JsRunner)}.{nameof(LogCookie)}] {cookie}");

            Cookie = cookie;
        }

        public void ChromiumWebBrowser_AddressChanged(dynamic sender, dynamic e)
        {
        }

        public void ChromiumWebBrowser_ConsoleMessage(dynamic sender, dynamic e)
        {
        }

        public void ChromiumWebBrowser_FrameLoadEnd(dynamic sender, dynamic e)
        {
            if (ExecuteJavaScriptAsync == null)
            {
                dynamic cwb = ChromiumWebBrowser.GetBrowser();
                dynamic mf = (cwb.GetType() as Type).GetProperty("MainFrame").GetValue(cwb);
                System.Reflection.MethodInfo ejsa = (mf.GetType() as Type).GetMethod("ExecuteJavaScriptAsync");
                ExecuteJavaScriptAsync = (string script, string source) =>
                {
                    ejsa.Invoke(mf, new object[] { script, "hjpoc://" + source, 1 });
                };
            }

            if (e.HttpStatusCode != 200)
            {
                return;
            }

            string url = e.Url as string;
            IEnumerable<KeyValuePair<Regex, string>> scripts = Scripts.Where(m => m.Key.IsMatch(url));
            foreach (KeyValuePair<Regex, string> script in scripts)
            {
                string path = script.Value;
                string text = File.ReadAllText("./JsRunner/" + path);
                ExecuteJavaScriptAsync(text, path);
            }
        }

        public void ChromiumWebBrowser_FrameLoadStart(dynamic sender, dynamic e)
        {
        }

        public void ChromiumWebBrowser_IsBrowserInitializedChanged(dynamic sender, dynamic e)
        {
        }

        public void ChromiumWebBrowser_JavascriptMessageReceived(dynamic sender, dynamic e)
        {
        }

        public void ChromiumWebBrowser_LoadError(dynamic sender, dynamic e)
        {
        }

        public void ChromiumWebBrowser_LoadingStateChanged(dynamic sender, dynamic e)
        {
        }

        public void ChromiumWebBrowser_StatusMessage(dynamic sender, dynamic e)
        {
        }

        public void ChromiumWebBrowser_TitleChanged(dynamic sender, dynamic e)
        {
        }
    }
}