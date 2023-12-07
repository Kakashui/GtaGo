using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    public class ResourcesConfigs
    {
        public static Dictionary<ItemNames, ResourcesConfig> Config;
        static ResourcesConfigs()
        {
            Config = new Dictionary<ItemNames, ResourcesConfig>();

            Config.Add(ItemNames.CommonIron, new ResourcesConfig
            {
                ModelHash = NAPI.Util.GetHashKey("iron_bullion"),
                Type = ItemTypes.Resources,
                Weight = 1700,
                DisplayName = "item_iron_common",
                Image = "iron",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.95),

                Stackable = true
            });
            Config.Add(ItemNames.PerfectIron, new ResourcesConfig
            {
                ModelHash = NAPI.Util.GetHashKey("iron_bullion"),
                Type = ItemTypes.Resources,
                Weight = 1900,
                DisplayName = "item_iron_perfect",
                Image = "iron_perfect",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.95),

                Stackable = true
            });

            Config.Add(ItemNames.CommonAluminum, new ResourcesConfig
            {
                ModelHash = NAPI.Util.GetHashKey("iron_bullion"),
                Type = ItemTypes.Resources,
                Weight = 550,
                DisplayName = "item_aluminum_common",
                Image = "aluminum",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.95),

                Stackable = true
            });
            Config.Add(ItemNames.PerfectAluminum, new ResourcesConfig
            {
                ModelHash = NAPI.Util.GetHashKey("iron_bullion"),
                Type = ItemTypes.Resources,
                Weight = 650,
                DisplayName = "item_aluminum_perfect",
                Image = "aluminum_perfect",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.95),

                Stackable = true
            });

            Config.Add(ItemNames.CommonCopper, new ResourcesConfig
            {
                ModelHash = NAPI.Util.GetHashKey("iron_bullion"),
                Type = ItemTypes.Resources,
                Weight = 2000,
                DisplayName = "item_copper_common",
                Image = "copper",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.95),

                Stackable = true
            });
            Config.Add(ItemNames.PerfectCopper, new ResourcesConfig
            {
                ModelHash = NAPI.Util.GetHashKey("iron_bullion"),
                Type = ItemTypes.Resources,
                Weight = 2300,
                DisplayName = "item_copper_perfect",
                Image = "copper_perfect",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.95),

                Stackable = true
            });

            Config.Add(ItemNames.Diamond, new ResourcesConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_coke_cutblock_01"),
                Type = ItemTypes.Resources,
                Weight = 100,
                DisplayName = "item_diamond",
                Image = "diamond",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.99),

                Stackable = true
            });
        }
    }
}
