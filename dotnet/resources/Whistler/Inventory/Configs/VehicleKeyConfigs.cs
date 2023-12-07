using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    public class VehicleKeyConfigs
    {
        public static Dictionary<ItemNames, VehicleKeyConfig> Config;
        static VehicleKeyConfigs()
        {
            Config = new Dictionary<ItemNames, VehicleKeyConfig>();

            Config.Add(ItemNames.CarKey, new VehicleKeyConfig
            {
                ModelHash = NAPI.Util.GetHashKey("p_car_keys_01"),
                Type = ItemTypes.CarKey,
                Weight = 100,
                DisplayName = "item_carkey",
                Image = "carkey",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99),

                Stackable = false,
                Disposable = false
            });

        }
    }
}
