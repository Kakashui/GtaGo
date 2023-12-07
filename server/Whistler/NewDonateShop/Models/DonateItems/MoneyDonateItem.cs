using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.NewDonateShop.Models
{
    class MoneyDonateItem: BaseDonateItem
    {
        public MoneyDonateItem(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public override bool TryUse(Player player, int count, bool sell)
        {
            var character = player.GetCharacter();
            var amount = GetRandomInRange();
            MoneySystem.Wallet.MoneyAdd(player.GetCharacter(), amount, "Money_DonateRoulette");
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:item:money:ok".Translate(amount), 3000);
            return true;
        }
    }
}
