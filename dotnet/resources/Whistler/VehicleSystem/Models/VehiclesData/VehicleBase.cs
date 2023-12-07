using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Whistler.Common;
using Whistler.Common.Interfaces;
using Whistler.Entities;
using Whistler.GUI.Documents.Enums;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.Jobs.HomeRobbery.Models;
using Whistler.PersonalEvents.Contracts;
using Whistler.PersonalEvents.Contracts.Models;
using Whistler.SDK;

namespace Whistler.VehicleSystem.Models.VehiclesData
{
    abstract public class VehicleBase
    {
        public int ID { get; set; }
        public int OwnerID { get; set; }
        public string ModelName { get; set; }
        public string Number { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public CustomizationVehicleModel VehCustomization { get; set; }
        public int Fuel { get; set; } = 100;
        public float Dirt { get; set; } = 0;
        public DateTime DirtClear { get; set; } = DateTime.UtcNow;
        public float EnginePower { get; set; }
        public float EngineTorque { get; set; }
        public OwnerType OwnerType { get; set; }
        public int Price { get; set; } = 0;
        public int DoorBreak { get; set; } = 0;
        public int InventoryId { get; set; } = -1;
        public bool IsDeath { get; set; } = false;


        private List<VehicleItemBase> VehicleItems = new List<VehicleItemBase>();

        public string DisplayName { get { return VehicleConfiguration.GetConfig(ModelName).DisplayName; } }
        public ExtVehicle Vehicle { get { return SafeTrigger.GetVehicleByDataId(ID); } }
        public VehConfig Config => VehicleConfiguration.GetConfig(ModelName);

        public VehicleBase(DataRow row)
        {
            ID = Convert.ToInt32(row["idkey"]);
            InventoryId = Convert.ToInt32(row["inventoryId"]);
            OwnerID = Convert.ToInt32(row["holderuuid"]);
            Number = Convert.ToString(row["number"]);
            ModelName = Convert.ToString(row["model"]).ToLower();
            switch (ModelName)
            {
                case "intityxf":
                    ModelName = "entityxf";
                    break;
                case "winsdor":
                    ModelName = "windsor";
                    break;
                case "santinel":
                    ModelName = "sentinel";
                    break;
                case "santinel2":
                    ModelName = "sentinel2";
                    break;
                case "santinel3":
                    ModelName = "sentinel3";
                    break;
                case "bista":
                    ModelName = "blista";
                    break;
                case "ztype1":
                    ModelName = "ztype";
                    break;
                default:
                    break;
            }
            VehCustomization = JsonConvert.DeserializeObject<CustomizationVehicleModel>(row["componentsnew"].ToString());
            Position = JsonConvert.DeserializeObject<Vector3>(row["position"].ToString());
            Rotation = JsonConvert.DeserializeObject<Vector3>(row["rotation"].ToString());
            EnginePower = Convert.ToSingle(row["power"]);
            EngineTorque = Convert.ToSingle(row["torque"]);

            if (InventoryId == -1) {
                CreateNewInventory();
                MySQL.Query("UPDATE `vehicles` SET `inventoryId`=@prop0 WHERE `idkey`=@prop1", InventoryId, ID);
            }
            else
            {
                //fix инвентарей и размеров инвентарей авто
                var inventory = InventoryService.GetById(InventoryId);
                if (inventory == null)
                {
                   CreateNewInventory();
                   MySQL.Query("UPDATE `vehicles` SET `inventoryId`=@prop0 WHERE `idkey`=@prop1", InventoryId, ID);
                }
                else
                {
                   var config = VehicleConfiguration.GetConfig(ModelName);
                   if (inventory.MaxWeight != config.MaxWeight)
                       inventory.ChangeMaxWeight(config.MaxWeight);
                   if (inventory.Size != config.Slots)
                       inventory.ChangeSlotsCount(config.Slots);
                }
            }
        }
        public VehicleBase()
        {
            Number = "ADMIN";
            ModelName = null;
            VehCustomization = new CustomizationVehicleModel();
            Position = null;
            Rotation = null;
            Dirt = 0;
            DirtClear = DateTime.UtcNow;
            EnginePower = 0;
            EngineTorque = 1;
            OwnerType = OwnerType.Temporary;

        }

        public void CreateNewInventory()
        {
            var config = VehicleConfiguration.GetConfig(ModelName);
            var inventory = new InventoryModel(config.MaxWeight, config.Slots, InventoryTypes.Vehicle);
            InventoryId = inventory.Id;
        }

        public ExtVehicle Spawn(Vector3 position, Vector3 rotation, uint dimension)
        {
            ExtVehicle vehicle = SpawnCar(position, rotation, dimension);
            if (vehicle == null) throw new Exception("SpawnCar: vehicle is returned null");
            VehicleManager.ApplyCustomization(vehicle);
            SafeTrigger.SetSharedData(vehicle, "HOLDERNAME", GetHolderName());
            SafeTrigger.SetSharedData(vehicle, "veh:dirtLevel", Dirt);
            SafeTrigger.SetSharedData(vehicle, "veh:vehDirtClear", DirtClear.GetTotalSeconds(DateTimeKind.Utc));
            return vehicle;
        }
        public ExtVehicle Spawn()
        {
            ExtVehicle vehicle = SpawnCar();
            return vehicle;
        }
        abstract protected ExtVehicle SpawnCar(Vector3 position, Vector3 rotation, uint dimension);
        abstract protected ExtVehicle SpawnCar();
        abstract public bool CanAccessVehicle(ExtPlayer player, AccessType access);
        abstract public string GetHolderName();
        abstract public void Save();
        abstract public void VehicleDeath(ExtVehicle vehicle);
        /// <summary>
        /// Удаляет автомобиль из БД
        /// </summary>
        /// <param name="vehicle"></param>
        abstract public void DeleteVehicle(ExtVehicle vehicle = null);
        /// <summary>
        /// Удаляет сущность автомобиля
        /// </summary>
        abstract public void DestroyVehicle();
        abstract public void RespawnVehicle();

        public void PlayerEnterVehicleBase(ExtPlayer player, ExtVehicle vehicle, sbyte seatid)
        {

            if (!player.IsLogged()) return;
            if (player.Character.Cuffed && player.VehicleSeat == VehicleConstants.DriverSeat)
            {
                VehicleManager.WarpPlayerOutOfVehicle(player);
                return;
            }
            if (player.Character.Follower != null)
            {
                VehicleManager.WarpPlayerOutOfVehicle(player);
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_54", 3000);
                return;
            }

            SafeTrigger.ClientEvent(player, "VehStream_PlayerEnterVehicle", vehicle, seatid, vehicle.Engine);
            SafeTrigger.ClientEvent(player, "veh:offRadio");

            if (!vehicle.AllOccupants.ContainsKey(seatid)) vehicle.AllOccupants.Add(seatid, player);
            else vehicle.AllOccupants[seatid] = player;

            if (seatid == VehicleConstants.DriverSeat)
            {
                player.TriggerCefEvent("speedometer/setCurFuel", Fuel);
                if (vehicle.RequiredLicense != null && !player.CheckLic((LicenseName)vehicle.RequiredLicense)) SafeTrigger.ClientEvent(player, "vehicleControl", true);
                else SafeTrigger.ClientEvent(player, "vehicleControl", false);
            }

            PlayerEnterVehicle(player, vehicle, seatid);
        }
        abstract protected void PlayerEnterVehicle(ExtPlayer player, ExtVehicle vehicle, sbyte seatid);
        public void PlayerExitVehicleBase(ExtPlayer player, ExtVehicle vehicle)
        {
            PlayerExitVehicle(player, vehicle);
        }
        abstract protected void PlayerExitVehicle(ExtPlayer player, ExtVehicle vehicle);

        internal int GiveAbstractItem(VehicleItemBase vehicleItemBase)
        {
            VehicleItems = VehicleItems.Where(item => item.IsActual()).ToList();
            int currMass = VehicleItems.Sum(item => item.GetWeight());
            InventoryModel inventory = InventoryService.GetById(InventoryId);
            if (currMass + vehicleItemBase.GetWeight() > inventory.MaxWeight) return 0;

            VehicleItemBase currItem = VehicleItems.FirstOrDefault(item => item.IsEqual(vehicleItemBase));
            if (currItem != null) currItem.Count++;
            else VehicleItems.Add(vehicleItemBase);
            return 1;
        }
        internal int GetAbstractItem(ContractProgress contractProgress, AbstractItemNames abstractItem, DateTime expirationDate, int maxCount)
        {
            var currItem = VehicleItems.FirstOrDefault(item => (item as ContractItem)?.IsEqual(contractProgress, abstractItem, expirationDate) ?? false);
            return GetAbstractItem(currItem, maxCount);
        }
        internal int GetRobberyAbstractItem(int maxCount)
        {
            var currItem = VehicleItems.FirstOrDefault(item => (item as RobberyItem)?.IsEqual(new RobberyItem(AbstractItemNames.Robbery, 0)) ?? false);
            return GetAbstractItem(currItem, maxCount);
        }

        private int GetAbstractItem(VehicleItemBase vehicleItemBase, int maxCount)
        {
            if (vehicleItemBase != null)
            {
                if (vehicleItemBase.Count <= maxCount)
                {
                    VehicleItems.Remove(vehicleItemBase);
                    return vehicleItemBase.Count;
                }
                else
                {
                    vehicleItemBase.Count -= maxCount;
                    return maxCount;
                }
            }
            return 0;
        }
    }
}

