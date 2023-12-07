using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.NewDonateShop.Configs;
using Whistler.NewDonateShop.Enums;
using Whistler.NewDonateShop.Models;
using Whistler.SDK;

namespace Whistler.NewDonateShop
{
    class RouletteGame
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(RouletteGame));
        private static Dictionary<int, RouletteModel> _configs;
        private Random _rand = new Random();
        private int _bank = 5000;
        private RarityManager _rarityManager;
        private int _id;
        private RouletteModel _config;
        private int _total_game = 0;
        private long _total_spend = 0;
        private long _total_drop = 0;
        private int _minAdminLevel = 8;
        private bool _changed = false;
        private RouletteGame(int id){
            _id = id;
            _config = _configs[_id];
            var result = MySQL.QueryRead("SELECT `bank`, `total_game`, `total_spend`, `total_drop`, `rarity_data` FROM `donate_roulettes` WHERE `rouletteid`=@prop0", _id);
            if (result == null || result.Rows.Count == 0)
            {
                _rarityManager = new RarityManager();
                _rarityManager.SetChance(ItemRarities.Epic, 10);
                _rarityManager.SetChance(ItemRarities.Legend, 70);
                _rarityManager.SetChance(ItemRarities.Hight, 150);
                _rarityManager.SetChance(ItemRarities.Medium, 250);
                _rarityManager.SetChance(ItemRarities.Low, 400);
                _rarityManager.SetChance(ItemRarities.Base, 600, true);
                MySQL.Query("INSERT INTO `donate_roulettes`(`rouletteid`, `rarity_data`, `bank`, `total_game`, `total_spend`, `total_drop`) VALUES (@prop0, @prop1, @prop2, 0, 0, 0)", _id, _rarityManager.GetChanceData(), _bank);
            }else{
                var row = result.Rows[0];
                _bank = Convert.ToInt32(row["bank"]);
                _total_game = Convert.ToInt32(row["total_game"]);
                _total_spend = Convert.ToUInt32(row["total_spend"]);
                _total_drop = Convert.ToUInt32(row["total_drop"]);
                _rarityManager = new RarityManager(row["rarity_data"].ToString());
            }
            _config.ItemLinks = DonateService.Items.GetItemsByPriceRange(_config.MinPrice, _config.MaxPrice);
            _config.Items = _config.ItemLinks.Select(i=>i.Id).ToList();
        }

        public static Dictionary<int, RouletteGame> Init()
        {            
            _configs = new RouletteConfig().Config;
            var games = new Dictionary<int, RouletteGame>();
            foreach (var item in _configs)
            {
                games.Add(item.Key, new RouletteGame(item.Key));
            }
            return games;
        }

        public void SetChances(int chanceBase, int chanceLow, int chanceMedium, int chanceHight, int chanceLegend, int chanceEpic)
        {
            _rarityManager.SetChance(ItemRarities.Base, chanceBase);
            _rarityManager.SetChance(ItemRarities.Low, chanceLow);
            _rarityManager.SetChance(ItemRarities.Medium, chanceMedium);
            _rarityManager.SetChance(ItemRarities.Hight, chanceHight);
            _rarityManager.SetChance(ItemRarities.Legend, chanceLegend);
            _rarityManager.SetChance(ItemRarities.Epic, chanceEpic, true); 
            MySQL.Query("UPDATE `donate_roulettes` SET `rarity_data`=@prop0 WHERE `rouletteid`=@prop1", _rarityManager.GetChanceData(), _id);
        }

        public void CalculateWinResult(ExtPlayer player)
        {
            ItemModel result = null;
            var character = player.Character;
            var gender = character.Customization.Gender;
            if (character.AdminLVL < _minAdminLevel)
            {
                if (_config.ForCoins)
                {
                    var account = player.Account;
                    if (account.MCoins >= _config.Price)
                    {
                        _total_game += 1;
                        _total_spend += _config.Price;
                        player.SubMCoins(_config.Price);
                        _bank += _config.Price;
                        result = GetRandomItem(player, gender);
                        _bank -= RouletteConfig.RarityPrice[result.Rarity].Price;
                        _total_drop += result.Price;
                        _changed = true;
                    }
                    else
                        player.UpdateCoins();
                }
                else
                {
                    if (character.Money >= _config.Price)
                    {
                        _total_game += 1;
                        _total_spend += _config.Price;
                        MoneySystem.Wallet.MoneySub(player.Character, _config.Price, "Money_Roulette");
                        _bank += _config.Price;
                        result = GetRandomItem(player, gender);
                        _bank -= RouletteConfig.RarityPrice[result.Rarity].Price;
                        _total_drop += result.Price;
                        _changed = true;
                    }
                }
            }
            else
            {
                result = GetRandomItem(player, gender);
            }
            if (result == null) return;
            if(result.Data is ComplectDonateItems)
            {
                foreach (var id in (result.Data as ComplectDonateItems).Items)
                    character.DonateInventory.AddItem(id, _config.ForCoins, false);
            }else if(result.Data is ComplectGenderDonateItem)
            {
                foreach (var id in (result.Data as ComplectGenderDonateItem).Items)
                    character.DonateInventory.AddItem(id, _config.ForCoins, false);
            }
            else
            {
                character.DonateInventory.AddItem(result.Id, _config.ForCoins, false);
            }

            SafeTrigger.ClientEvent(player,"dshop:roulette:result", result.Id);
            DonateLog.DonateItemlog(player, result, "win");
            if (result.Rarity > ItemRarities.Hight)
                NAPI.ClientEvent.TriggerClientEventForAll("dshop:roulette:notify", player.Name, result.Id);
        }

        private ItemModel GetRandomItem(ExtPlayer player, bool gender)
        {
            ItemRarities needRarity;
            if (player.HasData("DONATEROULETTE:NEXTRARITY"))
            {
                needRarity = player.GetData<ItemRarities>("DONATEROULETTE:NEXTRARITY");
                player.ResetData("DONATEROULETTE:NEXTRARITY");
            }else
                needRarity = _rarityManager.GetRandom();
            return GetItemByRarityAndGender(needRarity, gender);
        }

        private ItemModel GetItemByRarityAndGender(ItemRarities needRarity, bool gender)
        {
            while (!_config.ItemLinks.Any(i=>i.Rarity == needRarity) && needRarity != ItemRarities.Base)
            {
               needRarity--;
            }
            var items = _config.ItemLinks.Where(i => i.Rarity == needRarity && (!(i.Data is ClothesDonateItem) && !(i.Data is CostumeDonateItem) && !(i.Data is ComplectGenderDonateItem) || i.Data.Gender == gender)).ToList();
            if (items.Count == 0)
                return DonateService.Items[2002];
            return items[_rand.Next(items.Count)];
        }

        public static void ParseConfigs()
        {
            using var r1 = new StreamWriter("interfaces/gui/src/configs/newDonateShop/rarityPrices.js");
            r1.Write($"export default {JsonConvert.SerializeObject(RouletteConfig.RarityPrice.ToDictionary(i=>(int)i.Key, i => i.Value))}");
            using var r2 = new StreamWriter("interfaces/gui/src/configs/newDonateShop/roulette.js");
            r2.Write($"export default {JsonConvert.SerializeObject(_configs)}");
        }

        public void TestRandomItems(ExtPlayer player, int iterations)
        {
            var result = new Dictionary<ItemRarities, int>();
            var bank = _bank;
            _bank = 0;
            for (int i = 0; i < iterations; i++)
            {
                _bank += _config.Price;
                var r = GetRandomItem(player, true);
                _bank -= RouletteConfig.RarityPrice[r.Rarity].Price;
                if (result.ContainsKey(r.Rarity)) result[r.Rarity]++;
                else result.Add(r.Rarity, 1);
            }

            _logger.WriteInfo($"TestRandomRarity {_bank}");
            Chat.SendTo(player, $"TestRandomRarity {_bank}");
            foreach (var item in Enum.GetNames(typeof(ItemRarities)))
            {
                var rarity = (ItemRarities)Enum.Parse(typeof(ItemRarities), item);
                if (result.ContainsKey(rarity))
                {
                    Chat.SendTo(player, $"{item}:{result[rarity]}");

                    _logger.WriteInfo($"{item}:{result[rarity]}");
                }
            }
            _bank = bank;
        }

        public void Save()
        {
            if (_changed)
            {
                MySQL.Query("UPDATE `donate_roulettes` SET `bank`=@prop0, `total_game`=@prop1, `total_spend`=@prop2, `total_drop`=@prop3 WHERE `rouletteid`=@prop4", _bank, _total_game, _total_spend, _total_drop, _id);
                _changed = false;
            }
        }
    }
}
