using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Models
{
    public class Animal : BaseItem
    {
        public Animal() : base() { }
        public Animal(ItemNames name, int pedHash, bool promo, bool temporary) : base(name, 1, promo, temporary)
        {
            PedHash = pedHash;
            IsActive = false;
        }

        public int PedHash { get; set; }
        public bool IsActive { get; set; }

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Animal));
        private static AnimalConfig DefaultConfig(ItemNames name)
        {
            _logger.WriteWarning($"No config for: {name}");
            return new AnimalConfig
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
                Type = ItemTypes.Ammo
            };

        }

        [JsonIgnore]
        public AnimalConfig Config { get { return (!AnimalConfigs.Config.ContainsKey(Name)) ? DefaultConfig(Name) : AnimalConfigs.Config[Name]; } }

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
            return new List<int> { (int)Name, Count, Index, Promo ? 1 : 0, IsActive ? 1 : 0 };
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
                InventoryService.OnUseAnimal?.Invoke(player, this);
            else return false;
            return true;
        }

        public override bool CanUse(ExtPlayer player)
        {
            return true;
        }
        public override string GetItemLogData()
        {
            return $"{PedHash}" + (Promo ? ",prm" : "");
        }
    }
}
