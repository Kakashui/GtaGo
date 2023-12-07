using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Common;
using Whistler.Entities;
using Whistler.Families;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.Houses.Furnitures;
using Whistler.MoneySystem;
using Whistler.NewDonateShop;
using Whistler.Possessions;
using Whistler.SDK;
using static Whistler.Houses.DTOs.HouseOwnerMenuDTO;

namespace Whistler.Houses
{
    class HouseMenu : Script
    {

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(HouseMenu));
        [RemoteEvent("house:sellHouse")]
        public static void OnHouseSold(ExtPlayer player, int houseId)
        {
            try
            {
                House house = HouseManager.GetHouseById(houseId);
                if (house == null) return;
                if (house.Pledged)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_147", 3000);
                    return;
                }
                if (!house.GetAccess(player, FamilyHouseAccess.FullAccess))
                {
                    Notify.SendInfo(player, "House_142");
                    return;
                }
                var price = Convert.ToInt32(house.Price * 0.5);

                DialogUI.Open(player, "House_45".Translate(price), new List<DialogUI.ButtonSetting>
                {
                    new DialogUI.ButtonSetting
                    {
                        Name = "House_58",
                        Icon = "confirm",
                        Action = p =>
                        {
                            acceptHouseSellToGov(player, house, price);
                        }
                    },

                    new DialogUI.ButtonSetting
                    {
                        Name = "House_59",
                        Icon = "cancel",
                        Action = p => {}
                    }
                });
            }
            catch (Exception e) { _logger.WriteError($"Exception catched on 'houses:changeAccess' event: {e.ToString()}"); }
        }
        [RemoteEvent("homeMenu:buyGarage")]
        public static void OnHouseGarageBought(ExtPlayer player, int houseId, int garageIndex)
        {
            try
            {
                House house = HouseManager.GetHouseById(houseId);
                if (house == null || player.Character.InsideHouseID != house.ID)
                {
                    Notify.SendInfo(player, "House_54");
                    return;
                }
                if (!house.GetAccess(player, FamilyHouseAccess.UpgradeGarage))
                {
                    Notify.SendInfo(player, "House_142");
                    return;
                }
                int cost = Configs.HouseConfigs.GarageTypes[garageIndex].Cost;
                if (cost < 0)
                {
                    Notify.SendInfo(player, "Этот гараж недоступен для покупки.");
                    return;
                }
                DialogUI.Open(player, "newHouses_6".Translate(cost), new List<DialogUI.ButtonSetting>
                {
                    new DialogUI.ButtonSetting
                    {
                        Name = "House_58",
                        Icon = "confirm",
                        Action = p =>
                        {
                            if (!MoneySystem.Wallet.MoneySub(player.Character, cost, $"Money_UpdateGarage"))
                            {
                                Notify.SendAlert(player, "Core_178");
                                return;
                            }
                            house.HouseGarage.Type = garageIndex;
                            house.HouseGarage.Save();
                        }
                    },

                    new DialogUI.ButtonSetting
                    {
                        Name = "House_59",
                        Icon = "cancel",
                        Action = p => {}
                    }
                });

            }
            catch (Exception e) { _logger.WriteError($"Exception catched on 'houses:changeAccess' event: {e.ToString()}"); }
        }

        [RemoteEvent("homeMenu:installFurniture")]
        public static void OnHInstallFurniture(ExtPlayer player, int houseId, int id)
        {
            try
            {
                House house = HouseManager.GetHouseById(houseId);
                if (house == null || player.Character.InsideHouseID != house.ID)
                {
                    Notify.SendInfo(player, "House_54");
                    return;
                }
                if (!house.GetAccessFurniture(player, FamilyFurnitureAccess.MovingFurniture))
                {
                    Notify.SendInfo(player, "House_142");
                    return;
                }

                var furnitureToInstall = house.Furnitures[id];
                if (furnitureToInstall == null) return;
                SafeTrigger.ClientEvent(player,"house::startFurnitureInstallation", house.ID, JsonConvert.SerializeObject(new
                {
                    name = furnitureToInstall.ModelName,
                    id,
                    dimension = house.Dimension,
                }));
            }
            catch (Exception e) { _logger.WriteError($"Exception catched on 'houses:installFurniture' event: {e.ToString()}"); }
        }

        [RemoteEvent("homeMenu:furniturePlaced")]
        public static void OnHouseFurnitureInstalled(ExtPlayer player, int houseId, int furnitureId, string posData, string rotData)
        {
            try
            {
                House house = HouseManager.GetHouseById(houseId);
                if (house == null || player.Character.InsideHouseID != house.ID)
                {
                    Notify.SendInfo(player, "House_54");
                    return;
                }
                if (!house.GetAccessFurniture(player, FamilyFurnitureAccess.MovingFurniture))
                {
                    Notify.SendInfo(player, "House_142");
                    return;
                }

                var furnitureToInstall = house.Furnitures[furnitureId];
                if (furnitureToInstall == null) return;

                FurnitureService.InstallFurniture(house, furnitureToInstall, JsonConvert.DeserializeObject<Vector3>(posData),
                    JsonConvert.DeserializeObject<Vector3>(rotData));
                house.UpdateFurnitures();
            }
            catch (Exception e) { _logger.WriteError($"Exception catched on 'houses:installFurniture' event: {e.ToString()}"); }
        }

        [RemoteEvent("homeMenu:uninstallFurniture")]
        public static void OnHouseFurnitureUnInstalled(ExtPlayer player, int houseId, int id)
        {
            try
            {
                House house = HouseManager.GetHouseById(houseId);
                if (house == null || player.Character.InsideHouseID != house.ID)
                {
                    Notify.SendInfo(player, "House_54");
                    return;
                }
                if (!house.GetAccessFurniture(player, FamilyFurnitureAccess.MovingFurniture))
                {
                    Notify.SendInfo(player, "House_142");
                    return;
                }

                var furnitureToInstall = house.Furnitures[id];
                if (furnitureToInstall == null) return;
                FurnitureService.DeinstallFurniture(furnitureToInstall, house);
            }
            catch (Exception e) { _logger.WriteError($"Exception catched on 'houses:installFurniture' event: {e.ToString()}"); }
        }

        [RemoteEvent("homeMenu:uninstallAllFurniture")]
        public static void OnHouseAllFurnitureUnInstalled(ExtPlayer player, int houseId)
        {
            try
            {
                House house = HouseManager.GetHouseById(houseId);
                if (house == null || player.Character.InsideHouseID != house.ID)
                {
                    Notify.SendInfo(player, "House_54");
                    return;
                }
                if (!house.GetAccessFurniture(player, FamilyFurnitureAccess.MovingFurniture))
                {
                    Notify.SendInfo(player, "House_142");
                    return;
                }
                DialogUI.Open(player, "newHouses_8", new List<DialogUI.ButtonSetting>
                {
                    new DialogUI.ButtonSetting
                    {
                        Name = "House_58",
                        Icon = null,
                        Action = p =>
                        {
                            house.Furnitures.ForEach(f => FurnitureService.DeinstallFurniture(f, house));
                        }
                    },

                    new DialogUI.ButtonSetting
                    {
                        Name = "House_59",
                        Icon = null,
                        Action = p => {}
                    }
                });

            }
            catch (Exception e) { _logger.WriteError($"Exception catched on 'homeMenu:uninstallAllFurniture' event: {e.ToString()}"); }
        }
        public static void acceptHouseSellToGov(ExtPlayer player, House house, int price)
        {
            if (house == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_21", 3000);
                return;
            }

            if (player.Character.InsideHouseID != house.ID)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_35", 3000);
                return;
            }

            house.Furnitures.ToList().ForEach(f => FurnitureService.RemoveFurniture(f, house));
            house.UpdateFurnitures();
            house.HouseGarage.Type = house.HouseGarage.NativeType;
            house.HouseGarage.Save();

            house.SetOwner(-1, 0);
            MoneySystem.Wallet.MoneyAdd(player.Character, price, $"Продажа дома государству ({house.ID})");

            MainMenu.SendProperty(player);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "House_46".Translate(price), 3000);
        }

        [RemoteEvent("house:changeAccess")]
        public static void OnHouseAccessChange(ExtPlayer player, int houseId, int accessType, bool toggle, int occupierUuid)
        {
            try
            {
                House house = HouseManager.GetHouseById(houseId);
                if (house == null) return;
                if (house.OwnerType != OwnerType.Personal)
                    return;
                if (!house.GetAccess(player, FamilyHouseAccess.FullAccess))
                {
                    Notify.SendInfo(player, "House_142");
                    return;
                }
                var roommate = house.GetRoommate(occupierUuid);
                if (roommate == null) return;
                switch (accessType)
                {
                    case 0:
                        roommate.HasSafeAccess = toggle;
                        break;
                    case 1:
                        roommate.HasWardrobeAccess = toggle;
                        break;
                }
                house.UpdateRoommates();
            }
            catch (Exception e) { _logger.WriteError($"Exception catched on 'houses:changeAccess' event: {e.ToString()}"); }
        }

        [RemoteEvent("house:rentCostChanged")]
        public static void OnHouseRentCostChanged(ExtPlayer player, int houseId, int newValue)
        {
            try
            {
                House house = HouseManager.GetHouseById(houseId);
                if (house == null) return;
                if (house.OwnerType != OwnerType.Personal) return;
                if (newValue < 0) return;

                if (!house.GetAccess(player, FamilyHouseAccess.FullAccess))
                {
                    Notify.SendError(player, "House_142");
                    return;
                }
                if (house.RoomatesAny())
                {
                    Notify.SendError(player, "Сменить цену аренды невозможно, потому как в доме уже есть сожители.");
                    return;
                }
                house.SetRentPrice(newValue);
                player.TriggerCefEvent("homeMenu/setRentCost", newValue);
                Notify.SendSuccess(player, $"Вы успешно сменили цену аренды на {newValue}");
            }
            catch (Exception e) { _logger.WriteError($"Exception catched on 'houses:rentCostChanged' event: {e.ToString()}"); }
        }

        [RemoteEvent("homeMenu:lockToggle")]
        public static void OnHouseLockToggle(ExtPlayer player, int houseId, bool newValue)
        {
            try
            {
                House house = HouseManager.GetHouseById(houseId);
                if (house == null) return;
                if (!house.GetAccess(player, FamilyHouseAccess.OpenDoors))
                {
                    Notify.SendError(player, "House_142");
                    return;
                }
                house.SetLock(newValue);
                player.TriggerCefEvent("homeMenu/setHouseLocked", null);
            }
            catch (Exception e) { _logger.WriteError($"Exception catched on 'houses:lockToggle' event: {e.ToString()}"); }
        }

        [RemoteEvent("house:occupierDeleted")]
        public static void OnHouseOccupierDeleted(ExtPlayer player, int houseId, int uuid)
        {
            try
            {
                House house = HouseManager.GetHouseById(houseId);
                if (house == null) return;
                if (house.OwnerType != OwnerType.Personal)
                    return;
                if (!house.GetAccess(player, FamilyHouseAccess.FullAccess))
                {
                    Notify.SendError(player, "House_142");
                    return;
                }
                house.RemoveRoommate(uuid);
            }
            catch (Exception e) { _logger.WriteError($"Exception catched on 'houses:rentCostChanged' event: {e.ToString()}"); }
        }

        [RemoteEvent("house:allOccupiersDeleted")]
        public static void OnHouseOccupiersDeleted(ExtPlayer player, int houseId)
        {
            try
            {
                House house = HouseManager.GetHouseById(houseId);
                if (house == null) return;
                if (house.OwnerType != OwnerType.Personal)
                    return;
                if (!house.GetAccess(player, FamilyHouseAccess.FullAccess))
                {
                    Notify.SendError(player, "House_142");
                    return;
                }
                house.RemoveRoommates();
            }
            catch (Exception e) { _logger.WriteError($"Exception catched on 'houses:rentCostChanged' event: {e.ToString()}"); }
        }

        [RemoteEvent("house:occupierAddedRequest")]
        public static void OnHouseOccupiersAdded(ExtPlayer player, int houseId, int targetId)
        {
            try
            {
                if (player.Character.UUID == targetId) return;
                House house = HouseManager.GetHouseById(houseId);
                if (house == null || player.Character.InsideHouseID != house.ID)
                {
                    Notify.SendError(player, "House_54");
                    return;
                }
                if (house.OwnerType != OwnerType.Personal) return;
                if (!house.GetAccess(player, FamilyHouseAccess.FullAccess))
                {
                    Notify.SendError(player, "House_142");
                    return;
                }
                var newOccupier = Trigger.GetPlayerByUuid(targetId);
                if (newOccupier == null || newOccupier.Character.InsideHouseID != house.ID)
                {
                    Notify.SendError(player, "House_54");
                    return;
                }
                DialogUI.Open(newOccupier, $"{player.Name} ({player.Character.UUID}) пригласил Вас подселиться в дом. Цена аренды за сутки составит ${house.RentCost}. Деньги списываются каждый день в {HouseManager.HourTimeToProceedRoommatesRent}:00 с банковского счёта. Вы согласны?", new List<DialogUI.ButtonSetting>
                {
                    new DialogUI.ButtonSetting
                    {
                        Name = "House_58",
                        Icon = "confirm",
                        Action = p =>
                        {
                            HouseManager.CheckAndKick(p);
                            Roommate roommate = new Roommate(p.Character.UUID);
                            house.AddRoommate(roommate);

                            HouseMenuOccupierItem occupier = new HouseMenuOccupierItem
                            {
                                Name = Main.PlayerNames.GetValueOrDefault(p.Character.UUID, "Unknown"),
                                GarageAccess = roommate.HasWardrobeAccess,
                                SafeAccess = roommate.HasSafeAccess,
                                UUID = roommate.CharacterUUID
                            };
                            player.TriggerCefEvent("homeMenu/addOccupiers", JsonConvert.SerializeObject(occupier));
                            MainMenu.SendProperty(p);
                            Notify.SendSuccess(p, "house:occup:1");
                        }
                    },

                    new DialogUI.ButtonSetting
                    {
                        Name = "House_59",
                        Icon = "cancel",
                        Action = p => {}
                    }
                });
            }
            catch (Exception e) { _logger.WriteError($"Exception catched on 'houses:rentCostChanged' event: {e.ToString()}"); }
        }
    }
}
