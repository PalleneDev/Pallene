using System;
using System.Collections.Generic;
using System.Text;

namespace Pallene.SDK.MMC
{
    public interface IScsiSystemAdapter
    {
        public static abstract IEnumerable<DiscDrive> GetDiscDrives();

        public DiscDrive DiscDrive { get; set; }

        public byte[] ScsiReceiveData(CommandDescriptorBlock cdb, bool keepAlive = false);

        public void ScsiSendData(CommandDescriptorBlock cdb, byte[] dataBuffer, bool keepAlive = false);

        public void ScsiSendCommand(CommandDescriptorBlock cdb, bool keepAlive = false);
    }
}
