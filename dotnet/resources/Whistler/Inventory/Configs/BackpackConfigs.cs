using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    static class BackpackConfigs
    {
        public static Dictionary<ItemNames, BackpackConfig> Config;
        static BackpackConfigs()
        {
            Config = new Dictionary<ItemNames, BackpackConfig>();
            Config.Add(ItemNames.BackpackLight, new BackpackConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_michael_backpack"),
                Type = ItemTypes.Backpack,
                ComponentId = 5,
                Weight = 1000,
                Size = 20,
                MaxWeight = 50000,
                Slots = new List<ClothesSlots> { ClothesSlots.Bag },
                DisplayName = "item_bp_light",
                Image = "bplight",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.92), 
                DropRotation = new Vector3(80, 90, 0), 
            });

            Config.Add(ItemNames.BackpackMedium, new BackpackConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_michael_backpack"),
                Type = ItemTypes.Backpack,
                ComponentId = 5,
                Weight = 1500,
                Size = 40,
                MaxWeight = 100000,
                Slots = new List<ClothesSlots> { ClothesSlots.Bag },
                DisplayName = "item_bp_medium",
                Image = "bpmedium",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.92), 
                DropRotation = new Vector3(80, 90, 0), 
            });

            Config.Add(ItemNames.BackpackLarge, new BackpackConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_michael_backpack"),
                Type = ItemTypes.Backpack,
                ComponentId = 5,
                Weight = 2000,
                Size = 80,
                MaxWeight = 150000,
                Slots = new List<ClothesSlots> { ClothesSlots.Bag },
                DisplayName = "item_bp_large",
                Image = "bplarge",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.92), 
                DropRotation = new Vector3(80, 90, 0), 
            });
        }
    }
}
