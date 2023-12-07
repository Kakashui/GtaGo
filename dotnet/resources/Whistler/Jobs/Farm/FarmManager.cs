using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using WebSocketSharp;
using Whistler.Core;
using Whistler.Entities;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.Jobs.Farm.Configs;
using Whistler.Jobs.Farm.Configs.Models;
using Whistler.Jobs.Farm.DTO;
using Whistler.Jobs.Farm.Models;
using Whistler.Jobs.SteelMaking;
using Whistler.MoneySystem;
using Whistler.NewJobs.Models;
using Whistler.SDK;
using Whistler.SDK.CustomColShape;

namespace Whistler.Jobs.Farm
{
    class FarmManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(FarmManager));
        private static List<FarmZone> _zones = new List<FarmZone>
        {
            new FarmZone(0, new Vector3(2114.698, 4913.8887, 0), new Vector3(1921.8236, 5121.6743, 0), new Vector3(1826.7145, 5023.0566, 0), new Vector3(2027.6215, 4833.941, 0)),
            // new FarmZone(1, new Vector3(433.4487, 6580.435, 0), new Vector3(799, 6514, 0), new Vector3(798.1387, 6435.716, 306.1961), new Vector3(441.0995, 6437.524, 248.9833)),
            // new FarmZone(2, new Vector3(2735.696, 4443.497, 112.7389), new Vector3(2692, 4697, 50), new Vector3(2929, 4802, 60), new Vector3(3041, 4536, 60)),
        };
        private static List<PlantInLand> _plants = new List<PlantInLand>();
        public static Job Job { get; set; }
        private static List<WaterPoint> _points = new List<WaterPoint>();
        private static int _distanceToNearestPoint = 3;
        private static InteractShape _sellInteract;
        private static Vector3 _sellPoint = new Vector3(1180.6, -3113.8, 5.028025);
        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            try
            {
                foreach (var zone in _zones)
                {
                    var colShape = ShapeManager.CreateQuadColShape(zone.Point1, zone.Point2, zone.Point3, zone.Point4, 0);
                    colShape.OnEntityEnterColShape += (ExtPlayer player) => ColShape_OnEntityEnterColShape(player, zone.ID);
                    colShape.OnEntityExitColShape += (ExtPlayer player) => ColShape_OnEntityExitColShape(player, zone.ID);
                }
                foreach (var point in Configs.FarmConfigs.WaterPoints)
                {
                    _points.Add(new WaterPoint(point));
                    NAPI.Blip.CreateBlip(1, point, 0.4F, 53, "Вода", 255, 0, true, 0, 0);
                }
                Job = new Job
                {
                    Name = "farmer",
                    Levels = new List<JobLevel>
                    {
                        new JobLevel(1, 0, "Standart"),
                        new JobLevel(2, 2000, "Medium"),
                        new JobLevel(3, 6000, "Fast"),
                        new JobLevel(4, 15000, "Big"),
                        new JobLevel(5, 35000, "Fertilized"),
                        new JobLevel(6, 80000, "BigFertilized"),
                        new JobLevel(7, 200000, "FastFertilized"),
                    },
                    Condition = (ExtPlayer player) => true,
                    Limit = 50000000
                };
                Job.ParseLevels("interfaces/gui/src/configs/farm", "/levels.js");
                _sellInteract = InteractShape.Create(_sellPoint, 2, 2, 0)
                    .AddDefaultMarker()
                    .AddInteraction(SellHarvest, "farmHouse_27");
                NAPI.Blip.CreateBlip(587, _sellPoint, 1, 52, "Скупщик", 255, 0, true, 0, 0);
            }
            catch (Exception e)
            {
                _logger.WriteError("ResourceStart: " + e.ToString());
            }
        }

        private static void SellHarvest(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            var inventory = player.GetInventory();
            var price = GetProductPrices(player.GetInventory());
            if (price == 0)
            {
                Notify.SendError(player, "farmHouse_36");
                return;
            }

            if (player.Character.IsPrimeActive()) price *= 2;

            DialogUI.Open(player, "farmHouse_28".Translate(price),
                new List<DialogUI.ButtonSetting>
                {
                        new DialogUI.ButtonSetting
                        {
                            Name = "gui_727",
                            Icon = "confirm",
                            Action = p =>
                            {
                                var price = GetProductPrices(p.GetInventory());
                                if (p.Character.IsPrimeActive()) price *= 2;
                                inventory.RemoveItems(CheckProductDelete);
                                Wallet.MoneyAdd(p.Character, price, "Продажа фруктов на ферме");
                            }
                        },
                        new DialogUI.ButtonSetting
                        {
                            Name = "gui_728",
                            Icon = "cancel",
                        }
                });
        }

        private static int GetProductPrices(InventoryModel inventory)
        {
            var price = 0;
            inventory.Items.ForEach(item =>
            {
                switch (item.Type)
                {
                    case ItemTypes.Resources:
                        if (OreMiningSettings.ResourcePrices.ContainsKey(item.Name))
                            price += PriceSystem.PriceManager.GetPriceInDollars(PriceSystem.TypePrice.Resources, item.Name.ToString(), OreMiningSettings.ResourcePrices[item.Name]) * item.Count;
                        break;
                    case ItemTypes.Food:
                        var plant = FarmConfigs.PlantConfigsList.FirstOrDefault(plant => plant.Value.Fetus == item.Name).Value;
                        if (plant != null)
                            price += (int)(plant.Price * item.Count * FarmConfigs.CellCoef);
                        break;
                    default:
                        break;
                }
            });
            return price;
        }

        private static bool CheckProductDelete(BaseItem item)
        {
            switch (item.Type)
            {
                case ItemTypes.Resources:
                    return OreMiningSettings.ResourcePrices.ContainsKey(item.Name);
                case ItemTypes.Food:
                    return FarmConfigs.PlantConfigsList.FirstOrDefault(plant => plant.Value.Fetus == item.Name).Value != null;
                default:
                    return false;
            }
        }

        private void ColShape_OnEntityExitColShape(ExtPlayer player, int id)
        {
            SubscribeSystem.UnSubscribe(player, id);
            SafeTrigger.ClientEvent(player,"farm:unloadPoints", id);
        }

        private void ColShape_OnEntityEnterColShape(ExtPlayer player, int id)
        {
            if (player.Character.LVL < 1)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Для этой работы нужно быть как минимум 1м уровнем.", 3000);
                return;
            }
            SubscribeSystem.Subscribe(player, id);
            SafeTrigger.ClientEvent(player,"farm:loadPoints", id, JsonConvert.SerializeObject(_plants.Where(item => item.FarmId == id).Select(item => new PlantInLandDto(item))));
        }

        private static SeedingPlaceConfig GetNearestPoint(ExtPlayer player, int farmId)
        {
            if (!Configs.FarmConfigs._configsByFarmId.ContainsKey(farmId))
                return null;
            var nearestPoints = Configs.FarmConfigs._configsByFarmId[farmId].Where(item => player.Position.DistanceTo(item.Position) <= _distanceToNearestPoint);
            if (nearestPoints.Count() == 0)
                return null;
            var result = nearestPoints.FirstOrDefault();
            foreach (var point in nearestPoints)
            {
                if (point.Position.DistanceTo(player.Position) < result.Position.DistanceTo(player.Position))
                    result = point;
            }
            return result;
        }

        #region Use Items

        public static bool OnUseSeed(ExtPlayer player, Seed seed)
        {
            if (!player.GetInventory().Items.Contains(seed) || seed.Count <= 0)
                return false;
            int farmId = SubscribeSystem.GetSubscribe(player);
            if (farmId < 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "farmHouse_9", 3000);              
                return false;
            }    
            SeedingPlaceConfig point = GetNearestPoint(player, farmId);
            if (point == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "farmHouse_11", 3000);
                return false;
            }
            int countPlant = _plants.Where(item => item.UUID == player.Character.UUID).Count();
            int level = Job.GetLvlLvl(player);
            var levelData = FarmConfigs.Levels.GetValueOrDefault(level);
            if (levelData == null)
                return false;
            if (levelData.CountPits <= countPlant)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "farmHouse_13".Translate(levelData.CountPits), 3000);
                return false;
            }
            if ((int)point.PitType > (int)levelData.BestPits)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "farmHouse_14", 3000);
                return false;
            }
            if (PlantingSeedOnPoint(player, seed.Name, farmId, point))
            {
                player.GetInventory().SubItemByName(seed.Name, 1, LogAction.Use);
                player.CreatePlayerAction(PersonalEvents.PlayerActions.MoveOnFarm, 1);
                player.CreatePlayerAction(PersonalEvents.PlayerActions.PlantingSeed, 1);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "farmHouse_23", 3000);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Теперь нужно полить растение и удобрить его", 3000);
                return true;
            }
            return false;
        }

        public static bool OnUseWateringCan(ExtPlayer player, WateringCan watering)
        {
            if (!player.GetInventory().Items.Contains(watering))
                return false;
            int farmId = SubscribeSystem.GetSubscribe(player);
            if (farmId < 0)
            {
                Notify.SendError(player, "farmHouse_9");
                return false;
            }
            SeedingPlaceConfig point = GetNearestPoint(player, farmId);
            if (point == null)
            {
                Notify.SendError(player, "farmHouse_10");
                return false;
            }
            if (watering.Water == 0)
            {
                Notify.SendError(player, "farmHouse_15");
                return false;
            }
            if (WateringSeedOnPoint(player, farmId, point, watering))
            {
                player.GetInventory().UpdateItemData(watering.Index);
                player.CreatePlayerAction(PersonalEvents.PlayerActions.MoveOnFarm, 1);
                player.CreatePlayerAction(PersonalEvents.PlayerActions.WateringPlant, 1);
                Notify.SendSuccess(player, "farmHouse_24");
                return true;
            }
            return false;
        }

        public static bool OnUseFertilizer(ExtPlayer player, Fertilizer fertilizer)
        {
            if (!player.GetInventory().Items.Contains(fertilizer))
                return false;
            int farmId = SubscribeSystem.GetSubscribe(player);
            if (farmId < 0)
            {
                Notify.SendError(player, "farmHouse_9");
                return false;
            }
            SeedingPlaceConfig point = GetNearestPoint(player, farmId);
            if (point == null)
            {
                Notify.SendError(player, "farmHouse_10");
                return false;
            }
            if (FertilizerSeedOnPoint(player, fertilizer, farmId, point))
            {
                player.GetInventory().SubItemByName(fertilizer.Name, 1, LogAction.Use);
                player.CreatePlayerAction(PersonalEvents.PlayerActions.MoveOnFarm, 1);
                Notify.SendSuccess(player, "farmHouse_25");
                return true;
            }
            return false;
        }
        public static bool OnUseHarvesting(ExtPlayer player, Other foodBox)
        {
            if (!player.GetInventory().Items.Contains(foodBox))
                return false;
            if (foodBox.Name != ItemNames.FoodBox)
                return false;
            int farmId = SubscribeSystem.GetSubscribe(player);
            if (farmId < 0)
            {
                Notify.SendError(player, "farmHouse_9");
                return false;
            }
            SeedingPlaceConfig point = GetNearestPoint(player, farmId);
            if (point == null)
            {
                Notify.SendError(player, "farmHouse_10");
                return false;
            }

            if (HarvestingFoodOnPoint(player, farmId, point))
            {
                Notify.SendSuccess(player, "farmHouse_26");
                player.CreatePlayerAction(PersonalEvents.PlayerActions.MoveOnFarm, 1);
                player.CreatePlayerAction(PersonalEvents.PlayerActions.HarvestPlant, 1);
                return true;
            }
            return false;
        }

        #endregion

        #region Farm Moving
        private static bool PlantingSeedOnPoint(ExtPlayer player, ItemNames seed, int farmId, SeedingPlaceConfig point)
        {
            if (_plants.FirstOrDefault(item => item.FarmId == farmId && item.PointId == point.ID) != null)
            {
                Notify.SendError(player, "farmHouse_12");
                return false;
            }
            PlantInLand plant = PlantInLand.CreatePlant(farmId, point.ID, player.Character.UUID, seed, Core.Pets.Controller.Pet_IsFarmBuff(player));
            if (plant == null)
            {
                Notify.SendError(player, "farmHouse_16");
                return false;
            }
            _plants.Add(plant);
            SubscribeSystem.TriggerEventToSubscribers(farmId, "farm:updatePlant", JsonConvert.SerializeObject(new PlantInLandDto(plant)));
            return true;
        }

        private static bool WateringSeedOnPoint(ExtPlayer player, int farmId, SeedingPlaceConfig point, WateringCan watering)
        {
            var plant = _plants.FirstOrDefault(item => item.FarmId == farmId && item.PointId == point.ID);
            if (plant == null)
            {
                Notify.SendError(player, "farmHouse_10");
                return false;
            }
            if (plant.WateringTime > plant.PlantingTime || (DateTime.UtcNow - plant.PlantingTime).TotalSeconds > plant.ConfigPlant.SecondBeforeWatering)
            {
                Notify.SendError(player, "farmHouse_17");
                return false;
            }
            if (!watering.WateringSeed())
            {
                Notify.SendError(player, "farmHouse_15");
                return false;
            }    
            plant.WateringTime = DateTime.UtcNow;
            SubscribeSystem.TriggerEventToSubscribers(farmId, "farm:updatePlant", JsonConvert.SerializeObject(new PlantInLandDto(plant)));
            return true;
        }

        private static bool FertilizerSeedOnPoint(ExtPlayer player, Fertilizer fertilizer, int farmId, SeedingPlaceConfig point)
        {
            var plant = _plants.FirstOrDefault(item => item.FarmId == farmId && item.PointId == point.ID);
            if (plant == null)
            {
                Notify.SendError(player, "farmHouse_10");
                return false;
            }
            if ((DateTime.UtcNow - plant.PlantingTime).TotalSeconds > plant.ConfigPlant.SecondBeforeWatering)
            {
                Notify.SendError(player, "farmHouse_18");
                return false;
            }
            if (plant.Fertilizer != FertilizerType.None)
            {
                Notify.SendError(player, "farmHouse_19");
                return false;
            }
            if (plant.ConfigPlant.Type != fertilizer.Config.PlantType)
            {
                Notify.SendError(player, "farmHouse_20");
                return false;
            }
            plant.Fertilizer = fertilizer.Config.FertilizerType;
            SubscribeSystem.TriggerEventToSubscribers(farmId, "farm:updatePlant", JsonConvert.SerializeObject(new PlantInLandDto(plant)));
            return true;
        }

        private static bool HarvestingFoodOnPoint(ExtPlayer player, int farmId, SeedingPlaceConfig point)
        {
            var plant = _plants.FirstOrDefault(item => item.FarmId == farmId && item.PointId == point.ID);
            if (plant == null)
            {
                Notify.SendError(player, "farmHouse_10");
                return false;
            }
            var status = plant.GetStatusPlant();
            if (status == PlantStatus.Growing)
            {
                Notify.SendError(player, "farmHouse_21");
                return false;
            }
            if (status == PlantStatus.Ripe)
            {
                if (plant.UUID != player.Character.UUID)
                {
                    return false;
                }
                var food = ItemsFabric.CreateFood(plant.ConfigPlant.Fetus, plant.ConfigPlant.CountFetus + plant.PitConfig.FetusIncr + plant.FertilizerConfig.FetusIncr, false);
                if (food == null)
                    return false;
                if (!player.GetInventory().AddItem(food))
                {
                    Notify.SendError(player, "farmHouse_22");
                    return false;
                }
                var worker = NewJobs.JobService.GetWorker(player);
                worker?.AddExp(Job, plant.ConfigPlant.Exp + plant.PitConfig.ExpIncr + plant.FertilizerConfig.ExpIncr);
            }
            _plants.Remove(plant);
            SubscribeSystem.TriggerEventToSubscribers(farmId, "farm:deletePlant", farmId, plant.PointId);
            return true;
        }
        #endregion
    }
}
