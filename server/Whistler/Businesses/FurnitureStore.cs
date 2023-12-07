using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Helpers;
using Whistler.Houses;
using Whistler.Houses.Furnitures;
using Whistler.MoneySystem;
using Whistler.SDK;
using Whistler.GUI;
using Whistler.Businesses.Models;

namespace Whistler.Core
{
    partial class BusinessManager
    {
        private static void OnFurnitureStoreInteractionPressed(Player player, Business businessInstance)
        {
            if (businessInstance.Type != 27) return;
            player.TriggerEvent("furnitureStore:open", businessInstance.GetPriceByProductName("Parts").CurrentPrice);
        }

        [RemoteEvent("furnitureStore:playerBought")]
        public static void OnPlayerBoughtFurniture(Player player, string data)
        {
            var boughtItems = JsonConvert.DeserializeObject<List<FurnitureStoreItemDTO>>(data);
            DialogUI.Open(player, "houses_7", new List<DialogUI.ButtonSetting>
                {
                    new DialogUI.ButtonSetting
                    {
                        Name = "houses_4",
                        Icon = null,
                        Action = (p) => BuyFurnitures(player, HouseManager.GetHouse(player, true), boughtItems),
                    },

                    new DialogUI.ButtonSetting
                    {
                        Name = "houses_5",
                        Icon = null,
                        Action = (p) => BuyFurnitures(player, player.GetFamily()?.GetHouse(), boughtItems),
                    },

                    new DialogUI.ButtonSetting
                    {
                        Name = "houses_6",
                        Icon = null,
                        Action = (p) => { },
                    }
                });
        }
        internal static void BuyFurnitures(Player player, House house, List<FurnitureStoreItemDTO> boughtItems)
        {
            try
            {
                var businessId = player.GetData<int>("BIZ_ID");
                var business = BusinessManager.GetBusiness(businessId);
                if (business == null)
                    return;
                if (house == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_132", 3000);
                    return;
                }
                if (!house.GetAccessFurniture(player, Families.FamilyFurnitureAccess.ManagementFurniture))
                {                    
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_24", 3000);
                    return;
                }

                List<BuyModel> buyModels = new List<BuyModel>();
                foreach (var item in boughtItems)
                {
                    if (!FurnitureSettings.AllAvailableFurnitures.TryGetValue(item.Key, out var setting)) continue;
                    buyModels.Add(new BuyModel("Parts", setting.Cost * item.Count, false, 
                        (cnt) =>
                        {
                            int count = cnt / setting.Cost;
                            for (var i = 0; i < count; i++)
                                FurnitureService.AddFurniture(new Furniture(item.Key), house);
                            return cnt;
                        }));
                }

                if (BusinessManager.TakeProd(player, business, player.GetCharacter(), buyModels, "Money_BuyFurniture", null))
                {
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "buyFurn", 3000);
                    player.SendTip("tip_furniture_purchase");
                }
            }
            catch (Exception ex) { _logger.WriteError("FurnituresStore: " + ex); }
        }
    }


    internal class FurnitureStoreItemDTO
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}