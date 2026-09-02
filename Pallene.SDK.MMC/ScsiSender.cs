using Pallene.SDK.MMC.Windows;
using System.Collections;

namespace Pallene.SDK.MMC
{
    public class ScsiSender
    {
        public IScsiSystemAdapter Adapter;

        private static byte[] GenericCDB(byte length, byte opCode)
        {
            if (length != 6 && length != 10 && length != 12 && length != 16)
                throw new ArgumentOutOfRangeException("Command Descriptor Block length should only be one of the following: 6, 10, 12, 16.", nameof(length));
            byte[] CDB = new byte[length];
            CDB[0] = opCode;
            return CDB;
        }

        public static byte[] BLANK(bool immed, byte blankingType, int startAddress)
        {
            byte[] CDB = GenericCDB(12, 0xA1);
            BitConverter.GetBytes(startAddress)
                .CopyTo(CDB, 2);
            CDB[1] = (byte)(
                (blankingType & 0b00000111) << 0 +
                immed.ToByte() << 4
                );
            return CDB;
        }

        public static byte[] CLOSE_TRACK_SESSION(bool immed, byte closeFunction, ushort logicalTrackNo)
        {
            byte[] CDB = GenericCDB(10, 0x5B);
            CDB[1] = immed.ToByte();
            CDB[2] = (byte)(closeFunction & 0b00000111);
            BitConverter.GetBytes(logicalTrackNo)
                .CopyTo(CDB, 3);
            return CDB;
        }
    }
}
