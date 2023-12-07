using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.SDK;

namespace Whistler.NewDonateShop.Models
{
    class OrganizqtionDonateItem : BaseDonateItem
    {
        public override bool TryUse(Player player, int count, bool sell)
        {
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "dshop:item:admin:temp".Translate(), 3000);
            return false;
        }
    }
}
