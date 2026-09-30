using Pallene.Sdk.Core.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Pallene.Sdk.Core
{
    public abstract class CommonDriveController
    {
        [SuppressMessage("Usage", "CA1859", Justification = "Future-proofing against adding compatibility with new systems.")]
        protected IScsiSystemAdapter SystemAdapter { get; }

        public CommonDriveController(DiscDrive discDrive)
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                SystemAdapter = new ScsiWindowsAdapter(discDrive);
            else
                throw new PlatformNotSupportedException("Bummer, seems like your current operating system is not supported.");
        }

        public bool CheckDriveReady()
        {
            CommandDescriptorBlock cdb = CdbGenerator.TestUnitReady();
            try
            {
                SystemAdapter.SendCommand(cdb, 5);
            }
            catch(ScsiException exception)
            {
                // Technically there exists a recommended set of CheckCondition ASCs, but I don't feel like checking against all of them.
                if (!(
                    exception.ScsiStatus == ScsiStatus.CheckCondition &&
                    exception.SenseData.HasValue &&
                    exception.SenseData.Value.SenseKey == SenseKey.NotReady
                    ))
                    throw;
                return false;
            }
            return true;
        }

        public void EjectDisc()
        {
            CommandDescriptorBlock cdb = CdbGenerator.StartStopUnit(false, 0, 0, false, true, false);
            SystemAdapter.SendCommand(cdb, 10);
        }

        public void InsertDisc()
        {
            CommandDescriptorBlock cdb = CdbGenerator.StartStopUnit(false, 0, 0, false, true, true);
            SystemAdapter.SendCommand(cdb, 30);
        }

        public void PreventDiscRemoval()
        {
            CommandDescriptorBlock cdb = CdbGenerator.PreventAllowMediumRemoval(false, true);
            SystemAdapter.SendCommand(cdb, 10);
        }

        public void AllowDiscRemoval()
        {
            CommandDescriptorBlock cdb = CdbGenerator.PreventAllowMediumRemoval(false, false);
            SystemAdapter.SendCommand(cdb, 10);
        }
    }
}
