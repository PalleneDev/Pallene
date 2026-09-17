namespace Pallene.Sdk.Core
{
    public enum SenseKey
    {
        NoSense = 0x0,
        RecoveredError = 0x1,
        NotReady = 0x2,
        MediumError = 0x3,
        HardwareError = 0x4,
        IllegalRequest = 0x5,
        UnitAttention = 0x6,
        DataProtect = 0x7,
        BlankCheck = 0x8,
        VendorSpecific = 0x9,
        CopyAborted = 0xA,
        AbortedCommand = 0xB,
        VolumeOverflow = 0xD,
        Miscompare = 0xE,
        Completed = 0xF
    }
}
