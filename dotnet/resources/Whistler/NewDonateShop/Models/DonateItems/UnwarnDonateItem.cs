using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.NewDonateShop.Models
{
    class UnwarnDonateItem : BaseDonateItem
    {
        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            var character = player.Character;
            if(character.Warns < 1)
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:item:warn:no".Translate(), 3000);
                return false;
            }
            character.Warns--;
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_236".Translate(character.Warns), 3000);
            GUI.MainMenu.SendStats(player);
            return true;
        }
    }
}
