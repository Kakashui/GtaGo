using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    class AmmoConfigs
    {
        public static Dictionary<ItemNames, AmmoConfig> Config;
        static AmmoConfigs()
        {
            Config = new Dictionary<ItemNames, AmmoConfig>();
            Config.Add(ItemNames.RiflesAmmo, new AmmoConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_ld_ammo_pack_03"),
                Type = ItemTypes.Ammo,
                Weight = 10,
                DisplayName = "rifleammo",
                Image = "rifleammo",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.95), 
                
                Stackable = true
            });

            Config.Add(ItemNames.PistolAmmo, new AmmoConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_ld_ammo_pack_01"),
                Type = ItemTypes.Ammo,
                Weight = 11,
                DisplayName = "pistolammo",
                Image = "pistolammo",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.96), 
                
                Stackable = true,
            });

            Config.Add(ItemNames.ShotgunsAmmo, new AmmoConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_ld_ammo_pack_02"),
                Type = ItemTypes.Ammo,
                Weight = 25,
                DisplayName = "shotgunammo",
                Image = "shotgunammo",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.94), 
                DropRotation = new Vector3(0, 0, 0),
                Stackable = true
            });

            Config.Add(ItemNames.SMGAmmo, new AmmoConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_ld_ammo_pack_01"),
                Type = ItemTypes.Ammo,
                Weight = 11,
                DisplayName = "smgammo",
                Image = "smgammo",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.96), 
                
                Stackable = true
            });

            Config.Add(ItemNames.SniperAmmo, new AmmoConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_ld_ammo_pack_03"),
                Type = ItemTypes.Ammo,
                Weight = 22,
                DisplayName = "item_ammo_sniper",
                Image = "sniperammo",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.95), 
                
                Stackable = true
            });

            Config.Add(ItemNames.MusketAmmo, new AmmoConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_ld_ammo_pack_02"),
                Type = ItemTypes.Ammo,
                Weight = 25,
                DisplayName = "item_ammo_musket",
                Image = "musketammo",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.95),

                Stackable = true
            });
        }
    }
}
