using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Pallene.SDK.MMC
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Int24
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        private byte[] bytes = new byte[3];

        public Int24() { }

        public Int24(int input) { 
            bytes = BitConverter.GetBytes(input).Skip(1).ToArray();
            bytes[0] = (byte)(
                (input > 0).ToByte() << 7 +
                (bytes[0] & 0b01111111)
                ); 
        }

        public static implicit operator byte[](Int24 input) => input.bytes;

        public static explicit operator Int24(int input) => new Int24(input);

        public static explicit operator int(Int24 input)
        {
            byte[] padded = new byte[4];
            input.bytes.CopyTo(padded, 1);
            return BitConverter.ToInt32(padded);
        }
    }

}
