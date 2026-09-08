using System.Runtime.InteropServices;

namespace Pallene.Sdk.Core.Windows
{

    [StructLayout(LayoutKind.Sequential)]
    internal struct ScsiPassThrough
    {
        public readonly ushort Length = (ushort)Marshal.SizeOf<ScsiPassThrough>();
        public readonly byte ScsiStatus = 0; // Returned by command
        public readonly byte PathId = 0; // Assigned by driver
        public readonly byte TargetId = 0; // Assigned by driver
        public readonly byte LogicalUnitNumber = 0; // Assigned by driver
        public required byte CDBLength;
        public readonly byte SenseInfoLength = Constants.SPT_SENSE_INFO_LENGTH;
        public required byte DataIn;
        public required uint DataTransferLength;
        public required uint TimeOutValue;
        public readonly nint DataBufferOffset = Marshal.OffsetOf<SPTWithBuffers>("ucDataBuf");
        public readonly uint SenseInfoOffset = (uint)Marshal.OffsetOf<SPTWithBuffers>("ucSenseBuf");
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public required byte[] CommandDescriptorBlock;

        public ScsiPassThrough() { }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SPTWithBuffers
    {
        public required ScsiPassThrough SPT;
        public readonly uint Filler = 0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.SPT_SENSE_INFO_LENGTH)]
        public readonly byte[] ucSenseBuf = new byte[Constants.SPT_SENSE_INFO_LENGTH]; // Returned by command
        public required byte[] ucDataBuf;

        public SPTWithBuffers() { }
    }
}