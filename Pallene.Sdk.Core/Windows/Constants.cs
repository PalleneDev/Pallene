using System;
using System.Collections.Generic;
using System.Text;

namespace Pallene.Sdk.Core.Windows
{
    internal static class Constants
    {
        /// <summary>
        /// Multimedia devices only return fixed-type sense data, whose length can be max 252 bytes.
        /// </summary>
        public const byte SPT_SENSE_INFO_LENGTH = 252;
        public const int IOCTL_SCSI_PASS_THROUGH = 0x4D004;
        public const int IOCTL_SCSI_PASS_THROUGH_DIRECT = 0x4D014;
        public const int PASS_THROUGH_DIRECT_MIN_THRESHOLD = 16 * 1024;
    }
}
