using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Transactions;
using Whistler.Common;
using Whistler.Entities;
using Whistler.Fractions;
using Whistler.Fractions.TransportAlcohol;
using Whistler.GUI.Documents.Enums;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.VehicleSystem.Models.VehiclesData
{
    class FractionVehicle : VehicleBase
    {
        public ConvoyType CanConvoy { get; set; } = ConvoyType.None;
        public int MinRank { get; set; }
        public uint Dimension { get; set; }

        private GTANetworkAPI.Object _spike = null;
        private ColShape _shape = null;
        private string _timer = null;
        private DateTime _lastSpawn = DateTime.Now.AddDays(-1);

        private static int _respawnSpikeTime = 300;
        public DateTime LastLoadItems = DateTime.Now.AddHours(-1);

        public bool InAlcoholBox = false;
        public int targetId = 0;
        public FractionVehicle(DataRow row) : base(row)
        {
            MinRank = Convert.ToInt32(row["rank"]);
            Dimension = Convert.ToUInt32(row["dimension"]);
            OwnerType = OwnerType.Fraction;
        }
        public FractionVehicle(int fracid, string model, string number, int minRank, Vector3 position, Vector3 rotation, int color1, int color2, uint dimension = 0)
        {
            Number = number;
            OwnerID = fracid;
            ModelName = model;
            Position = position;
            Rotation = rotation;
            MinRank = minRank;
            VehCustomization = new CustomizationVehicleModel
            {
                PrimColor = new Color(color1),
                SecColor = new Color(color2),
            };
            OwnerType = OwnerType.Fraction;
            Dimension = dimension;
            CreateNewInventory();
            var dataQuery = MySQL.QueryRead("INSERT INTO `vehicles`(`number`, `holderuuid`, `model`, `componentsnew`, `position`, `rotation`, `typeowner`, `rank`, `dimension`, `inventoryId`, `dirtclear`) " +
                "VALUES (@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6, @prop7, @prop8, @prop9, @prop10); SELECT @@identity;",
                Number, 
                OwnerID, 
                ModelName, 
                JsonConvert.SerializeObject(VehCustomization), 
                JsonConvert.SerializeObject(Position), 
                JsonConvert.SerializeObject(Rotation), 
                (int)OwnerType, 
                MinRank, 
                Dimension, 
                InventoryId,
                MySQL.ConvertTime(DirtClear)
            );
            ID = Convert.ToInt32(dataQuery.Rows[0][0]);
            VehicleManager.Vehicles.Add(ID, this);
        }

        protected override ExtVehicle SpawnCar()
        {
            return Spawn(Position, Rotation, Dimension);
        }
        protected override ExtVehicle SpawnCar(Vector3 position, Vector3 rotation, uint dimension)
        {
            if (Thread.CurrentThread.Name != "Main") return null;

            uint model = NAPI.Util.GetHashKey(ModelName);
            ExtVehicle vehicle = NAPI.Vehicle.CreateVehicle(model, position, rotation.Z, 0, 0) as ExtVehicle;
            if (vehicle == null || !vehicle.Exists) return null;

            vehicle.Initialize(this);
            vehicle.InitializeLicense(vehicle.Class);
            ushort vehicleId = vehicle.Id;
            vehicle.Session.Id = vehicle.Id;
            vehicle.Session.Dimension = dimension;
            if (Main.AllVehicles.ContainsKey(vehicleId)) Main.AllVehicles[vehicleId] = vehicle;
            else Main.AllVehicles.Add(vehicleId, vehicle);

            vehicle.NumberPlate = Number;
            vehicle.Dimension = dimension;
            VehicleStreaming.SetEngineState(vehicle, false);
            VehicleStreaming.SetLockStatus(vehicle, false);
            VehicleStreaming.SetVehicleFuel(vehicle, 100);

            if(Enum.IsDefined(typeof(VehicleHash), model))
            {
                VehicleHash hashModel = (VehicleHash)model;
                if (hashModel == VehicleHash.Pbus || hashModel == VehicleHash.Pranger || hashModel == VehicleHash.Volatus)
                {
                    if (hashModel == VehicleHash.Volatus)
                        CanConvoy = ConvoyType.Helicopter;
                    else
                        CanConvoy = ConvoyType.Vehicle;
                }
            }
            DeleteSpike();
            return vehicle;
        }

        public override bool CanAccessVehicle(ExtPlayer player, AccessType access)
        {
            if (!player.IsLogged())
                return false;
            var fracid = player.Character.FractionID;
            switch (access)
            {
                case AccessType.OpenTrunk:
                    if (player.Character.AdminLVL >= 1)
                        return true;
                    if (OwnerID != fracid && (fracid == 7 || fracid == 9))
                        return true;
                    else if (OwnerID == fracid)
                    {
                        if (player.Character.FractionLVL < MinRank)
                            return false;
                        else
                            return true;
                    }
                    return false;
                case AccessType.OpenDoor:
                case AccessType.LockedDoor:
                case AccessType.EngineChange:

                    if (OwnerID == 14 && ModelName.ToLower() == "brickade" && access != AccessType.EngineChange)
                        return true;
                    /*if (CanConvoy != ConvoyType.None && OwnerID == 7 && fracid == 18)
                        if ((player.Character.FractionLVL < 4 && CanConvoy == ConvoyType.Vehicle) || (player.Character.FractionLVL == 3 && CanConvoy == ConvoyType.Helicopter))
                            return true;*/
                    if (player.Character.AdminLVL >= 1)
                        return true;
                    if (fracid == OwnerID && player.Character.FractionLVL >= MinRank)
                        return true;
                    if (player.Character.MediaHelper > 0)
                        return true;
                    return false;
                case AccessType.Inventory:

                    /*if (CanConvoy != ConvoyType.None && OwnerID == 7 && fracid == 18)
                        if ((player.Character.FractionLVL < 4 && CanConvoy == ConvoyType.Vehicle) || (player.Character.FractionLVL == 3 && CanConvoy == ConvoyType.Helicopter))
                            return true;*/
                    if (player.Character.AdminLVL >= 1)
                        return true;
                    if (fracid == OwnerID && player.Character.FractionLVL >= MinRank)
                        return true;
                    if (player.Character.MediaHelper > 0)
                        return true;
                    if (OwnerID == 14 && ModelName.ToLower() == "brickade")
                    {
                        if ((DateTime.Now - LastLoadItems).TotalSeconds > 300)
                            return true;
                    }
                    return false;
                case AccessType.Tuning:
                    if (player.Character.AdminLVL >= 5)
                        return true;
                    if (Manager.isLeader(player, OwnerID))
                        return true;
                    return false;
                case AccessType.SellDollars:
                case AccessType.SellRouletteCar:
                case AccessType.SellZero:
                    return false;
            }
            return false;
        }

        public override string GetHolderName()
        {
            return Fractions.Configs.GetConfigOrDefault(OwnerID).Name;
        }

        public override void Save()
        {
            MySQL.Query("UPDATE `vehicles` SET `holderuuid`=@prop0, `model`=@prop1, `typeowner`=@prop2, `componentsnew`=@prop3, `position`=@prop4, `rotation`=@prop5," +
                " `dirt`=@prop6, `number`=@prop7, `power`=@prop8, `torque`=@prop9, `rank`=@prop10, `dirtclear` = @prop11  WHERE `idkey`=@prop12",
                  OwnerID,
                  ModelName,
                  (int)OwnerType,
                  JsonConvert.SerializeObject(VehCustomization),
                  JsonConvert.SerializeObject(Position),
                  JsonConvert.SerializeObject(Rotation),
                  Dirt,
                  Number,
                  EnginePower,
                  EngineTorque,
                  MinRank,
                  MySQL.ConvertTime(DirtClear),
                  ID
              );
        }

        public override void VehicleDeath(ExtVehicle vehicle)
        {
            DeleteSpike();
            return;
        }

        public override void DeleteVehicle(ExtVehicle vehicle = null)
        {
            VehicleManager.Vehicles.Remove(ID);
            MySQL.Query("DELETE FROM `vehicles` WHERE `idkey` = @prop0", ID);
            if (vehicle != null)
            {
                vehicle.CustomDelete();
            }
            DeleteSpike();
        }

        public override void DestroyVehicle()
        {
            Main.AllVehicles.FirstOrDefault(item => item.Value.Data.ID == ID).Value?.CustomDelete();
        }
        public override void RespawnVehicle()
        {
            DestroyVehicle();
            NAPI.Task.Run(() => { Spawn(); }, 1500);            
        }

        public void SpawnSpike(ExtVehicle vehicle)
        {
            if (OwnerID == 7 || OwnerID == 9)
            {
                if (_lastSpawn.AddSeconds(_respawnSpikeTime) > DateTime.Now)
                    return;
                DeleteSpike();
                var pos = vehicle.Position.GetOffsetPosition(vehicle.Rotation, new Vector3(1.2, -2, 0));
                _spike = NAPI.Object.CreateObject(NAPI.Util.GetHashKey("p_stinger_03"), pos, vehicle.Rotation - new Vector3(0, 0, 90), 255, vehicle.Dimension);
                _spike.SetSharedData("placeOnGround", true);
                var posShape = pos.GetOffsetPosition(vehicle.Rotation, new Vector3(-1.2, 0, 0));
                _shape = NAPI.ColShape.CreateCylinderColShape(posShape - new Vector3(0, 0, 1), 1.5F, 3, vehicle.Dimension);
                _shape.OnEntityEnterColShape += OnEntityEnterColShape;
                _lastSpawn = DateTime.Now;
                if (_timer != null)
                {
                    Timers.Stop(_timer);
                    _timer = null;
                }
                _timer = Timers.StartOnce(_respawnSpikeTime * 1000, DeleteSpike);
            }
        }
        public void DeleteSpike()
        {
            if (_spike != null)
            {
                _spike.Delete();
                _spike = null;
            }
            if (_shape != null)
            {
                _shape.Delete();
                _shape = null;
            }
            if (_timer != null)
            {
                Timers.Stop(_timer);
                _timer = null;
            }
        }
        public bool SpikeIsSpawned()
        {
            return _spike != null && _shape != null;
        }

        private void OnEntityEnterColShape(ColShape colShape, Player player)
        {

            if (!(player is ExtPlayer client)) return;

            if (client.Vehicle != null)
            {
                SafeTrigger.ClientEvent(client, "veh:setTyreBurst");
            }
        }

        protected override void PlayerEnterVehicle(ExtPlayer player, ExtVehicle vehicle, sbyte seatid)
        {
            if (player.VehicleSeat == VehicleConstants.DriverSeat)
            {
                var fracid = player.Character.FractionID;
                //if (player.Character.AdminLVL >= 1) return;
                //if (Holder == 14 && ModelName.ToLower() == "brickade")
                //{
                //    if (DateTime.Now.Hour < 12 && fracid != Holder)
                //    {
                //        Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Core1_63", 3000);
                //        VehicleManager.WarpPlayerOutOfVehicle(player);
                //    }
                //    return; //чтоб все могли ездить на них
                //}
                //if (Holder != fracid || player.Character.FractionLVL < MinRank)
                //{
                //    if (CanConvoy != ConvoyType.None && Holder == 7 && fracid == 18)
                //        if ((player.Character.FractionLVL < 4 && CanConvoy == ConvoyType.Vehicle) || (player.Character.FractionLVL == 3 && CanConvoy == ConvoyType.Helicopter))
                //            return;
                //    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_62", 3000);
                //    VehicleManager.WarpPlayerOutOfVehicle(player);
                //    return;
                //}
                if (OwnerID == 16 && InAlcoholBox)
                {
                    if (fracid == OwnerID)
                        TransportManager.GetWaipoint(player, targetId);
                    else if (fracid > 0 || player.GetFamily() != null)
                        TransportManager.GetWaipoint(player, -1);
                }
            }
        }

        protected override void PlayerExitVehicle(ExtPlayer player, ExtVehicle vehicle)
        {
            if (OwnerID == 16 && InAlcoholBox)
                player.DeleteClientMarker(1699);
        }
    }
}
