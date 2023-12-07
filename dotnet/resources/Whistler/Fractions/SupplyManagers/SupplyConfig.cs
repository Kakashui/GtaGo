using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;

namespace Whistler.Fractions.SupplyManagers
{
    static class SupplyConfig
    {
        private static List<ItemData> RandomWeapon = new List<ItemData>
        {

            new ItemData(ItemNames.SMG,            50),
            new ItemData(ItemNames.AssaultSMG,     50),
            new ItemData(ItemNames.CombatPDW,      50),
            new ItemData(ItemNames.CombatMG,       2, 5),
            new ItemData(ItemNames.Gusenberg,      50),
            new ItemData(ItemNames.AssaultRifle,   50),
            new ItemData(ItemNames.CarbineRifle,   50),
            new ItemData(ItemNames.AdvancedRifle,  50),
            new ItemData(ItemNames.SpecialCarbine, 50),
            new ItemData(ItemNames.PumpShotgun,    50),
            new ItemData(ItemNames.HeavyShotgun,   2, 5),
            new ItemData(ItemNames.HeavySniper ,   1, 2),
        };
        private static List<ItemData> RandomAmmo = new List<ItemData>
        {
            new ItemData(ItemNames.PistolAmmo,   50),
            new ItemData(ItemNames.SMGAmmo,      100),
            new ItemData(ItemNames.RiflesAmmo,   100),
            new ItemData(ItemNames.ShotgunsAmmo, 50),
        };
        private static ItemData MedKit = new ItemData(ItemNames.HealthKit, 50);

        public static ItemBox GetRandomTypeBox(ItemNames boxName)
        {
            ItemData data = null;
            switch (boxName)
            {
                case ItemNames.WeaponBox:
                    data = RandomWeapon.GetRandomElement();
                    break;
                case ItemNames.AmmoBox:
                    data = RandomAmmo.GetRandomElement();
                    break;
                case ItemNames.MedkitBox:
                    data = MedKit;
                    break;
            }
            if (data != null)
                return ItemsFabric.CreateItemBox(boxName, data.Name, data.GetCount(), false);
            return null;
        }
    }
    class ItemData
    {
        private static Random _rnd = new Random();
        public ItemNames Name { get; }
        public int Count { get ; }
        public int MaxCount { get ; }
        public ItemData(ItemNames name, int count, int maxCount = -1)
        {
            Name = name;
            Count = count;
            MaxCount = maxCount <= 0 ? count : maxCount+1;
        }

        public int GetCount()
        {
            return Count < MaxCount ? _rnd.Next(Count, MaxCount) : Count;
        }
    }
}
