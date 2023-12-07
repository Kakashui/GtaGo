using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using GTANetworkAPI;

namespace Whistler.Inventory.Configs
{
    static class FoodConfigs
    {
        public static Dictionary<ItemNames, FoodConfig> Config;
        static FoodConfigs()
        {
            Config = new Dictionary<ItemNames, FoodConfig>();

            Config.Add(ItemNames.Snickers, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_choc_meto"),
                Type = ItemTypes.Food,
                Weight = 100,
                DisplayName = "item_snickers",
                Image = "snickers",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99), 

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,                
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 8,
                    ThirstIncrease = 0,
                    Hp = 5,
                }
            });
            
            Config.Add(ItemNames.KitKat, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_choc_meto"),
                Type = ItemTypes.Food,
                Weight = 100,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_kitkat",
                Image = "kitkat",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 8,
                    ThirstIncrease = 0,
                    Hp = 15,
                }
            }); 
            
            Config.Add(ItemNames.Gum, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_bar_beans"),
                Type = ItemTypes.Food,
                Weight = 50,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_gum",
                Image = "gum",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 2,
                    ThirstIncrease = 0,
                    Hp = 2,
                }
            }); 
            
            Config.Add(ItemNames.HotDog, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_cs_hotdog_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_hotdog",
                Image = "hotdog",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.96),

                SceneName = Scenes.Configs.SceneNames.HotDog,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 10,
                    ThirstIncrease = 0,
                    Hp = 10,
                }
            }); 
            
            Config.Add(ItemNames.Burger, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_cs_burger_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_burger",
                Image = "burger",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.97),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 15,
                    ThirstIncrease = 0,
                    Hp = 17,
                }
            }); 
            
            Config.Add(ItemNames.Pizza, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_pizza_box_01"),
                Type = ItemTypes.Food,
                Weight = 500,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_pizza",
                Image = "pizza",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.98),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 15,
                    ThirstIncrease = 0,
                    Hp = 17,
                }
            }); 
            
            Config.Add(ItemNames.Crisps, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("v_ret_ml_chips1"),
                Type = ItemTypes.Food,
                Weight = 300,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_crisps",
                Image = "crisps",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.97),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 5,
                    ThirstIncrease = 0,
                    Hp = 3,
                }
            }); 
            
            Config.Add(ItemNames.Sandwich, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_sandwich_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_sandwich",
                Image = "sandwich",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99),

                SceneName = Scenes.Configs.SceneNames.Sandwich,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 8,
                    ThirstIncrease = 0,
                    Hp = 7,
                }
            }); 
            
            Config.Add(ItemNames.ChickenFele, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_cs_steak"),
                Type = ItemTypes.Food,
                Weight = 500,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_chickenfele",
                Image = "chickenfele",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.98),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 17,
                    ThirstIncrease = 0,
                    Hp = 17,
                }
            }); 
            
            Config.Add(ItemNames.BeefSteak, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_cs_steak"),
                Type = ItemTypes.Food,
                Weight = 500,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_beefsteak",
                Image = "beefsteak",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.98),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 17,
                    ThirstIncrease = 0,
                    Hp = 26,
                }
            }); 
            
            Config.Add(ItemNames.Kebab, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_food_cb_bag_01"),
                Type = ItemTypes.Food,
                Weight = 400,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_kebab",
                Image = "kebab",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 13,
                    ThirstIncrease = 0,
                    Hp = 10,
                }
            }); 
            
            Config.Add(ItemNames.AlpenGold, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_choc_ego"),
                Type = ItemTypes.Food,
                Weight = 100,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_alpengold",
                Image = "alpengold",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 8,
                    ThirstIncrease = 0,
                    Hp = 7,
                }
            }); 
            
            Config.Add(ItemNames.Banana, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_banana",
                Image = "banana",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 10,
                    ThirstIncrease = 0,
                    Hp = 8,
                }
            }); 
            
            Config.Add(ItemNames.Pumpkin, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_pumpkin",
                Image = "pumpkin",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 8,
                    ThirstIncrease = 0,
                    Hp = 8,
                }
            }); 
            
            Config.Add(ItemNames.Orange, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_orange",
                Image = "orange",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 7,
                    ThirstIncrease = 0,
                    Hp = 8,
                }
            }); 
            
            Config.Add(ItemNames.Apple, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_apple",
                Image = "apple",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 8,
                    ThirstIncrease = 0,
                    Hp = 8,
                }
            }); 
            
            Config.Add(ItemNames.Peach, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_peach",
                Image = "peach",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 8,
                    ThirstIncrease = 0,
                    Hp = 8,
                }
            }); 
            
            Config.Add(ItemNames.Melon, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_melon",
                Image = "melon",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 10,
                    ThirstIncrease = 0,
                    Hp = 8
                }
            });

            Config.Add(ItemNames.Lemon, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_lemon",
                Image = "lemon",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 3,
                    ThirstIncrease = 0,
                    Hp = 9,
                }
            });

            Config.Add(ItemNames.Cabbage, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_cabbage",
                Image = "cabbage",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 3,
                    ThirstIncrease = 0,
                    Hp = 9,
                }
            });

            Config.Add(ItemNames.Zucchini, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_zucchini",
                Image = "zucchini",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 3,
                    ThirstIncrease = 0,
                    Hp = 9,
                }
            });

            Config.Add(ItemNames.Watermelon, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_watermelon",
                Image = "watermelon",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 3,
                    ThirstIncrease = 0,
                    Hp = 9,
                }
            });

            Config.Add(ItemNames.Tomato, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_tomato",
                Image = "tomato",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 3,
                    ThirstIncrease = 0,
                    Hp = 9,
                }
            });

            Config.Add(ItemNames.Strawberry, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_strawberry",
                Image = "strawberry",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 3,
                    ThirstIncrease = 0,
                    Hp = 9,
                }
            });

            Config.Add(ItemNames.Raspberries, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_raspberries",
                Image = "raspberries",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 3,
                    ThirstIncrease = 0,
                    Hp = 9,
                }
            });

            Config.Add(ItemNames.Radish, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_radish",
                Image = "radish",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 3,
                    ThirstIncrease = 0,
                    Hp = 9,
                }
            });

            Config.Add(ItemNames.Potatoes, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_potatoes",
                Image = "potatoes",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 3,
                    ThirstIncrease = 0,
                    Hp = 9,
                }
            });

            Config.Add(ItemNames.Cucumber, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_cucumber",
                Image = "cucumber",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 3,
                    ThirstIncrease = 0,
                    Hp = 9,
                }
            });

            Config.Add(ItemNames.Carrot, new FoodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_01"),
                Type = ItemTypes.Food,
                Weight = 250,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_carrot",
                Image = "carrot",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.87),

                SceneName = Scenes.Configs.SceneNames.Burger,
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 3,
                    ThirstIncrease = 0,
                    Hp = 9,
                }
            });

        }
    }
}
