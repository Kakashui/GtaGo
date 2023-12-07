using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using GTANetworkAPI;

namespace Whistler.Inventory.Configs
{
    static class DrinkConfigs
    {
        public static Dictionary<ItemNames, DrinkConfig> Config;
        static DrinkConfigs()
        {
            Config = new Dictionary<ItemNames, DrinkConfig>();

            Config.Add(ItemNames.AppleJuice, new DrinkConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_food_bs_juice03"),
                Type = ItemTypes.Drink,
                Weight = 400,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_applejuice",
                Image = "applejuice",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 

                SceneName = Scenes.Configs.SceneNames.DrinkSprunk,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0,
                    ThirstIncrease = 13,
                }
            });

            Config.Add(ItemNames.CarrotJuice, new DrinkConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_food_bs_juice03"),
                Type = ItemTypes.Drink,
                Weight = 400,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_carrotjuice",
                Image = "carrotjuice",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1),

                SceneName = Scenes.Configs.SceneNames.DrinkSprunk,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0,
                    ThirstIncrease = 14,
                }
            }); ;

            Config.Add(ItemNames.AlpelsinJuice, new DrinkConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_food_bs_juice03"),
                Type = ItemTypes.Drink,
                Weight = 400,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_alpelsinjuice",
                Image = "alpelsinjuice",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1),

                SceneName = Scenes.Configs.SceneNames.DrinkSprunk,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0,
                    ThirstIncrease = 14,
                }
            });

            Config.Add(ItemNames.Borjomi, new DrinkConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_ld_flow_bottle"),
                Type = ItemTypes.Drink,
                Weight = 500,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_borjomi",
                Image = "borjomi",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.DrinkSprunk,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0,
                    ThirstIncrease = 33,
                }
            });

            Config.Add(ItemNames.MineralWater, new DrinkConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_ld_flow_bottle"),
                Type = ItemTypes.Drink,
                Weight = 500,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_mineralwater",
                Image = "mineralwater",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.DrinkSprunk,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0,
                    ThirstIncrease = 20,
                }
            });

            Config.Add(ItemNames.Kvass, new DrinkConfig
            {
                ModelHash = NAPI.Util.GetHashKey("v_ret_fh_bscup"),
                Type = ItemTypes.Drink,
                Weight = 400,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_kvass",
                Image = "kvass",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.88),

                SceneName = Scenes.Configs.SceneNames.DrinkSprunk,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0,
                    ThirstIncrease = 3,
                }
            });

            Config.Add(ItemNames.Milk, new DrinkConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_cs_milk_01"),
                Type = ItemTypes.Drink,
                Weight = 300,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_milk",
                Image = "milk",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.94),

                SceneName = Scenes.Configs.SceneNames.DrinkSprunk,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0,
                    ThirstIncrease = 5,
                }
            });

            Config.Add(ItemNames.RedBull, new DrinkConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_orang_can_01"),
                Type = ItemTypes.Drink,
                Weight = 300,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_redbull",
                Image = "redbull",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.94),

                SceneName = Scenes.Configs.SceneNames.DrinkSprunk,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0,
                    ThirstIncrease = 3,
                }
            });

            Config.Add(ItemNames.Coffee, new DrinkConfig
            {
                ModelHash = NAPI.Util.GetHashKey("p_ing_coffeecup_01"),
                Type = ItemTypes.Drink,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_coffee",
                Image = "coffee",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.93),

                SceneName = Scenes.Configs.SceneNames.DrinkSprunk,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0,
                    ThirstIncrease = 5,
                }
            });

            Config.Add(ItemNames.Tea, new DrinkConfig
            {
                ModelHash = NAPI.Util.GetHashKey("p_amb_coffeecup_01"),
                Type = ItemTypes.Drink,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_tea",
                Image = "tea",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.93),

                SceneName = Scenes.Configs.SceneNames.DrinkSprunk,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0,
                    ThirstIncrease = 8,
                }
            });

            Config.Add(ItemNames.eCola, new DrinkConfig
            {
                ModelHash = NAPI.Util.GetHashKey("ng_proc_sodacan_01a"),
                Type = ItemTypes.Drink,
                Weight = 300,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_cocacola",
                Image = "cocacola",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1),

                SceneName = Scenes.Configs.SceneNames.DrinkSprunk,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0,
                    ThirstIncrease = 10,
                }
            });

            Config.Add(ItemNames.Sprunk, new DrinkConfig
            {
                ModelHash = NAPI.Util.GetHashKey("ng_proc_sodacan_01b"),
                Type = ItemTypes.Drink,
                Weight = 300,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_sprite",
                Image = "sprite",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1),

                SceneName = Scenes.Configs.SceneNames.DrinkSprunk,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0,
                    ThirstIncrease = 10,
                }
            });
        }   
    }
}
