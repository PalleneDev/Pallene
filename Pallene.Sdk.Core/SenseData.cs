using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Pallene.Sdk.Core
{
    public struct SenseData
    {
        public bool Deferred { get; }
        public SenseKey SenseKey { get; }

        public byte AdditionalSenseCode { get; }

        public byte AdditionalSenseCodeQualifier { get; }

        public byte[]? SenseKeySpecific { get; }

        internal SenseData(
            byte[] rawSenseData
            )
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(rawSenseData.Length, 17); // Has to at least reach Sense Key Specific
            Deferred = (rawSenseData[0] & 0b01111111) == 0x71;
            SenseKey = (SenseKey)(rawSenseData[2] & 0b00001111);
            byte additionalSenseLength = rawSenseData[7];
            // I'm assuming that ASC, ASCQ and SKS are always returned, even if not containing data.
            AdditionalSenseCode = rawSenseData[12];
            AdditionalSenseCodeQualifier = rawSenseData[13];
            SenseKeySpecific = (rawSenseData[15] >> 7 == 0b1) ? rawSenseData[15..17] : null;
        }
    }
}
