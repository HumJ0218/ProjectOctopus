# HumJ.ProjectOctopus
基于 CefSharp 的 Web 爬虫框架

--------

大致的实现思路是，启动一个 CefSharp 浏览器对象，载入指定页面。

将 CefSharp 的各种事件暴露给动态添加的插件

主要插件为 JsRunner，功能是当页面载入成功后根据当前页面地址自动执行指定的 JavaScript 脚本，以期实现数据提取、模拟点击、文件下载等功能

--------

预计主要的应用范围为，网络资源的抓取（比如 Pixiv 日榜等），Web 应用程序自动化测试
