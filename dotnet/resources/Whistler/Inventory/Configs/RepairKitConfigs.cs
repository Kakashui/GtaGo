using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    public class RepairKitConfigs
    {
        public static Dictionary<ItemNames, RepairKitConfig> Config;
        static RepairKitConfigs()
        {
            Config = new Dictionary<ItemNames, RepairKitConfig>();

            Config.Add(ItemNames.LowRepairKit, new RepairKitConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_tool_box_02"),
                Type = ItemTypes.RepairKit,
                Weight = 5000,
                DisplayName = "Ремкомплект",
                Image = "lowrepairkit",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.99), 
                
                Stackable = false
            });

        }
    }
}
