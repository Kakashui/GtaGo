using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using Newtonsoft.Json;
using System.Linq;
using Whistler.Entities;

namespace Whistler.Inventory.Models
{
    public class Weapon : BaseItem
    {
        public Weapon() : base() {
        }
        public Weapon(ItemNames name, List<int> components, int serial, bool promo, bool temporary) : base(name, 1, promo, temporary)
        {
            Components = (components == null || components.Count > 6) ? new List<int> { -1, -1, -1, -1, -1, -1 } : components;
            Serial = serial;            
        }

        public int Ammo { get; set; }

        [JsonIgnore]
        public int MaxAmmo { 
            get {
                
                if (Components == null || Components.Count > 6)
                    Components = new List<int> { -1, -1, -1, -1, -1, -1 };
                if (Components.Count <= ((int)WeaponComponentSlots.Clip - 1)) return Config.MaxAmmo;
                var componentIndex = Math.Max(0,Components[(int)WeaponComponentSlots.Clip - 1]);//TODO: return
                if (Config.Components[WeaponComponentSlots.Clip].Count <= componentIndex)
                {
                    return Config.MaxAmmo;
                } else {
                    var count = Config.Components[WeaponComponentSlots.Clip][componentIndex].MaxAmmo;
                    return count > 0 ? count : Config.MaxAmmo;
                }
            } 
        }
        public int Serial { get; set; }

        public List<int> Components { get; set; }

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Weapon));
        private static WeaponConfig DefaultConfig(ItemNames name)
        {
            _logger.WriteWarning($"No config for: {name}");
            return new WeaponConfig
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
                Type = ItemTypes.Weapon,
                AmmoType = ItemNames.RiflesAmmo,
                WeaponHash = 0,
                MaxAmmo = 0,
                Slots = new List<WeaponSlots>()
            };

        }

        [JsonIgnore]
        public WeaponConfig Config { get { return (!WeaponConfigs.Config.ContainsKey(Name)) ? DefaultConfig(Name) : WeaponConfigs.Config[Name]; } }

        public override bool Equip(ExtPlayer player)
        {
            return false;
        }

        public List<int> GetWeaponData()
        {
            if (Components == null || Components.Count != 6)
                Components = new List<int> { -1, -1, -1, -1, -1, -1 };
            var data = new List<int> { (int) Name};
            data.AddRange(Components);
            if (Config.AmmoType == ItemNames.ThrowablesAmmo) data.Add(Count);
            else data.Add(Ammo);
            return data; 
        }

        public override List<int> GetItemData()
        {
            return new List<int> { (int)Name, Count, Index, Promo ? 1 : 0, Serial };
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
            return false;
        }

        public override bool CanUse(ExtPlayer player)
        {
            return true;
        }

        public override Vector3 GetDropRotation()
        {
            return Config.DropRotation;
        }
        public override Vector3 GetDropOffset()
        {
            return Config.DropOffsetPosition;
        }

        public override uint GetModelHash()
        {
            return Config.ModelHash;
        }
        public override string GetItemLogData()
        {
            return $"{Serial}" + (Promo ? ",prm" : "");
        }
    }
}
