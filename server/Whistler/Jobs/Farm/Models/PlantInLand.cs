using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Enums;
using Whistler.Jobs.Farm.Configs.Models;

namespace Whistler.Jobs.Farm.Models
{
    class PlantInLand
    {
        public PlantInLand(int farmId, int pointId, int uuid, ItemNames plantName)
        {
            FarmId = farmId;
            PointId = pointId;
            UUID = uuid;
            PlantName = plantName;
            PlantingTime = DateTime.UtcNow;
            WateringTime = DateTime.UtcNow.AddDays(-1);
            Fertilizer = FertilizerType.None;
        }

        public int FarmId { get; set; }
        public int PointId { get; set; }
        public int UUID { get; set; }
        public ItemNames PlantName { get; set; }
        public DateTime PlantingTime { get; set; }
        public DateTime WateringTime { get; set; }
        public FertilizerType Fertilizer { get; set; }

        public SeedingPlaceConfig ConfigPlace
        {
            get
            {
                return Configs.FarmConfigs._configsByFarmId[FarmId].FirstOrDefault(item => item.ID == PointId);
            }
        }
        public BonusConfig PitConfig
        {
            get
            {
                return Configs.FarmConfigs.PitConfigsList[ConfigPlace.PitType];
            }
        }
        public BonusConfig FertilizerConfig
        {
            get
            {
                return Configs.FarmConfigs.FertilizerConfigsList[Fertilizer];
            }
        }
        public PlantConfig ConfigPlant
        {
            get
            {
                return Configs.FarmConfigs.PlantConfigsList[PlantName];
            }
        }

        public static PlantInLand CreatePlant(int farmId, int pointId, int uuid, ItemNames plantName)
        {
            if (!Configs.FarmConfigs._configsByFarmId.ContainsKey(farmId))
                return null;
            if (Configs.FarmConfigs._configsByFarmId[farmId].FirstOrDefault(item => item.ID == pointId) == null)
                return null;
            if (!Configs.FarmConfigs.PlantConfigsList.ContainsKey(plantName))
                return null;

            return new PlantInLand(farmId, pointId, uuid, plantName);
        }

        public PlantStatus GetStatusPlant()
        {

            var timePassed = (DateTime.UtcNow - PlantingTime).TotalSeconds;
            var isWatering = WateringTime > PlantingTime;
            var coeffRipeningTime = 1 - PitConfig.TimeCoeff - FertilizerConfig.TimeCoeff;
            var currentRipeningTime = ConfigPlant.RipeningTime * coeffRipeningTime;
            var currentStage = timePassed / currentRipeningTime; // получаем стадию растения
            if ((!isWatering && timePassed > ConfigPlant.SecondBeforeWatering) || (timePassed > currentRipeningTime + ConfigPlant.WitheringTime))
                return PlantStatus.Withered;
            if (currentStage > 1)
                return PlantStatus.Ripe;
            return PlantStatus.Growing;
        }
    }
}
