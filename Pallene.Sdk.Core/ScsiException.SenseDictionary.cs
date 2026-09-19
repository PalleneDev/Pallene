using System;
using System.Collections.Generic;
using System.Text;

namespace Pallene.Sdk.Core
{
    public partial class ScsiException
    {
        private static readonly Dictionary<(byte asc, byte ascq), string> StaticErrorAssignments = new Dictionary<(byte asc, byte ascq), string>
        {
            #region ASC 0x00
            {(0x00, 0x00), "No Additional Sense Information"},
            {(0x00, 0x06), "I/O Process Terminated" },
            {(0x00, 0x16), "Operation In Progress" },
            {(0x00, 0x17), "Cleaning Requested" },
            #endregion
            #region ASC 0x02
            {(0x02, 0x00), "No Seek Complete" },
            #endregion
            #region ASC 0x04
            {(0x04, 0x00), "Logical Unit Not Ready, Cause Not Reportable" },
            {(0x04, 0x01), "Logical Unit is in process of becoming ready" },
            {(0x04, 0x02), "Logical Unit Not Ready, Initializing Command Required" },
            {(0x04, 0x03), "Logical Unit Not Ready, Manual Intervention Required" },
            {(0x04, 0x04), "Logical Unit Not Ready, Format In Progress (SK Specific Bytes contains progress indication)" },
            {(0x04, 0x07), "Logical Unit Not Ready, Operation In Progress" },
            {(0x04, 0x08), "Logical Unit Not Ready, Long Write In Progress" },
            {(0x04, 0x09), "Logical Unit Not Ready, Self-Test In Progress"},
            #endregion
            #region ASC 0x05
            {(0x05, 0x00), "Logical Unit does not respond to selection"},
            #endregion
            #region ASC 0x06
            {(0x06, 0x00), "No Reference Position Found"},
            #endregion
            #region ASC 0x08
            {(0x08, 0x00), "Logical Unit Communication Failure"},
            {(0x08, 0x01), "Logical Unit Communication Timeout"},
            {(0x08, 0x02), "Logical Unit Communication Parity Error"},
            {(0x08, 0x03), "Logical Unit Communication CRC Error (ULTRA-DMA/32)"},
            #endregion
            #region ASC 0x09
            {(0x09, 0x00), "Track Following Error"},
            {(0x09, 0x01), "Tracking Servo Failure" },
            {(0x09, 0x02), "Focus Servo Failure" },
            {(0x09, 0x03), "Spindle Servo Failure" },
            {(0x09, 0x04), "Head Select Fault" },
            #endregion
            #region ASC 0x0A
            {(0x0A, 0x00), "Error Log Overflow"},
            #endregion
            #region ASC 0x0B
            {(0x0B, 0x00), "Warning"},
            {(0x0B, 0x01), "Warning - Specified Temperature Exceeded" },
            {(0x0B, 0x02), "Warning - Enclosure Degraded" },
            {(0x0B, 0x03), "Warning - Background Self-Test Failed" },
            {(0x0B, 0x04), "Warning - Background Pre-Scan Detected Medium Error" },
            {(0x0B, 0x05), "Warning - Background Medium Scan Detected Medium Error"},
            #endregion
            #region ASC 0x0C
            {(0x0C, 0x00), "Write Error"},
            {(0x0C, 0x01), "Write Error - Recovered With Auto-Reallocation"},
            {(0x0C, 0x02), "Write Error - Auto-Reallocation Failed"},
            {(0x0C, 0x03), "Write Error - Recommend Reassignment"},
            {(0x0C, 0x07), "Write Error - Recovery Needed"},
            {(0x0C, 0x08), "Write Error - Recovery Failed"},
            {(0x0C, 0x09), "Write Error - Loss Of Streaming"},
            {(0x0C, 0x0A), "Write Error - Padding Blocks Added"},
            {(0x0C, 0x0F), "Defects In Error Window"},
            #endregion
            #region ASC 0x11
            {(0x11, 0x00), "Unrecovered Read Error"},
            {(0x11, 0x01), "Read Retries Exhausted"},
            {(0x11, 0x02), "Error Too Long To Correct"},
            {(0x11, 0x05), "L-EC Uncorrectable Error"},
            {(0x11, 0x06), "CIRC Unrecovered Error"},
            {(0x11, 0x0D), "Error Reading UPC/EAN Number"},
            {(0x11, 0x10), "Error Reading ISRC Number"},
            {(0x11, 0x11), "Read Error - Loss Of Streaming"},
            #endregion
            #region ASC 0x15
            #endregion
        };

        public static string GetAdditionalSenseErrorDescription(byte asc, byte ascq)
        {
            if (StaticErrorAssignments.ContainsKey((asc, ascq)))
                return StaticErrorAssignments[(asc, ascq)];
            else if ((asc >= 0x80 && asc <= 0xFF) || (ascq >= 0x80 && ascq <= 0xFF))
                return $"Vendor Specific Value";
            else if (asc == 0x40)
                return $"Diagnostic Failure On Component {Convert.ToString(ascq, 16)}";
            else if (asc == 0x4D)
                return $"Tagged Overlapped Commands (Queue Tag {Convert.ToString(ascq, 16)})";
            else
                return $"Unable to parse ASC/ASCQ values";
        }
    }
}
