using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Families;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.Houses.Furnitures;
using Whistler.Possessions;
using Whistler.MoneySystem;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models.VehiclesData;
using System.Threading.Tasks;
using Whistler.NewDonateShop;
using Whistler.Common;
using Whistler.MiniGames.Lockpik;
using Whistler.Entities;
using Whistler.Jobs.HomeRobbery;
using Whistler.Inventory;
using Whistler.Houses.Models;
using Whistler.Core.Character;

namespace Whistler.Houses
{
    class HouseManager : Script
    {
        public const int PERSONAL_HOUSE_BLIP_ID = 9393;
        public const int HourTimeToProceedRoommatesRent = 12;

        private static WhistlerLogger _logger= new WhistlerLogger(typeof(HouseManager));

        public static List<House> Houses = new List<House>();
        public static Dictionary<HouseTypes, HouseType> HouseTypeList = new Dictionary<HouseTypes, HouseType>
        {
            // name, position
            [HouseTypes.NClass] = new HouseType("N class", new Vector3(1973.124, 3816.065, 32.42873), 0.0f, "trevorstrailer"),
            [HouseTypes.DClass] = new HouseType("D class", new Vector3(151.2052, -1008.007, -100.002), 0.0f, "hei_hw1_blimp_interior_v_motel_mp_milo_"),
            [HouseTypes.CClass] = new HouseType("C class", new Vector3(265.9691, -1007.078, -101.9838), 0.0f, "hei_hw1_blimp_interior_v_studio_lo_milo_"),
            [HouseTypes.BClass] = new HouseType("B class", new Vector3(346.6991, -1013.023, -100.1963), 264.0f, "hei_hw1_blimp_interior_v_apart_midspaz_milo_"),
            [HouseTypes.AClassArcadius] = new HouseType("A class", new Vector3(-31.35483, -594.9686, 79.0309), 159.84f, "hei_hw1_blimp_interior_32_dlc_apart_high2_new_milo_"),
            [HouseTypes.SClassArcadius] = new HouseType("S class", new Vector3(-17.85757, -589.0983, 89.11482), 50.8f, "hei_hw1_blimp_interior_10_dlc_apart_high_new_milo_"),
            
            [HouseTypes.PremiumPlus] = new HouseType("Premium+", new Vector3(-174.5012, 497.8107, 136.6337), 40.0f, "apa_ch2_05e_interior_0_v_mp_stilts_b_milo_"), //
            [HouseTypes.Penthouse] = new HouseType("Penthouse", new Vector3(373.815, 423.69, 145.90787), 168.9f, "apa_ch2_05e_interior_0_v_mp_stilts_b_milo_"), //

            [HouseTypes.AClassDellPerro] = new HouseType("A class", new Vector3(-1452.46, -540.45, 73.044), 35.1f, ""),
            [HouseTypes.AClassRockFord] = new HouseType("A class", new Vector3(-912.75, -365.13, 113.27478), 116.9f, ""),
            [HouseTypes.SClassDellPerro] = new HouseType("S class", new Vector3(-1451.46, -523.898, 55.92), 30.7f, ""),
            [HouseTypes.SClassRockFord] = new HouseType("S class", new Vector3(-603.2864, 58.95, 97.2), 100.6f, ""),
            
            [HouseTypes.PremiumPlus2] = new HouseType("Premium+", new Vector3(341.95, 437.73, 148.39), 117.6f, "apa_ch2_05e_interior_0_v_mp_stilts_b_milo_"), //
            [HouseTypes.Penthouse2] = new HouseType("Penthouse", new Vector3(-682.266, 592.487, 144.39262), -137.377f, "apa_ch2_05e_interior_0_v_mp_stilts_b_milo_"), //
            [HouseTypes.ModGarage] = new HouseType("Mod Garage", new Vector3(-1569.7882, -572.8358, 104.2), 36.0f, "imp_sm_13_modgarage"),
            [HouseTypes.OnlineAppartWhite] = new HouseType("Online Appartment White", new Vector3(-786.85, 315.79, 216.6385), -85.5f, "apa_v_mp_h_01_a"),
            [HouseTypes.OnlineAppartBlack] = new HouseType("Online Appartment Black", new Vector3(-774.13, 342.06, 195.69), 89.5f, "apa_v_mp_h_02_b"),
            [HouseTypes.OnlineAppartColor] = new HouseType("Online Appartment Color", new Vector3(-786.85, 315.79, 186.91373), 89.5f, "apa_v_mp_h_03_c"),
            //[HouseTypes.ArcadiusOffice] = new HouseType("Arcadius office", new Vector3(-141.2896, -620.9618, 167.8204), -80f, "ex_dt1_02_office_01a"),
        };

        public static Dictionary<HouseTypes, int> MaxRoommates = new Dictionary<HouseTypes, int>()
        { 
            [HouseTypes.NClass] = 1,
            [HouseTypes.DClass] = 2,
            [HouseTypes.CClass] = 3,
            [HouseTypes.BClass] = 4,
            [HouseTypes.AClassArcadius] = 5,
            [HouseTypes.AClassDellPerro] = 5,
            [HouseTypes.AClassRockFord] = 5,
            [HouseTypes.SClassArcadius] = 6,
            [HouseTypes.SClassDellPerro] = 6,
            [HouseTypes.SClassRockFord] = 6,
            [HouseTypes.PremiumPlus] = 7,
            [HouseTypes.PremiumPlus2] = 7,
            [HouseTypes.Penthouse] = 8,
            [HouseTypes.Penthouse2] = 8,
            [HouseTypes.ModGarage] = 5,
            [HouseTypes.OnlineAppartWhite] = 6,
            [HouseTypes.OnlineAppartBlack] = 6,
            [HouseTypes.OnlineAppartColor] = 6,
            [HouseTypes.ArcadiusOffice] = 5,
        };


        public static Action<ExtPlayer, House> PlayerEntered;
        public static Action<ExtPlayer> PlayerLeaved;

        public static uint DimensionID = 10000;
        
        #region Events
        public static void OnLoadHouses()
        {
            try
            {
                //var ipls = new List<string>
                //{
                //    "imp_sm_13_modgarage",
                //    "apa_v_mp_h_01_a",
                //    "apa_v_mp_h_02_b",
                //    "apa_v_mp_h_03_c",
                //    "ex_dt1_02_office_01a",
                //    "ex_dt1_02_office_02b",
                //    "ex_dt1_02_office_03c",
                //    "imp_dt1_02_cargarage_a",
                //    "imp_dt1_02_cargarage_b",
                //    "imp_dt1_02_cargarage_c",
                //    "imp_dt1_02_modgarage",
                //    "ex_dt1_11_office_01a",
                //    "ex_dt1_11_office_02b",
                //    "ex_dt1_11_office_03c",
                //    "imp_dt1_11_cargarage_a",
                //    "imp_dt1_11_cargarage_b",
                //    "imp_dt1_11_cargarage_c",
                //    "imp_dt1_11_modgarage",
                //    "ex_sm_13_office_01a",
                //    "ex_sm_13_office_02b",
                //    "ex_sm_13_office_03c",
                //    "imp_sm_13_cargarage_a",
                //    "imp_sm_13_cargarage_b",
                //    "imp_sm_13_cargarage_c",
                //    "imp_sm_13_modgarage",
                //    "ex_sm_15_office_01a",
                //    "ex_sm_15_office_02b",
                //    "ex_sm_15_office_03c",
                //    "imp_sm_15_cargarage_a",
                //    "imp_sm_15_cargarage_b",
                //    "imp_sm_15_cargarage_c",
                //    "imp_sm_15_modgarage",
                //    "bkr_biker_interior_placement_interior_0_biker_dlc_int_01_milo",
                //    "bkr_biker_interior_placement_interior_1_biker_dlc_int_02_milo",
                //    "bkr_biker_interior_placement_interior_2_biker_dlc_int_ware01_milo",
                //    "bkr_biker_interior_placement_interior_3_biker_dlc_int_ware02_milo",
                //    "bkr_biker_interior_placement_interior_4_biker_dlc_int_ware03_milo",
                //    "bkr_biker_interior_placement_interior_5_biker_dlc_int_ware04_milo",
                //    "bkr_biker_interior_placement_interior_6_biker_dlc_int_ware05_milo",
                //    "ex_exec_warehouse_placement_interior_1_int_warehouse_s_dlc_milo",
                //    "ex_exec_warehouse_placement_interior_0_int_warehouse_m_dlc_milo",
                //    "ex_exec_warehouse_placement_interior_2_int_warehouse_l_dlc_milo",
                //    "imp_impexp_interior_placement_interior_1_impexp_intwaremed_milo_",
                //    "bkr_bi_hw1_13_int",
                //    "cargoship",
                //    "DES_StiltHouse_imapend",
                //    "DES_stilthouse_rebuild",
                //    "FINBANK",
                //    "TrevorsTrailerTidy",
                //    "SP1_10_real_interior",
                //    "refit_unload",
                //    "post_hiest_unload",
                //    "FIBlobby",




                //};
                //foreach (var ipd in ipls)
                //{
                //    NAPI.World.RequestIpl(ipd);
                //}

                foreach (HouseType house_type in HouseTypeList.Values) 
                    house_type.Create();

                var result = MySQL.QueryRead("SELECT * FROM `houses`");
                if (result == null || result.Rows.Count == 0)
                {
                    _logger.WriteWarning("DB return null result.");
                    return;
                }

                Dictionary<int, int> garagesLoaded = new Dictionary<int, int>();
                int garageId;
                int houseId;
                foreach (DataRow row in result.Rows)
                {
                    try
                    {
                        garageId = Convert.ToInt32(row["garage"]);
                        houseId = Convert.ToInt32(row["id"]);
                        if (garagesLoaded.ContainsKey(garageId)) _logger.WriteWarning($"GARAGE {garageId} IS USED IN {garagesLoaded[garageId]} AND {houseId} HOUSES.");
                        else garagesLoaded.Add(garageId, houseId);

                        House house = new House(row);
                        Houses.Add(house);
                    }
                    catch (Exception e)
                    {
                        _logger.WriteError(row["id"].ToString() + e.ToString());
                    }
                }

                NAPI.Object.CreateObject(0x07e08443, new Vector3(1972.76892, 3815.36694, 33.6632576), new Vector3(0, 0, -109.999962), 255, NAPI.GlobalDimension);
                _logger.WriteInfo($"Loaded {Houses.Count} houses.");
            }
            catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
        }

        public static void Event_OnPlayerDeath(ExtPlayer player, Player entityKiller, uint weapon)
        {
            try
            {
                GetHouseById(player.Character.InsideHouseID)?.RemovePlayer(player);
            }
            catch (Exception e) { _logger.WriteError("PlayerDeath: " + e.ToString()); }
        }

        public static void Event_OnPlayerDisconnected(ExtPlayer player, DisconnectionType type, string reason)
        {
            try
            {
                GetHouseById(player.Character.InsideHouseID)?.RemoveFromList(player);
            }
            catch (Exception e) { _logger.WriteError("PlayerDisconnected: " + e.ToString()); }
        }
        #endregion

        #region Methods
        public static House GetHouse(ExtPlayer player, bool checkOwner = false)
        {
            House house = Houses.FirstOrDefault(h => h.OwnerType == OwnerType.Personal && h.OwnerID == player.Character.UUID);
            if (house != null) return house;
            if (checkOwner) return null;

            return Houses.FirstOrDefault(h => h.GetRoommate(player.Character.UUID) != null);
        }
        public static House GetHouseFamily(ExtPlayer player)
        {
            if (player.Character.FamilyID == 0)
                return null;
            return Houses.FirstOrDefault(h => h.OwnerID == player.Character.FamilyID && h.OwnerType == OwnerType.Family);
        }

        public static House GetHouse(int ownerId, OwnerType ownerType, bool checkOwner = false)
        {
            House house = Houses.FirstOrDefault(h => h.OwnerID == ownerId && h.OwnerType == ownerType);
            if (house != null)
                return house;
            else if (!checkOwner)
                return Houses.FirstOrDefault(h => h.GetRoommate(ownerId) != null);
            else
                return null;
        }
        public static House GetHouseById(int id)
        {
            return Houses.FirstOrDefault(h => h.ID == id);
        }
        public static House GetHouseByGarageId(int id)
        {
            return Houses.FirstOrDefault(h => h.GarageID == id);
        }

        public static void CheckAndKick(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            GetHouse(player)?.RemoveRoommate(player.Character.UUID);
        }
        #endregion

        [RemoteEvent("houses:buy")]
        public static void Houses_BuyEvent(ExtPlayer player, string type)
        {
            try
            {
                if (!player.HasData("HOUSEID")) return;

                House house = Houses.FirstOrDefault(h => h.ID == player.GetData<int>("HOUSEID"));
                if (house == null) return;
                DialogUI.Open(player, "Купить дом как:", new List<DialogUI.ButtonSetting>
                {
                    new DialogUI.ButtonSetting
                    {
                        Name = "Личный",
                        Icon = null,
                        Action = (p) => BuyHouse(p, house, OwnerType.Personal, type),
                    },

                    new DialogUI.ButtonSetting
                    {
                        Name = "Семейный",
                        Icon = null,
                        Action = (p) => BuyHouse(p, house, OwnerType.Family, type),
                    },

                    new DialogUI.ButtonSetting
                    {
                        Name = "Отмена",
                        Icon = null,
                        Action = (p) => { },
                    }
                });
            }
            catch (Exception e) { _logger.WriteError($"Exception catched on 'houses:buy' event: {e.ToString()}"); }
        }

        public static void BuyHouse(ExtPlayer player, House house, OwnerType typeOwner, string payType)
        {
            if (house.OwnerID != -1)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У этого дома уже есть владелец", 3000);
                return;
            }

            var character = player.Character;
            var paymentFrom = player.GetMoneyPayment((payType == "cash") ? PaymentsType.Cash : PaymentsType.Card);
            if (!MoneySystem.Wallet.TryChange(paymentFrom, -house.Price))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У вас недостаточно средств", 3000);
                return;
            }

            switch (typeOwner)
            {
                case OwnerType.Personal:
                    if (Houses.Count(h => h.OwnerID == character.UUID && h.OwnerType == OwnerType.Personal) >= 1)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы не можете купить более одного дома", 3000);
                        return;
                    }
                    Notify.Alert(player, "Вы приобрели дом");
                    CheckAndKick(player);
                    house.SetOwner(character.UUID, typeOwner);
                    break;
                case OwnerType.Family:
                    var family = player.GetFamily();
                    if (family == null || !family.IsLeader(player) )
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы должны быть лидером семьи для покупки этого дома", 3000);
                        return;
                    }
                    if (Houses.Count(h => h.OwnerID == character.FamilyID && h.OwnerType == OwnerType.Family) >= 1)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У вашей семьи уже есть дом", 3000);
                        return;
                    }
                    Notify.Alert(player, "Вы купили дом для семьи");
                    house.SetOwner(character.FamilyID, typeOwner);
                    break;
            }

            house.SetLock(true);

            house.SendPlayer(player);
            MoneySystem.Wallet.SetBankMoney(house.BankNew, house.HouseTax * 48);

            MainMenu.SendProperty(player);
            MoneySystem.Wallet.MoneySub(paymentFrom, house.Price, $"Покупка дома ({(int)typeOwner}, #{house.ID})");
        }

        [RemoteEvent("houses:enter")]
        public static void Houses_EnterEvent(ExtPlayer player, bool inHouse)
        {
            try
            {
                if (player.IsInVehicle) return;
                if (!player.HasData("HOUSEID")) return;

                House house = GetHouseById(player.GetData<int>("HOUSEID"));
                if (house == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_23", 3000);
                    return;
                }

                Garage garage = null;
                if (GarageManager.Garages.ContainsKey(house.GarageID)) garage = GarageManager.Garages[house.GarageID];

                if (house.OwnerID == -1)
                {
                    MoveToHouseOrGarage(player, house, garage, inHouse);
                }
                else
                {
                    if (house.Locked)
                    {
                        House playerHouse = GetHouse(player);
                        House familyHouse = GetHouseFamily(player);
                        if (playerHouse != null && playerHouse.ID == house.ID) MoveToHouseOrGarage(player, house, garage, inHouse);
                        else if (familyHouse != null && familyHouse.ID == house.ID) MoveToHouseOrGarage(player, house, garage, inHouse);
                        else Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_24", 3000);
                    }
                    else MoveToHouseOrGarage(player, house, garage, inHouse);
                }
            }
            catch (Exception e) { _logger.WriteError($"Exception catched on 'houses:buy' event: {e.ToString()}"); }
        }

        private static void MoveToHouseOrGarage(ExtPlayer player, House house, Garage garage, bool inHouse)
        {
            if (player == null) return;

            if (inHouse)
            {
                if (house == null) return;
                
                house.SendPlayer(player);
                return;
            }

            if (garage == null) return;

            garage.SendPlayer(player);
        }

        [RemoteEvent("houses:breakTheDoor")]
        public static void Houses_BreakTheDoor(ExtPlayer player)
        {
            if (player.IsInVehicle) return;
            if (!player.HasData("HOUSEID")) return;

            House house = GetHouseById(player.GetData<int>("HOUSEID"));
            if (house == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_23", 3000);
                return;
            }
            if (player.Character.HouseTarget != house.ID)
            {
                return;
            }
            if (player.GetInventory().GetItemLink(Inventory.Enums.ItemNames.Stetoskop) == null)
            {
                Notify.SendError(player, "home:robbery:19");
                return;
            }
            LockpickService.StartLockpickGame(player, "houses:theDoorIsBroken");
            HomeRobberyManager.CallPolice(house, true);
        }

        [RemoteEvent("houses:theDoorIsBroken")]
        public static void Houses_TheDoorIsBroken(ExtPlayer player)
        {

            if (player.IsInVehicle) return;
            if (!player.HasData("HOUSEID")) return;

            House house = GetHouseById(player.GetData<int>("HOUSEID"));
            if (house == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_23", 3000);
                return;
            }
            if (player.Character.HouseTarget != house.ID)
            {
                return;
            }
            house.SetLock(false);
            player.CreatePlayerAction(PersonalEvents.PlayerActions.BreakHouseLock, 1);
        }

        [RemoteEvent("house:OwnerInteracted")]
        public static void OnOwnerMenuOpenedFromMMenu(ExtPlayer player)
        {
            House house = GetHouse(player);
            if (house == null) return;

            house.OnOwnerInteracted(player, house.OwnerID == player.Character.UUID);
        }

        [RemoteEvent("house:openFamilyHouseMenu")]
        public static void OpenFamilyHouseMenu(ExtPlayer player)
        {
            Families.Models.Family family = player.GetFamily();
            if (family == null) return;

            House familyHouse = family.GetHouse();
            if (familyHouse == null) return;

            familyHouse.OnOwnerInteracted(player, reopen: false);
        }
        

        public static void ExitHouse(ExtPlayer player)
        {
            if (player.Character.InsideHouseID == -1) return;

            DialogUI.Open(player, "houses_2", new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "houses_0",
                    Icon = null,
                    Action = ExitToStreet,
                },

                new DialogUI.ButtonSetting
                {
                    Name = "houses_1",
                    Icon = null,
                    Action = ExitToGarage,
                }
            });
        }

        public static void ExitToGarage(ExtPlayer player)
        {
            if (player.Character.InsideHouseID == -1) return;

            House house = GetHouseById(player.Character.InsideHouseID);
            if (house == null) return;
            if (house.HouseGarage?.EnterGarage(player, false) ?? false)
                house.RemovePlayer(player, false);
        }

        public static void ExitToStreet(ExtPlayer player)
        {
            GetHouseById(player.Character.InsideHouseID)?.RemovePlayer(player);
        }

        public static void OfferHouseSell(ExtPlayer player, ExtPlayer target, int price)
        {
            if (player.Position.DistanceTo(target.Position) > 5)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_119", 3000);
                return;
            }
            House house = GetHouse(player, true);
            if (house == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_21", 3000);
                return;
            }
            if (house.Pledged)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_147", 3000);
                return;
            }
            if (GetHouse(target, true) != null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_120", 3000);
                return;
            }
            if (price > 1000000000 || price < house.Price / 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_121", 3000);
                return;
            }
            if (player.Position.DistanceTo(house.Position) > 30)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_122", 3000);
                return;
            }

            DialogUI.Open(target, "House_123".Translate(player.Character.UUID, price), new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "gui_727",
                    Icon = "confirm",
                    Action = (p) => acceptHouseSell(p, player, price)
                },
                new DialogUI.ButtonSetting
                {
                    Name = "gui_728",
                    Icon = "cancel",
                    Action = (p) => { }
                }
            });
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "House_124".Translate( target.Character.UUID, price), 3000);
        }

        public static void OfferFamilyHouseSell(ExtPlayer player, ExtPlayer target, int price)
        {
            if (player.Position.DistanceTo(target.Position) > 5)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_119", 3000);
                return;
            }
            House house = GetHouseFamily(player);
            if (house == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У Вашей семьи нет дома.", 3000);
                return;
            }
            if (house.Pledged)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_147", 3000);
                return;
            }
            if (!house.GetAccessFurniture(player, FamilyFurnitureAccess.MovingFurniture))
            {
                Notify.SendInfo(player, "House_142");
                return;
            }
            if (target.Character.FamilyID == 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Чтобы продать человеку семейный дом - он должен находиться в семье.", 3000);
                return;
            }
            if (GetHouseFamily(target) != null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У семьи человека уже есть семейный дом.", 3000);
                return;
            }
            if (price > 1000000000 || price < house.Price / 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_121", 3000);
                return;
            }
            if (player.Position.DistanceTo(house.Position) > 30)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы далеко от семейного дома", 3000);
                return;
            }

            DialogUI.Open(target, $"{player.Name} ({player.Character.UUID}) предложил Вам купить дом для семьи за ${price}.", new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "gui_727",
                    Icon = "confirm",
                    Action = (p) => acceptFamilyHouseSell(p, player, price)
                },
                new DialogUI.ButtonSetting
                {
                    Name = "gui_728",
                    Icon = "cancel",
                    Action = (p) => { }
                }
            });
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "House_124".Translate(target.Character.UUID, price), 3000);
        }

        public static void acceptHouseSell(ExtPlayer player, ExtPlayer seller, int price)
        {
            if (!player.IsLogged() || !seller.IsLogged()) return;
            if (GetHouse(player, true) != null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_125", 3000);
                return;
            }

            House house = GetHouse(seller, true);
            if (house == null) return;
            if (!MoneySystem.Wallet.TransferMoney(player.Character, seller.Character, price, 0, $"Покупка дома ({house.ID})"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_126", 3000);
                return;
            }
            house.RemoveRoommates();
            SafeTrigger.ClientEvent(seller, "deleteCheckpoint", 333);
            SafeTrigger.ClientEvent(seller, "deleteGarageBlip");
            house.SetOwner(player.Character.UUID, 0);

            MainMenu.SendProperty(player);
            MainMenu.SendProperty(seller);

            Notify.Send(seller, NotifyType.Info, NotifyPosition.BottomCenter, "House_127".Translate(player.Character.UUID), 3000);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "House_128".Translate( seller.Character.UUID), 3000);
        }

        public static void acceptFamilyHouseSell(ExtPlayer player, ExtPlayer seller, int price)
        {
            if (!player.IsLogged() || !seller.IsLogged()) return;
            if (player.Character.FamilyID == 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Для покупки семейного дома Вы должны состоять в семье.", 3000);
                return;
            }
            if (GetHouseFamily(player) != null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_125", 3000);
                return;
            }

            House house = GetHouseFamily(seller);
            if (house == null) return;
            if (!house.GetAccessFurniture(seller, FamilyFurnitureAccess.MovingFurniture))
            {
                Notify.SendInfo(seller, "У Вас нет доступа для продажи дома.");
                return;
            }
            if (!MoneySystem.Wallet.TransferMoney(player.Character, seller.Character, price, 0, $"Продажа дома ({house.ID})"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_126", 3000);
                return;
            }
            house.RemoveRoommates();
            SafeTrigger.ClientEvent(seller, "deleteCheckpoint", 333);
            SafeTrigger.ClientEvent(seller, "deleteGarageBlip");
            house.SetOwner(player.Character.FamilyID, OwnerType.Family);

            MainMenu.SendProperty(player);
            MainMenu.SendProperty(seller);

            Notify.Send(seller, NotifyType.Info, NotifyPosition.BottomCenter, $"{player.Character.UUID} успешно купил Ваш семейный дом.", 3000);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы успешно купили дом для семьи у {seller.Character.UUID}", 3000);
        }
    }
}
