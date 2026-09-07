using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Pallene.Sdk.Core.Windows
{
    internal static class DeviceIo
    {
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            uint ioControlCode,
            [In] ref ScsiPassThroughGeneric.SPTWithBuffers lpInBuffer,
            uint nInBufferSize,
            [In, Out] ref ScsiPassThroughGeneric.SPTWithBuffers lpOutBuffer,
            uint nOutBufferSize,
            ref uint lpBytesReturned,
            nint Overlapped
        );

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            uint ioControlCode,
            [In] ref ScsiPassThroughDirectGeneric.SPTDWithBuffers lpInBuffer,
            uint nInBufferSize,
            [In, Out] ref ScsiPassThroughDirectGeneric.SPTDWithBuffers lpOutBuffer,
            uint nOutBufferSize,
            ref uint lpBytesReturned,
            nint Overlapped
        );

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
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
