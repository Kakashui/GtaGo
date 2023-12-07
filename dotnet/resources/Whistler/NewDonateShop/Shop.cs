using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.NewDonateShop.Configs;
using Whistler.SDK;

namespace Whistler.NewDonateShop
{
    public class Shop
    {
        public void BuyItem(ExtPlayer player, int id)
        {
            var item = DonateService.Items[id];
            if (item.Exclusive)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:buy:wrong:excl".Translate(), 3000);
                return;
            }
            if(!player.SubMCoins(item.Price))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:buy:wrong:coins".Translate(), 3000);
                player.UpdateCoins();
                return;
            }
            player.Character.DonateInventory.AddItem(item.Id, false);
            DonateLog.DonateItemlog(player, item, "buy");
        }

        public void BuyPrimeAccount(ExtPlayer player)
        {
            Models.Donate primeData = DonateService.DonateList.FirstOrDefault(x => x.Type == 1);
            if (primeData == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Ошибка магазина, обратитесь к администрации.", 3000);
                player.UpdateCoins();
                return;
            }
            if (player.Account.MCoins < primeData.Price)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно средств", 3000);
                player.UpdateCoins();
                return;
            }
            player.SubMCoins(primeData.Price);
            player.AddPrime(DonateService.PrimeAccount.Days);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Вы приобрели Premium аккаунт на 30 дней", 3000);
            DonateLog.OperationLog(player, primeData.Price, $"buy Prime 30");
        }
    }
}
