using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Businesses.Models;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.MoneySystem;
using Whistler.SDK;

namespace Whistler.Core
{
    partial class BusinessManager : Script
    {
        public static void OpenBurgerShot(ExtPlayer player, int bizId)
        {
            var business = BizList[bizId];
            var items = business.Products
                .ToDictionary(p => p.Name, p => p.Price);

            var dto = new BurgerShotDto
            {
                Money = (int)player.Character.Money,
                ItemCostByName = items
            };

            player.TriggerEventSafe("burgerShot:open", JsonConvert.SerializeObject(dto));
        }

        [RemoteEvent("burgerShot:buyItems")]
        public static void HandleBurgerShotBuy(ExtPlayer player, string json, bool cashPay)
        {
            try
            {
                if (!player.IsLogged()) return;
                var businessId = player.GetData<int>("BIZ_ID");
                var business = GetBusiness(businessId);
                if (business == null) return;
                var cart = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);

                BusinessManager.TakeProd(
                    player,
                    business,
                    player.GetMoneyPayment(cashPay ? PaymentsType.Cash : PaymentsType.Card),
                    cart.Select(item => new BuyModel(
                        item.Key, 
                        item.Value, 
                        false, 
                        (cnt) => GiveProductToInventory(player, item.Key, cnt))).ToList(), 
                    "Покупка пищи в BurgerShot", "Недостаточно места в инвентаре");
            }
            catch (Exception e) 
            {
                _logger.WriteError($"HandleBurgerShotBuy:\n{e}\n{json}"); 
            }
        }

        private static int GiveProductToInventory(ExtPlayer player, string name, int count)
        {
            if (!Enum.TryParse(name, out ItemNames itemName))
                return 0;

            BaseItem addItem = ItemsFabric.CreateByName(itemName);
            addItem.Name = itemName;
            addItem.Count = count;
            addItem.Promo = false;
            addItem.Index = -1;
            if (player.GetInventory().AddItem(addItem))
                return count;
            return 0;
        }

        private class BurgerShotDto
        {
            public int Money { get; set; }
            public Dictionary<string, int> ItemCostByName { get; set; }
        }
    }
}
