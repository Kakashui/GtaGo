using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.NewDonateShop.Models
{
    class UndemorganDonateItem : BaseDonateItem
    {
        public override bool TryUse(Player player, int count, bool sell)
        {
            var character = player.GetCharacter();
            if(character.DemorganTime < 1)
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:item:demorgan:no".Translate(), 3000);
                return false;
            }
            player.GetCharacter().DemorganTime = 0;
            player.TriggerEvent("admin:releaseDemorgan");
            Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "dshop:item:demorgan:ok".Translate(), 3000);
            return true;
        }
    }
}
