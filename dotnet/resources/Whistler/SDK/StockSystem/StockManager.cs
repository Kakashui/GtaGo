using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Whistler.Common;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Fractions;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.SDK;

namespace Whistler.SDK.StockSystem
{
    class StockManager : Script
    {
        private static Dictionary<int, StockBase> Stocks = new Dictionary<int, StockBase>();

        private static Dictionary<int, StockConfig> StockTypes = new Dictionary<int, StockConfig>
        {
            { 1, new StockConfig(10000000, 100, InventoryTypes.General) },
            { 2, new StockConfig(10000000, 100, InventoryTypes.WeaponStock) },
            { 3, new StockConfig(10000000, 100, InventoryTypes.AmmoStock) },
            { 4, new StockConfig(10000000, 100, InventoryTypes.MedKit) },
            { 5, new StockConfig(10000000, 100, InventoryTypes.GangStock) },
            { 6, new StockConfig(10000000, 100, InventoryTypes.OrganizationStock) },
        };

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(StockManager));


        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            try
            {
                DataTable result = MySQL.QueryRead("SELECT * FROM `fractionstock`");
                if (result == null || result.Rows.Count == 0)
                {
                    return;
                }
                StockBase stock = null;
                foreach (DataRow row in result.Rows)
                {
                    stock = null;
                    OwnerType typeowner = (OwnerType)Convert.ToInt32(row["typeowner"]);
                    switch (typeowner)
                    {
                        case OwnerType.Family:
                            stock = new FamilyStock(row);
                            break;
                        case OwnerType.Fraction:
                            stock = new FractionStock(row);
                            break;
                        default:
                            break;
                    }                    
                    if (stock != null)
                        Stocks.Add(stock.Id, stock);
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($" OnResourceStart: {e.ToString()}");
            }
        }

        public static StockBase GetStockByPredicate(Func<StockBase, bool> predicate)
        {
            return Stocks.FirstOrDefault(item => predicate(item.Value)).Value;
        }

        public static void ChangePassword(ExtPlayer player, int stockId, string newpass)
        {
            if (!player.IsLogged())
                return;
            if (!Stocks.ContainsKey(stockId))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "stocks_2".Translate(stockId), 3000);
                return;
            }
            if (newpass.Length < 4 || newpass.Length > 8)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_86".Translate(stockId), 3000);
                return;
            }
            Stocks[stockId].ChangePassword(newpass);
            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы успешно поменяли пароль для склада", 3000);
        }

        public static void OpenStock(ExtPlayer player, int stockId, string inputpass)
        {
            if (!Stocks.ContainsKey(stockId)) return;

            StockBase stock = Stocks[stockId];
            if (stock.Password != inputpass)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Неверный пароль", 3000);
                return;
            }
            InventoryService.OpenStock(player, stock.InventoryId, Inventory.Enums.StockTypes.Default);
        }

        [Command("createstock")]
        public static void CMD_CreateFractionStock(ExtPlayer player, int fracId, string pass, int type, int size = 1)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (!Group.CanUseAdminCommand(player, "createstock"))
                    return;
                if (Manager.GetFraction(fracId) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "stocks_4", 3000);
                    return;
                }

                if (!StockTypes.ContainsKey(type))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "stocks_3", 3000);
                    return;
                }

                if (Stocks.Values.FirstOrDefault(x => x.OwnerId == fracId && x.TypeOwner == OwnerType.Fraction) != null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Эта фракция уже имеет свой склад, сначала удалите предыдущий.", 3000);
                    return;
                }
                FractionStock stock = new FractionStock(fracId, pass, StockTypes[type], player.Position - new Vector3(0, 0, 1), player.Dimension, size);
                Stocks.Add(stock.Id, stock);
            }
            catch (Exception e)
            {
                _logger.WriteError($" CMD_CreateFractionStock: {e.ToString()}");
            }
        }
        [Command("createfamilystock")]
        public static void CMD_CreateFamilyStock(ExtPlayer player, int id, string pass, int type)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (!Group.CanUseAdminCommand(player, "createfamilystock"))
                    return;
                var target = Trigger.GetPlayerByUuid(id);
                if (target == null)
                    return;
                var family = target.GetFamily();
                if (family == null)
                    return;

                if (!StockTypes.ContainsKey(type))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "stocks_3", 3000);
                    return;
                }

                if (Stocks.Values.FirstOrDefault(x => x.OwnerId == id && x.TypeOwner == OwnerType.Family) != null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Эта семья уже имеет свой склад, сначала удалите предыдущий.", 3000);
                    return;
                }
                var stock = new FamilyStock(family.Id, pass, StockTypes[type], player.Position - new Vector3(0, 0, 1), player.Dimension);
                Stocks.Add(stock.Id, stock);
            }
            catch (Exception e)
            {
                _logger.WriteError($" CMD_CreateFamilyStock: {e.ToString()}");
            }
        }

        [Command("changestockpos")]
        public static void CMD_ChangeStockPos(ExtPlayer player, int stock)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (!Group.CanUseAdminCommand(player, "changestockpos"))
                    return;
                if (!Stocks.ContainsKey(stock))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "stocks_2".Translate(stock), 3000);
                    return;
                }
                Stocks[stock].ChangePosition(player.Position - new Vector3(0, 0, 1), player.Dimension);
            }
            catch (Exception e)
            {
                _logger.WriteError($" CMD_ChangeStockPos: {e.ToString()}");
            }
        }


        [Command("deletestock")]
        public static void CMD_DeleteStock(ExtPlayer player, int stock)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (!Group.CanUseAdminCommand(player, "deletestock"))
                    return;
                if (!Stocks.ContainsKey(stock))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "stocks_2".Translate(stock), 3000);
                    return;
                }
                Stocks[stock].Destroy();
                Stocks.Remove(stock);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "stocks_6".Translate(stock), 3000);
            }
            catch (Exception e)
            {
                _logger.WriteError($" CMD_DeleteStock: {e.ToString()}");
            }
        }

        [Command("givestockmed")]
        public static void GiveStockMed(ExtPlayer player, int stock, int amount)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "givestockmed")) return;

                BaseItem item = ItemsFabric.CreateMedicaments(ItemNames.HealthKit, amount, false);
                if (item == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Ошибка создания предмета, свяжитесь с разработчиком.", 3000);
                    return;
                }

                if (!Stocks.ContainsKey(stock))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "stocks_2".Translate(stock), 3000);
                    return;
                }
                InventoryModel inventory = InventoryService.GetById(Stocks[stock].InventoryId);
                if (inventory == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Ошибка нахождения инвентаря склада, свяжитесь с разработчиком.", 3000);
                    return;
                }
                if (!inventory.AddItem(item))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Склад не может вместить новый предмет.", 3000);
                    return;
                }
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Вы успешно выдали предмет на склад.", 3000);
                GameLog.Admin($"{player.Name}", $"GiveStockMed", $"stock({stock})");
            }
            catch (Exception e)
            {
                _logger.WriteError($" GiveStockMed: {e.ToString()}");
            }
        }

        [Command("givestockmar")]
        public static void GiveStockMarijuana(ExtPlayer player, int stock, int amount)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "givestockmar")) return;
                var item = ItemsFabric.CreateNarcotic(ItemNames.Marijuana, amount, false);
                if (item == null)
                {
                    return;
                }
                if (!Stocks.ContainsKey(stock))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "stocks_2".Translate(stock), 3000);
                    return;
                }
                var inventory = InventoryService.GetById(Stocks[stock].InventoryId);
                if (inventory == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Ошибка нахождения инвентаря склада, свяжитесь с разработчиком.", 3000);
                    return;
                }
                if (!inventory.AddItem(item))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Склад не может вместить новый предмет.", 3000);
                    return;
                }
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Вы успешно выдали предмет на склад.", 3000);
                GameLog.Admin($"{player.Name}", $"GiveStockMar", $"stock({stock})");
            }
            catch (Exception e)
            {
                _logger.WriteError($" GiveStockMarijuana: {e.ToString()}");
            }
        }

        [Command("givestockarm")]
        public static void GiveStockArmor(ExtPlayer player, int stock, int amount)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "givestockgun")) return;
                if (!Stocks.ContainsKey(stock))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "stocks_2".Translate(stock), 3000);
                    return;
                }
                InventoryModel inventory = InventoryService.GetById(Stocks[stock].InventoryId);
                if (inventory == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Ошибка нахождения инвентаря склада, свяжитесь с разработчиком.", 3000);
                    return;
                }
                for (int i = 0; i < amount; i++)
                {
                    var item = ItemsFabric.CreateClothes(ItemNames.BodyArmor, true, 0, 0, false);
                    if (item == null)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_55", 3000);
                        return;
                    }
                    if (!inventory.AddItem(item))
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Склад не может вместить новый предмет.", 3000);
                        break;
                    }
                }
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Вы успешно выдали предмет на склад.", 3000);
                GameLog.Admin($"{player.Name}", $"giveArmorStock", $"stock({stock})");
            }
            catch (Exception e)
            {
                _logger.WriteError($" GiveStockArmor: {e.ToString()}");
            }
        }

        [Command("givestockbox")]
        public static void GiveStockBox(ExtPlayer player, int stock, string weapon, int amount)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "givestockbox")) return;
                if (!Stocks.ContainsKey(stock))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "stocks_2".Translate(stock), 3000);
                    return;
                }
                if (!Enum.TryParse(weapon, out ItemNames itemName))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Такого предмета не существует.", 3000);
                    return;
                }

                InventoryModel inventory = InventoryService.GetById(Stocks[stock].InventoryId);
                if (inventory == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Ошибка нахождения инвентаря склада, свяжитесь с разработчиком.", 3000);
                    return;
                }

                ItemNames boxType = itemName switch
                {
                    ItemNames.PistolAmmo => ItemNames.AmmoBox,
                    ItemNames.SMGAmmo => ItemNames.AmmoBox,
                    ItemNames.RiflesAmmo => ItemNames.AmmoBox,
                    ItemNames.SniperAmmo => ItemNames.AmmoBox,
                    ItemNames.ShotgunsAmmo => ItemNames.AmmoBox,
                    ItemNames.MusketAmmo => ItemNames.AmmoBox,
                    _ => ItemNames.WeaponBox
                };
                ItemBox item = ItemsFabric.CreateItemBox(boxType, itemName, amount, false);
                if (item == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Данный предмет не может находиться в коробке для оружия.", 3000);
                    return;
                }
                if (!inventory.AddItem(item))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Склад не может вместить новый предмет.", 3000);
                    return;
                }
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Вы успешно выдали предмет на склад.", 3000);
                GameLog.Admin($"{player.Name}", $"giveBoxStock", $"stock({stock})");
            }
            catch (Exception e)
            {
                _logger.WriteError($" GiveStockBox: {e.ToString()}");
            }
        }

        [Command("givestockboxm")]
        public static void GiveStockBoxHealth(ExtPlayer player, int stock, int amount)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "givestockboxm")) return;
                if (!Stocks.ContainsKey(stock))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "stocks_2".Translate(stock), 3000);
                    return;
                }
                var inventory = InventoryService.GetById(Stocks[stock].InventoryId);
                if (inventory == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Ошибка нахождения инвентаря склада, свяжитесь с разработчиком.", 3000);
                    return;
                }
                var item = ItemsFabric.CreateItemBox(ItemNames.MedkitBox, ItemNames.HealthKit, amount, false);
                if (item == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_55", 3000);
                    return;
                }
                if (!inventory.AddItem(item))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Склад не может вместить новый предмет.", 3000);
                    return;
                }
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Вы успешно выдали предмет на склад.", 3000);
                GameLog.Admin($"{player.Name}", $"giveBoxStockMed", $"stock({stock})");
            }
            catch (Exception e)
            {
                _logger.WriteError($" GiveStockBox: {e.ToString()}");
            }
        }

        [Command("givestockboxa")]
        public static void GiveStockBoxArmor(ExtPlayer player, int stock, int amount)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "givestockboxa")) return;
                if (!Stocks.ContainsKey(stock))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "stocks_2".Translate(stock), 3000);
                    return;
                }
                var inventory = InventoryService.GetById(Stocks[stock].InventoryId);
                if (inventory == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Ошибка нахождения инвентаря склада, свяжитесь с разработчиком.", 3000);
                    return;
                }
                var item = ItemsFabric.CreateItemBox(ItemNames.ArmorBox, ItemNames.BodyArmor, amount, false);
                if (item == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_55", 3000);
                    return;
                }
                if (!inventory.AddItem(item))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Склад не может вместить новый предмет.", 3000);
                    return;
                }
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Вы успешно выдали предмет на склад.", 3000);
                GameLog.Admin($"{player.Name}", $"giveBoxStockArm", $"stock({stock})");
            }
            catch (Exception e)
            {
                _logger.WriteError($" GiveStockBox: {e.ToString()}");
            }
        }
    }
}
