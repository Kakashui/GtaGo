using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.CustomSync.Attachments;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    public class ItemBoxConfigs
    {
        public static Dictionary<ItemNames, ItemBoxConfig> Config;
        static ItemBoxConfigs()
        {
            Config = new Dictionary<ItemNames, ItemBoxConfig>();

            Config.Add(ItemNames.WeaponBox, new ItemBoxConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_box_ammo03a"),
                AttachId = AttachId.SupplyBox,
                Type = ItemTypes.ItemBox,
                Weight = 200000,
                DisplayName = "item_weaponbox",
                Image = "weaponbox",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1),
                AvailableItem = ItemTypes.Weapon,
                CountGetItem = 1,

                Stackable = false,
                Disposable = false
            });

            Config.Add(ItemNames.AmmoBox, new ItemBoxConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_box_ammo03a"),
                AttachId = AttachId.SupplyBox,
                Type = ItemTypes.ItemBox,
                Weight = 200000,
                DisplayName = "item_ammobox",
                Image = "ammobox",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1),
                AvailableItem = ItemTypes.Ammo,
                CountGetItem = 1500,

                Stackable = false,
                Disposable = false
            });

            Config.Add(ItemNames.MedkitBox, new ItemBoxConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_box_ammo03a"),
                AttachId = AttachId.SupplyBox,
                Type = ItemTypes.ItemBox,
                Weight = 200000,
                DisplayName = "item_medkitbox",
                Image = "medkitbox",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1),
                AvailableItem = ItemTypes.Medicaments,
                CountGetItem = 5,

                Stackable = false,
                Disposable = false
            });

            Config.Add(ItemNames.ArmorBox, new ItemBoxConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_box_ammo03a"),
                AttachId = AttachId.SupplyBox,
                Type = ItemTypes.ItemBox,
                Weight = 200000,
                DisplayName = "item_armorbox",
                Image = "armorbox",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1),
                AvailableItem = ItemTypes.Clothes,
                CountGetItem = 5,

                Stackable = false,
                Disposable = false
            });

        }
    }
}
