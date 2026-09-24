using Pallene.Sdk.Core.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Pallene.Sdk.Core
{
    public abstract class CommonDriveController(DiscDrive discDrive)
    {
        protected DriveCommandSender CommandSender { get; } = new DriveCommandSender(discDrive);

        public bool CheckDriveReady()
        {
            try
            {
                CommandSender.TestUnitReady(5);
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

        public void EjectDisc() =>
            CommandSender.StartStopUnit(new CommandInputTypes.StartStopUnitArgs()
            {
                Immed = false,
                FormatLayerNumber = 0,
                PowerConditions = 0,
                FL = false,
                LoEj = true,
                Start = false
            }, 10);

        public void InsertDisc() =>
            CommandSender.StartStopUnit(new CommandInputTypes.StartStopUnitArgs()
            {
                Immed = false,
                FormatLayerNumber = 0,
                PowerConditions = 0,
                FL = false,
                LoEj = true,
                Start = true
            }, 30);

        public void PreventDiscRemoval() =>
            CommandSender.PreventAllowMediumRemoval(new CommandInputTypes.PreventAllowMediumRemovalArgs()
            {
                Persistent = false,
                Prevent = true
            }, 10);

        public void AllowDiscRemoval() =>
            CommandSender.PreventAllowMediumRemoval(new CommandInputTypes.PreventAllowMediumRemovalArgs()
            {
                Persistent = false,
                Prevent = true
            }, 10);
    }
}
