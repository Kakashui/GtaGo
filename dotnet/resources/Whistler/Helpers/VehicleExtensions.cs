using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;

namespace Whistler.Helpers
{
    internal static class VehicleExtensions
    {
        public static string GetModelName(this ExtVehicle veh)
        {
            if (veh == null) return null;
            return VehicleManager.GetModelName(veh.Model);
        }

        public static void CustomDelete(this ExtVehicle veh)
        {
            CustomDelete(veh as Vehicle);
        }

        public static void CustomDelete(this Vehicle veh)
        {
            if (veh == null) return;
            if (veh.HasData("Deleted")) return;
            veh.SetData("Deleted", true);
            NAPI.Task.Run(() =>
            {
                ushort id = veh.Id;
                veh.ResetData("Deleted");
                veh?.Delete();
                if (Main.AllVehicles.ContainsKey(id)) Main.AllVehicles.Remove(id);
            }, 100);

        }

    }
}
