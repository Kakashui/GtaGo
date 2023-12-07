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
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.VehicleSystem.Models.VehiclesData
{
    class AdminSaveVehicle : VehicleBase
    {
        public AdminSaveVehicle(DataRow row) : base(row)
        {
            OwnerType = OwnerType.AdminSave;
            Fuel = 0;
        }
        public AdminSaveVehicle(string model, Vector3 position, Vector3 rotation, int color1, int color2)
        {
            OwnerID = 8;
            ModelName = model;
            Position = position;
            Rotation = rotation;
            VehCustomization = new CustomizationVehicleModel();
            VehCustomization.PrimColor = new Color(color1);
            VehCustomization.PrimColor = new Color(color2);
            OwnerType = OwnerType.AdminSave;
            CreateNewInventory();
            var dataQuery = MySQL.QueryRead("INSERT INTO `vehicles`(`holderuuid`, `model`, `componentsnew`, `position`, `rotation`, `typeowner`, `inventoryId`, `number`) " +
                "VALUES (@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6, @prop7); SELECT @@identity;",
                OwnerID, 
                ModelName, 
                JsonConvert.SerializeObject(VehCustomization), 
                JsonConvert.SerializeObject(Position), 
                JsonConvert.SerializeObject(Rotation), 
                (int)OwnerType,
                InventoryId, 
                Number
            );
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
            Number = $"ELITE";
            vehicle.NumberPlate = Number;
            VehicleStreaming.SetEngineState(vehicle, false);
            VehicleStreaming.SetLockStatus(vehicle, true);
            VehicleStreaming.SetVehicleFuel(vehicle, 0);
            return vehicle;
        }

        public override bool CanAccessVehicle(Player player, AccessType access)
        {
            if (!player.IsLogged())
                return false;
            switch (access)
            {
                case AccessType.Tuning:
                case AccessType.SellDollars:
                case AccessType.SellRouletteCar:
                case AccessType.SellZero:
                    return false;
            }
            if (player.GetCharacter().AdminLVL >= 7)
                return true;
            return false;
        }

        public override string GetHolderName()
        {
            return "ELITE";
        }

        public override void Save()
        {
            MySQL.Query("UPDATE `vehicles` SET `holderuuid`=@prop0, `model`=@prop1, `typeowner`=@prop2, `componentsnew`=@prop3, `position`=@prop4, `rotation`=@prop5, `dirt`=@prop6, `number`=@prop7, `power`=@prop8, `torque`=@prop9, `dirtclear` = @prop10  WHERE `idkey`=@prop11",
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
                MySQL.ConvertTime(DirtClear),
                ID
            );
        }

        public override void VehicleDeath(Vehicle vehicle)
        {
            vehicle.CustomDelete();
            Spawn();
        }

        public override void DeleteVehicle(Vehicle vehicle = null)
        {
            DestroyVehicle();
            VehicleManager.Vehicles.Remove(ID);
            MySQL.Query("DELETE FROM `vehicles` WHERE `idkey`= @prop0", ID);
        }

        public override void DestroyVehicle()
        {
            VehicleManager.GoVehicles.FirstOrDefault(item => item.Value.Data.ID == ID).Key?.CustomDelete();
        }
        public override void RespawnVehicle()
        {
            DestroyVehicle();
            Spawn();
        }

        protected override void PlayerEnterVehicle(PlayerGo player, Vehicle vehicle, VehicleGo vehGo, sbyte seatid)
        {
            if (vehGo.Locked)
            {
                VehicleManager.WarpPlayerOutOfVehicle(player);
                Chat.SendToAdmins(1, $"[AntiCheat]: [{player.GetCharacter().UUID}]{player.Name} (seat-locked-car)");
            }
        }

        protected override void PlayerExitVehicle(PlayerGo player, Vehicle vehicle, VehicleGo vehGo)
        {
        }
    }
}