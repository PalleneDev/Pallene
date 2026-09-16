using System;
using System.Collections.Generic;
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
            bool deferred,
            SenseKey senseKey,
            byte additionalSenseCode,
            byte additionalSenseCodeQualifier,
            byte[]? senseKeySpecific
            )
        {
            Deferred = deferred;
            SenseKey = senseKey;
            AdditionalSenseCode = additionalSenseCode;
            AdditionalSenseCodeQualifier = additionalSenseCodeQualifier;
            SenseKeySpecific = senseKeySpecific;
        }
    }
}
