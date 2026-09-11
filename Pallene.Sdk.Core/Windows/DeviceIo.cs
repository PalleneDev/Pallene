using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Pallene.Sdk.Core.Windows
{
    [SupportedOSPlatform("windows")]
    internal static class DeviceIo
    {
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            uint ioControlCode,
            [In] ref SPTWithBuffers lpInBuffer,
            uint nInBufferSize,
            [In, Out] ref SPTWithBuffers lpOutBuffer,
            uint nOutBufferSize,
            ref uint lpBytesReturned,
            nint Overlapped
        );

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            uint ioControlCode,
            [In] ref SPTDWithBuffers lpInBuffer,
            uint nInBufferSize,
            [In, Out] ref SPTDWithBuffers lpOutBuffer,
            uint nOutBufferSize,
            ref uint lpBytesReturned,
            nint Overlapped
        );

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, BestFitMapping = false)]
        public static extern SafeFileHandle CreateFileA(
                string FileName,
                [MarshalAs(UnmanagedType.U4)] FileAccess DesiredAccess,
                [MarshalAs(UnmanagedType.U4)] FileShare ShareMode,
                nint SecurityAttributes,
                [MarshalAs(UnmanagedType.U4)] FileMode CreationDisposition,
                [MarshalAs(UnmanagedType.U4)] FileAttributes FlagsAndAttributes,
                nint TemplateFile
        );
    }
}
