using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.VehicleSystem.Models;

//Disapproved by god himself

//Just use the API functions, you have nothing else to worry about

//Things to note
//More things like vehicle mods will be added in the next version

/* API FUNCTIONS:
public static void SetVehicleWindowState(Vehicle veh, WindowID window, WindowState state)
public static WindowState GetVehicleWindowState(Vehicle veh, WindowID window)
public static void SetVehicleWheelState(Vehicle veh, WheelID wheel, WheelState state)
public static WheelState GetVehicleWheelState(Vehicle veh, WheelID wheel)
public static void SetVehicleDirt(Vehicle veh, float dirt)
public static float GetVehicleDirt(Vehicle veh)
public static void SetDoorState(Vehicle veh, DoorID door, DoorState state)
public static DoorState GetDoorState(Vehicle veh, DoorID door)
public static void SetEngineState(Vehicle veh, bool status)
public static bool GetEngineState(Vehicle veh)
public static void SetLockStatus(Vehicle veh, bool status)
public static bool GetLockState(Vehicle veh)
*/

namespace Whistler.VehicleSystem
{
    internal class VehicleStreaming : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(VehicleStreaming));

        public static void SetFreezePosition(Vehicle veh, bool isFreezed)
        {
            veh.GetVehicleGo().IsFreezed = isFreezed;
            veh.SetSharedData("veh:isFreeze", isFreezed);
        }

        public static void SetDoorState(Vehicle vehicle, DoorID door, DoorState state)
        {
            int mask = 1 << (int)door;
            int currState = (vehicle.GetVehicleGo().DoorState & mask) >> (int)door;
            if (currState != (int)state)
                vehicle.GetVehicleGo().DoorState = vehicle.GetVehicleGo().DoorState ^ mask;
            vehicle.SetSharedData("veh:doorStatus", vehicle.GetVehicleGo().DoorState);
        }

        public static DoorState GetDoorState(Vehicle vehicle, DoorID door)
        {

            int mask = 1 << (int)door;
            int currState = (vehicle.GetVehicleGo().DoorState & mask) >> (int)door;
            return (DoorState)currState;
        }

        public static void SetEngineState(Vehicle vehicle, bool status)
        {
            vehicle.GetVehicleGo().Engine = status;
            vehicle.SetSharedData("veh:engineStatus", status);
        }

        public static void SetVehicleFuel(Vehicle vehicle, int fuel)
        {
            vehicle.GetVehicleGo().Data.Fuel = fuel;
            vehicle.SetSharedData("PETROL", fuel);
        }

        public static bool GetEngineState(Vehicle vehicle)
        {
            return vehicle.GetVehicleGo().Engine;
        }

        public static void SetLockStatus(Vehicle vehicle, bool status)
        {
            VehicleGo vehGo = vehicle.GetVehicleGo();
           // if ((vehGo.Data.DoorBreak & VehicleConstants.CheckBrokenDoor) > 0)
           //     status = false;
            vehGo.Locked = status;
            vehicle.Locked = status;
            Trigger.ClientEventInRange(vehicle.Position, 50, "VehStream_SetLockStatus", vehicle, status);
        }

        public static bool GetLockState(Vehicle vehicle)
        {
            return vehicle.GetVehicleGo().Locked;
        }


        [RemoteEvent("VehStream_RadioChange")]
        public void VehStreamRadioChange(Player client, Vehicle vehicle, short index)
        {
            try
            {
                if (vehicle == null || client.Vehicle != vehicle)
                    return;
                vehicle.SetSharedData("vehradio", index);
            }
            catch (Exception e) { _logger.WriteError("VehStream_RadioChange: " + e.ToString()); }
        }

        [RemoteEvent("veh:setDirtLevel")]
        public void SetVehicleDirtLevel(Player player, float dirt)
        {
            try
            {
                if (player.Vehicle == null)
                    return;
                SetVehicleDirt(player.Vehicle, dirt);
            }
            catch (Exception e) { _logger.WriteError("VehStream_SetVehicleDirt: " + e.ToString()); }
        }

        [RemoteEvent("veh:setTurnSignal")]
        public static void VehStreamSetIndicatorLightsData(Player player, Vehicle vehicle, int turnSignal)
        {
            if (vehicle != null)
            {
                vehicle.GetVehicleGo().TurnSignal = turnSignal;
                vehicle.SetSharedData("veh:turnSignal", turnSignal);
            }
        }
        public static void ChangeIndicatorLightsData(Vehicle vehicle)
        {
            if (vehicle != null)
            {
                var vehGo = vehicle.GetVehicleGo();
                if (vehGo.TurnSignal > 0)
                    vehGo.TurnSignal = 0;
                else
                    vehGo.TurnSignal = 3;
                vehicle.SetSharedData("veh:turnSignal", vehGo.TurnSignal);
            }
        }

        public static void SetVehicleDirt(Vehicle vehicle, float dirt)
        {
            vehicle.GetVehicleGo().Data.Dirt = dirt;
            vehicle.SetSharedData("veh:dirtLevel", dirt);
        }

        public static void SetVehicleDirtClear(Vehicle vehicle, int minute)
        {
            vehicle.GetVehicleGo().Data.DirtClear = DateTime.UtcNow.AddMinutes(minute);
            vehicle.SetSharedData("veh:vehDirtClear", vehicle.GetVehicleGo().Data.DirtClear.GetTotalSeconds(DateTimeKind.Utc));
        }

        public static float GetVehicleDirt(Vehicle veh)
        {
            return veh.GetVehicleGo().Data.Dirt;
        }
    }
}
