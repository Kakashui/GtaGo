using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Whistler.Core.Character;
using Whistler.Families;
using Whistler.VehicleSystem;
using Whistler.Helpers;
using Whistler.VehicleSystem.Models;
using Whistler.Possessions;
using Whistler.VehicleSystem.Models.VehiclesData;
using Whistler.VehicleTrading;
using Whistler.Entities;

namespace Whistler.Houses.Models
{
    class Garage
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Garage));
        public int ID { get; }
        public int Type { get; set; }
        public Vector3 Position { get; }
        public Vector3 Rotation { get; }
        [JsonIgnore] public int NativeType { get; set; }
        [JsonIgnore] public uint Dimension { get; set; }

        [JsonIgnore]
        private InteractShape shape;

        [JsonIgnore]
        public House GarageHouse
        {
            get
            {
                return HouseManager.GetHouseByGarageId(ID);
            }
        }
        [JsonIgnore]
        public Whistler.Houses.Models.GarageType GarageConfig
        {
            get
            {
                return Whistler.Houses.Configs.HouseConfigs.GarageTypes[Type];
            }
        }

        [JsonIgnore]
        public Dictionary<int, VehicleGaragePlaces> spawnedVehicles = new Dictionary<int, VehicleGaragePlaces>();


        public Garage(int id, int type, Vector3 position, Vector3 rotation, int nativeType)
        {
            ID = id;
            NativeType = nativeType;
            Type = type;
            Position = position;
            Rotation = rotation;

            shape = InteractShape.Create(position, 2, 4)
                .AddEnterPredicate((c, player) => GetIsPlayerCanEnterGarage(player, true))
                .AddOnEnterColshapeExtraAction((c, player) =>
                {
                    SafeTrigger.SetData(player, "GARAGEID", ID);
                })
                .AddOnExitColshapeExtraAction((c, player) =>
                {
                    player.ResetData("GARAGEID");
                })
                .AddInteraction(p => EnterGarage(p, true), "interact_21");
        }

        public bool GetIsPlayerCanEnterGarage(ExtPlayer player, bool fromStreet)
        {
            if (!player.IsLogged())
                return false;
            return (GarageHouse?.GetAccessToGarage(player) ?? false) || (!fromStreet && GarageHouse.ID == player.Character.HouseTarget);
        }

        public void DeleteCar(int idkey, bool resetPosition = true)
        {
            if (spawnedVehicles.ContainsKey(idkey))
            {
                spawnedVehicles.Remove(idkey);
                if (resetPosition)
                    VehicleManager.Vehicles[idkey].Position = null;
            }
        }
        public void Create()
        {
            MySQL.Query("INSERT INTO `garages`(`id`,`type`,`position`,`rotation`, `nativeType`) VALUES (@prop0, @prop1, @prop2, @prop3, @prop4)", ID, Type, JsonConvert.SerializeObject(Position), JsonConvert.SerializeObject(Rotation), Type);
        }
        public void Save()
        {
            MySQL.Query("UPDATE `garages` SET `type`= @prop0 WHERE `id` = @prop1", Type, ID);
        }
        public void Destroy()
        {
            shape?.Destroy();
        }

        private int GetClearPlaceInGarage()
        {
            for (var i = 0; i < GarageConfig.MaxCars; i++)
            {
                if (spawnedVehicles.Values.FirstOrDefault(t => t.Place == i) == null)
                    return i;
            }
            return -1;
        }
        public void SpawnCar(int idkey)
        {
            try
            {
                if (spawnedVehicles.ContainsKey(idkey))
                    return;
                int place = GetClearPlaceInGarage();
                if (place < 0)
                    return;

                var vehData = VehicleManager.Vehicles[idkey] as PersonalBaseVehicle;
                if (vehData.IsDeath || vehData.TradePoint > 0) return;
                Vector3 position = GarageConfig.CoordsModel.VehiclesPositions[place];
                Vector3 rotation = GarageConfig.CoordsModel.VehiclesRotations[place];
                uint dimension = Dimension;
                if (vehData.Position != null && vehData.Rotation != null)
                {
                    position = vehData.Position;
                    rotation = vehData.Rotation;
                    dimension = 0;
                    place = -1;
                }
                else if (vehData.SavePosition != null && vehData.SaveRotation != null)
                {
                    position = vehData.SavePosition;
                    rotation = vehData.SaveRotation;
                    dimension = 0;
                    place = -1;
                }
                SpawnCarAtPosition(idkey, position, rotation, dimension, place);
            }
            catch (Exception e) { _logger.WriteError("SpawnCar: " + e.ToString()); }
        }

        public void SpawnCars(List<int> vehicles)
        {
            try
            {
                int i = 0;
                foreach (var idkey in vehicles)
                {
                    NAPI.Task.Run(() => {
                        SpawnCar(idkey);
                    }, i++ * 200);
                }
            }
            catch (Exception e) { _logger.WriteError("SpawnCars: " + e.ToString()); }
        }
        public void DestroyCars(bool clearPos = false)
        {
            NAPI.Task.Run(() =>
            {
                foreach (var veh in spawnedVehicles)
                {
                    if (VehicleManager.Vehicles.ContainsKey(veh.Key))
                    {
                        if (veh.Value.Place >= 0 || clearPos)
                            VehicleManager.Vehicles[veh.Key].Position = null;
                        else
                        {
                            VehicleManager.Vehicles[veh.Key].Position = veh.Value.Vehicle.Position;
                            VehicleManager.Vehicles[veh.Key].Rotation = veh.Value.Vehicle.Rotation;
                        }
                        VehicleManager.Vehicles[veh.Key].Save();
                    }
                    veh.Value.Vehicle.CustomDelete();
                }
                spawnedVehicles = new Dictionary<int, VehicleGaragePlaces>();
            });
        }
        public void RespawnCars()
        {
            try
            {
                var house = HouseManager.Houses.Find(h => h.GarageID == ID);
                if (house == null)
                    return;
                if (house.OwnerID < 0)
                    return;

                var vehicles = VehicleManager.getAllHolderVehicles(house.OwnerID, house.OwnerType);
                vehicles.RemoveAll(veh => spawnedVehicles.ContainsKey(veh));
                SpawnCars(vehicles);
            }
            catch (Exception e) { _logger.WriteError("SpawnCars: " + e.ToString()); }
        }
        public void SpawnCarAtPosition(int idkey, Vector3 position, Vector3 rotation, uint dimension = 0, int place = -1)
        {
            try
            {
                if (!spawnedVehicles.ContainsKey(idkey))
                {
                    ExtVehicle veh = VehicleManager.Vehicles[idkey].Spawn(position, rotation, dimension);
                    if (veh != null)
                        spawnedVehicles.Add(idkey, new VehicleGaragePlaces(veh, place));
                }
            }
            catch (Exception e) { _logger.WriteError("SpawnCars: " + e.ToString()); }
        }

        public void ExitGarageIntoVehicle(ExtPlayer player)
        {
            if (!player.IsInVehicle || player.Vehicle == null || player.VehicleSeat != VehicleConstants.DriverSeat)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_138", 3000);
                return;
            }
            if (!GetIsPlayerCanEnterGarage(player, true))
                return;
            ExtVehicle vehicle = player.Vehicle as ExtVehicle;
            if (!spawnedVehicles.ContainsKey(vehicle.Data.ID))
                return;
            spawnedVehicles[vehicle.Data.ID].Place = -1;

            player.Character.InsideGarageID = -1;
            player.ChangePositionWithCar(Position + new Vector3(0, 0, 0.2), Rotation, 1000);
            vehicle.Dimension = 0;
            //player.CustomSetIntoVehicle(vehicle, VehicleConstants.DriverSeatClientSideBroken);
        }

        public void SendVehicleIntoGarage(int idkey)
        {
            try
            {
                if (spawnedVehicles.ContainsKey(idkey))
                {
                    VehicleManager.Vehicles[idkey].Position = null;
                    spawnedVehicles[idkey].Vehicle?.CustomDelete();
                    spawnedVehicles.Remove(idkey);
                    SpawnCar(idkey);
                }
                else
                {
                    VehicleManager.Vehicles[idkey].Position = null;
                    SpawnCar(idkey);
                }
            }
            catch (Exception e) { _logger.WriteError($"SendVehiclesInsteadNearest: " + e.ToString()); }
        }

        public void SendPlayer(ExtPlayer player)
        {
            SafeTrigger.UpdateDimension(player,  Dimension);
            player.ChangePosition(GarageConfig.CoordsModel.Position);
            player.Character.InsideGarageID = ID;
            if (GarageConfig.IPLType > GarageIPLType.NoIPL)
                SafeTrigger.ClientEvent(player,"garage:loadInteriors", GarageConfig.CoordsModel.Position, (int)GarageConfig.IPLType);
            RespawnCars();
        }
        public void RemovePlayer(ExtPlayer player)
        {
            SafeTrigger.UpdateDimension(player,  0);
            player.ChangePosition(Position);
            player.Character.InsideGarageID = -1;
        }



        public bool EnterGarage(ExtPlayer player, bool fromStreet)
        {
            if (!GetIsPlayerCanEnterGarage(player, fromStreet))
                return false;

            var house = GarageHouse;

            if (player.IsInVehicle && player.Vehicle != null)
            {
                ExtVehicle playerVehicle = player.Vehicle as ExtVehicle;
                var vehicles = VehicleManager.getAllHolderVehicles(house.OwnerID, house.OwnerType);
                if (!vehicles.Contains(playerVehicle.Data.ID))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_1", 3000);
                    return false;
                }
                else
                {
                    GarageManager.SendVehicleIntoGarage(playerVehicle.Data);
                }
            }
            SendPlayer(player);
            return true;
        }
    }
}