using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Whistler.Common;
using Whistler.Helpers;
using Whistler.Houses;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.SDK;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.VehicleSystem
{
    class VehicleKeyManager : Script
    {
        private static int DistanceKeyRange = 50;
        public VehicleKeyManager()
        {
            InventoryService.OnUseCarKey += OnUseKey;
        }

        public static void OnUseKey(Player player, VehicleKey item)
        {
            if (item.Name != ItemNames.CarKey)
                return;
            var vehData = VehicleManager.GetVehicleBaseByUUID(item.VehicleId);
            if (vehData == null)
                return;
            if (vehData.OwnerType != OwnerType.Personal || (vehData as PersonalBaseVehicle).KeyNum != item.KeyNumber)
                return;
            OpenVehicleKey(player, item.VehicleId);
        }

        [Command("openkey")]
        public static void CommandOpenKey(Player player, int vehId)
        {
            if (!player.IsLogged())
                return;
            if (player.GetCharacter().AdminLVL < 10)
                return;
            var vehicle = VehicleManager.GetVehicleByRemoteId(vehId);
            if (vehicle != null)
                OpenVehicleKey(player, vehicle.GetVehicleGo().Data.ID);
        }

        [Command("openkeyid")]
        public static void CommandOpenKeyByIDKey(Player player, int idkey)
        {
            if (!player.IsLogged())
                return;
            if (player.GetCharacter().AdminLVL < 10)
                return;
            if (VehicleManager.Vehicles.ContainsKey(idkey))
                OpenVehicleKey(player, idkey);
        }

        public static void OpenVehicleKey(Player player, int vehicleID)
        {
            player.TriggerEvent("vehicle::key::openKey", vehicleID);
        }

        public static bool CheckContainsKeyInPlayerInventory(Player player, int vehicleID, int keyNumber)
        {

            if (player.GetInventory()?.Items.FirstOrDefault(item => item != null && item.Name == ItemNames.CarKey && item is VehicleKey && (item as VehicleKey).CheckTrueVehicle(vehicleID, keyNumber)) != null)
                return true;
            else
                return player.GetInventory()?.Items.FirstOrDefault(item => item != null && item.Name == ItemNames.KeyRing && item is KeyRing && (item as KeyRing).HasKey(vehicleID, keyNumber)) != null;
        }
        public static bool CheckContainsKeyInPlayerInventory(Player player, VehicleBase vehData)
        {
            if (!(vehData is PersonalBaseVehicle))
                return false;
            var personalVehData = vehData as PersonalBaseVehicle;
            return CheckContainsKeyInPlayerInventory(player, personalVehData.ID, personalVehData.KeyNum);
        }

        [RemoteEvent("vehicle::key::changeDoorState")]
        public static void ChangeDoorState(Player player, int vehicleID, int doorIndex)
        {
            if (!player.IsLogged())
                return;
            var vehicle = VehicleManager.GetVehicleByUUID(vehicleID);
            if (vehicle == null)
                return;
            if (vehicle.Dimension != player.Dimension || vehicle.Position.DistanceTo(player.Position) > DistanceKeyRange)
            {
                Notify.SendError(player, "veh:door:1");
                return;
            }
            if (player.GetCharacter().AdminLVL < 10 && !CheckContainsKeyInPlayerInventory(player, vehicle.GetVehicleGo().Data))
                return;
            if (!Enum.IsDefined(typeof(DoorID), doorIndex))
                return;
            VehicleManager.ChangeVehicleDoorOpen(player, vehicle, (DoorID)doorIndex);
        }

        [RemoteEvent("vehicle::key::enableGPS")]
        public void GiveGPS(Player player, int vehicleID)
        {
            if (!player.IsLogged())
                return;
            var vehicle = VehicleManager.GetVehicleByUUID(vehicleID);
            if (vehicle == null || vehicle.Dimension != 0)
                return;

            Trigger.ClientEvent(player, "createWaypoint", vehicle.Position.X, vehicle.Position.Y);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "House_98", 3000);
        }

        [RemoteEvent("vehicle::key::evacCar")]
        public void EvacCar(Player player, int vehicleID)
        {
            if (!player.IsLogged())
                return;
            var vData = VehicleManager.Vehicles.GetValueOrDefault(vehicleID);
            if (player.GetCharacter().AdminLVL < 10 && !CheckContainsKeyInPlayerInventory(player, vData))
                return;
            if (vData != null && vData.IsDeath == true)
                vData.IsDeath = false;
            GarageManager.SendVehicleIntoGarage(vehicleID);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "House_96", 3000);
        }

        [RemoteEvent("vehicle::key::changeEngine")]
        public void ChangeEngine(Player player, int vehicleID)
        {
            if (!player.IsLogged())
                return;
            var vehicle = VehicleManager.GetVehicleByUUID(vehicleID);
            if (vehicle == null)
                return;
            var vehGo = vehicle.GetVehicleGo();
            if (vehicle.Dimension != player.Dimension || vehicle.Position.DistanceTo(player.Position) > DistanceKeyRange)
            {
                Notify.SendError(player, "veh:door:1");
                return;
            }
            if (player.GetCharacter().AdminLVL < 10 && !CheckContainsKeyInPlayerInventory(player, vehGo.Data))
                return;
            if (vehGo.Data is PersonalBaseVehicle)
            {
                if ((vehGo.Data as PersonalBaseVehicle).TradePoint > 0)
                    return;
            }
            if (vehGo.Data.Fuel <= 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_69".Translate(), 3000);
                return;
            }
            VehicleStreaming.SetEngineState(vehicle, !VehicleStreaming.GetEngineState(vehicle));
        }

        [RemoteEvent("vehicle::key::lockCar")]
        public void LockCar(Player player, int vehicleID)
        {
            if (!player.IsLogged())
                return;
            var vehicle = VehicleManager.GetVehicleByUUID(vehicleID);
            if (vehicle == null)
                return;
            if (vehicle.Dimension != player.Dimension || vehicle.Position.DistanceTo(player.Position) > DistanceKeyRange)
            {
                Notify.SendError(player, "veh:door:1");
                return;
            }
            if (player.GetCharacter().AdminLVL < 10 && !CheckContainsKeyInPlayerInventory(player, vehicle.GetVehicleGo().Data))
                return;
            VehicleStreaming.SetLockStatus(vehicle, !VehicleStreaming.GetLockState(vehicle));
        }

        [RemoteEvent("vehicle::key::changeSignaling")]
        public void ChangeSignaling(Player player, int vehicleID)
        {
            if (!player.IsLogged())
                return;
            var vehicle = VehicleManager.GetVehicleByUUID(vehicleID);
            if (vehicle == null)
                return;
            if (vehicle.Dimension != player.Dimension || vehicle.Position.DistanceTo(player.Position) > DistanceKeyRange)
            {
                Notify.SendError(player, "veh:door:1");
                return;
            }
            if (player.GetCharacter().AdminLVL < 10 && !CheckContainsKeyInPlayerInventory(player, vehicle.GetVehicleGo().Data))
                return;
            VehicleStreaming.ChangeIndicatorLightsData(vehicle);
        }
    }
}
