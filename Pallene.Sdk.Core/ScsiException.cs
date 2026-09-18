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


        public static void ThrowIfStatusNotGood(CommandResult commandResult)
        {
            if (commandResult.Status == ScsiStatus.Good)
                return;
            else if (commandResult.SenseData.HasValue)
                throw new ScsiException(commandResult.Status, commandResult.SenseData.Value);
            else
                throw new ScsiException(commandResult.Status);
        }
    }
}
