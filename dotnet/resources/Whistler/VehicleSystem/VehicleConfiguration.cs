using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.VehicleSystem.Models;

namespace Whistler.VehicleSystem
{
    public class VehicleConfiguration : Script
    {
        private static SortedDictionary<int, int> FuelTankByVehicleClass = new SortedDictionary<int, int>()
        {
            { -1, 100 },
            { 0, 120 }, // compacts
            { 1, 150 }, // Sedans
            { 2, 200 }, // SUVs
            { 3, 100 }, // Coupes
            { 4, 130 }, // Muscle
            { 5, 150 }, // Sports
            { 6, 100 }, // Sports (classic?)
            { 7, 150 }, // Super
            { 8, 100 }, // Motorcycles
            { 9, 200 }, // Off-Road
            { 10, 150 }, // Industrial
            { 11, 150 }, // Utility
            { 12, 150 }, // Vans
            { 13, 1   }, // cycles
            { 14, 300 }, // Boats
            { 15, 400 }, // Helicopters
            { 16, 500 }, // Planes
            { 17, 130 }, // Service
            { 18, 200 }, // Emergency
            { 19, 150 }, // Military
            { 20, 150 }, // Commercial
            { 21, 1000 }, // trains
        };
        private static SortedDictionary<int, int> FuelConsumptionByVehicleClass = new SortedDictionary<int, int>()
        {
            { -1, 1 },
            { 0, 1 }, // compacts
            { 1, 1 }, // Sedans
            { 2, 1 }, // SUVs
            { 3, 1 }, // Coupes
            { 4, 2 }, // Muscle
            { 5, 2 }, // Sports
            { 6, 2 }, // Sports (classic?)
            { 7, 2 }, // Super
            { 8, 2 }, // Motorcycles
            { 9, 2 }, // Off-Road
            { 10, 1 }, // Industrial
            { 11, 1 }, // Utility
            { 12, 1 }, // Vans
            { 13, 0 }, // Cycles
            { 14, 1 }, // Boats
            { 15, 2 }, // Helicopters
            { 16, 3 }, // Planes
            { 17, 2 }, // Service
            { 18, 1 }, // Emergency
            { 19, 1 }, // Military
            { 20, 1 }, // Commercial
            { 21, 3 }, // trains
        };

        private static readonly WhistlerLogger _logger = new WhistlerLogger(typeof(VehicleConfiguration));

        public VehicleConfiguration()
        {
        }

        public static VehConfig GetConfig(uint model)
        {
            if (!Models.Configs.VehicleConfigs.VehicleConfigList.ContainsKey(model))
            {
                return GetDefaultConfig(model);
            }
            return Models.Configs.VehicleConfigs.VehicleConfigList[model];
        }

        public static VehConfig GetConfig(string modelName)
        {
            var model = NAPI.Util.GetHashKey(modelName);
            return GetConfig(model);
        }
        private static VehConfig GetDefaultConfig(uint model)
        {
            var modelName = VehicleManager.GetModelName(model);
            int vehicleClass = GetClassFromModelName(modelName);

            var config = new VehConfig
            {
                Model = model,
                ModelName = modelName,
                DisplayName = modelName,
                Slots = 40,
                MaxWeight = 300,
                FuelConsumption = FuelConsumptionByVehicleClass[vehicleClass],
                MaxFuel = FuelTankByVehicleClass[vehicleClass],
                fuelType = 1
            };

            return config;
        }

        private static int GetClassFromModelName(string modelName)
        {
            try
            {
                var modelHash = (VehicleHash)NAPI.Util.GetHashKey(modelName);
                return NAPI.Vehicle.GetVehicleClass(modelHash);
            }
            catch
            {
                return -1;
            }
        }
    }
}
