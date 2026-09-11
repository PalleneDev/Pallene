using Pallene.Sdk.Core.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Pallene.Sdk.Core
{
    public class DriveFinder: IDriveFinder
    {
        [SuppressMessage("Usage", "CA1859", Justification = "Future-proofing against adding compatibility with new systems.")]
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
