using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    public class OreConfigs
    {
        public static Dictionary<ItemNames, OreConfig> Config;
        static OreConfigs()
        {
            Config = new Dictionary<ItemNames, OreConfig>();

            Config.Add(ItemNames.AluniniumOre, new OreConfig
            {
                ModelHash = NAPI.Util.GetHashKey("iron"),
                Type = ItemTypes.Ore,
                Weight = 3600,
                DisplayName = "item_aluminum_ore",
                Image = "aluminum_ore",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.89),

                Stackable = true
            });

            Config.Add(ItemNames.CopperOre, new OreConfig
            {
                ModelHash = NAPI.Util.GetHashKey("iron"),
                Type = ItemTypes.Ore,
                Weight = 13300,
                DisplayName = "item_copper_ore",
                Image = "copper_ore",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.89),

                Stackable = true
            });

            Config.Add(ItemNames.IronOre, new OreConfig
            {
                ModelHash = NAPI.Util.GetHashKey("iron"),
                Type = ItemTypes.Ore,
                Weight = 11300,
                DisplayName = "item_iron_ore",
                Image = "iron_ore",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.89),

                Stackable = true
            });
        }
    }
}
