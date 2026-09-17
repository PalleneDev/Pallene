namespace Pallene.Sdk.Core
{
    public struct CommandResult
    {
        public ScsiStatus Status { get; }

        public SenseData? SenseData { get; }

        internal CommandResult(ScsiStatus status)
        {
            Status = status;
        }

        internal CommandResult(ScsiStatus status, SenseData senseData): this(status)
        {
            if(status == ScsiStatus.CheckCondition)
                SenseData = senseData;
        }
    }
}
