using System.Diagnostics.CodeAnalysis;

namespace Pallene.Sdk.Core
{
    public static class CdbGenerator
    {

        public static CommandDescriptorBlock Blank(bool immed, byte blankingType, int startAddress)
        {
            CommandDescriptorBlock CDB = new(12, 0xA1);
            BitConverter.GetBytes(startAddress)
                .CopyTo(CDB, 2);
            CDB[1, 4] = immed;
            CDB[1].SetBits(3, (byte)blankingType, 0);
            return CDB;
        }

        public static CommandDescriptorBlock CloseTrackSession(bool immed, byte closeFunction, ushort logicalTrackNo)
        {
            CommandDescriptorBlock CDB = new(10, 0x5B);
            CDB[1,0] = immed;
            CDB[2].SetBits(0, closeFunction, 3);
            BitConverter.GetBytes(logicalTrackNo)
                .CopyTo(CDB, 3);
            return CDB;
        }
        
        public static CommandDescriptorBlock StartStopUnit(CommandInputTypes.StartStopUnitArgs args)
        {
            CommandDescriptorBlock CDB = new(6, 0x1B);
            CDB[1, 0] = args.Immed;
            CDB[3].SetBits(0, args.FormatLayerNumber, 2);
            CDB[4, 0] = args.Start;
            CDB[4, 1] = args.LoEj;
            CDB[4, 2] = args.FL;
            CDB[4].SetBits(4, args.PowerConditions, 4);
            return CDB;
        }

        public static CommandDescriptorBlock TestUnitReady() => new CommandDescriptorBlock(6, 0x00);

        public static CommandDescriptorBlock GetConfiguration(byte rt, ushort startingFeatureNumber, ushort allocationLength)
        {
            CommandDescriptorBlock CDB = new(10, 0x46);
            CDB[1].SetBits(0, rt, 2);
            BitConverter.GetBytes(startingFeatureNumber)
                .CopyTo(CDB, 2);
            BitConverter.GetBytes(allocationLength)
                .CopyTo(CDB, 7);
            return CDB;
        }

        public static CommandDescriptorBlock PreventAllowMediumRemoval(CommandInputTypes.PreventAllowMediumRemovalArgs args)
        {
            CommandDescriptorBlock CDB = new(6, 0x1E);
            CDB[4, 0] = args.Prevent;
            CDB[4, 1] = args.Persistent;
            return CDB;
        }


        [Flags]
        public enum NotificationClassRequest: byte
        {
            OperationalChange = 0b0000_0010,
            PowerManagement = 0b0000_0100,
            ExternalRequest = 0b0000_1000,
            Media = 0b0001_0000,
            MultiHost = 0b0010_0000,
            DeviceBusy = 0b0100_0000
        }

        public static CommandDescriptorBlock GetEventStatusNotification(bool polled, NotificationClassRequest notificationClassRequest, ushort allocationLength)
        {
            CommandDescriptorBlock CDB = new(10, 0x4A);
            CDB[1, 0] = polled;
            CDB[4] = (byte)notificationClassRequest;
            BitConverter.GetBytes(allocationLength)
                .CopyTo(CDB, 7);
            return CDB;
        }
    }
}
