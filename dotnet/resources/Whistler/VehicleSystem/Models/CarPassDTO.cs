using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.PriceSystem;
using Whistler.VehicleSystem.Models.Configs;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.VehicleSystem.Models
{
    class CarPassDTO
    {
        public string carName { get; set; }
        public double miliage { get; set; }
        public double generalState { get; set; }
        public int price { get; set; }
        public string owner { get; set; }
        public List<MechanicStats> mechState { get; set; }
        public CarPassDTO(PersonalBaseVehicle vehData)
        {

            int maxTurbo = 1;
            if (VehicleModels.VehicleModelNames.ContainsKey(NAPI.Util.GetHashKey(vehData.ModelName)))
                maxTurbo = 4;
            carName = vehData.ModelName;
            miliage = Math.Round(vehData.Mileage / 1000, 2);
            generalState = Math.Round(vehData.EngineHealth / 10, 1);
            price = PriceManager.GetPriceInDollars(TypePrice.Car, vehData.ModelName, 0);
            owner = vehData.GetHolderName().Replace("_", " ");
            mechState = new List<MechanicStats>
            {
                new MechanicStats
                {
                    name= "engine",
                    unicName= "engine",
                    currentLevel= vehData.VehCustomization.GetComponent(ModTypes.Engine) + 2,
                    maxLevel= 5,
                    typeLevel= GetStage(vehData.VehCustomization.GetComponent(ModTypes.Engine))
                },
                new MechanicStats
                {
                    name = "turbine",
                    unicName = "turbine",
                    currentLevel = vehData.VehCustomization.GetComponent(ModTypes.Turbo) + 2,
                    maxLevel = maxTurbo+1,
                    typeLevel = GetStage(vehData.VehCustomization.GetComponent(ModTypes.Turbo))
                },
                new MechanicStats
                {
                    name = "transmission",
                    unicName = "transmission",
                    currentLevel = vehData.VehCustomization.GetComponent(ModTypes.Transmission) + 2,
                    maxLevel = 4,
                    typeLevel = GetStage(vehData.VehCustomization.GetComponent(ModTypes.Transmission))
                },
                new MechanicStats
                {
                    name = "brakes",
                    unicName = "brakes",
                    currentLevel = vehData.VehCustomization.GetComponent(ModTypes.Brakes) + 2,
                    maxLevel = 4,
                    typeLevel = GetStage(vehData.VehCustomization.GetComponent(ModTypes.Brakes))
                }
            };
        }
        private static string GetStage(int stage)
        {
            if (stage <= -1)
                return "Stock";
            else
                return $"Stage {stage + 1}";
        }
    }
    class MechanicStats
    {
        public string name { get; set; }
        public string unicName { get; set; }
        public int currentLevel { get; set; }
        public int maxLevel { get; set; }
        public string typeLevel { get; set; }
        public MechanicStats()
        {

        }
    }


}
