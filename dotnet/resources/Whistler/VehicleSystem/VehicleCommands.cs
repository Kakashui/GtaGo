using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.VehicleSystem.Models;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.VehicleSystem
{
    class VehicleCommands : Script
    {
        [Command("savecarpos")]
        public static void SaveCarPosition(ExtPlayer player)
        {
            if (!Group.CanUseAdminCommand(player, "savecarpos")) return;
            if (!player.IsLogged())
                return;
            ExtVehicle vehicle = player.Vehicle as ExtVehicle;
            if (vehicle == null)
                return;
            var vehData = vehicle.Data;
            if (vehData is PersonalBaseVehicle)
                (vehData as PersonalBaseVehicle).SetSavePosition(vehicle.Position, vehicle.Rotation);
        }
        [Command("delcarpos")]
        public static void DeleteCarPosition(ExtPlayer player)
        {
            if (!Group.CanUseAdminCommand(player, "delcarpos")) return;
            if (!player.IsLogged())
                return;
            ExtVehicle vehicle = player.Vehicle as ExtVehicle;
            if (vehicle == null)
                return;
            var vehData = vehicle.Data;
            if (vehData is PersonalBaseVehicle)
                (vehData as PersonalBaseVehicle).SetSavePosition(null, null);
        }

        [Command("copycust")]
        public static void CMD_CopyCustomization(ExtPlayer player, int vehId, int handling = 0)
        {
            if (!player.IsLogged())
                return;
            if (!Group.CanUseAdminCommand(player, "copycust")) return;
            if (!player.IsInVehicle || player.Vehicle == null) return;
            ExtVehicle targetVehicle = Trigger.GetVehicleById(vehId);
            if (targetVehicle == null) return;

            ExtVehicle playerVehicle = player.Vehicle as ExtVehicle;
            VehicleManager.CopyCustomization(playerVehicle, targetVehicle, handling == 1);
            GameLog.Admin(player.Name, $"copycust({playerVehicle.Data.ID},{targetVehicle.Data.ID})", "");
        }

        [Command("copyhandl")]
        public static void CMD_CopyHandling(ExtPlayer player, int vehId)
        {
            if (!player.IsLogged())
                return;
            if (!Group.CanUseAdminCommand(player, "copyhandl")) return;
            if (!player.IsInVehicle || player.Vehicle == null) return;
            ExtVehicle targetVehicle = Trigger.GetVehicleById(vehId);
            if (targetVehicle == null) return;

            ExtVehicle playerVehicle = player.Vehicle as ExtVehicle;
            VehicleManager.CopyHandling(playerVehicle, targetVehicle);
            GameLog.Admin(player.Name, $"copyhandl({playerVehicle.Data.ID},{targetVehicle.Data.ID})", "");
        }


        [Command("clearhandl")]
        public static void CMD_ClearHandling(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            if (player.Character.UUID != 529132) return;
            if (!player.IsInVehicle || player.Vehicle == null) return;
            VehicleManager.ClearHandling(player.Vehicle as ExtVehicle);
        }

        [Command("sethandl")]
        public static void SetHandling(ExtPlayer player, string key, object value)
        {
            if (!player.IsLogged())
                return;
            if (player.Character.UUID != 529132) return;
            ExtVehicle vehicle = player.Vehicle as ExtVehicle;
            if (vehicle == null)
                return;
            SafeTrigger.ClientEvent(player,"veh:setHandling", key, value);
        }

        [Command("checkhandl")]
        public static void CheckHandling(ExtPlayer player, string key)
        {
            if (!player.IsLogged())
                return;
            if (player.Character.UUID != 529132) return;
            ExtVehicle vehicle = player.Vehicle as ExtVehicle;
            if (vehicle == null)
                return;
            SafeTrigger.ClientEvent(player,"veh:checkHandling", key);
        }

        [Command("checkhandls")]
        public static void CheckHandlingEnumKey(ExtPlayer player, int key)
        {
            if (!player.IsLogged())
                return;
            if (player.Character.UUID != 529132) return;
            ExtVehicle vehicle = player.Vehicle as ExtVehicle;
            if (vehicle == null)
                return;
            if (Enum.IsDefined(typeof(HandlingKeys), key))
                SafeTrigger.ClientEvent(player,"veh:checkHandling", ((HandlingKeys)key).ToString());
        }

        [Command("sethandls")]
        public static void SetHandlingSharedData(ExtPlayer player, int key, object value)
        {
            if (!player.IsLogged())
                return;
            if (player.Character.UUID != 529132) return;
            ExtVehicle vehicle = player.Vehicle as ExtVehicle;
            if (vehicle == null)
                return;
            VehicleCustomization.SetHandlingMod(vehicle, (HandlingKeys)key, value);
            //SafeTrigger.ClientEvent(player,"veh:setHandling", key, value);
        }
    }
}
