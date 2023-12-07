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
    class CoinDonateItem : BaseDonateItem
    {
        public CoinDonateItem(int min, int max )
        {
            Min = min;
            Max = max;
        }

        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            var amount = GetRandomInRange();
            (player as ExtPlayer).AddMCoins(amount);
            player.UpdateCoins();
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:item:coins:ok".Translate(amount), 3000);
            return true;
        }
    }
}
