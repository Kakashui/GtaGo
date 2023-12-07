using GTANetworkAPI;
using System;
using System.IO;
using System.Collections.Generic;
using Whistler.GUI;
using System.Text;
using System.Linq;
using System.Data;
using System.Globalization;
using Newtonsoft.Json;
using Whistler.SDK;
using Whistler.MoneySystem;
using Whistler.Houses;
using Whistler.Fractions;
using ServerGo.Casino.Business;
using ServerGo.Casino.ChipModels;
using Whistler.ClothesCustom;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;
using Whistler.Helpers;
using Whistler.Core.Admins;
using Whistler.VehicleSystem.Models.VehiclesData;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Core.CustomSync;
using Whistler.Core.Weather;
using Whistler.Customization;
using Whistler.Common;
using Whistler.Entities;

namespace Whistler.Core
{
    class Commands: Script
    {
        private static Random rnd = new Random();
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Commands));
       
        [Command("getvariable")]
        public static void CMD_updateBiz(PlayerGo player, int id, string variable)
        {
            if (!Group.CanUseAdminCommand(player, "getvariable")) return;
            try
            {
                var vehicle = NAPI.Pools.GetAllVehicles().Where(a => a.Value == id).FirstOrDefault();
                if (vehicle == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_117", 3000);
                    return;
                };
                player.TriggerEvent("viewVariableData", vehicle, variable);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_updateBiz: {e}");
            }
        }

        [Command("updatepassword")]
        public static void CMD_updatePassword(PlayerGo player, string loginOrEmail, string newPassword)
        {
            if (!Group.CanUseAdminCommand(player, "updatepassword")) return;
            var playerGo = Main.GetPlayerGoByPredicate(item => item.Account.Email == loginOrEmail || item.Account.Login == loginOrEmail);
            if (playerGo == null) 
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_111", 3000);
            else
            {
                playerGo.Account.changePassword(newPassword);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "mmain:pwd:success", 3000);
            }
        }

        [Command("rapeon")]
        public static void CMD_RapeTargetOn(PlayerGo player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "rape")) return;
            try
            {
                PlayerGo target = Main.GetPlayerByID(id);
                
                if(target.IsLogged() && player.IsLogged())
                {
                    if(target.Position.DistanceTo(player.Position) > 4)
                    {
                        Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "cmd:target:range", 3000);
                        return;
                    }
                    var pos = player.Position + new Vector3(0, 0, -0.7);
                    target.TriggerEvent("rape:target", pos, player.Value);
                    player.TriggerEvent("rape:king", pos, target.Value);
                    player.SetData("rape:target", target);
                    player.SetSkin(0x5442C66B);
                } 
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_RapeTarget: {e}");
            }
        }

        [Command("rapeoff")]
        public static void CMD_RapeTargetOff(PlayerGo player)
        {
            if (!Group.CanUseAdminCommand(player, "rape")) return;
            try
            {
                if (player.HasData("rape:target"))
                {
                    PlayerGo target = player.GetData<PlayerGo>("rape:target");
                    if (target.IsLogged())
                    {
                        target.TriggerEvent("rape:off");
                    }
                }
                if (player.IsLogged())
                {
                    player.TriggerEvent("rape:off");
                    player.GetCharacter().Customization.Apply(player);
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_RapeTarget: {e}");
            }
        }

        [Command("updatebiz")]
        public static void CMD_updateBiz(PlayerGo player, int type)
        {
            try
            {
                BusinessManager.UpdateBusinessCommand(type, false);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_updateBiz: {e}");
            }
        }

        [Command("updatebizprice")]
        public static void CMD_updateBizPrice(PlayerGo player, int type)
        {
            try
            {
                BusinessManager.UpdateBusinessCommand(type, true);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_updateBizPrice: {e}");
            }
        }

        #region AdminCommands

        [Command("greenscreen")]
        public static void CMD_Test(PlayerGo player, bool flag = true)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "greenscreen")) return;
                Trigger.ClientEvent(player, "greenscreen:openedMenu", flag);                
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_Test: {e}");
            }
        }

        [Command("checkinv")]
        public static void CMD_CheckInv(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "checkinv")) return;
                var target = Main.GetPlayerByID(id);
                if(target == null){
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_199", 3000);
                    return;
                }
                Admin.CheckInventory(player, target);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_updateBiz: {e}");
            }
        }

        [Command("createadmincar")]
        public static void CMD_CreateAdminCar(PlayerGo player, string model)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "createadmincar")) return;
                var vehData = new AdminSaveVehicle(model, player.Position, player.Rotation, 0, 0);
                vehData.Spawn();
                GameLog.Admin(player.Name, $"createadmincar({model})", "");
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_CreateAdminCar: {e}");
            }
        }

        [Command("saveadmincar")]
        public static void CMD_SaveAdminCar(PlayerGo player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "saveadmincar")) return;
                if (player.Vehicle == null)
                    return;
                var vehData = player.Vehicle.GetVehicleGo();
                if (vehData.Data.OwnerType != OwnerType.AdminSave)
                    return;
                vehData.Data.Position = player.Vehicle.Position;
                vehData.Data.Rotation = player.Vehicle.Rotation;
                vehData.Data.Save();
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "", 3000);
                GameLog.Admin(player.Name, $"saveadmincar({player.Vehicle.GetVehicleGo().Data.ID})", "");
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_SaveAdminCar: {e}");
            }
        }

        [Command("deleteadmincar")]
        public static void CMD_DeleteAdminCar(PlayerGo player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "deleteadmincar")) return;
                if (player.Vehicle == null)
                    return;
                var vehData = player.Vehicle.GetVehicleGo();
                if (vehData.Data.OwnerType != OwnerType.AdminSave)
                    return;
                int id = player.Vehicle.GetVehicleGo().Data.ID;
                vehData.Data.DeleteVehicle(player.Vehicle);
                GameLog.Admin(player.Name, $"deleteadmincar({id})", "");
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_DeleteAdminCar: {e}");
            }
        }

        [Command("vct")]
        public static void CMD_ClientTrafficOn(PlayerGo player, int id, int status = 1)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "vct")) return;
                PlayerGo target = Main.GetPlayerByID(id);
                if (target == null)
                    return;
                target.TriggerEvent("ClientTrafficChangeStatus", status);
                if (status == 1)
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "local_170".Translate( target.Name), 3000);
                else
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "local_171".Translate( target.Name), 3000);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_ClientTrafficOn: {e}");
            }
        }

        [Command("delfracmd")]
        public static void CMD_ChangeFractionCommandAccess(PlayerGo player, int fraction, string command)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "changefracmdaccess")) return;

                if (Manager.GetFraction(fraction)?.RemoveCommand(command) ?? false)
                {
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "local_115".Translate(command), 3000);
                    GameLog.Admin(player.Name, $"delfracmd({fraction},{command})", "");
                }
                else
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_113", 3000);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_ChangeFractionCommandAccess: {e}");
            }
        }

        [Command("addfracmd")]
        public static void CMD_AddFractionCommandAccess(PlayerGo player, int fraction, string command, int minRank)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "addfracmd")) return;


                if (Manager.GetFraction(fraction) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "cmd:frac:no", 3000);
                    return;
                }

                if (Manager.GetFraction(fraction).Commands.ContainsKey(command))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "caddfracmd:2", 3000);
                    return;
                }

                if (Manager.GetFraction(fraction)?.AddCommand(command, minRank) ?? false)
                {
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "caddfracmd:4".Translate(command, fraction, minRank), 3000);
                    GameLog.Admin(player.Name, $"addfracmd({fraction},{command},rank:{minRank})", "");
                }
                else
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "caddfracmd:3", 3000);

            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_AddFractionCommandAccess: {e}");
            }
        }

        [Command("givemark")]
        public static void CMD_givemark(PlayerGo player, int id){
            try{
                if (!Group.CanUseAdminCommand(player, "tpmark")) return;
                PlayerGo target = Main.GetPlayerByID(id);
                if (target == null){
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_199", 3000);
                    return;
                }
                Trigger.ClientEvent(player, "GetWPAdmin");
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_GiveStreamerCar: {e}");
            }
        }

        [Command("givecar")]
        public static void CMD_GiveStreamerCar(PlayerGo player, int targetId, string model, int c1, int c2)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "givecar")) return;

                PlayerGo target = Main.GetPlayerByID(targetId);
                if (target == null || !target.IsLogged())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_112", 3000);
                    return;
                }
                var house = HouseManager.GetHouse(target, true);
                if (house == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_203", 3000);
                    return;
                }
                if (house.GarageID == 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_204", 3000);
                    return;
                }
                var vModel = VehicleManager.GetModelName(model);

                if (string.IsNullOrEmpty(vModel))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_205", 3000);
                    return;
                }
                
                var vehData = VehicleManager.Create(target.GetCharacter().UUID, vModel, new Color(c1), new Color(c2), status: PropBuyStatus.Given);
                MainMenu.SendProperty(target);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Core_206".Translate( vModel, target.Name), 3000);

                GarageManager.SendVehicleIntoGarage(vehData);
                GameLog.Admin(player.Name, $"givecar({model})", target.Name);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_GiveStreamerCar: {e}");
            }
        }

        [Command("givefamcar")]
        public static void CMD_GiveStreamerFamilyCar(PlayerGo player, int targetId, string model, int c1, int c2)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "givecar")) return;

                PlayerGo target = Main.GetPlayerByID(targetId);
                if (target == null || !target.IsLogged())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_112", 3000);
                    return;
                }
                var family = target.GetFamily();
                if (family == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Fam_27", 3000);
                    return;
                }
                var vModel = VehicleManager.GetModelName(model);

                if (string.IsNullOrEmpty(vModel))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_205", 3000);
                    return;
                }

                var vehData = VehicleManager.Create(family.Id, vModel, new Color(c1), new Color(c2), status: PropBuyStatus.Given, typeOwner: OwnerType.Family);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Fam_28".Translate( vModel, family.Name), 3000);

                GarageManager.SendVehicleIntoGarage(vehData);
                GameLog.Admin(player.Name, $"givefamcar({model})", family.Name);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_GiveStreamerFamilyCar: {e}");
            }
        }

        [Command("delgivencar")]
        public static void CMD_DelGivenCar(PlayerGo player, int id)
        {
            try
            {
                if (!player.IsLogged() || !Group.CanUseAdminCommand(player, "givecar")) return;

                Vehicle vehicle = NAPI.Pools.GetAllVehicles().FirstOrDefault(item => item.Value == id);
                if (vehicle == null)
                    return;
                VehicleGo vehGo = vehicle.GetVehicleGo();
                if (((vehGo.Data as PersonalBaseVehicle)?.PropertyBuyStatus ?? PropBuyStatus.Unknown) == PropBuyStatus.Given)
                {
                    GameLog.Admin(player.Name, $"delgivencar({vehGo.Data.ModelName},{vehGo.Data.ID})", vehGo.Data.GetHolderName());
                    VehicleManager.Remove(vehGo.Data.ID);
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_DelGivenCar: {e}");
            }
        }        

        [Command("checkchips")]
        public static void CMD_CheckChips(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "checkchips")) return;
                
                var target = Main.GetPlayerByID(id);
                if (target == null) return;
                var chips = target.GetCharacter().CasinoChips;
                if (chips != null || target.GetCharacter().CasinoChips.Length > 0)
                {
                    var chipList = new List<Chip>();
                    for (var i = 0; i < 5; i++)
                    for (var j = 0; j < target.GetCharacter().CasinoChips[i]; j++)
                    {
                        chipList.Add(ChipFactory.Create((ChipType)i));
                    }
                    var total = chipList.Sum(c => c.Value);
                    Chat.SendTo(player,$"Chips: [{chips[0]}, {chips[1]}, {chips[2]}, {chips[3]}, {chips[4]}] Balance: {total}");
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_CheckChips: {e}");
            }
        }

        [Command("playervehnumber")]
        public static void CMD_ChangeNumberPlate(PlayerGo player, int id, string newNumber)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "vehnumber")) return;

                Vehicle vehicle = NAPI.Pools.GetAllVehicles().FirstOrDefault(item => item.Value == id);

                if (vehicle != null)
                if (!VehicleManager.ChangeNumber(vehicle.GetVehicleGo().Data.ID, newNumber, true)) 
                    Chat.SendTo(player,$"car wasnt found or bad number");
                else
                    {
                        Chat.SendTo(player, $"number changed for car #{id} to {newNumber}");
                        GameLog.Admin(player.Name, $"playervehnumber({vehicle.GetVehicleGo().Data.ID},{newNumber})", "");
                    }
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_ChangeNumberPlate: {e}");
            }
        }

        [Command("vehnumber")]
        public static void CMD_ChangeNumberPlate(PlayerGo player, string newNumber)
        {
            try
            {
                newNumber = newNumber.ToUpper();
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "vehnumber")) return;
                if (!player.IsInVehicle)
                {
                    Chat.SendTo(player,"use (playervehnumber [oldNumber] [newNumber]) or sit in car");
                    return;
                }
                var oldNumber = player.Vehicle.NumberPlate;
                player.Vehicle.NumberPlate = newNumber;
                Chat.SendTo(player,$"number changed from {oldNumber} to {newNumber}");
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_ChangeNumberPlate: {e}");
            }
        }

        [Command("gethwid")]
        public static void CMD_gethwid(PlayerGo client, int ID)
        {
            try
            {
                if (!Group.CanUseAdminCommand(client, "setvehdirt")) return;
                PlayerGo target = Main.GetPlayerByID(ID);
                if (target == null)
                {
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Core_212".Translate(), 3000);
                    return;
                }
                string hwid = target.GetData<string>("RealHWID");
                Chat.SendTo(client, "gethwid:1".Translate(target.Name, hwid));
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_gethwid: {e}");
            }
        }
        
        [Command("getsocialclub")]
        public static void CMD_getsc(PlayerGo client, int ID)
        {
            try
            {
                if (!Group.CanUseAdminCommand(client, "setvehdirt")) return;
                PlayerGo target = Main.GetPlayerByID(ID);
                if (target == null)
                {
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Core_212".Translate(), 3000);
                    return;
                }
                Chat.SendTo(client, "getsclub".Translate(target.Name, client.SocialClubId));
            }
            catch(Exception e)
            {
                _logger.WriteError($"CMD_getsc: {e}");
            }
        }

        [Command("giveammo")]
        public static void CMD_ammo(PlayerGo client, int ID, int type, int amount = 1)
        {
            try
            {
                if (!Group.CanUseAdminCommand(client, "giveammo")) return;

                var target = Main.GetPlayerByID(ID);
                if (target == null)
                {
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212".Translate(), 3000);
                    return;
                }

                var item = ItemsFabric.CreateAmmo((ItemNames)(118 + type), amount, true);
                if (item == null)
                    return;
                if (!target.GetInventory().AddItem(item))
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Core_217".Translate(), 3000);
                GameLog.Admin(client.Name, $"giveammo(t:{type},cnt:{amount})", target.Name);
            }
            catch(Exception e)
            {
                _logger.WriteError($"CMD_ammo: {e}");
            }
        }

        [Command("giveammoc")]
        public static void CMD_ammoc(PlayerGo client, int ID, int type, int amount = 1)
        {
            try
            {
                if (!Group.CanUseAdminCommand(client, "giveammoc")) return;

                var target = Main.GetPlayerByID(ID);
                if (target == null)
                {
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212".Translate(), 3000);
                    return;
                }

                var item = ItemsFabric.CreateAmmo((ItemNames)(118 + type), amount, false);
                if (item == null)
                    return;
                if (!target.GetInventory().AddItem(item))
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Core_217".Translate(), 3000);
                GameLog.Admin(client.Name, $"giveammoc(t:{type},cnt:{amount})", target.Name);
            }
            catch(Exception e)
            {
                _logger.WriteError($"CMD_ammoc: {e}");
            }
        }
		
        [Command("adm")]
        public static void ACMD_redname(PlayerGo player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "adm")) return;

                if (!player.HasSharedData("ADM_NAME") || !player.GetSharedData<bool>("ADM_NAME"))
                {
                    Chat.SendTo(player, "ADM_NAME ON");
                    player.SetSharedData("ADM_NAME", true);
                }
                else
                {
                    Chat.SendTo(player, "ADM_NAME OFF");
                    player.SetSharedData("ADM_NAME", false);
                }

            }
            catch (Exception e)
            {
                _logger.WriteError($"ACMD_redname: {e}");
            }
        }
		
        [Command("hidenick")]
        public static void CMD_hidenick(PlayerGo player) {
            try
            {
                if (!Group.CanUseAdminCommand(player, "setvehdirt")) return;
                if (!player.HasSharedData("HideNick") || !player.GetSharedData<bool>("HideNick"))
                {
                    Chat.SendTo(player, "HideNick ON");
                    player.SetSharedData("HideNick", true);
                }
                else
                {
                    Chat.SendTo(player, "HideNick OFF");
                    player.SetSharedData("HideNick", false);
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_hidenick: {e}");
            }

        }

        [Command("givedonate")]
        public static void CMD_GiveDonatePoints(PlayerGo player, int id, int amount)
        {
            try
            {
                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.GiveDonatePoints(player, target, amount);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_GiveDonatePoints: {e}");
            }
        }

        [Command("checkprop")]
        public static void CMD_checkProperety(PlayerGo player, int id)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "checkprop")) return;

                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }

                if (!target.IsLogged()) return;
                var house = Houses.HouseManager.GetHouse(target);
                if (house != null)
                {
                    if (house.OwnerID == target.GetCharacter().UUID)
                    {
                        Chat.SendTo(player,$"Core_220".Translate( house.Price, Houses.HouseManager.HouseTypeList[house.Type].Name));
                    }
                    else
                        Chat.SendTo(player,$"Core_222".Translate( house.GetHouseOwnerName(), house.Price));
                }
                else
                    Chat.SendTo(player,"Core_223");
                var targetVehicles = VehicleManager.getAllHolderVehicles(target.GetCharacter().UUID, OwnerType.Personal);
                if (targetVehicles.Count() > 0)
                {
                    foreach (var veh in targetVehicles)
                        Chat.SendTo(player, $"Core_221".Translate(VehicleManager.Vehicles[veh].ModelName, VehicleManager.Vehicles[veh].Number));
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_checkProperety: {e}");
            }
        }
        
        [Command("moneypickup")]
        public static void CMD_createPickup(PlayerGo player, int money)
        {
            try
            {
                if (player.GetCharacter().AdminLVL < 7) return;
                var pos = player.Position - new Vector3(0, 0, 1);
                BonusPickup.Create(pos, money);
                GameLog.Admin(player.Name, "moneypickup", "");
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_createPickup: {e}");
            }
        }
        
        [Command("checkpickups")]
        public static void CMD_checkPickup(PlayerGo player)
        {
            try
            {
                if (player.GetCharacter().AdminLVL < 7) return;
                NAPI.Chat.SendChatMessageToPlayer(player, "checkpickups".Translate(BonusPickup.Pickups.Count,BonusPickup.Counter));
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_checkPickup: {e}");
            }
        }

        [Command("pa")]
        public void PlayeAnim(PlayerGo player, string dict, string name, int flag)
        {
            if (!Group.CanUseAdminCommand(player, "id")) return;
            AnimSync.PlayAnimGo(player, dict, name, (AnimFlag)flag);
        }

        [Command("sa")]
        public void StopAnim(PlayerGo player)
        {
            if (!Group.CanUseAdminCommand(player, "id")) return;
            AnimSync.StopAnimGo(player);
        }

      
        [Command("id", "/id [name/id]")]
        public static void CMD_checkId(PlayerGo player, string target)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "id")) return;

                int id;
                if (Int32.TryParse(target, out id))
                {
                    var sendPlayer = Main.GetPlayerByID(id);
                    if (sendPlayer.IsLogged() && (sendPlayer.GetCharacter().AdminLVL < 7 || sendPlayer.GetCharacter().AdminLVL <= player.GetCharacter().AdminLVL)) 
                        Chat.SendTo(player, $"ID: {sendPlayer.Value} | STATIC ID: {sendPlayer.GetCharacter().UUID} | {sendPlayer.Name}");
                    else
                        Chat.SendTo(player,"Core_212");
                }
                else
                {
                    var players = 0;
                    Main.ForEachAllPlayer((p) =>
                    {
                        if (p.Character.AdminLVL >= 7 && player.GetCharacter().AdminLVL < p.Character.AdminLVL) return;
                        if (p.Name.ToUpper().Contains(target.ToUpper()))
                        {
                            Chat.SendTo(player, $"ID: {p.Value} | STATIC ID: {p.GetCharacter().UUID} | {p.Name}");
                            players++;
                        }
                    });
                    if (players == 0)
                        Chat.SendTo(player,"Core_224");
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_checkId: {e}");
            }
        }

        [Command("setdim")]
        public static void CMD_setDim(PlayerGo player, int id, int dim)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "setdim")) return;

                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }

                if (!target.IsLogged()) return;
                target.Dimension = Convert.ToUInt32(dim);
                GameLog.Admin($"{player.Name}", $"setDim({dim})", $"{target.Name}");
            } catch(Exception e)
            {
                _logger.WriteError("setdim: " + e.ToString());
            }
        }

        [Command("setvehdim")]
        public static void CMD_setVehDim(PlayerGo player, int id, int dim)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "setdim")) return;
                var vehicle = NAPI.Pools.GetAllVehicles().FirstOrDefault(a => a.Value == id);
                if (vehicle == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }

                vehicle.Dimension = (uint)dim;
                GameLog.Admin($"{player.Name}", $"setVehDim({dim})", $"{vehicle.Value}");
            }
            catch (Exception e)
            {
                _logger.WriteError("setdim: " + e.ToString());
            }
        }

        [Command("checkdim")]
        public static void CMD_checkDim(PlayerGo player, int id)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "checkdim")) return;

                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }

                if (!target.IsLogged()) return;
                GameLog.Admin($"{player.Name}", $"checkDim", $"{target.Name}");
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_225".Translate( target.Dimension.ToString()), 4000);
            }
            catch (Exception e)
            {
                _logger.WriteError("checkdim: " + e.ToString());
            }
        }

        [Command("takeoffbiz")]
        public static void CMD_takeOffBusiness(PlayerGo admin, int bizid)
        {
            try
            {
                if (!admin.IsLogged()) return;
                if (!Group.CanUseAdminCommand(admin, "takeoffbiz")) return;

                var biz = BusinessManager.BizList[bizid];
                string owner = biz.GetOwnerName();
                if (biz.TakeBusinessFromOwner(Convert.ToInt32(biz.SellPrice * 0.8), "Money_TakeoffBiz".Translate(biz.ID), "Core_228"))
                {
                    Notify.Send(admin, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_229".Translate(owner), 3000);
                    GameLog.Admin($"{admin.Name}", $"takeoffBiz({biz.ID})", owner);
                }
            }
            catch (Exception e) { _logger.WriteError("takeoffbiz: " + e.ToString()); }
        }
      

        [Command("removeobj")]
        public static void CMD_removeObject(PlayerGo player)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "removeobj")) return;

                player.SetData("isRemoveObject", true);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_233", 3000);
            }
            catch (Exception e) { _logger.WriteError("removeobj: " + e.ToString()); }
        }

        [Command("unwarn")]
        public static void CMD_unwarn(PlayerGo player, int id)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "unwarn")) return;

                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }

                if (!target.IsLogged()) return;
                if (target.GetCharacter().Warns <= 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_234", 3000);
                    return;
                }

                target.GetCharacter().Warns--;
                GUI.MainMenu.SendStats(target);

                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Core_235".Translate( target.Name, target.GetCharacter().Warns), 3000);
                Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_236".Translate(target.GetCharacter().Warns), 3000);
                GameLog.Admin($"{player.Name}", $"unwarn", $"{target.Name}");
            }
            catch (Exception e) { _logger.WriteError("unwarn: " + e.ToString()); }
        }

        [Command("offunwarn")]
        public static void CMD_offunwarn(PlayerGo player, string target)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "unwarn")) return;

                if (!Main.PlayerNames.ContainsValue(target))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_224", 3000);
                    return;
                }
                if (NAPI.Player.GetPlayerFromName(target) != null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_237", 3000);
                    return;
                }

                var split = target.Split('_');
                var data = MySQL.QueryRead("SELECT warns FROM characters WHERE firstname = @prop0 AND lastname = @prop1", split[0], split[1]);
                var warns = 0;
                foreach (DataRow Row in data.Rows)
                {
                    warns = Convert.ToInt32(Row["warns"]);
                }

                if (warns <= 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_239", 3000);
                    return;
                }

                warns--;
                GameLog.Admin($"{player.Name}", $"offUnwarn", $"{target}");
                MySQL.Query("UPDATE characters SET warns = @prop0 WHERE firstname = @prop1 AND lastname = @prop2", warns, split[0], split[1]);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Core_238".Translate( target, warns), 3000);
            }
            catch (Exception e) { _logger.WriteError("offunwarn: " + e.ToString()); }
        }

        [Command("rescar")]
        public static void CMD_respawnCar(PlayerGo player)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "rescar")) return;
                if (!player.IsInVehicle) return;
                var vehicle = player.Vehicle;
                VehicleGo vehGo = vehicle.GetVehicleGo();
                switch (vehGo.Data.OwnerType)
                {
                    case OwnerType.Personal:
                    case OwnerType.Family:
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_240", 3000);
                        break;
                    case OwnerType.Fraction:
                        vehGo.Data.RespawnVehicle();
                        break;
                }

                GameLog.Admin($"{player.Name}", $"rescar", $"");
            }
            catch (Exception e) { _logger.WriteError("ResCar: " + e.ToString()); }
        }

        [Command("spawncar")]
        public static void CMD_spawnCar(PlayerGo player, int carId)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "rescar")) return;

                var vehicle = VehicleManager.GetVehicleByRemoteId(carId);
                
                if (vehicle == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_12", 3000);
                    return;
                }

                VehicleGo vehGo = vehicle.GetVehicleGo();
                switch (vehGo.Data.OwnerType)
                {
                    case OwnerType.Personal:
                    case OwnerType.Family:
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_240", 3000);
                        break;
                    case OwnerType.Fraction:
                        vehGo.Data.RespawnVehicle();
                        break;
                }

                GameLog.Admin($"{player.Name}", $"rescar", $"");
            }
            catch (Exception e) { _logger.WriteError("SpawnCar: " + e.ToString()); }
        }

        [Command("crob")]
        public static void CreateObject(PlayerGo player, string model, float zOffset)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "createobject")) return;
                Console.WriteLine($"hash: {NAPI.Util.GetHashKey(model)}");
                var obj = NAPI.Object.CreateObject(NAPI.Util.GetHashKey(model), player.Position + new Vector3(0, 0, zOffset), new Vector3(), 255, player.Dimension);

            }
            catch (Exception e) { _logger.WriteError("ResCar: " + e.ToString()); }
        }
        [Command("bansync")]
        public static void CMD_banlistSync(PlayerGo player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "ban")) return;
                Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Core_241", 4000);
                Ban.Sync();
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Core_242", 3000);
                GameLog.Admin(player.Name, "bansync", "");
            }
            catch (Exception e) { _logger.WriteError("bansync: " + e.ToString()); }
        }
        [Command("zonecolor")]
        public static void CMD_setTerritoryColor(PlayerGo player, int gangid)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "zonecolor")) return;

                if (player.GetData<int>("GANGPOINT") == -1)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_243", 3000);
                    return;
                }
                var terrid = player.GetData<int>("GANGPOINT");

                if (!Fractions.GangsCapture.gangPointsColor.ContainsKey(gangid))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_244", 3000);
                    return;
                }

                GangsCapture.gangPoints[terrid].GangOwner = gangid;
                Main.ClientEventToAll("setZoneColor", Fractions.GangsCapture.gangPoints[terrid].ID, Fractions.GangsCapture.gangPointsColor[gangid]);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_245".Translate((int)terrid, Manager.getName(gangid)), 3000);
                GameLog.Admin($"{player.Name}", $"setColour({terrid},{gangid})", $"");
                GangsCapture.SavingRegions();
            }
            catch (Exception e) { _logger.WriteError("CMD_SetColour: " + e.ToString()); }
        }
        [Command("clothes")]
        public static void CMD_SetClothesGo(PlayerGo player, int id, int draw, int texture)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "clothes")) return;
                player.SetWhistlerClothes(id, draw, texture);
                if (id == 11) player.SetWhistlerClothes(3, OldCustomization.CorrectTorso[player.GetGender()].GetValueOrDefault(draw), 0);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_SetClothesGo {id} {draw} {texture}: {e}");
            }
        }
        [Command("props")]
        public static void CMD_SetPropsGo(PlayerGo player, int id, int draw, int texture)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "props")) return;
                if (draw > -1)
                    player.SetWhistlerProps(id, draw, texture);
                else
                    player.ClearAccessory(id);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_SetPropsGo: {e}");
            }
        }

        [Command("forbes")]
        public async static void CMD_Forbes(PlayerGo player, int page)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "forbes")) return;
                var result = await MySQL.QueryReadAsync("SELECT ch.`uuid`, `firstname`,`lastname`, ch.`money`, IFNULL(m.`Balance`, 0) as balance, ch.`money` + IFNULL(m.`Balance`, 0) as amountmoney FROM `characters` ch LEFT JOIN `efcore_bank_account` m ON m.ID = ch.banknew WHERE `deleted` = false ORDER BY amountmoney DESC");
                if (result != null)
                {
                    WhistlerTask.Run(() =>
                    {
                        for (int i = (page - 1) * 10; i < page * 10; i++)
                        {
                            if (i < result.Rows.Count)
                            {
                                var uuid = Convert.ToInt32(result.Rows[i]["uuid"]);
                                var name = $"{Convert.ToString(result.Rows[i]["firstname"])}_{Convert.ToString(result.Rows[i]["lastname"])}";
                                var money = Convert.ToInt32(result.Rows[i]["money"]);
                                var bank = Convert.ToInt32(result.Rows[i]["balance"]);
                                var amount = Convert.ToInt32(result.Rows[i]["amountmoney"]);
                                Chat.SendTo(player, "forbes".Translate(i + 1, name, uuid, money, bank, amount));
                            }
                        }
                    });
                }
                GameLog.Admin(player.Name, "forbes", "");
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT \"CMD_Forbes\":" + e.ToString());
            }

        }


        [Command("checkwanted")]
        public static void CMD_checkwanted(PlayerGo player, int id)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "checkwanted")) return;
                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                var stars = (target.GetCharacter().WantedLVL == null) ? 0 : target.GetCharacter().WantedLVL.Level;
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_246".Translate( stars), 3000);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_SetPropsGo: {e}");
            }
        }
        [Command("fixcar")]
        public static void CMD_fixcar(PlayerGo player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "fixcar")) return;
                if (!player.IsInVehicle) return;
                VehicleManager.RepairBody(player.Vehicle);
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT \"CMD_fixcar\":" + e.ToString());
            }
        }
        [Command("fixcarid")]
        public static void CMD_fixcarById(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "fixcar")) return;
                var vehicle = NAPI.Pools.GetAllVehicles().FirstOrDefault(v => v.Value == id);
                if (vehicle == null)
                {
                    Notify.SendError(player, "fixcar");
                    return;
                }
                VehicleManager.RepairBody(vehicle);
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT \"CMD_fixcar\":" + e.ToString());
            }
        }
        [Command("propertystats")]
        public static void CMD_showPlayerHouseStats(PlayerGo admin, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(admin, "propertystats")) return;

                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(admin, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212".Translate(), 3000);
                    return;
                }

                PlayerGo player = Main.GetPlayerByID(id);
                if (!player.IsLogged())
                    return;

                var house = HouseManager.GetHouse(player, true);
                if (house == null)
                    Chat.SendTo(admin,"PlayerGo has no house");
                else
                    Chat.SendTo(admin,$"PlayerGo has {HouseManager.HouseTypeList[house.Type].Name} house | ID {house.ID}");

                var vehicles = VehicleManager.getAllHolderVehicles(player.GetCharacter().UUID, OwnerType.Personal);
                if (vehicles.Count() > 0)
                {
                    Chat.SendTo(admin, $"PlayerGo vehicles:");
                    foreach (var veh in vehicles)
                        Chat.SendTo(admin, $"{VehicleManager.Vehicles[veh].Number} - {VehicleManager.Vehicles[veh].ModelName}");
                }


                var biz = player.GetBusiness();
                if (biz == null)
                    Chat.SendTo(admin,$"PlayerGo has no business");
                else
                    Chat.SendTo(admin,$"PlayerGo has {biz.TypeModel.TypeName} (ID {biz.ID})");
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT \"CMD_housestats\":" + e.ToString());
            }
        }
        [Command("stats")]
        public static void CMD_showPlayerStats(PlayerGo admin, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(admin, "stats")) return;

                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(admin, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212".Translate(), 3000);
                    return;
                }

                PlayerGo player = Main.GetPlayerByID(id);

                #region Stats making
                var character = player.GetCharacter();
                string status =
                    (character.AdminLVL >= 1) ? "Core_247":
                    player.GetAccount().IsPrimeActive() ? 
                    $"Premium account({player.GetAccount().VipDate.ToString("dd.MM.yyyy")})" :
                    "Base account";
                long bank = character.BankModel?.Balance ?? 0;
                var lic = player.GetLicenses();
                if (lic == "") 
                    lic = "none";
                string work = (character.WorkID > 0 && (character.WorkID != Jobs.Technician.Work.WorkID && character.WorkID != Jobs.CarThief.Work.WorkID)) ? Jobs.WorkManager.JobStats[character.WorkID - 1] : "Core_248";
                string fraction = (character.FractionID > 0) ? Manager.getName(character.FractionID) : "";
                #endregion

                //Chat.SendTo(admin, "local_17".Translate(admin, character.LVL, character.EXP, 3 + character.LVL * 3));
                //Chat.SendTo(admin, "local_18".Translate(admin, status, character.Warns, character.CreateDate.ToString("dd.MM.yyyy")));
                //Chat.SendTo(admin, "local_19".Translate(admin, number));
                //Chat.SendTo(admin, "local_14".Translate(admin, lic));
                //Chat.SendTo(admin, "local_20".Translate(admin, character.UUID, character.Bank));
                //Chat.SendTo(admin, "local_16".Translate(admin, work, fraction, character.FractionLVL));

                admin.SendChatMessage("----------------------------");
                admin.SendChatMessage($"Statistics of {player.Name}:");

                admin.SendChatMessage($"LVL: {character.LVL}, EXP: {character.EXP}/{character.LVL * 3 + 3}");
                var phoneNumber = character.PhoneTemporary.Simcard?.Number.ToString() ?? "none";
                admin.SendChatMessage($"Phone number: {phoneNumber}");
                admin.SendChatMessage($"Licenses: {lic}");
                admin.SendChatMessage($"Faction: {character.FractionID} | Rank: {character.FractionLVL}");

                admin.SendChatMessage("----------------------------");
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT \"CMD_showPlayerStats\":" + e.ToString());
            }
        }
        [Command("admins")]
        public static void CMD_AllAdmins(PlayerGo client)
        {
            try
            {
                if (!Group.CanUseAdminCommand(client, "admins")) return;

                Chat.SendTo(client, "admins:1");
                Main.ForEachAllPlayer((p) =>
                {
                    if (p.Character.AdminLVL < 1 || p.Character.AdminLVL >= 7 && client.GetCharacter().AdminLVL < p.Character.AdminLVL) return;
                    Chat.SendTo(client, "admins:2".Translate(p.Name, p.Character.AdminLVL, p.Value));
                });
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT \"CMD_AllAdmins\":" + e.ToString());
            }
        }

      
        [Command("fixweaponsshops")]
        public static void CMD_fixweaponsshops(PlayerGo client)
        {
            try
            {
                if (!Group.CanUseAdminCommand(client, "fixweaponsshops")) return;

                foreach (var biz in BusinessManager.BizList.Values)
                {
                    if (biz.Type != 6) continue;
                    biz.Products = BusinessManager.fillProductList(6);
                }
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT \"CMD_fixweaponsshops\":\n" + e.ToString());
            }
        }
        [Command("giveproduct")]
        public static void CMD_setproductbyindex(PlayerGo player, int id, int index, int product)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "giveproduct")) return;

                var biz = BusinessManager.BizList[id];
                biz.Products[index].Lefts = product;
                GameLog.Admin(player.Name, $"giveproduct({id},{index},{product})", "") ;
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT \"CMD_setproductbyindex\":\n" + e.ToString());
            }
        }
        [Command("removeproducts")]
        public static void CMD_deleteproducts(PlayerGo client, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(client, "removeproducts")) return;

                var biz = BusinessManager.BizList[id];
                foreach (var p in biz.Products)
                    p.Lefts = 0;
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT \"CMD_setproductbyindex\":\n" + e.ToString());
            }
        }
        [Command("changebizprice")]
        public static void CMD_changeBusinessPrice(PlayerGo player, int newPrice)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "changebizprice")) return;
                if (player.GetData<int>("BIZ_ID") == -1)
                {
                    Chat.SendTo(player, "Core_250");
                    return;
                }
                Business biz = BusinessManager.BizList[player.GetData<int>("BIZ_ID")];
                biz.SellPrice = newPrice;
                GameLog.Admin(player.Name, $"changebizprice({biz.ID},{newPrice})", "");
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_changeBusinessPrice: {e}");
            }
        }
        
        [Command("tpc")]
        public static void CMD_tpCoord(PlayerGo player, double x, double y, double z)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "tpc")) return;
                var pos = new Vector3(x, y, z);               
                player.ChangePosition(pos);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_tpCoord: {e}");
            }
        }

        [Command("inv")]
        public static void CMD_ToogleInvisible(PlayerGo player, int state)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "inv")) return;

                Admin.SetInvisible(player, state > 0);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_tpCoord: {e}");
            }
        }

        [Command("removefrac")]
        public static void CMD_delFrac(PlayerGo player, int id)
        {
            try
            {

                if (!Group.CanUseAdminCommand(player, "removefrac")) return;
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.DelFrac(player, Main.GetPlayerByID(id), false);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_delFrac: {e}");
            }
        }

        [Command("tpcar")]
        public static void CMD_tpcar(PlayerGo player, int carID)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "tpcar")) return;
                var vehicles = NAPI.Pools.GetAllVehicles();
                var vehicle = vehicles.Where(a => a.Value == carID).FirstOrDefault();
                if (vehicle == null) {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_117", 3000);
                    return;
                };
                vehicle.Position = player.Position + new Vector3(1.0, 1.0, 0);
                vehicle.Rotation = player.Rotation;
                vehicle.Dimension = player.Dimension;
            }
            catch(Exception e)
            {
                _logger.WriteError($"CMD_tpcar: {e}");
            }
        }

        [Command("sendcreator")]
        public static void CMD_SendToCreator(PlayerGo player, int id)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "sendcreator")) return;
                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }

                if (player.GetCharacter().AdminLVL < target.GetCharacter().AdminLVL)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_10".Translate(), 3000);
                    return;
                }
                player.GetCharacter().ExteriorPos = player.Position;
                CustomizationService.SendToCreator(target, -1);
                GameLog.Admin(player.Name, $"sendCreator", target.Name);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_SendToCreator: {e}");
            }
        }

        [Command("fuel")]
        public static void CMD_setVehiclePetrol(PlayerGo player, int fuel)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "fuel")) return;
                if (!player.IsInVehicle) return;
                VehicleStreaming.SetVehicleFuel(player.Vehicle, fuel);
                GameLog.Admin($"{player.Name}", $"fuel({player.Vehicle.GetVehicleGo().Data.ID},{fuel})", $"");
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_tpCoord: {e}");
            }
        }
        [Command("changename")]
        public static void CMD_changeName(PlayerGo client, string current, string newName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(client, "changename")) return;

                var result = Character.Character.ChangeName(current, newName);

                switch (result)
                {
                    case ChangeNameResult.Success:
                        Notify.Send(client, NotifyType.Alert, NotifyPosition.BottomCenter, "Core_258".Translate(), 3000);
                        break;
                    case ChangeNameResult.BadCurrentName:
                        Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Core_256_2", 3000);
                        break;
                    case ChangeNameResult.IncorrectNewName:
                        Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Core_256_1", 3000);
                        break;
                    case ChangeNameResult.NewNameIsExist:
                        Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Core_256", 3000);
                        break;
                }
                GameLog.Admin(client.Name, $"changeName({newName})", current);

            }catch (Exception e)
            {
                _logger.WriteError($"changename: {e}");
            }
        }
        [Command("changenameid")]
        public static void CMD_changeName(PlayerGo client, int id, string newName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(client, "changenameid")) return;
                PlayerGo target = Main.GetPlayerByID(id);
                if (!target.IsLogged())
                {
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Core_256_3", 3000);
                    return;
                }
                string current = target.GetCharacter().FullName;
                var result = Character.Character.ChangeName(current, newName);
                switch (result)
                {
                    case ChangeNameResult.Success:
                        Notify.Send(client, NotifyType.Alert, NotifyPosition.BottomCenter, "Core_258".Translate(), 3000);
                        break;
                    case ChangeNameResult.BadCurrentName:
                        Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Core_256_2", 3000);
                        break;
                    case ChangeNameResult.IncorrectNewName:
                        Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Core_256_1", 3000);
                        break;
                    case ChangeNameResult.NewNameIsExist:
                        Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Core_256", 3000);
                        break;
                }

                GameLog.Admin(client.Name, $"changeName({newName})", current);

            }catch (Exception e)
            {
                _logger.WriteError($"changename: {e}");
            }
        }
        [Command("startarmwar")]
        public static void CMD_startMatWars(PlayerGo player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "startarmwar")) return;
                if (Fractions.MatsWar.isWar)
                {
                    Chat.SendTo(player,"Core_260");
                    return;
                }
                Fractions.MatsWar.startMatWarTimer();
                Chat.SendTo(player,"Core_260");
                GameLog.Admin(player.Name, $"startarmwar", "");
            }
            catch (Exception e) { _logger.WriteError("startmatwars: " + e.ToString()); }
        }
        [Command("setexp")]
        public static void CMD_giveExp(PlayerGo player, int id, int exp)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "setexp")) return;
                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                target.GetCharacter().EXP += exp;
                while (target.GetCharacter().EXP >= 3 + target.GetCharacter().LVL * 3)
                {
                    target.GetCharacter().EXP = target.GetCharacter().EXP - (3 + target.GetCharacter().LVL * 3);
                    target.GetCharacter().LVL += 1;
                    if(target.GetCharacter().LVL == 1) {
                        WhistlerTask.Run(() => {Trigger.ClientEvent(target, "disabledmg", false); }, 5000);
                    }
                }
                MainMenu.SendStats(target);
                player.SendExpUpdate();
                GameLog.Admin(player.Name, $"giveExp({exp})", target.Name);
            }
            catch (Exception e) { _logger.WriteError("setexp" + e.ToString()); }
        }
        [Command("sethtypeprice")]
        public static void CMD_replaceHousePrices(PlayerGo player, int type, int newPrice)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "sethtypeprice")) return;
                if (!Enum.IsDefined(typeof(HouseTypes), type))
                    return;
                HouseTypes houseTypes = (HouseTypes)type;
                foreach (var h in HouseManager.Houses.Where(item => item.Type == houseTypes && item.OwnerType != OwnerType.Family))
                {
                    h.SetPrice(newPrice);
                }
                GameLog.Admin(player.Name, $"sethtypeprice({type},{newPrice})", "");
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_replaceHousePrices: {e}");
            }
        }
        [Command("sethouseinter")]
        public static void CMD_ChangeHouseType(PlayerGo player, int houseId, int type)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "sethouseinter")) return;
                if (!Enum.IsDefined(typeof(HouseTypes), type))
                    return;
                HouseTypes houseTypes = (HouseTypes)type;
                var house = HouseManager.GetHouseById(houseId);
                house.SetType(houseTypes);
                GameLog.Admin(player.Name, $"sethouseinter({houseId},{type})", "");
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_ChangeHouseType: {e}");
            }
        }
        [Command("takeoffhouse")]
        public static void CMD_deleteHouseOwner(PlayerGo player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "takeoffhouse")) return;
                if (!player.HasData("HOUSEID"))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_261", 3000);
                    return;
                }

                Houses.House house = Houses.HouseManager.Houses.FirstOrDefault(h => h.ID == player.GetData<int>("HOUSEID"));
                if (house == null) return;

                var owner = house.GetHouseOwnerName();
                house.SetOwner(-1, 0);
                GameLog.Admin($"{player.Name}", $"delHouseOwner({house.ID})", owner);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_deleteHouseOwner: {e}");
            }
        }

        [Command("boost")]
        public static void CMD_SetTurboTorque(PlayerGo player, float power, float torque)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "boost")) return;
                if (!player.IsInVehicle || player.Vehicle == null) return;
                VehicleCustomization.SetPowerTorque(player.Vehicle, power, torque);
                GameLog.Admin(player.Name, $"boost({player.Vehicle.GetVehicleGo().Data.ID},{power},{torque})", "");
            }
            catch (Exception e)
            {
                _logger.WriteError("Error at \"STT\":" + e.ToString());
            }
        }

        /// <summary>
        /// Without save to databese
        /// </summary>
        [Command("sttlite")]
        public static void CMD_SetTurboTorqueLite(PlayerGo player, float power, float torque)
        {
            try
            {
                if (player.GetCharacter().AdminLVL < 3) return;
                if (!player.IsInVehicle) return;
                Trigger.ClientEvent(player, "svem", power, torque);
            }
            catch (Exception e)
            {
                _logger.WriteError("Error at \"STTLITE\":" + e.ToString());
            }
        }

        [Command("rtm")]
        public static void CMD_SetVehicleMod(PlayerGo player, int type, int index)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "rtm")) return;
                if (type == 54 && !Group.CanUseAdminCommand(player, "perl")) return;
                if (!player.IsInVehicle) return;
                                
                if (Enum.IsDefined(typeof(ModTypes), type))
                {
                    VehicleCustomization.SetMod(player.Vehicle, (ModTypes)type, index);
                }
                else
                    player.Vehicle.SetMod(type, index);
                GameLog.Admin(player.Name, $"rtm({player.Vehicle.GetVehicleGo().Data.ID},{type},{index})", "");

            }
            catch (Exception e)
            {
                _logger.WriteError("Error at \"SVM\":" + e.ToString());
            }
        }

        [Command("svn")]
        public static void CMD_SetVehicleNeon(PlayerGo player, int r, int g, int b)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "svn")) return;
                if (!player.IsInVehicle) return;

                VehicleCustomization.SetNeon(player.Vehicle, new List<Color>() { new Color(r, g, b) });
                GameLog.Admin(player.Name, $"svn({player.Vehicle.GetVehicleGo().Data.ID})", "");

            }
            catch (Exception e)
            {
                _logger.WriteError("Error at \"SVM\":" + e.ToString());
            }
        }

        [Command("addneon")]
        public static void CMD_AddVehicleNeon(PlayerGo player, int r, int g, int b)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "addneon")) return;
                if (!player.IsInVehicle) return;

                VehicleCustomization.AddNeon(player.Vehicle,  new Color(r, g, b));
                GameLog.Admin(player.Name, $"addneon({player.Vehicle.GetVehicleGo().Data.ID})", "");

            }
            catch (Exception e)
            {
                _logger.WriteError("Error at \"SVM\":" + e.ToString());
            }
        }

        [Command("svh")]
        public static void CMD_SetVehicleHealth(PlayerGo player, int health = 1000)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "svh")) return;
                if (!player.IsInVehicle) return;
                Vehicle vehicle = player.Vehicle;
                vehicle.Repair();
                vehicle.Health = health;
                VehicleManager.RepairVehicle(vehicle);
                GameLog.Admin(player.Name, $"svh({player.Vehicle.GetVehicleGo().Data.ID})", "");

            } catch (Exception e)
            {
                _logger.WriteError("Error at \"SVH\":" + e.ToString());
            }            
        }


        [Command("removecars")]
        public static void CMD_deleteAdminCars(PlayerGo player)
        {
            try
            {
                WhistlerTask.Run(() =>
                {
                    if (!Group.CanUseAdminCommand(player, "removecars")) return;
                    foreach (var veh in NAPI.Pools.GetAllVehicles())
                    {
                        if ((veh.GetVehicleGo().Data.OwnerType == OwnerType.Temporary) && (veh.GetVehicleGo().Data as TemporaryVehicle).Access == VehicleAccess.Admin && veh.HasData("ACCESSADMINBY"))
                            veh.CustomDelete();
                    }
                    GameLog.Admin($"{player.Name}", $"removecars", $"");
                });
            }
            catch (Exception e) { _logger.WriteError("delacars: " + e.ToString()); }
        }
        [Command("removecar")]
        public static void CMD_deleteThisAdminCar(PlayerGo client)
        {
            try
            {
                if (!Group.CanUseAdminCommand(client, "removecar")) return;
                if (!client.IsInVehicle) return;
                Vehicle veh = client.Vehicle;
                if ((veh.GetVehicleGo().Data.OwnerType == OwnerType.Temporary) && (veh.GetVehicleGo().Data as TemporaryVehicle).Access == VehicleAccess.Admin)
                    veh.CustomDelete();
                GameLog.Admin($"{client.Name}", $"removecar", $"");
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_deleteThisAdminCar: {e}");
            }
        }
        [Command("delmycars", "dmcs")]
        public static void CMD_delMyCars(PlayerGo client)
        {
            try
            {
                WhistlerTask.Run(() =>
                {
                    if (!Group.CanUseAdminCommand(client, "vehc")) return;
                    foreach (var v in NAPI.Pools.GetAllVehicles())
                    {
                        if (v.GetData<string>("ACCESSADMINBY") == client.Name)
                            v.CustomDelete();
                    }
                    GameLog.Admin($"{client.Name}", $"delmycars", $"");
                });
            }
            catch (Exception e) { _logger.WriteError("delacars: " + e.ToString()); }
        }
        [Command("spawnallcar")]
        public static void CMD_allSpawnCar(PlayerGo player)
        {
            try
            {
                Admin.respawnAllCars(player);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_allSpawnCar: {e}");
            }
        }
        [Command("scoord")]
        public static void CMD_saveCoord(PlayerGo player, string name)
        {
            try
            {
                Admin.saveCoords(player, name);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_allSpawnCar: {e}");
            }
        }
        [Command("carcoord")]
        public static void CMD_SaveCarCoords(PlayerGo player, string name)
        {
            try
            {
                Admin.SaveCarCoords(player, name);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_allSpawnCar: {e}");
            }
        }
        [Command("loadinteriors")]
        public static void CMD_loadinteriors(PlayerGo player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "loadinteriors")) return;
            player.TriggerEvent("garage:loadInteriors", player.Position, id);
        }

        [Command("stopserver")]
        public static void CMD_stopServer(PlayerGo player, string text = null)
        {
            if (!Group.CanUseAdminCommand(player, "stop")) return;
            Admin.ServerRestart(player.Name, text);
        }

        [Command("payday")]
        public static void payDay(PlayerGo player, string text = null)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "payday")) return;
                GameLog.Admin(player.Name, $"payDay", "");
                Main.payDayTrigger();
            }
            catch (Exception e)
            {
                _logger.WriteError($"payDay: {e}");
            }
        }

        [Command("giveleader")]
        public static void CMD_setLeader(PlayerGo player, int id, int fracid)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.setFracLeader(player, Main.GetPlayerByID(id), fracid);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("sp")]
        public static void CMD_spectateMode(PlayerGo player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "sp")) return;
            try
            {
                PlayerGo target = Main.GetPlayerByID(id);
                if (!target.IsLogged())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                if (target.GetCharacter().AdminLVL >= 7 && player.GetCharacter().AdminLVL < target.GetCharacter().AdminLVL) return;
                AdminSP.Spectate(player, target);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("usp")]
        public static void CMD_unspectateMode(PlayerGo player)
        {
            if (!Group.CanUseAdminCommand(player, "sp")) return;
            try
            {
                AdminSP.UnSpectate(player);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("metp")]
        public static void CMD_teleportToMe(PlayerGo player, int id)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.teleportTargetToPlayer(player, Main.GetPlayerByID(id), false);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("gethere")]
        public static void CMD_teleportVehToMe(PlayerGo player, int id)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.teleportTargetToPlayer(player, Main.GetPlayerByID(id), true);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("kill")]
        public static void CMD_kill(PlayerGo player, int id)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.killTarget(player, Main.GetPlayerByID(id));
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("hp")]
        public static void CMD_adminHeal(PlayerGo player, int id, int hp)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.healTarget(player, Main.GetPlayerByID(id), hp);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("frz")]
        public static void CMD_adminFreeze(PlayerGo player, int id)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.freezeTarget(player, Main.GetPlayerByID(id));
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("unfrz")]
        public static void CMD_adminUnFreeze(PlayerGo player, int id)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.unFreezeTarget(player, Main.GetPlayerByID(id));
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("makeadmin")]
        public static void CMD_setAdmin(PlayerGo player, int id)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.setPlayerAdminGroup(player, Main.GetPlayerByID(id));
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("takeadmin")]
        public static void CMD_delAdmin(PlayerGo player, int id)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.delPlayerAdminGroup(player, Main.GetPlayerByID(id));
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("makemedia")]
        public static void CMD_setMedia(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "makemedia")) return;
                var target = Main.GetPlayerByID(id);
                if (!target.IsLogged())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                target.GetCharacter().Media = 1;
                target.SetSharedData("IS_MEDIA", target.GetCharacter().Media > 0);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_339".Translate( target.Name), 3000);
                Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_340".Translate( player.Name), 3000);
                GameLog.Admin(player.Name, $"makemedia", target.Name);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("takemedia")]
        public static void CMD_delMedia(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "takemedia")) return;
                var target = Main.GetPlayerByID(id);
                if (!target.IsLogged())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                target.GetCharacter().Media = 0;
                target.SetSharedData("IS_MEDIA", target.GetCharacter().Media > 0);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_341".Translate( target.Name), 3000);
                Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_342".Translate( player.Name), 3000);
                GameLog.Admin(player.Name, $"takemedia", target.Name);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("makemediahelper")]
        public static void CMD_setMediaHelper(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "makemediahelper")) return;
                var target = Main.GetPlayerByID(id);
                if (!target.IsLogged())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                target.GetCharacter().MediaHelper = 1;
                target.SetSharedData("IS_MEDIAHELPER", target.GetCharacter().MediaHelper > 0);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_343".Translate( target.Name), 3000);
                Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_344".Translate( player.Name), 3000);
                GameLog.Admin(player.Name, $"makemediahelper", target.Name);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("takemediahelper")]
        public static void CMD_delMediaHelper(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "takemediahelper")) return;
                var target = Main.GetPlayerByID(id);
                if (!target.IsLogged())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                target.GetCharacter().MediaHelper = 0;
                target.SetSharedData("IS_MEDIAHELPER", target.GetCharacter().MediaHelper > 0);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_345".Translate( target.Name), 3000);
                Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_346".Translate( player.Name), 3000);
                GameLog.Admin(player.Name, $"takemediahelper", target.Name);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("setplayericon")]
        public static void CMD_SetPlayerIcon(PlayerGo player, int id, string dictionary = "none", string name = "none", int color = -1)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "setplayericon")) return;
                var target = Main.GetPlayerByID(id);
                if (!target.IsLogged())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                if (dictionary != "none")
                    target.GetCharacter().IconOverHead = new PlayerIconOverHead(dictionary, name, color);
                else
                    target.GetCharacter().IconOverHead = new PlayerIconOverHead();
                target.GetCharacter().IconOverHead?.UpdateSharedData(target);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_347".Translate( target.Name), 3000);
                GameLog.Admin(player.Name, $"setplayericon", target.Name);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("changeadminrank")]
        public static void CMD_setAdminRank(PlayerGo player, int id, int rank)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.setPlayerAdminRank(player, Main.GetPlayerByID(id), rank);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("givegun")]
        public static void CMD_adminGuns(PlayerGo player, int id, string wname)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.giveTargetGun(player, Main.GetPlayerByID(id), wname);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("giveguncomponents")]
        public static void CMD_AdminGunsWithComponents(PlayerGo player, int id, string wname, int muzzle, int flash, int clip, int scope, int grip, int skin)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.giveTargetGunWithComponents(player, Main.GetPlayerByID(id), wname, muzzle, flash, clip, scope, grip, skin);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("giveganja")]
        public static void GiveGanja(PlayerGo player, int id, int count)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "giveganja")) return;
                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                var item = ItemsFabric.CreateNarcotic(ItemNames.Marijuana, count, false);
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
            }
            catch (Exception e) { Console.WriteLine($"GiveGanja\n{e}"); }
        }

        [Command("givegunc")]
        public static void CMD_adminGunsCommon(PlayerGo player, int id, string wname)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.giveTargetGun(player, Main.GetPlayerByID(id), wname, false);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("giveclothes")]
        public static void CMD_adminClothesPromo(PlayerGo player, int id, int type, int drawable, int texture)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.giveTargetClothes(player, Main.GetPlayerByID(id), type, drawable, texture, true);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("giveclothesc")]
        public static void CMD_adminClothes(PlayerGo player, int id, int type, int drawable, int texture)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.giveTargetClothes(player, Main.GetPlayerByID(id), type, drawable, texture, false);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("setskin")]
        public static void CMD_adminSetSkin(PlayerGo player, int id, string pedModel)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.giveTargetSkin(player, Main.GetPlayerByID(id), pedModel);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("givemoney")]
        public static void CMD_adminGiveMoney(PlayerGo player, int id, int money)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "givemoney")) return;
                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                if (Admin.GiveMoney(player, target.GetCharacter(), money))
                {
                    GameLog.Admin(player.Name, $"giveMoney({money})", target.Name);
                    Notify.SendSuccess(player, "Core_353".Translate(money));
                }
                else
                    Notify.SendError(player, "Core_354");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("givebank")]
        public static void CMD_adminGiveBank(PlayerGo player, long bankAccount, int money)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "givemoney")) return;
                var target = BankManager.GetAccountByNumber(bankAccount);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                if (Admin.GiveMoney(player, target, money))
                {
                    GameLog.Admin(player.Name, $"giveBank({money})", $"{bankAccount}");
                    Notify.SendSuccess(player, "Core_353".Translate(money));
                }
                else
                    Notify.SendError(player, "Core_354");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("removeleader")]
        public static void CMD_delleader(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "removefrac")) return;
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.DelFrac(player, Main.GetPlayerByID(id), true);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("removejob")]
        public static void CMD_deljob(PlayerGo player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "removejob")) return;
            if (Main.GetPlayerByID(id) == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                return;
            }
            Admin.DelJob(player, Main.GetPlayerByID(id));
        }

        [Command("vehc")]
        public static void CMD_createVehicleCustom(PlayerGo player, string name, int r, int g, int b)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "vehc")) return;
                VehicleHash vh = (VehicleHash)NAPI.Util.GetHashKey(name);
                if (vh == 0) throw null;
                var vehicle = VehicleManager.CreateTemporaryVehicle(vh, player.Position, player.Rotation, "ELITE", VehicleAccess.Admin);
                vehicle.Dimension = player.Dimension;
                player.SetIntoVehicle(vehicle, VehicleConstants.DriverSeat);
                VehicleCustomization.SetColor(vehicle, new Color(r, g, b), 1, true);
                VehicleCustomization.SetColor(vehicle, new Color(r, g, b), 1, false);
                vehicle.SetData("ACCESSADMINBY", player.Name);
                VehicleStreaming.SetEngineState(vehicle, true);
                GameLog.Admin($"{player.Name}", $"vehCreate({name})", $"");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD_vehc\":\n" + e.ToString()); }
        }

        [Command("veh")]
        public static void CMD_createVehicle(PlayerGo player, string name, int a, int b)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "vehc")) return;
                VehicleHash vh = (VehicleHash)NAPI.Util.GetHashKey(name);
                if (vh == 0)
                {
                    Chat.SendTo(player,"created vehicle");
                    return;
                }
                var vehicle = VehicleManager.CreateTemporaryVehicle(vh, player.Position, player.Rotation, "ELITE", VehicleAccess.Admin);
                vehicle.Dimension = player.Dimension;
                vehicle.PrimaryColor = a;
                vehicle.SecondaryColor = b;
                vehicle.SetData("ACCESSADMINBY", player.Name);
                VehicleStreaming.SetEngineState(vehicle, true);
                GameLog.Admin($"{player.Name}", $"vehCreate({name})", $"");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD_veh\":\n" + e.ToString()); }
        }
        [Command("vehs")]
        public static void CMD_createVehicleCount(PlayerGo player, string name, int a, int b, int count)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "vehs")) return;
                VehicleHash vh = (VehicleHash)NAPI.Util.GetHashKey(name);
                if (vh == 0)
                {
                    Chat.SendTo(player,"created vehicle");
                    return;
                }
                for (int i=0; i<count; i++)
                {
                    var vehicle = VehicleManager.CreateTemporaryVehicle(vh, player.Position + new Vector3(i, 0, 0), player.Rotation, $"GO{i}", VehicleAccess.Admin);
                    vehicle.Dimension = player.Dimension;
                    vehicle.PrimaryColor = a;
                    vehicle.SecondaryColor = b;
                    vehicle.SetData("ACCESSADMINBY", player.Name);
                    VehicleStreaming.SetEngineState(vehicle, true);
                }
                GameLog.Admin($"{player.Name}", $"vehsCreates({name})({count})", $"");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD_veh\":\n" + e.ToString()); }
        }
        [Command("vehstest")]
        public static void CMD_createVehicleCountss(PlayerGo player, string name, int count)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "vehstest")) return;
                VehicleHash vh = (VehicleHash)NAPI.Util.GetHashKey(name);
                if (vh == 0)
                {
                    Chat.SendTo(player, "created vehicle");
                    return;
                }
                List<Vehicle> vehicles = new List<Vehicle>();
                for (int i = 0; i < count; i++)
                {
                    var vehicle = VehicleManager.CreateTemporaryVehicle(vh, player.Position + new Vector3(i, 0, 0), player.Rotation, $"GO{i}", VehicleAccess.Admin);
                    vehicle.SetData("ACCESSADMINBY", player.Name);
                    vehicle.Dimension = player.Dimension;
                    vehicles.Add(vehicle);
                }
                foreach (var veh in vehicles)
                {
                    veh.CustomDelete();
                }
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD_veh\":\n" + e.ToString()); }
        }
        [Command("vehtest")]
        public static void CMD_createVehicleCountssssdaasfas(PlayerGo player, string name)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "vehtest")) return;
                VehicleHash vh = (VehicleHash)NAPI.Util.GetHashKey(name);
                if (vh == 0)
                {
                    Chat.SendTo(player, "created vehicle");
                    return;
                }
                var vehicle = VehicleManager.CreateTemporaryVehicle(vh, player.Position, player.Rotation, $"name", VehicleAccess.Admin);
                vehicle.Dimension = player.Dimension;
                vehicle.CustomDelete();
                vehicle = VehicleManager.CreateTemporaryVehicle(NAPI.Util.GetHashKey("go812"), player.Position, player.Rotation, $"go812", VehicleAccess.Admin);
                vehicle.SetData("ACCESSADMINBY", player.Name);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD_veh\":\n" + e.ToString()); }
        }

        [Command("dirt")]
        public static void CMD_setdirt(PlayerGo player, float dirt)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "setvehdirt")) return;
                if (!player.IsInVehicle) return;

                VehicleStreaming.SetVehicleDirt(player.Vehicle, dirt);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("godspeed")]
        public static void CMD_godspeedon(PlayerGo player, int speed = 200, int step = 10)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "godspeed")) return;
                Trigger.ClientEvent(player, "godspeedon", speed, step);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("newjobcar")]
        public static void newjobveh(PlayerGo player, int typejob, string model, string number, int c1, int c2)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "newjobcar")) return;
                if (!Enum.IsDefined(typeof(WorkType), typejob))
                {
                    Chat.SendTo(player, "newjobcar:1");
                    return;
                }
                number = number.ToUpper();
                if (VehicleManager.Vehicles.FirstOrDefault(item => item.Value.Number == number).Value != null)
                {
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Bad number", 5000);
                    return;
                }
                VehicleHash vh = (VehicleHash)NAPI.Util.GetHashKey(model);
                if (vh == 0) return;
                var jobVeh = new JobVehicle((WorkType)typejob, number, model, player.Position, player.Rotation, c1, c2);
                jobVeh.Spawn();

                Chat.SendTo(player, "newjobcar:2");
                GameLog.Admin(player.Name, $"newjobcar({typejob},{model})", "");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"newjobveh\":\n" + e.ToString()); }
        }

        [Command("removejobcar")]
        public static void CMD_deletejveh(PlayerGo player, int carID)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "removejobcar")) return;
                var vehicles = NAPI.Pools.GetAllVehicles();
                var vehicle = vehicles.FirstOrDefault(a => a.Value == carID);
                if (vehicle == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_118", 3000);
                    return;
                }
                VehicleGo vehGo = vehicle.GetVehicleGo();
                if (vehGo.Data.OwnerType != OwnerType.Job)
                {

                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_119", 3000);
                    return;
                }
                vehGo.Data.DeleteVehicle(vehicle);

                Chat.SendTo(player,"removejobcar".Translate(carID));
                GameLog.Admin(player.Name, $"removejobcar({carID})", $"");
            }
            catch(Exception e) {
                _logger.WriteError("EXCEPTION AT \"deljobveh\":\n" + e.ToString());
            }
        }

        [Command("setjobcar")]
        public static void setjobveh(PlayerGo player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "setjobcar")) return;
              
                if (!player.IsInVehicle)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_150", 3000);
                    return;
                }
                VehicleGo vehGo = player.Vehicle.GetVehicleGo();

                if(vehGo.Data.OwnerType != OwnerType.Job)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_151", 3000);
                    return;
                }
                vehGo.Data.Position = player.Vehicle.Position;
                vehGo.Data.Rotation = player.Vehicle.Rotation;
                MySQL.Query("UPDATE `vehicles` SET `position` = @prop0, `rotation`=@prop1 WHERE idket=@prop2", 
                    JsonConvert.SerializeObject(vehGo.Data.Position), 
                    JsonConvert.SerializeObject(vehGo.Data.Rotation),
                    vehGo.Data.ID
                );
                
                Chat.SendTo(player, "setjobcar");
                GameLog.Admin(player.Name, $"setjobcar({vehGo.Data.ID})", $"");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"setjobveh\":\n" + e.ToString()); }
        }

        [Command("checkban")]
        public static void ACMD_checkban(PlayerGo player, string fullName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "checkban")) return;
                var info = Ban.Banned.FirstOrDefault(b => b.Name == fullName);
                if (info == null)
                {
                    player.SendChatMessage("Couldn't find banned player");
                    return;
                }
                var hard = info.isHard ? "hard" : string.Empty; 
                player.SendChatMessage($"Player: {fullName} {hard} banned by {info.ByAdmin} until {info.Until.Date.ToString(CultureInfo.CurrentCulture)}");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"ACMD_newfracveh\":\n" + e.ToString()); }
        }
        
        [Command("newfracveh")]
        public static void ACMD_newfracveh(PlayerGo player, string model, int fracid, int minRank, string number, int color1, int color2) // add rank, number
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "newfracveh")) return;
                number = number.ToUpper();
                if (VehicleManager.Vehicles.FirstOrDefault(item => item.Value.Number == number).Value != null)
                {
                    Chat.SendTo(player, "newfracveh:1");
                    return;
                }
                VehicleHash vh = (VehicleHash)NAPI.Util.GetHashKey(model);
                if (minRank <= 0 || color1 < 0 || color1 >= 160 || color2 < 0 || color2 >= 160)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_152", 3000);
                    return;
                }
                if (vh == 0) return;
                var vehModel = new FractionVehicle(fracid, model, number, minRank, player.Position, player.Rotation, color1, color2, player.Dimension);
                vehModel.Spawn();

                Chat.SendTo(player, "newfracveh:2");
                GameLog.Admin(player.Name, $"newfracveh({fracid},{model})", $"");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"ACMD_newfracveh\":\n" + e.ToString()); }
        }
        [Command("setfraccar")]
        public static void ACMD_setfracveh(PlayerGo player) {
            try {
                if (!Group.CanUseAdminCommand(player, "setfraccar")) return;
                if (!player.IsInVehicle) 
                {
                    Chat.SendTo(player,"Core_265");
                    return;
                }
                Vehicle vehicle = player.Vehicle;
                
                if (vehicle == null) 
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_153", 3000);
                    return;
                }
                VehicleGo vehGo = vehicle.GetVehicleGo();

                if (vehGo.Data.OwnerType == OwnerType.Fraction)
                {
                    Vector3 pos = NAPI.Entity.GetEntityPosition(vehicle) + new Vector3(0, 0, 0.5);
                    Vector3 rot = NAPI.Entity.GetEntityRotation(vehicle);

                    vehGo.Data.Position = pos;
                    vehGo.Data.Rotation = rot;

                    MySQL.Query("UPDATE vehicles SET position = @prop0, rotation = @prop1, dimension = @prop2 WHERE idkey = @prop3", JsonConvert.SerializeObject(pos), JsonConvert.SerializeObject(rot), vehicle.Dimension, vehGo.Data.ID);

                    Chat.SendTo(player,"Core_266");
                    GameLog.Admin(player.Name, $"setfraccar({vehGo.Data.ID})", $"");
                } 
                else 
                    Chat.SendTo(player,"Core_263");
            } catch (Exception e) { _logger.WriteError("EXCEPTION AT \"ACMD_setfracveh\":\n" + e.ToString()); }
        }
        [Command("changefracvehrank")]
        public static void ACMD_ChangeFractionVehicleMinimalRank(PlayerGo player, int newRank)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "changefracvehrank")) return;

                if (!player.IsInVehicle)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_154", 3000);
                    return;
                }

                var vehicle = player.Vehicle;
                VehicleGo vehGo = vehicle.GetVehicleGo();
                if (vehGo.Data.OwnerType != OwnerType.Fraction)
                    return;

                (vehGo.Data as FractionVehicle).MinRank = newRank;

                MySQL.Query("UPDATE `vehicles` SET `rank` = @prop1 WHERE `idkey` = @prop0", vehGo.Data.ID, newRank);

                Chat.SendTo(player,"changefracvehrank".Translate(newRank));
                GameLog.Admin(player.Name, $"changefracvehrank({vehGo.Data.ID},{newRank})", $"");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"ACMD_delfracveh\":\n" + e.ToString()); }
        }
        [Command("delfracveh")]
        public static void ACMD_delfracvehe(PlayerGo player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "delfracvehe")) return;

                if (!player.IsInVehicle)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_114", 3000);
                    return;
                }

                var vehicle = player.Vehicle;
                VehicleGo vehGo = vehicle.GetVehicleGo();
                if (vehGo.Data.OwnerType != OwnerType.Fraction)
                    return;
                int carId = vehGo.Data.ID;
                vehGo.Data.DeleteVehicle(vehicle);

                Chat.SendTo(player, "delfracveh");
                GameLog.Admin(player.Name, $"delfracvehe({carId})", $"");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"ACMD_delfracveh\":\n" + e.ToString()); }
        }
        [Command("delfraccar")]
        public static void ACMD_delfracveh(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "delfraccar")) return;
                var vehicles = NAPI.Pools.GetAllVehicles();
                var vehicle = vehicles.FirstOrDefault(v => v.Value == id);
                if (vehicle == null)
                    return;
                VehicleGo vehGo = vehicle.GetVehicleGo();
                if (vehGo.Data.OwnerType != OwnerType.Fraction)
                    return;
                int carId = vehGo.Data.ID;
                vehGo.Data.DeleteVehicle(vehicle);
                Chat.SendTo(player, "delfracveh");
                GameLog.Admin(player.Name, $"delfraccar({carId})", $"");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"ACMD_delfracveh\":\n" + e.ToString()); }
        }

        [Command("vehhash")]
        public static void CMD_createVehicleHash(PlayerGo player, string name, int a, int b)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "vehhash")) return;
                var vehicle = VehicleManager.CreateTemporaryVehicle(Convert.ToInt32(name, 16), player.Position, player.Rotation, "PROJECT", VehicleAccess.Admin);
                vehicle.Dimension = player.Dimension;
                vehicle.PrimaryColor = a;
                vehicle.SecondaryColor = b;
                vehicle.SetData("ACCESSADMINBY", player.Name);
                VehicleStreaming.SetEngineState(vehicle, true);
                GameLog.Admin(player.Name, $"vehhash({name})", $"");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD_vehhash\":\n" + e.ToString()); }
        }

        //[Command("aclear")]
        //public static void ACMD_aclear(PlayerGo player, string target) {
        //    try {
        //        if (!player.IsLogged()) return;
        //        if (!Group.CanUseCmd(player, "aclear")) return;
        //        var uuid = Main.PlayerNames.FirstOrDefault(item => item.Value == target).Key;
        //        if (uuid == 0)
        //        {
        //            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_224", 3000);
        //            return;
        //        }
        //        if (Main.GetPlayerByUUID(uuid) != null)
        //        {
        //            Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Core_267", 3000);
        //            return;
        //        }
        //        // CLEAR BIZ
        //        DataTable result = MySQL.QueryRead($"SELECT uuid, adminlvl, biz, bank, fraction FROM `characters` WHERE uuid = @prop0", uuid);
        //        if (result != null && result.Rows.Count != 0) {
        //            DataRow row = result.Rows[0];
        //            if(Convert.ToInt32(row["adminlvl"]) >= player.GetCharacter().AdminLVL) {
        //                Chat.SendToAdmins(3, "Com_100".Translate( player.Name, player.Value, target));
        //                return;
        //            }
        //            Manager.GetFraction(Convert.ToInt32(row["fraction"]))?.DeleteMember(uuid);
        //            var biz = BusinessManager.GetBusinessByOwner(uuid);
        //            if (biz != null)
        //            {
        //                biz.SetOwner(-1);
        //                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_269".Translate(target), 3000);
        //            }
        //            // CLEAR BANK MONEY
        //            Bank.Data bankAcc = Bank.Get(Convert.ToInt32(row["bank"]));
        //            if (bankAcc != null)
        //                Bank.Set(bankAcc.ID, 0);
        //        } else {
        //            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_270", 3000);
        //            return;
        //        }
        //        // CLEAR HOUSE
        //        House house = HouseManager.GetHouse(uuid, OwnerType.Personal, true);
        //        if (house != null)
        //        {
        //            house.SetOwner(-1, 0);
        //            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_271".Translate(target), 3000);
        //        }
        //        // CLEAR VEHICLES
        //        var vehicles = VehicleManager.getAllHolderVehicles(uuid, VehicleType.Personal);
        //        foreach (int item in vehicles)
        //        {
        //            VehicleManager.Remove(item);
        //        }


        //        // CLEAR MONEY, HOTEL, FRACTION, SIMCARD, PET
        //        MySQL.Query("UPDATE `characters` SET `money`=0 WHERE uuid = @prop0", uuid);

        //        // CLEAR ITEMS
        //        //if(tuuid != 0) MySQL.Query("UPDATE `inventory` SET `items`='[]' WHERE `uuid` = @prop0", tuuid);
        //        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Core_273".Translate( target), 3000);
        //        GameLog.Admin($"{player.Name}", $"aClear", $"{target}");
        //    } catch (Exception e) { _logger.WriteError("EXCEPTION AT aclear\n" + e.ToString()); }
        //}

        [Command("findbyveh")]
        public static void CMD_FindByVeh(PlayerGo player, string number) {
            try
            {
                if (!Group.CanUseAdminCommand(player, "findbyveh")) return;
                if (number.Length > 8)
                {
                    Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Core_274", 3000);
                    return;
                }
                var vehData = VehicleManager.Vehicles.FirstOrDefault(item => item.Value.Number == number);
                if (vehData.Value != null)
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Core_275".Translate( number, vehData.Value.ModelName, vehData.Value.GetHolderName()), 6000);
                else
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_276", 3000);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_FindByVeh: {e}");
            }
        }

        [Command("findvehbynumber")]
        public static void CMD_FindVehByNumber(PlayerGo player, string number)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "findvehbynumber")) return;
                var vehicles = NAPI.Pools.GetAllVehicles().Where(item => item.NumberPlate == number);
                foreach (var veh in vehicles)
                {
                    Chat.SendTo(player, $"model: {veh.GetModelName()}, pos: {veh.Position.X} {veh.Position.Y} {veh.Position.Z}, dim: {veh.Dimension}, id: {veh.Value}");
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"findvehbynumber: {e}");
            }
        }

        [Command("findvehholder")]
        public static void CMD_FindVehByHolder(PlayerGo player, int holder, int type)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "findvehholder")) return;
                var vehicles = VehicleManager.GoVehicles.Where(item => item.Value.Data.OwnerID == holder && (int)item.Value.Data.OwnerType == type);
                foreach (var veh in vehicles)
                {
                    Chat.SendTo(player, $"model: {veh.Key.GetModelName()}, pos: {veh.Key.Position.X} {veh.Key.Position.Y} {veh.Key.Position.Z}, dim: {veh.Key.Dimension}, id: {veh.Key.Value}");
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"findvehholder: {e}");
            }
        }

        //[Command("character")]
        //public static void SelectCharacter(PlayerGo player, int number)
        //{
        //    try
        //    {

        //        number--;
        //        if (!player.IsLogged()) return;
        //        if (number < 0 || number > 2)
        //        {
        //            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "selectchar:err", 3000);
        //            return;
        //        }
        //        var acc = player.GetAccount();
        //        if (acc.Characters[number] == -1)
        //        {
        //            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "selectchar:err:2", 3000);
        //            return;
        //        }
        //        acc.SetLastCharacter(number);
        //        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "selectchar:succ", 3000);
        //        WhistlerTask.Run(() =>{player.Kick();}, 1000);
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.WriteError($"SelectCharacter: {e}");
        //    }
        //}

        [Command("weather")]
        public static void CMD_SetWeather(PlayerGo player, byte weather)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "weather")) return;
                WeatherManager.ChangeWeather(weather);
                GameLog.Admin($"{player.Name}", $"setWeather({weather})", $"");
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_SetWeather: {e}");
            }
        }

        [Command("swtime")]
        public static void CMD_StopTime(PlayerGo player, int val)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "swtime")) return;
                NAPI.ClientEvent.TriggerClientEventForAll("switchTime", val);
                GameLog.Admin(player.Name, $"switchTime({val})", "");
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_StopTime: {e}");
            }
        }
        [Command("setRain")]
        public static void CMD_SetRain(PlayerGo player, float rain)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "setRain")) return;
                player.TriggerEvent("weather:set:rain", rain);
                GameLog.Admin(player.Name, $"setRain({rain})", "");
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_SetRain: {e}");
            }
        }
        [Command("st")]
        public static void CMD_setTime(PlayerGo player, int hours, int minutes, int seconds)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "st")) return;
                NAPI.World.SetTime(hours, minutes, seconds);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_setTime: {e}");
            }
        }

        [Command("tp")]
        public static void CMD_teleport(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "tp")) return;
               
                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }              
                Admin.teleportToPlayer(player, target);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("goto")]
        public static void CMD_teleportveh(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "tp")) return;
                if(!player.IsInVehicle) return;
                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                
                player.Vehicle.Dimension = NAPI.Entity.GetEntityDimension(target);
                player.Vehicle.Position = target.Position + new Vector3(2, 2, 2);
                AdminParticles.PlayAdminAppearanceEffect(player);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("flip")]
        public static void CMD_flipveh(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "tp")) return;
                PlayerGo target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                if(!target.IsInVehicle) {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_277", 3000);
                    return;
                }
                target.Vehicle.Position = target.Vehicle.Position + new Vector3(0,0,2.5f);
                target.Vehicle.Rotation = new Vector3(0,0,target.Vehicle.Rotation.Z);
                GameLog.Admin($"{player.Name}", $"flipVeh", $"{target.Name}");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("flipcar")]
        public static void CMD_flipCar(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "tp")) return;
                var vehicle = NAPI.Pools.GetAllVehicles().FirstOrDefault(v => v.Value == id);
                if (vehicle == null)
                {
                    Notify.SendError(player, "flipcar");
                    return;
                }
                vehicle.Position = vehicle.Position + new Vector3(0,0,2.5f);
                vehicle.Rotation = new Vector3(0,0,vehicle.Rotation.Z);
                GameLog.Admin($"{player.Name}", $"flipVeh", $"{vehicle.Model}");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("createbusiness")]
        public static void CMD_createBiz(PlayerGo player, int govPrice, int type)
        {
            try
            {
                BusinessManager.createBusinessCommand(player, govPrice, type);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("createunloadpoint")]
        public static void CMD_createUnloadPoint(PlayerGo player, int bizid)
        {
            try
            {
                BusinessManager.createBusinessUnloadpoint(player, bizid);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("deletebusiness")]
        public static void CMD_deleteBiz(PlayerGo player, int bizid)
        {
            try
            {
                BusinessManager.deleteBusinessCommand(player, bizid);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
 
        [Command("demorgan", GreedyArg = true)]
        public static void CMD_sendTargetToDemorgan(PlayerGo player, int id, int time, string reason)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.sendPlayerToDemorgan(player, Main.GetPlayerByID(id), time, reason);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("loadipl")]
        public static void CMD_LoadIPL(PlayerGo player, string ipl) {
            try {
                if (!Group.CanUseAdminCommand(player, "setvehdirt")) return;
                NAPI.World.RequestIpl(ipl);
                Chat.SendTo(player, "local_24".Translate( ipl));
            } catch {
            }
        }
        [Command("unloadipl")]
        public static void CMD_UnLoadIPL(PlayerGo player, string ipl) {
            try {
                if (!Group.CanUseAdminCommand(player, "setvehdirt")) return;
                NAPI.World.RemoveIpl(ipl);
                Chat.SendTo(player, "unloadipl".Translate(ipl));
            } catch(Exception e) {
                _logger.WriteError($"CMD_UnLoadIPL:\n{e}");
            }
        }

        [Command("starteffect")]
        public static void CMD_StartEffect(PlayerGo player, string effect, int dur = 0, bool loop = false) {
            try {
                if (!Group.CanUseAdminCommand(player, "setvehdirt")) return;
                Trigger.ClientEvent(player, "startScreenEffect", effect, dur, loop);
                Chat.SendTo(player, "local_25".Translate( effect));
            } catch {
            }
        }
        [Command("stopeffect")]
        public static void CMD_StopEffect(PlayerGo player, string effect) {
            try {
                if (!Group.CanUseAdminCommand(player, "setvehdirt")) return;
                Trigger.ClientEvent(player, "stopScreenEffect", effect);
                Chat.SendTo(player, "local_25".Translate( effect));
            } catch {
            }
        }
        [Command("udemorgan")]
        public static void CMD_releaseTargetFromDemorgan(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "udemorgan")) return;
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.ReleasePlayerFromDemorgan(player, Main.GetPlayerByID(id));
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        
        [Command("offjail", GreedyArg = true)]
        public static void CMD_offlineJailTarget(PlayerGo player, string target, int time, string reason)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "offjail")) return;
                if (!Main.PlayerNames.ContainsValue(target))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_224", 3000);
                    return;
                }
                if(player.Name.Equals(target)) return;
                if (NAPI.Player.GetPlayerFromName(target) != null)
                {
                    Admin.sendPlayerToDemorgan(player, NAPI.Player.GetPlayerFromName(target) as PlayerGo, time, reason);
                    Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "local_157", 3000);
                    return;
                }

                var uuid = Main.PlayerNames.FirstOrDefault(p => p.Value == target).Key;
                if (uuid == default)
                    return;
                var account = Main.GetAccountByUUID(uuid);
                if (account == null)
                {
                    DataTable responce = MySQL.QueryRead("SELECT `vipdate` FROM `accounts` WHERE `character1`=@prop0 OR `character2`=@prop0 OR `character3`=@prop0", uuid);
                    if(responce != null && responce.Rows.Count > 0)
                    {
                        var vipDate =((DateTime)responce.Rows[0]["vipdate"]);
                        if (vipDate > DateTime.UtcNow) time /= 2;
                    }
                }
                else
                {
                    if (account.VipDate > DateTime.UtcNow) time /= 2;
                }
                
                var firstTime = time * 60;
                var deTimeMsg = "m";
                if (time > 60)
                {
                    deTimeMsg = "h";
                    time /= 60;
                    if (time > 24)
                    {
                        deTimeMsg = "d";
                        time /= 24;
                    }
                }
                var character = Main.GetCharacterByUUID(uuid);
                if (character != null)
                {
                    character.DemorganTime = firstTime;
                    character.ArrestDate = DateTime.UtcNow;
                }
                MySQL.QuerySync("UPDATE characters SET demorgan = @prop0, arrest = 0 WHERE uuid = @prop1", firstTime, uuid);
                Chat.AdminToAll("Com_121".Translate(player.Name, target, time, deTimeMsg, reason));
                GameLog.Admin($"{player.Name}", $"demorgan({time}{deTimeMsg},{reason})", $"{target}");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("offwarn", GreedyArg = true)]
        public static void CMD_offlineWarnTarget(PlayerGo player, string target, int time, string reason)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "offwarn")) return;
                var uuid = Main.PlayerNames.FirstOrDefault(item => item.Value == target).Key;
                if (uuid == 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_224", 3000);
                    return;
                }
                if(player.Name.Equals(target)) return;
                if (NAPI.Player.GetPlayerFromName(target) != null)
                {
                    Admin.warnPlayer(player, NAPI.Player.GetPlayerFromName(target) as PlayerGo, reason);
                    Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Core_278", 3000);
                    return;
                } 
                else 
                {
                    string[] split1 = target.Split('_');
                    DataTable result = MySQL.QueryRead("SELECT adminlvl FROM characters WHERE uuid = @prop0", uuid);
                    DataRow row = result.Rows[0];
                    int targetadminlvl = Convert.ToInt32(row[0]);
                    if(targetadminlvl >= player.GetCharacter().AdminLVL) {
                        Chat.SendToAdmins(3, "Com_101".Translate( player.Name, player.GetCharacter().UUID, target));
                        return;
                    }
                }

                
                var split = target.Split('_');
                var data = MySQL.QueryRead("SELECT warns, fraction FROM characters WHERE firstname = @prop0 AND lastname = @prop1", split[0], split[1]);
                var warns = Convert.ToInt32(data.Rows[0]["warns"]);
                var frac = Convert.ToInt32(data.Rows[0]["fraction"]);
                Manager.GetFraction(frac)?.DeleteMember(uuid);
                warns++;
                if (warns >= 3)
                {
                    MySQL.Query("UPDATE `characters` SET `warns`=0 WHERE uuid = @prop0", uuid);
                    Ban.Offline(target, DateTime.Now.AddMinutes(43200), false, "Warns 3/3", "Server_Serverniy");
                }
                else
                    MySQL.Query($"UPDATE `characters` SET `unwarn`=@prop0,`warns`=@prop1 WHERE uuid=@prop2 ", MySQL.ConvertTime(DateTime.Now.AddDays(14)), warns, uuid);

                Chat.AdminToAll("Com_141".Translate( player.Name, target, warns, reason));
                GameLog.Admin($"{player.Name}", $"warn({time},{reason})", $"{target}");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("ban", GreedyArg = true)]
        public static void CMD_banTarget(PlayerGo player, int id, int time, string reason)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "ban")) return;
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.BanPlayer(player, Main.GetPlayerByID(id), time, reason, false);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("hardban", GreedyArg = true)]
        public static void CMD_hardbanTarget(PlayerGo player, int id, int time, string reason)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.hardbanPlayer(player, Main.GetPlayerByID(id), time, reason);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("offban", GreedyArg = true)]
        public static void CMD_offlineBanTarget(PlayerGo player, string name, int time, string reason)
        {
            try
            {
                if (!Main.PlayerNames.ContainsValue(name))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_279", 3000);
                    return;
                }
                Admin.offBanPlayer(player, name, time, reason);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("offhardban", GreedyArg = true)]
        public static void CMD_offlineHardbanTarget(PlayerGo player, string name, int time, string reason)
        {
            try
            {
                if (!Main.PlayerNames.ContainsValue(name))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_279", 3000);
                    return;
                }
                Admin.offHardBanPlayer(player, name, time, reason);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("unban", GreedyArg = true)]
        public static void CMD_unbanTarget(PlayerGo player, string name)
        {
            if (!Group.CanUseAdminCommand(player, "unban")) return;
            try
            {
                Admin.unbanPlayer(player, name);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("unhardban", GreedyArg = true)]
        public static void CMD_unhardbanTarget(PlayerGo player, string name)
        {
            if (!Group.CanUseAdminCommand(player, "unhardban")) return;
            try
            {
                Admin.UnhardbanPlayer(player, name);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("givedonateoff")]
        public static void CMD_offdonate(PlayerGo client, string name, long amount)
        {
            if (!Group.CanUseAdminCommand(client, "givedonateoff")) return;
            try
            {
                name = name.ToLower();
                
                var playerGo = Main.GetPlayerGoByPredicate(playerGo => playerGo.Account.Login == name);
                if (playerGo != null)
                {
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_280".Translate(), 8000);
                    return;
                }
                MySQL.Query("update `accounts` set `gocoins`=`gocoins`+@prop0 where `login`=@prop1", amount, name);
                GameLog.Admin(client.Name, $"offgivedonate({amount})", name);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("updatemail")]
        public static void CMD_UpdateMail(Player client, string login, string newEmail)
        {
            if (!Group.CanUseAdminCommand(client, "updatemail")) return;
            var currEmail = Main.Emails.FirstOrDefault(item => item.Value == login);
            if (currEmail.Key == null)
            {
                Notify.SendError(client, "Логин не найден");
                return;
            }
            Main.Emails.Remove(currEmail.Key);
            Main.Emails.Add(newEmail, currEmail.Value);

            var playerGo = Main.GetPlayerGoByPredicate(playerGo => playerGo.Account.Login == login);
            if (playerGo != null)
                playerGo.Account.UpdateEmail(newEmail);
            else
                MySQL.Query("update `accounts` set `email` = @prop0 where `login`=@prop1", newEmail, login);
            GameLog.Admin(client.Name, $"updatemail({newEmail})", login);
        }
        [Command("mute", GreedyArg = true)]
        public static void CMD_muteTarget(PlayerGo player, int id, int time, string reason)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.mutePlayer(player, Main.GetPlayerByID(id) as PlayerGo, time, reason);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("offmute", GreedyArg = true)]
        public static void CMD_offlineMuteTarget(PlayerGo player, string target, int time, string reason)
        {
            try
            {
                if (!Main.PlayerNames.ContainsValue(target))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_224", 3000);
                    return;
                }
                Admin.OffMutePlayer(player, target, time, reason);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("unmute")]
        public static void CMD_muteTarget(PlayerGo player, int id)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.unmutePlayer(player, Main.GetPlayerByID(id) as PlayerGo);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("vmute")]
        public static void CMD_voiceMuteTarget(PlayerGo player, int id)
        {
            try
            {
                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }

                if (!Group.CanUseAdminCommand(player, "mute")) return;

                if (player.GetCharacter().AdminLVL < target.GetCharacter().AdminLVL)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_10".Translate(), 3000);
                    return;
                }

                target.SetSharedData("voice.muted", true);
                Trigger.ClientEvent(target, "voice.mute");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("vunmute")]
        public static void CMD_voiceUnMuteTarget(PlayerGo player, int id)
        {
            try
            {
                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }

                if (!Group.CanUseAdminCommand(player, "unmute")) return;
                target.SetSharedData("voice.muted", false);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("sban")]
        public static void CMD_silenceBan(PlayerGo player, int id, int time)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "sban")) return;
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.BanPlayer(player, Main.GetPlayerByID(id), time, "", true);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("kick", GreedyArg = true)]
        public static void CMD_kick(PlayerGo player, int id, string reason)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.kickPlayer(player, Main.GetPlayerByID(id), reason, false);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("skick")]
        public static void CMD_silenceKick(PlayerGo player, int id)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.kickPlayer(player, Main.GetPlayerByID(id), "Silence kick", true);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("gm")]
        public static void CMD_enableGodmode(PlayerGo player)
        {
            if (!Group.CanUseAdminCommand(player, "gm")) return;
            if (!player.HasData("AGM") || !player.GetData<bool>("AGM")) {
                player.TriggerEvent("AGM", true);
                player.SetData("AGM", true);
            } else {
                player.TriggerEvent("AGM", false);
                player.SetData("AGM", false);
            }
        }
        [Command("warn", GreedyArg = true)]
        public static void CMD_warnTarget(PlayerGo player, int id, string reason)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.warnPlayer(player, Main.GetPlayerByID(id), reason);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("pm", GreedyArg = true)]
        public static void CMD_adminSMS(PlayerGo player, int id, string msg)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.adminSMS(player, Main.GetPlayerByID(id), msg);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
       
       
        [Command("setprime")]
        public static void CMD_SetPlayerPrime(PlayerGo player, int id, int days)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.setPlayerPrimeAccount(player, Main.GetPlayerByID(id), days);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        [Command("checkmoney")]
        public static void CMD_checkMoney(PlayerGo player, int id)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_212", 3000);
                    return;
                }
                Admin.checkMoney(player, Main.GetPlayerByID(id));
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("reportoff")]
        public static void CMD_reportoff(PlayerGo player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "reportoff")) return;

                if (player.GetCharacter().ReportNotification == true)
                {
                    player.GetCharacter().ReportNotification = false;
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "local_120", 3000);
                }
                else
                {
                    player.GetCharacter().ReportNotification = true;
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"local_121", 3000);
                }
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }        

        [Command("allseeingeye")]
        public static void CMD_allseeingeye(PlayerGo player, int flag)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "takeadmin")) return;
                if (flag == 1)
                    player.SetData("ALLSEEINEYE", true);
                else
                    player.SetData("ALLSEEINEYE", false);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        #endregion

        [Command("farmsave")]
        public static void CMD_SaveFarmPoint(PlayerGo player, int farmId)
        {
            try
            {
                if (!Directory.Exists("farms"))
                {
                    Directory.CreateDirectory("farms");
                }

                using (StreamWriter saveCoords = new StreamWriter($"farms/{farmId}.txt", true, Encoding.UTF8))
                {
                    var pos = player.Position - new Vector3(0, 0, 1.12);

                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                    saveCoords.Write($"new Vector3({pos.X}, {pos.Y}, {pos.Z}),\r\n");
                    saveCoords.Close();
                }
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"farmsave\":\n" + e.ToString()); }
        }

        [Command("checkdonat")]
        public static void CMD_checkDonat(PlayerGo player, int hour)
        {
            if (!Group.CanUseAdminCommand(player, "checkdonat")) return;
            try
            {
                if (hour < 1)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_122", 3000);
                    return;
                };
                var result = MySQL.QueryRead("SELECT SUM(`sum`) FROM `@prop1` WHERE unitpayid > 0 AND NOW() -  INTERVAL @prop0 HOUR < `date`;", hour, Main.ServerConfig.DonateConfig.Database);
                if (result == null || result.Rows.Count == 0 || result.Rows[0][0] is DBNull)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_123", 3000);
                    return;
                };
                var sum = Convert.ToInt32(result.Rows[0][0]);
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_124".Translate( hour, sum), 3000);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        #region VipCommands
        [Command("leave")]
        public static void CMD_leaveFraction(PlayerGo player)
        {
            try
            {
                if (!player.GetAccount().IsPrimeActive()) return;
                if (Manager.GetFraction(player).DeleteMember(player.GetCharacter().UUID))
                {
                    Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "local_125", 3000);
                }

            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        #endregion

        [Command("ticket", GreedyArg = true)]
        public static void CMD_govTicket(PlayerGo player, int id, int sum, string reason)
        {
            try
            {
                var target = Main.GetPlayerByID(id);
                if (sum < 1) return;
                if (target == null || !target.IsLogged())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_281", 3000);
                    return;
                }
                if (target.Position.DistanceTo(player.Position) > 2)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Biz_52", 3000);
                    return;
                }
                Fractions.FractionCommands.ticketToTarget(player, target, sum, reason);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("respawn")]
        public static void CMD_respawnFracCars(PlayerGo player)
        {
            try
            {
                var fraction = Manager.GetFraction(player);
                if (fraction == null || !fraction.IsLeaderOrSub(player)) return;
                if (DateTime.Now < FractionCommands.NextCarRespawn[player.GetCharacter().FractionID])
                {
                    DateTime g = new DateTime((FractionCommands.NextCarRespawn[player.GetCharacter().FractionID] - DateTime.Now).Ticks);
                    var min = g.Minute;
                    var sec = g.Second;
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_178".Translate( min, sec), 3000);
                    return;
                }
                Fractions.FractionCommands.RespawnFractionCars(player.GetCharacter().FractionID);

                FractionCommands.NextCarRespawn[player.GetCharacter().FractionID] = DateTime.Now.AddHours(2);
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_179", 3000);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("givemedlic")]
        public static void CMD_givemedlic(PlayerGo player, int id)
        {
            try
            {
                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_281", 3000);
                    return;
                }
                if (target.Position.DistanceTo(player.Position) > 2)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_52", 3000);
                    return;
                }
                FractionCommands.giveMedicalLic(player, target);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
               

        [Command("setgamblingtax")]
        public static void CmdSetGamblingTaxCallBack(PlayerGo player, int percent)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "setgamblingtax")) return;
                if (percent < 0 || percent >= 100) return;
                var oldTax = CasinoManager.StateShare;
                CasinoManager.UpdateStateShare((double)percent / 100);
                Notify.Send(player, NotifyType.Success, NotifyPosition.Bottom, "local_126".Translate( oldTax * 100, CasinoManager.StateShare * 100), 3000);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        
        [Command("password")]
        public static void CMD_ResetPassword(PlayerGo player, string new_password)
        {
            try
            {
                if (!player.IsLogged()) return;
                player.GetAccount().changePassword(new_password);
                Notify.Send(player, NotifyType.Alert, NotifyPosition.BottomCenter, "Core_286", 3000);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_ResetPassword: {e}");
            }
        }
        [Command("time")]
        public static void CMD_checkPrisonTime(PlayerGo player)
        {
            try
            {
                if (player.GetCharacter().ArrestDate > DateTime.UtcNow)
                {
                    var period = player.GetCharacter().ArrestDate - DateTime.UtcNow;
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_287".Translate(period.TotalMinutes), 3000);
                }                    
                else if (player.GetCharacter().DemorganTime != 0)
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_287".Translate( Convert.ToInt32(player.GetCharacter().DemorganTime / 60.0)), 3000);
                else if (player.GetCharacter().ArrestiligalTime != 0)
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_287".Translate( Convert.ToInt32(player.GetCharacter().ArrestiligalTime / 60.0)), 3000);
                else if (player.GetCharacter().CourtTime != 0)
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_287".Translate( Convert.ToInt32(player.GetCharacter().CourtTime / 60.0)), 3000);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("ptime")]
        public static void CMD_pcheckPrisonTime(PlayerGo player, int id)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "a")) return;
                PlayerGo target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_281", 3000);
                    return;
                }
                if (target.GetCharacter().ArrestDate > DateTime.UtcNow)
                {
                    var period = target.GetCharacter().ArrestDate - DateTime.UtcNow;
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_288".Translate( target.Name, period.TotalMinutes), 3000);
                }  
                else if (target.GetCharacter().DemorganTime != 0)
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_288".Translate( target.Name, Convert.ToInt32(target.GetCharacter().DemorganTime / 60.0)), 3000);
                    else if (target.GetCharacter().ArrestiligalTime != 0)
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_288".Translate( target.Name, Convert.ToInt32(target.GetCharacter().ArrestiligalTime / 60.0)), 3000);
                    else if (target.GetCharacter().CourtTime != 0)
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_288".Translate( target.Name, Convert.ToInt32(target.GetCharacter().CourtTime / 60.0)), 3000);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }
        
        [Command("q")]
        public static void CMD_disconnect(PlayerGo player)
        {
            try
            {
                Trigger.ClientEvent(player, "kick", null);
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_disconnect: {e}");
            }
        }

        [Command("eject")]
        public static void CMD_ejectTarget(PlayerGo player, int id)
        {
            try
            {
                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_281", 3000);
                    return;
                }
                if (!player.IsInVehicle || player.VehicleSeat != VehicleConstants.DriverSeat)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_292", 3000);
                    return;
                }
                if (!target.IsInVehicle || player.Vehicle != target.Vehicle) return;
                VehicleManager.WarpPlayerOutOfVehicle(target);

                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Core_293".Translate( target.GetCharacter().UUID), 3000);
                Notify.Send(target, NotifyType.Warning, NotifyPosition.BottomCenter, $"Core_294".Translate( player.GetCharacter().UUID), 3000);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        //[Command("pocket")]
        //public static void CMD_pocketTarget(PlayerGo player, int id)
        //{
        //    try
        //    {
        //        if (Main.GetPlayerByID(id) == null)
        //        {
        //            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_281", 3000);
        //            return;
        //        }
        //        if (player.Position.DistanceTo(Main.GetPlayerByID(id).Position) > 2)
        //        {
        //            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Biz_52", 3000);
        //            return;
        //        }

        //        Fractions.FractionCommands.playerChangePocket(player, Main.GetPlayerByID(id));
        //    }
        //    catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        //}


        [Command("setrank")]
        public static void CMD_setRank(PlayerGo player, int id, int newrank)
        {
            try
            {
                if (!Manager.CanUseCommand(player, "setrank")) return;
                if (newrank <= 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_202".Translate(), 3000);
                    return;
                }
                var target = Main.GetPlayerByID(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_281", 3000);
                    return;
                }
                FractionCommands.SetFracRank(player, target.GetCharacter().UUID, newrank);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("arrest")]
        public static void CMD_arrest(PlayerGo player, int id)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_281", 3000);
                    return;
                }
                FractionCommands.arrestTarget(player, Main.GetPlayerByID(id));
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("rfp")]
        public static void CMD_rfp(PlayerGo player, int id)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_281", 3000);
                    return;
                }
                FractionCommands.releasePlayerFromPrison(player, Main.GetPlayerByID(id));
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("unfollow")]
        public static void CMD_unfollow(PlayerGo player)
        {
            try
            {
                FractionCommands.targetUnFollowPlayer(player);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("c")]
        public static void CMD_getCoords(PlayerGo player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "a")) return;
                Chat.SendTo(player, $"Position: {NAPI.Entity.GetEntityPosition(player)}");
                Chat.SendTo(player, $"Rotation: {NAPI.Entity.GetEntityRotation(player)}");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("pull")]
        public static void CMD_pullOut(PlayerGo player, int id)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_281", 3000);
                    return;
                }
                FractionCommands.playerOutCar(player, Main.GetPlayerByID(id));
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("accept")]
        public static void CMD_accept(PlayerGo player, int id)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_281", 3000);
                    return;
                }
                Fractions.FractionCommands.acceptEMScall(player, Main.GetPlayerByID(id));
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }


        [Command("capture")]
        public static void CMD_capture(PlayerGo player)
        {
            try
            {
                Fractions.GangsCapture.CMD_startCapture(player);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [Command("carpass")]
        public static void CMD_CarPass(PlayerGo player)
        {
            try
            {
                var vehicle = player.Vehicle;
                if (vehicle == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_172", 3000);
                    return;
                };
                VehicleManager.ViewVehicleTechnicalCertificate(player, vehicle);
                Chat.Action(player, "veh:carpass:check");
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"CMD\":\n" + e.ToString()); }
        }

        [RemoteEvent("tpmark")]
        [Command("tpmark")]
        public static void CMD_tpmark(PlayerGo player){
            try
            {
                if (!Group.CanUseAdminCommand(player, "tpmark")) return;
                Trigger.ClientEvent(player, "GetMyWaypoint");
            }
            catch (Exception e)
            {
                _logger.WriteError($"CMD_tpmark: {e}");
            }
        }

        [Command("changegtype")]
        public static void CMD_changeGarageType(PlayerGo player, int houseID, int newGarageType) {
            try {
                if (!Group.CanUseAdminCommand(player, "changegtype")) 
                    return;                
                if (newGarageType < 0 || newGarageType > 14) {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_128", 3000);
                    return;
                }
                DataTable result = MySQL.QueryRead("SELECT `garage` FROM `houses` WHERE `id` = @prop0", houseID);
                if (result == null || result.Rows.Count == 0) {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_129".Translate( houseID), 3000);
                    return;
                }
                var garageID = Convert.ToInt32(result.Rows[0]["garage"]);
                MySQL.Query("UPDATE `garages` SET `type` = @prop0 WHERE `id` = @prop1", newGarageType, garageID);
                
                var garage =  GarageManager.Garages[garageID];
                garage.Type = newGarageType;
                var house = HouseManager.Houses.Where(x => x.ID == houseID).FirstOrDefault();
                _logger.WriteInfo($"Updated type of garage {garageID}.");
                WhistlerTask.Run(() =>
                {
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "local_130".Translate( houseID), 3000);
                });
            } catch (Exception e)
            {
                _logger.WriteError($"CMD_changeGarageType: {e}");
            }
        }
    }
}