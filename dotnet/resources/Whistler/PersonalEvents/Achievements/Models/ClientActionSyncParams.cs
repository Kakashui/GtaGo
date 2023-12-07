using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.PersonalEvents.Achievements.Models
{
    class ClientActionSyncParams
    {
        public int SyncTime { get; set; }
        public int SyncMaxPoint { get; set; }
        public ClientActionSyncParams(int syncTime, int syncMaxPoint)
        {
            SyncTime = syncTime;
            SyncMaxPoint = syncMaxPoint;
        }
    }
}
