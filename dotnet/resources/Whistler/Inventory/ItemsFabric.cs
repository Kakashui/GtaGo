using System;
using System.Collections.Generic;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;

namespace Whistler.Inventory
{
    public static class ItemsFabric
    {
        private static int _serialHash = (int)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        internal static BaseItem CreateByName(ItemNames name)
        {
            var type = Config.GetTypeByName(name);
            switch (type)
            {
                case ItemTypes.Weapon: return new Weapon();
                case ItemTypes.Clothes: return new Clothes();
                case ItemTypes.Props: return new Props();
                case ItemTypes.Backpack:return new Backpack();
                case ItemTypes.Drink: return new Drink();
                case ItemTypes.Food: return new Food();
                case ItemTypes.Medicaments: return new Medicaments();
                case ItemTypes.Ammo: return new Ammo();
                case ItemTypes.Alcohol: return new Alcohol();
                case ItemTypes.Narcotic: return new Narcotic();
                case ItemTypes.Other: return new Other();
                case ItemTypes.Animal: return new Animal();
                case ItemTypes.Rod: return new Rod();
                case ItemTypes.Cage: return new Cage();
                case ItemTypes.GasCan: return new GasCan();
                case ItemTypes.CarKey: return new VehicleKey();
                case ItemTypes.ItemBox: return new ItemBox();
                case ItemTypes.Costume: return new Costume();
                case ItemTypes.KeyRing: return new KeyRing(ItemNames.KeyRing, false, false);
                case ItemTypes.Seed: return new Seed();
                case ItemTypes.WateringCan: return new WateringCan();
                case ItemTypes.Fertilizer: return new Fertilizer();
                case ItemTypes.Coals: return new Coals();
                case ItemTypes.Resources: return new Resources();
                case ItemTypes.Ore: return new Ore();
                case ItemTypes.RepairKit: return new RepairKit();
                default: return null;
            }
        }

        public static BaseItem CreateWeapon(ItemNames name, List<int> components, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Weapon) return null;
            return new Weapon(name, components, _serialHash++, promo, temporary);
        }
        public static BaseItem CreateWeapon(ItemNames name, int muzzle, int flash, int clip, int scope, int grip, int skin, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Weapon) return null;
            return new Weapon(name, new List<int> { muzzle, flash, clip, scope, grip, skin}, _serialHash++, promo, temporary);
        }
        public static BaseItem CreateWeapon(ItemNames name, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Weapon) return null;
            return new Weapon(name, new List<int> { -1, -1, -1, -1, -1, -1 }, _serialHash++, promo, temporary);
        }

        public static ClothesBase CreateClothes(ItemNames name, bool gender, int drawable, int texture, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Clothes && type != ItemTypes.Backpack && type != ItemTypes.Props) return null;
            switch (type)
            {
                case ItemTypes.Clothes:
                    return new Clothes(name, gender, drawable, texture, promo, temporary);
                case ItemTypes.Props:
                    return new Props(name, gender, drawable, texture, promo, temporary);
                case ItemTypes.Backpack:
                    return new Backpack(name, gender, drawable, texture, promo, true, temporary);
                default: return null;
            }
        }

        public static BaseItem CreateDrink(ItemNames name, int count, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Drink) return null;
            return new Drink(name, count, promo, temporary);
        }

        public static BaseItem CreateFood(ItemNames name, int count, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Food) return null;
            return new Food(name, count, promo, temporary);
        }

        public static BaseItem CreateMedicaments(ItemNames name, int count, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Medicaments) return null;
            return new Medicaments(name, count, promo, temporary);
        }

        public static Narcotic CreateNarcotic(ItemNames name, int count, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Narcotic) return null;
            return new Narcotic(name, count, promo, temporary);
        }

        public static Alcohol CreateAlcohol(ItemNames name, int count, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Alcohol) return null;
            return new Alcohol(name, count, promo, temporary);
        }

        public static Ammo CreateAmmo(ItemNames name, int count, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Ammo) return null;
            return new Ammo(name, count, promo, temporary);
        }

        public static Animal CreateAnimal(ItemNames name, int pedHash, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Animal) return null;
            return new Animal(name, pedHash, promo, temporary);
        }

        public static Other CreateOther(ItemNames name, int count, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Other) return null;
            return new Other(name, count, promo, temporary);
        }

        public static Rod CreateRod(ItemNames name, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Rod) return null;
            return new Rod(name, promo, temporary);
        }

        public static Cage CreateCage(ItemNames name, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Cage) return null;
            return new Cage(name, promo, temporary);
        }

        public static GasCan CreateGasCan(ItemNames name, int count, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.GasCan) return null;
            return new GasCan(name, count, promo, temporary);
        }

        public static VehicleKey CreateCarKey(ItemNames name, int vehId, int keyNumber, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.CarKey) return null;
            return new VehicleKey(name, vehId, keyNumber, promo, temporary);
        }

        public static ItemBox CreateItemBox(ItemNames name, ItemNames storedName, int countStoredItem, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            var typeStored = Config.GetTypeByName(storedName);
            if (type != ItemTypes.ItemBox || !ItemBoxConfigs.Config.ContainsKey(name) || typeStored != ItemBoxConfigs.Config[name].AvailableItem) return null;
            return new ItemBox(name, storedName, countStoredItem, _serialHash++, promo, temporary);
        }

        public static Costume CreateCostume(ItemNames name, CostumeNames costumeName, ClothesOwn costumeOwner, bool gender, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Costume) return null;
            return new Costume(name, costumeName, costumeOwner, gender, promo, temporary);
        }

        public static Seed CreateSeed(ItemNames name, int count, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Seed) return null;
            return new Seed(name, count, promo, temporary);
        }

        public static WateringCan CreateWatering(ItemNames name, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.WateringCan) return null;
            return new WateringCan(name, promo, temporary);
        }

        public static Fertilizer CreateFertilizer(ItemNames name, int count, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Fertilizer) return null;
            return new Fertilizer(name, count, promo, temporary);
        }

        public static Ore CreateOre(ItemNames name, int count, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Ore) return null;
            return new Ore(name, count, promo, temporary);
        }

        public static Coals CreateCoals(ItemNames name, int count, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Coals) return null;
            return new Coals(name, count, promo, temporary);
        }

        public static Resources CreateResources(ItemNames name, int count, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Resources) return null;
            return new Resources(name, count, promo, temporary);
        }

        public static RepairKit CreateRepairKit(ItemNames name, bool promo, bool temporary = false)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.RepairKit) return null;
            return new RepairKit(name, promo, temporary);
        }
    }
}
