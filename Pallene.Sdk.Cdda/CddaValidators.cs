using System;
using System.Collections.Generic;
using System.Text;

namespace Pallene.Sdk.Cdda
{
    internal static class CddaValidators
    {
        public static void ThrowIfNotValidIndexNumber(byte value) => ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 99);

        public static void ThrowIfNotValidTrackNumber(byte value)
        {
            ArgumentOutOfRangeException.ThrowIfZero(value);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 99);
        }

        public static void ThrowIfNotValidIsrc(string value)
        {
            if (!value.All(char.IsAscii))
                throw new ArgumentException("ISRC must contain only ASCII characters.", nameof(value));
            if (value.Length != 12)
                throw new ArgumentException("ISRC must be exactly 12 characters.", nameof(value));
        }

        public static void ThrowIfNotValidMcn(string value)
        {
            if (!value.All(char.IsAscii))
                throw new ArgumentException("MCN must contain only ASCII characters.", nameof(value));
            if (value.Length != 13)
                throw new ArgumentException("MCN must be exactly 13 characters.", nameof(value));
        }
    }
}
