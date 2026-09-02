using System;
using System.Collections.Generic;
using System.Text;

namespace Pallene.SDK.MMC
{
    public interface IScsiSystemAdapter
    {
        public static abstract IEnumerable<DiscDrive> GetDiscDrives();

        public DiscDrive DiscDrive { get; set; }

        public byte[] ScsiReceiveData(byte[] commandDescriptorBlock, bool keepAlive = false);

        public void ScsiSendData(byte[] commandDescriptorBlock, byte[] dataBuffer, bool keepAlive = false);

        public void ScsiSendCommand(byte[] commandDescriptorBlock, bool keepAlive = false);
    }
}
