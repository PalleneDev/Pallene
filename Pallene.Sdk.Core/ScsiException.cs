namespace Pallene.Sdk.Core
{

    public class ScsiException : Exception
    {
        public ScsiStatus ScsiStatus { get; }
    
        public SenseData? SenseData { get; }

        public ScsiException(): base() { }

        public ScsiException(string? message): base(message) { }

        public ScsiException(string? message, Exception? innerException): base(message, innerException) { }

        public ScsiException(ScsiStatus scsiStatus): base($"Drive returned status {scsiStatus} in transaction.")
        {
            ScsiStatus = scsiStatus;
        }

        public ScsiException(ScsiStatus scsiStatus, SenseData senseData):
            base($"Drive returned {scsiStatus}: {senseData.SenseKey} (ASC {Convert.ToString(senseData.AdditionalSenseCode, 16)}/ASCQ {Convert.ToString(senseData.AdditionalSenseCodeQualifier, 16)}).")
        {
            ScsiStatus = scsiStatus;
            SenseData = senseData;
        }


        public static void ThrowIfStatusNotGood(ScsiStatus scsiStatus, SenseData? senseData)
        {
            // For purposes of multimedia control a simple check against Good is enough I think
            if (scsiStatus == ScsiStatus.Good) return;
            // Only CheckCondition provides sense data alongside itself
            if (scsiStatus == ScsiStatus.CheckCondition && senseData.HasValue)
                throw new ScsiException(scsiStatus, senseData.Value);
            else
                throw new ScsiException(scsiStatus);
        }
    }
}
