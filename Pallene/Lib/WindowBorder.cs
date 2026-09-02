using Avalonia.Media;
using System;
using System.Runtime.InteropServices;

namespace Pallene.Lib
{
    internal partial class WindowBorder
    {

        [LibraryImport("dwmapi.dll")]
        private static partial int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref uint attrValue, int attrSize);

        private const int ATTR = 34;

        public static bool SetBorderColor(IntPtr hwnd, Color color)
        {
            if (!IsWindows11OrGreater()) return false;
            uint convColor = (uint)((color.B << 16) | (color.G << 8) | color.R);
            return DwmSetWindowAttribute(hwnd, ATTR, ref convColor, sizeof(uint)) == 0;
        }

        public static bool SetBorderDefault(IntPtr hwnd)
        {
            if (!IsWindows11OrGreater()) return false;
            uint color = 0xFFFFFFFF;
            return DwmSetWindowAttribute(hwnd, ATTR, ref color, sizeof(uint)) == 0;
        }

        public static bool SetBorderNone(IntPtr hwnd)
        {
            if (!IsWindows11OrGreater()) return false;
            uint color = 0xFFFFFFFE;
            return DwmSetWindowAttribute(hwnd, ATTR, ref color, sizeof(uint)) == 0;
        }

        private static bool IsWindows11OrGreater()
        {
            return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= 22000;
        }
    }
}
