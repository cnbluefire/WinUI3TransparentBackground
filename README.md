# Winui3 透明背景

WinUI3的窗口背景有两层，一层是Win32窗口的背景，一层是DesktopWindowXamlSource中Visual的背景。

去除Win32窗口背景的方式有很多，这里使用了Hook CreateWindowExW，添加WS_EX_NOREDIRECTIONBITMAP的方式以保留系统标题栏和边框样式，其他方案此处不再赘述，如果需要请查阅其他资料。

去除Visual背景的方式，是通过ICompositionSupportsSystemBackdrop接口设置一个透明画刷，此时框架会自动移除黑底。

![](Images/Preview.png)