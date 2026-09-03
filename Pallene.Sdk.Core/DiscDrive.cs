namespace Pallene.Sdk.Core
{
    public struct DiscDrive
    {
        /// <summary>
        /// The string that can be displayed to the user when prompted with selecting a disc drive.<br/>
        /// For example, <c>ACME 35728E Disc Recorder</c>
        /// </summary>
        public required string FriendlyName;
        /// <summary>
        /// The internal identifier of the device, that can be passed on to connect to the selected disc drive.<br/>
        /// On Windows, this would be the drive letter, such as <c>E:</c>.
        /// </summary>
        public required string AccessPath;
    }
}
