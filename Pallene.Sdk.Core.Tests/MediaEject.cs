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
            ScsiSender sender = new(drives[0]);
            sender.START_STOP_UNIT(false, 0, 0, false, true, false);
        }
    }
}
