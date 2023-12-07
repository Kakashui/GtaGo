using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    static class Config
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Config));
        private static Dictionary<ItemNames, ItemTypes> _list = new Dictionary<ItemNames, ItemTypes>();

        static Config()
        {
            foreach (var item in AlcoholConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in AmmoConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in BackpackConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in ClothesConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in DrinkConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in FoodConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in MedicamentsConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in WeaponConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in NarcoticConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in OtherConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in AnimalConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in RodConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in CageConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in GasCanConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in VehicleKeyConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in ItemBoxConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in CostumeConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in KeyRingConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in SeedConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in WateringCanConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in FertilizerConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in OreConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in ResourcesConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in CoalsConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            foreach (var item in RepairKitConfigs.Config)
            {
                _list.Add(item.Key, item.Value.Type);
            }
            _logger.WriteInfo($"Items config contains {_list.Count} keys.");
        }
        public static ItemTypes GetTypeByName(ItemNames name)
        {
            return _list.ContainsKey(name) ? _list[name] : ItemTypes.Invalid;
        }
    }
}
