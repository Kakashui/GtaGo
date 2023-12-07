using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.NewDonateShop.Configs;
using Whistler.NewDonateShop.Enums;
using Whistler.SDK;
using Newtonsoft.Json;
using Whistler.NewDonateShop.Models;
using Whistler.Entities;

namespace Whistler.NewDonateShop
{
    class DonateEvents: Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(DonateEvents));
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            var query = $"CREATE TABLE IF NOT EXISTS `donate_roulettes`(" +
                  $"`id` int(11) NOT NULL AUTO_INCREMENT," +
                  $"`rouletteid` int(11) NOT NULL," +
                  $"`bank` int(11) NOT NULL," +
                  $"`total_game` INT(11) NOT NULL," +
                  $"`total_spend` BIGINT NOT NULL," +
                  $"`total_drop` BIGINT NOT NULL," +
                  $"`rarity_data` TEXT NOT NULL," +
                  $"PRIMARY KEY(`id`)" +
                  $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
            MySQL.QuerySync(query);

            query = $"CREATE TABLE IF NOT EXISTS `donate_inventories`(" +
                $"`id` int(11) NOT NULL AUTO_INCREMENT," +
                $"`items` TEXT NOT NULL," +
                $"PRIMARY KEY(`id`)" +
                $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
            MySQL.QuerySync(query);
            query = $"CREATE TABLE IF NOT EXISTS `donate_items_log`(" +
               $"`id` int(11) NOT NULL AUTO_INCREMENT," +
               $"`login` TEXT NOT NULL," +
               $"`uuid` int(11) NOT NULL," +
               $"`itemid` int(11) NOT NULL," +
               $"`itemname` TEXT NOT NULL," +
               $"`sum` int(11) NOT NULL," +
               $"`operation` TEXT NOT NULL," +
               $"`date` DATETIME NOT NULL," +
               $"PRIMARY KEY(`id`)" +
               $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
            MySQL.QuerySync(query);
            DonateService.LoadConfig();
            DonateService.ParseConfigs();
            //DonateService.RouletteGames[0].TestRandomItems(10000);
        }

        [ServerEvent(Event.PlayerConnected)]
        public static void OnPlayerConnected(PlayerGo player)
        {
            DonateService.Items.SetUpdatedPrices(player);
        }

        [RemoteEvent("dshop:roulette:start")]
        public void OnRouletteStart(PlayerGo player, int type)
        {
            if (!player.IsLogged()) return;
            DonateService.RouletteGames[type].CalculateWinResult(player);
        }

        [RemoteEvent("dshop:coins:request")]
        public void OnCoinsRequest(PlayerGo player)
        {
            if (!player.IsLogged()) return;
            player.UpdateCoins();
        }

        [RemoteEvent("dshop:coins:exch:money")]
        public void OnExchangeCoinsToMoney(PlayerGo player, int amount)
        {
            if (!player.IsLogged()) return;
            DonateService.Wallet.ExchangeCoinsToMoney(player, amount);
        }

        [RemoteEvent("dshop:coins:buy:single")]
        public void OnBuyCoinsSingle(PlayerGo player, int amount)
        {
            if (!player.IsLogged()) return;
            DonateService.Wallet.OrderCoins(player, amount);
        }

        [RemoteEvent("dshop:coins:kit:buy")]
        public void OnBuyCoinsKit(PlayerGo player, int id)
        {
            if (!player.IsLogged()) return;
            DonateService.Wallet.OrderCoinKit(player, id);
        }

        [RemoteEvent("dshop:prime:buy")]
        public void OnPrimeAccountBuy(PlayerGo player)
        {
            if (!player.IsLogged()) return;
            DonateService.Shop.BuyPrimeAccount(player);
        }

        [RemoteEvent("dshop:shop:buy")]
        public void OnBuyShopItem(PlayerGo player, int id)
        {
            if (!player.IsLogged()) return;
            DonateService.Shop.BuyItem(player, id);
        }

        [RemoteEvent("dshop:inventory:item:use")]
        public void OnUseItemFromDonateInventory(PlayerGo player, int id, bool sell, int count)
        {
            if (!player.IsLogged()) return;
            var character = player.GetCharacter();
            character.DonateInventory.UseItem(player, id, sell, count);
        }

        [RemoteEvent("dshop:inventory:item:sell")]
        public void OnSellItemFromDonateInventory(PlayerGo player, int id, bool sell)
        {
            if (!player.IsLogged() || !sell) return;
            var character = player.GetCharacter();
            character.DonateInventory.SellItem(player, id, sell);
        }


        [RemoteEvent("dshop:item:take")]
        public void GetDonateItem(PlayerGo player, int itemId, bool sell)
        {
            if (!player.IsLogged()) return;
            if (!Group.CanUseAdminCommand(player, "takedoanteitem")) return;
            if(!player.HasData("takeDonate"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:take:error", 3000);
                return;
            }
            Player target = player.GetData<Player>("takeDonate");
            if (!target.IsLogged())
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:take:error", 3000);
                return;
            }
            var inventory = target.GetCharacter().DonateInventory;
            if(inventory.RemoveItem(itemId, sell))
            {
                Notify.Send(target, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:take:ok1".Translate(player.Name, DonateService.Items[itemId].Name), 3000);
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:take:ok2".Translate(target.Name, DonateService.Items[itemId].Name), 3000);
                GameLog.Admin(player.Name, $"takeDonateItem {DonateService.Items[itemId].Name}", target.Name);
            }
        }

        [Command("testroulette")]
        public void TestRouletteRandom(PlayerGo player, int idRoulette, int iterations)
        {
            if (!Group.CanUseAdminCommand(player, "testroulette")) return;
            if (!DonateService.RouletteGames.ContainsKey(idRoulette)) return;
            DonateService.RouletteGames[idRoulette].TestRandomItems(player, iterations);
        }

        [Command("showminmaxdonateprices")]
        public void ShowMinMaxRow(PlayerGo player)
        {
            if (!Group.CanUseAdminCommand(player, "showminmaxdonateprices")) return;
            var sorted = DonateService.Items.SortedByPrice;
            foreach (var item in sorted)
            {
                Console.WriteLine($"{item.Name} : {item.Price}({item.Rarity})");
            }
            Chat.SendTo(player, $"item prices from {sorted[0].Price} to {sorted[sorted.Count - 1].Price}");
        }

        [Command("takedonateitem")]
        public void TakeDonateItemFromInventory(PlayerGo player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "takedoanteitem")) return;
            var target = Main.GetPlayerByID(id);
            if (target == null || !target.IsLogged() || !player.IsLogged()) return;
            var itemData = target.GetCharacter().DonateInventory.GetItemData();
            player.SetData("takeDonate", target);
            player.TriggerEvent("dshop:take:item", itemData);
        }

        [Command("chanceroulette")]
        public void SetRouletteChance(PlayerGo player, int id, int baseChance, int lowChance, int mediumChance, int hightChance, int legendChance, int epicChance)
        {
            if (!Group.CanUseAdminCommand(player, "chanceroulette")) return;
            if (!player.IsLogged() || !DonateService.RouletteGames.ContainsKey(id)) return;
            DonateService.RouletteGames[id].SetChances(baseChance, lowChance, mediumChance, hightChance, legendChance, epicChance);
        }

        [Command("givedonateitem")]
        public void GiveDonateItem(PlayerGo player, int idItem)
        {
            if (!Group.CanUseAdminCommand(player, "givedonateitem")) return;
            if (!player.IsLogged()) return;
            var character = player.GetCharacter();
            
            var item = DonateService.Items[idItem];
            if (item == null) return;
            if (item.Data is ComplectDonateItems)
            {
                foreach (var id in (item.Data as ComplectDonateItems).Items)
                    character.DonateInventory.AddItem(id, true, true);
            }
            else if (item.Data is ComplectGenderDonateItem)
            {
                foreach (var id in (item.Data as ComplectGenderDonateItem).Items)
                    character.DonateInventory.AddItem(id, true, true);
            }
            else
            {
                character.DonateInventory.AddItem(item.Id, true, true);
            }

        }

        [Command("setnextdroprarity")]
        public static void Command_SetNextDropRarityForPlayer(PlayerGo player, int id, int rarity)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "setnextdroprarity")) return;

                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_1", 3000);
                    return;
                }

                var target = Main.GetPlayerByID(id);
                var dropRarity = (ItemRarities)rarity;

                target.SetData("DONATEROULETTE:NEXTRARITY", dropRarity);

                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "local_99".Translate(target.Name, dropRarity.ToString()), 3000);
                GameLog.Admin(player.Name, $"setnextdroprarity({rarity})", target.Name);
            }
            catch (Exception e) { _logger.WriteError($"Command_SetNextDropRarityForPlayer:\n{e}"); }
        }
    }
}
