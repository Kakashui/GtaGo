using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using System.Linq;
using Whistler.SDK;
using System.Text.RegularExpressions;
using Whistler.VehicleSystem.Models;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.VehicleSystem.Models.VehiclesData;
using Whistler.VehicleSystem.Models.Configs;
using Whistler.Businesses;
using Whistler.Houses;
using Whistler.Common;
using Whistler.Inventory.Enums;
using Whistler.Entities;
using Whistler.Jobs;
using Whistler.PersonalEvents.Contracts.Models;
using System.IO;
using Whistler.Inventory;
using Whistler.GUI;
using Whistler.GUI.Documents.Enums;
using Whistler.Inventory.Models;

namespace Whistler.VehicleSystem
{
    public class VehicleManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(VehicleManager));
        private static Random Rnd = new Random();

        public static Action<PersonalBaseVehicle> NewVehicleToHolder;
        public static Action<PersonalBaseVehicle> RemoveVehicleFromHolder;

        public static Action<ExtPlayer, ExtVehicle, sbyte> OnPlayerEnterVehicle;
        public static Action<ExtPlayer, ExtVehicle> OnPlayerExitVehicle;

        public static Dictionary<int, VehicleBase> Vehicles = new Dictionary<int, VehicleBase>();

        public static List<int> CarRoomCustom = new List<int>()
        {
            15,
            20,
            21,
            22,
            23,
            24,
            25,
            26,
            29, 
            30,
            31,
            32,
            33,
            34,
            35,
            36,
            37,
            38
        };

        public static Dictionary<uint, string> CustomModelNames = new Dictionary<uint, string>();

        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            try
            {
                foreach (int i in CarRoomCustom)
                    foreach (var product in BusinessesSettings.GetProductSettings(i))
                    {
                        var hash = NAPI.Util.GetHashKey(product.Name.ToLower());
                        if (!CustomModelNames.ContainsKey(hash))
                            CustomModelNames.Add(hash, product.Name.ToLower());
                    }
                Inventory.InventoryService.OnUseOtherItem += ArmyLockPickUse;
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.Message);
            }
        }

        public static void Init()
        {
            try
            {
                Timers.Start("fuel", 60000, FuelControl);
                _logger.WriteInfo("Loading Vehicles...");
                
                DataTable result = MySQL.QueryRead("SELECT * FROM `vehicles` where `isdeleted` = 0");
                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        int owner = Convert.ToInt32(row["typeowner"]);
                        VehicleBase vehModel = CreateVehicle(row, (OwnerType)owner);
                        if (vehModel == null) continue;

                        Vehicles.Add(vehModel.ID, vehModel);
                        if (vehModel.VehCustomization.NeonColors.Count == 0 && vehModel.VehCustomization.NeonColor.Alpha > 0)
                        {
                            vehModel.VehCustomization.NeonColors = new List<Color>() { vehModel.VehCustomization.NeonColor };
                            vehModel.Save();
                        }
                    }
                }
                else
                {
                    _logger.WriteWarning("DB return null result.");
                }

                
                VehicleBase vehicleBase;
                foreach (KeyValuePair<int, VehicleBase> vehicle in Vehicles)
                {
                    vehicleBase = vehicle.Value;
                    if (vehicleBase.OwnerType == OwnerType.Rent/* || vehicleBase.OwnerType == OwnerType.AdminSave*/) continue;

                    if (vehicleBase is PersonalBaseVehicle personalVehicle)
                    {
                        if (personalVehicle.SavePosition == null) continue;

                        GarageManager.SendVehicleIntoGarage(vehicle.Key);
                        continue;

                    }
                    vehicleBase.Spawn();
                }

                if (Directory.Exists("client/vehiclesync"))
                {
                    Dictionary<int, string> handlingList = new Dictionary<int, string>();
                    foreach (var item in Enum.GetValues(typeof(HandlingKeys)))
                    {
                        handlingList.Add((int)item, item.ToString());
                    }
                    using (var w = new StreamWriter("client/vehiclesync/handlingKeys.js"))
                    {
                        w.Write($"module.exports = {JsonConvert.SerializeObject(handlingList)}");
                    }
                }
                //Test();

            }
            catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
        }

        private static void Test()
        {
            Console.WriteLine($"short: {short.MinValue}/{short.MaxValue}");
            Console.WriteLine($"ushort: {ushort.MinValue}/{ushort.MaxValue}");
            Console.WriteLine($"int: {int.MinValue}/{int.MaxValue}");
            Console.WriteLine($"uint: {uint.MinValue}/{uint.MaxValue}");
            Console.WriteLine($"long: {long.MinValue}/{long.MaxValue}");
            Console.WriteLine($"ulong: {ulong.MinValue}/{ulong.MaxValue}");
        }

        public static VehicleBase CreateVehicle(DataRow row, OwnerType type)
        {
            return type switch
            {
                OwnerType.Personal => new PersonalVehicle(row),
                OwnerType.Family => new FamilyVehicle(row),
                OwnerType.Fraction => new FractionVehicle(row),
                OwnerType.Job => new JobVehicle(row),
                OwnerType.Rent => new RentVehicle(row),
                OwnerType.AdminSave => new AdminSaveVehicle(row),
                _ => null,
            };
        }

        private static void FuelControl()
        {
            List<ExtVehicle> allVehicles = Trigger.GetAllVehicles();
            if (allVehicles == null || !allVehicles.Any()) return;

            VehConfig config;
            int oldFuel;
            int newFuel;
            foreach (ExtVehicle vehicle in allVehicles)
            {
                if (vehicle == null) continue;
                if (vehicle.Data == null) continue;
                if (!vehicle.Engine || vehicle.Data.Fuel <= 0) continue;

                try
                {
                    config = vehicle.Config;
                    oldFuel = vehicle.Data.Fuel;
                    newFuel = oldFuel - config.FuelConsumption;
                    if (newFuel <= 0) newFuel = 0;

                    VehicleStreaming.SetVehicleFuel(vehicle, newFuel);
                    if (newFuel == 0) VehicleStreaming.SetEngineState(vehicle, false);
                }
                catch (Exception e)
                {
                    _logger.WriteError($"FUELCONTROL_TIMER: {vehicle.NumberPlate} \n{e.ToString()}");
                }
            }
        }


        private static void ArmyLockPickUse(ExtPlayer player, Inventory.Models.BaseItem item)
        {
            if (item.Name == ItemNames.ArmyLockpick)
            {
                if (player.IsInVehicle && player.VehicleSeat == VehicleConstants.DriverSeat)
                {
                    ExtVehicle vehicle = player.Vehicle as ExtVehicle;
                    if (vehicle.Data.OwnerType == OwnerType.Fraction && vehicle.Data.OwnerID == 14 && vehicle.Data.ModelName.ToLower() == "brickade" && vehicle.Data.Fuel > 0)
                        VehicleStreaming.SetEngineState(vehicle, true);
                }
            }
        }
        [ServerEvent(Event.EntityDeleted)]
        public void Event_EntityDeleted(Entity entity)
        {
            try
            {
                if (entity.Type != EntityType.Vehicle) return;

                ExtVehicle veh = entity as ExtVehicle;
                if (Main.AllVehicles.ContainsKey(veh.Id)) Main.AllVehicles.Remove(veh.Id);
            }
            catch (Exception e) { _logger.WriteError("Event_EntityDeleted: " + e.ToString()); }
        }

        [ServerEvent(Event.PlayerEnterVehicle)]
        public void OnPlayerEnterVehicleHandler(ExtPlayer player, ExtVehicle vehicle, sbyte seatid)
        {
            try
            {
                if (vehicle == null) return;

                OnPlayerEnterVehicle?.Invoke(player, vehicle, seatid);
                if (vehicle.Data == null) return;

                vehicle.Data.PlayerEnterVehicleBase(player, vehicle, seatid);

            }
            catch (Exception e) { _logger.WriteError("PlayerEnterVehicle: " + e.ToString()); }
        }

        [ServerEvent(Event.PlayerExitVehicle)]
        public void OnPlayerExitVehicleHandler(ExtPlayer player, ExtVehicle vehicle)
        {
            try
            {
                if (vehicle == null) return;

                OnPlayerExitVehicle?.Invoke(player, vehicle);
                if (vehicle.Data == null) return;

                vehicle.Data.PlayerExitVehicleBase(player, vehicle);
            }
            catch (Exception e) { _logger.WriteError("PlayerExitVehicle: " + e.ToString()); }
        }

        [ServerEvent(Event.PlayerExitVehicleAttempt)]
        public void onPlayerExitVehicleHandler(ExtPlayer player, ExtVehicle vehicle)
        {
            try
            {
                SafeTrigger.ClientEvent(player, "VehStream_PlayerExitVehicleAttempt", vehicle, vehicle.Engine);

                if (!vehicle.AllOccupants.ContainsValue(player)) return;

                sbyte key = vehicle.AllOccupants.First(x => x.Value == player).Key;
                vehicle.AllOccupants.Remove(key);
            }
            catch (Exception e) { _logger.WriteError("PlayerExitVehicleAttempt: " + e.ToString()); }
        }

        public static void API_onPlayerDisconnected(ExtPlayer player, DisconnectionType type, string reason)
        {
            try
            {
                if (player.IsInVehicle)
                {
                    ExtVehicle vehicle = player.Vehicle as ExtVehicle;
                    if (!vehicle.AllOccupants.ContainsValue(player)) return;

                    sbyte key = vehicle.AllOccupants.First(x => x.Value == player).Key;
                    vehicle.AllOccupants.Remove(key);
                }
            }
            catch (Exception e) { _logger.WriteError("PlayerDisconnected: " + e.ToString()); }
        }

        public static void WarpPlayerOutOfVehicle(ExtPlayer player)
        {
            player.WarpOutOfVehicle();

            ExtVehicle vehicle = player.Vehicle as ExtVehicle;
            if (vehicle == null) return;
            if (!vehicle.AllOccupants.ContainsValue(player)) return;

            sbyte key = vehicle.AllOccupants.First(x => x.Value == player).Key;
            vehicle.AllOccupants.Remove(key);
        }

        public static void RepairCar(ExtVehicle vehicle)
        {
            vehicle.Repair();
        }

        public static PersonalBaseVehicle Create(int holder, string model, Color color1, Color color2, int fuel = 100, int price = 0, PropBuyStatus status = PropBuyStatus.Bought, OwnerType typeOwner = OwnerType.Personal)
        {
            if (typeOwner == OwnerType.Personal)
                return new PersonalVehicle(holder, model, color1, color2, fuel, price, status);
            else
                return new FamilyVehicle(holder, model, color1, color2, fuel, price, status);
        }

        public static void Remove(int idkey)
        {

            if (Vehicles.ContainsKey(idkey))
            {
                Vehicles[idkey].DeleteVehicle();
            }
        }
        public static void SaveFamilyCars()
        {
            if (!Vehicles.Any()) return;

            List<KeyValuePair<int, VehicleBase>> listFamilyVehs = Vehicles.Where(item => item.Value.OwnerType == OwnerType.Family).ToList();
            if (!listFamilyVehs.Any()) return;

            foreach (KeyValuePair<int, VehicleBase> veh in listFamilyVehs)
            {
                if (veh.Value == null) continue;

                veh.Value.Save();
            }
        }
        public static ExtVehicle getNearestVehicle(ExtPlayer player, int radius)
        {
            List<ExtVehicle> all_vehicles = Trigger.GetAllVehicles();
            ExtVehicle nearest_vehicle = null;
            foreach (ExtVehicle v in all_vehicles)
            {
                if (v.Dimension != player.Dimension) continue;
                if (nearest_vehicle == null && player.Position.DistanceTo(v.Position) < radius)
                {
                    nearest_vehicle = v;
                    continue;
                }
                else if (nearest_vehicle != null)
                {
                    if (player.Position.DistanceTo(v.Position) < player.Position.DistanceTo(nearest_vehicle.Position))
                    {
                        nearest_vehicle = v;
                        continue;
                    }
                }
            }
            return nearest_vehicle;
        }

        public static List<int> getAllHolderVehicles(int holder, OwnerType type)
        {
            return Vehicles.Where(item => item.Value.OwnerID == holder && item.Value.OwnerType == type).Select(item => item.Key).ToList();
        }
        public static List<VehicleBase> GetAllHolderVehicles(int holder, OwnerType type)
        {
            return Vehicles.Values.Where(item => item.OwnerID == holder && item.OwnerType == type).ToList();
        }

        public static VehicleBase GetVehicleBaseByUUID(int id)
        {
            return Vehicles.GetValueOrDefault(id);
        }


        public static void ApplyCustomization(ExtVehicle vehicle)
        {
            try
            {
                if (vehicle == null) return;

                VehicleCustomization.ApplyVehCustomization(vehicle);
            }
            catch (Exception e) { _logger.WriteError("ApplyCustomization: " + e.ToString()); }
        }
        public static void ApplyHandlingVehCustomization(ExtVehicle vehicle)
        {
            try
            {
                if (vehicle == null) return;

                VehicleCustomization.ApplyHandlingVehCustomization(vehicle);
            }
            catch (Exception e) { _logger.WriteError("ApplyCustomization: " + e.ToString()); }
        }


        public static bool ChangeNumber(int idkey, string newNumber, bool ignoreRepeat = false)
        {
            if (!Vehicles.ContainsKey(idkey))
                return false;
            newNumber = newNumber.ToUpper();
            string format = @"[0-9A-Z]{1,8}";
            Regex regex = new Regex(format);
            MatchCollection matches = regex.Matches(newNumber);
            if (matches.Count != 1)
                return false;
            else if (matches[0].Value != newNumber)
                return false;
            if (!ignoreRepeat && Vehicles.FirstOrDefault(item => item.Value.Number == newNumber).Value != null)
                return false;

            Vehicles[idkey].Number = newNumber;
            var vehicle = Main.AllVehicles.FirstOrDefault(item => item.Value.Data.ID == idkey).Value;
            if (vehicle != null)
                vehicle.NumberPlate = newNumber;
            Vehicles[idkey].Save();

            return true;
        }

        /// <summary>
        /// Генерация рандомного номера для авто, где <paramref name="noRepeat"/> - без повторений с другими авто
        /// </summary>
        /// <param name="noRepeat"></param>
        /// <returns></returns>
        public static string GenerateNumber(bool noRepeat = true)
        {
            string number;
            do
            {
                number = "";
                for (int i = 0; i < 6; i++)
                    number += Rnd.Next(0, 2) == 0 ? (char)Rnd.Next(0x0030, 0x003A) : (char)Rnd.Next(0x0041, 0x005B);
            } while (noRepeat && VehicleManager.Vehicles.FirstOrDefault(item => item.Value.Number == number).Value != null);
            return number;
        }
        public static string GetModelName(uint hash)
        {
            if (Enum.IsDefined(typeof(VehicleHash), hash))
                return Enum.GetName(typeof(VehicleHash), hash).ToLower();
            else if (VehicleModels.VehicleModelNames.ContainsKey(hash))
                return VehicleModels.VehicleModelNames[hash].ToLower();
            else if (VehicleConfigs.VehicleConfigList.ContainsKey(hash))
                return VehicleConfigs.VehicleConfigList[hash].ModelName.ToLower();
            else if (CustomModelNames.ContainsKey(hash))
                return CustomModelNames[hash].ToLower();
            return null;
        }

        public static string GetModelName(string model)
        {
            var vh = NAPI.Util.GetHashKey(model);
            return GetModelName(vh);
        }

        public static void changeOwner(int uuid, string newName)
        {
            foreach (var veh in Main.AllVehicles.Where(item => item.Value.Data.OwnerID == uuid && item.Value.Data.OwnerType == OwnerType.Personal))
            {
                veh.Value.SetSharedData("HOLDERNAME", newName);
            }
        }

        public static void LockCarPressed(ExtPlayer sender)
        {
            ExtVehicle vehicle = NAPI.Player.IsPlayerInAnyVehicle(sender) && sender.VehicleSeat == VehicleConstants.DriverSeat ? sender.Vehicle as ExtVehicle : getNearestVehicle(sender, 10);
            if (vehicle == null) return;

            ChangeVehicleDoors(sender, vehicle);
        }

        public static void ChangeVehicleDoors(ExtPlayer player, ExtVehicle vehicle)
        {
            if (vehicle == null) return;
            if (vehicle.Data == null) return;

            if (vehicle.Data.CanAccessVehicle(player, AccessType.LockedDoor))
            {
                VehicleStreaming.SetLockStatus(vehicle, !VehicleStreaming.GetLockState(vehicle));
                return;

            }
            
            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У Вас нет ключей от этого транспорта", 3000);

        }

        public static void ChangeVehicleDoorOpen(ExtPlayer player, ExtVehicle vehicle, DoorID indexDoor)
        {
            if (VehicleStreaming.GetDoorState(vehicle, indexDoor) == DoorState.DoorOpen)
            {
                VehicleStreaming.SetDoorState(vehicle, indexDoor, DoorState.DoorClosed);
                if (indexDoor == DoorID.DoorTrunk)
                {
                    Chat.Action(player, "Core1_7");
                }
            }
            else
            {
                if (VehicleStreaming.GetLockState(vehicle) && indexDoor != DoorID.DoorTrunk)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_5", 3000);
                    return;
                }
                if (vehicle.Data.CanAccessVehicle(player, indexDoor == DoorID.DoorTrunk ? AccessType.OpenTrunk : AccessType.OpenDoor))
                {
                    VehicleStreaming.SetDoorState(vehicle, indexDoor, DoorState.DoorOpen);
                    if (indexDoor == DoorID.DoorTrunk)
                        Chat.Action(player, "Core1_10");
                }
                else
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_70".Translate(), 3000);
            }
        }

        public static void EngineCarPressed(ExtPlayer player)
        {
            if (!NAPI.Player.IsPlayerInAnyVehicle(player)) return;
            if (player.VehicleSeat != VehicleConstants.DriverSeat)
            {
                //Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_68".Translate(), 3000);
                return;
            }
            ExtVehicle vehicle = player.Vehicle as ExtVehicle;
            if (vehicle.Data.Fuel <= 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_69".Translate(), 3000);
                return;
            }
            if (vehicle.Data.CanAccessVehicle(player, AccessType.EngineChange))
            {
                if (vehicle.RequiredLicense != null && !player.CheckLic((LicenseName)vehicle.RequiredLicense)) 
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У Вас нет лицензии нужной категории", 4000);
                    return;
                }
                VehicleStreaming.SetEngineState(vehicle, !VehicleStreaming.GetEngineState(vehicle));
            }
            else Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_70".Translate(), 3000);            
        }

        public static string GetModelAndNumber(int key)
        {
            if (Vehicles.ContainsKey(key))
                return $"{Vehicles[key].ModelName}({Vehicles[key].Number})";
            else
                return "Unknown";
        }

        public static ExtVehicle CreateTemporaryVehicle(string model, Vector3 pos, float rot, string number, VehicleAccess access, ExtPlayer player = null, uint dimension = 0)
        {
            try
            {
                ExtVehicle vehicle = NAPI.Vehicle.CreateVehicle((VehicleHash)NAPI.Util.GetHashKey(model), pos, rot, 0, 0, dimension: dimension) as ExtVehicle;
                if (vehicle == null) return null;

                return CreateTemporaryData(vehicle, number, access, player);
            }
            catch (Exception e)
            {
                _logger.WriteDebug($"veh: {(VehicleHash)NAPI.Util.GetHashKey(model)} number: {number} dim: {dimension}");
                throw e;
            }

        }

        public static ExtVehicle CreateTemporaryVehicle(string model, Vector3 pos, Vector3 rot, string number, VehicleAccess access, ExtPlayer player = null, uint dimension = 0)
        {
            try
            {
                ExtVehicle vehicle = NAPI.Vehicle.CreateVehicle((VehicleHash)NAPI.Util.GetHashKey(model), pos, rot.Z, 0, 0, dimension: dimension) as ExtVehicle;
                if (vehicle == null) return null;

                return CreateTemporaryData(vehicle, number, access, player);
            }
            catch (Exception e)
            {
                _logger.WriteDebug($"veh: {(VehicleHash)NAPI.Util.GetHashKey(model)} number: {number} dim: {dimension}");
                throw e;
            }
            
        }

        public static ExtVehicle CreateTemporaryVehicle(VehicleHash model, Vector3 pos, Vector3 rot, string number, VehicleAccess access, ExtPlayer player = null, uint dimension = 0)
        {
            ExtVehicle vehicle = NAPI.Vehicle.CreateVehicle(model, pos, rot.Z, 0, 0, dimension: dimension) as ExtVehicle;
            if (vehicle == null) return null;

            return CreateTemporaryData(vehicle, number, access, player);
        }

        public static ExtVehicle CreateTemporaryVehicle(uint model, Vector3 pos, Vector3 rot, string number, VehicleAccess access, ExtPlayer player = null, uint dimension = 0)
        {
            ExtVehicle vehicle = NAPI.Vehicle.CreateVehicle(model, pos, rot.Z, 0, 0, dimension: dimension) as ExtVehicle;
            if (vehicle == null) return null;

            return CreateTemporaryData(vehicle, number, access, player);
        }

        public static ExtVehicle CreateTemporaryVehicle(int model, Vector3 pos, Vector3 rot, string number, VehicleAccess access, ExtPlayer player = null, uint dimension = 0)
        {
            ExtVehicle vehicle = NAPI.Vehicle.CreateVehicle(model, pos, rot.Z, 0, 0, dimension: dimension) as ExtVehicle;
            if (vehicle == null) return null;

            return CreateTemporaryData(vehicle, number, access, player);
        }

        private static ExtVehicle CreateTemporaryData(ExtVehicle vehicle, string number, VehicleAccess access, ExtPlayer player)
        {
            TemporaryVehicle vehData = new TemporaryVehicle(vehicle.Model, number, access, player);
            vehicle.Initialize(vehData);
            ushort vehicleId = vehicle.Id;
            vehicle.Session.Id = vehicleId;
            if (access != VehicleAccess.School && access != VehicleAccess.RoyalBattle) vehicle.InitializeLicense(vehicle.Class);
            if (Main.AllVehicles.ContainsKey(vehicleId)) Main.AllVehicles[vehicleId] = vehicle;
            else Main.AllVehicles.Add(vehicleId, vehicle);
            vehicle.NumberPlate = number;
            if (access != VehicleAccess.ShowRoom && access != VehicleAccess.RoyalBattle) VehicleStreaming.SetVehicleFuel(vehicle, vehicle.Data.Config.MaxFuel);
            return vehicle;
        }

        [ServerEvent(Event.EntityCreated)]
        public void Event_entityCreated(Entity entity)
        {
            try
            {
                if (entity.Type != EntityType.Vehicle) return;
                ExtVehicle vehicle = NAPI.Entity.GetEntityFromHandle<ExtVehicle>(entity);

                string[] keys = NAPI.Data.GetAllEntityData(vehicle);
                foreach (string key in keys) vehicle.ResetData(key);
                int fuel = VehicleConfiguration.GetConfig(vehicle.Model).MaxFuel;
                int fueltype = VehicleConfiguration.GetConfig(vehicle.Model).fuelType;
                // Chat.AdminToAll($"{fueltype}");
                SafeTrigger.SetSharedData(vehicle, "PETROL", fuel);
                SafeTrigger.SetSharedData(vehicle, "MAXPETROL", fuel);
                SafeTrigger.SetSharedData(vehicle, "TYPEFUEL", fueltype-1);
                SafeTrigger.SetSharedData(vehicle, "hlcolor", 0);
                SafeTrigger.SetSharedData(vehicle, "vehradio", 255);
                //SafeTrigger.SetSharedData(vehicle, "vehmodel", vehicle.Model);
            }
            catch (Exception e) { _logger.WriteError("EntityCreated: " + e.ToString()); }
        }


        [ServerEvent(Event.VehicleDeath)]
        public void Event_vehicleDeath(ExtVehicle vehicle)
        {
            try
            {
                if (Main.AllVehicles.ContainsKey(vehicle.Id))
                {
                    vehicle.Data.VehicleDeath(vehicle);
                }
            }
            catch (Exception e) { _logger.WriteError("VehicleDeath: " + e.ToString()); }
        }


        public static void UseRepairKit(ExtPlayer player, ExtVehicle vehicle)
        {
            var item = player.GetInventory().SubItemByName(ItemNames.LowRepairKit, 1, LogAction.Use);
            if (item == null)
            {
                Notify.SendError(player, "veh:repair:kitNotFound");
                return;
            }
            RepairBody(vehicle);
        }

        public static int GetVehicleMaxPassengers(ExtVehicle vehicle)
        {
            uint hash = NAPI.Util.GetHashKey(vehicle.GetModelName());
            int passengerSize = 3;
            if (Enum.IsDefined(typeof(VehicleHash), hash)) passengerSize = NAPI.Vehicle.GetVehicleMaxOccupants((VehicleHash)hash);
            return passengerSize;
        }

        public static bool VehicleCheckPassenger(ExtVehicle vehicle, int slot)
        {
            //int passengerSize = NAPI.Vehicle.GetVehicleMaxOccupants((VehicleHash)NAPI.Util.GetHashKey(vehicle.GetModelName()));
            return true;
        }


        public static void EjectPassengers(ExtPlayer player, ExtVehicle vehicle, sbyte slot)
        {
            if (!player.IsInVehicle || player.VehicleSeat != VehicleConstants.DriverSeat)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы не в машине или не на водительском месте", 3000);
                return;
            }
            if (vehicle == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Ошибка определения машины", 3000);
                return;
            }

            if (!vehicle.AllOccupants.ContainsKey(slot))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"На этом сидении нет игрока.", 3000);
                return;
            }

            ExtPlayer target = vehicle.AllOccupants[slot];
            if (target == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Ошибка нахождения игрока на данном сидении.", 3000);
                return;
            }

            if (!target.IsInVehicle || player.Vehicle != target.Vehicle) return;
            WarpPlayerOutOfVehicle(target);

            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы выкинули {target.Character.UUID} из машины", 3000);
            Notify.Send(target, NotifyType.Warning, NotifyPosition.BottomCenter, $"Игрок {player.Character.UUID} выкинул Вас из машины", 3000);
        }

        public static void GetPassengers(ExtPlayer player, int slot)
        {
            // if (!player.IsInVehicle || player.VehicleSeat != VehicleConstants.DriverSeat)
            // {
            //     Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы не в машине или не на водительском месте", 3000);
            //     return;
            // }

            // Player ejectPlayer = vehicle.AllOccupants[slot];
            // if (!ejectPlayer.IsInVehicle || player.Vehicle != ejectPlayer.Vehicle) return;
            // VehicleManager.WarpPlayerOutOfVehicle(ejectPlayer);

            // Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы выкинули {ejectPlayer.Character.UUID} из машины", 3000);
            // Notify.Send(ejectPlayer, NotifyType.Warning, NotifyPosition.BottomCenter, $"Игрок {player.Character.UUID} выкинул Вас из машины", 3000);
        }
            
        // public static void GetVehicleMaxPassengers(ExtPlayer player, ExtVehicle vehicle)
        // {
        //     if (!player.IsInVehicle || player.VehicleSeat != VehicleConstants.DriverSeat)
        //     {
        //         Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы не в машине или не на водительском месте", 3000);
        //         return;
        //     }

        //     // if (!target.IsInVehicle || player.Vehicle != target.Vehicle) return;
        //     // VehicleManager.WarpPlayerOutOfVehicle(target);

        //     // Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы выкинули {target.Character.UUID} из машины", 3000);
        //     // Notify.Send(target, NotifyType.Warning, NotifyPosition.BottomCenter, $"Игрок {player.Character.UUID} выкинул Вас из машины", 3000);
        // }   


        public static void ViewVehicleTechnicalCertificate(ExtPlayer player, ExtVehicle vehicle)
        {
            if (!vehicle.IsWearable())
                return;
            var state = new CarPassDTO(vehicle.Data as PersonalBaseVehicle);
            SafeTrigger.ClientEvent(player, "veh:technicalCertificate", JsonConvert.SerializeObject(state));
        }

        public static void SetContainerToVehicle(ExtPlayer player, ExtVehicle vehicle)
        {
            object containerItem = player.GetLincContainerFromPlayer();
            if (containerItem == null || !(containerItem is VehicleItemBase container))
            {
                Notify.SendError(player, "У Вас в руках контейнера, который не подходит для транспортного средства.");
                return;
            }
            if (vehicle.Data == null)
            {
                Notify.SendError(player, "Ошибка получения данных транспорта.");
                return;
            }
            int res = vehicle.Data.GiveAbstractItem(container);
            if (res > 0)
            {
                player.TakeContainerFromPlayer();
                Notify.SendSuccess(player, $"Вы погрузили {res} предмет в транспортное средство");
            }
            else Notify.SendError(player, $"В транспорте недостаточно места для погрузки");
        }

        public static void CopyCustomization(ExtVehicle vehicle, ExtVehicle copy, bool handlingCopy)
        {
            vehicle.Data.VehCustomization = new CustomizationVehicleModel(copy.Data.VehCustomization, handlingCopy);
            ApplyCustomization(vehicle);
            vehicle.Data.Save();
        }

        public static void CopyHandling(ExtVehicle vehicle, ExtVehicle copy)
        {
            var vehData = vehicle.Data;
            vehData.VehCustomization.HandlingTuning = new Dictionary<HandlingKeys, object>();
            foreach (var comp in copy.Data.VehCustomization.HandlingTuning)
            {
                vehData.VehCustomization.HandlingTuning.Add(comp.Key, comp.Value);
            }
            ApplyHandlingVehCustomization(vehicle);
            vehicle.Data.Save();
        }

        public static void ClearHandling(ExtVehicle vehicle)
        {
            var vehData = vehicle.Data;
            vehData.VehCustomization.HandlingTuning = new Dictionary<HandlingKeys, object>();
            ApplyHandlingVehCustomization(vehicle);
            vehicle.Data.Save();
        }

        public static void ApplyVehicleState(ExtVehicle vehicle)
        {
            ExtVehicle extVehicle = vehicle;
            SetEngineHealth(vehicle, (extVehicle.Data as PersonalBaseVehicle).EngineHealth);
            SetDoorBreak(vehicle, -1);
            SetBrakeBroken(vehicle);
            SetTransmissionCoef(vehicle);
        }

        #region Поломки авто
        public static void SetEngineHealth(ExtVehicle vehicle, float health)
        {
            if (!vehicle.IsWearable())
                return;
            if (health < 0)
                health = 0;
            (vehicle.Data as PersonalBaseVehicle).EngineHealth = health;
            SafeTrigger.SetSharedData(vehicle, "veh:engineHealth", health);
        }
        public static void SetDoorBreak(ExtVehicle vehicle, int state)
        {
            if (!vehicle.IsWearable())
                return;

            vehicle.Data.DoorBreak = state;
            SafeTrigger.SetSharedData(vehicle, "veh:doorBreak", vehicle.Data.DoorBreak);
            //if ((vehicle.Data.DoorBreak & VehicleConstants.CheckBrokenDoor) > 0) VehicleStreaming.SetLockStatus(vehicle, false);
        }

        public static void SetBrakeBroken(ExtVehicle vehicle)
        {
            if (!vehicle.IsWearable())
                return;
            bool state = (vehicle.Data as PersonalBaseVehicle).Mileage - (vehicle.Data as PersonalBaseVehicle).MileageBrakePadsChange >= VehicleConstants.MileageBrakeBroken;
            if (!vehicle.HasSharedData("veh:BrakesBroke") || state != vehicle.GetSharedData<bool>("veh:BrakesBroke"))
                SafeTrigger.SetSharedData(vehicle, "veh:BrakesBroke", (vehicle.Data as PersonalBaseVehicle).Mileage - (vehicle.Data as PersonalBaseVehicle).MileageBrakePadsChange >= VehicleConstants.MileageBrakeBroken);
        }

        public static void SetTransmissionCoef(ExtVehicle vehicle)
        {
            if (!vehicle.IsWearable())
                return;
            float coef = VehicleConstants.GetTransmissCoef((vehicle.Data as PersonalBaseVehicle).Mileage - (vehicle.Data as PersonalBaseVehicle).MileageTransmissionService);
            if (!vehicle.HasSharedData("veh:coefTransm") || coef != vehicle.GetSharedData<float>("veh:coefTransm"))
                SafeTrigger.SetSharedData(vehicle, "veh:coefTransm", coef);

        }
        #endregion

        #region Ремонт и ТО
        public static void RepairVehicle(ExtVehicle vehicle)
        {
            if (!vehicle.IsWearable())
                return;
            RepairBody(vehicle);
            RepairEngine(vehicle);
            BrakeService(vehicle);
            TransmissionService(vehicle);
            EngineService(vehicle);
        }
        public static void RepairBody(ExtVehicle vehicle)
        {
            vehicle.Repair();
            SetDoorBreak(vehicle, 0);
        }

        /// <summary>
        /// Ремонт двигателя
        /// </summary>
        /// <param name="vehicle"></param>
        public static void RepairEngine(ExtVehicle vehicle)
        {
            SetEngineHealth(vehicle, 1000F);
        }

        /// <summary>
        /// Обслуживание тормозов
        /// </summary>
        /// <param name="vehicle"></param>
        public static void BrakeService(ExtVehicle vehicle)
        {
            if (!vehicle.IsWearable())
                return;
            (vehicle.Data as PersonalBaseVehicle).MileageBrakePadsChange = (vehicle.Data as PersonalBaseVehicle).Mileage;
            SetBrakeBroken(vehicle);
        }

        /// <summary>
        /// Обслуживание трансмиссии
        /// </summary>
        /// <param name="vehicle"></param>
        public static void TransmissionService(ExtVehicle vehicle)
        {
            if (!vehicle.IsWearable())
                return;
            (vehicle.Data as PersonalBaseVehicle).MileageTransmissionService = (vehicle.Data as PersonalBaseVehicle).Mileage;
            SetTransmissionCoef(vehicle);
        }

        /// <summary>
        /// Обслуживание двигателя
        /// </summary>
        /// <param name="vehicle"></param>
        public static void EngineService(ExtVehicle vehicle)
        {
            if (!vehicle.IsWearable()) return;
            (vehicle.Data as PersonalBaseVehicle).MileageOilChange = (vehicle.Data as PersonalBaseVehicle).Mileage;
        }

        #endregion

        public static void EngineDescHealth(ExtVehicle vehicle, bool typeBreak)
        {
            ExtVehicle extVehicle = vehicle;
            if (!extVehicle.IsWearable())
                return;
            if (typeBreak)
                SetEngineHealth(vehicle, (extVehicle.Data as PersonalBaseVehicle).EngineHealth - VehicleConstants.GetMileageEngineBroke((extVehicle.Data as PersonalBaseVehicle).Mileage - (extVehicle.Data as PersonalBaseVehicle).MileageOilChange));
            else
                SetEngineHealth(vehicle, (extVehicle.Data as PersonalBaseVehicle).EngineHealth - VehicleConstants.EngineBrokenHealth);
        }

        /// <summary>
        /// Увеличение пробега автомобиля
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="value"></param>
        public static void AddMileage(ExtVehicle vehicle, float value)
        {
            ExtVehicle extVehicle = vehicle;
            if (!extVehicle.IsWearable())
                return;
            (extVehicle.Data as PersonalBaseVehicle).Mileage += value;
            if (VehicleConstants.CheckUpdateEngineState((extVehicle.Data as PersonalBaseVehicle).Mileage, value))
            {
                EngineDescHealth(vehicle, true);
                if ((extVehicle.Data as PersonalBaseVehicle).Mileage - (extVehicle.Data as PersonalBaseVehicle).MileageTransmissionService >= VehicleConstants.MileageTransmissionService)
                    SetTransmissionCoef(vehicle);
            }
            if ((extVehicle.Data as PersonalBaseVehicle).Mileage - (extVehicle.Data as PersonalBaseVehicle).MileageBrakePadsChange >= VehicleConstants.MileageBrakeBroken)
                SetBrakeBroken(vehicle);
        }

        [RemoteEvent("veh:doorBroken")]
        public static void RemoteEvent_EventSetDoorState(ExtPlayer player, int indexDoor)
        {
            if (player.IsInVehicle && player.VehicleSeat == VehicleConstants.DriverSeat)
            {
                ExtVehicle vehicle = player.Vehicle as ExtVehicle;
                if (!vehicle.IsWearable()) return;
                if ((vehicle.Data.DoorBreak >> indexDoor) % 2 == 0)
                {
                    int mask = 1 << indexDoor;
                    int state = vehicle.Data.DoorBreak ^ mask;
                    SetDoorBreak(vehicle, state);
                }
            }
        }
        /// <summary>
        /// Поломка двигателя
        /// </summary>
        /// <param name="player"></param>
        [RemoteEvent("veh:engBroken")]
        public static void RemoteEvent_EventEngineBroken(ExtPlayer player)
        {
            if (player.IsInVehicle && player.VehicleSeat == VehicleConstants.DriverSeat)
            {
                ExtVehicle vehicle = player.Vehicle as ExtVehicle;
                EngineDescHealth(vehicle, false);
            }
        }

        /// <summary>
        /// Увеличение пройденного пути
        /// </summary>
        /// <param name="player"></param>
        /// <param name="vehicle"></param>
        /// <param name="value"></param>
        [RemoteEvent("veh:addDistance")]
        public static void RemoteEvent_DistanceTraveled(ExtPlayer player, ExtVehicle vehicle, float value)
        {
            try
            {
                if (vehicle != null)
                    AddMileage(vehicle, value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
