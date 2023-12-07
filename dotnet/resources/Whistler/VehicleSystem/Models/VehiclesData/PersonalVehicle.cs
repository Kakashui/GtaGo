using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Whistler.Common;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.Possessions;
using Whistler.SDK;

namespace Whistler.VehicleSystem.Models.VehiclesData
{
    public class PersonalVehicle : PersonalBaseVehicle
    {
        public PersonalVehicle(DataRow row) : base(row, OwnerType.Personal)
        {
        }
        public PersonalVehicle(int holderId, string model, Color color1, Color color2, int fuel = 100, int price = 0, PropBuyStatus status = PropBuyStatus.Bought) : base(holderId, model, color1, color2, fuel , price, status, OwnerType.Personal)
        {
        }
        public PersonalVehicle(int holderId, PersonalBaseVehicle personal)
        {
            ID = personal.ID;
            OwnerID = holderId; OwnerID = holderId;
            ModelName = personal.ModelName;
            Number = personal.Number;
            Position = personal.Position;
            Rotation = personal.Rotation;
            VehCustomization = personal.VehCustomization;
            Fuel = personal.Fuel;
            Dirt = personal.Dirt;
            DirtClear = personal.DirtClear;
            EngineTorque = personal.EngineTorque;
            Price = personal.Price;
            EngineHealth = personal.EngineHealth;
            OwnerType = OwnerType.Personal;
            DoorBreak = personal.DoorBreak;
            Mileage = personal.Mileage;
            MileageOilChange = personal.MileageOilChange;
            MileageBrakePadsChange = personal.MileageBrakePadsChange;
            MileageTransmissionService = personal.MileageTransmissionService;
            IsDeath = personal.IsDeath;
            KeyNum = personal.KeyNum;
            PropertyBuyStatus = personal.PropertyBuyStatus;
            TradePoint = personal.TradePoint;

            VehicleManager.NewVehicleToHolder?.Invoke(this);
            VehicleManager.RemoveVehicleFromHolder?.Invoke(personal);
        }

        public override string GetHolderName()
        {
            if (Main.PlayerNames.ContainsKey(OwnerID))
                return Main.PlayerNames[OwnerID];
            else
                return "Unknown";
        }

        public override bool CanAccessVehicle(ExtPlayer player, AccessType access)
        {
            if (!player.IsLogged())
                return false;
            switch (access)
            {
                case AccessType.EngineChange:
                    if (TradePoint > 0)
                        return false;
                    if (player.Character.AdminLVL >= 3)
                        return true;
                    if (player.Character.MediaHelper > 0)
                    {
                        if (OwnerID != 529132 && access != AccessType.Inventory)
                            return true;
                    }
                    return VehicleKeyManager.CheckContainsKeyInPlayerInventory(player, ID, KeyNum);
                case AccessType.LockedDoor:
                    if (TradePoint > 0)
                        return true;
                    if (player.Character.AdminLVL >= 3)
                        return true;
                    if (player.Character.MediaHelper > 0)
                    {
                        if (OwnerID != 529132 && access != AccessType.Inventory)
                            return true;
                    }
                    return VehicleKeyManager.CheckContainsKeyInPlayerInventory(player, ID, KeyNum);
                case AccessType.OpenDoor:
                case AccessType.OpenTrunk:
                case AccessType.Inventory:
                    if (player.Character.AdminLVL >= 3)
                        return true;
                    if (player.Character.MediaHelper > 0)
                    {
                        if (OwnerID != 529132 && access != AccessType.Inventory)
                            return true;
                    }
                    return VehicleKeyManager.CheckContainsKeyInPlayerInventory(player, ID, KeyNum);
                case AccessType.Tuning:
                    return OwnerID == player.Character.UUID;
                case AccessType.SellDollars:
                    return OwnerID == player.Character.UUID && PropertyBuyStatus == PropBuyStatus.Bought && !Pledged;
                case AccessType.SellRouletteCar:
                    return OwnerID == player.Character.UUID && PropertyBuyStatus == PropBuyStatus.Roulette && !Pledged;
                case AccessType.SellZero:
                    return OwnerID == player.Character.UUID && PropertyBuyStatus == PropBuyStatus.Given && !Pledged;
                default:
                    return false;
            }
        }

        public override void DestroyVehicle()
        {
            Vehicle vehicle = Vehicle;
            if (vehicle != null)
            {
                vehicle.CustomDelete();
                Houses.HouseManager.GetHouse(OwnerID, (OwnerType)OwnerType, true)?.HouseGarage?.DeleteCar(ID);
                ExtPlayer player = Trigger.GetPlayerByUuid(OwnerID);
                if (player.IsLogged() && player.Character.ParkingSpawnCar == ID)
                {
                    player.Character.ParkingSpawnCar = -1;
                }
            }
        }
    }
}