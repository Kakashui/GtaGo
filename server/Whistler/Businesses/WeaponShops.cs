using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using Whistler.Core;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.MoneySystem;
using Whistler.SDK;
using Newtonsoft.Json;
using Whistler.Inventory.Configs;
using Whistler.Helpers;
using Whistler.Businesses.Models;

namespace Whistler.Businesses
{
    public class WeaponShops : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(WeaponShops)); 
        private static Vector3 _playerPosition = new Vector3(254.787, -48.306, 71.151);
        public static void Open(Player player, Business business)
        {
            List<List<int>> prices = new List<List<int>>();
            business.Products.ForEach(p =>
            {
                if(p.Lefts > 0)
                {
                    ItemNames item;
                    if (Enum.TryParse(p.Name, out item))
                    {
                        prices.Add(new List<int> { (int)item, p.Price });
                    }
                    else
                        _logger.WriteError($"bad weapon product name: {p.Name}");
                }
            });
            //player.GetCharacter().ExteriorPos = player.Position;
            player.Dimension = Dimensions.RequestPrivateDimension();
            //player.ChangePosition(_playerPosition);
            player.TriggerEvent("wshop:open", prices);
        }

        [RemoteEvent("wshop:close")]
        public void CloseShop(Player player)
        {
            player.Dimension = 0;
        }


        [RemoteEvent("wshop:buy:weapon")]
        public void Buy(Player player, int id, string componentsString, int count, bool payCard)
        {
            try
            {
                if (!player.CheckLic(GUI.Documents.Enums.LicenseName.Weapon))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_115".Translate(), 3000);
                    return;
                }
                var name = (ItemNames)id;
                var type = Inventory.Configs.Config.GetTypeByName(name);
                if (type != ItemTypes.Weapon && type != ItemTypes.Ammo && name != ItemNames.BodyArmor) 
                    return;
                int bizid = player.GetData<int>("GUNSHOP");
                var biz = BusinessManager.BizList[bizid];
                
                if (type == ItemTypes.Weapon)
                {
                    BuyWeapon(player, name, biz, JsonConvert.DeserializeObject<List<int>>(componentsString), payCard);
                }
                else if (name == ItemNames.BodyArmor)
                {
                    BuyArmor(player, name, biz, payCard);
                }
                else if (type == ItemTypes.Ammo)
                {
                    BuyAmmo(player, name, biz, count, payCard);
               }                
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }
            
        }

        private int GetComponentPriceBySlot(int slotId, int weaponPrice)
        {
            if (slotId < 0) return 0;
            var slot = (WeaponComponentSlots)slotId;
            switch (slot)
            {
                case WeaponComponentSlots.Muzzle: return (int)(.3 * weaponPrice);
                case WeaponComponentSlots.FlashLight: return (int)(.1 * weaponPrice);
                case WeaponComponentSlots.Clip: return (int)(.2 * weaponPrice);
                case WeaponComponentSlots.Scope: return (int)(.5 * weaponPrice);
                case WeaponComponentSlots.Grip: return (int)(.1 * weaponPrice);
                case WeaponComponentSlots.Skin: return 5 * weaponPrice;
                default: return 0;
            }
            //if (!config.Components.ContainsKey(slot)) return 0;
            //var components = config.Components[slot];
            //if (components.Count < cIndex)
            //{
            //    return components[cIndex].Price;
            //} else return 0;
        }

        private void BuyWeapon(Player player, ItemNames name, Business biz, List<int> components, bool payCard)
        {
            var prod = biz.Products.FirstOrDefault(p => p.Name == name.ToString());
            if (prod == null) return;

            var price = prod.Price;
            var componentPrice = 0;
            var config = WeaponConfigs.Config[name];

            for (int i = 0; i < components.Count; i++)
            {
                var index = components[i];
                if (index < 0) continue;
                //var conf = config.Components[(WeaponComponentSlots)i+1];
                componentPrice += GetComponentPriceBySlot(i + 1, price);
            }

            if (!Wallet.TryChange(player.GetMoneyPayment(payCard ? PaymentsType.Card : PaymentsType.Cash), -price - componentPrice))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_1", 3000);
                return;
            }

            if (BusinessManager.TakeProd(player, biz, player.GetMoneyPayment(payCard ? PaymentsType.Card : PaymentsType.Cash), 
                new BuyModel(prod.Name, 1, true, 
                (cnt) => 
                {
                    var item = ItemsFabric.CreateWeapon(name, components, false);
                    if (player.GetInventory().AddItem(item))
                        return cnt;
                    return 0;
                }), 
                "Money_BuyWShop".Translate(name), "Biz_116"))
            {
                Wallet.MoneySub(player.GetMoneyPayment(payCard ? PaymentsType.Card : PaymentsType.Cash), componentPrice, "Money_BuyWShopCompon");
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "wshop_success".Translate(config.DisplayName), 3000);
            }
        }


        private void BuyAmmo(Player player, ItemNames name, Business biz, int count, bool payCard)
        {
            var config = AmmoConfigs.Config[name];
            if (BusinessManager.TakeProd(player, biz, player.GetMoneyPayment(payCard ? PaymentsType.Card : PaymentsType.Cash), new BuyModel(name.ToString(), count, false,
                (cnt) =>
                {
                    var item = ItemsFabric.CreateAmmo(name, cnt, false);
                    if (player.GetInventory().AddItem(item))
                        return cnt;
                    return 0;
                }), "Money_BuyWShop".Translate(name), "Biz_116"))
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "wshop_success".Translate(config.DisplayName), 3000);
        }

        private void BuyArmor(Player player, ItemNames name, Business biz, bool payCard)
        {
            var config = ClothesConfigs.Config[name];
            if (BusinessManager.TakeProd(player, biz, player.GetMoneyPayment(payCard ? PaymentsType.Card : PaymentsType.Cash), new BuyModel(name.ToString(), 1, false,
                (cnt) =>
                {
                    var item = ItemsFabric.CreateClothes(name, true, 0, 0, false);
                    if (player.GetInventory().AddItem(item))
                        return cnt;
                    return 0;
                }), "Money_BuyWShop".Translate(name), "Biz_116"))
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "wshop_success".Translate(config.DisplayName), 3000);
        }

    }
}
