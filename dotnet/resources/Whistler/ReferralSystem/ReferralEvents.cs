using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.ReferralSystem
{
    public class ReferralEvents: Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            ReferralService.Init();        
        }
    }
}
