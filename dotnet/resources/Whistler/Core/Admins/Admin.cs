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
    public delegate void SetPlayerToAdminGroupDelegate(ExtPlayer player);
    public delegate void DeletePlayerFromAdminGroupDelegate(ExtPlayer player);

    class Admin : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Admin));
        public static bool IsServerStoping = false;

        public static SetPlayerToAdminGroupDelegate SetPlayerToAdminGroup;
        public static DeletePlayerFromAdminGroupDelegate DeletePlayerFromAdminGroup;


        [RemoteEvent("Server_GetJailBack")]
        public static void GetJailBack(ExtPlayer target){
            try
            {
                target.ChangePosition(PrisonFib.randomPrisonpointFib());
            }
            catch (Exception e) { _logger.WriteError("Server_GetJailBack: " + e.ToString()); }
        }

        
        // [Command("testjail")]
        // public static void CMD_TestJail(ExtPlayer player, int time)
        // {
        //     try
        //     {
        //         // if (!Group.CanUseAdminCommand(player, "offcheckmoney")) return;
        //         player.ChangePosition(PrisonFib.randomPrisonpointFib());
        //         player.UnCuffed();
        //         Weapons.RemoveAll(player, true);
        //         // target.ResetData("putprison");

        //         SafeTrigger.SetData(player, "ARREST_TIMER", Timers.StartTask(1000, () => PrisonFib.timer_prisFib(player)));
        //         player.Character.CourtTime = time;
        //         player.Character.ArrestID = player.Character.FractionID;
        //         Chat.SendTo(player, $"{player.Name} посадил Вас в тюрьму на {time} минут");
        //         Chat.SendTo(player, $"Вы посадили в тюрьму {player.Name} на {time} минут");
        //         SafeTrigger.ClientEvent(player, "Client_CheckIsInJail");
        //         //Client_CheckIsInJail
                
        //     }
        //     catch (Exception e) { _logger.WriteError($"testjail:\n{e}"); }
        // }

        // [Command("checkprison")]
        // public static void CMD_CheckPrison(ExtPlayer player, int id)
        // {
        //     try
        //     {
        //         // if (!Group.CanUseAdminCommand(player, "offcheckmoney")) return;
        //         player.ChangePosition(PrisonFib.checkPrison(id));
        //         Chat.SendTo(player, $"{id}");
                
        //     }
        //     catch (Exception e) { _logger.WriteError($"testjail:\n{e}"); }
        // }


        [Command("setsendexceptionstatus")]
        public void CreateTeleportPoint(ExtPlayer player, int value)
        {
            if ((player?.Character?.AdminLVL ?? 0) < 10) return;
            Main.ServerConfig.Main.SendClientExceptions = value != 0;
            NAPI.ClientEvent.TriggerClientEventForAll("SendClientExceptions", Main.ServerConfig.Main.SendClientExceptions);
        }

        [Command("testid")]
        public void testID(ExtPlayer player, int id)
        {
            SafeTrigger.ClientEvent(player,"toggleTestInv", id);
        }

        // [Command("testcam")]
        // public void testcam(ExtPlayer player, int id)
        // {
        //     NAPI.Util.ConsoleOutput($"{player.Character.AdminLVL}");
        //     var target = Trigger.GetPlayerByUuid(id);
        //     if (target == null) return;
        //     player.SendChatMessage($"{player.isFriend(target)}");
        // }

        [Command("createtp")]
        public void CreateTeleportPoint(ExtPlayer player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "createtp")) return;

                if (!player.HasData("TELEPORT:CREATING"))
                {
                    SafeTrigger.SetData(player, "TELEPORT:CREATING", new Tuple<Vector3, uint>(player.Position - new Vector3(0, 0, 1), player.Dimension));
                    player.SendChatMessage("Вы создали точку входа, поставьте точку выхода");
                }
                else
                {
                    var enterPoint = player.GetData<Tuple<Vector3, uint>>("TELEPORT:CREATING");

                    Teleports.CreateTeleport(enterPoint.Item1, enterPoint.Item2, player.Position - new Vector3(0, 0, 1), player.Dimension);
                    player.ResetData("TELEPORT:CREATING");

                    player.SendChatMessage("Телепорт создан");
                }
            }
            catch (Exception e) { _logger.WriteError($"CreateTeleportPoint:\n{e}"); }
        }
        [Command("deletetp")]
        public void DeleteTeleportPoint(ExtPlayer player)
        {
            try
            {
                Teleports.DeleteTeleport(player);
            }
            catch (Exception e) { _logger.WriteError($"DeleteTeleportPoint:\n{e}"); }
        }

        [Command("hpf")]
        public void ResetLifeParameters(ExtPlayer player, int id, int hp = 100, int hunger = 100, int thirst = 100)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "hpf")) return;

                ExtPlayer target = Trigger.GetPlayerByUuid(id);
                if (target == null) return;

                var character = target.Character;
                hunger = hunger > 100 ? 100 : hunger < 0 ? 0 : hunger;
                thirst = thirst > 100 ? 100 : thirst < 0 ? 0 : thirst;
                hp = hp > 100 ? 100 : hp < 0 ? 0 : hp;
                character.LifeActivity.Hunger.Level = hunger;
                character.LifeActivity.Thirst.Level = thirst;
                LifeSystem.LifeActivity.Sync(player);
                target.Health = hp;
                GameLog.Admin($"{player.Name}", $"healPlayerFull({hp}, {hunger}, {thirst})", $"{target.Name}");
                player.SendChatMessage($"Вы успешно установили для {id} ID {hp} HP, {hunger} голода и {thirst} жажды.");
            }
            catch (Exception e) { _logger.WriteError($"ResetLifeParameters:\n{e}"); }
        }

        [Command("ohv")]
        public void ShowAdminHuntingVision(ExtPlayer player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "ohv")) return;

                SafeTrigger.ClientEvent(player,"hunting:toggleAdminVision");
            }
            catch (Exception e) { _logger.WriteError($"ShowAdminHuntingVision:\n{e}"); }
        }

        [RemoteEvent("saveObjectPos")]
        public void SaveObjectPosition(ExtPlayer player, string pos, string rotation, string camPos)
        {
            using (var w = new StreamWriter("ObjectPos.txt", true))
            {
                w.WriteLine($"pos: {pos} rot: {rotation} gameplayCam: {camPos}");
            }
        }

        internal static void CheckInventory(ExtPlayer player, ExtPlayer target)
        {
            var inventory = target.GetInventory()?.GetInventoryData();
            var equip = target.GetEquip()?.GetEquipData();
            SafeTrigger.ClientEvent(player,"admin:checkinventory:responce", equip, inventory);
        }

        [ServerEvent(Event.ResourceStart)]
        public void Event_ResourceStart()
        {
            ColShape colShape = NAPI.ColShape.CreateCylinderColShape(DemorganPosition, DemorganRange, DemorganHeight, uint.MaxValue);
            colShape.OnEntityExitColShape += (s, e) =>
            {
                if (!(e is ExtPlayer eGo)) return;

                if (!eGo.IsLogged()) return;
                if (eGo.Character.DemorganTime > 0)
                {
                    SafeTrigger.UpdateDimension(eGo, 1337);
                    eGo.SendTODemorgan();
                }
            };
            Group.LoadCommandsConfigs();
        }

        [RemoteEvent("invisible")]
        public static void SetInvisible(ExtPlayer player, bool toggle)
        {
            if (player == null || player.Character == null || player.Character.AdminLVL <= 0) return;

            SafeTrigger.SetSharedData(player, "INVISIBLE", toggle);
            player.Transparency = toggle ? 0 : 255;
            player.Session.Invisible = toggle;
        }

        //[Command("setvehicledamagemodifier")]
        //public void setVehicleDamageModifier(ExtPlayer player, double value)
        //{
        //    player.Eval($"mp.players.local.setVehicleDamageModifier({value});");
        //}

        //[Command("setvehicledefensemodifier")]
        //public void ShowBusinessMarkers(ExtPlayer player, double value)
        //{
        //    player.Eval($"mp.players.local.setVehicleDefenseModifier({value});");
        //}

        [Command("getanimalspos")]
        public void GetAnimalsPosition_ADMINCMD(ExtPlayer player)
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
        public void ShowBusinessMarkers(ExtPlayer player)
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
                SafeTrigger.ClientEvent(player,"businesses:setMarkers", JsonConvert.SerializeObject(markers));
            }
            catch (Exception e) { _logger.WriteError($"ShowBusinessMarkers:\n{e}"); }
        }

        [Command("bizhidemarkers")]
        public void HideBusinessMarkers(ExtPlayer player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "bizhidemarkers")) return;

                SafeTrigger.ClientEvent(player,"businesses:clearMarkers");
            }
            catch (Exception e) { _logger.WriteError($"HideBusinessMarkers:\n{e}"); }
        }

        [Command("changebizenterpoint")]
        public void ChangeBusinessEnterPoint(ExtPlayer player, int bizId, int range)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "changebizenterpoint")) return;

                BusinessManager.BizList[bizId].ColshapeRange = range;
                BusinessManager.BizList[bizId].ChangeEnterPoint(player.Position - new Vector3(0, 0, 1.12));
                BusinessManager.BizList[bizId].Save();
            }
            catch (Exception e) { _logger.WriteError($"ChangeBusinessEnterPoint:\n{e}"); }
        }

        [Command("addbizenterpoint")]
        public void AddBizEnterpoint(ExtPlayer player, int bizId)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "addbizenterpoint")) return;

                BusinessManager.BizList[bizId].AdditionalPositions.Add(player.Position);
                MySQL.Query("UPDATE businesses SET additionalpos=@prop0 WHERE id=@prop1", JsonConvert.SerializeObject(BusinessManager.BizList[bizId].AdditionalPositions), bizId);
                BusinessManager.BizList[bizId].CreateEnterColshape();
            }
            catch (Exception e) { _logger.WriteError($"AddBizEnterpoint:\n{e}"); }
        }

        [Command("clearbizenterpoint")]
        public void ClearBizEnterpoint(ExtPlayer player, int bizId)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "clearbizenterpoint")) return;

                BusinessManager.BizList[bizId].AdditionalPositions = new List<Vector3>();
                MySQL.Query("UPDATE businesses SET additionalpos=@prop0 WHERE id=@prop1", JsonConvert.SerializeObject(BusinessManager.BizList[bizId].AdditionalPositions), bizId);
                BusinessManager.BizList[bizId].CreateEnterColshape();
            }
            catch (Exception e) { _logger.WriteError($"ClearBizEnterpoint:\n{e}"); }
        }

        [Command("createbizped", GreedyArg = true)]
        public void CreateBusinessPed(ExtPlayer player, int bizId, int model, string pedName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "createbizped")) return;

                if (!BusinessManager.BizList.ContainsKey(bizId))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Бизнес с таким ID не существует", 3000);
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
                BusinessManager.BizList[bizId].Save();
            }
            catch (Exception e) { _logger.WriteError($"CreateBusinessPed:\n{e}"); }
        }

        [Command("getbizpeds")]
        public void GetBusinessPedsInfo(ExtPlayer player, int bizId)
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
        public void RemoveBizPed(ExtPlayer player, int bizId, int pedIndex)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "removebizped")) return;

                BusinessManager.BizList[bizId].Peds.RemoveAt(pedIndex);

                NAPI.ClientEvent.TriggerClientEventForAll("businesses:clearPeds");
                NAPI.ClientEvent.TriggerClientEventForAll("businesses:setPeds",
                JsonConvert.SerializeObject(BusinessManager.BizList.SelectMany(b => b.Value.Peds)));
                BusinessManager.BizList[bizId].Save();
            }
            catch (Exception e) { _logger.WriteError($"RemoveBizPed:\n{e}"); }
        }

        [Command("setcasinomaxwin")]
        public void setCasinoMaxWin(ExtPlayer player, int amount)
        {
            if (!Group.CanUseAdminCommand(player, "setcasinomaxwin")) return;
            if (amount < 500000)
            {
                Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Имейте совесть дайте людям выиграть хоть 500 000", 3000);
                return;
            }
            MySQL.Query("UPDATE `casino` set `maxWinOfBet`=@prop0", amount);
            ServerGo.Casino.Games.Roulette.RouletteGame.MaxWin = amount;
        }

        [Command("resetcharacter")]//resetcharacter Wedfeqwd_Ewdwq
        public static void ResetCharacter(ExtPlayer player, string name)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "offresetcharacter")) return;

                if (!Main.PlayerUUIDs.ContainsKey(name))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок не найден", 3000);
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

                ExtPlayer target = Trigger.GetPlayerByUuid(uuid);
                if (target != null)
                {
                    target.Character.CasinoChips = new int[5];
                    target.Character.AdminLVL = 0;
                    SafeTrigger.ClientEvent(target, "setadminlvl", 0);
                    target.Character.Licenses = new List<GUI.Documents.Models.License>();
                    MoneySystem.Wallet.SetBankMoney(target.Character.BankNew, 0);
                    target.Character.Money = 0;
                    target.Character.Inventory.Reset();
                    target.Character.Equip.Reset();
                    target.Character.DonateInventory.Reset();
                    target.Character.Save();
                }
                else
                {
                    MoneySystem.Wallet.SetBankMoneyByUUID(uuid, 0);
                    InventoryService.GetByUUID(uuid)?.Reset();
                    EquipService.GetByUUID(uuid)?.Reset();
                    DonateService.GetInventoryByUUID(uuid)?.Reset();
                    MySQL.QuerySync("UPDATE `characters` SET `money`=0, `biz`='[]', `chips`=null, `licenses`='[]', `adminlvl`=0 WHERE `uuid`=@prop0", uuid);
                }

                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы обнулили аккаунт {name}", 3000);
                GameLog.Admin(player.Name, $"resetcharacter", name);
                Trigger.GetPlayerByUuid(uuid)?.Kick();
            }
            catch (Exception e) { _logger.WriteError($"ResetCharacter:\n{e}"); }
        }


        [Command("checkbizmoney")]
        public static void CheckBusinessMoney(ExtPlayer player, int bizid)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "checkbizmoney")) return;

                var biz = BusinessManager.BizList.Values.FirstOrDefault(b => b.ID == bizid);
                if (biz == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Бизнеса с таким ID не существует", 3000);
                    return;
                }

                Chat.SendTo(player, "checkbizmoney".Translate(bizid, biz.BankNalogModel.Balance));
            }
            catch (Exception e) { _logger.WriteError($"CheckBusinessMoney:\n{e}"); }
        }

        [Command("checkhousemoney")]
        public static void Checkhousemoney(ExtPlayer player, int houseId)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "checkhousemoney")) return;

                var house = HouseManager.GetHouseById(houseId);
                if (house == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Дома с таким ID не существует", 3000);
                    return;
                }

                Chat.SendTo(player, "checkhousemoney".Translate(houseId, house.BankModel.Balance));
            }
            catch (Exception e) { _logger.WriteError($"Checkhousemoney:\n{e}"); }
        }

        [Command("respawnfractioncars")]
        public static void RespawnFractionCars(ExtPlayer player, int fractionId)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "respawnfractioncars")) return;
                FractionCommands.RespawnFractionCars(fractionId);

                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Машины {fractionId.ToString()} фракции успешно зареспавнены", 3000);
                GameLog.Admin(player.Name, $"respawnfractioncars({fractionId})", "");
            }
            catch (Exception e) { _logger.WriteError($"RespawnFractionCars:\n{e}"); }
        }

        [Command("takedonate")]
        public static void takeDonate(ExtPlayer player, int id, int amount)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "takedonate")) return;

                var target = Trigger.GetPlayerByUuid(id);
                if (!target.IsLogged()) return;

                if (player.Character.AdminLVL < target.Character.AdminLVL)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы не можете применить команду к этому администратору", 3000);
                    return;
                }
                var coins = target.Account.MCoins;
                if (coins <  amount) amount = coins;

                target.SubMCoins(amount);
                SafeTrigger.ClientEvent(target, "starset", coins);

                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы забрали у {target.Name} {amount.ToString()} Donate Coins", 3000);
                Notify.Send(target, NotifyType.Success, NotifyPosition.BottomCenter, $"-{amount} Donate Coins", 3000);
                GameLog.Admin(player.Name, $"takedonate({amount})", target.Name);
            }
            catch (Exception e) { _logger.WriteError($"takeDonate:\n{e}"); }
        }

        [Command("changephonenumber")]
        public static void ChangePhoneNumber(ExtPlayer player, int targetId, int newNumber)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "changephonenumber")) return;

                var target = Trigger.GetPlayerByUuid(targetId);
                if (!target.IsLogged()) return;
                if (PhoneLoader.ChangeNumber(target, newNumber))
                    Notify.SendSuccess(player, $"Вы успешно сменили номер игроку {target.Name}");
                else
                    Notify.SendError(player, "Ошибка, проверьте номер телефона");
            }
            catch (Exception e) { _logger.WriteError($"takeDonate:\n{e}"); }
        }

        public static void GiveDonatePoints(ExtPlayer player, ExtPlayer target, int amount)
        {
            if (!Group.CanUseAdminCommand(player, "givedonate")) return;
            if (target.Account.MCoins + amount < 0) amount = 0;
            target.AddMCoins(amount);
            SafeTrigger.ClientEvent(target, "starset", target.Account.MCoins);

            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы дали {target.Name} {amount.ToString()} Donate Coins", 3000);
            Notify.Send(target, NotifyType.Success, NotifyPosition.BottomCenter, $"+{amount} Donate Coins", 3000);

            GameLog.Admin(player.Name, $"givedonate({amount})", target.Name);
        }
     
        public static void ServerRestart(string name, string reason)
        {
            IsServerStoping = true;
            GameLog.Admin(name, $"stopServer{reason}", "");
            foreach (ExtPlayer player in Trigger.GetAllPlayers())
                NAPI.Player.KickPlayer(player, reason);

            BusinessManager.SavingBusiness();
            InventoryService.SaveAllInventories();
            EquipService.SaveAllEquips();
            Manager.SaveStocksDic();
            VehicleManager.SaveFamilyCars();
            FamilyManager.SavingFamilies();
            PromoCodesService.Save();

            NAPI.Task.Run(() =>
            {
                Environment.Exit(0);
            }, 60000);
            Infrastructure.DataAccess.DbManager.SaveDatabase();
            Whistler.EFCore.DBManager.SaveDatabase();
        }


        public static void saveCoords(ExtPlayer player, string msg)
        {
            if (!Group.CanUseAdminCommand(player, "scoord")) return;
            Vector3 pos = NAPI.Entity.GetEntityPosition(player);
            Vector3 rot = NAPI.Entity.GetEntityRotation(player);
            if (NAPI.Player.IsPlayerInAnyVehicle(player))
            {
                var vehicle = (ExtVehicle)player.Vehicle;
                pos = NAPI.Entity.GetEntityPosition(vehicle) + new Vector3(0, 0, 0.5);
                rot = NAPI.Entity.GetEntityRotation(vehicle);
            }

            SaveCoord(pos, rot, msg);
            Chat.SendTo(player, $"Coords: {NAPI.Entity.GetEntityPosition(player)}");
        }
        public static void SaveCarCoords(ExtPlayer player, string msg)
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
        public static void Command_GetStrangeAmountOfCoins(ExtPlayer player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "wtfcoins")) return;

                Chat.SendTo(player, "Люди с большим количеством Donate Coins:");

                var result = MySQL.QueryRead("SELECT login, mcoins FROM accounts WHERE mcoins > 10000");
                if (result == null) return;

                foreach (DataRow row in result.Rows)
                {
                    var login = row["login"].ToString();
                    var gobucks = Convert.ToInt32(row["mcoins"]);

                    Chat.SendTo(player, $"{login} имеет {gobucks} Donate Coins");
                }

                Chat.SendTo(player, "Люди с большим количеством $ на руках:");

                result = MySQL.QueryRead("SELECT firstname,lastname,money FROM characters WHERE money>10000000");
                if (result == null) return;

                foreach (DataRow row in result.Rows)
                {
                    var fisrtname = row["firstname"].ToString();
                    var lastname = row["lastname"].ToString();
                    var money = Convert.ToInt32(row["money"]);

                    Chat.SendTo(player, $"{fisrtname} {lastname} имеет {money}$ на руках");
                }

                Chat.SendTo(player, "Люди с большим количеством $ в банке:");

                foreach (var bankAccount in BankManager.GetAccountsByPredicate(item => item.OwnerType == TypeBankAccount.Player && item.Balance > 10000000))
                {
                    Chat.SendTo(player, $"{Main.PlayerNames.GetValueOrDefault(bankAccount.UUID, "Unknown")} имеет {bankAccount.Balance}$ в банке");
                }
            }
            catch (Exception e) { _logger.WriteError($"Command_GetStrangeAmountOfCoins:\n{e}"); }
        }        

        [Command("coins")]
        public static void Command_GetFullTargetCoinsInfo(ExtPlayer player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "getcoinsinfo")) return;

                var target = Trigger.GetPlayerByUuid(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок не найден!", 3000);
                    return;
                }

                var account = target.Account;
                Chat.SendTo(player, $"Сейчас у {target.Name} {account.MCoins} Donate Coins");
                Chat.SendTo(player, "Истории покупок Donate Coins:");
                var queryResult = MySQL.QueryRead("SELECT `sum`, `date` FROM `@prop1` WHERE `login`=@prop0 AND `unitpayid` > 0", account.Login.ToLower(), Main.ServerConfig.DonateConfig.Database);
                if (queryResult == null || queryResult.Rows.Count == 0)
                {
                    Chat.SendTo(player, "Игрок не покупал Donate Coins");
                    return;
                }

                foreach (DataRow row in queryResult.Rows)
                {
                    var date = row["date"].ToString();
                    var value = row["sum"].ToString();

                    Chat.SendTo(player, $"{date} купил {value} Donate Coinms");
                }
            }
            catch (Exception e) { _logger.WriteError($"Command_GetFullTargetCoinsInfo:\n{e}"); }
        }
              
        [Command("arenapoints")]
        public static void GetArenaPoints(ExtPlayer player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "arenapoints")) return;

                if (Trigger.GetPlayerByUuid(id) == null)
                {
                    Chat.SendTo(player, "Игрок не найден");
                    return;
                }

                Chat.SendTo(player, $"У игрока {Trigger.GetPlayerByUuid(id).Character.ArenaPoints} арена-очков");
            }
            catch (Exception e) { _logger.WriteError($"GetArenaPoints:\n{e}"); }
        }
       
        [Command("admhelp")]
        public static void ahelp(ExtPlayer player)
        {
            try
            {
                if (player.Character.AdminLVL == 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы не Администратор", 3000);
                    return;
                }

                int adminLvl = player.Character.AdminLVL;
                var cmdGroupsByLvl = Group.GroupCommands.Where(cmd => cmd.MinLVL <= adminLvl)
                                              .OrderBy(cmd => cmd.MinLVL)
                                              .GroupBy(cmd => cmd.MinLVL, cmd => cmd.Command);

                foreach (var cmdGroup in cmdGroupsByLvl)
                {
                    var lvl = cmdGroup.Key;
                    Chat.SendTo(player,cmdGroup.Aggregate($"[{lvl}] /", (current, next) => current + ", /" + next));
                }
                Chat.SendTo(player, "Все команды хранятся в логах");
            }
            catch (Exception e) { _logger.WriteError($"ahelp:\n{e}"); }
        }

        [Command("setacommandtech")]
        public static void Command_SetAdminCommandTechnical(ExtPlayer player, string command, bool istech)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "setacommandtech")) return;

                var cmd = Group.GroupCommands.FirstOrDefault(c => c.Command == command);
                if (cmd == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Команда не найдена", 3000);
                    return;
                }

                cmd.IsTechnical = istech;
                MySQL.Query("UPDATE adminaccess SET istech=@prop0 WHERE command=@prop1", istech, command);

                var noWord = istech ? "" : "not";
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Команда {command} теперь {noWord} техническая", 3000);
            }
            catch (Exception e) { _logger.WriteError($"Command_SetAdminCommandTechnical:\n{e}"); }
        }

        [Command("setacommandlvl")]
        public static void Command_SetCommandLvl(ExtPlayer player, string command, int newLvl)
        {
            if (!Group.CanUseAdminCommand(player, "setacommandlvl")) return;

            if (command == "setacommandlvl")
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Нельзя применить на эту команду", 3000);
                return;
            }

            var cmd = Group.GroupCommands.FirstOrDefault(c => c.Command == command);
            if (cmd == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Команда не найдена", 3000);
                return;
            }

            cmd.MinLVL = newLvl;
            MySQL.Query("UPDATE adminaccess SET minrank=@prop0 WHERE command=@prop1", newLvl, command);

            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Минимальный уровень доступа для команды {command} был изменен на {newLvl.ToString()}", 3000);
            GameLog.Admin(player.Name, $"setacommandlvl({command},{newLvl})", "");
        }

        [Command("givelic")]
        public static void GiveLic(ExtPlayer player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "givelic")) return;
            var target = Trigger.GetPlayerByUuid(id);
            if (target == null) return;
            target.GiveLic(Enum.GetValues(typeof(LicenseName)).Cast<LicenseName>());
            Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"{player.Name.Replace('_', ' ')} дал вам всё лицензии", 3000);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы выдали игроку {target.Name.Replace('_', ' ')} всё лицензии", 3000);
            MainMenu.SendStats(target);
            GameLog.Admin(player.Name, $"givelic", target.Name);
        }

        [Command("atakelic")]
        public static void TakeLic(ExtPlayer player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "givelic")) return;
            var target = Trigger.GetPlayerByUuid(id);
            if (target == null) return;

            target.TakeLic(Enum.GetValues(typeof(LicenseName)).Cast<LicenseName>());

            Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"{player.Name.Replace('_', ' ')} забрал все лицензии", 3000);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы забрали у игрока {target.Name.Replace('_', ' ')} все лицензии", 3000);
            MainMenu.SendStats(target);
            GameLog.Admin(player.Name, $"atakelic", target.Name);
        }

        [Command("giveonelic")]
        public static void GiveOneLic(ExtPlayer player, int id, int index)
        {
            if (!Group.CanUseAdminCommand(player, "giveonelic")) return;
            var target = Trigger.GetPlayerByUuid(id);
            if (target == null) return;
            if (Enum.IsDefined(typeof(LicenseName), index))
            {
                target.GiveLic((LicenseName)index);
                Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"{player.Name.Replace('_', ' ')} дал вам лицензию {((LicenseName)index).ToString()}", 3000);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы выдали игроку {target.Name.Replace('_', ' ')} лицензию {((LicenseName)index).ToString()}", 3000);
                MainMenu.SendStats(target);
                GameLog.Admin(player.Name, $"giveonelic({index})", target.Name);
            }
            else
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Лицензия {index.ToString()} не найдена", 3000);
        }

        private const int MuteDistance = 20;
        private const int MuteTime = 10;

        [RemoteEvent("media:mute:press")]
        public static void OnMediaPressKeyMute(ExtPlayer player)
        {
            var character = player.Character;
            if (character == null || (character.Media < 1 && character.AdminLVL < 8)) return;
            character.MediaMuted = !character.MediaMuted;
            SafeTrigger.ClientEvent(player,"media:mute:state", character.MediaMuted);
            var msg = character.MediaMuted ? $"Медийный партнёр c ID: {player.Character.UUID} включил Mute. Возможно ему нужна помощь." : $"Медийный партнёр c ID: {player.Character.UUID} отключил Mute.";
            Chat.SendToAdmins(3, msg);
        }

        [RemoteEvent("srv_consoleLog")]
        public void OnConsolelog(ExtPlayer client, string log)
        {
            _logger.WriteClient(log);
        }
        public static void setPlayerAdminGroup(ExtPlayer player, ExtPlayer target)
        {
            if (!Group.CanUseAdminCommand(player, "makeadmin")) return;
            if (target.Character.AdminLVL >= 1)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У игрока уже есть админ. права", 3000);
                return;
            }
            target.Character.AdminLVL = 1;
            SafeTrigger.ClientEvent(target, "setadminlvl", 1);
            target.SetSharedData("ALVL", 1);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы выдали админ. права игроку {target.Name}", 3000);
            Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы были назначены администратором 1-го уровня.", 3000);
            SetPlayerToAdminGroup?.Invoke(target);
            GameLog.Admin($"{player.Name}", $"makeAdmin", $"{target.Name}");
        }
        public static void delPlayerAdminGroup(ExtPlayer player, ExtPlayer target)
        {
            if (!Group.CanUseAdminCommand(player, "takeadmin")) return;
            if (player == target)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы не можете забрать админ. права у себя", 3000);
                return;
            }
            if (target.Character.AdminLVL >= player.Character.AdminLVL)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы не можете забрать права у этого администратора", 3000);
                return;
            }
            if (target.Character.AdminLVL < 1)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У игрока нет админ. прав", 3000);
                return;
            }
            target.Character.AdminLVL = 0;
            SafeTrigger.ClientEvent(target, "setadminlvl", 0);
            target.SetSharedData("ALVL", 0);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы забрали права у администратора {target.Name}", 3000);
            Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"{player.Name} забрал у Вас админ. права", 3000);
            DeletePlayerFromAdminGroup?.Invoke(target);
            GameLog.Admin($"{player.Name}", $"takeadmin", $"{target.Name}");
        }
        public static void setPlayerAdminRank(ExtPlayer player, ExtPlayer target, int rank)
        {
            if (!Group.CanUseAdminCommand(player, "changeadminrank")) return;
            if (player == target)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы не можете установить себе ранг", 3000);
                return;
            }
            if (target.Character.AdminLVL < 1)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок не является администратором!", 3000);
                return;
            }
            if (target.Character.AdminLVL >= player.Character.AdminLVL)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы не можете изменить уровень прав у этого администратора", 3000);
                return;
            }
            if (rank < -1 || rank >= player.Character.AdminLVL)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Невозможно выдать такой ранг", 3000);
                return;
            }
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы выдали игроку {target.Name}, {rank.ToString()} уровень админ. прав", 3000);
            Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"{player.Name} выдал Вам {rank.ToString()} уровень админ. прав", 3000);
            target.Character.AdminLVL = rank;
            target.SetSharedData("ALVL", rank);
            SafeTrigger.ClientEvent(target, "setadminlvl", rank);
            GameLog.Admin($"{player.Name}", $"setAdminRank({rank})", $"{target.Name}");
        }
        public static void setPlayerPrimeAccount(ExtPlayer player, ExtPlayer target, int days)
        {
            if (!Group.CanUseAdminCommand(player, "setprime")) return;
            if (days < 1)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Укажите количетво дней больше 0", 3000);
                return;
            }
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы выдали {target.Name} прайм аккаунт на {days.ToString()} дней", 3000);
            target.AddPrime(days);
            MainMenu.SendStats(target);
            GameLog.Admin($"{player.Name}", $"setPlayerPrimeAccount({days} days)", $"{target.Name}");
        }

        public static void setFracLeader(ExtPlayer sender, ExtPlayer target, int fracid)
        {
            if (!Group.CanUseAdminCommand(sender, "giveleader")) return;
            var fraction = Manager.GetFraction(fracid);

            if (fraction != null)
            {
                var targetCharacter = target.Character;
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


                Notify.Alert(target, $"Вы стали лидером фракции {Manager.getName(fracid)}");
                Notify.Send(sender, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы поставили {target.Name} на лидерство {Manager.getName(fracid)}", 3000);
                MainMenu.SendStats(target);
                target.SendTip("Получив статус лидера организации управляйте ею при помощи М меню");
                GameLog.Admin($"{sender.Name}", $"setFracLeader({fracid})", $"{target.Name}");
            }
        }
        [Command("takeadminoff")]
        public static void delAdminOffline(ExtPlayer player, string name)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "takeadminoff")) return;

                if (!Main.PlayerNames.ContainsValue(name))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Игрок не найден", 3000);
                    return;
                }

                ExtPlayer target = Trigger.GetPlayerByName(name);
                if (target != null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Этот игрок сейчас онлайн ({target.Character.UUID}), воспользуйтесь другой командой.", 3000);
                    return;
                }
                var split = name.Split('_');

                MySQL.Query("UPDATE characters SET adminlvl=0 WHERE firstname=@prop0 and lastname=@prop1", split[0], split[1]);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "С игрока успешно снята админка", 3000);
                GameLog.Admin(player.Name, "takeadminoff", name);
            }
            catch (Exception e)
            {
                _logger.WriteError($"delAdminOffline:\n{e}");
            }
        }

        [Command("clearfraction")]
        public static void Command_ClearFraction(ExtPlayer player, int frac)
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

                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Фракция {fraction.ToString()} была очищена", 3000);
                GameLog.Admin(player.Name, $"clearfraction({fraction})", "");
            }
            catch (Exception e)
            {
                _logger.WriteError($"Command_ClearFraction:\n{e}");
            }
        }

        public static void DelJob(ExtPlayer sender, ExtPlayer target)
        {
            if (target.Character.WorkID != 0)
            {
                if (target.Session.OnWork)
                {
                    Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, $"Игрок должен быть не в рабочей форме".Translate(), 3000);
                    return;
                }
                target.Character.WorkID = 0;
                Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"{sender.Name.Replace('_', ' ')} уволил вас с работы", 3000);
                Notify.Send(sender, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы сняли {target.Name.Replace('_', ' ')} с трудоустройства", 3000);
                MainMenu.SendStats(target);
                GameLog.Admin($"{sender.Name}", $"delJob", $"{target.Name}");
            }
            else Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, $"У игрока нет работы", 3000);
        }
        public static void DelFrac(ExtPlayer sender, ExtPlayer target, bool isLeader)
        {

            if (sender.Character.AdminLVL < target.Character.AdminLVL)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "У игрока выше админлвл, чем у Вас", 3000);
                return;
            }
            if (Manager.isLeader(target) && !isLeader)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, $"Игрок лидер фракции", 3000);
                return;
            }
            if (!Manager.isLeader(target) && isLeader)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок не лидер", 3000);
                return;
            }
            if (Manager.GetFraction(target)?.DeleteMember(target.Character.UUID) ?? false)
            {

                if (isLeader)
                {
                    GameLog.Admin($"{sender.Name}", $"delFracLeader", $"{target.Name}");
                    Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"{sender.Name.Replace('_', ' ')} снял Вас с поста лидера фракции", 3000);
                    Notify.Send(sender, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы сняли {target.Name.Replace('_', ' ')} с поста лидера фракции", 3000);
                }
                else
                {
                    GameLog.Admin($"{sender.Name}", $"removefrac", $"{target.Name}");
                    Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Администратор {sender.Name.Replace('_', ' ')} выгнал Вас из фракции", 3000);
                    Notify.Send(sender, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы выгнали {target.Name.Replace('_', ' ')} из фракции", 3000);
                }
            }
            else
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, $"У игрока нет фракции", 3000);
        }

        public static bool GiveMoney(ExtPlayer player, IMoneyOwner target, int amount)
        {
            if (amount > 0)
                return MoneySystem.Wallet.TransferMoney(new ServerMoney(TypeMoneyAcc.Admin, player.Character.UUID), target, amount, 0, "Выдача денег");
            else
                return MoneySystem.Wallet.TransferMoney(target, new ServerMoney(TypeMoneyAcc.Admin, player.Character.UUID), Math.Abs(amount), 0, "Выдача денег");
        }

        public static void OffMutePlayer(ExtPlayer player, string target, int time, string reason)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "mute")) return;
                if (Trigger.GetPlayerByName(target) != null)
                {
                    mutePlayer(player, Trigger.GetPlayerByName(target) as ExtPlayer, time, reason);
                    Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Вы не можете дать mute больше, чем на 480 минут", 3000);
                    return;
                }
                if(player.Name.Equals(target)) return;               
                var split = target.Split('_');
                MySQL.QueryRead("UPDATE `characters` SET `unmutedate`= @prop0 WHERE firstname = @prop1 AND lastname = @prop2", MySQL.ConvertTime(DateTime.Now.AddMinutes(time)), split[0], split[1]);

                Chat.AdminToAll($"{player.Name} выдал мут игроку {target} на {time.ToString()} м причина: {reason}");

                GameLog.Admin($"{player.Name}", $"mutePlayer({time}, {reason})", $"{target}");
            }
            catch { }

        }
        public static void mutePlayer(ExtPlayer player, ExtPlayer target, int time, string reason)
        {
            if (!Group.CanUseAdminCommand(player, "mute")) return;

            if (player.Character.AdminLVL < target.Character.AdminLVL)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У игрока выше админлвл, чем у Вас", 3000);
                return;
            }

            if (player == target) return;
            //if (time > 480)
            //{
            //    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_36", 3000);
            //    return;
            //}
            //target.Character.Unmute = time * 60;
            target.MutePlayer(time);
            Chat.AdminToAll($"{player.Name} выдал мут игроку {target.Name} на {time.ToString()} м причина: {reason}");
            GameLog.Admin($"{player.Name}", $"mutePlayer({time}, {reason})", $"{target.Name}");
        }
        public static void unmutePlayer(ExtPlayer player, ExtPlayer target)
        {
            if (!Group.CanUseAdminCommand(player, "unmute")) return;

            target.Unmute();

            Chat.AdminToAll($"{player.Name} снял мут с игрока {target.Name}");
            GameLog.Admin($"{player.Name}", $"unmutePlayer", $"{target.Name}");
        }
        public static void BanPlayer(ExtPlayer player, ExtPlayer target, int time, string reason, bool isSilence)
        {
            if (player == target) return;
            if(target.Character.AdminLVL >= player.Character.AdminLVL)
            {
                Chat.SendToAdmins(3, $"[BAN-DENIED] {player.Name} ({player.Character.UUID} попытался забанить {target.Name} ({target.Character.UUID}), который имеет выше уровень администратора.");
                return;
            }
            DateTime unbanTime = DateTime.Now.AddDays(time);
            string banTimeMsg = "День";           

            if (!isSilence)
                Chat.AdminToAll($"{player.Name} забанил игрока {target.Name} на {time.ToString()}{banTimeMsg} причина: {reason}");

            Ban.Online(target, unbanTime, false, reason, player.Name);

            Notify.Send(target, NotifyType.Warning, NotifyPosition.Center, $"Ban снимется через: {unbanTime.ToString()}", 30000);
            Notify.Send(target, NotifyType.Warning, NotifyPosition.Center, $"Причина: {reason}", 30000);

            int AUUID = player.Character.UUID;
            int TUUID = target.Character.UUID;
            
            GameLog.Ban(AUUID, TUUID, unbanTime, reason, false);

            target.Kick(reason);
        }
        public static void hardbanPlayer(ExtPlayer player, ExtPlayer target, int time, string reason)
        {
            if (!Group.CanUseAdminCommand(player, "hardban")) return;
            if (player == target) return;
            if(target.Character.AdminLVL >= player.Character.AdminLVL)
            {
                Chat.SendToAdmins(3, $"[HARDBAN-DENIED] {player.Name} ({player.Character.UUID}) попытался заблокировать {target.Name} ({target.Character.UUID}), Администратора высокого уровня.");
                return;
            }
            DateTime unbanTime = DateTime.Now.AddDays(time);
            string banTimeMsg = "День";
           
            Chat.AdminToAll($"{player.Name} заблокировал игрока {target.Name} на {time.ToString()}{banTimeMsg} причина: {reason}");

            Ban.Online(target, unbanTime, true, reason, player.Name);

            Notify.Send(target, NotifyType.Warning, NotifyPosition.Center, $"Блок снимется через: {unbanTime.ToString()}", 30000);
            Notify.Send(target, NotifyType.Warning, NotifyPosition.Center, $"Причина: {reason}", 30000);

            int AUUID = player.Character.UUID;
            int TUUID = target.Character.UUID;

            GameLog.Ban(AUUID, TUUID, unbanTime, reason, true);

            target.Kick(reason);
        }
        public static void hardbanPlayer( ExtPlayer target, int time, string reason)
        {
            DateTime unbanTime = DateTime.Now.AddDays(time);
            string banTimeMsg = "День";

            Chat.AdminToAll($"[Anticheat] заблокировал игрока {target.Name} на { time.ToString()}{banTimeMsg} причина: {reason}");

            Ban.Online(target, unbanTime, true, reason, "[Anticheat]");

            Notify.Send(target, NotifyType.Warning, NotifyPosition.Center, $"Блок снимется через: {unbanTime.ToString()}", 30000);
            Notify.Send(target, NotifyType.Warning, NotifyPosition.Center, $"Причина: {reason}", 30000);

            int TUUID = target.Character.UUID;

            GameLog.Ban(000000, TUUID, unbanTime, reason, true);

            target.Kick(reason);
        }
        public static void offBanPlayer(ExtPlayer player, string name, int time, string reason)
        {
            if (!Group.CanUseAdminCommand(player, "offban")) return;
            if (player.Name == name) return;
            ExtPlayer target = Trigger.GetPlayerByName(name) as ExtPlayer;
            if (target != null) {
                if(target.IsLogged()) {
                    if(target.Character.AdminLVL >= player.Character.AdminLVL)
                    {
                        Chat.SendToAdmins(3, $"[OFFBAN-DENIED] {player.Name} ({player.Character.UUID} попытался забанить {target.Name} ({target.Character.UUID}), который имеет выше уровень администратора.");
                        return;
                    } else {
                        target.Kick();
                        Notify.Send(player, NotifyType.Success, NotifyPosition.Center, "Игрок находился Online, но был кикнут", 3000);
                    }
                }
            } else {
                string[] split = name.Split('_');
                DataTable result = MySQL.QueryRead("SELECT adminlvl FROM characters WHERE firstname = @prop0 AND lastname = @prop1", split[0], split[1]);
                DataRow row = result.Rows[0];
                int targetadminlvl = Convert.ToInt32(row[0]);
                if(targetadminlvl >= player.Character.AdminLVL)
                {
                    Chat.SendToAdmins(3, $"[OFFBAN-DENIED] {player.Name} ({ player.Character.UUID} попытался забанить {name} ({target.Character.UUID}), который имеет выше уровень администратора.");
                    return;
                }
            }

            int AUUID = player.Character.UUID;
            int TUUID = Main.PlayerUUIDs[name];

            Ban ban = Ban.Get2(TUUID);
            if (ban != null)
            {
                string hard = (ban.isHard) ? "hard " : "";
                Notify.Send(player, NotifyType.Warning, NotifyPosition.Center, $"Игрок уже заблокирован {hard}", 3000);
                return;
            }

            DateTime unbanTime = DateTime.Now.AddDays(time);
            string banTimeMsg = "День";

            Ban.Offline(name, unbanTime, false, reason, player.Name);

            GameLog.Ban(AUUID, TUUID, unbanTime, reason, false);

            Chat.AdminToAll($"{player.Name} забанил игрока {name} на {time.ToString()}{banTimeMsg} {reason}");
        }
        public static void offHardBanPlayer(ExtPlayer player, string name, int time, string reason)
        {
            if (!Group.CanUseAdminCommand(player, "offban")) return;
            if(player.Name.Equals(name)) return;
            ExtPlayer target = Trigger.GetPlayerByName(name) as ExtPlayer;
            if (target != null) {
                if(target.IsLogged()) {
                    if(target.Character.AdminLVL >= player.Character.AdminLVL)
                    {
                        Chat.SendToAdmins(3, $"[OFFHARDBAN-DENIED] {player.Name} ({player.Character.UUID}) попытался заблокировать {target.Name} ({target.Character.UUID}), Администратора высокого уровния");
                        return;
                    } else {
                        target.Kick();
                        Notify.Send(player, NotifyType.Success, NotifyPosition.Center, "Игрок находился в Online, но был кикнут", 3000);
                    }
                }
            } else {
                string[] split = name.Split('_');
                DataTable result = MySQL.QueryRead("SELECT adminlvl FROM characters WHERE firstname = @prop0 AND lastname = @prop1", split[0], split[1]);
                DataRow row = result.Rows[0];
                int targetadminlvl = Convert.ToInt32(row[0]);
                if(targetadminlvl >= player.Character.AdminLVL)
                {
                    Chat.SendToAdmins(3, $"[OFFHARDBAN-DENIED] {player.Name} ({player.Character.UUID}) попытался заблокировать {name} (offline), Администратора высокого уровния.");
                    return;
                }
            }

            int AUUID = player.Character.UUID;
            int TUUID = Main.PlayerUUIDs[name];

            Ban ban = Ban.Get2(TUUID);
            if (ban != null)
            {
                string hard = (ban.isHard) ? "Блок" : "";
                Notify.Send(player, NotifyType.Warning, NotifyPosition.Center, $"Игрок уже заблокирован {hard}", 3000);
                return;
            }

            DateTime unbanTime = DateTime.Now.AddDays(time);
            string banTimeMsg = "День";

            Ban.Offline(name, unbanTime, true, reason, player.Name);

            GameLog.Ban(AUUID, TUUID, unbanTime, reason, true);

            Chat.AdminToAll($"{player.Name} заблокировал игрока {name} на {time.ToString()}{banTimeMsg} причина: {reason}");
        }
        public static void unbanPlayer(ExtPlayer player, string name)
        {
            if (!Main.PlayerNames.ContainsValue(name))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Такого имени нет!", 3000);
                return;
            }
            if (!Ban.Pardon(name))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"{name} не находится в бане!", 3000);
                return;
            }
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "С игрока снят бан!", 3000);
            GameLog.Admin($"{player.Name}", $"unban", $"{name}");
        }
        public static void UnhardbanPlayer(ExtPlayer player, string name)
        {
            if (!Main.PlayerNames.ContainsValue(name))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Такого имени нет!", 3000);
                return;
            }
            if (!Ban.PardonHard(name))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"{name} не заблокирован!", 3000);
                return;
            }
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Игрок разблокирован!", 3000);
            GameLog.Admin($"{player.Name}", $"unhardban", $"{name}");
        }
        public static void kickPlayer(ExtPlayer player, ExtPlayer target, string reason, bool isSilence)
        {
            string cmd = (isSilence) ? "skick" : "kick";
            if (!Group.CanUseAdminCommand(player, cmd)) return;
            if(target.Character.AdminLVL >= player.Character.AdminLVL)
            {
                Chat.SendToAdmins(3, $"[KICK-DENIED] {player.Name} ({player.Character.UUID}) попытался кикнуть {target.Name} ({target.Character.UUID}), который имеет выше уровень администратора");
                return;
            }
            if (!isSilence)
                Chat.AdminToAll($"{player.Name} кикнул игрока {target.Name} причина: {reason}");
            else
            {
                foreach (ExtPlayer p in ReportSystem.ReportManager.Admins)
                {
                    if (!p.IsLogged()) continue;
                    if (p.Character.AdminLVL >= 1 && player.Character.AdminLVL < 9 || p.Character.AdminLVL >= 9 && player.Character.AdminLVL >= 9)
                    {
                        Chat.SendTo(p, $"{player.Name} Тихо кикнул игрока {target.Name}");
                    }
                }
            }
            GameLog.Admin($"{player.Name}", $"kickPlayer{reason}", $"{target.Name}");
            NAPI.Player.KickPlayer(target, reason);
        }
        public static void warnPlayer(ExtPlayer player, ExtPlayer target, string reason)
        {
            if (!Group.CanUseAdminCommand(player, "warn")) return;
            if(player == target) return;
            if(target.Character.AdminLVL >= player.Character.AdminLVL)
            {
                Chat.SendToAdmins(3, $"[WARN-DENIED] {player.Name} ({player.Character.UUID}) попытался предупредить {target.Name} ({target.Character.UUID}), который имеет выше уровень администратора.");
                return;
            }
            target.Character.Warns++;
            target.Character.Unwarn = DateTime.Now.AddDays(14);

            Manager.GetFraction(target)?.DeleteMember(target.Character.UUID);            

            Chat.AdminToAll($"{player.Name} выдал предупреждение игроку {target.Name} причина:{reason} [{ target.Character.Warns.ToString()}/3]");

            if (target.Character.Warns >= 3)
            {
                DateTime unbanTime = DateTime.Now.AddMinutes(43200);
                target.Character.Warns = 0;
                Ban.Online(target, unbanTime, false, "Warns 3/3", $"{player.Name}");
                target.Kick("warn+ban");
            }
            else
            {
                target.Character.ArrestDate = DateTime.Now;
                PoliceArrests.SetRecordAboutReleasePlayer(target, null, 0);
                target.Character.DemorganTime = 180 * 60;
                target.UnCuffed();
                target.UnFollow();
                target.SendTODemorgan();
                if (target.HasData("ARREST_TIMER")) Timers.Stop(target.GetData<string>("ARREST_TIMER"));
                SafeTrigger.SetData(target, "ARREST_TIMER", Timers.StartTask(1000, () => timer_demorgan(target)));
                SafeTrigger.UpdateDimension(target, 1337);
                Weapons.RemoveAll(target, true);
                RemoveMasks(target);
                GameLog.Admin($"{player.Name}", $"demorgan(180Минут,{reason})", $"{target.Name}");
                FamilyManager.ChangePoints(target, FamilyActions.GoToDemorgan);
            }

            GameLog.Admin($"{player.Name}", $"warnPlayer{reason}", $"{target.Name}");
            
        }
        public static void kickPlayerByName(ExtPlayer player, string name)
        {
            if (!Group.CanUseAdminCommand(player, "nkick")) return;
            ExtPlayer target = Trigger.GetPlayerByName(name) as ExtPlayer;
            if (target == null) return;
            NAPI.Player.KickPlayer(target);
            GameLog.Admin($"{player.Name}", $"kickPlayer", $"{name}");
        }

        public static void killTarget(ExtPlayer player, ExtPlayer target)
        {
            if (!Group.CanUseAdminCommand(player, "kill")) return;

            if (player.Character.AdminLVL < target.Character.AdminLVL)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У игрока выше админлвл, чем у Вас", 3000);
                return;
            }

            NAPI.Player.SetPlayerHealth(target, 0);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы убили игрока {target.Name}", 3000);
            GameLog.Admin($"{player.Name}", $"killPlayer", $"{target.Name}");
        }
        public static void healTarget(ExtPlayer player, ExtPlayer target, int hp)
        {
            if (!Group.CanUseAdminCommand(player, "hp")) return;

            if (player.Character.AdminLVL < target.Character.AdminLVL)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У игрока выше админлвл, чем у Вас", 3000);
                return;
            }
            NAPI.Player.SetPlayerHealth(target, hp);
            GameLog.Admin($"{player.Name}", $"healPlayer({hp})", $"{target.Name}");
        }
        [Command("offcheckmoney")]
        public static void OfflineCheckMoney(ExtPlayer player, string name)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "offcheckmoney")) return;

                if (!Main.PlayerNames.ContainsValue(name))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок не найден", 3000);
                    return;
                }

                ExtPlayer client = Trigger.GetPlayerByName(name);
                if (client != null && client.IsLogged())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок онлайн", 3000);
                    return;
                }

                var uuid = Main.PlayerUUIDs[name];
                var result = MySQL.QueryRead("SELECT money, chips FROM characters WHERE uuid=@prop0", uuid);
                var mcoins = MySQL.QueryRead("SELECT mcoins FROM accounts WHERE character1=@prop0 OR character2=@prop0 OR character3=@prop0", uuid).Rows[0];
                
                Chat.SendTo(player,"--------------------------");

                #region checking money
                int coins = Convert.ToInt32(mcoins["mcoins"]);
                var cashMoney = Convert.ToInt32(result.Rows[0]["money"]);
                var bankAcc = BankManager.GetAccountByUUID(uuid);
                long bankMoney = bankAcc?.Balance ?? 0;
                Chat.SendTo(player,$"{name} {cashMoney}$ | Bank: {bankMoney}$ | Donate Coins: {coins}");
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
                    Chat.SendTo(player,"Player has no house");
                }
                else
                {
                    Chat.SendTo(player,$"Player has {HouseManager.HouseTypeList[house.Type].Name} house | ID {house.ID}");
                    
                }
                
                var vehicles = VehicleManager.getAllHolderVehicles(uuid, OwnerType.Personal);
                if (vehicles.Count() > 0)
                {
                    Chat.SendTo(player, $"Player vehicles:");
                    foreach (var veh in vehicles)
                        Chat.SendTo(player, $"{VehicleManager.Vehicles[veh].Number} - {VehicleManager.Vehicles[veh].ModelName}");
                }

                var biz = BusinessManager.GetBusinessByOwner(uuid);
                if (biz == null)
                {
                    Chat.SendTo(player,$"Player has no business");
                }
                else
                {
                    Chat.SendTo(player,$"Player has {biz.TypeModel.TypeName} (ID {biz.ID})");
                }
                #endregion
                Chat.SendTo(player,"--------------------------");
            }
            catch (Exception e) { _logger.WriteError($"OfflineCheckMoney:\n{e}"); }
        }
        public static void checkMoney(ExtPlayer player, ExtPlayer target)
        {

            try
            {
                if (!Group.CanUseAdminCommand(player, "checkmoney")) return;

                var bankAcc = target.Character.BankModel;
                long bankMoney = bankAcc?.Balance ?? 0;
                Chat.SendTo(player,"--------------------------");
                Chat.SendTo(player, $"Money of {target.Name}");
                Chat.SendTo(player, $"{target.Name} has cash: {target.Character.Money}$");
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
        
        public static void teleportToPlayer(ExtPlayer admin, ExtPlayer target)
        {
            if (!Group.CanUseAdminCommand(admin, "tp")) return;
            if (target.Character.AdminLVL >= 7 && admin.Character.AdminLVL < target.Character.AdminLVL) return;
            admin.ChangePosition(target.Position + new Vector3(1, 0, 1.5));
            SafeTrigger.UpdateDimension(admin, target.Dimension);

            AdminParticles.PlayAdminAppearanceEffect(admin);
        }
        
        public static void teleportTargetToPlayer(ExtPlayer player, ExtPlayer target, bool withveh = false)
        {
            if (!Group.CanUseAdminCommand(player, "metp")) return;
            if (target.Character.AdminLVL >= 7 && player.Character.AdminLVL < target.Character.AdminLVL) return;
            if (!withveh) {
                GameLog.Admin($"{player.Name}", $"metp", $"{target.Name}");
                target.ChangePosition(player.Position);
                SafeTrigger.UpdateDimension(target, player.Dimension);
            } else {
                if (!target.IsInVehicle) return;
                target.ChangePosition(null);
                target.Vehicle.Position = player.Position + new Vector3(2,2,2);
                target.Vehicle.Dimension = player.Dimension;
                GameLog.Admin($"{player.Name}", $"gethere", $"{target.Name}");
            }
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы телепортировали {target.Name} к себе", 3000);
            Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"{player.Name} телепортировал Вас к себе", 3000);
        }
        public static void freezeTarget(ExtPlayer player, ExtPlayer target)
        {
            if (!Group.CanUseAdminCommand(player, "frz")) return;

            if (player.Character.AdminLVL < target.Character.AdminLVL)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У игрока выше админлвл, чем у Вас", 3000);
                return;
            }

            SafeTrigger.ClientEvent(target, "freeze", true);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы заморозили игрока {target.Name}", 3000);
            GameLog.Admin($"{player.Name}", $"freeze", $"{target.Name}");
        }
        public static void unFreezeTarget(ExtPlayer player, ExtPlayer target)
        {
            if (!Group.CanUseAdminCommand(player, "unfrz")) return;
            SafeTrigger.ClientEvent(target, "freeze", false);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы разморозили игрока {target.Name}", 3000);
            GameLog.Admin($"{player.Name}", $"unfreeze", $"{target.Name}");
        }
        
        public static void giveTargetGun(ExtPlayer player, ExtPlayer target, string weapon, bool promo = true)
        {
            if (promo && !Group.CanUseAdminCommand(player, "givegun")) return;
            if (!promo && !Group.CanUseAdminCommand(player, "givegunc")) return;

            if (!Enum.TryParse(weapon, out ItemNames itemName))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Оружия с таким названием не существует", 3000);
                return;
            }

            var item = ItemsFabric.CreateWeapon(itemName, promo);
            if (item == null) 
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Выдача предметов одежды запрещена", 3000);
                return;
            }
            if (!target.GetInventory().AddItem(item))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У игрока недостаточно места в инвентаре", 3000);
                return;
            }
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы выдали игроку {target.Name} оружие {weapon.ToString()}", 3000);
            if (promo)
                GameLog.Admin($"{player.Name}", $"giveGun({weapon},{(item as Whistler.Inventory.Models.Weapon).Serial})", $"{target.Name}");
            else
                GameLog.Admin($"{player.Name}", $"giveGunc({weapon},{(item as Whistler.Inventory.Models.Weapon).Serial})", $"{target.Name}");
        }
        public static void giveTargetGunWithComponents(ExtPlayer player, ExtPlayer target, string weapon, int muzzle, int flash, int clip, int scope, int grip, int skin)
        {
            if (!Group.CanUseAdminCommand(player, "giveguncomponents")) return;
            if (!Enum.TryParse(weapon, out ItemNames itemName))
                return;
            var item = ItemsFabric.CreateWeapon(itemName, muzzle, flash, clip, scope, grip, skin, true);
            if (item == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Выдача предметов одежды запрещена", 3000);
                return;
            }
            if (!target.GetInventory().AddItem(item))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У игрока недостаточно места в инвентаре", 3000);
                return;
            }
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы выдали игроку {target.Name} оружие {weapon.ToString()}", 3000);           
            GameLog.Admin($"{player.Name}", $"giveGunc({weapon},{(item as Whistler.Inventory.Models.Weapon).Serial})", $"{target.Name}");
        }

        public static void giveTargetSkin(ExtPlayer player, ExtPlayer target, string pedModel)
        {
            if (!Group.CanUseAdminCommand(player, "setskin")) return;
            if(pedModel.Equals("-1")) {
                if(target.HasData("AdminSkin")) {
                    target.ResetData("AdminSkin");
                    target.Character.Customization.Apply(player);
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Вы восстановили игроку внешность", 3000);
                } else {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игроку не меняли внешность", 3000);
                    return;
                }
            } else {
                PedHash pedHash = NAPI.Util.PedNameToModel(pedModel);
                if (pedHash != 0) {
                    SafeTrigger.SetData(target, "AdminSkin", true);
                    target.SetSkin(pedHash);
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы сменили игроку {target.Name} внешность на {pedModel}", 3000);
                } else {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Внешности с таким названием не было найдено", 3000);
                    return;
                }
            }
            GameLog.Admin(player.Name, $"setSkin({pedModel})", target.Name);
        }
        public static void giveTargetClothes(ExtPlayer player, ExtPlayer target, int type, int drawable, int texture, bool promo)
        {
            if (promo && !Group.CanUseAdminCommand(player, "giveclothes")) return;
            if (!promo && !Group.CanUseAdminCommand(player, "giveclothesc")) return;
            if (!Enum.IsDefined(typeof(ItemNames), type))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Этой командой можно выдавать только предметы одежды", 3000);
                return;
            }
            ItemNames itemName = (ItemNames)type;
            var item = ItemsFabric.CreateClothes(itemName, player.GetGender(), drawable, texture, promo);
            if (item == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Этой командой можно выдавать только предметы одежды", 3000);
                return;
            }
            if (!target.GetInventory().AddItem(item))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"У игрока недостаточно места в инвентаре", 3000);
                return;
            }
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы выдали игроку {target.Name} одежду {itemName.ToString()}", 3000);
            if (promo)
                GameLog.Admin(player.Name, $"giveClothes({type},{drawable},{texture})", target.Name);
            else
                GameLog.Admin(player.Name, $"giveClothesc({type},{drawable},{texture})", target.Name);
        }
        public static void takeTargetGun(ExtPlayer player, ExtPlayer target)
        {
            if (!Group.CanUseAdminCommand(player, "oguns")) return;

            if (player.Character.AdminLVL < target.Character.AdminLVL)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У игрока выше админлвл, чем у Вас", 3000);
                return;
            }

            Weapons.RemoveAll(target, true);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы забрали у игрока {target.Name} всё оружие", 3000);
            GameLog.Admin($"{player.Name}", $"takeGuns", $"{target.Name}");
        }

        public static void adminSMS(ExtPlayer player, ExtPlayer target, string message)
        {
            if (!Group.CanUseAdminCommand(player, "pm")) return;
            //Chat.AdminSMS(target,$"[Admin]{player.Name} ({player.Character.UUID}): {message}");
            Chat.AdmiAnswer(target, target, player, message);
            Chat.AdminSMS(player,$"[Success] for {target.Name} ({target.Character.UUID}): {message}");
            GameLog.Chat(player.Character.UUID, (int)ChatType.AdminWarning, message);
        }
       
      
        public static void sendPlayerToDemorgan(ExtPlayer admin, ExtPlayer target, int time, string reason)
        {
            if (!Group.CanUseAdminCommand(admin, "demorgan")) return;
            if (!target.IsLogged()) return;

            if (admin.Character.AdminLVL < target.Character.AdminLVL)
            {
                Notify.Send(admin, NotifyType.Error, NotifyPosition.BottomCenter, "У игрока выше админлвл, чем у Вас", 3000);
                return;
            }
            int firstTime = time * 60;
            if (target.Character.IsPrimeActive())
            {
                firstTime = (int)(firstTime * DonateService.PrimeAccount.DemorganAndArrestMultipler);
            }

            string deTimeMsg = "Минут";
            if (time > 60)
            {
                deTimeMsg = "Час";
                time /= 60;
                if (time > 24)
                {
                    deTimeMsg = "День";
                    time /= 24;
                }
            }

            Chat.AdminToAll($"{admin.Name.Replace('_', ' ')} посадил игрока {target.Name} в спец. тюрьму на {time.ToString()} {deTimeMsg} за: {reason}");
            target.Character.ArrestDate = DateTime.Now;
            PoliceArrests.SetRecordAboutReleasePlayer(target, null, 0);
            target.Character.DemorganTime = firstTime;
            target.UnCuffed();
            // target.LetGoFollower();
            target.UnFollow();

            target.SendTODemorgan();
            if (target.HasData("ARREST_TIMER")) Timers.Stop(target.GetData<string>("ARREST_TIMER"));
            SafeTrigger.SetData(target, "ARREST_TIMER", Timers.StartTask(1000, () => timer_demorgan(target)));
            SafeTrigger.UpdateDimension(target, 1337);
            Weapons.RemoveAll(target, true);
            RemoveMasks(target);
            GameLog.Admin($"{admin.Name}", $"demorgan({time}{deTimeMsg},{reason})", $"{target.Name}");
            FamilyManager.ChangePoints(target, FamilyActions.GoToDemorgan);
        }
        public static void ReleasePlayerFromDemorgan(ExtPlayer admin, ExtPlayer target)
        {
            target.Character.DemorganTime = 0;
            SafeTrigger.ClientEvent(target, "admin:releaseDemorgan");
            Notify.Send(admin, NotifyType.Warning, NotifyPosition.BottomCenter, $"Вы освободили {target.Name} из деморгана", 3000);
            GameLog.Admin($"{admin.Name}", $"undemorgan", $"{target.Name}");
        }

        public static void ReleasePlayerFromJail(ExtPlayer admin, ExtPlayer target)
        {
            DateTime now = DateTime.Now;
            if (target.Character.ArrestDate > now)
            {
                target.Character.ArrestDate = now;
                target.Character.ResetArrestTimer(target);
                target.ChangePosition(FractionCommands.KpzOutPosition);
                Notify.Send(admin, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы успешно вытащили {target.Name} из КПЗ.", 3000);
                Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Администратор {admin.Name} вытащил Вас из КПЗ.", 3000);
                GameLog.Admin($"{admin.Name}", $"unjail", $"{target.Name}");
                return;
            }
            if (target.Character.CourtTime > 0)
            {
                target.Character.ArrestID = 0;
                target.Character.CourtTime = 0;
                target.Character.ResetArrestTimer(target);
                SafeTrigger.ClientEvent(target, "Client_CheckIsInJailDestroy");
                target.ChangePosition(PrisonFib.ExitPoint);
                Notify.Send(admin, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы успешно вытащили {target.Name} из тюрьмы.", 3000);
                Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Администратор {admin.Name} вытащил Вас из тюрьмы.", 3000);
                GameLog.Admin($"{admin.Name}", $"unjail", $"{target.Name}");
                return;
            }
            Notify.Send(admin, NotifyType.Info, NotifyPosition.BottomCenter, $"Игрок не находится ни в КПЗ, ни в тюрьме.", 3000);
        }

        #region Demorgan
        //public static Vector3 DemorganPosition = new Vector3(1651.217, 2570.393, 44.44485); //OLD
        public static Vector3 DemorganPosition = new Vector3(5012, -4967, 36); //dont royal battle
        //public static Vector3 DemorganPosition = new Vector3(5070, -4883, 18); //for royal battle
        public static int DemorganRange = 1400;
        public static int DemorganHeight = 500;
        public static void timer_demorgan(ExtPlayer player)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (player.Character.DemorganTime <= 0)
                {
                    player.Character.DemorganTime = 0;
                    SafeTrigger.ClientEvent(player,"admin:releaseDemorgan");
                    FractionCommands.freePlayer(player);
                    return;
                }
                player.Character.DemorganTime--;
            }
            catch (Exception e)
            {
                _logger.WriteError($"timer_demorgan:\n{e}");
            }
        }
        #endregion
        // need refactor
        public static void respawnAllCars(ExtPlayer player)
        {
            if (!Group.CanUseAdminCommand(player, "spawnallcar")) return;
            var all_vehicles = VehicleManager.Vehicles.Where(item => item.Value.OwnerType == OwnerType.Fraction || item.Value.OwnerType == OwnerType.Job);

            ExtVehicle extVehicle;
            foreach (var vehicle in all_vehicles)
            {
                extVehicle = SafeTrigger.GetVehicleByDataId(vehicle.Value.ID);
                if (extVehicle == null)
                {
                    vehicle.Value.RespawnVehicle();
                    continue;
                }

                if (extVehicle.AllOccupants.Count >= 1) continue;

                vehicle.Value.RespawnVehicle();
            }
            GameLog.Admin(player.Name, $"spawnallcar", $"");
        }

        public static void RemoveMasks(ExtPlayer player)
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
            DataTable result = MySQL.QueryRead("SELECT * FROM adminaccess");
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
            "Игрок",
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

      
        public static bool CanUseAdminCommand(ExtPlayer player, string cmd, bool notify = true)
        {
            if (!player.IsLogged())
                return false;
            GroupCommand command = GroupCommands.FirstOrDefault(c => c.Command == cmd);
            if (command == null)
            {
                MySQL.Query("INSERT INTO `adminaccess`(`command`, `isadmin`, `minrank`) VALUES (@prop0, 1, 10)", cmd);
                command = new GroupCommand(cmd, true, 10, false);
                GroupCommands.Add(command);
            }
            if (command.IsAdmin)
            {
                var adminLvl = player.Character.AdminLVL;
                if (command.MinLVL <= adminLvl || (command.IsTechnical && adminLvl == -1))
                    return true;
            }
            if (notify)
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно прав", 3000);
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
