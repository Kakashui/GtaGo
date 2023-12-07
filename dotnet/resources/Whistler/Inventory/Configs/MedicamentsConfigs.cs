using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using GTANetworkAPI;

namespace Whistler.Inventory.Configs
{
    static class MedicamentsConfigs
    {
        public static Dictionary<ItemNames, MedicamentsConfig> Config;
        static MedicamentsConfigs()
        {
            Config = new Dictionary<ItemNames, MedicamentsConfig>();
            Config.Add(ItemNames.HealthKit, new MedicamentsConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_ld_health_pack"),
                Type = ItemTypes.Medicaments,
                Weight = 500,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_healkit",
                Image = "healhkit",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                SceneName = Scenes.Configs.SceneNames.MedKit,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0, //Еда
                    ThirstIncrease = 0, //Вода
                    Hp = 100,
                }
            });

            Config.Add(ItemNames.Bandage, new MedicamentsConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_gaffer_arm_bind"),
                Type = ItemTypes.Medicaments,
                Weight = 500,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_bandage",
                Image = "bandage",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                SceneName = Scenes.Configs.SceneNames.Bandage,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0, //Еда
                    ThirstIncrease = 0, //Вода
                    Hp = 80,
                }
            });

            Config.Add(ItemNames.Adrenalin, new MedicamentsConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_syringe_01"),
                Type = ItemTypes.Medicaments,
                Weight = 200,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_item_adrenalin",
                Image = "adrenalin",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99), 
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 100, //Еда
                    ThirstIncrease = 100, //Вода
                    Hp = 100,
                }
            });
            Config.Add(ItemNames.LowHealthKit, new MedicamentsConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_stat_pack_01"),
                Type = ItemTypes.Medicaments,
                Weight = 500,
                DropRotation = new Vector3(90, 0, 0),
                Stackable = true,
                DisplayName = "item_lowhealkit",
                Image = "lowhealhkit",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                SceneName = Scenes.Configs.SceneNames.MedKit,
                LifeActivity = new LifeActivityData
                {
                    HungerIncrease = 0, //Еда
                    ThirstIncrease = 0, //Вода
                    Hp = 30,
                }
            });

        }   
    }
}
