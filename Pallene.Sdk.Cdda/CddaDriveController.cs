using Pallene.Sdk.Core;

namespace Pallene.Sdk.Cdda
{
    public sealed class CddaDriveController(DiscDrive discDrive): CommonDriveController(discDrive)
    {

        public void SendCueSheet(CDAlbum album)
        {
            byte[] data = CueSheetEntry.ToByteArray(album.GetCueSheetEntries());
            CommandDescriptorBlock cdb = CdbGenerator.SendCueSheet((UInt24)data.Length);
            SystemAdapter.SendCommand(cdb, data, 50, false);
        }

    }
}
