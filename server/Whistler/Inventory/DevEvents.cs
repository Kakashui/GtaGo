using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Whistler.Core;
using Whistler.Inventory.Configs.ClothesDetails;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.SDK;

namespace Whistler.Inventory
{
    public class DevEvents
    {
        public DevEvents(){}

        public static void ParseConfigs()
        {
            //InventoryService.ParseWeaponHashes();
            InventoryService.parseConfig();
            InventoryService.ParseAttachWeapons();

            EquipService.ParseConfig();
            if (Directory.GetFiles("Configs/Clothes/Male").Length == 0)
            {
                OldCustomization.ParseUnderwear();
                OldCustomization.ParseTops();
                OldCustomization.ParseTorso();
                OldCustomization.ParseUnderTorso();
                OldCustomization.ParseAccessories();
                OldCustomization.ParseFeets();
                OldCustomization.ParseGlasses();
                OldCustomization.ParseGloves();
                OldCustomization.ParseHats();
                OldCustomization.ParseJewerly();
                OldCustomization.ParseLegs();
                OldCustomization.ParseMasks();
            }
        }

        [Command("scr")]
        public void screenEffect(Player player, string cycle)
        {
            player.TriggerEvent("mal:timecycle", cycle);
        }

    }
}
