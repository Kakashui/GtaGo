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
using Whistler.Inventory.Enums;
using Whistler.Inventory;
using Whistler.VehicleSystem;
using Whistler.Houses;
using Whistler.GUI;
using Whistler.Common;
using Whistler.GUI.Documents.Enums;
using System.Linq;

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
        public static void OnPlayerConnected(ExtPlayer player)
        {
            DonateService.Items.SetUpdatedPrices(player);
        }

        [RemoteEvent("dshop:roulette:start")]
        public void OnRouletteStart(ExtPlayer player, int type)
        {
            if (!player.IsLogged()) return;
            DonateService.RouletteGames[type].CalculateWinResult(player);
        }

        [RemoteEvent("dshop:coins:request")]
        public void OnCoinsRequest(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            player.UpdateCoins();
        }

        [RemoteEvent("dshop:coins:exch:money")]
        public void OnExchangeCoinsToMoney(ExtPlayer player, int amount)
        {
            if (!player.IsLogged()) return;
            DonateService.Wallet.ExchangeCoinsToMoney(player, amount);
        }

        [RemoteEvent("dshop:coins:buy:single")]
        public void OnBuyCoinsSingle(ExtPlayer player, int amount)
        {
            if (!player.IsLogged()) return;
            DonateService.Wallet.OrderCoins(player, amount);
        }

        [RemoteEvent("dshop:coins:kit:buy")]
        public void OnBuyCoinsKit(ExtPlayer player, int id)
        {
            if (!player.IsLogged()) return;
            DonateService.Wallet.OrderCoinKit(player, id);
        }

        [RemoteEvent("donate::check")]
        public void DonateShow(ExtPlayer player, string type, int id)
        {
            if (!player.IsLogged()) return;
            if (type != "exclusive") return;
            if (id < 1 || id > 3) return;

            player.CreateWaypoint(new Vector3(-132.968, -596.68195, 37.926804));
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Мы установили Вам точку в GPS, где Вы можете посмотреть на этот набор.", 5000);
        }


        [RemoteEvent("donate::buy")]
        public void DonateBuy(ExtPlayer player, string type, int id)
        {
            if (!player.IsLogged()) return;

            if(type == "premium")
            {
                DonateService.Shop.BuyPrimeAccount(player);
                return;
            }
            if(type == "money")
            {
                Donate don = DonateService.DonateList.Find(d => d.Id == id);
                if (don == null) return;

                DonateService.Wallet.BuyMoneyPack(player, don.Count, don.Price);
                return;
            }
            if(type == "exclusive")
            {
                Donate exc = DonateService.DonateList.First(d => d.Type == 3);
                if(exc.Count >= exc.MaxCount)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Продажа эксклюзивных наборов приостановлена", 3000);
                    return;
                }

                int goCoins = player.Account.MCoins;
                if (goCoins < exc.Price)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "К сожалению, у Вас не хватает донат.валюты.", 3000);
                    return;
                }

                if (id == 1)
                {
                    if (!player.Character.Customization.Gender)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Мы очень сожалеем, но предметы из данного набора не подходят для женского персонажа.", 5000);
                        return;
                    }
                    // верх
                    ItemNames itemName = (ItemNames)13;
                    var item = ItemsFabric.CreateClothes(itemName, player.GetGender(), 500, 0, false);
                    // штаны
                    ItemNames itemName2 = (ItemNames)4;
                    var item2 = ItemsFabric.CreateClothes(itemName2, player.GetGender(), 547, 2, false);
                    // ботинки
                    ItemNames itemName3 = (ItemNames)8;
                    var item3 = ItemsFabric.CreateClothes(itemName3, player.GetGender(), 530, 0, false);

                    //CanAddItem
                    if (player.GetInventory().CanAddItem(item) && player.GetInventory().CanAddItem(item2) && player.GetInventory().CanAddItem(item3))
                    {

                        player.SubMCoins(exc.Price);

                        player.GetInventory().AddItem(item);
                        player.GetInventory().AddItem(item2);
                        player.GetInventory().AddItem(item3);

                        Color color = new Color(0, 0, 0);
                        var vehData = VehicleManager.Create(player.Character.UUID, "rs5", color, color, 100, 2000000, typeOwner: OwnerType.Personal);
                        GarageManager.SendVehicleIntoGarage(vehData);

                        MainMenu.SendProperty(player);
                        player.UpdateExclusive(exc);

                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Вы купили Exlusive Pack #1", 3000);
                    }
                    else Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Освободите больше места в инвентаре", 3000);
                }
                
                else if(id == 2)
                {
                    if (!player.Character.Customization.Gender)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Мы очень сожалеем, но предметы из данного набора не подходят для женского персонажа.", 5000);
                        return;
                    }
                    // верх
                    ItemNames itemName = (ItemNames)13;
                    var item = ItemsFabric.CreateClothes(itemName, player.GetGender(), 501, 0, false);
                    // штаны
                    ItemNames itemName2 = (ItemNames)4;
                    var item2 = ItemsFabric.CreateClothes(itemName2, player.GetGender(), 553, 0, false);
                    // ботинки
                    ItemNames itemName3 = (ItemNames)8;
                    var item3 = ItemsFabric.CreateClothes(itemName3, player.GetGender(), 531, 0, false);

                    //CanAddItem
                    if(player.GetInventory().CanAddItem(item) && player.GetInventory().CanAddItem(item2) && player.GetInventory().CanAddItem(item3)){

                        player.SubMCoins(exc.Price);

                        player.GetInventory().AddItem(item);
                        player.GetInventory().AddItem(item2);
                        player.GetInventory().AddItem(item3);

                        Color color = new Color(0, 0, 0);
                        var vehData = VehicleManager.Create(player.Character.UUID, "rs5", color, color, 100, 2000000, typeOwner: OwnerType.Personal);
                        GarageManager.SendVehicleIntoGarage(vehData);

                        MainMenu.SendProperty(player);
                        player.UpdateExclusive(exc);

                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Вы купили Exlusive Pack #2", 3000);
                    }
                    else Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Освободите больше места в инвентаре", 3000);
                }
                else if(id == 3)
                {
                    if (!player.Character.Customization.Gender)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Мы очень сожалеем, но предметы из данного набора не подходят для женского персонажа.", 5000);
                        return;
                    }
                    // верх
                    ItemNames itemName = (ItemNames)13;
                    var item = ItemsFabric.CreateClothes(itemName, player.GetGender(), 502, 0, false);
                    // штаны
                    ItemNames itemName2 = (ItemNames)4;
                    var item2 = ItemsFabric.CreateClothes(itemName2, player.GetGender(), 554, 0, false);
                    // ботинки
                    ItemNames itemName3 = (ItemNames)8;
                    var item3 = ItemsFabric.CreateClothes(itemName3, player.GetGender(), 532, 0, false);

                    //CanAddItem
                    if(player.GetInventory().CanAddItem(item) && player.GetInventory().CanAddItem(item2) && player.GetInventory().CanAddItem(item3)){

                        player.SubMCoins(exc.Price);

                        player.GetInventory().AddItem(item);
                        player.GetInventory().AddItem(item2);
                        player.GetInventory().AddItem(item3);

                        Color color = new Color(0, 0, 0);
                        var vehData = VehicleManager.Create(player.Character.UUID, "rs5", color, color, 100, 2000000, typeOwner: OwnerType.Personal);
                        GarageManager.SendVehicleIntoGarage(vehData);

                        MainMenu.SendProperty(player);
                        player.UpdateExclusive(exc);

                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Вы купили Exlusive Pack #3", 3000);
                    }
                    else Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Освободите больше места в инвентаре", 3000);
                }
                else
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Ошибка магазина #2 (обратитесь к администрации)", 3000);
                }
            }
            // Admin.giveTargetClothes(player, Trigger.GetPlayerByUuid(id), type, drawable, texture, false);
        }

        [RemoteEvent("dshop:shop:buy")]
        public void OnBuyShopItem(ExtPlayer player, int id)
        {
            if (!player.IsLogged()) return;
            DonateService.Shop.BuyItem(player, id);
        }

        [RemoteEvent("dshop:inventory:item:use")]
        public void OnUseItemFromDonateInventory(ExtPlayer player, int id, bool sell, int count)
        {
            if (!player.IsLogged()) return;
            var character = player.Character;
            character.DonateInventory.UseItem(player, id, sell, count);
        }

        [RemoteEvent("dshop:inventory:item:sell")]
        public void OnSellItemFromDonateInventory(ExtPlayer player, int id, bool sell)
        {
            if (!player.IsLogged() || !sell) return;
            var character = player.Character;
            character.DonateInventory.SellItem(player, id, sell);
        }


        [RemoteEvent("dshop:item:take")]
        public void GetDonateItem(ExtPlayer player, int itemId, bool sell)
        {
            if (!player.IsLogged()) return;
            if (!Group.CanUseAdminCommand(player, "takedoanteitem")) return;
            if(!player.HasData("takeDonate"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:take:error", 3000);
                return;
            }
            ExtPlayer target = player.GetData<ExtPlayer>("takeDonate");
            if (!target.IsLogged())
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:take:error", 3000);
                return;
            }
            var inventory = target.Character.DonateInventory;
            if(inventory.RemoveItem(itemId, sell))
            {
                Notify.Send(target, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:take:ok1".Translate(player.Name, DonateService.Items[itemId].Name), 3000);
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:take:ok2".Translate(target.Name, DonateService.Items[itemId].Name), 3000);
                GameLog.Admin(player.Name, $"takeDonateItem {DonateService.Items[itemId].Name}", target.Name);
            }
        }

        [Command("testroulette")]
        public void TestRouletteRandom(ExtPlayer player, int idRoulette, int iterations)
        {
            if (!Group.CanUseAdminCommand(player, "testroulette")) return;
            if (!DonateService.RouletteGames.ContainsKey(idRoulette)) return;
            DonateService.RouletteGames[idRoulette].TestRandomItems(player, iterations);
        }

        [Command("showminmaxdonateprices")]
        public void ShowMinMaxRow(ExtPlayer player)
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
        public void TakeDonateItemFromInventory(ExtPlayer player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "takedoanteitem")) return;
            var target = Trigger.GetPlayerByUuid(id);
            if (target == null || !target.IsLogged() || !player.IsLogged()) return;
            var itemData = target.Character.DonateInventory.GetItemData();
            SafeTrigger.SetData(player, "takeDonate", target);
            SafeTrigger.ClientEvent(player,"dshop:take:item", itemData);
        }

        [Command("chanceroulette")]
        public void SetRouletteChance(ExtPlayer player, int id, int baseChance, int lowChance, int mediumChance, int hightChance, int legendChance, int epicChance)
        {
            if (!Group.CanUseAdminCommand(player, "chanceroulette")) return;
            if (!player.IsLogged() || !DonateService.RouletteGames.ContainsKey(id)) return;
            DonateService.RouletteGames[id].SetChances(baseChance, lowChance, mediumChance, hightChance, legendChance, epicChance);
        }

        [Command("givedonateitem")]
        public void GiveDonateItem(ExtPlayer player, int idItem)
        {
            if (!Group.CanUseAdminCommand(player, "givedonateitem")) return;
            if (!player.IsLogged()) return;
            var character = player.Character;
            
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
        public static void Command_SetNextDropRarityForPlayer(ExtPlayer player, int id, int rarity)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "setnextdroprarity")) return;

                if (Trigger.GetPlayerByUuid(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_1", 3000);
                    return;
                }

                var target = Trigger.GetPlayerByUuid(id);
                var dropRarity = (ItemRarities)rarity;

                SafeTrigger.SetData(target, "DONATEROULETTE:NEXTRARITY", dropRarity);

                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "local_99".Translate(target.Name, dropRarity.ToString()), 3000);
                GameLog.Admin(player.Name, $"setnextdroprarity({rarity})", target.Name);
            }
            catch (Exception e) { _logger.WriteError($"Command_SetNextDropRarityForPlayer:\n{e}"); }
        }
    }
}
