using System.Collections.Generic;
using System.Data;
using System;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using Whistler.GUI;
using Whistler.Core.Character;
using System.Linq;
using Newtonsoft.Json;
using Whistler.ClothesCustom;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;
using Whistler.Helpers;
using Whistler.VehicleSystem.Models.VehiclesData;
using Whistler.Fractions.PDA;
using Whistler.Inventory;
using Whistler.Inventory.Models;
using System.Threading.Tasks;
using Whistler.Fractions.GOV;
using Whistler.Fractions.Models;
using Whistler.Families.FamilyMenu;
using Whistler.Families.WarForCompany;
using Whistler.Domain.Phone.Bank;
using Whistler.MoneySystem;
using Whistler.Common;
using Whistler.Families.Models;
using Whistler.Entities;

namespace Whistler.Fractions
{
    class Manager : Script
    { // Revision 3.0
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Manager));
        private static Dictionary<int, Fraction> _fractions = new Dictionary<int, Fraction>();

        public static readonly int MAX_MONEY_SUB_IN_DAY = 5000000;
        public static readonly int MAX_MONEY_SUB_LEADER_IN_DAY = 9000000;

        //Dimension no change!!!
        private static Dictionary<int, FractionGarage> _fractionGarages = new Dictionary<int, FractionGarage>()
        {
            {0, new FractionGarage(new Vector3(-580.2, -123.0, 32.75), new Vector3(-584.9395, -126.4, 33.7), new Vector3(0, 0, 204), FractionGarage.GarageTypes.Garage50Place, 6, 555777) },
            {1, new FractionGarage(new Vector3(534.4, -26.5, 69.7), new Vector3(530.8, -30.6, 70.6), new Vector3(0, 0, 210), FractionGarage.GarageTypes.Garage50Place, 7, 555778) },
            {2, new FractionGarage(new Vector3(169.4, -697.5, 32.2), new Vector3(156.2, -692.4, 32.1), new Vector3(0, 0, 160), FractionGarage.GarageTypes.Garage50Place, 9, 555779) },
            {3, new FractionGarage(new Vector3(-2412.4, 3334, 31.9), new Vector3(-2418.6, 3322.7, 31.8), new Vector3(0, 0, 240), FractionGarage.GarageTypes.Garage50Place, 14, 555780) },
            {4, new FractionGarage(new Vector3(338.2, -561, 27.75), new Vector3(343.5, -559.6, 27.75), new Vector3(0, 0, -17), FractionGarage.GarageTypes.Garage50Place, 8, 555781) },
            //{1, new FractionGarage(new Vector3(), new Vector3(), new Vector3(82.54144, 76.12762, 2.644107), 7, 555778) },
        };
        public static bool FractionsReady { get; private set; } = false;

        [ServerEvent(Event.ResourceStart)]
        public void fillStocks()
        {
            try
            {
                var result = MySQL.QueryRead("SELECT * FROM fractions");
                if (result == null || result.Rows.Count == 0)
                {
                    return;
                }

                foreach (DataRow Row in result.Rows)
                {
                    var data = new Fraction(Convert.ToInt32(Row["id"]), Convert.ToInt32(Row["money"]), Convert.ToInt32(Row["fuellimit"]), Convert.ToInt32(Row["fuelleft"]));
                    _fractions.Add(data.Id, data);
                }
                FractionsReady = true;
            }
            catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
        }

        public static void SaveStocksDic()
        {
            try
            {
                foreach (var fraction in _fractions.Values)
                {
                    fraction.Save();
                }
            }
            catch (Exception e) { _logger.WriteError("saveStocksDic: " + e.ToString()); }
        }

        public static void UpdateMoneyLimit()
        {
            foreach (var frac in _fractions)
            {
                frac.Value.MoneyLimit = 0;
            }
        }
        public static void UpdateFuelLimit()
        {
            _fractions[6].FuelLeft =  _fractions[6].FuelLimit; // city
            _fractions[7].FuelLeft =  _fractions[7].FuelLimit; // police
            _fractions[8].FuelLeft =  _fractions[8].FuelLimit; // fib
            _fractions[9].FuelLeft =  _fractions[9].FuelLimit; // ems
            _fractions[14].FuelLeft = _fractions[14].FuelLimit; // army
        }


        public static Fraction GetFraction(Player player)
        {
            return _fractions.GetValueOrDefault(player.GetCharacter()?.FractionID ?? 0);
        }
        public static Fraction GetFraction(int fracId)
        {
            return _fractions.GetValueOrDefault(fracId);
        }
        public static Fraction GetFractionByUUID(int uuid)
        {
            return _fractions.FirstOrDefault(item => item.Value.Members.ContainsKey(uuid)).Value;
        }

        public static void PayFractionMoneyForEnterprise()
        {
            foreach (var frac in _fractions)
            {
                if (frac.Value.MoneyForEnterprise > 0)
                {
                    Wallet.MoneyAdd(frac.Value, frac.Value.MoneyForEnterprise, "Money_CompanyOwn");
                    frac.Value.MoneyForEnterprise = 0;
                }
            }
        }

        public static void onResourceStart()
        {
            try
            {
                /*
                //NAPI.Blip.CreateBlip(491, Government.BlipPosition, 1.3f, 75, Main.StringToU16("Government"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(661, Manager.FractionSpawns[16], 1, 49, Main.StringToU16("The Lost"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(564, LSNewsManager.LSNewsCoords[0], 1, 4, Main.StringToU16("News"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(419, new Vector3(-550.1906, -195.2256, 37.10304), 1.5f, 14, Main.StringToU16("Goverment"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(419, new Vector3(2474, -384, 94), 1.5f, 12, Main.StringToU16("Parliament"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(605, Cityhall.CityhallChecksCoords[1], 1.5f, 0, Main.StringToU16("Bank"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(305, Ems.emsCheckpoints[0], 1, 49, Main.StringToU16("Hospital"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(88, new Vector3(101.5185, -744.3046, 45.47476), 1, 58, Main.StringToU16("FIB"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(526, Police.policeCheckpoints[1], 1, 38, Main.StringToU16("Police"), 255, 0, true, 0, 0);
                //NAPI.Blip.CreateBlip(85, Army.ArmyCheckpoints[2], 1, 28, Main.StringToU16("Docks"), 255, 0, true, 0, 0);

                NAPI.Blip.CreateBlip(84, Manager.FractionSpawns[1], 1, 52, Main.StringToU16("The Families"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(84, Manager.FractionSpawns[2], 1, 58, Main.StringToU16("The Ballas"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(84, Manager.FractionSpawns[3], 1, 28, Main.StringToU16("Los Santos Vagos"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(84, Manager.FractionSpawns[4], 1, 74, Main.StringToU16("Marabunta Grande"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(84, Manager.FractionSpawns[5], 1, 49, Main.StringToU16("Blood Street"), 255, 0, true, 0, 0);
                */

                //NAPI.Blip.CreateBlip(491, Government.BlipPosition, 1.3f, 75, Main.StringToU16("Government"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(661, Manager.FractionSpawns[16], 1, 49, Main.StringToU16("The Lost"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(564, LSNewsManager.LSNewsCoords[0], 1, 4, Main.StringToU16("Новости"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(419, new Vector3(-550.1906, -195.2256, 37.10304), 1.5f, 14, Main.StringToU16("Правительство"), 255, 0, true, 0, 0);
                
                NAPI.Blip.CreateBlip(605, Cityhall.CityhallChecksCoords[1], 1, 0, Main.StringToU16("Банк"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(305, Ems.emsCheckpoints[0], 1, 49, Main.StringToU16("Госпиталь"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(88, new Vector3(101.5185, -744.3046, 45.47476), 1, 58, Main.StringToU16("FIB"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(526, Police.policeCheckpoints[1], 1, 38, Main.StringToU16("LSPD"), 255, 0, true, 0, 0);
                //NAPI.Blip.CreateBlip(85, Army.ArmyCheckpoints[2], 1, 28, Main.StringToU16("Docks"), 255, 0, true, 0, 0);

                NAPI.Blip.CreateBlip(84, Manager.FractionSpawns[1], 1, 52, Main.StringToU16("The Families"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(84, Manager.FractionSpawns[2], 1, 58, Main.StringToU16("The Ballas"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(84, Manager.FractionSpawns[3], 1, 28, Main.StringToU16("Los Santos Vagos"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(84, Manager.FractionSpawns[4], 1, 74, Main.StringToU16("Marabunta Grande"), 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(84, Manager.FractionSpawns[5], 1, 49, Main.StringToU16("Blood Street"), 255, 0, true, 0, 0);

                LoadGarageFraction();
                LoadClothesPoint();
            }
            catch (Exception e)
            {
                _logger.WriteError($"onResourceStart:\n {e}");
            }
        }

        public static SortedList<int, Vector3> FractionSpawns = new SortedList<int, Vector3>()
        {
            {1, new Vector3(122.7856, -1974.354, 21.31973)},    // The Families
            {2, new Vector3(-53.28416, -1389.858, 29.32066)},     // The Ballas Gang
            {3, new Vector3(977.5455, -1793.983, 31.32506)},     // Los Santos Vagos
            {4, new Vector3(1565.976, -2127.869, 78.31924)},     // Marabunta Grande
            {5, new Vector3(878.0268, -2216.593, 30.64049)},     // Blood Street
            {6, new Vector3(-573.8327, -197.5951, 47.38987)},      // Cityhall
            {7, new Vector3(596.0463, -13.40078, 82.74032)},      // LSPD police
            {8, new Vector3(322.2866, -595.14, 43.09046)},      // Emergency care
            {9, new Vector3(109.8975, -753.0673, 242.1521)},     // FBI 
            {10, new Vector3(-1884.8302, 2058.7583, 140.5)},     // La Cosa Nostra 
            {11, new Vector3(1401.0343, 1132.0896, 113.21363)},    // mexican 
            {12, new Vector3(-107.1426, 990.8696, 234.6603)},    // Yakuza 
            {13, new Vector3(2456.746, 4979.63, 45.6903)},    // ADMINS
            {14, new Vector3(-2357.276, 3251.052, 32.81071)},    // Army
            {15, new Vector3(-1063.046, -249.463, 44.0211)},    // LSNews
            {16, new Vector3(1987, 3050, 47.2)},    // The Lost
            {17, new Vector3(-442.969, 6016.779, 30.59221)},    // Government
            // {18, new Vector3(2467.687, -384.1666, 94.38915)},    //REFEREE
        };

        public static SortedList<int, int> FractionTypes = new SortedList<int, int>() // 0 - mafia, 1 gangs, 2 - gov, 
        {
            {-1, -1},
            {0, -1},
            {1, 1}, // The Families
            {2, 1}, // The Ballas Gang
            {3, 1},  // Los Santos Vagos
            {4, 1}, // Marabunta Grande
            {5, 1}, // Blood Street
            {6, 2}, // Cityhall
            {7, 2}, // LSPD police
            {8, 2}, // Emergency care
            {9, 2}, // FBI 
            {10, 0}, // La Cosa Nostra 
            {11, 0}, // Russian Mafia
            {12, 0}, // Yakuza 
            {13, 0}, // Armenian Mafia 
            {14, 2}, // Army
            {15, 2}, // News
            {16, 1}, // The Lost
            {17, 2}, // Government
            //{18, 2 }, //REFEREE
        };


        public static Dictionary<int, int> GovIds = new Dictionary<int, int>
        {
            { 7, 14 },
            { 6, 8 },
            { 8, 11 },
            { 9, 14 },
            { 14, 15 },
            { 15, 16 },
            { 17, 15 },
            //{ 18, 13 }
        };

        private static Dictionary<int, Vector3> ClothesPoint = new Dictionary<int, Vector3>
        {
            { 1, new Vector3(113.0714, -1969.851, 20.31973) },
            { 2, new Vector3(-70.39008, -1388.968, 28.32061) },
            { 3, new Vector3(976.78, -1791.86, 30.32) },
            { 4, new Vector3(0, 0, 0) },
            { 5, new Vector3(866.0859, -2215.121, 29.6405) },
            { 6, null },
            { 7, null },
            { 8, null },
            { 9, null },
            { 10, null },
            { 11, null },
            { 12, null },
            { 13, new Vector3(2454.708, 4971.838, 47.8103) },
            { 14, null },
            { 15, new Vector3(-1081.789, -247.9009, 43.02136) },
            { 16, new Vector3(1991.8, 3041.3, 46.2) },
            { 17, null },
            //{ 18, null },
        };


        public static void InvitePlayerToFraction(Player player, Fraction fraction, int rank = 1)
        {
            if (!player.IsLogged())
                return;
            fraction.NewMember(player.GetCharacter().UUID, rank);
            LoadFraction(player);
        }
        public static void LoadFraction(Player player)
        {
            var fraction = GetFraction(player);
            if (fraction != null)
            {
                player.SetSharedData("fraction", fraction.Id);
                GangsCapture.SetMemberInGangTeam(player, 0);
                FractionSubscribeSystem.Subscribe(player, fraction.Id);
                fraction.ConnectedMember(player.GetCharacter().UUID, player);
                WarCompanyManager.LoadAllWars(player);
                UpdateFracData(player);
                if (fraction.OrgActiveType == OrgActivityType.Crime)
                    GangsCapture.LoadBlips(player);
            }
            else
            {
                player.SetSharedData("fraction", 0);
            }
        }
        public static void UnloadFraction(Player player)
        {
            var fraction = GetFraction(player);
            if (fraction != null)
            {
                fraction.DisconnectedMember(player.GetCharacter().UUID);
                FractionSubscribeSystem.UnSubscribe(player);
            }
        }

        public static int countOfFractionMembers(int fracID)
        {
            return GetFraction(fracID)?.OnlineMembers.Count ?? 0;
        }
        public static bool isHaveFraction(Player player)
        {
            return player.GetCharacter().FractionID != 0;
        }
        public static bool inFraction(Player player, int FracID)
        {
            return player.GetCharacter().FractionID == FracID;
        }
        public static bool isLeader(Player player, int fracID = 0)
        {
            if (fracID != 0)
            {
                return GetFraction(fracID).IsLeader(player);
            }
            return GetFraction(player)?.IsLeader(player) ?? false;
        }
        public static bool IsLeaderOrSub(Player player, int fracID = 0)
        {
            if (fracID != 0)
            {
                return GetFraction(fracID).IsLeaderOrSub(player);
            }
            return GetFraction(player)?.IsLeaderOrSub(player) ?? false;
        }

        public static string getName(int FractionID)
        {
            return Configs.GetConfigOrDefault(FractionID).Name;
        }

        public static bool CanUseFunctionality(Player player)
        {
            bool onDuty = player.GetCharacter().OnDuty;

            int pFraction = player.GetCharacter().FractionID;
            bool specialAccess = (pFraction == 7 || pFraction == 9) && player.GetCharacter().FractionLVL >= 9;

            return onDuty || specialAccess;
        }

        public static bool CanUseCommand(Player player, string command, bool notify = true)
        {
            if (!player.IsLogged()) 
                return false;
            // Get player FractionID
            int fracid = player.GetCharacter().FractionID;
            int fraclvl = player.GetCharacter().FractionLVL;
            int minrank = 100;
            // Fractions available commands //
            if (_fractions.ContainsKey(fracid) && _fractions[fracid].Commands.ContainsKey(command))
                minrank = _fractions[fracid].Commands[command];

            if (fraclvl < minrank)
            {
                if (notify)
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_326", 3000);
                return false;
            }
            else return true;
        }

        public static string getNickname(int fracID, int fracLvl)
        {
            return GetFraction(fracID)?.GetRankForLevel(fracLvl)?.RankName ?? "Unknown";
        }

        public static bool IsMafia(Player player)
        {
            if ((player.GetCharacter().FractionID > 0 & player.GetCharacter().FractionID < 6) || (player.GetCharacter().FractionID > 9 & player.GetCharacter().FractionID < 14))
            {
                return true;
            }
            return false;
        }

        public static bool IsGang(Player player)
        {
            if ((player.GetCharacter().FractionID > 0 && player.GetCharacter().FractionID < 6))
                return true;
            return false;
        }

        public static bool IsSilovic(Player player)
        {
            return new[] { 7, 9, 14 }.ToList().Contains(player.GetCharacter().FractionID);
        }

        public static bool IsGovFraction(Player player)
        {
            return new[] { 6, 7, 8, 9, 14, 15, 17 }.ToList().Contains(player.GetCharacter().FractionID);
        }

        public static bool IsPoliceAndFIB(Player player)
        {
            return new[] { 7, 9 }.ToList().Contains(player.GetCharacter().FractionID);
        }

        public static bool IsMedic(Player player)
        {
            return player.GetCharacter().FractionID == 8;
        }

        public static void enterInterier(Player player, int fractionId)
        {
            var position = (fractionId <= 5) ? Gangs.ExitPoints[fractionId] : Mafia.ExitPoints[fractionId];
            player.ChangePosition(position + new Vector3(0, 0, 1.12));
            Main.PlayerEnterInterior(player, position + new Vector3(0, 0, 1.12));

            Trigger.ClientEvent(player, "stopTime");
        }

        public static void exitInterier(Player player, int fractionId)
        {
            var position = (fractionId <= 5) ? Gangs.EnterPoints[fractionId] : Mafia.EnterPoints[fractionId];
            player.ChangePosition(position + new Vector3(0, 0, 1.12));
            Main.PlayerEnterInterior(player, position + new Vector3(0, 0, 1.12));

            Trigger.ClientEvent(player, "resumeTime");
        }

        private class FractionMemberData
        {
            public int id { get; set; }
            public string username { get; set; }
            public int rank { get; set; }
        }
      
        public static void UpdateFracData(Player player)
        {
            try
            {
                Character acc = player.GetCharacter();
                var frac = GetFraction(acc.FractionID);
                if (frac == null) 
                {
                    Trigger.ClientEvent(player, "mmenu:frac:data:update", acc.FractionID, false, false, false, false);
                }
                else
                {
                    var fMembers = frac.Members.Select(item => new FractionMemberData
                    {
                        id = Main.GetPlayerByUUID(item.Value.PlayerUUID)?.Value ?? -1,
                        rank = item.Value.Rank,
                        username = Main.PlayerNames.GetValueOrDefault(item.Value.PlayerUUID)
                    }).ToList();
                    Trigger.ClientEvent(player, "mmenu:frac:data:update",
                        frac.Id,
                        frac.Money,
                        frac.LastHour,
                        frac.LastDay,
                        CanUseCommand(player, "invite", false),
                        CanUseCommand(player, "uninvite", false),
                        CanUseCommand(player, "setrank", false),
                        CanUseCommand(player, "takemoney", false),
                        isLeader(player, frac.Id)
                    );
                }
               
            }
            catch (Exception e)
            {
                _logger.WriteError($"UpdateFracMembers:\n {e}");
            }
        }

        public static void UpdateFracMembers(Player player)
        {
            try
            {
                Character acc = player.GetCharacter();
                var frac = GetFraction(acc.FractionID);
                if (frac == null) return;

                var fMembers = frac.Members.Select(item => new FractionMemberData
                {
                    id = Main.GetPlayerByUUID(item.Value.PlayerUUID)?.Value ?? -1,
                    rank = item.Value.Rank,
                    username = Main.PlayerNames.GetValueOrDefault(item.Value.PlayerUUID, "Unknown")
                }).ToList();
                var res = JsonConvert.SerializeObject(fMembers);
                Trigger.ClientEvent(player, "mmenu:frac:members:update", res);
            }
            catch (Exception e)
            {
                _logger.WriteError($"UpdateFracMembers:\n {e}");
            }
        }

        [RemoteEvent("openfmenu")]
        public static void OpenFractionMenu(Player player)
        {
            UpdateFracData(player);
            UpdateFracMembers(player);
        }

        [RemoteEvent("mmenu:frac:access:request")]
        public static void RequestFractionAccess(Player player)
        {
            try
            {
                var fId = player.GetCharacter().FractionID;
                if (!isLeader(player, fId)) return;
                player.TriggerEvent("mmenu:frac:access:set", JsonConvert.SerializeObject(_fractions[fId].Commands));
            }
            catch (Exception e)
            {
                _logger.WriteError($"RequestFractionAccess:\n {e}");
            }
            
        }

        [RemoteEvent("mmenu:frac:access:change")]
        public static void ChangeFractionAccess(Player player,string key, int val)
        {
            try
            {
                var character = player.GetCharacter();
                if (!isLeader(player, character.FractionID)) return;
                if(character.FractionLVL >= val)
                {
                    _fractions[character.FractionID].ChangeCommandAccess(key, val);
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "mmain:frac:access:success", 3000);
                    player.TriggerEvent("mmenu:frac:access:set", JsonConvert.SerializeObject(_fractions[character.FractionID].Commands));
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"ChangeFractionAccess:\n {e}");
            }

        }

        [RemoteEvent("fmenu:kick")]
        public static void FractionKick(PlayerGo player, string name)
        {
            try
            {
                if (!Main.PlayerNames.ContainsValue(name))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_342", 3000);
                    return;
                }

                if (!Manager.CanUseCommand(player, "uninvite"))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_69", 3000);
                    return;
                };
                int uuid = Main.PlayerNames.FirstOrDefault(item => item.Value == name).Key;
                if (FractionCommands.UnInviteFromFraction(player, uuid))
                {
                    var target = Main.GetPlayerByUUID(uuid);
                    if (target != null)
                        Notify.Send(target, NotifyType.Warning, NotifyPosition.BottomCenter, "Frac_218".Translate(Manager.getName(player.GetCharacter().FractionID)), 3000);
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_219".Translate(name), 3000);
                    UpdateFracMembers(player);
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"FractionKick:\n {e}");
            }
            
        }

        [RemoteEvent("fmenu:rank")]
        public static void FractionRank(PlayerGo player, string name, int rank)
        {
            try
            {
                if (rank <= 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_202", 3000);
                    return;
                }

                if (!Main.PlayerNames.ContainsValue(name))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_342", 3000);
                    return;
                }

                if (!CanUseCommand(player, "setrank") || rank >= player.GetCharacter().FractionLVL)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_203", 3000);
                    return;
                }

                int uuid = Main.PlayerNames.FirstOrDefault(item => item.Value == name).Key;

                if (FractionCommands.SetFracRank(player, uuid, rank))
                {
                    var target = Main.GetPlayerByUUID(uuid);
                    var nameRank = Manager.getNickname(Manager.GetFraction(player).Id, rank);
                    if (target != null)
                        Notify.Send(target, NotifyType.Warning, NotifyPosition.BottomCenter, "Frac_205".Translate(nameRank), 3000);
                    Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Frac_206".Translate(name, nameRank), 3000);

                    UpdateFracMembers(player);
                }


            }
            catch (Exception e)
            {
                _logger.WriteError($"FractionRank:\n {e}");
            }
        }

        [RemoteEvent("fmenu:invite")]
        public static void FractionInvite(PlayerGo player, int id)
        {
            try
            {
                if (!Manager.CanUseCommand(player, "invite"))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_69", 3000);
                    return;
                }

                var target = NAPI.Pools.GetAllPlayers().FirstOrDefault(p => p.Value == id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_342", 3000);
                    return;
                }
                FractionCommands.InviteToFraction(player, target as PlayerGo);

                UpdateFracMembers(player);

            }
            catch (Exception e)
            {
                _logger.WriteError($"FractionInvite:\n {e}");
            }
        }

        [RemoteEvent("fmenu:money:withdraw")]
        public static bool FractionWithdrawMoney(Player player, int amount)
        {
            try
            {
                if (!CanUseCommand(player, "takemoney"))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_69", 3000);
                    return false;
                }
                var fraction = player.GetFraction();
                if (player.GetCharacter().AdminLVL < 5)
                {
                    if (Manager.isLeader(player))
                    {
                        if (fraction.MoneyLimit + amount > Manager.MAX_MONEY_SUB_LEADER_IN_DAY)
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_546".Translate(Manager.MAX_MONEY_SUB_LEADER_IN_DAY, Manager.MAX_MONEY_SUB_LEADER_IN_DAY - fraction.MoneyLimit), 3000);
                            return false;
                        }
                    }
                    else
                    {
                        if (fraction.MoneyLimit + amount > Manager.MAX_MONEY_SUB_IN_DAY)
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_546".Translate(Manager.MAX_MONEY_SUB_IN_DAY, Manager.MAX_MONEY_SUB_IN_DAY - fraction.MoneyLimit), 3000);
                            return false;
                        }
                    }
                }
                if (Wallet.TransferMoney(fraction, player.GetBankAccount(), amount, 0, "Money_FracWithdraw", player.GetCharacter().AdminLVL < 5))
                {
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "mmain:frac:wmoney:succ".Translate(amount), 3000);
                    Manager.UpdateFracData(player);
                    return true;
                }
                else
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "mmain:frac:dmoney:err", 3000);
                return false;

            }
            catch (Exception e)
            {
                _logger.WriteError($"FractionWithdrowMoney:\n {e}");
                return false;
            }
        }

        [RemoteEvent("fmenu:money:deposit")]
        public static bool FractionDepositMoney(Player player, int amount)
        {
            try
            {
                if (MoneySystem.Wallet.TransferMoney(player.GetBankAccount(), player.GetFraction(), amount, 0, "Money_FracDeposit"))
                {
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "mmain:frac:dmoney:succ".Translate(amount), 3000);
                    Manager.UpdateFracData(player);
                    return true;
                }
                else
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "mmain:frac:dmoney:err", 3000);
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.WriteError($"FractionDepositMoney:\n {e}");
                return false;
            }
        }

        private static void LoadClothesPoint()
        {
            foreach (var point in ClothesPoint)
            {
                if (point.Value != null)
                {
                    InteractShape.Create(point.Value, 1, 2)
                        .AddDefaultMarker(Configs.GetConfigOrDefault(point.Key).FracColor)
                        .AddInteraction((player) =>
                        {
                            interactPressedClothes(player, point.Key);
                        }, "interact_3");
                }
            }
        }

        private static void LoadGarageFraction()
        {
            foreach(var garage in _fractionGarages)
            {
                InteractShape.Create(garage.Value.EnterPoint, 3, 2)
                    .AddMarker(27, garage.Value.EnterPoint, 3, InteractShape.DefaultMarkerColor)
                    .AddInteraction((player) =>
                    {
                        interactPressedGarage(player, true, garage.Value);
                    }, "interact_21");
                InteractShape.Create(garage.Value.GaragePosition, 2, 2, garage.Value.Dimension)
                    .AddInteraction((player) =>
                    {
                        interactPressedGarage(player, false, garage.Value);
                    }, "interact_22");
            }
        }
        private static void interactPressedGarage(Player player, bool enter, FractionGarage garage)
        {
            int playerFrac = player.GetCharacter().FractionID;
            if (playerFrac == garage.Fraction || playerFrac == 14 || player.GetCharacter().AdminLVL > 0)
                if (enter)
                {
                    SendPlayerToCoord(player, garage.GaragePosition, garage.GarageRotation, garage.Dimension);
                    player.GetCharacter().ExteriorPos = garage.ExitPoint;
                }
                else
                {
                    SendPlayerToCoord(player, garage.ExitPoint, garage.ExitRotation, 0);
                    player.GetCharacter().ExteriorPos = null;
                }
        }

        private static void SendPlayerToCoord(Player player, Vector3 pos, Vector3 rotation, uint dim)
        {
            var vehicle = player.Vehicle;
            if (vehicle == null)
            {
                player.Dimension = dim;
                player.ChangePosition(pos + new Vector3(0, 0, 0.12));
            }
            if (vehicle != null && player.VehicleSeat == VehicleConstants.DriverSeat)
            {
                var vehData = vehicle.GetVehicleGo().Data;
                if (vehData.OwnerType != OwnerType.Fraction)
                    return;
                vehicle.CustomDelete();
                WhistlerTask.Run(() =>
                {
                    var newVeh = vehData.Spawn(pos + new Vector3(0, 0, 0.12), rotation, dim);
                    player.CustomSetIntoVehicle(newVeh, VehicleConstants.DriverSeatClientSideBroken);
                }, 1000);
                
            }
        }

        private static void interactPressedClothes(Player player, int fraction)
        {
            if (player.GetCharacter().FractionID == fraction)
                SkinManager.OpenSkinMenu(player);
            else
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_522", 3000);
        }
    }

}
