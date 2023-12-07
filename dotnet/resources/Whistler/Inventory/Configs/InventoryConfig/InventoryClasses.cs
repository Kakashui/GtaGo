using System.Collections.Generic;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;

namespace Whistler.Inventory.Configs.InventoryConfig
{
    class InventoryClasses
    {
        private static readonly Dictionary<InventoryTypes, InventoryClass> Config;
        static InventoryClasses()
        {
            Config = new Dictionary<InventoryTypes, InventoryClass>();
            Config.Add(InventoryTypes.Invalid, new InventoryClass
            {
                _itemTypesWhiteList = new List<ItemTypes>
                {
                    ItemTypes.Invalid
                }
            });


            Config.Add(InventoryTypes.General, new InventoryClass());
            Config.Add(InventoryTypes.Personal, new InventoryClass());
            Config.Add(InventoryTypes.BackPack, new InventoryClass
            {
                _itemNamesBlackList = new List<ItemNames> { ItemNames.BackpackLarge, ItemNames.BackpackLight, ItemNames.BackpackMedium },
                _itemTypesBlackList = new List<ItemTypes> { ItemTypes.Backpack }
            });
            Config.Add(InventoryTypes.Vehicle, new InventoryClass());
            Config.Add(InventoryTypes.KeyRing, new InventoryClass { 
                _itemTypesWhiteList = new List<ItemTypes>
                {
                    ItemTypes.CarKey
                }
            });

            Config.Add(InventoryTypes.AmmoStock, new InventoryClass
            {
                _itemTypesWhiteList = new List<ItemTypes>
                {
                    ItemTypes.Ammo
                },
                _itemNamesWhiteList = new List<ItemNames>
                {
                    ItemNames.AmmoBox
                }
            });

            Config.Add(InventoryTypes.WeaponStock, new InventoryClass
            {
                _itemTypesWhiteList = new List<ItemTypes>
                {
                    ItemTypes.Weapon,
                    ItemTypes.Ammo
                },
                _itemNamesWhiteList = new List<ItemNames>
                {
                    ItemNames.WeaponBox,
                    ItemNames.AmmoBox
                }
            });

            Config.Add(InventoryTypes.ClothesStock, new InventoryClass
            {
                _itemTypesWhiteList = new List<ItemTypes>
                {
                    ItemTypes.Clothes,
                    ItemTypes.Costume,
                }
            });

            Config.Add(InventoryTypes.WeedStock, new InventoryClass
            {
                _itemNamesWhiteList = new List<ItemNames>
                {
                   ItemNames.Marijuana
                }
            });

            Config.Add(InventoryTypes.MedKit, new InventoryClass
            {
                _itemNamesWhiteList = new List<ItemNames>
                {
                    ItemNames.MedkitBox,
                    ItemNames.HealthKit,
                }
            });

            Config.Add(InventoryTypes.GangStock, new InventoryClass
            {
                _itemTypesWhiteList = new List<ItemTypes>
                {
                    ItemTypes.Weapon,
                    ItemTypes.Ammo,
                    ItemTypes.Medicaments,
                    ItemTypes.Narcotic
                },
                _itemNamesWhiteList = new List<ItemNames>
                {
                    ItemNames.WeaponBox,
                    ItemNames.AmmoBox,
                    ItemNames.ArmorBox,
                    ItemNames.MedkitBox,
                    ItemNames.HealthKit,
                    ItemNames.BodyArmor,
                    ItemNames.Marijuana
                }
            });

            Config.Add(InventoryTypes.OrganizationStock, new InventoryClass
            {
                _itemTypesWhiteList = new List<ItemTypes>
                {
                    ItemTypes.Weapon,
                    ItemTypes.Ammo,
                    ItemTypes.Medicaments
                },
                _itemNamesWhiteList = new List<ItemNames>
                {
                    ItemNames.WeaponBox,
                    ItemNames.AmmoBox,
                    ItemNames.ArmorBox,
                    ItemNames.MedkitBox,
                    ItemNames.HealthKit,
                    ItemNames.BodyArmor,
                }
            });
        }

        public static InventoryClass GetClass(InventoryTypes type)
        { 
            if (Config.ContainsKey(type)) 
                return Config[type]; 
            else 
                return Config[InventoryTypes.Invalid]; 
        }
    }
}
