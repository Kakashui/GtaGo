using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Businesses.Models;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.MoneySystem;
using Whistler.SDK;
using Whistler.Inventory.Enums;

namespace Whistler.Businesses
{
    class IllegalShop : Script
    {

        [Command("illegal")]
        public static void CMD_OpenIllegalShop(Player player)
        {
            Trigger.ClientEvent(player, "illegalShop:open");
        }

        public static void OpenMenu(Player player, Business biz)
        {
            if (!player.IsLogged())
                return;
            player.GetCharacter().BusinessInsideId = biz.ID;
            var products = biz.Products
                .ToDictionary(p => p.Name, p => p.Price);
            Trigger.ClientEvent(player, "illegalShop:open", JsonConvert.SerializeObject(products));
        }

        [RemoteEvent("illegalShop:buyProduct")]
        public static void RemoteEvent_BuyProduct(Player player, string items)
        {
            if (!player.IsLogged())
                return;
            Business biz = BusinessManager.GetBusiness(player.GetCharacter().BusinessInsideId);
            if (biz == null)
                return;
            Dictionary<string, int> products = JsonConvert.DeserializeObject<Dictionary<string, int>>(items);

            BusinessManager.TakeProd(
                player, 
                biz, 
                player.GetCharacter(), 
                products.Select(item => new BuyModel(item.Key, item.Value, false, (cnt) =>  GiveProductByName(player, item.Key, cnt))).ToList(), 
                "Money_Illegal", 
                "Biz_116");
        }

        private static BaseItem CreateItemByType(ItemTypes itemType, ItemNames name , int count = 1)
        {
            BaseItem newItem = null;
            switch (itemType)
            {
                case ItemTypes.Weapon:
                    newItem = ItemsFabric.CreateWeapon(name, new List<int>() { -1, -1, -1, -1, -1, -1 }, false);
                    break;
                case ItemTypes.Other:
                    newItem = ItemsFabric.CreateOther(name, count, false);
                    break;
                case ItemTypes.Rod:
                    newItem = ItemsFabric.CreateRod(name, false);
                    break;
                case ItemTypes.Cage:
                    newItem = ItemsFabric.CreateCage(name, false);
                    break;
                default: break;
            }
            return newItem;
        }

        private static int GiveProductByName(Player player, string name, int count)
        {
            if (!Enum.TryParse(name, out ItemNames itemType))
                return 0;
            var type = Inventory.Configs.Config.GetTypeByName(itemType);

            if (type == ItemTypes.Other)
            {
                var newItem = CreateItemByType(type, itemType, count);
                if (newItem == null)
                    return 0;
                if (player.GetInventory().AddItem(newItem))
                    return count;
            }
            else
            {
                int countProd = 0;
                for (int i = 0; i < count; i++)
                {
                    var newItem = CreateItemByType(type, itemType);
                    if (newItem == null)
                        break;
                    if (player.GetInventory().AddItem(newItem))
                        countProd++;
                }
                return countProd;
            }
            return 0;
        }

        [RemoteEvent("illegalShop:closeMenu")]
        public static void RemoteEvent_CloseMenu(Player player)
        {
            if (!player.IsLogged())
                return;
            player.GetCharacter().BusinessInsideId = -1;
        }
    }
}