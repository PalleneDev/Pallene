using Pallene.Sdk.Core;

namespace Pallene.Sdk.Core.Tests
{
    public class MediaEject
    {
        [Fact]
        public void MediaEjectTest()
        {
            DriveFinder df = new();
            var drives = df.GetDiscDrives();
            CommonDriveController sender = new(drives[0]);
            sender.EjectDisc();
        }
    }
}
