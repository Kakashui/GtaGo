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
using Whistler.Entities;

namespace Whistler.Businesses
{
    public class WeaponShops : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(WeaponShops)); 
        private static Vector3 _playerPosition = new Vector3(254.787, -48.306, 71.151);
        public static void Open(ExtPlayer player, Business business)
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
            //player.Character.ExteriorPos = player.Position;
            SafeTrigger.UpdateDimension(player,  Dimensions.RequestPrivateDimension());
            //player.ChangePosition(_playerPosition);
            SafeTrigger.ClientEvent(player,"wshop:open", 
                prices,
                JsonConvert.SerializeObject(new { money = player.GetMoneyPayment(PaymentsType.Cash).IMoneyBalance, bank = player.GetMoneyPayment(PaymentsType.Card).IMoneyBalance })
            );
        }

        [RemoteEvent("wshop:close")]
        public void CloseShop(ExtPlayer player)
        {
            SafeTrigger.UpdateDimension(player,  0);
        }


        [RemoteEvent("wshop:buy:weapon")]
        public void Buy(ExtPlayer player, int id, string componentsString, int count, bool payCard)
        {
            try
            {
                GUI.Documents.Models.License license = player.Character.Licenses.FirstOrDefault(item => item.Name == GUI.Documents.Enums.LicenseName.Weapon);
                if (license == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вам нужно приобрести лицензию на оружие, чтобы покупать оружия в магазине.", 5000);
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

        private void BuyWeapon(ExtPlayer player, ItemNames name, Business biz, List<int> components, bool payCard)
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
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно денежных средств", 3000);
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
                $"Покупка оружия ({name})", "Недостаточно места в инвентаре"))
            {
                Wallet.MoneySub(player.GetMoneyPayment(payCard ? PaymentsType.Card : PaymentsType.Cash), componentPrice, "Покупка оружия");
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы купили {config.DisplayName}", 3000);
            }
        }


        private void BuyAmmo(ExtPlayer player, ItemNames name, Business biz, int count, bool payCard)
        {
            var config = AmmoConfigs.Config[name];
            if (BusinessManager.TakeProd(player, biz, player.GetMoneyPayment(payCard ? PaymentsType.Card : PaymentsType.Cash), new BuyModel(name.ToString(), count, false,
                (cnt) =>
                {
                    var item = ItemsFabric.CreateAmmo(name, cnt, false);
                    if (player.GetInventory().AddItem(item))
                        return cnt;
                    return 0;
                }), $"Покупка оружия ({name})", "Недостаточно места в инвентаре"))
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы купили {config.DisplayName}", 3000);
        }

        private void BuyArmor(ExtPlayer player, ItemNames name, Business biz, bool payCard)
        {
            var config = ClothesConfigs.Config[name];
            if (BusinessManager.TakeProd(player, biz, player.GetMoneyPayment(payCard ? PaymentsType.Card : PaymentsType.Cash), new BuyModel(name.ToString(), 1, false,
                (cnt) =>
                {
                    var item = ItemsFabric.CreateClothes(name, true, 0, 0, false);
                    if (player.GetInventory().AddItem(item))
                        return cnt;
                    return 0;
                }), $"Покупка оружия ({name})", "Недостаточно места в инвентаре"))
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы купили {config.DisplayName}", 3000);
        }

    }
}
