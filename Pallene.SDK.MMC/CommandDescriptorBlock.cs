namespace Pallene.SDK.MMC
{
    public struct CommandDescriptorBlock
    {
        private byte[] bytes;

        public CommandDescriptorBlock(int length, byte opCode = 0x00)
        {
            if (length != 6 && length != 10 && length != 12 && length != 16)
                throw new ArgumentOutOfRangeException(nameof(length), "Command Descriptor Block length must only be one of the following: 6, 10, 12, 16.");
            bytes = new byte[length];
            bytes[0] = opCode;
        }

        public byte this[int i]
        {
            get => bytes[i];
            set => bytes[i] = value;
        }

        public bool this[int i, int j]
        {
            get => (bytes[i] >> j) % 2 == 1;
            set {
                if (value)
                    bytes[i] |= (byte)(1 << j);
                else
                    bytes[i] &= (byte)(~(1 << j));
            }
        }

        public static implicit operator byte[](CommandDescriptorBlock cdb) => cdb.bytes;

        public override string ToString() => $"CDB 0x{Convert.ToString(bytes[0], 16)}";
    }
}
