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
    public interface IScsiSystemAdapter
    {
        /// <summary>
        /// The disc drive that the class will communicate with.
        /// </summary>
        public DiscDrive Recorder { get; }

        /// <summary>
        /// Execute a SCSI command that requires no data transfer.
        /// </summary>
        /// <param name="cdb">The command descriptor block.</param>
        public void SendCommand(CommandDescriptorBlock cdb, uint timeout);

        /// <summary>
        /// Excecute a SCSI command that sends data to the drive.
        /// </summary>
        /// <param name="cdb">The command descriptor block.</param>
        /// <param name="inputBuffer">The data to be sent to the drive.</param>
        public void SendCommand(CommandDescriptorBlock cdb, byte[] inputBuffer, uint timeout, bool? directMemAccess);

        /// <summary>
        /// Execute a SCSI command that sends data back to the host.
        /// </summary>
        /// <param name="cdb">The command descriptor block.</param>
        public void SendCommand(CommandDescriptorBlock cdb, ref byte[] outputBuffer, uint timeout, bool? directMemAccess);
    }
}
