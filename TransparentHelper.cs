using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI3TransparentBackground
{
    public static class TransparentHelper
    {
        public static void SetTransparent(Window window, bool isTransparent)
        {
            var visual = GetBackgroundVisual(window);

            if (visual != null)
            {
                visual.IsVisible = !isTransparent;
            }
        }

        private static Microsoft.UI.Composition.SpriteVisual GetBackgroundVisual(Window window)
        {
            var islands = Microsoft.UI.Content.ContentIsland.FindAllForCurrentThread();

            var island = islands.FirstOrDefault(c => c.Window.WindowId == window.AppWindow.Id);

            var visual = island?.Root as Microsoft.UI.Composition.ContainerVisual;

            while (visual != null)
            {
                if (visual.Children.Count == 1)
                {
                    visual = visual.Children.First() as Microsoft.UI.Composition.ContainerVisual;
                }
                else
                {
                    foreach (var item in visual.Children)
                    {
                        if (item is Microsoft.UI.Composition.SpriteVisual backgroundVisual)
                        {
                            return backgroundVisual;
                        }
                    }
                    visual = null;
                }
            }

            return null;
        }
    }
}
