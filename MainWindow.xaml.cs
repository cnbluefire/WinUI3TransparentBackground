using HotLyric.Win32.Utils;
using Microsoft.UI.Composition;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Vanara.PInvoke;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3TransparentBackground
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        bool transparent = true;

        ComCtl32.SUBCLASSPROC wndProcHandler;

        public MainWindow()
        {
            this.InitializeComponent();

            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(AppTitlebar);

            var windowHandle = new IntPtr((long)this.AppWindow.Id.Value);

            DwmApi.DwmExtendFrameIntoClientArea(windowHandle, new DwmApi.MARGINS(0));
            using var rgn = Gdi32.CreateRectRgn(-2, -2, -1, -1);
            DwmApi.DwmEnableBlurBehindWindow(windowHandle, new DwmApi.DWM_BLURBEHIND(true)
            {
                dwFlags = DwmApi.DWM_BLURBEHIND_Mask.DWM_BB_ENABLE | DwmApi.DWM_BLURBEHIND_Mask.DWM_BB_BLURREGION,
                hRgnBlur = rgn
            });
            TransparentHelper.SetTransparent(this, true);

            wndProcHandler = new ComCtl32.SUBCLASSPROC(WndProc);
            ComCtl32.SetWindowSubclass(windowHandle, wndProcHandler, 1, IntPtr.Zero);
        }

        private unsafe IntPtr WndProc(HWND hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, nuint uIdSubclass, IntPtr dwRefData)
        {
            if (uMsg == (uint)User32.WindowMessage.WM_ERASEBKGND)
            {
                if (User32.GetClientRect(hWnd, out var rect))
                {
                    using var brush = Gdi32.CreateSolidBrush(new COLORREF(0, 0, 0));
                    User32.FillRect(wParam, rect, brush);
                    return new IntPtr(1);
                }
            }
            else if (uMsg == (uint)User32.WindowMessage.WM_DWMCOMPOSITIONCHANGED)
            {
                DwmApi.DwmExtendFrameIntoClientArea(hWnd, new DwmApi.MARGINS(0));
                using var rgn = Gdi32.CreateRectRgn(-2, -2, -1, -1);
                DwmApi.DwmEnableBlurBehindWindow(hWnd, new DwmApi.DWM_BLURBEHIND(true)
                {
                    dwFlags = DwmApi.DWM_BLURBEHIND_Mask.DWM_BB_ENABLE | DwmApi.DWM_BLURBEHIND_Mask.DWM_BB_BLURREGION,
                    hRgnBlur = rgn
                });

                return IntPtr.Zero;
            }

            return ComCtl32.DefSubclassProc(hWnd, uMsg, wParam, lParam);
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            transparent = !transparent;
            TransparentHelper.SetTransparent(this, transparent);
            TitlebarBackground.Opacity = transparent ? 0 : 1;
        }
    }
}
