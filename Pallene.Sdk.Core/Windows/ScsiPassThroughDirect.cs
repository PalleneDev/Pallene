using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Pallene.Sdk.Core.Windows
{


    [StructLayout(LayoutKind.Sequential)]
    internal struct ScsiPassThroughDirect
    {
        public readonly ushort Length = (ushort)Marshal.SizeOf<ScsiPassThroughDirect>();
        public readonly byte ScsiStatus = 0; // Returned by command
        public readonly byte PathId = 0; // Assigned by driver
        public readonly byte TargetId = 0; // Assigned by driver
        public readonly byte LogicalUnitNumber = 0; // Assigned by driver
        public required byte CDBLength;
        public readonly byte SenseInfoLength = Constants.SPT_SENSE_INFO_LENGTH;
        public required byte DataIn;
        public required uint DataTransferLength;
        public required uint TimeOutValue;
        [MarshalAs(UnmanagedType.LPArray)]
        public required nint DataBuffer;
        public readonly uint SenseInfoOffset = (uint)Marshal.OffsetOf<SPTDWithBuffers>("ucSenseBuf");
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public required byte[] CommandDescriptorBlock;

        public ScsiPassThroughDirect() { }
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct SPTDWithBuffers
    {
        public required ScsiPassThroughDirect SPTD;
        public readonly uint Filler = 0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.SPT_SENSE_INFO_LENGTH)]
        public readonly byte[] ucSenseBuf = new byte[Constants.SPT_SENSE_INFO_LENGTH]; // Returned by command

        public SPTDWithBuffers() { }
    }

}
