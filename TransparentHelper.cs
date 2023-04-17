using HotLyric.Win32.Utils;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinRT;

namespace WinUI3TransparentBackground
{
    public static class TransparentHelper
    {
        public static void SetTransparent(Window window, bool isTransparent)
        {
            var brushHolder = window.As<ICompositionSupportsSystemBackdrop>();

            if (isTransparent)
            {
                var colorBrush = WindowsCompositionHelper.Compositor.CreateColorBrush(Windows.UI.Color.FromArgb(0, 255, 255, 255));
                brushHolder.SystemBackdrop = colorBrush;
            }
            else
            {
                brushHolder.SystemBackdrop = null;
            }
        }
    }
}
