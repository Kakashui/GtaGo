using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    class AlcoholConfigs
    {
        public static Dictionary<ItemNames, AlcoholConfig> Config;
        static AlcoholConfigs()
        {
            Config = new Dictionary<ItemNames, AlcoholConfig>();
            Config.Add(ItemNames.WhiteWine, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_wine_white"),
                Type = ItemTypes.Alcohol,
                Weight = 1000,
                DisplayName = "item_whitewine",
                Image = "whitewine",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.WhiteWine,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.CameraShake, 3, 40)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealLight, 3, 40)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenBeastLaunch, 3, 40)
                    }
                },                
                LifeActivity =  new LifeActivityData
                {
                    HungerIncrease = -2,
                    ThirstIncrease = 2,
                }
            });

            Config.Add(ItemNames.RedWine, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_wine_red"),
                Type = ItemTypes.Alcohol,
                Weight = 1000,
                DisplayName = "item_redwine",
                Image = "redwine",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                DropRotation = new Vector3(0, 0, 0),
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.RedWine,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.CameraShake, 3, 40)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealLight, 3, 40)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenDrugGanja, 3, 40)
                    }
                },
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 2,
                    ThirstIncrease = 3,
                }
            });

            Config.Add(ItemNames.Negroni, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_drink_whisky"),
                Type = ItemTypes.Alcohol,
                Weight = 500,
                DisplayName = "item_negroni",
                Image = "negroni",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.Whiskey,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.CameraShake, 3, 60)
                    },
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealHight, 3, 60)
                    },
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ShotElectro, 3, 60)
                    }
                },
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 2,
                    ThirstIncrease = 3,
                }
            });

            Config.Add(ItemNames.Pinacolada, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_pinacolada"),
                Type = ItemTypes.Alcohol,
                Weight = 500,
                DisplayName = "item_pinacolada",
                Image = "pinacolada",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.Mojito,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {                  
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealMedium, 3, 60)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenBeastLaunch, 3, 60)
                    }
                },
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 2,
                    ThirstIncrease = 3
                }
            });

            Config.Add(ItemNames.Mojito, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_mojito"),
                Type = ItemTypes.Alcohol,
                Weight = 500,
                DisplayName = "item_mojito",
                Image = "mojito",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.Mojito,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {                  
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealMedium, 3, 60)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenDrugGanja, 3, 60)
                    }
                },
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 2,
                    ThirstIncrease = 3,
                }
            });

            Config.Add(ItemNames.Daiquiri, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_daiquiri"),
                Type = ItemTypes.Alcohol,
                Weight = 500,
                DisplayName = "item_daiquiri",
                Image = "daiquiri",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.Daiquiri,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.CameraShake, 3, 60)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealMedium, 3, 60)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenBeastLaunch, 3, 60)
                    }
                },
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 2,
                    ThirstIncrease = 3,
                }
            });

            Config.Add(ItemNames.TequilaSunrise, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_tequsunrise"),
                Type = ItemTypes.Alcohol,
                Weight = 500,
                DisplayName = "item_tequilasunrise",
                Image = "tequilasunrise",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.Sunrise,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealHight, 3, 100)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenDrugGanja, 3, 100)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ShotBubble, 3, 100)
                    }
                },
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 2,
                    ThirstIncrease = 3,
                }
            });

            Config.Add(ItemNames.Margarita, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_tequila"),
                Type = ItemTypes.Alcohol,
                Weight = 500,
                DisplayName = "item_margarita",
                Image = "margarita",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.Daiquiri,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.CameraShake, 3, 40)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealLight, 3, 40)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenBeastLaunch, 3, 40)
                    }
                },
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 2,
                    ThirstIncrease = 3,
                }
            });

            Config.Add(ItemNames.Cristal, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_wine_rose"),
                Type = ItemTypes.Alcohol,
                Weight = 1000,
                DisplayName = "item_cristal",
                Image = "cristal",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.Champange,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {                
                     new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealMedium, 3, 90)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenDrugGanja, 3, 90)
                    }
                },                
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 2,
                    ThirstIncrease = 3,
                }
            });

            Config.Add(ItemNames.Lambrusco, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_champ_01a"),
                Type = ItemTypes.Alcohol,
                Weight = 1000,
                DisplayName = "item_lambrusco",
                Image = "lambrusco",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.Champange,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealLight, 3, 90)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenDrugGanja, 3, 90)
                    }
                },
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 2,
                    ThirstIncrease = 3,
                }
            });

            Config.Add(ItemNames.Alexandra, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_cava"),
                Type = ItemTypes.Alcohol,
                Weight = 1000,
                DisplayName = "item_alexandra",
                Image = "alexandra",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1),
                Stackable = true,
                SceneName = Scenes.Configs.SceneNames.RedWine,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealLight, 3, 90)
                    },
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenDrugGanja, 3, 90)
                    }
                },
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 2,
                    ThirstIncrease = 3,
                }
            });

            Config.Add(ItemNames.LaurentPerrier, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_champ_01a"),
                Type = ItemTypes.Alcohol,
                Weight = 1000,
                DisplayName = "item_laurentperrier",
                Image = "laurentperrier",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.Champange,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {               
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealLight, 3, 90)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenDrugGanja, 3, 90)
                    }
                },
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 2,
                    ThirstIncrease = 3,
                }
            });

            Config.Add(ItemNames.Whiskey, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_whiskey_bottle"),
                Type = ItemTypes.Alcohol,
                Weight = 1000,
                DisplayName = "item_whiskey",
                Image = "whiskey",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.Whiskey,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.CameraShake, 3, 90)
                    },
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealHight, 3, 90)
                    },
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenBeastLaunch, 3, 90)
                    }
                },
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0,
                    ThirstIncrease = -2,
                }
            });

            Config.Add(ItemNames.Cognac, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_bottle_brandy"),
                Type = ItemTypes.Alcohol,
                Weight = 1000,
                DisplayName = "item_cognac",
                Image = "cognac",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.Cognak,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.CameraShake, 3, 90)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealHight, 3, 90)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenDrugGanja, 3, 90)
                    }
                },
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0,
                    ThirstIncrease = -2,
                }
            });

            Config.Add(ItemNames.Vodka, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_vodka_bottle"),
                Type = ItemTypes.Alcohol,
                Weight = 1000,
                DisplayName = "item_vodka",
                Image = "vodka",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1),

                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.Vodka,
                ActionsCount = 1,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.CameraShake, 3, 180)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealMedium, 3, 180)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenBeastLaunch, 3, 180)
                    }
                },
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = -6,
                    ThirstIncrease = -6,
                }
            }); ;

            Config.Add(ItemNames.Chacha, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_cherenkov_03"),
                Type = ItemTypes.Alcohol,
                Weight = 1000,
                DisplayName = "item_chacha",
                Image = "chacha",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.Chacha,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.CameraShake, 3, 180)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealLight, 3, 180)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenDrugGanja, 3, 180)
                    }
                },
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0,
                    ThirstIncrease = -2,
                }
            });

            Config.Add(ItemNames.Moonshine, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_cherenkov_01"),
                Type = ItemTypes.Alcohol,
                Weight = 1000,
                DisplayName = "item_moonshine",
                Image = "moonshine",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.Whiskey,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealLight, 3, 60)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenBeastLaunch, 3, 60)
                    }
                },
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = -2,
                    ThirstIncrease = -3,
                }
            });

            Config.Add(ItemNames.Beer, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_beer_bottle"),
                Type = ItemTypes.Alcohol,
                Weight = 500,
                DisplayName = "item_beer",
                Image = "beer",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.Beer,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.CameraShake, 3, 30)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealLight, 3, 30)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenBeastLaunch, 3, 30)
                    }
                },
                ActionsCount = 3,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = -3,
                    ThirstIncrease = 3,
                }
            });

            Config.Add(ItemNames.Tequila, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_tequila_bottle"),
                Type = ItemTypes.Alcohol,
                Weight = 1000,
                DisplayName = "item_tequila",
                Image = "tequila",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1),                 
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.Tequila,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {
                    new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.CameraShake, 3, 60)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealMedium, 3, 60)
                    }
                },
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0,
                    ThirstIncrease = -2,
                }
            });

            Config.Add(ItemNames.Rom, new AlcoholConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_rum_bottle"),
                Type = ItemTypes.Alcohol,
                Weight = 1000,
                DisplayName = "item_rom",
                Image = "rom",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = true,

                SceneName = Scenes.Configs.SceneNames.Rom,
                ActionsCount = 3,
                Effects = new List<List<PlayerEffects.EffectModel>>
                {                  
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.HealMedium, 3, 60)
                    },
                      new List<PlayerEffects.EffectModel>
                    {
                        new PlayerEffects.EffectModel(PlayerEffects.EffectNames.ScreenBeastLaunch, 3, 60)
                    }
                },
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = -2,
                    ThirstIncrease = -2,
                }
            });

        }
    }
}
