using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Data;
using Whistler.GUI;
using Whistler.SDK;
using Newtonsoft.Json;
using Whistler.Houses;
using ServerGo.Casino.ChipModels;
using Whistler.Businesses.Manager.DTOs;
using Whistler.Helpers;
using Whistler.VehicleSystem;
using Whistler.Fractions;
using Whistler.VehicleSystem.Models;
using Whistler.Core.Admins;
using Whistler.Inventory.Enums;
using Whistler.Inventory;
using Whistler.Possessions;
using Whistler.MoneySystem;
using Whistler.Families;
using Whistler.Fractions.PDA;
using Whistler.NewDonateShop;
using Whistler.GUI.Documents.Enums;
using Whistler.Phone;
using Whistler.Inventory.Models;
using Whistler.MoneySystem.Interface;
using Whistler.MoneySystem.Models;
using Whistler.Common;
using Whistler.Entities;

namespace Whistler.Core
{
    public delegate void SetPlayerToAdminGroupDelegate(PlayerGo player);
    public delegate void DeletePlayerFromAdminGroupDelegate(PlayerGo player);

    class Admin : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Admin));
        public static bool IsServerStoping = false;

        public static SetPlayerToAdminGroupDelegate SetPlayerToAdminGroup;
        public static DeletePlayerFromAdminGroupDelegate DeletePlayerFromAdminGroup;

        [Command("setsendexceptionstatus")]
        public void CreateTeleportPoint(PlayerGo player, int value)
        {
            if ((player?.Character?.AdminLVL ?? 0) < 10) return;
            Main.ServerConfig.Main.SendClientExceptions = value != 0;
            NAPI.ClientEvent.TriggerClientEventForAll("SendClientExceptions", Main.ServerConfig.Main.SendClientExceptions);
        }

        [Command("testid")]
        public void testID(PlayerGo player, int id)
        {
            player.TriggerEvent("toggleTestInv", id);
        }

        [Command("testcam")]
        public void testcam(PlayerGo player, int id)
        {
            NAPI.Util.ConsoleOutput($"{player.Character.AdminLVL}");
            var target = Main.GetPlayerByID(id);
            if (target == null) return;
            player.SendChatMessage($"{player.isFriend(target)}");
        }

        [Command("createtp")]
        public void CreateTeleportPoint(PlayerGo player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "createtp")) return;

                if (!player.HasData("TELEPORT:CREATING"))
                {
                    player.SetData("TELEPORT:CREATING", new Tuple<Vector3, uint>(player.Position - new Vector3(0, 0, 1), player.Dimension));
                    player.SendChatMessage("create:tp:1");
                }
                else
                {
                    var enterPoint = player.GetData<Tuple<Vector3, uint>>("TELEPORT:CREATING");

                    Teleports.CreateTeleport(enterPoint.Item1, enterPoint.Item2, player.Position - new Vector3(0, 0, 1), player.Dimension);
                    player.ResetData("TELEPORT:CREATING");

                    player.SendChatMessage("create:tp:2");
                }
            }
            catch (Exception e) { _logger.WriteError($"CreateTeleportPoint:\n{e}"); }
        }
        [Command("deletetp")]
        public void DeleteTeleportPoint(PlayerGo player)
        {
            try
            {
                Teleports.DeleteTeleport(player);
            }
            catch (Exception e) { _logger.WriteError($"DeleteTeleportPoint:\n{e}"); }
        }

        [Command("hpf")]
        public void ResetLifeParameters(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "hpf")) return;

                var target = Main.GetPlayerByID(id);
                if (target == null) return;

                var character = target.GetCharacter();
                character.LifeActivity.Hunger.Level = 100;
                character.LifeActivity.Thirst.Level = 100;
                character.LifeActivity.Rest.Level = 100;

                target.Health = 100;
            }
            catch (Exception e) { _logger.WriteError($"ResetLifeParameters:\n{e}"); }
        }

        [Command("ohv")]
        public void ShowAdminHuntingVision(PlayerGo player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "ohv")) return;

                player.TriggerEvent("hunting:toggleAdminVision");
            }
            catch (Exception e) { _logger.WriteError($"ShowAdminHuntingVision:\n{e}"); }
        }

        [RemoteEvent("saveObjectPos")]
        public void SaveObjectPosition(PlayerGo player, string pos, string rotation, string camPos)
        {
            using (var w = new StreamWriter("ObjectPos.txt", true))
            {
                w.WriteLine($"pos: {pos} rot: {rotation} gameplayCam: {camPos}");
            }
        }

        internal static void CheckInventory(PlayerGo player, PlayerGo target)
        {
            var inventory = target.GetInventory()?.GetInventoryData();
            var equip = target.GetEquip()?.GetEquipData();
            player.TriggerEvent("admin:checkinventory:responce", equip, inventory);
        }

        [ServerEvent(Event.ResourceStart)]
        public void Event_ResourceStart()
        {
            ColShape colShape = NAPI.ColShape.CreateCylinderColShape(DemorganPosition, DemorganRange, DemorganHeight, uint.MaxValue);
            colShape.OnEntityExitColShape += (s, e) =>
            {
                if (!e.IsLogged()) return;
                if (e.GetCharacter().DemorganTime > 0)
                {
                    e.Dimension = 1337;
                    e.SendTODemorgan();
                }
            };
            Group.LoadCommandsConfigs();
        }

        [RemoteEvent("invisible")]
        public static void SetInvisible(PlayerGo player, bool toggle)
        {
            try
            {
                if (player.GetCharacter().AdminLVL == 0) return;
                player.SetSharedData("INVISIBLE", toggle);

                player.Transparency = toggle ? 255 : 0;
            }
            catch (Exception e) { _logger.WriteError($"InvisibleEvent:\n{e}"); }
        }

        //[Command("setvehicledamagemodifier")]
        //public void setVehicleDamageModifier(PlayerGo player, double value)
        //{
        //    player.Eval($"mp.players.local.setVehicleDamageModifier({value});");
        //}

        //[Command("setvehicledefensemodifier")]
        //public void ShowBusinessMarkers(PlayerGo player, double value)
        //{
        //    player.Eval($"mp.players.local.setVehicleDefenseModifier({value});");
        //}

        [Command("getanimalspos")]
        public void GetAnimalsPosition_ADMINCMD(PlayerGo player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "getanimalspos")) return;

                var huntingGround = Jobs.Hunting.Work.GetHuntingGroundWithPlayer(player);
                if (huntingGround == null) return;

                Chat.SendTo(player, "ANIMALS POSITIONS:");
                var animalsPositions = huntingGround.GetAnimalsPositions();
                foreach (var animalPosition in animalsPositions)
                {
                    Chat.SendTo(player, $"Animal on {animalPosition}");
                }
            }
            catch (Exception e) { _logger.WriteError($"GetAnimalsPosition_ADMINCMD:\n{e}"); }
        }

        [Command("bizshowmarkers")]
        public void ShowBusinessMarkers(PlayerGo player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "bizshowmarkers")) return;

                var markers = BusinessManager.BizList.Values.Select(b => new MarkerDTO
                {
                    BizId = b.ID,
                    Position = b.EnterPoint,
                    Range = b.ColshapeRange
                });
                player.TriggerEvent("businesses:setMarkers", JsonConvert.SerializeObject(markers));
            }
            catch (Exception e) { _logger.WriteError($"ShowBusinessMarkers:\n{e}"); }
        }

        [Command("bizhidemarkers")]
        public void HideBusinessMarkers(PlayerGo player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "bizhidemarkers")) return;

                player.TriggerEvent("businesses:clearMarkers");
            }
            catch (Exception e) { _logger.WriteError($"HideBusinessMarkers:\n{e}"); }
        }

        [Command("changebizenterpoint")]
        public void ChangeBusinessEnterPoint(PlayerGo player, int bizId, int range)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "changebizenterpoint")) return;

                BusinessManager.BizList[bizId].ColshapeRange = range;
                BusinessManager.BizList[bizId].ChangeEnterPoint(player.Position - new Vector3(0, 0, 1.12));
            }
            catch (Exception e) { _logger.WriteError($"ChangeBusinessEnterPoint:\n{e}"); }
        }

        [Command("addbizenterpoint")]
        public void AddBizEnterpoint(PlayerGo player, int bizId)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "addbizenterpoint")) return;

                BusinessManager.BizList[bizId].AdditionalPositions.Add(player.Position);
                MySQL.Query($"UPDATE businesses SET additionalpos=@prop0 WHERE id=@prop1", JsonConvert.SerializeObject(BusinessManager.BizList[bizId].AdditionalPositions), bizId);
                BusinessManager.BizList[bizId].CreateEnterColshape();
            }
            catch (Exception e) { _logger.WriteError($"AddBizEnterpoint:\n{e}"); }
        }

        [Command("clearbizenterpoint")]
        public void ClearBizEnterpoint(PlayerGo player, int bizId)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "clearbizenterpoint")) return;

                BusinessManager.BizList[bizId].AdditionalPositions = new List<Vector3>();
                MySQL.Query($"UPDATE businesses SET additionalpos=@prop0 WHERE id=@prop1", JsonConvert.SerializeObject(BusinessManager.BizList[bizId].AdditionalPositions), bizId);
                BusinessManager.BizList[bizId].CreateEnterColshape();
            }
            catch (Exception e) { _logger.WriteError($"ClearBizEnterpoint:\n{e}"); }
        }

        [Command("createbizped", GreedyArg = true)]
        public void CreateBusinessPed(PlayerGo player, int bizId, int model, string pedName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "createbizped")) return;

                if (!BusinessManager.BizList.ContainsKey(bizId))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_89", 3000);
                    return;
                }

                var pedDto = new PedDTO()
                {
                    Position = player.Position,
                    Rotation = player.Rotation,
                    Model = model,
                    Name = pedName,
                    Dimension = BusinessManager.BizList[bizId].Dimension
                };

                BusinessManager.BizList[bizId].Peds.Add(pedDto);
                NAPI.ClientEvent.TriggerClientEventForAll("businesses:setPed", JsonConvert.SerializeObject(pedDto));
            }
            catch (Exception e) { _logger.WriteError($"CreateBusinessPed:\n{e}"); }
        }

        [Command("getbizpeds")]
        public void GetBusinessPedsInfo(PlayerGo player, int bizId)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "getbizpeds")) return;

                BusinessManager.BizList[bizId].Peds.ForEach(ped =>
                {
                    Chat.SendTo(player, $"Name: {ped.Name} | Index: {BusinessManager.BizList[bizId].Peds.IndexOf(ped)}");
                });
            }
            catch (Exception e) { _logger.WriteError($"GetBusinessPedsInfo:\n{e}"); }
        }

        [Command("removebizped")]
        public void RemoveBizPed(PlayerGo player, int bizId, int pedIndex)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "removebizped")) return;

                BusinessManager.BizList[bizId].Peds.RemoveAt(pedIndex);

                NAPI.ClientEvent.TriggerClientEventForAll("businesses:clearPeds");
                NAPI.ClientEvent.TriggerClientEventForAll("businesses:setPeds",
                JsonConvert.SerializeObject(BusinessManager.BizList.SelectMany(b => b.Value.Peds)));
            }
            catch (Exception e) { _logger.WriteError($"RemoveBizPed:\n{e}"); }
        }

        [Command("setcasinomaxwin")]
        public void setCasinoMaxWin(PlayerGo player, int amount)
        {
            if (!Group.CanUseAdminCommand(player, "setcasinomaxwin")) return;
            if (amount < 500000)
            {
                Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "local_90", 3000);
                return;
            }
            MySQL.Query("UPDATE `casino` set `maxWinOfBet`=@prop0", amount);
            ServerGo.Casino.Games.Roulette.RouletteGame.MaxWin = amount;
        }

        [Command("resetcharacter")]//resetcharacter Wedfeqwd_Ewdwq
        public static void ResetCharacter(PlayerGo player, string name)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "offresetcharacter")) return;

                if (!Main.PlayerUUIDs.ContainsKey(name))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_1", 3000);
                    return;
                }
                var uuid = Main.PlayerUUIDs[name];
                var allVehicles = VehicleManager.getAllHolderVehicles(uuid, OwnerType.Personal);
                foreach (var vehID in allVehicles)
                    VehicleManager.Remove(vehID);

                var house = HouseManager.GetHouse(uuid, OwnerType.Personal);
                if (house != null)
                {
                    if (house.OwnerID == uuid)
                        house.SetOwner(-1, OwnerType.Personal);
                    else
                        house.RemoveRoommate(uuid);
                }

                BusinessManager.GetBusinessByOwner(uuid)?.SetOwner(-1);
                Manager.GetFractionByUUID(uuid)?.DeleteMember(uuid);
                FamilyManager.GetFamilyByUUID(uuid)?.DeleteMember(uuid);

                var characterInCache = Main.GetCharacterByUUID(uuid);
                if (characterInCache != null)
                {
                    characterInCache.CasinoChips = new int[5];
                    characterInCache.AdminLVL = 0;
                    characterInCache.Licenses = new List<GUI.Documents.Models.License>();
                    MoneySystem.Wallet.SetBankMoney(characterInCache.BankNew, 0);
                    characterInCache.Money = 0;
                    characterInCache.Inventory.Reset();
                    characterInCache.Equip.Reset();
                    characterInCache.DonateInventory.Reset();
                    characterInCache.Save();
                }
                else
                {
                    MoneySystem.Wallet.SetBankMoneyByUUID(uuid, 0);
                    InventoryService.GetByUUID(uuid)?.Reset();
                    EquipService.GetByUUID(uuid)?.Reset();
                    DonateService.GetInventoryByUUID(uuid)?.Reset();
                    MySQL.QuerySync($"UPDATE `characters` SET `money`=0, `biz`='[]', `chips`=null, `licenses`='[]', `adminlvl`=0 WHERE `uuid`=@prop0", uuid);
                }

                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Core_2".Translate(name), 3000);
                GameLog.Admin(player.Name, $"resetcharacter", name);
                Main.GetPlayerByUUID(uuid)?.Kick();
            }
            catch (Exception e) { _logger.WriteError($"ResetCharacter:\n{e}"); }
        }


        [Command("checkbizmoney")]
        public static void CheckBusinessMoney(PlayerGo player, int bizid)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "checkbizmoney")) return;

                var biz = BusinessManager.BizList.Values.FirstOrDefault(b => b.ID == bizid);
                if (biz == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_94", 3000);
                    return;
                }

                Chat.SendTo(player, "checkbizmoney".Translate(bizid, biz.BankNalogModel.Balance));
            }
            catch (Exception e) { _logger.WriteError($"CheckBusinessMoney:\n{e}"); }
        }

        [Command("checkhousemoney")]
        public static void Checkhousemoney(PlayerGo player, int houseId)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "checkhousemoney")) return;

                var house = HouseManager.GetHouseById(houseId);
                if (house == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_95", 3000);
                    return;
                }

                Chat.SendTo(player, "checkhousemoney".Translate(houseId, house.BankModel.Balance));
            }
            catch (Exception e) { _logger.WriteError($"Checkhousemoney:\n{e}"); }
        }

        [Command("respawnfractioncars")]
        public static void RespawnFractionCars(PlayerGo player, int fractionId)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "respawnfractioncars")) return;
                FractionCommands.RespawnFractionCars(fractionId);

                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_96".Translate(fractionId.ToString()), 3000);
                GameLog.Admin(player.Name, $"respawnfractioncars({fractionId})", "");
            }
            catch (Exception e) { _logger.WriteError($"RespawnFractionCars:\n{e}"); }
        }

        [Command("takedonate")]
        public static void takeDonate(PlayerGo player, int id, int amount)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "takedonate")) return;

                var target = Main.GetPlayerByID(id);
                if (!target.IsLogged()) return;

                if (player.GetCharacter().AdminLVL < target.GetCharacter().AdminLVL)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_10".Translate(), 3000);
                    return;
                }
                var coins = target.GetPlayerGo().Account.GoCoins;
                if (coins <  amount) amount = coins;

                target.SubGoCoins(amount);
                Trigger.ClientEvent(target, "starset", coins);

                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "local_97".Translate( target.Name, amount.ToString()), 3000);
                Notify.Send(target, NotifyType.Success, NotifyPosition.BottomCenter, $"-{amount} GO Coins", 3000);
                GameLog.Admin(player.Name, $"takedonate({amount})", target.Name);
            }
            catch (Exception e) { _logger.WriteError($"takeDonate:\n{e}"); }
        }

        [Command("changephonenumber")]
        public static void ChangePhoneNumber(PlayerGo player, int targetId, int newNumber)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "changephonenumber")) return;

                var target = Main.GetPlayerByID(targetId);
                if (!target.IsLogged()) return;
                if (PhoneLoader.ChangeNumber(target, newNumber))
                    Notify.SendSuccess(player, "changepnumb:ok".Translate(target.Name));
                else
                    Notify.SendError(player, "changepnumb:err");
            }
            catch (Exception e) { _logger.WriteError($"takeDonate:\n{e}"); }
        }

        public static void GiveDonatePoints(PlayerGo player, PlayerGo target, int amount)
        {
            if (!Group.CanUseAdminCommand(player, "givedonate")) return;
            if (target.Account.GoCoins + amount < 0) amount = 0;
            target.AddGoCoins(amount);
            Trigger.ClientEvent(target, "starset", target.Account.GoCoins);

            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "local_7".Translate(target.Name, amount.ToString()), 3000);
            Notify.Send(target, NotifyType.Success, NotifyPosition.BottomCenter, $"+{amount} GO Coins", 3000);

            GameLog.Admin(player.Name, $"givedonate({amount})", target.Name);
        }
     
        public static void ServerRestart(string name, string reason)
        {
            IsServerStoping = true;
            GameLog.Admin(name, $"stopServer{reason}", "");
            foreach (PlayerGo player in NAPI.Pools.GetAllPlayers())
                NAPI.Player.KickPlayer(player, reason);
            InventoryService.SaveAllInventories();
            BusinessManager.SavingBusiness();
            GangsCapture.SavingRegions();
            Manager.SaveStocksDic();

            WhistlerTask.Run(() =>
            {
                Environment.Exit(0);
            }, 60000);
            Infrastructure.DataAccess.DbManager.SaveDatabase();
            Whistler.EFCore.DBManager.SaveDatabase();
        }


        public static void saveCoords(PlayerGo player, string msg)
        {
            if (!Group.CanUseAdminCommand(player, "scoord")) return;
            Vector3 pos = NAPI.Entity.GetEntityPosition(player);
            //NAPI.Blip.CreateBlip(1, pos, 1, 69);
            Vector3 rot = NAPI.Entity.GetEntityRotation(player);
            if (NAPI.Player.IsPlayerInAnyVehicle(player))
            {
                Vehicle vehicle = player.Vehicle;
                pos = NAPI.Entity.GetEntityPosition(vehicle) + new Vector3(0, 0, 0.5);
                rot = NAPI.Entity.GetEntityRotation(vehicle);
            }
            SaveCoord(pos, rot, msg);
            Chat.SendTo(player, $"Coords: {NAPI.Entity.GetEntityPosition(player)}");
        }
        public static void SaveCarCoords(PlayerGo player, string msg)
        {
            if (!Group.CanUseAdminCommand(player, "scoord")) return;

            foreach (var vehicle in NAPI.Pools.GetAllVehicles())
            {
                if (vehicle.GetData<string>("ACCESSADMINBY") == player.Name)
                    SaveCoord(NAPI.Entity.GetEntityPosition(vehicle) + new Vector3(0, 0, 0.5), NAPI.Entity.GetEntityRotation(vehicle), msg);
            }
            Chat.SendTo(player, $"Coords: {NAPI.Entity.GetEntityPosition(player)}");
        }

        private static void SaveCoord(Vector3 pos, Vector3 rot, string msg)
        {
            try
            {
                StreamWriter saveCoords = new StreamWriter("coords.txt", true, Encoding.UTF8);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                saveCoords.Write($"{msg}   Coords: new Vector3({pos.X}, {pos.Y}, {pos.Z}),    JSON: {JsonConvert.SerializeObject(pos)}      \r\n");
                saveCoords.Write($"{msg}   Rotation: new Vector3({rot.X}, {rot.Y}, {rot.Z}),     JSON: {JsonConvert.SerializeObject(rot)}    \r\n");
                saveCoords.Close();
            }

            catch (Exception e)
            {
                _logger.WriteError($"saveCoords:\n{e}");
            }
        }

        [Command("wtfcoins")]
        public static void Command_GetStrangeAmountOfCoins(PlayerGo player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "wtfcoins")) return;

                Chat.SendTo(player, "wtfcoins:1");

                var result = MySQL.QueryRead($"SELECT login, gocoins FROM accounts WHERE gocoins>10000");
                if (result == null) return;

                foreach (DataRow row in result.Rows)
                {
                    var login = row["login"].ToString();
                    var gobucks = Convert.ToInt32(row["gocoins"]);

                    Chat.SendTo(player, "wtfcoins:2".Translate(login, gobucks));
                }

                Chat.SendTo(player, "wtfcoins:3");

                result = MySQL.QueryRead($"SELECT firstname,lastname,money FROM characters WHERE money>10000000");
                if (result == null) return;

                foreach (DataRow row in result.Rows)
                {
                    var fisrtname = row["firstname"].ToString();
                    var lastname = row["lastname"].ToString();
                    var money = Convert.ToInt32(row["money"]);

                    Chat.SendTo(player, "wtfcoins:4".Translate(fisrtname, lastname, money));
                }

                Chat.SendTo(player, "wtfcoins:5");

                foreach (var bankAccount in BankManager.GetAccountsByPredicate(item => item.OwnerType == TypeBankAccount.Player && item.Balance > 10000000))
                {
                    Chat.SendTo(player, "wtfcoins:6".Translate(Main.PlayerNames.GetValueOrDefault(bankAccount.UUID, "Unknown"),  bankAccount.Balance));
                }
            }
            catch (Exception e) { _logger.WriteError($"Command_GetStrangeAmountOfCoins:\n{e}"); }
        }        

        [Command("coins")]
        public static void Command_GetFullTargetCoinsInfo(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "getcoinsinfo")) return;

                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_98", 3000);
                    return;
                }

                var account = target.GetPlayerGo().Account;
                Chat.SendTo(player, "wtfcoins:7".Translate(target.Name, account.GoCoins));
                Chat.SendTo(player, "wtfcoins:8");
                var queryResult = MySQL.QueryRead($"SELECT `sum`, `date` FROM `@prop1` WHERE `login`=@prop0 AND `unitpayid` > 0", account.Login.ToLower(), Main.ServerConfig.DonateConfig.Database);
                if (queryResult == null || queryResult.Rows.Count == 0)
                {
                    Chat.SendTo(player, "wtfcoins:9");
                    return;
                }

                foreach (DataRow row in queryResult.Rows)
                {
                    var date = row["date"].ToString();
                    var value = row["sum"].ToString();

                    Chat.SendTo(player, "wtfcoins:10".Translate(date, value));
                }
            }
            catch (Exception e) { _logger.WriteError($"Command_GetFullTargetCoinsInfo:\n{e}"); }
        }
              
        [Command("arenapoints")]
        public static void GetArenaPoints(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "arenapoints")) return;

                if (Main.GetPlayerByID(id) == null)
                {
                    Chat.SendTo(player, "arenapts:1");
                    return;
                }

                Chat.SendTo(player, "arenapts:2".Translate(Main.GetPlayerByID(id).GetCharacter().ArenaPoints));
            }
            catch (Exception e) { _logger.WriteError($"GetArenaPoints:\n{e}"); }
        }
       
        public static int SERVER_ONLINE_CHEATED = 0;
        [Command("adm_onl")]
        public static void CheatServerOnline(PlayerGo player, int cheatOnline)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "adm_onl")) return;

                SERVER_ONLINE_CHEATED = cheatOnline;
                NAPI.ClientEvent.TriggerClientEventForAll("A_ONL", cheatOnline);
            }
            catch (Exception e) { _logger.WriteError($"CheatServerOnline:\n{e}"); }
        }
        [Command("admhelp")]
        public static void ahelp(PlayerGo player)
        {
            try
            {
                if (player.GetCharacter().AdminLVL == 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_3", 3000);
                    return;
                }

                int adminLvl = player.GetCharacter().AdminLVL;
                var cmdGroupsByLvl = Group.GroupCommands.Where(cmd => cmd.MinLVL <= adminLvl)
                                              .OrderBy(cmd => cmd.MinLVL)
                                              .GroupBy(cmd => cmd.MinLVL, cmd => cmd.Command);

                foreach (var cmdGroup in cmdGroupsByLvl)
                {
                    var lvl = cmdGroup.Key;
                    Chat.SendTo(player,cmdGroup.Aggregate($"[{lvl}] /", (current, next) => current + ", /" + next));
                }
                Chat.SendTo(player, "local_8");
            }
            catch (Exception e) { _logger.WriteError($"ahelp:\n{e}"); }
        }

        [Command("setacommandtech")]
        public static void Command_SetAdminCommandTechnical(PlayerGo player, string command, bool istech)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "setacommandtech")) return;

                var cmd = Group.GroupCommands.FirstOrDefault(c => c.Command == command);
                if (cmd == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_100", 3000);
                    return;
                }

                cmd.IsTechnical = istech;
                MySQL.Query($"UPDATE adminaccess SET istech=@prop0 WHERE command=@prop1", istech, command);

                var noWord = istech ? "" : "not";
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "local_101".Translate( command, noWord), 3000);
            }
            catch (Exception e) { _logger.WriteError($"Command_SetAdminCommandTechnical:\n{e}"); }
        }

        [Command("setacommandlvl")]
        public static void Command_SetCommandLvl(PlayerGo player, string command, int newLvl)
        {
            if (!Group.CanUseAdminCommand(player, "setacommandlvl")) return;

            if (command == "setacommandlvl")
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_102", 3000);
                return;
            }

            var cmd = Group.GroupCommands.FirstOrDefault(c => c.Command == command);
            if (cmd == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_103", 3000);
                return;
            }

            cmd.MinLVL = newLvl;
            MySQL.Query($"UPDATE adminaccess SET minrank=@prop0 WHERE command=@prop1", newLvl, command);

            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "local_104".Translate( command, newLvl.ToString()), 3000);
            GameLog.Admin(player.Name, $"setacommandlvl({command},{newLvl})", "");
        }

        [Command("givelic")]
        public static void GiveLic(PlayerGo player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "givelic")) return;
            var target = Main.GetPlayerByID(id);
            if (target == null) return;
            target.GiveLic(Enum.GetValues(typeof(LicenseName)).Cast<LicenseName>());
            Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_4".Translate(player.Name.Replace('_', ' ')), 3000);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_6".Translate( target.Name.Replace('_', ' ')), 3000);
            MainMenu.SendStats(target);
            GameLog.Admin(player.Name, $"givelic", target.Name);
        }

        [Command("giveonelic")]
        public static void GiveOneLic(PlayerGo player, int id, int index)
        {
            if (!Group.CanUseAdminCommand(player, "givelic")) return;
            var target = Main.GetPlayerByID(id);
            if (target == null) return;
            if (Enum.IsDefined(typeof(LicenseName), index))
            {
                target.GiveLic((LicenseName)index);
                Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_337".Translate(player.Name.Replace('_', ' '), ((LicenseName)index).ToString()), 3000);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_336".Translate(target.Name.Replace('_', ' '), ((LicenseName)index).ToString()), 3000);
                MainMenu.SendStats(target);
                GameLog.Admin(player.Name, $"giveonelic({index})", target.Name);
            }
            else
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_338".Translate(index.ToString()), 3000);
        }

        private const int MuteDistance = 20;
        private const int MuteTime = 10;

        [RemoteEvent("media:mute:press")]
        public static void OnMediaPressKeyMute(PlayerGo player)
        {
            var character = player.GetCharacter();
            if (character == null || (character.Media < 1 && character.AdminLVL < 8)) return;
            character.MediaMuted = !character.MediaMuted;
            player.TriggerEvent("media:mute:state", character.MediaMuted);
            var msg = character.MediaMuted ? "media:mute:on" : "media:mute:off";
            Chat.SendToAdmins(3, msg.Translate(player.Value));
        }

        [RemoteEvent("srv_consoleLog")]
        public void OnConsolelog(PlayerGo client, string log)
        {
            _logger.WriteClient(log);
        }
        public static void setPlayerAdminGroup(PlayerGo player, PlayerGo target)
        {
            if (!Group.CanUseAdminCommand(player, "makeadmin")) return;
            if (target.GetCharacter().AdminLVL >= 1)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_7", 3000);
                return;
            }
            target.GetCharacter().AdminLVL = 1;
            target.SetSharedData("ALVL", 1);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_8".Translate( target.Name), 3000);
            Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_9".Translate(player.Name), 3000);
            SetPlayerToAdminGroup?.Invoke(target);
            GameLog.Admin($"{player.Name}", $"makeAdmin", $"{target.Name}");
        }
        public static void delPlayerAdminGroup(PlayerGo player, PlayerGo target)
        {
            if (!Group.CanUseAdminCommand(player, "takeadmin")) return;
            if (player == target)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_9".Translate(), 3000);
                return;
            }
            if (target.GetCharacter().AdminLVL >= player.GetCharacter().AdminLVL)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_10", 3000);
                return;
            }
            if (target.GetCharacter().AdminLVL < 1)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_11", 3000);
                return;
            }
            target.GetCharacter().AdminLVL = 0;

            target.SetSharedData("ALVL", 0);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_12".Translate( target.Name), 3000);
            Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_13".Translate(player.Name), 3000);
            DeletePlayerFromAdminGroup?.Invoke(target);
            GameLog.Admin($"{player.Name}", $"takeadmin", $"{target.Name}");
        }
        public static void setPlayerAdminRank(PlayerGo player, PlayerGo target, int rank)
        {
            if (!Group.CanUseAdminCommand(player, "changeadminrank")) return;
            if (player == target)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_14", 3000);
                return;
            }
            if (target.GetCharacter().AdminLVL < 1)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_15", 3000);
                return;
            }
            if (target.GetCharacter().AdminLVL >= player.GetCharacter().AdminLVL)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_16", 3000);
                return;
            }
            if (rank < -1 || rank >= player.GetCharacter().AdminLVL)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_17", 3000);
                return;
            }
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_18".Translate(target.Name, rank.ToString()), 3000);
            Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_19".Translate(player.Name, rank.ToString()), 3000);
            target.GetCharacter().AdminLVL = rank;
            target.SetSharedData("ALVL", rank);
            GameLog.Admin($"{player.Name}", $"setAdminRank({rank})", $"{target.Name}");
        }
        public static void setPlayerPrimeAccount(PlayerGo player, PlayerGo target, int days)
        {
            if (!Group.CanUseAdminCommand(player, "setprime")) return;
            if (days < 1)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_20", 3000);
                return;
            }
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_21".Translate(target.Name, days.ToString()), 3000);
            target.AddPrime(days);
            MainMenu.SendStats(target);
            GameLog.Admin($"{player.Name}", $"setPlayerPrimeAccount({days} days)", $"{target.Name}");
        }

        public static void setFracLeader(PlayerGo sender, PlayerGo target, int fracid)
        {
            if (!Group.CanUseAdminCommand(sender, "giveleader")) return;
            var fraction = Manager.GetFraction(fracid);

            if (fraction != null)
            {
                var targetCharacter = target.GetCharacter();
                int newRank = fraction.Ranks.Max(item => item.Key);
                var currentFraction = Manager.GetFraction(target);
                if (currentFraction != null)
                {
                    if (currentFraction.Id == fraction.Id)
                        fraction.ChangeRank(targetCharacter.UUID, newRank);
                    else
                    {
                        currentFraction.DeleteMember(targetCharacter.UUID);
                        Manager.InvitePlayerToFraction(target, fraction, newRank);
                    }
                }
                else
                    Manager.InvitePlayerToFraction(target, fraction, newRank);


                Notify.Alert(target, "Core_22".Translate(Manager.getName(fracid)));
                Notify.Send(sender, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_23".Translate(target.Name, Manager.getName(fracid)), 3000);
                MainMenu.SendStats(target);
                target.SendTip("tip_became_leader");
                GameLog.Admin($"{sender.Name}", $"setFracLeader({fracid})", $"{target.Name}");
            }
        }
        [Command("takeadminoff")]
        public static void delAdminOffline(PlayerGo player, string name)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "takeadminoff")) return;

                if (!Main.PlayerNames.ContainsValue(name))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_1", 3000);
                    return;
                }

                var split = name.Split('_');

                var characterInCache = Main.GetCharacterByName(name);
                if(characterInCache != null)
                    characterInCache.AdminLVL = 0;

                MySQL.Query($"UPDATE characters SET adminlvl=0 WHERE firstname=@prop0 and lastname=@prop1", split[0], split[1]);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "local_105", 3000);
                GameLog.Admin(player.Name, "takeadminoff", name);
            }
            catch (Exception e)
            {
                _logger.WriteError($"delAdminOffline:\n{e}");
            }
        }

        [Command("clearfraction")]
        public static void Command_ClearFraction(PlayerGo player, int frac)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "clearfraction")) return;

                var fraction = Manager.GetFraction(frac);
                if (fraction == null) return;
                foreach (var uuid in fraction.Members.Select(item => item.Value.PlayerUUID).ToList())
                {
                    fraction.DeleteMember(uuid);
                }

                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "local_107".Translate( fraction.ToString()), 3000);
                GameLog.Admin(player.Name, $"clearfraction({fraction})", "");
            }
            catch (Exception e)
            {
                _logger.WriteError($"Command_ClearFraction:\n{e}");
            }
        }

        public static void DelJob(PlayerGo sender, PlayerGo target)
        {
            if (target.GetCharacter().WorkID != 0)
            {
                if (target.GetData<bool>("ON_WORK") == true)
                {
                    Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_27".Translate(), 3000);
                    return;
                }
                target.GetCharacter().WorkID = 0;
                Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_28".Translate(sender.Name.Replace('_', ' ')), 3000);
                Notify.Send(sender, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_29".Translate(target.Name.Replace('_', ' ')), 3000);
                MainMenu.SendStats(target);
                GameLog.Admin($"{sender.Name}", $"delJob", $"{target.Name}");
            }
            else Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_30".Translate(), 3000);
        }
        public static void DelFrac(PlayerGo sender, PlayerGo target, bool isLeader)
        {

            if (sender.GetCharacter().AdminLVL < target.GetCharacter().AdminLVL)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "local_10".Translate(), 3000);
                return;
            }
            if (Manager.isLeader(target) && !isLeader)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_31".Translate(), 3000);
                return;
            }
            if (!Manager.isLeader(target) && isLeader)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "local_9".Translate(), 3000);
                return;
            }
            if (Manager.GetFraction(target)?.DeleteMember(target.GetCharacter().UUID) ?? false)
            {

                if (isLeader)
                {
                    GameLog.Admin($"{sender.Name}", $"delFracLeader", $"{target.Name}");
                    Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_24".Translate(sender.Name.Replace('_', ' ')), 3000);
                    Notify.Send(sender, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_25".Translate(target.Name.Replace('_', ' ')), 3000);
                }
                else
                {
                    GameLog.Admin($"{sender.Name}", $"removefrac", $"{target.Name}");
                    Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_32".Translate(sender.Name.Replace('_', ' ')), 3000);
                    Notify.Send(sender, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_33".Translate(target.Name.Replace('_', ' ')), 3000);
                }
            }
            else
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_34".Translate(), 3000);
        }

        public static bool GiveMoney(PlayerGo player, IMoneyOwner target, int amount)
        {
            if (amount > 0)
                return MoneySystem.Wallet.TransferMoney(new ServerMoney(TypeMoneyAcc.Admin, player.GetCharacter().UUID), target, amount, 0, "Money_Givemoney");
            else
                return MoneySystem.Wallet.TransferMoney(target, new ServerMoney(TypeMoneyAcc.Admin, player.GetCharacter().UUID), Math.Abs(amount), 0, "Money_Givemoney");
        }

        public static void OffMutePlayer(PlayerGo player, string target, int time, string reason)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "mute")) return;
                if (NAPI.Player.GetPlayerFromName(target) != null)
                {
                    mutePlayer(player, NAPI.Player.GetPlayerFromName(target) as PlayerGo, time, reason);
                    Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Core_36", 3000);
                    return;
                }
                if(player.Name.Equals(target)) return;               
                var split = target.Split('_');
                MySQL.QueryRead($"UPDATE `characters` SET `unmutedate`= @prop0 WHERE firstname = @prop1 AND lastname = @prop2", MySQL.ConvertTime(DateTime.Now.AddMinutes(time)), split[0], split[1]);

                Chat.AdminToAll("Com_132".Translate( player.Name, target, time.ToString(), reason));

                GameLog.Admin($"{player.Name}", $"mutePlayer({time}, {reason})", $"{target}");
            }
            catch { }

        }
        public static void mutePlayer(PlayerGo player, PlayerGo target, int time, string reason)
        {
            if (!Group.CanUseAdminCommand(player, "mute")) return;

            if (player.Character.AdminLVL < target.Character.AdminLVL)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_10".Translate(), 3000);
                return;
            }

            if (player == target) return;
            //if (time > 480)
            //{
            //    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_36", 3000);
            //    return;
            //}
            //target.GetCharacter().Unmute = time * 60;
            target.MutePlayer(time);
            Chat.AdminToAll("Com_133".Translate( player.Name, target.Name, time.ToString(), reason));
            GameLog.Admin($"{player.Name}", $"mutePlayer({time}, {reason})", $"{target.Name}");
        }
        public static void unmutePlayer(PlayerGo player, PlayerGo target)
        {
            if (!Group.CanUseAdminCommand(player, "unmute")) return;

            target.Unmute();

            Chat.AdminToAll("Com_134".Translate( player.Name, target.Name));
            GameLog.Admin($"{player.Name}", $"unmutePlayer", $"{target.Name}");
        }
        public static void BanPlayer(PlayerGo player, PlayerGo target, int time, string reason, bool isSilence)
        {
            if (player == target) return;
            if(target.GetCharacter().AdminLVL >= player.GetCharacter().AdminLVL)
            {
                Chat.SendToAdmins(3, "Com_108".Translate( player.Name, player.GetCharacter().UUID, target.Name, target.GetCharacter().UUID));
                return;
            }
            DateTime unbanTime = DateTime.Now.AddDays(time);
            string banTimeMsg = "local_21";           

            if (!isSilence)
                Chat.AdminToAll("Com_135".Translate( player.Name, target.Name, time.ToString(), banTimeMsg, reason));

            Ban.Online(target, unbanTime, false, reason, player.Name);

            Notify.Send(target, NotifyType.Warning, NotifyPosition.Center, $"Core_37".Translate(unbanTime.ToString()), 30000);
            Notify.Send(target, NotifyType.Warning, NotifyPosition.Center, $"Core_38".Translate(reason), 30000);

            int AUUID = player.GetCharacter().UUID;
            int TUUID = target.GetCharacter().UUID;
            
            GameLog.Ban(AUUID, TUUID, unbanTime, reason, false);

            target.Kick(reason);
        }
        public static void hardbanPlayer(PlayerGo player, PlayerGo target, int time, string reason)
        {
            if (!Group.CanUseAdminCommand(player, "hardban")) return;
            if (player == target) return;
            if(target.GetCharacter().AdminLVL >= player.GetCharacter().AdminLVL)
            {
                Chat.SendToAdmins(3, "Com_109".Translate( player.Name, player.GetCharacter().UUID, target.Name, target.GetCharacter().UUID));
                return;
            }
            DateTime unbanTime = DateTime.Now.AddDays(time);
            string banTimeMsg = "local_21";
           
            Chat.AdminToAll("Com_136".Translate( player.Name, target.Name, time.ToString(), banTimeMsg, reason));

            Ban.Online(target, unbanTime, true, reason, player.Name);

            Notify.Send(target, NotifyType.Warning, NotifyPosition.Center, $"Core_39".Translate( unbanTime.ToString()), 30000);
            Notify.Send(target, NotifyType.Warning, NotifyPosition.Center, "Core_38".Translate(reason), 30000);

            int AUUID = player.GetCharacter().UUID;
            int TUUID = target.GetCharacter().UUID;

            GameLog.Ban(AUUID, TUUID, unbanTime, reason, true);

            target.Kick(reason);
        }
        public static void hardbanPlayer( PlayerGo target, int time, string reason)
        {
            DateTime unbanTime = DateTime.Now.AddDays(time);
            string banTimeMsg = "local_21";

            Chat.AdminToAll("Com_136".Translate("[Anticheat]", target.Name, time.ToString(), banTimeMsg, reason));

            Ban.Online(target, unbanTime, true, reason, "[Anticheat]");

            Notify.Send(target, NotifyType.Warning, NotifyPosition.Center, $"Core_39".Translate(unbanTime.ToString()), 30000);
            Notify.Send(target, NotifyType.Warning, NotifyPosition.Center, "Core_38".Translate(reason), 30000);

            int TUUID = target.GetCharacter().UUID;

            GameLog.Ban(000000, TUUID, unbanTime, reason, true);

            target.Kick(reason);
        }
        public static void offBanPlayer(PlayerGo player, string name, int time, string reason)
        {
            if (!Group.CanUseAdminCommand(player, "offban")) return;
            if (player.Name == name) return;
            PlayerGo target = NAPI.Player.GetPlayerFromName(name) as PlayerGo;
            if (target != null) {
                if(target.IsLogged()) {
                    if(target.GetCharacter().AdminLVL >= player.GetCharacter().AdminLVL)
                    {
                        Chat.SendToAdmins(3, "Com_112".Translate( player.Name, player.GetCharacter().UUID, target.Name, target.GetCharacter().UUID));
                        return;
                    } else {
                        target.Kick();
                        Notify.Send(player, NotifyType.Success, NotifyPosition.Center, "Core_40", 3000);
                    }
                }
            } else {
                string[] split = name.Split('_');
                DataTable result = MySQL.QueryRead("SELECT adminlvl FROM characters WHERE firstname = @prop0 AND lastname = @prop1", split[0], split[1]);
                DataRow row = result.Rows[0];
                int targetadminlvl = Convert.ToInt32(row[0]);
                if(targetadminlvl >= player.GetCharacter().AdminLVL)
                {
                    Chat.SendToAdmins(3, "Com_113".Translate( player.Name, player.GetCharacter().UUID, name));
                    return;
                }
            }

            int AUUID = player.GetCharacter().UUID;
            int TUUID = Main.PlayerUUIDs[name];

            Ban ban = Ban.Get2(TUUID);
            if (ban != null)
            {
                string hard = (ban.isHard) ? "hard " : "";
                Notify.Send(player, NotifyType.Warning, NotifyPosition.Center, $"Core_41".Translate(hard), 3000);
                return;
            }

            DateTime unbanTime = DateTime.Now.AddDays(time);
            string banTimeMsg = "local_21";

            Ban.Offline(name, unbanTime, false, reason, player.Name);

            GameLog.Ban(AUUID, TUUID, unbanTime, reason, false);

            Chat.AdminToAll("Com_138".Translate( player.Name, name, time.ToString(), banTimeMsg, reason));
        }
        public static void offHardBanPlayer(PlayerGo player, string name, int time, string reason)
        {
            if (!Group.CanUseAdminCommand(player, "offban")) return;
            if(player.Name.Equals(name)) return;
            PlayerGo target = NAPI.Player.GetPlayerFromName(name) as PlayerGo;
            if (target != null) {
                if(target.IsLogged()) {
                    if(target.GetCharacter().AdminLVL >= player.GetCharacter().AdminLVL)
                    {
                        Chat.SendToAdmins(3, "Com_105".Translate( player.Name, player.GetCharacter().UUID, target.Name, target.GetCharacter().UUID));
                        return;
                    } else {
                        target.Kick();
                        Notify.Send(player, NotifyType.Success, NotifyPosition.Center, "Core_42", 3000);
                    }
                }
            } else {
                string[] split = name.Split('_');
                DataTable result = MySQL.QueryRead($"SELECT adminlvl FROM characters WHERE firstname = @prop0 AND lastname = @prop1", split[0], split[1]);
                DataRow row = result.Rows[0];
                int targetadminlvl = Convert.ToInt32(row[0]);
                if(targetadminlvl >= player.GetCharacter().AdminLVL)
                {
                    Chat.SendToAdmins(3, "Com_106".Translate( player.Name, player.GetCharacter().UUID, name));
                    return;
                }
            }

            int AUUID = player.GetCharacter().UUID;
            int TUUID = Main.PlayerUUIDs[name];

            Ban ban = Ban.Get2(TUUID);
            if (ban != null)
            {
                string hard = (ban.isHard) ? "local_22" : "";
                Notify.Send(player, NotifyType.Warning, NotifyPosition.Center, $"Core_43".Translate( hard), 3000);
                return;
            }

            DateTime unbanTime = DateTime.Now.AddDays(time);
            string banTimeMsg = "local_21";

            Ban.Offline(name, unbanTime, true, reason, player.Name);

            GameLog.Ban(AUUID, TUUID, unbanTime, reason, true);

            Chat.AdminToAll("Com_137".Translate( player.Name, name, time.ToString(), banTimeMsg, reason));
        }
        public static void unbanPlayer(PlayerGo player, string name)
        {
            if (!Main.PlayerNames.ContainsValue(name))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_44", 3000);
                return;
            }
            if (!Ban.Pardon(name))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_45".Translate( name), 3000);
                return;
            }
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Core_46", 3000);
            GameLog.Admin($"{player.Name}", $"unban", $"{name}");
        }
        public static void UnhardbanPlayer(PlayerGo player, string name)
        {
            if (!Main.PlayerNames.ContainsValue(name))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_44", 3000);
                return;
            }
            if (!Ban.PardonHard(name))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_47".Translate( name), 3000);
                return;
            }
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Core_48", 3000);
            GameLog.Admin($"{player.Name}", $"unhardban", $"{name}");
        }
        public static void kickPlayer(PlayerGo player, PlayerGo target, string reason, bool isSilence)
        {
            string cmd = (isSilence) ? "skick" : "kick";
            if (!Group.CanUseAdminCommand(player, cmd)) return;
            if(target.GetCharacter().AdminLVL >= player.GetCharacter().AdminLVL)
            {
                Chat.SendToAdmins(3, "Com_107".Translate( player.Name, player.GetCharacter().UUID, target.Name, target.GetCharacter().UUID));
                return;
            }
            if (!isSilence)
                Chat.AdminToAll("Com_139".Translate( player.Name, target.Name, reason));
            else
            {
                foreach (PlayerGo p in ReportSystem.ReportManager.Admins)
                {
                    if (!p.IsLogged()) continue;
                    if (p.GetCharacter().AdminLVL >= 1 && player.GetCharacter().AdminLVL < 9 || p.GetCharacter().AdminLVL >= 9 && player.GetCharacter().AdminLVL >= 9)
                    {
                        Chat.SendTo(p, "Com_146".Translate( player.Name, target.Name));
                    }
                }
            }
            GameLog.Admin($"{player.Name}", $"kickPlayer{reason}", $"{target.Name}");
            NAPI.Player.KickPlayer(target, reason);
        }
        public static void warnPlayer(PlayerGo player, PlayerGo target, string reason)
        {
            if (!Group.CanUseAdminCommand(player, "warn")) return;
            if(player == target) return;
            if(target.GetCharacter().AdminLVL >= player.GetCharacter().AdminLVL)
            {
                Chat.SendToAdmins(3, "Com_114".Translate( player.Name, player.GetCharacter().UUID, target.Name, target.GetCharacter().UUID));
                return;
            }
            target.GetCharacter().Warns++;
            target.GetCharacter().Unwarn = DateTime.Now.AddDays(14);

            Manager.GetFraction(target)?.DeleteMember(target.GetCharacter().UUID);            

            Chat.AdminToAll("Com_140".Translate( player.Name, target.Name, reason, target.GetCharacter().Warns.ToString()));

            if (target.GetCharacter().Warns >= 3)
            {
                DateTime unbanTime = DateTime.Now.AddMinutes(43200);
                target.GetCharacter().Warns = 0;
                Ban.Online(target, unbanTime, false, "Warns 3/3", "Server_Serverniy");
            }

            GameLog.Admin($"{player.Name}", $"warnPlayer{reason}", $"{target.Name}");
            target.Kick("Warn");
        }
        public static void kickPlayerByName(PlayerGo player, string name)
        {
            if (!Group.CanUseAdminCommand(player, "nkick")) return;
            PlayerGo target = NAPI.Player.GetPlayerFromName(name) as PlayerGo;
            if (target == null) return;
            NAPI.Player.KickPlayer(target);
            GameLog.Admin($"{player.Name}", $"kickPlayer", $"{name}");
        }

        public static void killTarget(PlayerGo player, PlayerGo target)
        {
            if (!Group.CanUseAdminCommand(player, "kill")) return;

            if (player.GetCharacter().AdminLVL < target.GetCharacter().AdminLVL)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_10".Translate(), 3000);
                return;
            }

            NAPI.Player.SetPlayerHealth(target, 0);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_49".Translate( target.Name), 3000);
            GameLog.Admin($"{player.Name}", $"killPlayer", $"{target.Name}");
        }
        public static void healTarget(PlayerGo player, PlayerGo target, int hp)
        {
            if (!Group.CanUseAdminCommand(player, "hp")) return;

            if (player.GetCharacter().AdminLVL < target.GetCharacter().AdminLVL)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_10".Translate(), 3000);
                return;
            }
            NAPI.Player.SetPlayerHealth(target, hp);
            GameLog.Admin($"{player.Name}", $"healPlayer({hp})", $"{target.Name}");
        }
        [Command("offcheckmoney")]
        public static void OfflineCheckMoney(PlayerGo player, string name)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "offcheckmoney")) return;

                if (!Main.PlayerNames.ContainsValue(name))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_108", 3000);
                    return;
                }

                var client = NAPI.Pools.GetAllPlayers().FirstOrDefault(c => c.Name == name);
                if (client.IsLogged())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_109", 3000);
                    return;
                }

                var uuid = Main.PlayerUUIDs[name];
                var result = MySQL.QueryRead($"SELECT money, chips FROM characters WHERE uuid=@prop0", uuid);
                var gocoins = MySQL.QueryRead($"SELECT gocoins FROM accounts WHERE character1=@prop0 OR character2=@prop0 OR character3=@prop0", uuid).Rows[0];
                
                Chat.SendTo(player,"--------------------------");

                #region checking money
                int coins = Convert.ToInt32(gocoins["gocoins"]);
                var cashMoney = Convert.ToInt32(result.Rows[0]["money"]);
                var bankAcc = BankManager.GetAccountByUUID(uuid);
                long bankMoney = bankAcc?.Balance ?? 0;
                Chat.SendTo(player,$"{name} {cashMoney}$ | Bank: {bankMoney}$ | GoCoins: {coins}");
                #endregion

                #region checking chips
                var chips = JsonConvert.DeserializeObject<int[]>(result.Rows[0]["chips"].ToString());
                if (chips != null && chips.Length > 0)
                {
                    var chipList = new List<Chip>();
                    for (var i = 0; i < 5; i++)
                        for (var j = 0; j < chips[i]; j++)
                        {
                            chipList.Add(ChipFactory.Create((ChipType)i));
                        }
                    var total = chipList.Sum(c => c.Value);
                    Chat.SendTo(player,$"Chips: [{chips[0]}, {chips[1]}, {chips[2]}, {chips[3]}, {chips[4]}] Balance: {total}");
                }
                #endregion

                #region checking property
                var house = HouseManager.GetHouse(uuid, 0, true);
                if (house == null)
                {
                    Chat.SendTo(player,"PlayerGo has no house");
                }
                else
                {
                    Chat.SendTo(player,$"PlayerGo has {HouseManager.HouseTypeList[house.Type].Name} house | ID {house.ID}");
                    
                }
                
                var vehicles = VehicleManager.getAllHolderVehicles(uuid, OwnerType.Personal);
                if (vehicles.Count() > 0)
                {
                    Chat.SendTo(player, $"PlayerGo vehicles:");
                    foreach (var veh in vehicles)
                        Chat.SendTo(player, $"{VehicleManager.Vehicles[veh].Number} - {VehicleManager.Vehicles[veh].ModelName}");
                }

                var biz = BusinessManager.GetBusinessByOwner(uuid);
                if (biz == null)
                {
                    Chat.SendTo(player,$"PlayerGo has no business");
                }
                else
                {
                    Chat.SendTo(player,$"PlayerGo has {biz.TypeModel.TypeName} (ID {biz.ID})");
                }
                #endregion
                Chat.SendTo(player,"--------------------------");
            }
            catch (Exception e) { _logger.WriteError($"OfflineCheckMoney:\n{e}"); }
        }
        public static void checkMoney(PlayerGo player, PlayerGo target)
        {

            try
            {
                if (!Group.CanUseAdminCommand(player, "checkmoney")) return;

                var bankAcc = target.GetBankAccount();
                long bankMoney = bankAcc?.Balance ?? 0;
                Chat.SendTo(player,"--------------------------");
                Chat.SendTo(player, $"Money of {target.Name}");
                Chat.SendTo(player, $"{target.Name} has cash: {target.GetCharacter().Money}$");
                Chat.SendTo(player, $"Bank account balance: {bankMoney}$");

                Commands.CMD_CheckChips(player, target.Value);

                Commands.CMD_showPlayerHouseStats(player, target.Value);
                Chat.SendTo(player,"--------------------------");
                var deposits = DepositManager.GetDepositDTOs(player);
                if (deposits != null)
                {
                    Chat.SendTo(player, $"Bank account deposits:");
                    foreach (var deposit in deposits)
                    {
                        Chat.SendTo(player, $"  balance: {deposit.balance}$, profit: {deposit.profit}$");
                    }
                    Chat.SendTo(player, "--------------------------");
                }
                var credits = CreditManager.GetCreditDTOs(player);
                if (credits != null)
                {
                    Chat.SendTo(player, $"Bank account credits:");
                    foreach (var credit in credits)
                    {
                        Chat.SendTo(player, $"  долг: {credit.amount}$, погашено на текущем этапе: {credit.payedAmount}$");
                    }
                    Chat.SendTo(player, "--------------------------");
                }

                GameLog.Admin($"{player.Name}", $"checkMoney", $"{target.Name}");
            }
            catch (Exception e) { _logger.WriteError($"checkMoney:\n{e}"); }
        }
        
        public static void teleportToPlayer(PlayerGo admin, PlayerGo target)
        {
            if (!Group.CanUseAdminCommand(admin, "tp")) return;
            if (target.GetCharacter().AdminLVL >= 7 && admin.GetCharacter().AdminLVL < target.GetCharacter().AdminLVL) return;
            admin.ChangePosition(target.Position + new Vector3(1, 0, 1.5));
            admin.Dimension = target.Dimension;

            AdminParticles.PlayAdminAppearanceEffect(admin);
        }
        
        public static void teleportTargetToPlayer(PlayerGo player, PlayerGo target, bool withveh = false)
        {
            if (!Group.CanUseAdminCommand(player, "metp")) return;
            if (target.GetCharacter().AdminLVL >= 7 && player.GetCharacter().AdminLVL < target.GetCharacter().AdminLVL) return;
            if (!withveh) {
                GameLog.Admin($"{player.Name}", $"metp", $"{target.Name}");
                target.ChangePosition(player.Position);
                target.Dimension = player.Dimension;
            } else {
                if (!target.IsInVehicle) return;
                target.ChangePosition(null);
                target.Vehicle.Position = player.Position + new Vector3(2,2,2);
                target.Vehicle.Dimension = player.Dimension;
                GameLog.Admin($"{player.Name}", $"gethere", $"{target.Name}");
            }
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_50".Translate(target.Name), 3000);
            Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_51".Translate(player.Name), 3000);
        }
        public static void freezeTarget(PlayerGo player, PlayerGo target)
        {
            if (!Group.CanUseAdminCommand(player, "frz")) return;

            if (player.GetCharacter().AdminLVL < target.GetCharacter().AdminLVL)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_10".Translate(), 3000);
                return;
            }

            Trigger.ClientEvent(target, "freeze", true);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_52".Translate( target.Name), 3000);
            GameLog.Admin($"{player.Name}", $"freeze", $"{target.Name}");
        }
        public static void unFreezeTarget(PlayerGo player, PlayerGo target)
        {
            if (!Group.CanUseAdminCommand(player, "unfrz")) return;
            Trigger.ClientEvent(target, "freeze", false);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_53".Translate( target.Name), 3000);
            GameLog.Admin($"{player.Name}", $"unfreeze", $"{target.Name}");
        }
        
        public static void giveTargetGun(PlayerGo player, PlayerGo target, string weapon, bool promo = true)
        {
            if (promo && !Group.CanUseAdminCommand(player, "givegun")) return;
            if (!promo && !Group.CanUseAdminCommand(player, "givegunc")) return;
            if (!Enum.TryParse(weapon, out ItemNames itemName))
                return;
            var item = ItemsFabric.CreateWeapon(itemName, promo);
            if (item == null) 
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_55", 3000);
                return;
            }
            if (!target.GetInventory().AddItem(item))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_56", 3000);
                return;
            }
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_57".Translate(target.Name, weapon.ToString()), 3000);
            if (promo)
                GameLog.Admin($"{player.Name}", $"giveGun({weapon},{(item as Whistler.Inventory.Models.Weapon).Serial})", $"{target.Name}");
            else
                GameLog.Admin($"{player.Name}", $"giveGunc({weapon},{(item as Whistler.Inventory.Models.Weapon).Serial})", $"{target.Name}");
        }
        public static void giveTargetGunWithComponents(PlayerGo player, PlayerGo target, string weapon, int muzzle, int flash, int clip, int scope, int grip, int skin)
        {
            if (!Group.CanUseAdminCommand(player, "giveguncomponents")) return;
            if (!Enum.TryParse(weapon, out ItemNames itemName))
                return;
            var item = ItemsFabric.CreateWeapon(itemName, muzzle, flash, clip, scope, grip, skin, true);
            if (item == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_55", 3000);
                return;
            }
            if (!target.GetInventory().AddItem(item))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_56", 3000);
                return;
            }
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_57".Translate(target.Name, weapon.ToString()), 3000);           
            GameLog.Admin($"{player.Name}", $"giveGunc({weapon},{(item as Whistler.Inventory.Models.Weapon).Serial})", $"{target.Name}");
        }

        public static void giveTargetSkin(PlayerGo player, PlayerGo target, string pedModel)
        {
            if (!Group.CanUseAdminCommand(player, "setskin")) return;
            if(pedModel.Equals("-1")) {
                if(target.HasData("AdminSkin")) {
                    target.ResetData("AdminSkin");
                    target.GetCustomization().Apply(player);
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Core_58", 3000);
                } else {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_59", 3000);
                    return;
                }
            } else {
                PedHash pedHash = NAPI.Util.PedNameToModel(pedModel);
                if (pedHash != 0) {
                    target.SetData("AdminSkin", true);
                    target.SetSkin(pedHash);
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_60".Translate( target.Name, pedModel), 3000);
                } else {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_61", 3000);
                    return;
                }
            }
            GameLog.Admin(player.Name, $"setSkin({pedModel})", target.Name);
        }
        public static void giveTargetClothes(PlayerGo player, PlayerGo target, int type, int drawable, int texture, bool promo)
        {
            if (promo && !Group.CanUseAdminCommand(player, "giveclothes")) return;
            if (!promo && !Group.CanUseAdminCommand(player, "giveclothesc")) return;
            if (!Enum.IsDefined(typeof(ItemNames), type))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_63", 3000);
                return;
            }
            ItemNames itemName = (ItemNames)type;
            var item = ItemsFabric.CreateClothes(itemName, player.GetGender(), drawable, texture, promo);
            if (item == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_63", 3000);
                return;
            }
            if (!target.GetInventory().AddItem(item))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_64", 3000);
                return;
            }
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_65".Translate( target.Name, itemName.ToString()), 3000);
            if (promo)
                GameLog.Admin(player.Name, $"giveClothes({type},{drawable},{texture})", target.Name);
            else
                GameLog.Admin(player.Name, $"giveClothesc({type},{drawable},{texture})", target.Name);
        }
        public static void takeTargetGun(PlayerGo player, PlayerGo target)
        {
            if (!Group.CanUseAdminCommand(player, "oguns")) return;

            if (player.GetCharacter().AdminLVL < target.GetCharacter().AdminLVL)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_10".Translate(), 3000);
                return;
            }

            Weapons.RemoveAll(target, true);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_66".Translate( target.Name), 3000);
            GameLog.Admin($"{player.Name}", $"takeGuns", $"{target.Name}");
        }

        public static void adminSMS(PlayerGo player, PlayerGo target, string message)
        {
            if (!Group.CanUseAdminCommand(player, "pm")) return;
            Chat.AdminSMS(target,$"[Admin]{player.Name} ({player.GetCharacter().UUID}): {message}");
            Chat.AdminSMS(player,$"[Success] for {target.Name} ({target.GetCharacter().UUID}): {message}");
            GameLog.Chat(player.GetCharacter().UUID, (int)ChatType.AdminWarning, message);
        }
       
      
        public static void sendPlayerToDemorgan(PlayerGo admin, PlayerGo target, int time, string reason)
        {
            if (!Group.CanUseAdminCommand(admin, "demorgan")) return;
            if (!target.IsLogged()) return;

            if (admin.GetCharacter().AdminLVL < target.GetCharacter().AdminLVL)
            {
                Notify.Send(admin, NotifyType.Error, NotifyPosition.BottomCenter, "local_10".Translate(), 3000);
                return;
            }
            int firstTime = time * 60;
            if (target.GetAccount().IsPrimeActive())
            {
                firstTime = (int)(firstTime * DonateService.PrimeAccount.DemorganAndArrestMultipler);
            }

            string deTimeMsg = "local_3";
            if (time > 60)
            {
                deTimeMsg = "local_4";
                time /= 60;
                if (time > 24)
                {
                    deTimeMsg = "local_5";
                    time /= 24;
                }
            }

            Chat.AdminToAll("Com_120".Translate(target.Name, time.ToString(), deTimeMsg, reason, admin.Name.Replace('_', ' ')));
            target.GetCharacter().ArrestDate = DateTime.UtcNow;
            PoliceArrests.SetRecordAboutReleasePlayer(target, null, 0);
            target.GetCharacter().DemorganTime = firstTime;
            target.UnCuffed();
            target.LetGoFollower();
            target.UnFollow();

            target.SendTODemorgan();
            if (target.HasData("ARREST_TIMER")) Timers.Stop(target.GetData<string>("ARREST_TIMER"));
            target.SetData("ARREST_TIMER", Timers.StartTask(1000, () => timer_demorgan(target)));
            target.Dimension = 1337;
            Weapons.RemoveAll(target, true);
            RemoveMasks(target);
            GameLog.Admin($"{admin.Name}", $"demorgan({time}{deTimeMsg},{reason})", $"{target.Name}");
            FamilyManager.ChangePoints(target, FamilyActions.GoToDemorgan);
        }
        public static void ReleasePlayerFromDemorgan(PlayerGo admin, PlayerGo target)
        {
            target.GetCharacter().DemorganTime = 0;
            target.TriggerEvent("admin:releaseDemorgan");
            Notify.Send(admin, NotifyType.Warning, NotifyPosition.BottomCenter, $"Core_67".Translate(target.Name), 3000);
            GameLog.Admin($"{admin.Name}", $"undemorgan", $"{target.Name}");
        }

        #region Demorgan
        //public static Vector3 DemorganPosition = new Vector3(1651.217, 2570.393, 44.44485); //OLD
        public static Vector3 DemorganPosition = new Vector3(5012, -4967, 36); //dont royal battle
        //public static Vector3 DemorganPosition = new Vector3(5070, -4883, 18); //for royal battle
        public static int DemorganRange = 1400;
        public static int DemorganHeight = 500;
        public static void timer_demorgan(PlayerGo player)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (player.GetCharacter().DemorganTime <= 0)
                {
                    player.GetCharacter().DemorganTime = 0;
                    player.TriggerEvent("admin:releaseDemorgan");
                    FractionCommands.freePlayer(player);
                    return;
                }
                player.GetCharacter().DemorganTime--;
            }
            catch (Exception e)
            {
                _logger.WriteError($"timer_demorgan:\n{e}");
            }
        }
        #endregion
        // need refactor
        public static void respawnAllCars(PlayerGo player)
        {
            if (!Group.CanUseAdminCommand(player, "spawnallcar")) return;
            var all_vehicles = VehicleManager.Vehicles.Where(item => item.Value.OwnerType == OwnerType.Fraction || item.Value.OwnerType == OwnerType.Job);

            foreach (var vehicle in all_vehicles)
            {
                VehicleGo vehGo = VehicleManager.GetVehicleGo(vehicle.Value.ID);
                if (vehGo == null)
                {
                    vehicle.Value.RespawnVehicle();
                    continue;
                }
                vehGo.Occupants = vehGo.Occupants.Where(item => item.IsLogged()).ToList();
                if (vehGo.Occupants.Count >= 1)
                    continue;
                vehicle.Value.RespawnVehicle();
            }
            GameLog.Admin(player.Name, $"spawnallcar", $"");
        }

        public static void RemoveMasks(PlayerGo player)
        {
            if (!player.IsLogged()) return;
            if (player.IsAdmin()) return;
            player.GetInventory().RemoveItems(item => item.Name == ItemNames.Mask && !IsBeard(item) && item.Promo == false);
            var equip = player.GetEquip();
            if (equip.Clothes[ClothesSlots.Mask] != null && !equip.Clothes[ClothesSlots.Mask].Promo)
                equip.RemoveItem(player, ClothesSlots.Mask);
        }
        private static bool IsBeard(BaseItem item)
        {
            var cloth = (ClothesBase)item;
            return cloth.Drawable > 499 && cloth.Drawable < 507;
        }
    }

    public class Group
    {
        public static List<GroupCommand> GroupCommands = new List<GroupCommand>();
        public static void LoadCommandsConfigs()
        {
            DataTable result = MySQL.QueryRead($"SELECT * FROM adminaccess");
            if (result == null || result.Rows.Count == 0) return;
            List<GroupCommand> groupCmds = new List<GroupCommand>();
            foreach (DataRow Row in result.Rows)
            {
                string cmd = Convert.ToString(Row["command"]);
                bool isadmin = Convert.ToBoolean(Row["isadmin"]);
                int minrank = Convert.ToInt32(Row["minrank"]);
                bool istechnical = Convert.ToBoolean(Row["istech"]);

                groupCmds.Add(new GroupCommand(cmd, isadmin, minrank, istechnical));
            }
            GroupCommands = groupCmds;
        }

        public static List<string> GroupNames = new List<string>()
        {
            "local_6",
            "Prime Account",
        };
        private static List<double> GroupPayAdd = new List<double>()
        {
            1.0,
            1.35,
        };
        private static List<int> GroupAddPayment = new List<int>()
        {
            0,
            700
        };
        public static List<int> GroupMaxContacts = new List<int>()
        {
            50,
            100,
        };
        public static List<int> GroupMaxBusinesses = new List<int>()
        {
            1,
            1,
        };
        private static List<int> GroupEXP = new List<int>()
        {
            1,
            3,
        };

      
        public static bool CanUseAdminCommand(Player player, string cmd, bool notify = true)
        {
            if (!player.IsLogged())
                return false;
            GroupCommand command = GroupCommands.FirstOrDefault(c => c.Command == cmd);
            if (command == null)
            {
                MySQL.Query($"INSERT INTO `adminaccess`(`command`, `isadmin`, `minrank`) VALUES (@prop0, 1, 10)", cmd);
                command = new GroupCommand(cmd, true, 10, false);
                GroupCommands.Add(command);
            }
            if (command.IsAdmin)
            {
                var adminLvl = player.GetCharacter().AdminLVL;
                if (command.MinLVL <= adminLvl || (command.IsTechnical && adminLvl == -1))
                    return true;
            }
            if (notify)
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_69", 3000);
            return false;
        }

        public class GroupCommand
        {
            public GroupCommand(string command, bool isAdmin, int minlvl, bool isTechnical)
            {
                Command = command;
                IsAdmin = isAdmin;
                MinLVL = minlvl;
                IsTechnical = isTechnical;
            }

            public string Command { get; }
            public bool IsAdmin { get; }
            public bool IsTechnical { get; set; }
            public int MinLVL { get; set; }
        }
    }
}
