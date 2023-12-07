using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    public class RodConfigs
    {
        public static Dictionary<ItemNames, RodConfig> Config;
        static RodConfigs()
        {
            Config = new Dictionary<ItemNames, RodConfig>();

            Config.Add(ItemNames.LowRod, new RodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_fishing_rod_01"),
                Type = ItemTypes.Rod,
                Weight = 500,
                DisplayName = "item_lowrod",
                Image = "lowrod",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9), 
                DropRotation = new Vector3(0,0,90),
                Stackable = false,
                Disposable = false,
                Power = 0,
                CountUsing = 40
            });

            Config.Add(ItemNames.MiddleRod, new RodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_fishing_rod_01"),
                Type = ItemTypes.Rod,
                Weight = 500,
                DisplayName = "item_middlerod",
                Image = "middlerod",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                DropRotation = new Vector3(0, 0, 0),
                Stackable = false,
                Disposable = false,
                Power = 3,
                CountUsing = 60
            });

            Config.Add(ItemNames.HightRod, new RodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_fishing_rod_02"),
                Type = ItemTypes.Rod,
                Weight = 500,
                DisplayName = "item_hightrod",
                Image = "hightrod",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                DropRotation = new Vector3(0, 0, 0),
                Stackable = false,
                Disposable = false,
                Power = 7,
                CountUsing = 80
            });

            Config.Add(ItemNames.PerfectRod, new RodConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_fishing_rod_02"),
                Type = ItemTypes.Rod,
                Weight = 500,
                DisplayName = "item_perfectrod",
                Image = "perfectrod",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                DropRotation = new Vector3(0, 0, 0),
                Stackable = false,
                Disposable = false,
                Power = 10,
                CountUsing = 101
            });

        }
    }
}
