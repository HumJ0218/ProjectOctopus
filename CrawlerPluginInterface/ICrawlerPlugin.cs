using System;

/// <summary>
/// 爬虫插件接口<br/>
/// 将暴露类中小写字母开头的属性及方法给浏览器页面
/// </summary>
public interface ICrawlerPlugin
{
    /// <summary>
    /// 插件 ID
    /// </summary>
    Guid ID { get; }

    /// <summary>
    /// 暴露在页面中的 JS 对象名称
    /// </summary>
    string JsObjectName { get; }

    /// <summary>
    /// 初始化插件
    /// </summary>
    /// <param name="chromiumWebBrowser">爬虫浏览器控件</param>
    void Initialize(dynamic chromiumWebBrowser);

    /// <summary>
    /// 插件标题
    /// </summary>
    string Title { get; }

    /// <summary>
    /// 插件版本
    /// </summary>
    string Version { get; }

    /// <summary>
    /// 插件描述
    /// </summary>
    string Description { get; }

    /// <summary>
    /// 插件作者
    /// </summary>
    string Author { get; }

    /// <summary>
    /// 相关链接
    /// </summary>
    Uri Link { get; }

    #region CefSharp 事件

    void ChromiumWebBrowser_TitleChanged(dynamic sender, dynamic e);

    void ChromiumWebBrowser_LoadError(dynamic sender, dynamic e);

    void ChromiumWebBrowser_AddressChanged(dynamic sender, dynamic e);

    void ChromiumWebBrowser_StatusMessage(dynamic sender, dynamic e);

    void ChromiumWebBrowser_ConsoleMessage(dynamic sender, dynamic e);

    void ChromiumWebBrowser_LoadingStateChanged(dynamic sender, dynamic e);

    void ChromiumWebBrowser_FrameLoadEnd(dynamic sender, dynamic e);

    void ChromiumWebBrowser_FrameLoadStart(dynamic sender, dynamic e);

    void ChromiumWebBrowser_JavascriptMessageReceived(dynamic sender, dynamic e);

    void ChromiumWebBrowser_IsBrowserInitializedChanged(dynamic sender, dynamic e);

    #endregion

}