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

namespace Whistler.Core
{
    partial class BusinessManager : Script
    {
        public static void OpenBizShopMenu(Player player)
        {
            Business biz = BizList[player.GetData<int>("BIZ_ID")];
            List<List<string>> items = new List<List<string>>();

            foreach (var p in biz.Products) 
            {
                List<string> item = new List<string>();
                item.Add(p.Name);
                item.Add(p.Price.ToString());
                items.Add(item);
            }
            string json = JsonConvert.SerializeObject(items);
            
            player.TriggerEvent("shop24:open", json, player.GetCharacter().Money);
        }


        [RemoteEvent("shop24:buy")]
        public static void Event_ShopCallback(Player client, string data)
        {
            try
            {
                if (!client.IsLogged()) return;
                if (client.GetData<int>("BIZ_ID") == -1) return;
                Business biz = BizList[client.GetData<int>("BIZ_ID")];
                var basket = JsonConvert.DeserializeObject<Dictionary<string, int>>(data);
                if (basket == null) return;

                BusinessManager.TakeProd(client, biz, client.GetCharacter(), basket.Select(item => new BuyModel(item.Key, item.Value, false, (cnt) => GiveItems(client, item.Key, cnt))).ToList(), "Money_BuyShop", "Biz_116");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        private static int GiveItems(Player player, string name, int count)
        {
            if (!Enum.TryParse(name, out ItemNames itemName))
                return 0;
            BaseItem addItem = ItemsFabric.CreateByName(itemName);
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
                    addItem = ItemsFabric.CreateByName(itemName);
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
    }
}
