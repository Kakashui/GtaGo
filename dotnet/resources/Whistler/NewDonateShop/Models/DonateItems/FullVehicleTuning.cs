using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.NewDonateShop.Models
{
    class FullVehicleTuning : BaseDonateItem
    {
        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            if (!player.IsInVehicle)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:veh:noveh".Translate(), 3000);
                return false;
            }
            BusinessManager.LsCustomOpen(player, true);
            SafeTrigger.SetData(player, "lscustomByDonate", true);
            return true;
        }
    }
}
