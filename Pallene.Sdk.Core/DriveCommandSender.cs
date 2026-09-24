using Pallene.Sdk.Core.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Pallene.Sdk.Core
{
    public sealed class DriveCommandSender
    {
        [SuppressMessage("Usage", "CA1859", Justification = "Future-proofing against adding compatibility with new systems.")]
        private IScsiSystemAdapter SystemAdapter { get; }

        public DriveCommandSender(DiscDrive discDrive)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                SystemAdapter = new ScsiWindowsAdapter(discDrive);
            else
                throw new PlatformNotSupportedException("Bummer, seems like your current operating system is not supported.");
        }

        public void TestUnitReady(uint timeout)
        {
            CommandDescriptorBlock cdb = CdbGenerator.TestUnitReady();
            SystemAdapter.SendCommand(cdb, timeout);
        }

        public void StartStopUnit(CommandInputTypes.StartStopUnitArgs args, uint timeout)
        {
            CommandDescriptorBlock cdb = CdbGenerator.StartStopUnit(args);
            SystemAdapter.SendCommand(cdb, timeout);
        }

        public void PreventAllowMediumRemoval(CommandInputTypes.PreventAllowMediumRemovalArgs args, uint timeout)
        {
            CommandDescriptorBlock cdb = CdbGenerator.PreventAllowMediumRemoval(args);
            SystemAdapter.SendCommand(cdb, timeout);
        }
    }
}
