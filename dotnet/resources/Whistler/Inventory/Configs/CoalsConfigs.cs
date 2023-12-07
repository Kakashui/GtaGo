using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    public class CoalsConfigs
    {
        public static Dictionary<ItemNames, CoalsConfig> Config;
        static CoalsConfigs()
        {
            Config = new Dictionary<ItemNames, CoalsConfig>();

            Config.Add(ItemNames.CommonCoal, new CoalsConfig
            {
                ModelHash = NAPI.Util.GetHashKey("coat"),
                Type = ItemTypes.Coals,
                Weight = 2000,
                DisplayName = "item_coal",
                Image = "coal_common",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.89),
                
                Stackable = true
            });

            Config.Add(ItemNames.BrownCoalm, new CoalsConfig
            {
                ModelHash = NAPI.Util.GetHashKey("coat"),
                Type = ItemTypes.Coals,
                Weight = 2800,
                DisplayName = "item_brown",
                Image = "brown",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.89),

                Stackable = true
            });

            Config.Add(ItemNames.Anthracite, new CoalsConfig
            {
                ModelHash = NAPI.Util.GetHashKey("coat"),
                Type = ItemTypes.Coals,
                Weight = 3400,
                DisplayName = "item_anthracite",
                Image = "anthracite",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.89),

                Stackable = true
            });
        }
    }
}
