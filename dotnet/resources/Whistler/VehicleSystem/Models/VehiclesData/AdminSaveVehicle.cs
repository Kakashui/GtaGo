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
            OwnerID = 1;
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
            vehicle.Session.Dimension= dimension;
            if (Main.AllVehicles.ContainsKey(vehicleId)) Main.AllVehicles[vehicleId] = vehicle;
            else Main.AllVehicles.Add(vehicleId, vehicle); 
            Number = $"MIKZZZ";
            vehicle.NumberPlate = Number;
            VehicleStreaming.SetEngineState(vehicle, false);
            VehicleStreaming.SetLockStatus(vehicle, true); // true
            VehicleStreaming.SetVehicleFuel(vehicle, 0); // 0
            return vehicle;
        }

        public override bool CanAccessVehicle(ExtPlayer player, AccessType access)
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
            if (player.Character.AdminLVL >= 7)
                return true;
            return false;
        }

        public override string GetHolderName()
        {
            return "MIKZZZ";
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

        public override void VehicleDeath(ExtVehicle vehicle)
        {
            vehicle.CustomDelete();
            Spawn();
        }

        public override void DeleteVehicle(ExtVehicle vehicle = null)
        {
            DestroyVehicle();
            VehicleManager.Vehicles.Remove(ID);
            MySQL.Query("DELETE FROM `vehicles` WHERE `idkey`= @prop0", ID);
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
            if (vehicle.IsLocked)
            {
                VehicleManager.WarpPlayerOutOfVehicle(player);
                Chat.SendToAdmins(1, $"[AntiCheat]: [{player.Character.UUID}]{player.Name} (seat-locked-car)");
            }
        }

        protected override void PlayerExitVehicle(ExtPlayer player, ExtVehicle vehicle)
        {
        }
    }
}