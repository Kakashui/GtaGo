using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.VehicleSystem.Models;

//Disapproved by god himself

//Just use the API functions, you have nothing else to worry about

//Things to note
//More things like vehicle mods will be added in the next version

/* API FUNCTIONS:
public static void SetVehicleWindowState(ExtVehicle veh, WindowID window, WindowState state)
public static WindowState GetVehicleWindowState(ExtVehicle veh, WindowID window)
public static void SetVehicleWheelState(ExtVehicle veh, WheelID wheel, WheelState state)
public static WheelState GetVehicleWheelState(ExtVehicle veh, WheelID wheel)
public static void SetVehicleDirt(ExtVehicle veh, float dirt)
public static float GetVehicleDirt(ExtVehicle veh)
public static void SetDoorState(ExtVehicle veh, DoorID door, DoorState state)
public static DoorState GetDoorState(ExtVehicle veh, DoorID door)
public static void SetEngineState(ExtVehicle veh, bool status)
public static bool GetEngineState(ExtVehicle veh)
public static void SetLockStatus(ExtVehicle veh, bool status)
public static bool GetLockState(ExtVehicle veh)
*/

namespace Whistler.VehicleSystem
{
    internal class VehicleStreaming : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(VehicleStreaming));

        public static void SetFreezePosition(ExtVehicle vehicle, bool isFreezed)
        {
            if (vehicle == null) return;

            vehicle.IsFreezed = isFreezed;
            SafeTrigger.SetSharedData(vehicle, "veh:isFreeze", isFreezed);
        }

        public static void SetDoorState(ExtVehicle vehicle, DoorID door, DoorState state)
        {
            if (vehicle == null) return;

            int mask = 1 << (int)door;
            int currState = (vehicle.DoorState & mask) >> (int)door;
            if (currState != (int)state)
                vehicle.DoorState ^= mask;
            SafeTrigger.SetSharedData(vehicle, "veh:doorStatus", vehicle.DoorState);
        }

        public static DoorState GetDoorState(ExtVehicle vehicle, DoorID door)
        {
            if (vehicle == null) return DoorState.DoorClosed;

            int mask = 1 << (int)door;
            int currState = (vehicle.DoorState & mask) >> (int)door;
            return (DoorState)currState;
        }

        public static void SetEngineState(ExtVehicle vehicle, bool status)
        {
            if (vehicle == null) return;

            vehicle.Engine = status;
            SafeTrigger.SetSharedData(vehicle, "veh:engineStatus", status);
        }

        public static void SetVehicleFuel(ExtVehicle vehicle, int fuel)
        {
            if (vehicle == null) return;

            SafeTrigger.SetSharedData(vehicle, "PETROL", fuel);
            if (vehicle.Data == null) return;

            vehicle.Data.Fuel = fuel;
        }


        public static bool GetEngineState(ExtVehicle vehicle)
        {
            if (vehicle == null) return false;

            return vehicle.Engine;
        }

        public static void SetLockStatus(ExtVehicle vehicle, bool status)
        {
            if (vehicle == null) return;

            //if ((vehicle.Data.DoorBreak & VehicleConstants.CheckBrokenDoor) > 0) status = false;
            vehicle.Locked = status;
            vehicle.IsLocked = status;
            SafeTrigger.ClientEventInRange(vehicle.Position, 50, "VehStream_SetLockStatus", vehicle, status);
        }

        public static bool GetLockState(ExtVehicle vehicle)
        {
            if (vehicle == null) return true;

            return vehicle.IsLocked;
        }


        [RemoteEvent("VehStream_RadioChange")]
        public void VehStreamRadioChange(ExtPlayer client, ExtVehicle vehicle, short index)
        {
            try
            {
                if (vehicle == null || client.Vehicle != vehicle)
                    return;
                SafeTrigger.SetSharedData(vehicle, "vehradio", index);
            }
            catch (Exception e) { _logger.WriteError("VehStream_RadioChange: " + e.ToString()); }
        }

        [RemoteEvent("veh:setDirtLevel")]
        public void SetVehicleDirtLevel(ExtPlayer player, float dirt)
        {
            try
            {
                if (player.Vehicle == null) return;
                SetVehicleDirt(player.Vehicle as ExtVehicle, dirt);
            }
            catch (Exception e) { _logger.WriteError("VehStream_SetVehicleDirt: " + e.ToString()); }
        }

        [RemoteEvent("veh:setTurnSignal")]
        public static void VehStreamSetIndicatorLightsData(ExtPlayer player, ExtVehicle vehicle, int turnSignal)
        {
            if (vehicle == null) return;

            vehicle.TurnSignal = turnSignal;
            SafeTrigger.SetSharedData(vehicle, "veh:turnSignal", turnSignal);
        }
        public static void ChangeIndicatorLightsData(ExtVehicle vehicle)
        {
            if (vehicle == null) return;

            if (vehicle.TurnSignal > 0) vehicle.TurnSignal = 0;
            else vehicle.TurnSignal = 3;
            SafeTrigger.SetSharedData(vehicle, "veh:turnSignal", vehicle.TurnSignal);
        }

        public static void SetVehicleDirt(ExtVehicle vehicle, float dirt)
        {
            if (vehicle == null) return;

            SafeTrigger.SetSharedData(vehicle, "veh:dirtLevel", dirt);
            if (vehicle.Data == null) return;

            vehicle.Data.Dirt = dirt;
        }

        public static void SetVehicleDirtClear(ExtVehicle vehicle, int minute)
        {
            if (vehicle == null || vehicle.Data == null) return;

            vehicle.Data.DirtClear = DateTime.UtcNow.AddMinutes(minute);
            SafeTrigger.SetSharedData(vehicle, "veh:vehDirtClear", vehicle.Data.DirtClear.GetTotalSeconds(DateTimeKind.Utc));
        }

        public static float GetVehicleDirt(ExtVehicle vehicle)
        {
            if (vehicle == null || vehicle.Data == null) return 0.0f;

            return vehicle.Data.Dirt;
        }
    }
}
