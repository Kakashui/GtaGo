using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.Inventory;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.PlayerEffects;
using Whistler.Scenes.Configs;
using Whistler.SDK;
using Whistler.Entities;

namespace Whistler.Scenes.Actions
{
    public static class ItemActions
    {
        public static void Load()
        {
            InventoryService.OnUseOtherItem += Add;
            InventoryService.OnUseDrinkItem += Add;
            InventoryService.OnUseFoodItem += Add;
            InventoryService.OnUseNarcoticsItem += Add;
            InventoryService.OnUseMedicaments += Add;
            InventoryService.OnUseAlcoholeItem += Add;
            InventoryService.OnUseSeed += Add;
            InventoryService.OnUseWateringCan += Add;
            InventoryService.OnUseFertilizer += Add;
        }

        public static void Add(ExtPlayer player, Other item)
        {
            if (item.Config.SceneName == SceneNames.NoAction) return;
            if (player.IsInVehicle)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Center, "scene:item:nouse:veh", 3000);
                return;
            }
            player.Session.SceneItem = item;
            player.Session.SceneCount = item.Config.ActionsCount;
            SceneManager.StartScene(player, item.Config.SceneName);
        }
        public static void Add(ExtPlayer player, Food item)
        {
            if (item.Config.SceneName == SceneNames.NoAction)
            {
                InventoryService.OnUseLifeActivityItem?.Invoke(player, item.Config.LifeActivity.GetMultipled(item.Config.ActionsCount));
                return;
            }
            if (player.IsInVehicle)
                UseLifeActivityItemInVehicle(player, item.Config.ActionsCount, item.Config.LifeActivity);
            else
            {
                player.Session.SceneItem = item;
                player.Session.SceneCount = item.Config.ActionsCount;
                SceneManager.StartScene(player, item.Config.SceneName);
            }
        }
        public static void Add(ExtPlayer player, Drink item)
        {
            if (item.Config.SceneName == SceneNames.NoAction)
            {
                InventoryService.OnUseLifeActivityItem?.Invoke(player, item.Config.LifeActivity.GetMultipled(item.Config.ActionsCount));
                return;
            }
            if (player.IsInVehicle)
                UseLifeActivityItemInVehicle(player, item.Config.ActionsCount, item.Config.LifeActivity);
            else
            {
                player.Session.SceneItem = item;
                player.Session.SceneCount = item.Config.ActionsCount;
                SceneManager.StartScene(player, item.Config.SceneName);
            }
        }
        public static void Add(ExtPlayer player, Alcohol item)
        {
            if (item.Config.SceneName == SceneNames.NoAction)
            {
                InventoryService.OnUseLifeActivityItem?.Invoke(player, item.Config.LifeActivity.GetMultipled(item.Config.ActionsCount));
                return;
            }
            if (player.IsInVehicle)
                UseLifeActivityItemInVehicle(player, item.Config.ActionsCount, item.Config.LifeActivity);
            else
            {
                player.Session.SceneItem = item;
                player.Session.SceneCount = item.Config.ActionsCount;
                SceneManager.StartScene(player, item.Config.SceneName);
            }
        }
        public static void Add(ExtPlayer player, Narcotic item)
        {
            if(item.Config.SceneName == SceneNames.NoAction)
            {
                InventoryService.OnUseLifeActivityItem?.Invoke(player, item.Config.LifeActivity.GetMultipled(item.Config.ActionsCount));
                return;
            }
            if (player.IsInVehicle)
                UseLifeActivityItemInVehicle(player, item.Config.ActionsCount, item.Config.LifeActivity);
            else
            {
                player.Session.SceneItem = item;
                player.Session.SceneCount = item.Config.ActionsCount;
                SceneManager.StartScene(player, item.Config.SceneName);
            }
        }   
        public static void Add(ExtPlayer player, Medicaments item)
        {
            if (item.Config.SceneName == SceneNames.NoAction)
            {
                InventoryService.OnUseLifeActivityItem?.Invoke(player, item.Config.LifeActivity.GetMultipled(item.Config.ActionsCount));
                return;
            }
            if (player.IsInVehicle)
                UseLifeActivityItemInVehicle(player, item.Config.ActionsCount, item.Config.LifeActivity);
            else
            {
                player.Session.SceneItem = item;
                player.Session.SceneCount = item.Config.ActionsCount;
                SceneManager.StartScene(player, item.Config.SceneName);
            }
        }
        public static void Add(ExtPlayer player, Seed item)
        {
            if (item.Config.SceneName == SceneNames.NoAction)
            {
                return;
            }
            if (player.IsInVehicle)
                return;
            player.Session.SceneItem = item;
            player.Session.SceneCount = item.Config.ActionsCount;
            SceneManager.StartScene(player, item.Config.SceneName);
        }
        public static void Add(ExtPlayer player, WateringCan item)
        {
            if (item.Config.SceneName == SceneNames.NoAction)
            {
                return;
            }
            if (player.IsInVehicle)
                return;
            player.Session.SceneItem = item;
            player.Session.SceneCount = item.Config.ActionsCount;
            SceneManager.StartScene(player, item.Config.SceneName);
        }
        public static void Add(ExtPlayer player, Fertilizer item)
        {
            if (item.Config.SceneName == SceneNames.NoAction)
            {
                return;
            }
            if (player.IsInVehicle)
                return;
            player.Session.SceneItem = item;
            player.Session.SceneCount = item.Config.ActionsCount;
            SceneManager.StartScene(player, item.Config.SceneName);
        }
        public static bool SmokingCigarette(ExtPlayer player)
        {
            if (player.Session.SceneCount < 1) return false;
            player.Session.SceneCount--;
            if (!(player.Session.SceneItem is Other)) return false;
            return true;
        }
        public static bool Drink(ExtPlayer player)
        {
            if (player.Session.SceneCount < 1) return false;
            if (!(player.Session.SceneItem is Drink)) return false;
            Drink drink = player.Session.SceneItem as Drink;
            player.Session.SceneCount--;
            InventoryService.OnUseLifeActivityItem?.Invoke(player, drink.Config.LifeActivity);
            return true;
        }
        public static bool Aclohol(ExtPlayer player)
        {
            if (player.Session.SceneCount < 1) return false;
            if (!(player.Session.SceneItem is Alcohol)) return false;
            Alcohol alco = player.Session.SceneItem as Alcohol;
            alco.CheckEffects(player, alco.Config.Effects);
            player.Session.SceneCount--;
            InventoryService.OnUseLifeActivityItem?.Invoke(player, alco.Config.LifeActivity);
            return true;
        }
        public static bool Eat(ExtPlayer player)
        {
            if (player.Session.SceneCount < 1) return false;
            if (!(player.Session.SceneItem is Food)) return false;
            Food food = player.Session.SceneItem as Food;
            player.Session.SceneCount--;
            InventoryService.OnUseLifeActivityItem?.Invoke(player, food.Config.LifeActivity);
            return true;
        }
        public static bool Medicaments(ExtPlayer player)
        {
            if (!(player.Session.SceneItem is Medicaments)) return false;
            Medicaments narc = player.Session.SceneItem as Medicaments;
            InventoryService.OnUseLifeActivityItem?.Invoke(player, narc.Config.LifeActivity);
            return true;
        }
        public static bool Narcotics(ExtPlayer player)
        {
            if (!(player.Session.SceneItem is Narcotic)) return false;
            Narcotic narc = player.Session.SceneItem as Narcotic;
            narc.CheckEffects(player, narc.Config.Effects);
            InventoryService.OnUseLifeActivityItem?.Invoke(player, narc.Config.LifeActivity);
            return true;
        }
        public static bool SeatSeed(ExtPlayer player)
        {
            if (!(player.Session.SceneItem is Seed)) return false;
            Seed seed = player.Session.SceneItem as Seed;
            return Jobs.Farm.FarmManager.OnUseSeed(player, seed);
        }
        public static bool WateringSeed(ExtPlayer player)
        {
            if (!(player.Session.SceneItem is WateringCan)) return false;
            WateringCan watering = player.Session.SceneItem as WateringCan;
            return Jobs.Farm.FarmManager.OnUseWateringCan(player, watering);
        }
        public static bool FertilizeringSeed(ExtPlayer player)
        {
            if (!(player.Session.SceneItem is Fertilizer)) return false;
            Fertilizer fertilizer = player.Session.SceneItem as Fertilizer;
            return Jobs.Farm.FarmManager.OnUseFertilizer(player, fertilizer);
        }
        public static bool Harvesting(ExtPlayer player)
        {
            if (!(player.Session.SceneItem is Other)) return false;
            Other harvesting = player.Session.SceneItem as Other;
            return Jobs.Farm.FarmManager.OnUseHarvesting(player, harvesting);
        }
        public static bool DynamitePlant(ExtPlayer player)
        {
            if (!(player.Session.SceneItem is Other)) return false;
            Other dynamite = player.Session.SceneItem as Other;
            return Jobs.SteelMaking.OreMining.OnUseDynamite(player, dynamite);
        }
        public static void UseHealthKit(ExtPlayer player, BaseItem item)
        {
            if (item.Name != ItemNames.HealthKit)
                return;
            player.Health = 100;
            Main.OnAntiAnim(player);
            player.PlayAnimation("amb@code_human_wander_texting_fat@female@enter", "enter", 49);
            NAPI.Task.Run(() =>
            {
                if (player == null) return;
                if (!player.IsInVehicle)
                    player.StopAnimation();
                else
                    SafeTrigger.SetData(player, "ToResetAnimPhone", true);
                Main.OffAntiAnim(player);
                SafeTrigger.ClientEvent(player, "stopScreenEffect", "PPFilter");
            }, 5000);
            Chat.Action(player, $"Core_119");
        }
        private static void UseLifeActivityItemInVehicle(ExtPlayer player, int multiple, LifeActivityData data)
        {
            var time = Math.Max(multiple * 2, 4);
            SafeTrigger.ClientEvent(player,"scene:action:delay", time);
            InventoryService.OnUseLifeActivityItem?.Invoke(player, data.GetMultipled(multiple));
        }
    }
}
