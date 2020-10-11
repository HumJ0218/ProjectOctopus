using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace CrawlerPlugin.JsRunner
{
    public class JsRunner : ICrawlerPlugin
    {
        private dynamic ChromiumWebBrowser;
        private readonly Dictionary<Regex, string> Scripts;
        private Action<string, string> ExecuteJavaScriptAsync;
        private readonly HttpClient HttpClient;
        public string TempData { get; set; } = "";

        public JsRunner()
        {
            Directory.CreateDirectory("./JsRunner");

            File.WriteAllText("./JsRunner/sample.js", "console.log('JsRunner')");
            if (!File.Exists("./JsRunner/scripts.txt"))
            {
                File.WriteAllText("./JsRunner/scripts.txt", ".*\tsample.js");
            }

            Scripts = File.ReadAllLines("./JsRunner/scripts.txt")
                .Select(m => m.Split('\t').Select(n => n.Trim()).ToArray())
                .Where(m => m.Length == 2)
                .ToDictionary(m => new Regex(m[0]), m => m[1]);

            ConsoleLogger.Success($"{nameof(JsRunner)} {Scripts.Count} 条规则已读取");
            foreach (KeyValuePair<Regex, string> i in Scripts)
            {
                ConsoleLogger.Success($"{nameof(JsRunner)} {i.Key}\t{i.Value}");
            }

            HttpClient = new HttpClient();
            ConsoleLogger.Success($"{nameof(JsRunner)} HTTP 客户端初始化完毕");
        }

        public Guid ID { get; } = Guid.Parse("38ED6E6D-9800-4435-9AA5-7B1F4627BE7E");

        public string JsObjectName { get; } = nameof(JsRunner);

        public string Title { get; } = nameof(JsRunner);

        public string Version { get; } = "0.0.0.0";

        public string Description { get; } = nameof(JsRunner);

        public string Author { get; } = "HumJ";

        public Uri Link { get; } = new Uri("https://humj.ink");

        public CookieManager Cookie { get; } = new CookieManager();

        public void Initialize(object chromiumWebBrowser)
        {
            ConsoleLogger.Debug($"{nameof(JsRunner)} {nameof(Initialize)} {chromiumWebBrowser}");

            ChromiumWebBrowser = chromiumWebBrowser;
        }

        public void LogCookie(string host, string cookie)
        {
            ConsoleLogger.Debug($"{nameof(JsRunner)} {nameof(LogCookie)} {host} {cookie}");

            Cookie[host] = cookie;
        }

        public string RestoreCookie(string host)
        {
            ConsoleLogger.Debug($"{nameof(JsRunner)} {nameof(RestoreCookie)} {host}");

            return Cookie[host];
        }

        public string DownloadString(string requestUri, string referer)
        {
            ConsoleLogger.Info($"{nameof(JsRunner)} {nameof(DownloadString)} {requestUri} {referer}");

            if (!string.IsNullOrEmpty(referer))
            {
                HttpClient.DefaultRequestHeaders.Referrer = new Uri(referer);
            }
            string s = HttpClient.GetStringAsync(requestUri).Result;

            ConsoleLogger.Success($"{nameof(JsRunner)} {nameof(DownloadString)} 拉取字符串成功，共 {s.Length} 字符");
            return s;
        }

        public long DownloadFile(string requestUri, string referer, string fileName)
        {
            ConsoleLogger.Info($"{nameof(JsRunner)} {nameof(DownloadFile)} {requestUri} {referer} {fileName}");

            FileInfo fi = new FileInfo("./JsRunner/download/" + fileName + ".partial");
            FileInfo destFile = new FileInfo(string.Join(".partial", fi.FullName.Replace(".partial", "\a").Split('\a').Reverse().Skip(1).Reverse()));
            destFile.Directory.Create();

            if (destFile.Exists)
            {
                ConsoleLogger.WarnBrighter($"{nameof(JsRunner)} {nameof(DownloadFile)} 本地文件已存在，跳过下载");
                return -1;
            }
            else
            {
                if (!string.IsNullOrEmpty(referer))
                {
                    HttpClient.DefaultRequestHeaders.Referrer = new Uri(referer);
                }
                Stream hs = HttpClient.GetStreamAsync(requestUri).Result;

                FileStream fs = fi.OpenWrite();

                int currentByte;
                while ((currentByte = hs.ReadByte()) != -1)
                {
                    fs.WriteByte((byte)currentByte);
                }

                long length = fs.Length;

                fs.Dispose();
                hs.Dispose();

                fi.MoveTo(destFile.FullName);

                ConsoleLogger.Success($"{nameof(JsRunner)} {nameof(DownloadString)} 下载文件成功，已保存至 {destFile}，共 {length} 字节");
                return length;
            }
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
            foreach (KeyValuePair<Regex, string> script in scripts.ToArray())
            {
                string path = script.Value;
                string text = File.ReadAllText("./JsRunner/" + path);
                ExecuteJavaScriptAsync(text, path);
                ConsoleLogger.DebugBrighter($"{nameof(JsRunner)} 执行脚本 {script.Key}");
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

    public class CookieManager : IDictionary<string, string>
    {
        private readonly DirectoryInfo rootPath;
        private readonly Dictionary<string, string> text;
        private readonly Dictionary<string, bool> dirty;

        public CookieManager()
        {
            rootPath = Directory.CreateDirectory("./JsRunner/cookie");

            Dictionary<string, (string, bool)> cookie = rootPath.GetFiles().ToDictionary(fi => fi.Name, fi => (File.ReadAllText(fi.FullName), false));
            text = cookie.ToDictionary(kv => kv.Key, kv => kv.Value.Item1);
            dirty = cookie.ToDictionary(kv => kv.Key, kv => kv.Value.Item2);
        }

        private void SaveAllChanges()
        {
            Directory.CreateDirectory("./JsRunner/cookie");

            foreach (KeyValuePair<string, bool> kv in dirty.Where(kv => kv.Value).ToArray())
            {
                File.WriteAllText(rootPath.FullName + "/" + kv.Key, text[kv.Key]);
                dirty[kv.Key] = false;
            }

            IEnumerable<string> filesToDelete = rootPath.GetFiles().Select(m => m.Name).Except(text.Keys);
            foreach (string k in filesToDelete.ToArray())
            {
                File.Delete("./JsRunner/cookie" + "/" + k);
            }
        }

        public string this[string key]
        {
            get
            {
                if (!text.ContainsKey(key))
                {
                    text[key] = "";
                }

                return text[key];
            }
            set
            {
                if (text.ContainsKey(key))
                {
                    text[key] = value;
                    dirty[key] = true;
                }
                else
                {
                    text.Add(key, value);
                    dirty.Add(key, true);
                }

                SaveAllChanges();
            }
        }

        public ICollection<string> Keys => ((IDictionary<string, string>)text).Keys;

        public ICollection<string> Values => ((IDictionary<string, string>)text).Values;

        public int Count => ((ICollection<KeyValuePair<string, string>>)text).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<string, string>>)text).IsReadOnly;

        public void Add(string key, string value)
        {
            ((IDictionary<string, string>)text).Add(key, value);
            ((IDictionary<string, bool>)dirty).Add(key, true);
            SaveAllChanges();
        }

        public void Add(KeyValuePair<string, string> item)
        {
            ((ICollection<KeyValuePair<string, string>>)text).Add(item);
            ((ICollection<KeyValuePair<string, bool>>)dirty).Add(new KeyValuePair<string, bool>(item.Key, true));
            SaveAllChanges();
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<string, string>>)text).Clear();
            ((ICollection<KeyValuePair<string, bool>>)dirty).Clear();
            SaveAllChanges();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return ((ICollection<KeyValuePair<string, string>>)text).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, string>)text).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, string>>)text).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, string>>)text).GetEnumerator();
        }

        public bool Remove(string key)
        {
            return ((IDictionary<string, string>)text).Remove(key) && ((IDictionary<string, bool>)dirty).Remove(key);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return ((IDictionary<string, string>)text).Remove(item) && ((IDictionary<string, bool>)dirty).Remove(item.Key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return ((IDictionary<string, string>)text).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)text).GetEnumerator();
        }
    }
}