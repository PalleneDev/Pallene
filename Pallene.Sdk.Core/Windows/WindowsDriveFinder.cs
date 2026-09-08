using System.Management;
using System.Runtime.Versioning;

namespace Pallene.Sdk.Core.Windows
{
    [SupportedOSPlatform("windows")]
    internal class WindowsDriveFinder : IDriveFinder
    {
        public DiscDrive[] GetDiscDrives()
        {
            var devices = new ManagementObjectSearcher("SELECT * FROM Win32_CDROMDrive")
                .Get();
            DiscDrive[] output = new DiscDrive[devices.Count];
            int i = 0;
            foreach (var drive in devices)
                output[i++] = new DiscDrive {
                    FriendlyName = (string)drive.GetPropertyValue("Name"),
                    AccessPath = (string)drive.GetPropertyValue("Drive")
                };
            return output;
        }
    }
}
