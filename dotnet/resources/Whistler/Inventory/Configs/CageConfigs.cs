using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    public class CageConfigs
    {
        public static Dictionary<ItemNames, CageConfig> Config;
        static CageConfigs()
        {
            Config = new Dictionary<ItemNames, CageConfig>();

            Config.Add(ItemNames.LowFishingCage, new CageConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Cage,
                Weight = 1000,
                DisplayName = "item_lowcage",
                Image = "lowcage",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.9), 
                
                Stackable = false,
                Disposable = false,
                Capacity = 20
            });

            Config.Add(ItemNames.MiddleFishingCage, new CageConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Cage,
                Weight = 1000,
                DisplayName = "item_middlecage",
                Image = "middlecage",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.9), 
                
                Stackable = false,
                Disposable = false,
                Capacity = 30
            });

            Config.Add(ItemNames.HightFishingCage, new CageConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Cage,
                Weight = 1000,
                DisplayName = "item_hightcage",
                Image = "hightcage",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.9), 
                
                Stackable = false,
                Disposable = false,
                Capacity = 40
            });
        }
    }
}
