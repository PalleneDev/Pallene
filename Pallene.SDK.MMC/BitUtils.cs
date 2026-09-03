using Pallene.SDK.MMC.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Pallene.SDK.MMC
{
    internal static class BitUtils
    {
        public static byte ToByte(this bool input) { return input ? (byte)0b1 : (byte)0b0; }

        public static byte SetBits(this byte input, int startIndex, byte value, int length)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(startIndex, 8);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(startIndex + length, 8);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);
            int mask = 0;
            for (int i = 0; i < length; i++)
            {
                mask += 0b1 << i;
            }
            value <<= startIndex;
            return input = (byte)(input & ~mask + value & mask);
        }
    }
}
