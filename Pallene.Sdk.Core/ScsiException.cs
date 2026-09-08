namespace Pallene.Sdk.Core
{
    public enum SenseKey {
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
        Miscompare = 0xE
    }

    public class ScsiException : Exception
    {
        public SenseKey SenseKey { get; }
        public int Asc { get; }

        public int Ascq { get; }

        public ScsiException(): base() { }

        public ScsiException(string? message): base(message) { }

        public ScsiException(string? message, Exception? innerException): base(message, innerException) { }

        public ScsiException(SenseKey senseKey, int asc, int ascq) :
            base($"Drive reported error: {senseKey.ToString()} -- ASC {asc} / ASCQ {ascq}")
        {
            SenseKey = senseKey;
            Asc = asc;
            Ascq = ascq;
        }
    }
}
