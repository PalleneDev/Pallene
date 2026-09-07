namespace Pallene.Sdk.Core.Windows
{
    public enum DataDirection
    {
        /// <summary>
        /// Write data to the device.
        /// </summary>
        DataOut = 0x0,
        /// <summary>
        /// Read data from the device.
        /// </summary>
        DataIn = 0x1,
        /// <summary>
        /// No data transfer or transfer direction is unknown.
        /// </summary>
        DataUnspecified = 0x2
    }
}