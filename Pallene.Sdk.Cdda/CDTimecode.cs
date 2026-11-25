using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Pallene.Sdk.Cdda
{
    public record struct CDTimecode
    {
        private const int BytesPerFrame = 2352;
        private const byte MaxAddressableMinute = 99;
        private const byte SecondsInMinute = 60;
        private const byte FramesPerSecond = 75;

        public static CDTimecode Empty { get; } = new();

        public byte Minutes { 
            get;
            set
            {
                ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value, MaxAddressableMinute);
                field = value;
            } 
        }

        public byte Seconds { 
            get;
            set
            {
                field = value;
                if(field >= SecondsInMinute)
                { field -= SecondsInMinute; Minutes++; }
            }
        }

        public byte Frames { 
            get; 
            set {
                field = value;
                if (field >= FramesPerSecond)
                { field -= FramesPerSecond; Seconds++; }
            }
        }

        public static implicit operator byte[](CDTimecode val) => [ val.Minutes, val.Seconds, val.Frames ];

        public long GetFrames() => (Minutes * SecondsInMinute + Seconds) * FramesPerSecond + Frames;

        public long GetBytes() => GetFrames() * BytesPerFrame;

        public CDTimecode(long byteCount) 
            : this( 
                  (byte)(byteCount / BytesPerFrame / FramesPerSecond / SecondsInMinute), 
                  (byte)((byteCount / BytesPerFrame / FramesPerSecond) % SecondsInMinute),
                  (byte)((byteCount / BytesPerFrame) % FramesPerSecond)
                  ) 
        { }

        public CDTimecode(): this(0, 0, 0) { }

        public CDTimecode(byte minutes, byte seconds, byte frames)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(seconds, SecondsInMinute);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(frames, FramesPerSecond);
            Minutes = minutes;
            Seconds = seconds;
            Frames = frames;
        }

        public override string ToString() => $"{Minutes:D2}:{Seconds:D2}:{Frames:D2}";

        public static CDTimecode operator +(CDTimecode left, CDTimecode right)
        {
            CDTimecode res = left;
            res.Minutes += right.Minutes;
            res.Seconds += right.Seconds;
            res.Frames += right.Frames;
            return res;
        }

        // I can't be bothered to write actual logic here
        // Go go gadget GetBytes()
        // Maybe I should do the same in operator+
        public static CDTimecode operator -(CDTimecode left, CDTimecode right)
        {
            if (right > left) return new();
            return new(left.GetBytes() - right.GetBytes());
        }

        public static bool operator >(CDTimecode left, CDTimecode right)
        {
            if (left.Minutes < right.Minutes) return false;
            if (left.Minutes == right.Minutes)
            {
                if (left.Seconds < right.Seconds) return false;
                if (left.Seconds == right.Seconds)
                {
                    if (left.Frames <= right.Frames) return false;
                }
            }
            return true;
        }

        public static bool operator <(CDTimecode left, CDTimecode right) => right > left;

        public static bool operator >=(CDTimecode left, CDTimecode right) => left == right || left > right;

        public static bool operator <=(CDTimecode left, CDTimecode right) => left == right || left < right;
    }
}
