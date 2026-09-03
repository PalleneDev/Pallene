using System.Runtime.InteropServices;

namespace Pallene.SDK.MMC
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct UInt24
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        private byte[] bytes = new byte[3];

        public UInt24() { }

        public UInt24(uint input) { bytes = BitConverter.GetBytes(input).Skip(1).ToArray(); }

        public static implicit operator byte[](UInt24 input) => input.bytes;

        public static explicit operator UInt24(uint input) => new UInt24(input);

        public static explicit operator uint(UInt24 input) {
            byte[] padded = new byte[4];
            input.bytes.CopyTo(padded, 1);
            return BitConverter.ToUInt32(padded);
        }
    }

}
