using Reloaded.Hooks.Definitions;
using Reloaded.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinUI3TransparentBackground
{
    public static class HookHelper
    {
        private const uint WS_EX_NOREDIRECTIONBITMAP = 0x00200000;
        private static IHook<CreateWindowExFunc> createWindowEx;

        public unsafe static void AddHook()
        {
            IntPtr dll = Reloaded.Assembler.Kernel32.Kernel32.LoadLibraryW("user32.dll");
            IntPtr proc = Reloaded.Assembler.Kernel32.Kernel32.GetProcAddress(dll, "CreateWindowExW");
            createWindowEx = ReloadedHooks.Instance.CreateHook<CreateWindowExFunc>(CreateWindowExImpl, proc.ToInt64()).Activate();
        }


        private static unsafe IntPtr CreateWindowExImpl(
            uint dwExStyle,
            IntPtr lpClassName,
            IntPtr lpWindowName,
            uint dwStyle,
            int X,
            int Y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam)
        {

            try
            {
                var str = Marshal.PtrToStringUni(lpClassName);
                if (str == "WinUIDesktopWin32WindowClass")
                {
                    dwExStyle |= WS_EX_NOREDIRECTIONBITMAP;
                }
            }
            catch { }


            return createWindowEx.OriginalFunction.Invoke(dwExStyle, lpClassName, lpWindowName, dwStyle, X, Y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);
        }


        [Reloaded.Hooks.Definitions.X64.Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
        [Reloaded.Hooks.Definitions.X86.Function(Reloaded.Hooks.Definitions.X86.CallingConventions.Stdcall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public delegate IntPtr CreateWindowExFunc(
            uint dwExStyle,
            IntPtr lpClassName,
            IntPtr lpWindowName,
            uint dwStyle,
            int X,
            int Y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam);
    }
}
