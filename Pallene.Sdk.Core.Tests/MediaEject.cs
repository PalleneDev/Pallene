using Pallene.Sdk.Core;

namespace Pallene.Sdk.Core.Tests
{
    internal class OpenDriveController(DiscDrive dd) : CommonDriveController(dd) { }

    public class MediaEject
    {
        [Fact]
        public void MediaEjectTest()
        {
            DriveFinder df = new();
            var drives = df.GetDiscDrives();
            OpenDriveController sender = new(drives[0]);
            sender.EjectDisc();
        }
    }
}
