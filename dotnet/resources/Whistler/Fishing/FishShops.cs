using GTANetworkAPI;
using Whistler.Core;
using Whistler.Fishing.Extensions;
using Whistler.Fishing.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Inventory;
using Whistler.MoneySystem;
using System.IO;
using Whistler.Core.QuestPeds;
using Whistler.Helpers;
using Whistler.Entities;

namespace Whistler.Fishing
{
    class FishShops
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(FishShops));
        private const float _priceCoef = 1.0f;

        public FishShops(List<Trader> traders) 
        {
            if (Directory.Exists("interfaces/gui/src/configs/fishing"))
            {
                ParseConfig();
            }
            foreach (var trader in traders)
            {
                var blip = NAPI.Blip.CreateBlip(trader.BlipSprite, trader.Position, 1, trader.BlipColor, trader.BlipName, 255, 0, true);
                var ped = new QuestPed(trader.Hash, trader.Position, trader.Name, trader.Role, trader.Rotation.Z, 0, 2);
                ped.PlayerInteracted += OpenShop;
            }
        }

        protected List<ShopItem> _shopItems { get; } = new List<ShopItem>
        {
            new ShopItem(Fish.Herring, "Сельдь", 0, (int)(60 * _priceCoef)),
            new ShopItem(Fish.Bass, "Окунь", 0, (int)(70 * _priceCoef)),
            new ShopItem(Fish.Eel, "Угорь", 0, (int)(85 * _priceCoef)),
            new ShopItem(Fish.Pike, "Щука", 0, (int)(100 * _priceCoef)),
            new ShopItem(Fish.Sterlet, "Стерлядь", 0, (int)(110 * _priceCoef)),
            new ShopItem(Fish.Salmon, "Лосось", 0, (int)(125 * _priceCoef)),
            new ShopItem(Fish.Sturgeon, "Осетр", 0, (int)(135 * _priceCoef)),
            new ShopItem(Fish.Amur, "Амур", 0, (int)(150 * _priceCoef)),
            new ShopItem(Fish.Stingray, "Скат", 0, (int)(160 * _priceCoef)),
            new ShopItem(Fish.Tuna, "Тунец", 0, (int)(175 * _priceCoef)),
            new ShopItem(Fish.Trout, "Форель", 0, (int)(185 * _priceCoef)),

            new ShopItem(Fish.PerfectHerring, "Сельдь элитная", 0, (int)(200 * _priceCoef)),
            new ShopItem(Fish.PerfectBass, "Окунь элитный", 0,(int)(210 * _priceCoef)),
            new ShopItem(Fish.PerfectEel, "Угорь элитный", 0, (int)(225 * _priceCoef)),
            new ShopItem(Fish.PerfectPike, "Щука элитная", 0, (int)(235 * _priceCoef)),
            new ShopItem(Fish.PerfectSterlet, "Стерлядь элитная", 0, (int)(250 * _priceCoef)),
            new ShopItem(Fish.PerfectSalmon, "Лосось элитный", 0, (int)(260 * _priceCoef)),
            new ShopItem(Fish.PerfectSturgeon, "Осетр элитный", 0, (int)(270 * _priceCoef)),
            new ShopItem(Fish.PerfectAmur, "Амур элитный", 0, (int)(285 * _priceCoef)),
            new ShopItem(Fish.PerfectStingray, "Скат элитный", 0, (int)(300 * _priceCoef)),
            new ShopItem(Fish.PerfectTuna, "Тунец элитный", 0, (int)(310 * _priceCoef)),
            new ShopItem(Fish.PerfectTrout, "Форель элитная", 0, (int)(325 * _priceCoef)),

            new ShopItem(Fish.GoldFish, "Золотая рыбка", 0, (int)(350 * _priceCoef)),
        };     

        private void OpenShop(ExtPlayer player, QuestPed ped)
        {
            try
            {
                DialogPage startPage;
                var inventory = player.GetInventory();
                var cage = inventory.Items.GetNotFreeCage();
                if (cage == null || !cage.Fishings.Any())
                {
                    startPage = new DialogPage("Дружище, у меня очень много дел. Возвращайся когда у тебя будет рыба.", ped.Name, ped.Role)
                           .AddCloseAnswer("Я понял. Скоро вернусь с рыбой.");
                }
                else
                {
                    startPage = new DialogPage("Показывай что словил, я готов купить весь твой улов.", ped.Name, ped.Role)
                        .AddAnswer("Я хочу продать тебе немного рыбы", p => SafeTrigger.ClientEvent(p, Const.CLIENT_EVENT_SHOW_FISH_SHOP, cage.Fishings))
                        .AddCloseAnswer("Я передумал, загляну как-нибудь позже");
                }              
                startPage.OpenForPlayer(player);
            }
            catch (Exception e) { _logger.WriteError("Open: " + e.ToString()); }
        }

        private void ParseConfig()
        {
            using (var w = new StreamWriter("interfaces/gui/src/configs/fishing/fishShop.js"))
            {
                w.Write($"export default {JsonConvert.SerializeObject(_shopItems, Formatting.Indented)}");
            }
        }

        internal string GetFishName(int fish)
        {
            return _shopItems[fish].Name;
        }

        internal int GetFishPrice(int key)
        {
            return _shopItems.First(i => i.Id == key).Price;
        }

        internal void CellFish(ExtPlayer client, int key)
        {
            try
            {
                var count = 0;
                var inventory = client.GetInventory();
                var cage = inventory.Items.GetCage();
                if (cage == null)
                {
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Ведро не найдено", 3000);
                    return;
                }
                var price = 0;
                if (key < 0)
                {
                    foreach (var item in cage.Fishings)
                    {
                        price += (GetFishPrice(item.Key) * item.Value);
                        count += item.Value;
                    }
                    cage.Fishings.Clear();
                }
                else
                {
                    if (!cage.Fishings.ContainsKey(key))
                    {
                        Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Такая рыба не найдена", 3000);
                        return;
                    }
                    count = cage.Fishings[key];
                    price = GetFishPrice(key) * count;
                    cage.Fishings.Remove(key);
                }
                if (price > 0)
                {
                    inventory.MarkAsChanged();
                    inventory.UpdateItemData(cage.Index);
                    SafeTrigger.ClientEvent(client, Const.CLIENT_EVENT_UPDATE_CAGE, cage.Fishings);

                    Wallet.MoneyAdd(client.Character, price, "Продажа рыбы");
                    Notify.Send(client, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы продали {count} киллограм рыбы за: {price}$", 3000);
                }
            }
            catch (Exception e) { _logger.WriteError("CellFish: " + e.ToString()); }
        }
    }
}
