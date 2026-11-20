
namespace Pallene.Sdk.Cdda
{
    public enum SubchannelDataForm : byte
    {
        NoData = 0b00,
        RawData = 0b01 << 6,
        PackData = 0b11 << 6
    }
}