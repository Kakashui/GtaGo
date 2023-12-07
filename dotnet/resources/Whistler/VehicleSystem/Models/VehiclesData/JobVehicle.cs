using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using Whistler.Common;
using Whistler.Core;
using Whistler.Entities;
using Whistler.GarbageCollector;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.VehicleSystem.Models.VehiclesData
{
    class JobVehicle : VehicleBase
    {
        public WorkType WorkType { get; set; }
        public bool OnWork { get; set; } = false;
        public ExtPlayer Driver { get; set; } = null;
        public JobVehicle(DataRow row) : base(row)
        {
            Price = Convert.ToInt32(row["price"]);
            OwnerType = OwnerType.Job;
            WorkType = (WorkType)OwnerID;

        }
        public JobVehicle(WorkType typeJob, string number, string model, Vector3 position, Vector3 rotation, int color1, int color2)
        {
            OwnerID = (int)typeJob;
            Number = number;
            ModelName = model;
            Position = position;
            Rotation = rotation;
            VehCustomization = new CustomizationVehicleModel();
            VehCustomization.PrimColor = new Color(color1);
            VehCustomization.PrimColor = new Color(color2);
            OwnerType = OwnerType.Job;
            CreateNewInventory();

            var dataQuery = MySQL.QueryRead("INSERT INTO `vehicles`(`holderuuid`, `number`, `model`, `componentsnew`, `position`, `rotation`, `typeowner`, `inventoryId`) " +
                "VALUES (@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6, @prop7); SELECT @@identity;",
                OwnerID, 
                Number, 
                ModelName, 
                JsonConvert.SerializeObject(VehCustomization), 
                JsonConvert.SerializeObject(Position), 
                JsonConvert.SerializeObject(Rotation), 
                (int)OwnerType, 
                InventoryId
            );
            ID = Convert.ToInt32(dataQuery.Rows[0][0]);
            VehicleManager.Vehicles.Add(ID, this);
        }
        protected override ExtVehicle SpawnCar()
        {
            return Spawn(Position, Rotation, 0);
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
            VehicleStreaming.SetEngineState(vehicle, false);
            VehicleStreaming.SetLockStatus(vehicle, false);
            VehicleManager.ApplyCustomization(vehicle);
            VehicleStreaming.SetVehicleFuel(vehicle, VehicleConfiguration.GetConfig(model).MaxFuel);
            OnWork = false;

            SafeTrigger.SetSharedData(vehicle, "HOLDERNAME", GetHolderName());
            return vehicle;
        }

        public override bool CanAccessVehicle(ExtPlayer player, AccessType access)
        {
            if (!player.IsLogged())
                return false;
            switch (access)
            {
                case AccessType.LockedDoor:
                case AccessType.OpenDoor:
                case AccessType.OpenTrunk:
                case AccessType.EngineChange:
                    if (player.Character.AdminLVL >= 3)
                        return true;
                    if (Driver == player)
                        return true;
                    return false;
                case AccessType.Inventory:
                    if (player.Character.AdminLVL >= 3)
                        return true;
                    return false;
                case AccessType.Tuning:
                case AccessType.SellDollars:
                case AccessType.SellRouletteCar:
                case AccessType.SellZero:
                default:
                    return false;
            }
        }

        public override string GetHolderName()
        {
            return $"{OwnerType} {WorkType}";
        }

        public override void Save()
        {
             MySQL.Query("UPDATE `vehicles` SET `holderuuid`=@prop0, `model`=@prop1, `typeowner`=@prop2, `componentsnew`=@prop3, `number`=@prop4, `power`=@prop5, `torque`=@prop6  WHERE `idkey`=@prop7",
                  OwnerID,
                  ModelName,
                  (int)OwnerType,
                  JsonConvert.SerializeObject(VehCustomization),
                  Number,
                  EnginePower,
                  EngineTorque,
                  ID
             );
        }

        public override void VehicleDeath(ExtVehicle vehicle)
        {
            ExtPlayer player = vehicle.GetData<ExtPlayer>("DRIVER");
            if (player != null)
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Core1_76", 3000);
                player.Session.OnWork = false;
                player.Character.Customization.Apply(player);
            }
            switch (WorkType)
            {
                case WorkType.Bus:
                    return;
            }
            return;
        }

        public override void DeleteVehicle(ExtVehicle vehicle = null)
        {
            DestroyVehicle();
            VehicleManager.Vehicles.Remove(ID);
            MySQL.Query("DELETE FROM `vehicles` WHERE `idkey` = @prop0", ID);
        }

        public override void DestroyVehicle()
        {
            Main.AllVehicles.FirstOrDefault(item => item.Value.Data.ID == ID).Value?.CustomDelete();
        }
        public override void RespawnVehicle()
        {
            DestroyVehicle();
            Spawn();
        }

        protected override void PlayerEnterVehicle(ExtPlayer player, ExtVehicle vehicle, sbyte seatid)
        {

            if (player.VehicleSeat == VehicleConstants.DriverSeat)
            {
                if (Driver == player)
                    GarbageManager.Remove(vehicle);
                if (((JobVehicle)vehicle.Data).WorkType == WorkType.DockLoader)
                {
                    if (player.Character.WorkID != (int)WorkType.DockLoader)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "DockLoader_1".Translate(), 3000);
                        VehicleManager.WarpPlayerOutOfVehicle(player);
                        return;
                    }
                    if (BusinessManager.GetBusinessByOwner(player.Character.UUID) != null)
                    {
                        Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "DockLoader_2".Translate(),
                            3000);
                        VehicleManager.WarpPlayerOutOfVehicle(player);
                        return;
                    }

                    Notify.Send(player, NotifyType.Info, NotifyPosition.Center, "DockLoader_4".Translate(), 3000);
                }
            }
        }

        protected override void PlayerExitVehicle(ExtPlayer player, ExtVehicle vehicle)
        {
        }
    }
}