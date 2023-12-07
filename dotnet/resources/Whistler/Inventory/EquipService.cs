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
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.SDK;

namespace Whistler.Inventory
{
    public static class EquipService
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(EquipService), true);
        private static ConcurrentDictionary<int, Equip> _equips = new ConcurrentDictionary<int, Equip>();
        private static int _tempCounter = -2;
        public static void InitDataBase()
        {
            var query = $"CREATE TABLE IF NOT EXISTS `equips`(" +
                 $"`id` int(11) NOT NULL AUTO_INCREMENT," +
                 $"`clothes` TEXT NOT NULL," +
                 $"`weapons` TEXT NOT NULL," +
                 $"PRIMARY KEY(`id`)" +
                 $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
            MySQL.Query(query);
        }

        public static Equip GetById(int id)
        {
            return _equips.GetOrAdd(id, LoadEquipFromDB);
        }
        private static Equip LoadEquipFromDB(int id)
        {
            if (id < 1) return null;
            var result = MySQL.QueryRead("SELECT `clothes`, `weapons` FROM `equips` WHERE `id`=@prop0", id);
            if (result != null && result.Rows.Count > 0)
            {
                DataRow row = result.Rows[0];
                var equip = new Equip(id, row["clothes"].ToString(), row["weapons"].ToString());
                return equip;
            }
            else return null;
        }

        public static void SaveAllEquips()
        {
            Task.Run(() =>
            {
                if (!_equips.Any()) return;

                foreach (Equip equip in _equips.Values)
                {
                    if (equip == null) continue;

                    equip.Save();
                }
            });
        }

        public static void GiveStartEquip(ExtPlayer player, bool gender)
        {
            var equip = player.GetEquip();
            if (equip == null) return;
            BaseItem link = null;

            if (equip.Clothes[ClothesSlots.Top] == null)
                equip.EquipItem(player, new Clothes(ItemNames.Top, gender, 0, 0, true, false), ClothesSlots.Top, ref link);

            if (equip.Clothes[ClothesSlots.Leg] == null)
                equip.EquipItem(player, new Clothes(ItemNames.Leg, gender, 0, 1, true, false), ClothesSlots.Leg, ref link);

            if (equip.Clothes[ClothesSlots.Feet] == null)
                equip.EquipItem(player, new Clothes(ItemNames.Feet, gender, 1, 0, true, false), ClothesSlots.Feet, ref link);
        }

        public static int AddNew(Equip equip)
        {
            var data = MySQL.QueryRead("INSERT INTO `equips`(`clothes`, `weapons`) VALUES(@prop0, @prop1); SELECT @@identity;",
               JsonConvert.SerializeObject(equip.Clothes, InventoryService.JsonOption),
               JsonConvert.SerializeObject(equip.Weapons, InventoryService.JsonOption)
           );
            var id = Convert.ToInt32(data.Rows[0][0]);
            _equips.TryAdd(id, equip);
            return id;
        }

        public static int AddTemp(Equip equip)
        {
            int id = _tempCounter--;
            _equips.TryAdd(id, equip);
            return id;
        }

        public static void ParseConfig()
        {
            var equip = new Equip();
            using (var w = new StreamWriter("ParsedConfigs/Equip.json"))
            {
                w.Write(JsonConvert.SerializeObject(equip.GetEquipData(), Formatting.Indented));
            }
        }

        public static void DestroyEquip(int id)
        {
            _equips.TryRemove(id, out var _);
            MySQL.Query("DELETE FROM `equips` WHERE `id`=@prop0", id);
        }

        internal static Equip GetByUUID(int uuid)
        {
            var result = MySQL.QueryRead("SELECT `equipId` FROM `characters` WHERE `uuid`=@prop0", uuid);
            return result.Rows.Count > 0 ? GetById(Convert.ToInt32(result.Rows[0]["equipId"])) : null;
        }
    }
}
