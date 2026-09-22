using Pallene.Sdk.Core.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Pallene.Sdk.Core
{
    public class DriveController
    {
        [SuppressMessage("Usage", "CA1859", Justification = "Future-proofing against adding compatibility with new systems.")]
        private readonly IScsiSystemAdapter SystemAdapter;

        public DriveController(DiscDrive discDrive)
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

        private async Task PollForMediaEventNotifications(int pollingInterval, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            byte[] outputBuffer = new byte[8];
            CommandDescriptorBlock cdb = CdbGenerator.GetEventStatusNotification(true, CdbGenerator.NotificationClassRequest.Media, (ushort)outputBuffer.Length);
            while(true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                SystemAdapter.SendCommand(cdb, ref outputBuffer, 1, false);
                MediaNotificationData notificationData = new(outputBuffer);
                if (notificationData.EventCode == EventCode.EjectRequest || notificationData.EventCode == EventCode.NewMedia)
                    Console.WriteLine("Hurrah");
                await Task.Delay(pollingInterval, cancellationToken);
            }
        }

        private enum EventCode : byte
        {
            NoChange,
            EjectRequest,
            NewMedia,
            MediaRemoval,
            MediaChanged,
            BGFormatCompleted,
            BGFormatRestarted
        }

        private struct MediaNotificationData
        {
            public EventCode EventCode { get; }
            public bool MediaPresent { get; }
            public bool TrayOpen { get; }

            internal MediaNotificationData(byte[] notificationBuffer)
            {
                ArgumentOutOfRangeException.ThrowIfLessThan(notificationBuffer.Length, 8);
                EventCode = (EventCode)(notificationBuffer[4] & 0b0000_1111);
                MediaPresent = (notificationBuffer[5] & 0b0000_0010 >> 1) == 0b1;
                TrayOpen = (notificationBuffer[5] & 0b0000_0001) == 0b1;
            }
        }
    }
}
