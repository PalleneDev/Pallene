using System.Runtime.InteropServices;

namespace Pallene.Sdk.Core.Windows
{
    internal class ScsiPassThroughGeneric
    {
        private const byte SPT_SENSE_INFO_LENGTH = 32;

        [StructLayout(LayoutKind.Sequential)]
        public struct ScsiPassThrough
        {
            public readonly ushort Length = (ushort)Marshal.SizeOf<ScsiPassThrough>();
            public readonly byte ScsiStatus = 0; // Returned by command
            public readonly byte PathId = 0; // Assigned by driver
            public readonly byte TargetId = 0; // Assigned by driver
            public readonly byte LogicalUnitNumber = 0; // Assigned by driver
            public required byte CDBLength;
            public readonly byte SenseInfoLength = SPT_SENSE_INFO_LENGTH;
            public required byte DataIn;
            public required uint DataTransferLength;
            public required uint TimeOutValue;
            public readonly nint DataBufferOffset = Marshal.OffsetOf<SPTWithBuffers>("ucDataBuf");
            public required uint SenseInfoOffset = (uint)Marshal.OffsetOf<SPTWithBuffers>("ucSenseBuf");
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public required byte[] CommandDescriptorBlock;

            public ScsiPassThrough() { }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SPTWithBuffers
        {
            public required ScsiPassThrough SPT;
            public readonly uint Filler = 0;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = SPT_SENSE_INFO_LENGTH)]
            public readonly byte[] ucSenseBuf = new byte[SPT_SENSE_INFO_LENGTH]; // Returned by command
            public required byte[] ucDataBuf;

            public SPTWithBuffers() { }
        }
    }
}