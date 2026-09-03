using Pallene.SDK.MMC.Windows;
using System.Collections;

namespace Pallene.SDK.MMC
{
    public static class CdbGenerator
    {

        public static CommandDescriptorBlock BLANK(bool immed, byte blankingType, int startAddress)
        {
            CommandDescriptorBlock CDB = new(12, 0xA1);
            BitConverter.GetBytes(startAddress)
                .CopyTo(CDB, 2);
            CDB[1, 4] = immed;
            CDB[1].SetBits(blankingType, 3, 0);
            return CDB;
        }

        public static CommandDescriptorBlock CLOSE_TRACK_SESSION(bool immed, byte closeFunction, ushort logicalTrackNo)
        {
            CommandDescriptorBlock CDB = new(10, 0x5B);
            CDB[1,0] = immed;
            CDB[2].SetBits(0, closeFunction, 3);
            BitConverter.GetBytes(logicalTrackNo)
                .CopyTo(CDB, 3);
            return CDB;
        }
    }
}
