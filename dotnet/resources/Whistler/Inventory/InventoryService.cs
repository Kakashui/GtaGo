using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.SDK;

namespace Whistler.Inventory
{
    public static class InventoryService
    {
        private static ConcurrentDictionary<int, InventoryModel> _inventories = new ConcurrentDictionary<int, InventoryModel>();
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(InventoryService), true);
        
        private static int _tempCounter = -2;
        public static int BasePlayerWeight { get; } = 40000;
        public static int BasePlayerSize { get; } = 36;
        public static Dictionary<uint, int> WeaponDamageConfigs { get; private set; }
        public static Dictionary<int, float> WeaponDamageModifiers { get; private set; }
        private static Dictionary<int, float> _baseWeaponDamageModifiers = new Dictionary<int, float>{ {1, 1.3f}, {2, .6f}, { 3, .5f } };

        public static Action<ExtPlayer, BaseItem> OnUseItem;
        public static Action<ExtPlayer, Animal> OnUseAnimal;
        public static Action<ExtPlayer, Seed> OnUseSeed;
        public static Action<ExtPlayer, ItemBox> OnUseItemBox;
        public static Action<ExtPlayer, WateringCan> OnUseWateringCan;
        public static Action<ExtPlayer, Fertilizer> OnUseFertilizer;
        public static Action<ExtPlayer, VehicleKey> OnUseCarKey;
        public static Action<ExtPlayer, Other> OnUseOtherItem;
        public static Action<ExtPlayer, Food> OnUseFoodItem;
        public static Action<ExtPlayer, Drink> OnUseDrinkItem;
        public static Action<ExtPlayer, Alcohol> OnUseAlcoholeItem;
        public static Action<ExtPlayer, Narcotic> OnUseNarcoticsItem;
        public static Action<ExtPlayer, Medicaments> OnUseMedicaments;
        public static Action<ExtPlayer, LifeActivityData> OnUseLifeActivityItem;

        public static JsonSerializerSettings JsonOption { get; } = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        internal static void UpdateWeaponsSetting(string config, string coef)
        {
            var confValues = JsonConvert.DeserializeObject<Dictionary<uint, int>>(config);
            var coefValues = JsonConvert.DeserializeObject< Dictionary<int, float>>(coef);
            var confChanged = false;
            var coefChanged = false;
            foreach (var item in confValues)
            {
                if (WeaponDamageConfigs.ContainsKey(item.Key))
                {
                    WeaponDamageConfigs[item.Key] = item.Value;
                    MySQL.Query("UPDATE `weapondamageconfigs` SET `baseDamage`=@prop0 WHERE `weaponHash`=@prop1", item.Value, item.Key);
                    confChanged = true;
                }
            }
            foreach (var item in coefValues)
            {
                if (WeaponDamageModifiers.ContainsKey(item.Key))
                {
                    WeaponDamageModifiers[item.Key] = item.Value;
                    MySQL.Query("UPDATE `weapondamagemodifiers` SET `value`=@prop0 WHERE `id`=@prop1", item.Value, item.Key);
                    coefChanged = true;
                }
            }
            if (confChanged)
            {
                NAPI.ClientEvent.TriggerClientEventForAll("weapon:damage:config:update", WeaponDamageConfigs);
            }
            if (coefChanged)
            {
                NAPI.ClientEvent.TriggerClientEventForAll("weapon:damage:coef:update", WeaponDamageModifiers);
            }
        }

        #region default clothing
        public static Dictionary<ClothesSlots, DefaultClothes> DefaultClothes = new Dictionary<ClothesSlots, DefaultClothes>
        {
            {
                ClothesSlots.Mask, new DefaultClothes(
                    new Clothes(ItemNames.Mask, true, 0, 0, true, false),
                    new Clothes(ItemNames.Mask, false, 0, 0, true, false)
                )
            },
            {
                ClothesSlots.Hat, new DefaultClothes(
                    new Props(ItemNames.Hat, true, -1, 0, true, false),
                    new Props(ItemNames.Hat, false, -1, 0, true, false)
                )
            },
            {
                ClothesSlots.Glasses, new DefaultClothes(
                    new Props(ItemNames.Glasses, true, -1, 0, true, false),
                    new Props(ItemNames.Glasses, false, -1, 0, true, false)
                )
            },
            {
                ClothesSlots.Ear, new DefaultClothes(
                    new Props(ItemNames.Ear, true, -1, 0, true, false),
                    new Props(ItemNames.Ear, false, -1, 0, true, false)
                )
            },
            {
                ClothesSlots.Accessories, new DefaultClothes(
                    new Clothes(ItemNames.Accessories, true, 0, 0, true, false),
                    new Clothes(ItemNames.Accessories, false, 0, 0, true, false)
                )
            },
            {
                ClothesSlots.Top, new DefaultClothes(
                    new Clothes(ItemNames.Top, true, 15, 0, true, false),
                    new Clothes(ItemNames.Top, false, 101, 0, true, false)
                )
            },
            {
                ClothesSlots.Shirt, new DefaultClothes(
                    new Clothes(ItemNames.Shirt, true, 15, 0, true, false),
                    new Clothes(ItemNames.Shirt, false, 6, 0, true, false)
                )
            },
            {
                ClothesSlots.BodyArmor, new DefaultClothes(
                    new Clothes(ItemNames.BodyArmor, true, -1, 0, true, false),
                    new Clothes(ItemNames.BodyArmor, false, -1, 0, true, false)
                )
            },
            {
                ClothesSlots.Gloves, new DefaultClothes(
                    new Clothes(ItemNames.Gloves, true, 15, 0, true, false),
                    new Clothes(ItemNames.Gloves, false, 15, 0, true, false)
                )
            },
            {
                ClothesSlots.Bracelets, new DefaultClothes(
                    new Props(ItemNames.Bracelets, true, -1, 0, true, false),
                    new Props(ItemNames.Bracelets, false, -1, 0, true, false)
                )
            },
            {
                ClothesSlots.Watches, new DefaultClothes(
                    new Props(ItemNames.Watches, true, -1, 0, true, false),
                    new Props(ItemNames.Watches, false, -1, 0, true, false)
                )
            },
            {
                ClothesSlots.Leg, new DefaultClothes(
                    new Clothes(ItemNames.Leg, true, 61, 0, true, false),
                    new Clothes(ItemNames.Leg, false, 56, 0, true, false)
                )
            },
            {
                ClothesSlots.Feet, new DefaultClothes(
                    new Clothes(ItemNames.Feet, true, 34, 0, true, false),
                    new Clothes(ItemNames.Feet, false, 35, 0, true, false)
                )
            },
            {
                ClothesSlots.Bag, new DefaultClothes(
                    new Backpack(ItemNames.BackpackLight, true, 0, 0, true, false),
                    new Backpack(ItemNames.BackpackLight, false, 0, 0, true, false)
                )
            },
            {
                ClothesSlots.Costume, new DefaultClothes(
                    new Costume(ItemNames.StandartCostume, CostumeNames.MBase, ClothesOwn.Fraction, true, false, false),
                    new Costume(ItemNames.StandartCostume, CostumeNames.FBase, ClothesOwn.Fraction, false, false, false)
                )
            }
        };

        internal static void OnPlayerSpawn(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"weapon:damage:config:update", WeaponDamageConfigs);
            SafeTrigger.ClientEvent(player,"weapon:damage:coef:update", WeaponDamageModifiers);
        }

        #endregion

        public static void InitDataBase()
        {
            var query = $"CREATE TABLE IF NOT EXISTS `inventories`(" +
              $"`id` int(11) NOT NULL AUTO_INCREMENT," +
              $"`maxWeight` int(11) NOT NULL," +
              $"`size` int(11) NOT NULL," +
              $"`items` TEXT NOT NULL," +
              $"`type` int(11) NOT NULL," +
              $"PRIMARY KEY(`id`)" +
              $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
            MySQL.QuerySync(query);
            query = $"CREATE TABLE IF NOT EXISTS `weapondamageconfigs`(" +
              $"`id` int(11) NOT NULL AUTO_INCREMENT," +
              $"`weaponHash` int(11) UNSIGNED NOT NULL," +
              $"`baseDamage` int(11) NOT NULL," +
              $"PRIMARY KEY(`id`)" +
              $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
            MySQL.QuerySync(query);
            query = $"CREATE TABLE IF NOT EXISTS `weapondamagemodifiers`(" +
              $"`id` int(11) NOT NULL," +
              $"`value` FLOAT NOT NULL," +
              $"PRIMARY KEY(`id`)" +
              $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
            MySQL.QuerySync(query);
        }

        public static InventoryModel GetById(int id)
        {
            return _inventories.GetOrAdd(id, LoadInventoryFromDB);           
        }

        private static InventoryModel LoadInventoryFromDB(int id)
        {
            if (id < 1) return null;
            var result = MySQL.QueryRead("SELECT `items`, `maxWeight`, `size`, `type` FROM `inventories` WHERE `id`=@prop0", id);
            if (result != null && result.Rows.Count > 0)
            {
                DataRow row = result.Rows[0];
                var inventory = new InventoryModel(id, Convert.ToInt32(row["maxweight"]), Convert.ToInt32(row["size"]), row["items"].ToString(), (InventoryTypes)Convert.ToInt32(row["type"]));
                return inventory;
            }
            else return null;
        }

        public static void SaveAllInventories()
        {
            Task.Run(() =>
            {
                if (!_inventories.Any()) return;

                foreach (InventoryModel inventory in _inventories.Values)
                {
                    if (inventory == null) continue;

                    inventory.Save();
                }
            });
        }

        public static int AddNew(InventoryModel inventory)
        {
            DataTable data = MySQL.QueryRead("INSERT INTO `inventories`(`items`, `maxWeight`, `size`, `type`) VALUES(@prop0, @prop1, @prop2, @prop3); SELECT @@identity;",
                JsonConvert.SerializeObject(inventory.Items, JsonOption),
                inventory.MaxWeight,
                inventory.Size,
                (int)inventory.Type
            );
            var id = Convert.ToInt32(data.Rows[0][0]);
            _inventories.TryAdd(id, inventory);
            return id;
        }

        public static int AddTemp(InventoryModel inventory)
        {
            _inventories.TryAdd(_tempCounter, inventory);
            return _tempCounter--;
        }

        private class WeaponClientConfig
        {
            public uint Hash { get; set; }
            public int AmmoType { get; set; }
            public Dictionary<int, List<uint>> Components { get; set; }
            public string Name { get; set; }
            public WeaponClientConfig(uint hash, int ammoType, Dictionary<WeaponComponentSlots, List<WeaponComponentModel>> components)
            {
                Hash = hash;
                AmmoType = ammoType;
                Components = new Dictionary<int, List<uint>>();
                foreach (var component in components)
                {
                    Components.Add((int)component.Key, component.Value.Select(k => k.Hash).ToList());
                }
            }
        }

        public static void ParseAttachWeapons()
        {
            var config = new Dictionary<int, WeaponClientConfig>();
            WeaponDamageConfigs = new Dictionary<uint, int>();
            WeaponDamageModifiers = new Dictionary<int, float>();
            var result = MySQL.QueryRead("SELECT * FROM `weapondamageconfigs`;");
            foreach (DataRow row in result.Rows)
            {
                WeaponDamageConfigs.Add(Convert.ToUInt32(row["weaponHash"]), Convert.ToInt32(row["baseDamage"]));
            }
            result = MySQL.QueryRead("SELECT * FROM `weapondamagemodifiers`;");
            foreach (DataRow row in result.Rows)
            {
                WeaponDamageModifiers.Add(Convert.ToInt32(row["id"]), Convert.ToSingle(row["value"]));
            }
            if(WeaponDamageModifiers.Count == 0)
            {
                foreach (var modifier in _baseWeaponDamageModifiers)
                {
                    WeaponDamageModifiers.Add(modifier.Key, modifier.Value);
                    MySQL.Query("INSERT INTO `weapondamagemodifiers`(`id`, `value`) VALUES(@prop0, @prop1);", modifier.Key, modifier.Value);
                }
            }
            foreach (var c in WeaponConfigs.Config)
            {
                if (!WeaponDamageConfigs.ContainsKey(c.Value.WeaponHash))
                {
                    WeaponDamageConfigs.Add(c.Value.WeaponHash, 15);
                    MySQL.Query("INSERT INTO `weapondamageconfigs`(`weaponHash`, `baseDamage`) VALUES(@prop0, @prop1);", c.Value.WeaponHash, 15);
                }
                config.Add((int)c.Key, new WeaponClientConfig(c.Value.WeaponHash, (int)c.Value.AmmoType, c.Value.Components));
            }
            if (!Directory.Exists("client/WeaponSystem")) return;
            using (var w = new StreamWriter("client/WeaponSystem/weaponConfigs.js"))
            {
                w.Write($"module.exports = {JsonConvert.SerializeObject(config, Formatting.Indented)}");
            }
        }

        public static void ClearInventoryCache(int id)
        {          
            if(_inventories.TryRemove(id, out var inv))
                inv.Save();
        }

        public static void SyncInventoryId(this ExtPlayer player)
        {
            var inventory = player.GetInventory();
            if (inventory == null) return;
            SafeTrigger.ClientEvent(player,"inv:set:personal", inventory.Id);
        }

        public static void SyncInventory(this ExtPlayer player, int id)
        {
            try
            {
                var inventory = GetById(id);
                if (inventory == null) return;
                NAPI.Task.Run(() => { SafeTrigger.ClientEvent(player,"inv:set:byid", inventory.Id, inventory.GetInventoryData(), inventory.MaxWeight, inventory.Size); });
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Error at {nameof(InventoryService)} ex: {ex}");
            }
        }

        public static void OpenStock(ExtPlayer player, int inventoryId, StockTypes type)
        {
            var inventory = GetById(inventoryId);
            if (inventory == null) return;
            inventory.Subscribe(player);
            DropSystem.Subscribe(player);
            SafeTrigger.ClientEvent(player,"inv:open:stock", inventoryId, (int)type);
        }

        public static void DestroyInventory(int id)
        {
            _inventories.TryRemove(id, out var inv);
            MySQL.Query("DELETE FROM `inventories` WHERE `id`=@prop0", id);
        }

        #region configParser
        public static void parseConfig()
        {
            if (Directory.Exists("interfaces/gui/src/configs/inventory"))
            {
                var names = "";
                var config = new Dictionary<int, object>();
                names += "\nAlcohol:\n";
                foreach (var c in AlcoholConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nAmmo:\n";
                foreach (var c in AmmoConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nBackpacks:\n";
                foreach (var c in BackpackConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nClothes:\n";
                foreach (var c in ClothesConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nDrink:\n";
                foreach (var c in DrinkConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nFood:\n";
                foreach (var c in FoodConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nMedicaments:\n";
                foreach (var c in MedicamentsConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nWeapons:\n";
                foreach (var c in WeaponConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nNarcotic:\n";
                foreach (var c in NarcoticConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nOther:\n";
                foreach (var c in OtherConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nAnimal:\n";
                foreach (var c in AnimalConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nRod:\n";
                foreach (var c in RodConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nCage:\n";
                foreach (var c in CageConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nGasCan:\n";
                foreach (var c in GasCanConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nCarKey:\n";
                foreach (var c in VehicleKeyConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nItemBox:\n";
                foreach (var c in ItemBoxConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nCostume:\n";
                foreach (var c in CostumeConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nKeyRing:\n";
                foreach (var c in KeyRingConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nSeed:\n";
                foreach (var c in SeedConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nWateringCan:\n";
                foreach (var c in WateringCanConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }
                names += "\nFertilizer:\n";
                foreach (var c in FertilizerConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }

                names += "\nOre:\n";
                foreach (var c in OreConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }

                names += "\nResources:\n";
                foreach (var c in ResourcesConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }

                names += "\nCoals:\n";
                foreach (var c in CoalsConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }

                names += "\nRepairKit:\n";
                foreach (var c in RepairKitConfigs.Config)
                {
                    names += $"{c.Key}, ";
                    config.Add((int)c.Key, c.Value);
                }

                using (var w = new StreamWriter("interfaces/gui/src/configs/inventory/configs.js"))
                {
                    w.Write($"export default {JsonConvert.SerializeObject(config, Formatting.Indented)}");
                }
            };

            if (Directory.Exists("interfaces/gui/src/store/inventory"))
            {
                Dictionary<int, string> costumes = new Dictionary<int, string>();
                foreach (var name in Enum.GetValues(typeof(CostumeNames)))
                {
                    costumes.Add((int)name, name.ToString());
                }
                using (var w = new StreamWriter("interfaces/gui/src/store/inventory/costumes.js"))
                {
                    w.Write($"export default {JsonConvert.SerializeObject(costumes)}");
                }
            };

            if (Directory.Exists("client/configs"))
            {
                var skins = SkinCostumeConfigs.GetSkins();
                Dictionary<int, CostumeModel> costumeConfigs = new Dictionary<int, CostumeModel>();
                foreach (var item in skins)
                {
                    costumeConfigs.Add((int)item.Key, item.Value);
                }
                using (var w = new StreamWriter("client/configs/costumes.js"))
                {
                    w.Write($"global.costumeConfigs = JSON.parse(`{JsonConvert.SerializeObject(costumeConfigs)}`);");
                }
            };

            //using (var w = new StreamWriter("ParsedConfigs/ItemNames.txt"))
            //{
            //    w.Write(names);
            //}
        }

        internal static InventoryModel GetByUUID(int uuid)
        {
            DataTable result = MySQL.QueryRead("SELECT `inventoryId` FROM `characters` WHERE `uuid`=@prop0", uuid);
            return result.Rows.Count > 0 ? GetById(Convert.ToInt32(result.Rows[0]["inventoryId"])) : null;

        }
        #endregion
        public static void ParseWeaponHashes(){
            var _hashes = new Dictionary<string, uint>();
            foreach (var c in WeaponConfigs.Config)
            {
                _hashes.Add(c.Key.ToString(), c.Value.WeaponHash);
            }
            using (var w = new StreamWriter("ParsedConfigs/WeaponHashes.json"))
            {
                w.Write($"{JsonConvert.SerializeObject(_hashes)}");
            }
        }
    }
}
