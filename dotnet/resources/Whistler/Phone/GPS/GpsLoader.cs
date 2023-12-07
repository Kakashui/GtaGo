using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Phone.GPS.Dtos;
using Whistler.SDK;

namespace Whistler.Phone.GPS
{
    public class GpsLoader : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(GpsLoader));

        public GpsLoader()
        {
            Main.OnPlayerReady += LoadGpsPointsToPlayer;
        }

        private void LoadGpsPointsToPlayer(ExtPlayer player)
        {
            try
            {
                var gpsItemsJson = GetGpsItemsJson();
                SafeTrigger.ClientEvent(player,"phone:gps:load", gpsItemsJson);
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on LoadGpsPointsToPlayer ({player?.Name}) - " + e.ToString()); }
        }

        private static string _cachedGpsItems;
        private string GetGpsItemsJson()
        {
            if (_cachedGpsItems == null)
            {
                var itemsByCategory = new Dictionary<string, List<GpsItemDto>>();

                itemsByCategory.Add("gun", GetBusinessGpsItemsByType(BusinessManager.BusinessType.GunShop));
                itemsByCategory.Add("clothes", GetBusinessGpsItemsByType(BusinessManager.BusinessType.ClothesShop));
                itemsByCategory.Add("carrent", GetBusinessGpsItemsByType(BusinessManager.BusinessType.RentCar));
                itemsByCategory.Add("gasstations", GetBusinessGpsItemsByType(BusinessManager.BusinessType.PetrolStation));
                itemsByCategory.Add("shops24", GetBusinessGpsItemsByType(BusinessManager.BusinessType.Shop247));
                itemsByCategory.Add("autorooms", GetCarromsGpsItems());
                itemsByCategory.Add("burgershots", GetBusinessGpsItemsByType(BusinessManager.BusinessType.BurgerShop));
                itemsByCategory.Add("carwashes", GetBusinessGpsItemsByType(BusinessManager.BusinessType.Carwash));
                itemsByCategory.Add("servicestation", GetBusinessGpsItemsByType(BusinessManager.BusinessType.Autorepair));
                itemsByCategory.Add("works", GetWorksGpsItems());
                itemsByCategory.Add("gov", GetFractionsGpsItemsByType(2));
                itemsByCategory.Add("gangs", GetFractionsGpsItemsByType(1, 16));
                itemsByCategory.Add("atms", GetAtmsGpsItems());

                _cachedGpsItems = JsonConvert.SerializeObject(itemsByCategory);
            }

            return _cachedGpsItems;
        }

        private List<GpsItemDto> GetAtmsGpsItems()
        {
            return MoneySystem.ATM.ATMs
                .Select(pos => new GpsItemDto {
                    Title = "ATM",
                    Position = pos
                })
                .ToList();
        }
        private List<GpsItemDto> GetFractionsGpsItemsByType(int fractionType, params int[] exceptionsIds)
        {
            var items = new List<GpsItemDto>();

            foreach ((var id, var position) in Fractions.Manager.FractionSpawns
                .Where(f => Fractions.Manager.FractionTypes[f.Key] == fractionType && !exceptionsIds.Contains(f.Key)))
            {
                items.Add(new GpsItemDto
                {
                    Position = position,
                    Title = Fractions.Manager.getName(id)
                });
            }

            return items;
        }

        private List<GpsItemDto> GetWorksGpsItems()
        {
            var resultList = new List<GpsItemDto>();

            // Hunting
            var huntingGrounds = Jobs.Hunting.Work.GetHuntingGroundsPositions();
            foreach (var position in huntingGrounds)
                resultList.Add(new GpsItemDto { Position = position, Title = "phone:gps:1" });

            // Technician
            resultList.Add(new GpsItemDto { Position = Jobs.Technician.Work.StartEndWorkPoint, Title = "phone:gps:2" });

            // Transporteur
            resultList.Add(new GpsItemDto { Position = Jobs.Transporteur.Work.StartEndWorkPoint, Title = "phone:gps:3" });
            

            return resultList;
        }

        private List<GpsItemDto> GetCarromsGpsItems()
        {
            var autorooms = new List<BusinessManager.BusinessType>
            {
                BusinessManager.BusinessType.PremiumCarRoom,
                BusinessManager.BusinessType.SportCarRoom,
                BusinessManager.BusinessType.MiddleCarRoom,
                BusinessManager.BusinessType.MotoCarRoom,
                BusinessManager.BusinessType.SuperCarRoom,
                BusinessManager.BusinessType.RetroCarRoom,
                BusinessManager.BusinessType.JDMCarRoom,
                BusinessManager.BusinessType.NewCarRoom1,
                BusinessManager.BusinessType.NewCarRoom2,
                BusinessManager.BusinessType.NewCarRoom3,
                BusinessManager.BusinessType.NewCarRoom4,
                BusinessManager.BusinessType.NewCarRoom5,
                BusinessManager.BusinessType.MercedesPage,
                BusinessManager.BusinessType.BmwPage,
                BusinessManager.BusinessType.RollsPage,
                BusinessManager.BusinessType.BentleyPage,
                BusinessManager.BusinessType.FerrariPage,
                BusinessManager.BusinessType.DiamondPage,
                BusinessManager.BusinessType.LuxePage,
                BusinessManager.BusinessType.JeepPage,
                BusinessManager.BusinessType.CasualPage
            };

            return autorooms.SelectMany(t => GetBusinessGpsItemsByType(t)).ToList();
        }

        private List<GpsItemDto> GetBusinessGpsItemsByType(BusinessManager.BusinessType type) 
        {
            return BusinessManager.BizList
                .Values
                .Where(b => b.Type == (int)type)
                .Select(b => new GpsItemDto
                {
                    Title = b.TypeModel.TypeName,
                    Position = b.BlipPosition ?? b.EnterPoint
                })
                .ToList();
        }
    }
}
