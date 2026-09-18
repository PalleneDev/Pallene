using System;
using System.Collections.Generic;
using System.Text;

namespace Pallene.Sdk.Core
{
    public struct SenseData
    {
        public bool Deferred { get; }
        public SenseKey SenseKey { get; }

        public byte? AdditionalSenseCode { get; }

        public byte? AdditionalSenseCodeQualifier { get; }

        public byte[]? SenseKeySpecific { get; }

        internal SenseData(
            byte[] rawSenseData
            )
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(rawSenseData.Length, 8);
            Deferred = (rawSenseData[0] & 0b01111111) == 0x71;
            SenseKey = (SenseKey)(rawSenseData[2] & 0b00001111);
            byte additionalSenseLength = rawSenseData[7];
            AdditionalSenseCode = additionalSenseLength + 7 >= 12 ? rawSenseData[12] : null;
            AdditionalSenseCodeQualifier = additionalSenseLength + 7 >= 13 ? rawSenseData[13] : null;
            SenseKeySpecific = (rawSenseData[15] >> 7 == 0b1) ? [rawSenseData[15], rawSenseData[16], rawSenseData[17]] : null;
        }
    }
}
