using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Whistler.Businesses;
using Whistler.Common;
using Whistler.Core;
using Whistler.Entities;
using Whistler.GarbageCollector;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.VehicleSystem.Models.VehiclesData
{
    class RentVehicle : VehicleBase
    {
        private static int platecounter = 0;
        public Player Driver { get; set; } = null;
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
        protected override Vehicle SpawnCar()
        {
            return Spawn(Position, Rotation, 0);
        }
        protected override Vehicle SpawnCar(Vector3 position, Vector3 rotation, uint dimension)
        {
            var vehicle = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(ModelName), position, rotation.Z, 0, 0);
            if (VehicleManager.GoVehicles.ContainsKey(vehicle))
                VehicleManager.GoVehicles[vehicle] = new VehicleGo(this);
            else
                VehicleManager.GoVehicles.Add(vehicle, new VehicleGo(this));
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

        public override bool CanAccessVehicle(Player player, AccessType access)
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
                    if (player.GetCharacter().AdminLVL >= 3)
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

        public override void VehicleDeath(Vehicle vehicle)
        {
            RespawnVehicle();
        }

        public override void DeleteVehicle(Vehicle vehicle = null)
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
            var vehicle = Vehicle;
            if (Driver != null)
            {
                Driver.ResetData("RENTED_CAR");
                Driver = null;
            }
            vehicle.Position = Position;
            vehicle.Rotation = Rotation;
            VehicleManager.RepairCar(vehicle);
            VehicleStreaming.SetEngineState(vehicle, false);
            VehicleStreaming.SetVehicleFuel(vehicle, 50);
            VehicleStreaming.SetLockStatus(vehicle, false);
        }

        protected override void PlayerEnterVehicle(PlayerGo player, Vehicle vehicle, VehicleGo vehGo, sbyte seatid)
        {
            if (seatid != VehicleConstants.DriverSeat) return;
            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_145", 3000);
            VehicleManager.WarpPlayerOutOfVehicle(player);
        }

        protected override void PlayerExitVehicle(PlayerGo player, Vehicle vehicle, VehicleGo vehGo)
        {
            if (Driver == player)
            {
                Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Biz_144".Translate(RentCarBusiness.RespawnTime), 3000);
                GarbageManager.Add(vehicle, RentCarBusiness.RespawnTime);
            }
        }
    }
}