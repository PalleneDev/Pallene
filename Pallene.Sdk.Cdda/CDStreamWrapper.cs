using System;
using System.Collections.Generic;
using System.Text;

namespace Pallene.Sdk.Cdda
{
    public sealed class CDStreamWrapper(Stream inStream)
    {
        public Stream Stream { get; } = inStream;

        public CDTimecode Length
        {
            get => new CDTimecode(Stream.Length);
        }

        public bool HasContent
        {
            get => Stream.Length != 0;
        }

        public static CDStreamWrapper Empty = new(Stream.Null);

        public static explicit operator Stream(CDStreamWrapper wrapper) => wrapper.Stream;

        public static explicit operator CDStreamWrapper(Stream stream) => new(stream);
    }
}
