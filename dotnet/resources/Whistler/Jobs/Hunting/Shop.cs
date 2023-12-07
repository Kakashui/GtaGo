using GTANetworkAPI;
using Whistler.Core;
using Whistler.GUI;
using Whistler.MoneySystem;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Helpers;
using Whistler.Entities;

namespace Whistler.Jobs.Hunting
{
    public class Shop
    {
        private const int HunterRiflePrice = 300000;
        private const int HunterKnifePrice = 500000;
        private const int HunterAmmoPrice = 30;    
        private const int HunterAmmoAmount = 10;

        private const int AnimalSkinSellPrice = 1000;

        private Vector3 Position;

        public Shop(Vector3 position)
        {
            Position = position;

            InteractShape.Create(position, 2.5f, 2)
                .AddInteraction(OpenShopMenu);

            NAPI.Blip.CreateBlip(141, position, 1, 73, "Магазин охотника", shortRange: true);

            ShopEvents.PlayerSelectItem += HandlePlayerSelectItem;
        }

        private void HandlePlayerSelectItem(ExtPlayer player, string itemKey)
        {
            if (player.Position.DistanceTo(Position) > 5)
            {
                return;
            }

            switch (itemKey)
            {
                case "huntingrifle":
                    BuyHuntingRifle(player);
                    return;
                case "huntingAmmo":
                    BuyHuntingAmmo(player);
                    return;
                case "huntingKnife":
                    BuyHuntingKnife(player);
                    return;
                case "skin":
                    SellSkins(player);
                    return;
            }
        }

        private void SellSkins(ExtPlayer player)
        {
            var item = player.GetInventory().GetItemLink(ItemNames.AnimalSkin);
            if (item == null) return;
            item = player.GetInventory().SubItem(item.Index, item.Count);
            if (item.Count == 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "hunting_9", 3000);
                return;
            }

            Wallet.MoneyAdd(player.Character, item.Count * AnimalSkinSellPrice, "Продажа шкур животных");
            Notify.Send(player, NotifyType.Success, NotifyPosition.Bottom, "hunting_10", 3000);

            OpenShopMenu(player);
        }

        private void BuyHuntingKnife(ExtPlayer player)
        {
            if (!player.CheckLic(GUI.Documents.Enums.LicenseName.Weapon))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "Biz_115", 3000);
                return;
            }

            if (player.Character.Money < HunterKnifePrice)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "Core_178", 3000);
                return;
            }
            if (!player.GetInventory().AddItem(ItemsFabric.CreateWeapon(ItemNames.Knife, false)))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "Core_180", 3000);
                return;
            }

            Wallet.MoneySub(player.Character, HunterKnifePrice, "Money_BuyHunterKnife");
            Notify.Send(player, NotifyType.Success, NotifyPosition.Bottom, "hunting_7", 3000);
        }

        private void BuyHuntingAmmo(ExtPlayer player)
        {
            if (player.Character.Money < HunterAmmoPrice * HunterAmmoAmount)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "Core_178", 3000);
                return;
            }

            if (!player.GetInventory().AddItem(ItemsFabric.CreateAmmo(ItemNames.MusketAmmo, HunterAmmoAmount, false)))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "Core_180", 3000);
                return;
            }

            Wallet.MoneySub(player.Character, HunterAmmoPrice * HunterAmmoAmount, "Money_BuyHunterAmmo");
            Notify.Send(player, NotifyType.Success, NotifyPosition.Bottom, "hunting_8", 3000);
        }

        private void BuyHuntingRifle(ExtPlayer player)
        {
            if (player.Character.Money < HunterRiflePrice)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "Core_178", 3000);
                return;
            }

            if (!player.GetInventory().AddItem(ItemsFabric.CreateWeapon(ItemNames.Musket, false)))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "Core_180", 3000);
                return;
            }

            Wallet.MoneySub(player.Character, HunterRiflePrice, "Money_BuyHunterRifle");
            Notify.Send(player, NotifyType.Success, NotifyPosition.Bottom, "hunting_6", 3000);
        }

        private void OpenShopMenu(ExtPlayer player)
        {
            var skinsCount = player.GetInventory().Items.Where(item => item.Name == ItemNames.AnimalSkin).Sum(item => item.Count);
            var skinsPrice = skinsCount * AnimalSkinSellPrice;

            var dtos = new List<ShopDto>
            {
                new ShopDto { Key = "huntingrifle", Price = HunterRiflePrice },
                new ShopDto { Key = "huntingAmmo", Price = HunterAmmoPrice * HunterAmmoAmount, Amount = HunterAmmoAmount },
                new ShopDto { Key = "huntingKnife", Price = HunterKnifePrice },
                new ShopDto { Key = "skin", Price = skinsPrice, Amount = skinsCount }
            };

            SafeTrigger.ClientEvent(player,"huntingStore:open", JsonConvert.SerializeObject(dtos));
        }

        private class ShopDto
        {
            [JsonProperty("key")]
            public string Key { get; set; }

            [JsonProperty("price")]
            public int Price { get; set; }

            [JsonProperty("amount")]
            public int? Amount { get; set; }
        }
    }

    public class ShopEvents : Script
    {
        public static event Action<ExtPlayer, string> PlayerSelectItem;
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ShopEvents));

        [RemoteEvent("huntingStore:select")]
        public void HandlePlayerSelectItem(ExtPlayer player, string itemKey)
        {
            try
            {
                PlayerSelectItem?.Invoke(player, itemKey);
            }
            catch (Exception e) { _logger.WriteError("Unhandled exception catched on huntingStore:select - " + e.ToString()); }
        }
    }
}