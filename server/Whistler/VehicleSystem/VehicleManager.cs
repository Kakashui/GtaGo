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
using Whistler.PersonalEvents.Contracts.Models;
using System.IO;
using Whistler.Inventory;

namespace Whistler.VehicleSystem
{
    public class VehicleManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(VehicleManager));
        private static Random Rnd = new Random();

        public static Action<PersonalBaseVehicle> NewVehicleToHolder;
        public static Action<PersonalBaseVehicle> RemoveVehicleFromHolder;

        public static Action<Player, Vehicle, sbyte> OnPlayerEnterVehicle;
        public static Action<Player, Vehicle> OnPlayerExitVehicle;

        public static Dictionary<int, VehicleBase> Vehicles = new Dictionary<int, VehicleBase>();

        public static Dictionary<Vehicle, VehicleGo> GoVehicles = new Dictionary<Vehicle, VehicleGo>();

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
                Timers.Start("fuel", 30000, FuelControl);
                _logger.WriteInfo("Loading Vehicles...");
                
                DataTable result = MySQL.QueryRead("SELECT * FROM `vehicles` where `isdeleted` = 0");
                if (result != null && result.Rows.Count > 0)
                {

                    foreach (DataRow row in result.Rows)
                    {
                        int owner = Convert.ToInt32(row["typeowner"]);
                        VehicleBase vehModel = CreateVehicle(row, (OwnerType)owner);
                        if (vehModel != null)
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
                foreach (var vehicle in Vehicles)
                {
                    if (vehicle.Value.OwnerType == OwnerType.Rent/* || vehicle.Value.OwnerType == OwnerType.AdminSave*/)
                        continue;
                    if (!(vehicle.Value is PersonalBaseVehicle))
                        vehicle.Value.Spawn();
                    else if ((vehicle.Value as PersonalBaseVehicle).SavePosition != null)
                        GarageManager.SendVehicleIntoGarage(vehicle.Key);
                }


                if (Directory.Exists("Client"))
                {

                    Dictionary<int, string> handlingList = new Dictionary<int, string>();
                    foreach (var item in Enum.GetValues(typeof(HandlingKeys)))
                    {
                        handlingList.Add((int)item, item.ToString());
                    }
                    using (var w = new StreamWriter("Client/src/client/vehiclesync/handlingKeys.js"))
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
            switch (type)
            {
                case OwnerType.Personal:
                    return new PersonalVehicle(row);
                case OwnerType.Family:
                    return new FamilyVehicle(row);
                case OwnerType.Fraction:
                    return new FractionVehicle(row);
                case OwnerType.Job:
                    return new JobVehicle(row);
                case OwnerType.Rent:
                    return new RentVehicle(row);
                case OwnerType.AdminSave:
                    return new AdminSaveVehicle(row);
            }
            return null;
        }

        private static void FuelControl()
        {
            List<Vehicle> allVehicles = NAPI.Pools.GetAllVehicles();
            if (allVehicles.Count == 0) return;
            foreach (Vehicle veh in allVehicles)
            {
                try
                {
                    VehicleGo vehGo = veh.GetVehicleGo();
                    if (!vehGo.Engine || vehGo.Data.Fuel == 0)
                        continue;

                    var config = vehGo.Config;

                    if (vehGo.Data.Fuel - config.FuelConsumption <= 0)
                    {
                        vehGo.Data.Fuel = 0;
                        VehicleStreaming.SetEngineState(veh, false);
                    }
                    else vehGo.Data.Fuel -= config.FuelConsumption;
                    veh.SetSharedData("PETROL", vehGo.Data.Fuel);
                }
                catch (Exception e)
                {
                    _logger.WriteError($"FUELCONTROL_TIMER: {veh.NumberPlate} \n{e.ToString()}");
                }
            }
        }


        private static void ArmyLockPickUse(Player player, Inventory.Models.BaseItem item)
        {
            if (item.Name == ItemNames.ArmyLockpick)
            {
                if (player.IsInVehicle && player.VehicleSeat == VehicleConstants.DriverSeat)
                {
                    var vehicle = player.Vehicle.GetVehicleGo();
                    if (vehicle.Data.OwnerType == OwnerType.Fraction && vehicle.Data.OwnerID == 14 && vehicle.Data.ModelName.ToLower() == "brickade" && vehicle.Data.Fuel > 0)
                        VehicleStreaming.SetEngineState(player.Vehicle, true);
                }
            }
        }
        [ServerEvent(Event.EntityDeleted)]
        public void Event_EntityDeleted(Entity entity)
        {
            try
            {
                var veh = entity as Vehicle;
                if (entity.Type == EntityType.Vehicle && GoVehicles.ContainsKey(/*NAPI.Entity.GetEntityFromHandle<Vehicle>(entity.Handle)*/(Vehicle)entity))
                    GoVehicles.Remove(NAPI.Entity.GetEntityFromHandle<Vehicle>(entity));
            }
            catch (Exception e) { _logger.WriteError("Event_EntityDeleted: " + e.ToString()); }
        }

        [ServerEvent(Event.PlayerEnterVehicle)]
        public void OnPlayerEnterVehicleHandler(PlayerGo player, Vehicle vehicle, sbyte seatid)
        {
            try
            {
                OnPlayerEnterVehicle?.Invoke(player, vehicle, seatid);
                vehicle.GetVehicleGo().Data.PlayerEnterVehicleBase(player, vehicle, seatid);

            }
            catch (Exception e) { _logger.WriteError("PlayerEnterVehicle: " + e.ToString()); }
        }

        [ServerEvent(Event.PlayerExitVehicle)]
        public void OnPlayerExitVehicleHandler(PlayerGo player, Vehicle vehicle)
        {
            try
            {
                if (vehicle != null)
                {
                    OnPlayerExitVehicle?.Invoke(player, vehicle);
                    vehicle.GetVehicleGo().Data.PlayerExitVehicleBase(player, vehicle);
                }
            }
            catch (Exception e) { _logger.WriteError("PlayerExitVehicle: " + e.ToString()); }
        }

        [ServerEvent(Event.PlayerExitVehicleAttempt)]
        public void onPlayerExitVehicleHandler(Player player, Vehicle vehicle)
        {
            try
            {
                Trigger.ClientEvent(player, "VehStream_PlayerExitVehicleAttempt", vehicle, vehicle.GetVehicleGo().Engine);
                VehicleGo vehGo = vehicle.GetVehicleGo();
                if (vehGo.Occupants.Contains(player))
                    vehGo.Occupants.Remove(player);
            }
            catch (Exception e) { _logger.WriteError("PlayerExitVehicleAttempt: " + e.ToString()); }
        }

        public static void API_onPlayerDisconnected(Player player, DisconnectionType type, string reason)
        {
            try
            {
                if (player.IsInVehicle)
                {
                    VehicleGo vehGo = player.Vehicle.GetVehicleGo();
                    if (vehGo.Occupants.Contains(player))
                        vehGo.Occupants.Remove(player);
                }
            }
            catch (Exception e) { _logger.WriteError("PlayerDisconnected: " + e.ToString()); }
        }

        public static void WarpPlayerOutOfVehicle(Player player)
        {
            Vehicle vehicle = player.Vehicle;
            if (vehicle == null) return;

            VehicleGo vehGo = player.Vehicle.GetVehicleGo();
            if (vehGo.Occupants.Contains(player))
                vehGo.Occupants.Remove(player);
            player.WarpOutOfVehicle();
        }

        public static void RepairCar(Vehicle vehicle)
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
            try
            {
                foreach (var veh in Vehicles.Where(item => item.Value.OwnerType == OwnerType.Family))
                {
                    veh.Value.Save();
                }
            }
            catch (Exception e) { _logger.WriteError("SaveFamilyCars: " + e.ToString()); }
        }
        public static Vehicle getNearestVehicle(Player player, int radius)
        {
            List<Vehicle> all_vehicles = NAPI.Pools.GetAllVehicles();
            Vehicle nearest_vehicle = null;
            foreach (Vehicle v in all_vehicles)
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

        public static VehicleGo GetVehicleGo(int idkey)
        {
            return GoVehicles.FirstOrDefault(item => item.Value.Data.ID == idkey).Value;
        }

        public static Vehicle GetVehicleByRemoteId(int id)
        {
            return NAPI.Pools.GetAllVehicles().FirstOrDefault(item => item.Value == id);
        }

        public static Vehicle GetVehicleByUUID(int id)
        {
            return GoVehicles.FirstOrDefault(item => item.Value.Data.ID == id).Key;
        }

        public static VehicleBase GetVehicleBaseByUUID(int id)
        {
            return Vehicles.GetValueOrDefault(id);
        }


        public static void ApplyCustomization(Vehicle vehicle)
        {
            try
            {
                if (vehicle == null) return;

                VehicleCustomization.ApplyVehCustomization(vehicle);
            }
            catch (Exception e) { _logger.WriteError("ApplyCustomization: " + e.ToString()); }
        }
        public static void ApplyHandlingVehCustomization(Vehicle vehicle)
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
            var vehicle = GoVehicles.FirstOrDefault(item => item.Value.Data.ID == idkey).Key;
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
            foreach (var veh in GoVehicles.Where(item => item.Value.Data.OwnerID == uuid && item.Value.Data.OwnerType == OwnerType.Personal))
            {
                veh.Key.SetSharedData("HOLDERNAME", newName);
            }
        }

        public static void LockCarPressed(PlayerGo sender) //go test
        {
            if (NAPI.Player.IsPlayerInAnyVehicle(sender) && sender.VehicleSeat == VehicleConstants.DriverSeat)
            {
                var vehicle = sender.Vehicle;
                ChangeVehicleDoors(sender, vehicle);
                return;
            }
            else
            {
                var vehicle = getNearestVehicle(sender, 10);
                if (vehicle != null)
                    ChangeVehicleDoors(sender, vehicle);
            }
        }

        public static void ChangeVehicleDoors(Player player, Vehicle vehicle)
        {
            VehicleGo vehGo = vehicle.GetVehicleGo();
            if (vehGo.Data.CanAccessVehicle(player, AccessType.LockedDoor))
                VehicleStreaming.SetLockStatus(vehicle, !VehicleStreaming.GetLockState(vehicle));
            else
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_8", 3000);
        }

        public static void ChangeVehicleDoorOpen(Player player, Vehicle vehicle, DoorID indexDoor)
        {
            VehicleGo vehGo = vehicle.GetVehicleGo();
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
                if (vehGo.Data.CanAccessVehicle(player, indexDoor == DoorID.DoorTrunk ? AccessType.OpenTrunk : AccessType.OpenDoor))
                {
                    VehicleStreaming.SetDoorState(vehicle, indexDoor, DoorState.DoorOpen);
                    if (indexDoor == DoorID.DoorTrunk)
                        Chat.Action(player, "Core1_10");
                }
                else
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_70".Translate(), 3000);
            }
        }

        public static void EngineCarPressed(Player player)
        {
            if (!NAPI.Player.IsPlayerInAnyVehicle(player)) return;
            if (player.VehicleSeat != VehicleConstants.DriverSeat)
            {
                //Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_68".Translate(), 3000);
                return;
            }
            Vehicle vehicle = player.Vehicle;
            VehicleGo vehGo = vehicle.GetVehicleGo();

            if (vehGo.Data.Fuel <= 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_69".Translate(), 3000);
                return;
            }
            if (vehGo.Data.CanAccessVehicle(player, AccessType.EngineChange))
                VehicleStreaming.SetEngineState(vehicle, !VehicleStreaming.GetEngineState(vehicle));
            else
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_70".Translate(), 3000);            
        }

        public static string GetModelAndNumber(int key)
        {
            if (Vehicles.ContainsKey(key))
                return $"{Vehicles[key].ModelName}({Vehicles[key].Number})";
            else
                return "Unknown";
        }

        public static Vehicle CreateTemporaryVehicle(string model, Vector3 pos, Vector3 rot, string number, VehicleAccess access, Player player = null, uint dimension = 0)
        {
            try
            {
                Vehicle vehicle = NAPI.Vehicle.CreateVehicle((VehicleHash)NAPI.Util.GetHashKey(model), pos, rot, 0, 0, dimension: dimension);
                return CreateTemporaryData(vehicle, number, access, player);
            }
            catch (Exception e)
            {
                _logger.WriteDebug($"veh: {(VehicleHash)NAPI.Util.GetHashKey(model)} number: {number} dim: {dimension}");
                throw e;
            }
            
        }

        public static Vehicle CreateTemporaryVehicle(VehicleHash model, Vector3 pos, Vector3 rot, string number, VehicleAccess access, Player player = null, uint dimension = 0)
        {
            Vehicle vehicle = NAPI.Vehicle.CreateVehicle(model, pos, rot, 0, 0, dimension: dimension);
            return CreateTemporaryData(vehicle, number, access, player);
        }

        public static Vehicle CreateTemporaryVehicle(uint model, Vector3 pos, Vector3 rot, string number, VehicleAccess access, Player player = null, uint dimension = 0)
        {
            Vehicle vehicle = NAPI.Vehicle.CreateVehicle(model, pos, rot.Z, 0, 0, dimension: dimension);
            return CreateTemporaryData(vehicle, number, access, player);
        }

        public static Vehicle CreateTemporaryVehicle(int model, Vector3 pos, Vector3 rot, string number, VehicleAccess access, Player player = null, uint dimension = 0)
        {
            Vehicle vehicle = NAPI.Vehicle.CreateVehicle(model, pos, rot.Z, 0, 0, dimension: dimension);
            return CreateTemporaryData(vehicle, number, access, player);
        }

        private static Vehicle CreateTemporaryData(Vehicle vehicle, string number, VehicleAccess access, Player player)
        {
            TemporaryVehicle vehData = new TemporaryVehicle(vehicle.Model, number, access, player);
            if (GoVehicles.ContainsKey(vehicle))
                GoVehicles[vehicle] = new VehicleGo(vehData);
            else
                GoVehicles.Add(vehicle, new VehicleGo(vehData));
            vehicle.NumberPlate = number;
            VehicleStreaming.SetVehicleFuel(vehicle, 70);
            return vehicle;
        }


        [ServerEvent(Event.EntityCreated)]
        public void Event_entityCreated(Entity entity)
        {
            try
            {
                if (entity.Type != EntityType.Vehicle) return;
                Vehicle vehicle = NAPI.Entity.GetEntityFromHandle<Vehicle>(entity);

                string[] keys = NAPI.Data.GetAllEntityData(vehicle);
                foreach (string key in keys) vehicle.ResetData(key);
                int fuel = VehicleConfiguration.GetConfig(vehicle.Model).MaxFuel;
                vehicle.SetSharedData("PETROL", fuel);
                vehicle.SetSharedData("MAXPETROL", fuel);
                vehicle.SetSharedData("hlcolor", 0);
                vehicle.SetSharedData("vehradio", 255);
                //vehicle.SetSharedData("vehmodel", vehicle.Model);
            }
            catch (Exception e) { _logger.WriteError("EntityCreated: " + e.ToString()); }
        }

        [ServerEvent(Event.VehicleDeath)]
        public void Event_vehicleDeath(Vehicle vehicle)
        {
            try
            {
                if (GoVehicles.ContainsKey(vehicle))
                {
                    VehicleGo vehGo = vehicle.GetVehicleGo();
                    vehGo.Data.VehicleDeath(vehicle);
                }
            }
            catch (Exception e) { _logger.WriteError("VehicleDeath: " + e.ToString()); }
        }


        public static void UseRepairKit(Player player, Vehicle vehicle)
        {
            var item = player.GetInventory().SubItemByName(ItemNames.LowRepairKit, 1, LogAction.Use);
            if (item == null)
            {
                Notify.SendError(player, "veh:repair:kitNotFound");
                return;
            }
            RepairBody(vehicle);
        }

        public static void ViewVehicleTechnicalCertificate(Player player, Vehicle vehicle)
        {
            VehicleGo vehGo = vehicle.GetVehicleGo();
            if (!vehGo.IsWearable())
                return;
            var state = new CarPassDTO(vehGo.Data as PersonalBaseVehicle);
            Trigger.ClientEvent(player, "veh:technicalCertificate", JsonConvert.SerializeObject(state));
        }

        public static void SetContainerToVehicle(PlayerGo player, Vehicle vehicle)
        {
            VehicleItemBase container = player.GetLincContainerFromPlayer() as VehicleItemBase;
            if (container != null)
            {
                var vehGo = vehicle.GetVehicleGo();
                var res = vehGo.Data.GiveAbstractItem(container);
                if (res > 0)
                {
                    player.TakeContainerFromPlayer();
                    Notify.SendSuccess(player, $"Вы погрузили {res} предмет в автомобиль");
                }
                else
                    Notify.SendError(player, $"В авто недостаточно места для погрузки");
            }
        }

        public static void CopyCustomization(Vehicle vehicle, Vehicle copy, bool handlingCopy)
        {
            vehicle.GetVehicleGo().Data.VehCustomization = new CustomizationVehicleModel(copy.GetVehicleGo().Data.VehCustomization, handlingCopy);
            ApplyCustomization(vehicle);
            vehicle.GetVehicleGo().Data.Save();
        }

        public static void CopyHandling(Vehicle vehicle, Vehicle copy)
        {
            var vehData = vehicle.GetVehicleGo().Data;
            vehData.VehCustomization.HandlingTuning = new Dictionary<HandlingKeys, object>();
            foreach (var comp in copy.GetVehicleGo().Data.VehCustomization.HandlingTuning)
            {
                vehData.VehCustomization.HandlingTuning.Add(comp.Key, comp.Value);
            }
            ApplyHandlingVehCustomization(vehicle);
            vehicle.GetVehicleGo().Data.Save();
        }

        public static void ClearHandling(Vehicle vehicle)
        {
            var vehData = vehicle.GetVehicleGo().Data;
            vehData.VehCustomization.HandlingTuning = new Dictionary<HandlingKeys, object>();
            ApplyHandlingVehCustomization(vehicle);
            vehicle.GetVehicleGo().Data.Save();
        }

        public static void ApplyVehicleState(Vehicle vehicle)
        {
            VehicleGo vehGo = vehicle.GetVehicleGo();
            SetEngineHealth(vehicle, (vehGo.Data as PersonalBaseVehicle).EngineHealth);
            SetDoorBreak(vehicle, -1);
            SetBrakeBroken(vehicle);
            SetTransmissionCoef(vehicle);
        }

        #region Поломки авто
        public static void SetEngineHealth(Vehicle vehicle, float health)
        {
            VehicleGo vehGo = vehicle.GetVehicleGo();
            if (!vehGo.IsWearable())
                return;
            if (health < 0)
                health = 0;
            (vehGo.Data as PersonalBaseVehicle).EngineHealth = health;
            vehicle.SetSharedData("veh:engineHealth", health);
        }
        public static void SetDoorBreak(Vehicle vehicle, int state)
        {
            VehicleGo vehGo = vehicle.GetVehicleGo();
            if (!vehGo.IsWearable())
                return;

            vehGo.Data.DoorBreak = state;
            vehicle.SetSharedData("veh:doorBreak", vehGo.Data.DoorBreak);
            if ((vehGo.Data.DoorBreak & VehicleConstants.CheckBrokenDoor) > 0)
                VehicleStreaming.SetLockStatus(vehicle, false);
        }

        public static void SetBrakeBroken(Vehicle vehicle)
        {
            VehicleGo vehGo = vehicle.GetVehicleGo();
            if (!vehGo.IsWearable())
                return;
            bool state = (vehGo.Data as PersonalBaseVehicle).Mileage - (vehGo.Data as PersonalBaseVehicle).MileageBrakePadsChange >= VehicleConstants.MileageBrakeBroken;
            if (!vehicle.HasSharedData("veh:BrakesBroke") || state != vehicle.GetSharedData<bool>("veh:BrakesBroke"))
                vehicle.SetSharedData("veh:BrakesBroke", (vehGo.Data as PersonalBaseVehicle).Mileage - (vehGo.Data as PersonalBaseVehicle).MileageBrakePadsChange >= VehicleConstants.MileageBrakeBroken);
        }

        public static void SetTransmissionCoef(Vehicle vehicle)
        {
            VehicleGo vehGo = vehicle.GetVehicleGo();
            if (!vehGo.IsWearable())
                return;
            float coef = VehicleConstants.GetTransmissCoef((vehGo.Data as PersonalBaseVehicle).Mileage - (vehGo.Data as PersonalBaseVehicle).MileageTransmissionService);
            if (!vehicle.HasSharedData("veh:coefTransm") || coef != vehicle.GetSharedData<float>("veh:coefTransm"))
                vehicle.SetSharedData("veh:coefTransm", coef);

        }
        #endregion

        #region Ремонт и ТО
        public static void RepairVehicle(Vehicle vehicle)
        {
            VehicleGo vehGo = vehicle.GetVehicleGo();
            if (!vehGo.IsWearable())
                return;
            RepairBody(vehicle);
            RepairEngine(vehicle);
            BrakeService(vehicle);
            TransmissionService(vehicle);
            EngineService(vehicle);
        }
        public static void RepairBody(Vehicle vehicle)
        {
            vehicle.Repair();
            SetDoorBreak(vehicle, 0);
        }

        /// <summary>
        /// Ремонт двигателя
        /// </summary>
        /// <param name="vehicle"></param>
        public static void RepairEngine(Vehicle vehicle)
        {
            SetEngineHealth(vehicle, 1000F);
        }

        /// <summary>
        /// Обслуживание тормозов
        /// </summary>
        /// <param name="vehicle"></param>
        public static void BrakeService(Vehicle vehicle)
        {
            VehicleGo vehGo = vehicle.GetVehicleGo();
            if (!vehGo.IsWearable())
                return;
            (vehGo.Data as PersonalBaseVehicle).MileageBrakePadsChange = (vehGo.Data as PersonalBaseVehicle).Mileage;
            SetBrakeBroken(vehicle);
        }

        /// <summary>
        /// Обслуживание трансмиссии
        /// </summary>
        /// <param name="vehicle"></param>
        public static void TransmissionService(Vehicle vehicle)
        {
            VehicleGo vehGo = vehicle.GetVehicleGo();
            if (!vehGo.IsWearable())
                return;
            (vehGo.Data as PersonalBaseVehicle).MileageTransmissionService = (vehGo.Data as PersonalBaseVehicle).Mileage;
            SetTransmissionCoef(vehicle);
        }

        /// <summary>
        /// Обслуживание двигателя
        /// </summary>
        /// <param name="vehicle"></param>
        public static void EngineService(Vehicle vehicle)
        {
            VehicleGo vehGo = vehicle.GetVehicleGo();
            if (!vehGo.IsWearable())
                return;
            (vehGo.Data as PersonalBaseVehicle).MileageOilChange = (vehGo.Data as PersonalBaseVehicle).Mileage;
        }

        #endregion

        public static void EngineDescHealth(Vehicle vehicle, bool typeBreak)
        {
            VehicleGo vehGo = vehicle.GetVehicleGo();
            if (!vehGo.IsWearable())
                return;
            if (typeBreak)
                SetEngineHealth(vehicle, (vehGo.Data as PersonalBaseVehicle).EngineHealth - VehicleConstants.GetMileageEngineBroke((vehGo.Data as PersonalBaseVehicle).Mileage - (vehGo.Data as PersonalBaseVehicle).MileageOilChange));
            else
                SetEngineHealth(vehicle, (vehGo.Data as PersonalBaseVehicle).EngineHealth - VehicleConstants.EngineBrokenHealth);
        }

        /// <summary>
        /// Увеличение пробега автомобиля
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="value"></param>
        public static void AddMileage(Vehicle vehicle, float value)
        {
            VehicleGo vehGo = vehicle.GetVehicleGo();
            if (!vehGo.IsWearable())
                return;
            (vehGo.Data as PersonalBaseVehicle).Mileage += value;
            if (VehicleConstants.CheckUpdateEngineState((vehGo.Data as PersonalBaseVehicle).Mileage, value))
            {
                EngineDescHealth(vehicle, true);
                if ((vehGo.Data as PersonalBaseVehicle).Mileage - (vehGo.Data as PersonalBaseVehicle).MileageTransmissionService >= VehicleConstants.MileageTransmissionService)
                    SetTransmissionCoef(vehicle);
            }
            if ((vehGo.Data as PersonalBaseVehicle).Mileage - (vehGo.Data as PersonalBaseVehicle).MileageBrakePadsChange >= VehicleConstants.MileageBrakeBroken)
                SetBrakeBroken(vehicle);
        }

        [RemoteEvent("veh:doorBroken")]
        public static void RemoteEvent_EventSetDoorState(Player player, int indexDoor)
        {
            if (player.IsInVehicle && player.VehicleSeat == VehicleConstants.DriverSeat)
            {
                var vehicle = player.Vehicle;
                VehicleGo vehGo = vehicle.GetVehicleGo();
                if (!vehGo.IsWearable())
                    return;
                if ((vehGo.Data.DoorBreak >> indexDoor) % 2 == 0)
                {
                    int mask = 1 << indexDoor;
                    int state = vehGo.Data.DoorBreak ^ mask;
                    SetDoorBreak(vehicle, state);
                }
            }
        }
        /// <summary>
        /// Поломка двигателя
        /// </summary>
        /// <param name="player"></param>
        [RemoteEvent("veh:engBroken")]
        public static void RemoteEvent_EventEngineBroken(Player player)
        {
            if (player.IsInVehicle && player.VehicleSeat == VehicleConstants.DriverSeat)
            {
                var vehicle = player.Vehicle;
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
        public static void RemoteEvent_DistanceTraveled(Player player, Vehicle vehicle, float value)
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
