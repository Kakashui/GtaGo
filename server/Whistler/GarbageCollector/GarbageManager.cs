using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.VehicleSystem.Models;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.GarbageCollector
{
    class GarbageManager : Script
    {
        static WhistlerLogger _logger = new WhistlerLogger(typeof(GarbageManager));
        const int _checkTime = 60000;
        public const string DATA_VEH_RESPAWN = "RESPAWN_VEH";

        [ServerEvent(Event.ResourceStart)]
        public void StartGarbageCollector()
        {
            Timers.Start(_checkTime, CheckVehicleList);
        }

        private void CheckVehicleList()
        {
            foreach (var vehicle in NAPI.Pools.GetAllVehicles())
            {
                try
                {
                    if (vehicle.HasData(DATA_VEH_RESPAWN))
                    {
                        DateTime respawnData = vehicle.GetData<DateTime>(DATA_VEH_RESPAWN);
                        if (respawnData < DateTime.Now)
                        {
                            RespawnVehicle(vehicle);
                            vehicle.ResetData(DATA_VEH_RESPAWN);
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.WriteError($"CheckVehicleList:\n{e}");
                }
            }
        }

        private static void RespawnVehicle(Vehicle vehicle)
        {
            vehicle.GetVehicleGo().Data.RespawnVehicle();
        }

        internal static void Add(Vehicle vehicle, int min)
        {
            vehicle.SetData(DATA_VEH_RESPAWN, DateTime.Now.AddMinutes(min));
        }
        internal static void Remove(Vehicle vehicle)
        {
            if (vehicle.HasData(DATA_VEH_RESPAWN)) vehicle.ResetData(DATA_VEH_RESPAWN);
        }
        internal static bool InGarbage(Vehicle vehicle)
        {
            return vehicle.HasData(DATA_VEH_RESPAWN);
        }
    }
}
