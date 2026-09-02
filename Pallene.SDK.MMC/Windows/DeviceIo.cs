using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace Pallene.SDK.MMC.Windows
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
