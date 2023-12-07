using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Whistler.Common;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Houses.Furnitures;
using Whistler.MoneySystem;
using Whistler.NewDonateShop;
using Whistler.VehicleSystem;

namespace Whistler.Houses.DTOs
{
    internal class HouseOwnerMenuDTO
    {
        [JsonProperty("houseId")]
        public int HouseId { get; set; }

        [JsonProperty("houseType")]
        public int HouseType { get; set; }
        
        [JsonProperty("houseCost")]
        public int Cost { get; set; }

        [JsonProperty("houseLocked")]
        public bool HouseLocked { get; set; }

        [JsonProperty("paidBefore")]
        public string PaidBeforePretified { get; set; }

        [JsonProperty("currentInteriorId")]
        public int CurrentInteriorId { get; set; }

        [JsonProperty("totalVehicles")]
        public int TotalVehicles { get; set; }

        [JsonProperty("rentCost")]
        public int RentCost { get; set; }

        [JsonProperty("furnitureList")]
        public List<HouseMenuFurnitureItem> AvailableFurnitures { get; set; }

        [JsonProperty("interiorList")]
        public List<HouseMenuInteriorItem> AvailableInteriors { get; set; }

        [JsonProperty("occupiers")]
        public List<HouseMenuOccupierItem> Occupiers { get; set; }

        [JsonProperty("currentGarage")]
        public HouseMenuGarageItem CurrentGarage { get; set; }

        [JsonProperty("typeOfGarages")] 
        public List<HouseMenuGarageItem> AvailableGarages { get; set; }

        public HouseOwnerMenuDTO(ExtPlayer player, House house, List<Roommate> roommates, bool owner)
        {
            var garageSettings = house.HouseGarage.GarageConfig;
            int tax = house.HouseTax;
            var paydaysWithoutouseLoss = house.BankModel.Balance / (tax <= 0 ? 1 : tax);
            var paidBeforeDate = DateTime.Now.AddHours(paydaysWithoutouseLoss);

            List<HouseMenuOccupierItem> occupiers = new List<HouseMenuOccupierItem>();
            if (!owner)
            {
                int uuid = player.Character.UUID;
                Roommate myData = roommates.FirstOrDefault(x => x.CharacterUUID == uuid);
                if (myData != null) occupiers.Add(new HouseMenuOccupierItem
                {
                    Name = Main.PlayerNames.GetValueOrDefault(uuid, player.Session.Name),
                    GarageAccess = myData.HasWardrobeAccess,
                    SafeAccess = myData.HasSafeAccess,
                    UUID = uuid,
                });
            }
            else occupiers = roommates.Select(item => new HouseMenuOccupierItem
            {
                Name = Main.PlayerNames.GetValueOrDefault(item.CharacterUUID, "Unknown"),
                GarageAccess = item.HasWardrobeAccess,//todo: ispravit huinu
                SafeAccess = item.HasSafeAccess,
                UUID = item.CharacterUUID
            }).ToList();

            Occupiers = occupiers;

            AvailableFurnitures = new List<HouseMenuFurnitureItem>();

            if (owner && house.Furnitures != null)
            {
                AvailableFurnitures = house.Furnitures
                    .Where(item => item.ModelName != null)
                    .Select(furniture => new HouseMenuFurnitureItem
                    {
                        Id = house.Furnitures.IndexOf(furniture),
                        Key = furniture.ModelName,
                        Name = FurnitureSettings.AllAvailableFurnitures[furniture.ModelName]?.Name,
                        Placed = furniture.Installed
                    }).ToList();
            }

            Cost = owner ? Convert.ToInt32(house.Price * 0.5) : - 1;
            AvailableGarages = owner ? GetAvailableGarages() : new List<HouseMenuGarageItem>();
            CurrentGarage = !owner ? null : new HouseMenuGarageItem
            {
                Cost = garageSettings.Cost,
                Description = garageSettings.Description,
                Type = house.HouseGarage.Type,
                TotalPlaces = garageSettings.MaxCars
            };
            HouseId = house.ID;
            HouseLocked = house.Locked;
            HouseType = (int)house.OwnerType;
            RentCost = house.RentCost;
            TotalVehicles = owner ? VehicleManager.getAllHolderVehicles(house.OwnerID, house.OwnerType).Count : -1;
            CurrentInteriorId = (int)house.Type;
            PaidBeforePretified = paidBeforeDate.ToShortDateString();
        }
        internal class HouseMenuFurnitureItem
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("placed")]
            public bool Placed { get; set; }

            [JsonProperty("key")]
            public string Key { get; set; }
        }
        
        internal class HouseMenuInteriorItem
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("bought")]
            public bool Bought { get; set; }

            [JsonProperty("key")]
            public int Key { get; set; }
        }
        
        internal class HouseMenuOccupierItem
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("garageAccess")]
            public bool GarageAccess { get; set; }

            [JsonProperty("safeAccess")]
            public bool SafeAccess { get; set; }

            [JsonProperty("uuid")]
            public int UUID { get; set; }
        }
        
        internal class HouseMenuGarageItem
        {
            [JsonProperty("type")]
            public int Type { get; set; }

            [JsonProperty("desc")]
            public string Description { get; set; }

            [JsonProperty("placesCount")]
            public int TotalPlaces { get; set; }

            [JsonProperty("cost")]
            public int Cost { get; set; }
        }

        private static List<HouseMenuGarageItem> GetAvailableGarages()
        {
            List<HouseMenuGarageItem> garageList = new List<HouseMenuGarageItem>();

            HouseMenuGarageItem garageData;
            Models.GarageType garageConfigData;
            foreach (KeyValuePair<int, Models.GarageType> garageConfig in Configs.HouseConfigs.GarageTypes)
            {
                garageConfigData = garageConfig.Value;
                if (garageConfigData.GarageCoordID == GarageCoordType.G30place) continue;
                if (garageConfigData.GarageCoordID == GarageCoordType.G50Place) continue;

                garageData = new HouseMenuGarageItem
                {
                    Description = garageConfigData.Description,
                    TotalPlaces = garageConfigData.MaxCars,
                    Cost = garageConfigData.Cost,
                    Type = garageConfig.Key
                };
                garageList.Add(garageData);
            }
            return garageList;
        }
    }
}