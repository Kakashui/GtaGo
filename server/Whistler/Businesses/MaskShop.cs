using GTANetworkAPI;
using System;
using System.Linq;
using Whistler.MoneySystem;
using Whistler.SDK;
using Whistler.Businesses;
using Whistler.Inventory.Models;
using Whistler.Inventory;
using Whistler.Helpers;
using Whistler.Businesses.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Core
{
    partial class BusinessManager : Script
    {
        [RemoteEvent("cancelMasks")]
        public static void RemoteEvent_cancelMasks(Player player)
        {
            try
            {
                player.StopAnimation();
                player.GetEquip().Update(false);
                //player.GetCustomization().SetMaskFace(player, false);
            }
            catch (Exception e) { _logger.WriteError("cancelMasks: " + e.ToString()); }
        }

        [RemoteEvent("buyMasks")]
        public static void RemoteEvent_buyMasks(Player player, int variation, int texture)
        {
            try
            {
                Business biz = BizList[player.GetData<int>("MASKS_SHOP")];
                var tempPrice = OldCustomization.Masks.FirstOrDefault(f => f.Variation == variation).Price;
                var priceModel = biz.GetProductPriceByProductId(0, tempPrice);

                BusinessManager.TakeProd(player, biz, player.GetCharacter(), new BuyModel("Mask", priceModel.MaterialsAmount, true, (cnt) =>
                {
                    BaseItem newItem = ItemsFabric.CreateClothes(ItemNames.Mask, true, variation, texture, false);
                    if (player.GetInventory().AddItem(newItem))
                    {
                        return cnt;
                    }
                    return 0;
                }), "Money_BuyMask", "Biz_129");
            }
            catch (Exception e) { _logger.WriteError("buyMasks: " + e.ToString()); }
        }
    }
}
