using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.NewDonateShop.Configs;
using Whistler.SDK;

namespace Whistler.NewDonateShop
{
    public class Shop
    {
        public void BuyItem(PlayerGo player, int id)
        {
            var item = DonateService.Items[id];
            if (item.Exclusive)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:buy:wrong:excl".Translate(), 3000);
                return;
            }
            if(player.Account.GoCoins < item.Price)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:buy:wrong:coins".Translate(), 3000);
                player.UpdateCoins();
                return;
            }
            player.SubGoCoins(item.Price);
            player.UpdateCoins();
            player.Character.DonateInventory.AddItem(item.Id, false);
            DonateLog.DonateItemlog(player, item, "buy");
        }

        public void BuyPrimeAccount(PlayerGo player)
        {
            if (player.Account.GoCoins < DonateService.PrimeAccount.Price)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:buy:wrong:coins".Translate(), 3000);
                player.UpdateCoins();
                return;
            }
            player.SubGoCoins(DonateService.PrimeAccount.Price);
            player.AddPrime(DonateService.PrimeAccount.Days);
            DonateLog.OperationLog(player, DonateService.PrimeAccount.Price, $"buy Prime 30");
        }
    }
}
