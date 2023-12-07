using System;
using System.Linq;
using Whistler.SDK;
using Whistler.Inventory.Models;
using Whistler.Inventory;
using GTANetworkAPI;
using Whistler.Businesses;
using Whistler.MoneySystem;
using Whistler.Helpers;
using Whistler.Businesses.Models;
using Whistler.Inventory.Enums;
using Whistler.Entities;

namespace Whistler.Core
{
    partial class BusinessManager : Script
    {
        [RemoteEvent("cancelClothes")]
        public static void RemoteEvent_cancelClothes(ExtPlayer player)
        {
            try
            {
                player.StopAnimation();
                player.GetEquip().Update(false);
                player.ChangePosition(player.Character.ExteriorPos);
                SafeTrigger.UpdateDimension(player,  0);

                player.Character.ExteriorPos = null;
            }
            catch (Exception e) { _logger.WriteError("cancelClothes: " + e.ToString()); }
        }

        [RemoteEvent("buyClothes")]
        public static void RemoteEvent_buyClothes(ExtPlayer player, int type, int variation, int texture, bool cashPay)
        {
            try
            {
                Business biz = BizList[player.GetData<int>("CLOTHES_SHOP")];
                var prod = biz.Products[0];

                var tempPrice = 0;
                bool gender = player.GetGender();
                BaseItem addItem = null;
                switch (type)
                {
                    case 0:
                        tempPrice = OldCustomization.Hats[gender].FirstOrDefault(h => h.Variation == variation).Price;
                        addItem = ItemsFabric.CreateClothes(ItemNames.Hat, gender, variation, texture, false);
                        break;
                    case 1:
                        tempPrice = OldCustomization.Tops[gender].FirstOrDefault(t => t.Variation == variation).Price;
                        addItem = ItemsFabric.CreateClothes(ItemNames.Top, gender, variation, texture, false);
                        break;
                    case 2:
                        tempPrice = OldCustomization.Underwears[gender].FirstOrDefault(h => h.Value.Top == variation).Value.Price;
                        addItem = ItemsFabric.CreateClothes(ItemNames.Shirt, gender, variation, texture, false);
                        break;
                    case 3:
                        tempPrice = OldCustomization.Legs[gender].FirstOrDefault(l => l.Variation == variation).Price;
                        addItem = ItemsFabric.CreateClothes(ItemNames.Leg, gender, variation, texture, false);
                        break;
                    case 4:
                        tempPrice = OldCustomization.Feets[gender].FirstOrDefault(f => f.Variation == variation).Price;
                        addItem = ItemsFabric.CreateClothes(ItemNames.Feet, gender, variation, texture, false);
                        break;
                    case 5:
                        tempPrice = OldCustomization.Gloves[gender].FirstOrDefault(f => f.Variation == variation).Price;
                        addItem = ItemsFabric.CreateClothes(ItemNames.Gloves, gender, variation, texture, false);
                        break;
                    case 6:
                        tempPrice = OldCustomization.Accessories[gender].FirstOrDefault(f => f.Variation == variation).Price;
                        addItem = ItemsFabric.CreateClothes(ItemNames.Watches, gender, variation, texture, false);
                        break;
                    case 7:
                        tempPrice = OldCustomization.Glasses[gender].FirstOrDefault(f => f.Variation == variation).Price;
                        addItem = ItemsFabric.CreateClothes(ItemNames.Glasses, gender, variation, texture, false);
                        break;
                    case 8:
                        tempPrice = OldCustomization.Jewerly[gender].FirstOrDefault(f => f.Variation == variation).Price;
                        addItem = ItemsFabric.CreateClothes(ItemNames.Jewelry, gender, variation, texture, false);
                        break;
                    case 9:
                        tempPrice = OldCustomization.Bags[gender].FirstOrDefault(f => f.Variation == variation).Price;
                        ItemNames backpackType = OldCustomization.BackpacksConfig[gender].ContainsKey(variation) ? OldCustomization.BackpacksConfig[gender][variation] : ItemNames.Invalid;
                        addItem = ItemsFabric.CreateClothes(backpackType, gender, variation, texture, false);
                        break;
                }
                if (addItem == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно товара на складе", 3000);
                    return;
                }

                var priceModel = biz.GetProductPriceByProductId(0, tempPrice);

                BusinessManager.TakeProd(player, biz, player.GetMoneyPayment(cashPay ? PaymentsType.Cash : PaymentsType.Card),
                    new BuyModel("Clothes", priceModel.MaterialsAmount, true, 
                        (cnt) =>
                        {
                            if (player.GetInventory().AddItem(addItem))
                            {
                                player.CreatePlayerAction(PersonalEvents.PlayerActions.BuyClothes, priceModel.Price);
                                return cnt;
                            }
                            return 0;
                        }
                    ), "Покупка одежды", "Недостаточно места в инвентаре");
            }
            catch (Exception e) { _logger.WriteError($"BuyClothes (type: {type}, variation: {variation}, texture: {texture}): " + e.ToString()); }
        }
    }
}
