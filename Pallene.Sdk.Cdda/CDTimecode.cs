using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Pallene.Sdk.Core;

namespace Pallene.Sdk.Cdda
{
    public struct CDTimecode
    {

        public byte Minutes { get; set; }

        public byte Seconds { get; set; }

        public byte Frames { get; set; }

        public static implicit operator byte[](CDTimecode val) => [ val.Minutes, val.Seconds, val.Frames ];

        public CDTimecode(byte minutes, byte seconds, byte frames)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(seconds, 60);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(frames, 75);
            Minutes = minutes;
            Seconds = seconds;
            Frames = frames;
        }
    }
}
