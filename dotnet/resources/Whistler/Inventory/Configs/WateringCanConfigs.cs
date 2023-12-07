using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using Whistler.Scenes.Configs;

namespace Whistler.Inventory.Configs
{
    public class WateringCanConfigs
    {
        public static Dictionary<ItemNames, WateringCanConfig> Config;
        static WateringCanConfigs()
        {
            Config = new Dictionary<ItemNames, WateringCanConfig>();

            Config.Add(ItemNames.WateringBig, new WateringCanConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_wateringcan"),
                Type = ItemTypes.WateringCan,
                Weight = 1000,
                DisplayName = "item_wateringbig",
                Image = "wateringbig",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9), 
                SceneName = SceneNames.WateringSeed,
                MaxWater = 10000,

                Stackable = false,
                Disposable = false
            });

            Config.Add(ItemNames.WateringMedium, new WateringCanConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_wateringcan"),
                Type = ItemTypes.WateringCan,
                Weight = 800,
                DisplayName = "item_wateringmedium",
                Image = "wateringmedium",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9), 
                SceneName = SceneNames.WateringSeed,
                MaxWater = 6000,

                Stackable = false,
                Disposable = false
            });

            Config.Add(ItemNames.WateringLow, new WateringCanConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_wateringcan"),
                Type = ItemTypes.WateringCan,
                Weight = 500,
                DisplayName = "item_wateringlow",
                Image = "wateringlow",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9), 
                SceneName = SceneNames.WateringSeed,
                MaxWater = 3000,

                Stackable = false,
                Disposable = false
            });


        }
    }
}
