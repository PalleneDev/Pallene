using Pallene.Sdk.Core.Windows;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Pallene.Sdk.Core
{
    public class DriveFinder: IDriveFinder
    {
        private readonly IDriveFinder SystemFinder;

        public DriveFinder()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                SystemFinder = new WindowsDriveFinder();
            else
                throw new PlatformNotSupportedException("Bummer, seems like your current operating system is not supported.");
        }

        public DiscDrive[] GetDiscDrives() => SystemFinder.GetDiscDrives();
    }
}
