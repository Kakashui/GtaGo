using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.NewDonateShop.Interfaces;
using Whistler.SDK;

namespace Whistler.NewDonateShop.Models
{
    class ExpirianceDonateItem : BaseDonateItem
    {
        public ExpirianceDonateItem(int amount)
        {
            Amount = amount;
        }

        public override bool TryUse(Player player, int count, bool sell)
        {
            var character = player.GetCharacter();
            character.AddExp(player as PlayerGo, false);
            //Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:item:exp:ok".Translate(), 3000);
            return true;
        }
    }
}
