using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Management;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using Microsoft.Win32.SafeHandles;
using System.Diagnostics.CodeAnalysis;

namespace Pallene.Sdk.Core.Windows
{
    internal class ScsiWindowsAdapter : IScsiSystemAdapter
    {
        private SafeFileHandle FileHandle;

        public DiscDrive Recorder { get; }

        public ScsiWindowsAdapter(DiscDrive recorder)
        {
            Recorder = recorder;
            FileHandle = OpenFileHandle(recorder);
        }

        private SafeFileHandle OpenFileHandle(DiscDrive recorder)
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


        private SPTWithBuffers GenerateSptwb(CommandDescriptorBlock cdb, DataDirection direction, uint timeout, byte[] dataBuffer)
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

        private SPTDWithBuffers GenerateSptdwb(CommandDescriptorBlock cdb, DataDirection direction, uint timeout, nint bufferPtr, byte bufferLength)
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

        private uint SendIoControl(SafeFileHandle fileHandle, ref SPTDWithBuffers sptdwb)
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

        private uint SendIoControl(SafeFileHandle fileHandle, ref SPTWithBuffers sptwb)
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
        }

        public byte[] ReceiveData(CommandDescriptorBlock cdb, int bufferSize, uint timeout, bool? directMemAccess)
        {
            if (directMemAccess == null)
                directMemAccess = bufferSize >= Constants.PASS_THROUGH_DIRECT_MIN_THRESHOLD;
            if(directMemAccess.Value)
            {
                nint ptr = Marshal.AllocHGlobal(bufferSize);
                SPTDWithBuffers sptdwb = GenerateSptdwb(
                    cdb,
                    DataDirection.DataIn,
                    timeout,
                    ptr,
                    (byte)bufferSize
                    );
                SendIoControl(FileHandle, ref sptdwb);
                byte[] output = new byte[bufferSize];
                Marshal.Copy(ptr, output, 0, bufferSize);
                Marshal.FreeHGlobal(ptr);
                return output;
            }
            else
            {
                SPTWithBuffers sptwb = GenerateSptwb(
                    cdb,
                    DataDirection.DataIn,
                    timeout,
                    new byte[bufferSize]
                    );
                SendIoControl(FileHandle, ref sptwb);
                return sptwb.ucDataBuf;
            }
        }

        public void SendData(CommandDescriptorBlock cdb, byte[] inputBuffer, uint timeout, bool? directMemAccess)
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
            }
        }
    }
}
