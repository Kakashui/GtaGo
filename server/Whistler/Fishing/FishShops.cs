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
        public FishShops(List<Trader> traders) {
            if (Directory.Exists("interfaces"))
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
        private static float _priceCoef = 9;

        protected List<ShopItem> _shopItems { get; } = new List<ShopItem>
        {
            new ShopItem(Fish.Herring, "Fish_33", 0, (int)(120 * _priceCoef)),
            new ShopItem(Fish.Bass, "Fish_34", 0, (int)(145 * _priceCoef)),
            new ShopItem(Fish.Eel, "Fish_35", 0, (int)(170 * _priceCoef)),
            new ShopItem(Fish.Pike, "Fish_36", 0, (int)(195 * _priceCoef)),
            new ShopItem(Fish.Sterlet, "Fish_37", 0, (int)(220 * _priceCoef)),
            new ShopItem(Fish.Salmon, "Fish_38", 0, (int)(245 * _priceCoef)),
            new ShopItem(Fish.Sturgeon, "Fish_39", 0, (int)(270 * _priceCoef)),
            new ShopItem(Fish.Amur, "Fish_40", 0, (int)(295 * _priceCoef)),
            new ShopItem(Fish.Stingray, "Fish_41", 0, (int)(320 * _priceCoef)),
            new ShopItem(Fish.Tuna, "Fish_42", 0, (int)(345 * _priceCoef)),
            new ShopItem(Fish.Trout, "Fish_43", 0, (int)(370 * _priceCoef)),

            new ShopItem(Fish.PerfectHerring, "Fish_44", 0, (int)(395 * _priceCoef)),
            new ShopItem(Fish.PerfectBass, "Fish_45", 0,(int)( 420 * _priceCoef)),
            new ShopItem(Fish.PerfectEel, "Fish_46", 0, (int)(445 * _priceCoef)),
            new ShopItem(Fish.PerfectPike, "Fish_47", 0, (int)(470 * _priceCoef)),
            new ShopItem(Fish.PerfectSterlet, "Fish_48", 0, (int)(495 * _priceCoef)),
            new ShopItem(Fish.PerfectSalmon, "Fish_49", 0, (int)(520 * _priceCoef)),
            new ShopItem(Fish.PerfectSturgeon, "Fish_50", 0, (int)(545 * _priceCoef)),
            new ShopItem(Fish.PerfectAmur, "Fish_51", 0, (int)(570 * _priceCoef)),
            new ShopItem(Fish.PerfectStingray, "Fish_52", 0, (int)(595 * _priceCoef)),
            new ShopItem(Fish.PerfectTuna, "Fish_53", 0, (int)(620 * _priceCoef)),
            new ShopItem(Fish.PerfectTrout, "Fish_54", 0, (int)(645 * _priceCoef)),

            new ShopItem(Fish.GoldFish, "Fish_55", 0, (int)(700 * _priceCoef)),
        };     

        private void OpenShop(PlayerGo player, QuestPed ped)
        {
            try
            {
                DialogPage startPage;
                var inventory = player.GetInventory();
                var cage = inventory.Items.GetNotFreeCage();
                if (cage == null || cage.Fishings.Count < 1)
                {
                    startPage = new DialogPage("Fish_20", ped.Name, ped.Role)
                           .AddCloseAnswer("Fish_20_1");
                }
                else
                {
                    startPage = new DialogPage("Fish_21", ped.Name, ped.Role)
                        .AddAnswer("Fish_21_1", p => p.TriggerEvent(Const.CLIENT_EVENT_SHOW_FISH_SHOP, cage.Fishings))
                        .AddCloseAnswer("Fish_21_2");
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

        internal void CellFish(Player client, int key)
        {
            try
            {
                var count = 0;
                var inventory = client.GetInventory();
                var cage = inventory.Items.GetCage();
                if (cage == null)
                {
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Fish_22".Translate(), 3000);
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
                        Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Fish_23".Translate(), 3000);
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
                    client.TriggerEvent(Const.CLIENT_EVENT_UPDATE_CAGE, cage.Fishings);

                    Wallet.MoneyAdd(client.GetCharacter(), price, "Money_CellFish");
                    Notify.Send(client, NotifyType.Success, NotifyPosition.BottomCenter, "Fish_24".Translate(count, price), 3000);
                }
            }
            catch (Exception e) { _logger.WriteError("CellFish: " + e.ToString()); }
        }
    }
}
