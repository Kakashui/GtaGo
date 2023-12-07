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
using Whistler.Inventory.Enums;
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

        public static void Add(PlayerGo player, Other item)
        {
            if (item.Config.SceneName == SceneNames.NoAction) return;
            if (player.IsInVehicle)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Center, "scene:item:nouse:veh", 3000);
                return;
            }
            player.SetData("scene:action:count", item.Config.ActionsCount);
            player.SetData("scene:action:item", item);
            SceneManager.StartScene(player, item.Config.SceneName);
        }
        public static void Add(PlayerGo player, Food item)
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
                player.SetData("scene:action:count", item.Config.ActionsCount);
                player.SetData("scene:action:item", item);
                SceneManager.StartScene(player, item.Config.SceneName);
            }
        }
        public static void Add(PlayerGo player, Drink item)
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
                player.SetData("scene:action:count", item.Config.ActionsCount);
                player.SetData("scene:action:item", item);
                SceneManager.StartScene(player, item.Config.SceneName);
            }
        }
        public static void Add(PlayerGo player, Alcohol item)
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
                player.SetData("scene:action:count", item.Config.ActionsCount);
                player.SetData("scene:action:item", item);
                SceneManager.StartScene(player, item.Config.SceneName);
            }
        }
        public static void Add(PlayerGo player, Narcotic item)
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
                player.SetData("scene:action:count", item.Config.ActionsCount);
                player.SetData("scene:action:item", item);
                SceneManager.StartScene(player, item.Config.SceneName);
            }
        }   
        public static void Add(PlayerGo player, Medicaments item)
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
                player.SetData("scene:action:count", item.Config.ActionsCount);
                player.SetData("scene:action:item", item);
                SceneManager.StartScene(player, item.Config.SceneName);
            }
        }
        public static void Add(PlayerGo player, Seed item)
        {
            if (item.Config.SceneName == SceneNames.NoAction)
            {
                return;
            }
            if (player.IsInVehicle)
                return;
            player.SetData("scene:action:count", item.Config.ActionsCount);
            player.SetData("scene:action:item", item);
            SceneManager.StartScene(player, item.Config.SceneName);
        }
        public static void Add(PlayerGo player, WateringCan item)
        {
            if (item.Config.SceneName == SceneNames.NoAction)
            {
                return;
            }
            if (player.IsInVehicle)
                return;
            player.SetData("scene:action:count", item.Config.ActionsCount);
            player.SetData("scene:action:item", item);
            SceneManager.StartScene(player, item.Config.SceneName);
        }
        public static void Add(PlayerGo player, Fertilizer item)
        {
            if (item.Config.SceneName == SceneNames.NoAction)
            {
                return;
            }
            if (player.IsInVehicle)
                return;
            player.SetData("scene:action:count", item.Config.ActionsCount);
            player.SetData("scene:action:item", item);
            SceneManager.StartScene(player, item.Config.SceneName);
        }
        public static bool SmokingCigarette(PlayerGo player)
        {
            if (!player.HasData("scene:action:count") || player.GetData<int>("scene:action:count") < 1) return false;
            player.SetData("scene:action:count", player.GetData<int>("scene:action:count") - 1);
            if (!(player.GetData<BaseItem>("scene:action:item") is Other)) return false;
            return true;
        }
        public static bool Drink(PlayerGo player)
        {
            if (!player.HasData("scene:action:count") || player.GetData<int>("scene:action:count") < 1) return false;
            if (!(player.GetData<BaseItem>("scene:action:item") is Drink)) return false;
            Drink drink = player.GetData<Drink>("scene:action:item");
            player.SetData("scene:action:count", player.GetData<int>("scene:action:count") - 1);
            InventoryService.OnUseLifeActivityItem?.Invoke(player, drink.Config.LifeActivity);
            return true;
        }
        public static bool Aclohol(PlayerGo player)
        {
            if (!player.HasData("scene:action:count") || player.GetData<int>("scene:action:count") < 1) return false;
            if (!(player.GetData<BaseItem>("scene:action:item") is Alcohol)) return false;
            Alcohol alco = player.GetData<Alcohol>("scene:action:item");
            alco.CheckEffects(player, alco.Config.Effects);
            player.SetData("scene:action:count", player.GetData<int>("scene:action:count") - 1);
            InventoryService.OnUseLifeActivityItem?.Invoke(player, alco.Config.LifeActivity);
            return true;
        }
        public static bool Eat(PlayerGo player)
        {
            if (!player.HasData("scene:action:count") || player.GetData<int>("scene:action:count") < 1) return false;
            if (!(player.GetData<BaseItem>("scene:action:item") is Food)) return false;
            Food food = player.GetData<Food>("scene:action:item");
            var newCount = player.GetData<int>("scene:action:count") - 1;
            player.SetData("scene:action:count", player.GetData<int>("scene:action:count") - 1);
            InventoryService.OnUseLifeActivityItem?.Invoke(player, food.Config.LifeActivity);
            return true;
        }
        public static bool Medicaments(PlayerGo player)
        {
            if (!(player.GetData<BaseItem>("scene:action:item") is Medicaments)) return false;
            Medicaments narc = player.GetData<Medicaments>("scene:action:item");
            InventoryService.OnUseLifeActivityItem?.Invoke(player, narc.Config.LifeActivity);
            return true;
        }
        public static bool Narcotics(PlayerGo player)
        {
            if (!(player.GetData<BaseItem>("scene:action:item") is Narcotic)) return false;
            Narcotic narc = player.GetData<Narcotic>("scene:action:item");
            narc.CheckEffects(player, narc.Config.Effects);
            InventoryService.OnUseLifeActivityItem?.Invoke(player, narc.Config.LifeActivity);
            return true;
        }
        public static bool SeatSeed(PlayerGo player)
        {
            if (!(player.GetData<BaseItem>("scene:action:item") is Seed)) return false;
            Seed seed = player.GetData<Seed>("scene:action:item");
            return Jobs.Farm.FarmManager.OnUseSeed(player, seed);
        }
        public static bool WateringSeed(PlayerGo player)
        {
            if (!(player.GetData<BaseItem>("scene:action:item") is WateringCan)) return false;
            WateringCan watering = player.GetData<WateringCan>("scene:action:item");
            return Jobs.Farm.FarmManager.OnUseWateringCan(player, watering);
        }
        public static bool FertilizeringSeed(PlayerGo player)
        {
            if (!(player.GetData<BaseItem>("scene:action:item") is Fertilizer)) return false;
            Fertilizer fertilizer = player.GetData<Fertilizer>("scene:action:item");
            return Jobs.Farm.FarmManager.OnUseFertilizer(player, fertilizer);
        }
        public static bool Harvesting(PlayerGo player)
        {
            if (!(player.GetData<BaseItem>("scene:action:item") is Other)) return false;
            Other harvesting = player.GetData<Other>("scene:action:item");
            return Jobs.Farm.FarmManager.OnUseHarvesting(player, harvesting);
        }
        public static bool DynamitePlant(PlayerGo player)
        {
            if (!(player.GetData<BaseItem>("scene:action:item") is Other)) return false;
            Other dynamite = player.GetData<Other>("scene:action:item");
            return Jobs.SteelMaking.OreMining.OnUseDynamite(player, dynamite);
        }
        public static void UseHealthKit(PlayerGo player, BaseItem item)
        {
            if (item.Name != ItemNames.HealthKit)
                return;
            player.Health = 100;
            Main.OnAntiAnim(player);
            player.PlayAnimation("amb@code_human_wander_texting_fat@female@enter", "enter", 49);
            WhistlerTask.Run(() =>
            {
                if (player == null) return;
                if (!player.IsInVehicle)
                    player.StopAnimation();
                else
                    player.SetData("ToResetAnimPhone", true);
                Main.OffAntiAnim(player);
                Trigger.ClientEvent(player, "stopScreenEffect", "PPFilter");
            }, 5000);
            Chat.Action(player, $"Core_119");
        }
        private static void UseLifeActivityItemInVehicle(PlayerGo player, int multiple, LifeActivityData data)
        {
            var time = Math.Max(multiple * 2, 4);
            player.TriggerEvent("scene:action:delay", time);
            InventoryService.OnUseLifeActivityItem?.Invoke(player, data.GetMultipled(multiple));
        }
    }
}
