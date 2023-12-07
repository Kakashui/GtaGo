using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    static class KeyRingConfigs
    {
        public static Dictionary<ItemNames, KeyRingConfig> Config;
        static KeyRingConfigs()
        {
            Config = new Dictionary<ItemNames, KeyRingConfig>();
            Config.Add(ItemNames.KeyRing, new KeyRingConfig
            {
                ModelHash = NAPI.Util.GetHashKey("p_car_keys_01"),
                Type = ItemTypes.KeyRing,
                Weight = 500,
                Size = 20,
                MaxWeight = 2000,
                DisplayName = "item_keyring",
                Image = "keyring",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.92), 
                DropRotation = new Vector3(80, 90, 0),     
                Stackable = false,
                Disposable = false
            });
        }
    }
}
