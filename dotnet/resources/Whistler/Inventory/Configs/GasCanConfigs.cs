using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    public class GasCanConfigs
    {
        public static Dictionary<ItemNames, OtherConfig> Config;

        static GasCanConfigs()
        {
            Config = new Dictionary<ItemNames, OtherConfig>();

            Config.Add(ItemNames.GasCan, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_jerrycan_01a"),
                Type = ItemTypes.GasCan,
                Weight = 5000,
                DisplayName = "item_gascan",
                Image = "gascan",
                CanUse = true,
                Disposable = true,
                DropOffsetPosition = new Vector3(0, 0, 1),

                Stackable = true
            });
        }
    }
}
