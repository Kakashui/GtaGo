using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.Inventory;
using Whistler.Helpers;
using Whistler.Businesses.Models;
using Whistler.Entities;
using Whistler.SDK;
using Whistler.MoneySystem;

namespace Whistler.Core
{
    partial class BusinessManager : Script
    {
        public static void OpenBizShopMenu(ExtPlayer player)
        {
            Business biz = BizList[player.GetData<int>("BIZ_ID")];
            List<List<string>> items = new List<List<string>>();

            foreach (var p in biz.Products) 
            {
                List<string> item = new List<string>
                {
                    p.Name,
                    p.Price.ToString()
                };
                items.Add(item);
            }
            string json = JsonConvert.SerializeObject(items);
            
            SafeTrigger.ClientEvent(player,"shop24:open", json, player.Character.Money);
        }


        [RemoteEvent("shop24:buy")]
        public static void Event_ShopCallback(ExtPlayer client, string data, bool cashPay)
        {
            try
            {
                if (!client.IsLogged()) return;
                if (client.GetData<int>("BIZ_ID") == -1) return;
                Business biz = BizList[client.GetData<int>("BIZ_ID")];
                var basket = JsonConvert.DeserializeObject<Dictionary<string, int>>(data);
                if (basket == null) return;

                BusinessManager.TakeProd(client, biz, client.GetMoneyPayment(cashPay ? PaymentsType.Cash : PaymentsType.Card), basket.Select(item => new BuyModel(item.Key, item.Value, false, (cnt) => GiveItems(client, item.Key, cnt))).ToList(), "Покупка товаров в 24/7", "Недостаточно места в инвентаре");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static int GiveItems(ExtPlayer player, string name, int count)
        {
            if (!Enum.TryParse(name, out ItemNames itemName)) return 0;
            if (itemName == ItemNames.Invalid) return 0;

            ItemTypes type = Inventory.Configs.Config.GetTypeByName(itemName);
            BaseItem addItem = GetItemByNameAndType(type, itemName);
            if (addItem == null) return 0;

            addItem.Name = itemName;
            if (addItem.IsStackable())
            {
                addItem.Count = count;
                addItem.Promo = false;
                addItem.Index = -1;
                if (player.GetInventory().AddItem(addItem))
                    return count;
                else
                    return 0;
            }
            else
            {
                int newCount = 0;
                while (newCount < count)
                {
                    addItem = GetItemByNameAndType(type, itemName);
                    addItem.Name = itemName;
                    addItem.Count = 1;
                    addItem.Promo = false;
                    addItem.Index = -1;
                    if (player.GetInventory().AddItem(addItem))
                        newCount++;
                    else
                        return newCount;
                }
                return newCount;
            }
        }

        private static BaseItem GetItemByNameAndType(ItemTypes type, ItemNames name)
        {
            return type switch
            {
                ItemTypes.Rod => ItemsFabric.CreateRod(name, false),
                ItemTypes.Cage => ItemsFabric.CreateCage(name, false),
                _ => ItemsFabric.CreateByName(name)
            };
        }
    }
}
