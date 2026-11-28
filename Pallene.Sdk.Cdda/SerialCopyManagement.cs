
namespace Pallene.Sdk.Cdda
{
    [Flags]
    public enum SerialCopyManagement : byte
    {
        None = 0,
        /// <summary>
        /// If set, when making a copy of the disc, the <see cref="ControlField.IsDigitalCopyPermitted"/> will be unset on the copy, preventing second-generation copies.<br/>
        /// This may be ignored by copying software.
        /// </summary>
        AlternateCopyBit = 1 << 7
    }
}