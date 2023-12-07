using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using Whistler.PlayerEffects;
using Whistler.Scenes.Configs;

namespace Whistler.Inventory.Configs
{
    class NarcoticConfigs
    {
        public static Dictionary<ItemNames, NarcoticConfig> Config;
        static NarcoticConfigs()
        {
            Config = new Dictionary<ItemNames, NarcoticConfig>();

            Config.Add(ItemNames.Cocaine, new NarcoticConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_meth_bag_01"),
                Type = ItemTypes.Narcotic,
                Weight = 100,
                DisplayName = "item_cocaine",
                Image = "cocaine",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99), 
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 100, //Еда
                    ThirstIncrease = -30, //Вода
                    Hp = 100,
                }

            });

            Config.Add(ItemNames.Amphetamine, new NarcoticConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_meth_bag_01"),
                Type = ItemTypes.Narcotic,
                Weight = 100,
                DisplayName = "item_amphetamine",
                Image = "amphetamine",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99), 
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = -10,
                    ThirstIncrease = -10,
                    Hp = 50,
                }

            });

            Config.Add(ItemNames.Heroin, new NarcoticConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_syringe_01"),
                Type = ItemTypes.Narcotic,
                Weight = 100,
                DisplayName = "item_heroin",
                Image = "heroin",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99), 
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = -20,
                    ThirstIncrease = -20,
                    Hp = 70,
                }

            });

            Config.Add(ItemNames.Marijuana, new NarcoticConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_weed_bottle"),
                Type = ItemTypes.Narcotic,
                Weight = 100,
                DisplayName = "item_marijuana",
                Image = "marijuana",
                CanUse = true,
                DropRotation = new Vector3(0, 0, 0),
                Stackable = true,
                DropOffsetPosition = new Vector3(0, 0, 0.93),

                SceneName = SceneNames.Bong,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = -10,
                    ThirstIncrease = -5,
                    Hp = 20,
                },
                Effects = new List<List<EffectModel>>
                {
                    new List<EffectModel>
                    {
                        new EffectModel(EffectNames.ShotPole, 3, 120),
                        new EffectModel(EffectNames.ShotBlood, 3, 120),
                        new EffectModel(EffectNames.ShotBubble, 3, 120),
                        new EffectModel(EffectNames.ShotDust, 3, 120),
                        new EffectModel(EffectNames.ShotElectro, 3, 120),
                        new EffectModel(EffectNames.ShotFlashlight, 3, 120),
                        new EffectModel(EffectNames.ShotMetal, 3, 120)
                    },
                    new List<EffectModel>
                    {
                        new EffectModel(EffectNames.ScreenDrugGanja, 3, 120),
                    },
                    new List<EffectModel>
                    {
                        new EffectModel(EffectNames.HealLight, 2, 120),
                    }
                }
                

            });

            Config.Add(ItemNames.LSD, new NarcoticConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_drug_bottle"),
                Type = ItemTypes.Narcotic,
                Weight = 100,
                DisplayName = "item_lsd",
                Image = "lsd",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1.01), 
                
                Stackable = true,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = -30,
                    ThirstIncrease = -30,
                    Hp = 60,
                }

            });

            Config.Add(ItemNames.Ecstasy, new NarcoticConfig
            {
                ModelHash = NAPI.Util.GetHashKey("stt_prop_lives_bottle"),
                Type = ItemTypes.Narcotic,
                Weight = 100,
                DisplayName = "item_ecstasy",
                Image = "ecstasy",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.89), 
                
                Stackable = true,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = -15,
                    ThirstIncrease = -15,
                    Hp = 70,
                }

            });
            
        }
    }
}
