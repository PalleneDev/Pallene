using Pallene.SDK.MMC.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Pallene.SDK.MMC
{
    public struct DiscDrive
    {
        /// <summary>
        /// The string that can be displayed to the user when prompted with selecting a disc drive.<br/>
        /// For example, <c>ACME 35728E Disc Recorder</c>
        /// </summary>
        public required string FriendlyName;
        /// <summary>
        /// The internal identifier of the device, that can be passed on to connect to the selected disc drive.<br/>
        /// On Windows, this would be the drive letter, such as <c>E:</c>.
        /// </summary>
        public required string AccessPath;
    }

    public enum ScsiStatus
    {
        GOOD = 0x00,
        CHECK_CONDITION = 0x02,
        CONDITION_MET = 0x04,
        BUSY = 0x08,
        RESERVATION_CONFLICT = 0x18,
        TASK_SET_FULL = 0x28,
        ACA_ACTIVE = 0x30,
        TASK_ABORTED = 0x40
    }

    public class ScsiException: Exception
    {
        public ScsiException(): base() { }

        public ScsiException(string? message): base(message) { }

        public ScsiException(string? message, Exception? innerException): base(message, innerException) { }
    }

    internal static class BitUtils
    {
        public static byte ToByte(this bool input) { return input ? (byte)0b1 : (byte)0b0; }
    }
}
