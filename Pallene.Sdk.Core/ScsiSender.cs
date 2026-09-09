using Pallene.Sdk.Core.Windows;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Pallene.Sdk.Core
{
    public class ScsiSender
    {
        private readonly IScsiSystemAdapter SystemAdapter;

        public ScsiSender(DiscDrive discDrive)
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                SystemAdapter = new ScsiWindowsAdapter(discDrive);
            else
                throw new PlatformNotSupportedException("Bummer, seems like your current operating system is not supported.");
        }

        public void BLANK(bool immed, CdbGenerator.BlankingType blankingType, int startAddress)
        {
            CommandDescriptorBlock cdb = CdbGenerator.BLANK(immed, blankingType, startAddress);
            SystemAdapter.SendCommand(cdb, 60);
        }

        public void START_STOP_UNIT(bool immed, byte formatLayerNumber, byte powerConditions, bool formatLayer, bool loEj, bool start)
        {
            CommandDescriptorBlock cdb = CdbGenerator.START_STOP_UNIT(immed, formatLayerNumber, powerConditions, formatLayer, loEj, start);
            SystemAdapter.SendCommand(cdb, 5);
        }
    }
}
