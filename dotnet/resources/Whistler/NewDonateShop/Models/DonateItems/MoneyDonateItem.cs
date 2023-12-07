using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
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

        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            var character = player.Character;
            var amount = GetRandomInRange();
            MoneySystem.Wallet.MoneyAdd(player.Character, amount, "Победа в рулетке");
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:item:money:ok".Translate(amount), 3000);
            return true;
        }
    }
}
