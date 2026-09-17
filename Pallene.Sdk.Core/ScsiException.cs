namespace Pallene.Sdk.Core
{

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
