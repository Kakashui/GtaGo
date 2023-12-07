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

        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            var character = player.Character;
            character.AddExp(player as ExtPlayer, false);
            //Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:item:exp:ok".Translate(), 3000);
            return true;
        }
    }
}
