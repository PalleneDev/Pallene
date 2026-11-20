
namespace Pallene.Sdk.Cdda
{
    [Flags]
    public enum ControlField : byte
    {
        None = 0b0,
        UsesFourAudioChannels = 1 << 7,
        IsDataTrack = 1 << 6,
        IsDigitalCopyPermitted = 1 << 5,
        HasAudioPreEmphasis = 1 << 4,
    }
}