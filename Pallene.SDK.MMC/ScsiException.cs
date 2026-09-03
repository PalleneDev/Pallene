namespace Pallene.SDK.MMC
{
    public class ScsiException : Exception
    {
        public ScsiException(): base() { }

        public ScsiException(string? message): base(message) { }

        public ScsiException(string? message, Exception? innerException): base(message, innerException) { }
    }
}
