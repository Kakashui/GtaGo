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
        public static string GetModelName(this Vehicle veh)
        {
            if (veh == null)
                return null;
            return VehicleManager.GetModelName(veh.Model);
        }

        public static VehicleGo GetVehicleGo(this Vehicle veh)
        {
            if (veh == null)
                return null;
            if (VehicleManager.GoVehicles.ContainsKey(veh))
                return VehicleManager.GoVehicles[veh];
            else
            {
                VehicleManager.GoVehicles.Add(veh, new VehicleGo(veh.Model));
                return VehicleManager.GoVehicles[veh];
            }
        }

        public static void CustomDelete(this Vehicle veh)
        {
            if (veh == null)
                return;
            if (veh.HasData("Deleted"))
                return;
            veh.SetData("Deleted", true);
            WhistlerTask.Run(() => 
            {
                veh.ResetData("Deleted");
                veh?.Delete();
            }, 100);
            
        }


    }
}
