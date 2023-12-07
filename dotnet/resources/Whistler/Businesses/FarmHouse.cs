using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Businesses.Models;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.Jobs.Farm;
using Whistler.NewJobs;
using Whistler.SDK;
using Whistler.Entities;

namespace Whistler.Businesses
{
    class FarmHouse : Script
    {
        private static int _countBuy = 3;
        public static void OpenFarmMenu(ExtPlayer player, Business business)
        {
            player.Character.BusinessInsideId = business.ID;
            var experience = player.GetWorker().GetExp(FarmManager.Job);
            SafeTrigger.ClientEvent(player,"farm::openMenu", experience, JsonConvert.SerializeObject(business.Products));
        }

        [RemoteEvent("farmHouse:buyProduct")]
        public static void FarmHouseBuyProduct(ExtPlayer player, string productName)
        {
            var character = player.Character;
            if (character == null)
                return;
            var business = BusinessManager.GetBusiness(character.BusinessInsideId);
            if (business == null)
                return;
            if (!Enum.IsDefined(typeof(ItemNames), productName))
                return;
            ItemNames name = (ItemNames)Enum.Parse(typeof(ItemNames), productName);
            var type = Inventory.Configs.Config.GetTypeByName(name);
            BaseItem item = null;
            switch (type)
            {
                case ItemTypes.Other:
                    item = Inventory.ItemsFabric.CreateOther(name, 1, false);
                    break;
                case ItemTypes.Seed:
                    item = Inventory.ItemsFabric.CreateSeed(name, _countBuy, false);
                    break;
                case ItemTypes.WateringCan:
                    item = Inventory.ItemsFabric.CreateWatering(name, false);
                    break;
                case ItemTypes.Fertilizer:
                    item = Inventory.ItemsFabric.CreateFertilizer(name, _countBuy, false);
                    break;
            }
            if (item == null)
            {
                Notify.SendError(player, "Недостаточно товара на складе");
                return;
            }
            BusinessManager.TakeProd(player, business, player.Character, 
                new BuyModel(
                    productName, 
                    item.Count, 
                    true, 
                    (cnt) => 
                    {
                        if (player.GetInventory().AddItem(item))
                            return cnt;
                        return 0;
                    }), 
                "Покупки на ферме", "Недостаточно места в инвентаре");
        }

        [RemoteEvent("farmHouse:closeMenu")]
        public static void CloseMenu(ExtPlayer player)
        {
            if (player.IsLogged())
                player.Character.BusinessInsideId = -1;
        }
    }
}
