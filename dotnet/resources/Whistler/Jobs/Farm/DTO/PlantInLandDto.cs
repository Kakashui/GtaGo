using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.Inventory.Enums;
using Whistler.Jobs.Farm.Models;

namespace Whistler.Jobs.Farm.DTO
{
    class PlantInLandDto
    {
        public int FarmId { get; set; }
        public int PointId { get; set; }
        public int UUID { get; set; }
        public int PlantName { get; set; }
        public int PlantingTime { get; set; }
        public int WateringTime { get; set; }
        public int Fertilizer { get; set; }
        public bool BuffedByPet { get; set; }
        public PlantInLandDto(PlantInLand plantInLand)
        {
            FarmId = plantInLand.FarmId;
            PointId = plantInLand.PointId;
            UUID = plantInLand.UUID;
            PlantName = (int)plantInLand.PlantName;
            PlantingTime = plantInLand.PlantingTime.GetTotalSeconds(DateTimeKind.Utc);
            WateringTime = plantInLand.WateringTime.GetTotalSeconds(DateTimeKind.Utc);
            Fertilizer = (int)plantInLand.Fertilizer;
            BuffedByPet = plantInLand.BuffedByPet;
        }
    }
}
