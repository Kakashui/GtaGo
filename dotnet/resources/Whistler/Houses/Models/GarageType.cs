using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.Houses.Models
{
    class GarageType
    {
        public GarageCoordType GarageCoordID { get; set; }
        public string Description { get; set; }
        public GarageIPLType IPLType { get; set; }
        public int MaxCars { get; }
        public int Cost { get; set; }
        public GarageCoordsModel CoordsModel { get { return Configs.HouseConfigs.GarageCoordModels[GarageCoordID]; } }

        public GarageType(GarageCoordType garageCoordID, GarageIPLType iplType, int maxCars, int cost, string description)
        {
            GarageCoordID = garageCoordID;
            IPLType = iplType;
            MaxCars = maxCars;
            Cost = cost;
            Description = description;
        }
    }
}
