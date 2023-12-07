using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.VehicleSystem
{
    class VehicleCommands : Script
    {
        [Command("savecarpos")]
        public static void SaveCarPosition(Player player)
        {
            if (!Group.CanUseAdminCommand(player, "savecarpos")) return;
            if (!player.IsLogged())
                return;
            Vehicle vehicle = player.Vehicle;
            if (vehicle == null)
                return;
            var vehData = vehicle.GetVehicleGo().Data;
            if (vehData is PersonalBaseVehicle)
                (vehData as PersonalBaseVehicle).SetSavePosition(vehicle.Position, vehicle.Rotation);
        }
        [Command("delcarpos")]
        public static void DeleteCarPosition(Player player)
        {
            if (!Group.CanUseAdminCommand(player, "delcarpos")) return;
            if (!player.IsLogged())
                return;
            Vehicle vehicle = player.Vehicle;
            if (vehicle == null)
                return;
            var vehData = vehicle.GetVehicleGo().Data;
            if (vehData is PersonalBaseVehicle)
                (vehData as PersonalBaseVehicle).SetSavePosition(null, null);
        }

        [Command("copycust")]
        public static void CMD_CopyCustomization(PlayerGo player, int vehId, int handling = 0)
        {
            if (!player.IsLogged())
                return;
            if (!Group.CanUseAdminCommand(player, "copycust")) return;
            if (!player.IsInVehicle || player.Vehicle == null) return;
            Vehicle targetVehicle = NAPI.Pools.GetAllVehicles().FirstOrDefault(item => item.Value == vehId);
            if (targetVehicle == null)
                return;
            VehicleManager.CopyCustomization(player.Vehicle, targetVehicle, handling == 1);
            GameLog.Admin(player.Name, $"copycust({player.Vehicle.GetVehicleGo().Data.ID},{targetVehicle.GetVehicleGo().Data.ID})", "");
        }

        [Command("copyhandl")]
        public static void CMD_CopyHandling(PlayerGo player, int vehId)
        {
            if (!player.IsLogged())
                return;
            if (!Group.CanUseAdminCommand(player, "copyhandl")) return;
            if (!player.IsInVehicle || player.Vehicle == null) return;
            Vehicle targetVehicle = NAPI.Pools.GetAllVehicles().FirstOrDefault(item => item.Value == vehId);
            if (targetVehicle == null)
                return;
            VehicleManager.CopyHandling(player.Vehicle, targetVehicle);
            GameLog.Admin(player.Name, $"copyhandl({player.Vehicle.GetVehicleGo().Data.ID},{targetVehicle.GetVehicleGo().Data.ID})", "");
        }


        [Command("clearhandl")]
        public static void CMD_ClearHandling(PlayerGo player)
        {
            if (!player.IsLogged())
                return;
            if (player.Character.UUID != 529132) return;
            if (!player.IsInVehicle || player.Vehicle == null) return;
            VehicleManager.ClearHandling(player.Vehicle);
        }

        [Command("sethandl")]
        public static void SetHandling(PlayerGo player, string key, object value)
        {
            if (!player.IsLogged())
                return;
            if (player.Character.UUID != 529132) return;
            Vehicle vehicle = player.Vehicle;
            if (vehicle == null)
                return;
            player.TriggerEvent("veh:setHandling", key, value);
        }

        [Command("checkhandl")]
        public static void CheckHandling(PlayerGo player, string key)
        {
            if (!player.IsLogged())
                return;
            if (player.Character.UUID != 529132) return;
            Vehicle vehicle = player.Vehicle;
            if (vehicle == null)
                return;
            player.TriggerEvent("veh:checkHandling", key);
        }

        [Command("checkhandls")]
        public static void CheckHandlingEnumKey(PlayerGo player, int key)
        {
            if (!player.IsLogged())
                return;
            if (player.Character.UUID != 529132) return;
            Vehicle vehicle = player.Vehicle;
            if (vehicle == null)
                return;
            if (Enum.IsDefined(typeof(HandlingKeys), key))
                player.TriggerEvent("veh:checkHandling", ((HandlingKeys)key).ToString());
        }

        [Command("sethandls")]
        public static void SetHandlingSharedData(PlayerGo player, int key, object value)
        {
            if (!player.IsLogged())
                return;
            if (player.Character.UUID != 529132) return;
            Vehicle vehicle = player.Vehicle;
            if (vehicle == null)
                return;
            VehicleCustomization.SetHandlingMod(vehicle, (HandlingKeys)key, value);
            //player.TriggerEvent("veh:setHandling", key, value);
        }
    }
}
