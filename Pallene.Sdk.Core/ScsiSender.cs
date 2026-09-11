using Pallene.Sdk.Core.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Pallene.Sdk.Core
{
    public class ScsiSender
    {
        [SuppressMessage("Usage", "CA1859", Justification = "Future-proofing against adding compatibility with new systems.")]
        private readonly IScsiSystemAdapter SystemAdapter;

        public ScsiSender(DiscDrive discDrive)
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                SystemAdapter = new ScsiWindowsAdapter(discDrive);
            else
                throw new PlatformNotSupportedException("Bummer, seems like your current operating system is not supported.");
        }

        public void Blank(bool immed, byte blankingType, int startAddress)
        {
            CommandDescriptorBlock cdb = CdbGenerator.Blank(immed, blankingType, startAddress);
            SystemAdapter.SendCommand(cdb, 60);
        }

        public void StartStopUnit(bool immed, byte formatLayerNumber, byte powerConditions, bool formatLayer, bool loEj, bool start)
        {
            CommandDescriptorBlock cdb = CdbGenerator.StartStopUnit(immed, formatLayerNumber, powerConditions, formatLayer, loEj, start);
            SystemAdapter.SendCommand(cdb, 5);
        }
    }
}
