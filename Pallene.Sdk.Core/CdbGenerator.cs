namespace Pallene.Sdk.Core
{
    public static class CdbGenerator
    {
        #region BLANK
        public enum BlankingType
        {
            BlankDisc = 0b000,
            BlankDiscMinimally = 0b001,
            CDRW_BlankTrack = 0b010,
            UnReserveTrack = 0b011,
            BlankTrackTail = 0b100,
            UncloseLastCompleteSession = 0b101,
            CDRW_BlankLastNonEmptySession = 0b110,
            DVDRW_SL_BlankLastSession = 0b110
        }

        public static CommandDescriptorBlock BLANK(bool immed, BlankingType blankingType, int startAddress)
        {
            CommandDescriptorBlock CDB = new(12, 0xA1);
            BitConverter.GetBytes(startAddress)
                .CopyTo(CDB, 2);
            CDB[1, 4] = immed;
            CDB[1].SetBits(3, (byte)blankingType, 0);
            return CDB;
        }
        #endregion

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
