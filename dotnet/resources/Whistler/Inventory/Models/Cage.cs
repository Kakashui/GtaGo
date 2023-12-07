using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Models
{
    public class Cage : BaseItem
    {
        public Cage() : base() { }
        public Cage(ItemNames name, bool promo, bool temporary) : base(name, 1, promo, temporary) { }

        public Dictionary<int, int> Fishings { get; set; } = new Dictionary<int, int>();

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Cage));
        private static CageConfig DefaultConfig(ItemNames name)
        {
            _logger.WriteWarning($"No config for: {name}");
            return new CageConfig
            {
                Weight = 5000,
                CanUse = true,
                Stackable = true,
                DisplayName = "bad_item",
                Disposable = true,
                DropOffsetPosition = new Vector3(),
                DropRotation = new Vector3(),
                Image = "bad_item",               
                ModelHash = NAPI.Util.GetHashKey("prop_box_ammo07a"),
                Type = ItemTypes.Cage,
                Capacity = 10
            };

        }

        [JsonIgnore]
        public CageConfig Config { get { return (!CageConfigs.Config.ContainsKey(Name)) ? DefaultConfig(Name) : CageConfigs.Config[Name]; } }

        public override bool Equip(ExtPlayer player)
        {
            return false;
        }

        public override Vector3 GetDropRotation()
        {
            return Config.DropRotation;
        }
        public override Vector3 GetDropOffset()
        {
            return Config.DropOffsetPosition;
        }

        public override List<int> GetItemData()
        {
            return new List<int> 
            { 
                (int)Name, 
                Count, 
                Index, 
                Promo ? 1 : 0, 
                Fishings.Any() ? Fishings.Values.Sum() : 0
            };
        }

        public override uint GetModelHash()
        {
            return Config.ModelHash;
        }

        public override int GetWeight()
        {
            return Config.Weight * Count;
        }

        public override bool IsStackable()
        {
            return Config.Stackable;
        }
        public override bool IsDisposable()
        {
            return Config.Disposable;
        }
        public override bool Use(ExtPlayer player)
        {
            if (Config.CanUse)
                InventoryService.OnUseItem?.Invoke(player, this);
            else return false;
            return true;
        }

        public override bool CanUse(ExtPlayer player)
        {
            return false;
        }

        public int GetFreeSpace()
        {
            if (!Fishings.Any()) return Config.Capacity;
            return Config.Capacity - Fishings.Values.Sum();
        }

        public int GetCapacity()
        {
            return Config.Capacity;
        }
        public override string GetItemLogData()
        {
            return Promo ? "prm" : "";
        }
    }
}
