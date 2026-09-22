using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Pallene.Sdk.Core.Windows
{
    [SupportedOSPlatform("windows")]
    internal sealed class ScsiWindowsAdapter : IScsiSystemAdapter
    {
        private SafeFileHandle FileHandle;

        public DiscDrive Recorder { get; }

        public ScsiWindowsAdapter(DiscDrive recorder)
        {
            Recorder = recorder;
            FileHandle = OpenFileHandle(recorder);
        }

        private static SafeFileHandle OpenFileHandle(DiscDrive recorder)
        {
            SafeFileHandle handle = DeviceIo.CreateFileA(
                @$"\\.\{recorder.AccessPath}",
                FileAccess.ReadWrite,
                FileShare.None,
                nint.Zero,
                FileMode.Open,
                0,
                nint.Zero
                );
            if (handle.IsInvalid)
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            return handle;
        }

        private static byte[] ToResizedArray(byte[] input, int length)
        {
            byte[] resized = new byte[length];
            input.CopyTo(resized);
            return resized;
        }


        private static SPTWithBuffers GenerateSptwb(CommandDescriptorBlock cdb, DataDirection direction, uint timeout, byte[] dataBuffer)
        {
            return new()
            {
                SPT = new()
                {
                    CDBLength = (byte)cdb.Length,
                    DataIn = (byte)direction,
                    DataTransferLength = (byte)dataBuffer.Length,
                    TimeOutValue = timeout,
                    CommandDescriptorBlock = ToResizedArray(cdb, 16)
                },
                ucDataBuf = dataBuffer
            };
        }

        private static SPTDWithBuffers GenerateSptdwb(CommandDescriptorBlock cdb, DataDirection direction, uint timeout, nint bufferPtr, byte bufferLength)
        {
            return new()
            {
                SPTD = new()
                {
                    CDBLength = (byte)cdb.Length,
                    DataIn = (byte)direction,
                    DataTransferLength = bufferLength,
                    DataBuffer = bufferPtr,
                    TimeOutValue = timeout,
                    CommandDescriptorBlock = ToResizedArray(cdb, 16)
                }
            };
        }

        private static uint SendIoControl(SafeFileHandle fileHandle, ref SPTDWithBuffers sptdwb)
        {
            uint bytesReturned = 0;
            bool success = DeviceIo.DeviceIoControl(
                fileHandle,
                Constants.IOCTL_SCSI_PASS_THROUGH_DIRECT,
                ref sptdwb,
                (uint)Marshal.SizeOf(sptdwb),
                ref sptdwb,
                (uint)Marshal.SizeOf(sptdwb),
                ref bytesReturned,
                nint.Zero
            );
            if (!success)
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            return bytesReturned;
        }

        private static uint SendIoControl(SafeFileHandle fileHandle, ref SPTWithBuffers sptwb)
        {
            uint bytesReturned = 0;
            bool success = DeviceIo.DeviceIoControl(
                fileHandle,
                Constants.IOCTL_SCSI_PASS_THROUGH,
                ref sptwb,
                (uint)Marshal.SizeOf(sptwb),
                ref sptwb,
                (uint)Marshal.SizeOf(sptwb),
                ref bytesReturned,
                nint.Zero
            );
            if (!success)
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            return bytesReturned;
        }

        public void SendCommand(CommandDescriptorBlock cdb, uint timeout)
        {
            SPTWithBuffers sptwb = GenerateSptwb(
                cdb,
                DataDirection.DataUnspecified,
                timeout,
                Array.Empty<byte>()
                );
            SendIoControl(FileHandle, ref sptwb);
            ScsiException.ThrowIfStatusNotGood(
                (ScsiStatus)sptwb.SPT.ScsiStatus,
                new SenseData(sptwb.ucSenseBuf)
                );
        }

        public void SendCommand(CommandDescriptorBlock cdb, ref byte[] outputBuffer, uint timeout, bool? directMemAccess)
        {
            if (directMemAccess == null)
                directMemAccess = outputBuffer.Length >= Constants.PASS_THROUGH_DIRECT_MIN_THRESHOLD;
            if(directMemAccess.Value)
            {
                nint ptr = Marshal.AllocHGlobal(outputBuffer.Length);
                SPTDWithBuffers sptdwb = GenerateSptdwb(
                    cdb,
                    DataDirection.DataIn,
                    timeout,
                    ptr,
                    (byte)outputBuffer.Length
                    );
                SendIoControl(FileHandle, ref sptdwb);
                Marshal.Copy(ptr, outputBuffer, 0, outputBuffer.Length);
                Marshal.FreeHGlobal(ptr);
                ScsiException.ThrowIfStatusNotGood(
                    (ScsiStatus)sptdwb.SPTD.ScsiStatus,
                    new SenseData(sptdwb.ucSenseBuf)
                    );
            }
            else
            {
                SPTWithBuffers sptwb = GenerateSptwb(
                    cdb,
                    DataDirection.DataIn,
                    timeout,
                    outputBuffer
                    );
                SendIoControl(FileHandle, ref sptwb);
                ScsiException.ThrowIfStatusNotGood(
                    (ScsiStatus)sptwb.SPT.ScsiStatus,
                    new SenseData(sptwb.ucSenseBuf)
                    );
            }
        }

        public void SendCommand(CommandDescriptorBlock cdb, byte[] inputBuffer, uint timeout, bool? directMemAccess)
        {
            if (directMemAccess == null)
                directMemAccess = inputBuffer.Length >= Constants.PASS_THROUGH_DIRECT_MIN_THRESHOLD;
            if(directMemAccess.Value)
            {
                nint ptr = Marshal.AllocHGlobal(inputBuffer.Length);
                Marshal.Copy(inputBuffer, 0, ptr, inputBuffer.Length);
                SPTDWithBuffers sptdwb = GenerateSptdwb(
                    cdb,
                    DataDirection.DataOut,
                    timeout,
                    ptr,
                    (byte)inputBuffer.Length
                    );
                SendIoControl(FileHandle, ref sptdwb);
                Marshal.FreeHGlobal(ptr);
                ScsiException.ThrowIfStatusNotGood(
                    (ScsiStatus)sptdwb.SPTD.ScsiStatus,
                    new SenseData(sptdwb.ucSenseBuf)
                    );

            }
            else
            {
                SPTWithBuffers sptwb = GenerateSptwb(
                    cdb,
                    DataDirection.DataOut,
                    timeout,
                    inputBuffer
                    );
                SendIoControl(FileHandle, ref sptwb);
                ScsiException.ThrowIfStatusNotGood(
                    (ScsiStatus)sptwb.SPT.ScsiStatus,
                    new SenseData(sptwb.ucSenseBuf)
                    );
            }
        }
    }
}
