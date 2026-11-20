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
    }
}
