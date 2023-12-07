using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using Whistler.Businesses;
using Whistler.Common;
using Whistler.Core;
using Whistler.Domain.Fractions;
using Whistler.Entities;
using Whistler.GarbageCollector;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.SDK;
using static AutoMapper.Internal.ExpressionFactory;

namespace Whistler.VehicleSystem.Models.VehiclesData
{
    class RentVehicle : VehicleBase
    {
        private static int platecounter = 0;
        public ExtPlayer Driver { get; set; } = null;
        public RentVehicle(DataRow row) : base(row)
        {
            OwnerType = OwnerType.Rent;
            Fuel = 50;
        }
        public RentVehicle(int businessId, string model, Vector3 position, Vector3 rotation, int color1, int color2)
        {
            OwnerID = businessId;
            ModelName = model;
            Position = position;
            Rotation = rotation;
            VehCustomization = new CustomizationVehicleModel();
            VehCustomization.PrimColor = new Color(color1);
            VehCustomization.PrimColor = new Color(color2);
            OwnerType = OwnerType.Rent;
            CreateNewInventory();
            var dataQuery = MySQL.QueryRead("INSERT INTO `vehicles`(`holderuuid`, `model`, `componentsnew`, `position`, `rotation`, `typeowner`, `inventoryId`) " +
                "VALUES (@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6); SELECT @@identity;",
                OwnerID, ModelName, JsonConvert.SerializeObject(VehCustomization), JsonConvert.SerializeObject(Position), JsonConvert.SerializeObject(Rotation), (int)OwnerType, InventoryId);
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
            Number = $"RC{OwnerID}{platecounter++}";
            vehicle.NumberPlate = Number;
            VehicleStreaming.SetEngineState(vehicle, false);
            VehicleStreaming.SetLockStatus(vehicle, false);
            VehicleStreaming.SetVehicleFuel(vehicle, 50);
            var business = BusinessManager.BizList.GetValueOrDefault(OwnerID);
            if (business != null)
            {
                Price = business.GetPriceByProductName(ModelName).CurrentPrice;
            }
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
                case AccessType.Inventory:
                    if (player.Character.AdminLVL >= 3)
                        return true;
                    if (Driver == player)
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
            return $"RENT {OwnerID}";
        }

        public override void Save()
        {
            MySQL.Query("UPDATE `vehicles` SET `holderuuid`=@prop0, `model`=@prop1, `typeowner`=@prop2, `componentsnew`=@prop3 WHERE `idkey`=@prop4",
                OwnerID,
                ModelName,
                (int)OwnerType,
                JsonConvert.SerializeObject(VehCustomization),
                ID
            );
        }

        public override void VehicleDeath(ExtVehicle vehicle)
        {
            RespawnVehicle();
        }

        public override void DeleteVehicle(ExtVehicle vehicle = null)
        {
            if (!BusinessManager.BizList.ContainsKey(OwnerID))
                return;
            var business = BusinessManager.BizList[OwnerID];
            var product = business.Products.Where(x => x.Name == ModelName).FirstOrDefault();
            if (product != null)
            {
                product.Lefts--;
                if (product.Lefts < 0)
                {
                    business.Products.Remove(product);
                }
            }
            VehicleManager.Vehicles.Remove(ID);
            MySQL.Query("DELETE FROM `vehicles` WHERE `idkey` = @prop0", ID);
            if (vehicle != null)
            {
                vehicle.CustomDelete();
            }

        }

        public override void DestroyVehicle()
        {
            Vehicle?.CustomDelete();
        }
        public override void RespawnVehicle()
        {
            Vehicle vehicle = Vehicle;
            if (Driver != null)
            {
                Driver.ResetData("RENTED_CAR");
                Driver = null;
            }
            vehicle.Position = Position;
            vehicle.Rotation = Rotation;

            ExtVehicle extVehicle = vehicle as ExtVehicle;
            VehicleManager.RepairCar(extVehicle);
            VehicleStreaming.SetEngineState(extVehicle, false);
            VehicleStreaming.SetVehicleFuel(extVehicle, 50);
            VehicleStreaming.SetLockStatus(extVehicle, false);
        }

        protected override void PlayerEnterVehicle(ExtPlayer player, ExtVehicle vehicle, sbyte seatid)
        {
            if (seatid != VehicleConstants.DriverSeat) return;
            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_145", 3000);
            VehicleManager.WarpPlayerOutOfVehicle(player);
        }

        protected override void PlayerExitVehicle(ExtPlayer player, ExtVehicle vehicle)
        {
            if (Driver == player)
            {
                Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Biz_144".Translate(RentCarBusiness.RespawnTime), 3000);
                GarbageManager.Add(vehicle, RentCarBusiness.RespawnTime);
            }
        }
    }
}