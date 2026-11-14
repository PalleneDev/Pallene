
namespace Pallene.Sdk.Cdda
{
    public enum MainDataForm : byte
    {
        AudioData = 0b00,
        AudioNoData = 0b01,
        // There's multiple others but they're not audio so I'm not implementing them rn
    }
}