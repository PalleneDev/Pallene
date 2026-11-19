using System;
using System.Collections.Generic;
using System.Text;

namespace Pallene.Sdk.Cdda
{
    public sealed record class Isrc
    {
        private string recordingCode;

        public Isrc(string recordingCode)
        {
            if (!recordingCode.All(char.IsAscii))
                throw new ArgumentException("ISRC must contain only ASCII characters.", nameof(recordingCode));
            if (recordingCode.Length != 12)
                throw new ArgumentException("ISRC must be exactly 12 characters.", nameof(recordingCode));
            this.recordingCode = recordingCode;   
        }

        public static implicit operator string(Isrc val) => val.recordingCode;

        public static implicit operator Isrc(string val) => new Isrc(val);
    }
}
