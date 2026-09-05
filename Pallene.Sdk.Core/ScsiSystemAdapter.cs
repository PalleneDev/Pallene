namespace Pallene.Sdk.Core
{
    public interface IDriveFinder
    {
        public DiscDrive[] GetDiscDrives();
    }

    /// <summary>
    /// The base class for abstracting system-specific driver code.
    /// </summary>
    /// <param name="recorder">The disc drive to communicate with.</param>
    internal abstract class ScsiSystemAdapter(DiscDrive recorder)
    {
        /// <summary>
        /// The disc drive that the class will communicate with.
        /// </summary>
        public DiscDrive Recorder = recorder;

        /// <summary>
        /// Execute a SCSI command that requires no data transfer.
        /// </summary>
        /// <param name="cdb">The command descriptor block.</param>
        /// <param name="keepAlive"><see langword="true"/>, if application should keep exclusive connection to drive.</param>
        public abstract void SendCommand(CommandDescriptorBlock cdb, bool keepAlive = false);

        /// <summary>
        /// Excecute a SCSI command that sends data to the drive.
        /// </summary>
        /// <param name="cdb">The command descriptor block.</param>
        /// <param name="inputBuffer">The data to be sent to the drive.</param>
        /// <param name="keepAlive"><see langword="true"/>, if application should keep exclusive connection to drive.</param>
        public abstract void SendData(CommandDescriptorBlock cdb, byte[] inputBuffer, bool keepAlive = false);

        /// <summary>
        /// Execute a SCSI command that sends data back to the host.
        /// </summary>
        /// <param name="cdb">The command descriptor block.</param>
        /// <param name="keepAlive"><see langword="true"/>, if application should keep exclusive connection to drive.</param>
        public abstract void ReceiveData(CommandDescriptorBlock cdb, bool keepAlive = false);
    }
}
