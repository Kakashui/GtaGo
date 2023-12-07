using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Common;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using Whistler.SDK;
using Whistler.VehicleSystem;

namespace Whistler.Inventory.Models
{
    public class Resources : BaseItem
    {
        public Resources() : base() { }
        public Resources(ItemNames name, int count, bool promo, bool temporary) : base(name, count, promo, temporary)
        {

        }

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Resources));
        private static ResourcesConfig DefaultConfig(ItemNames name)
        {
            _logger.WriteWarning($"No config for: {name}");
            return new ResourcesConfig
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
                Type = ItemTypes.Other
            };
        }

        [JsonIgnore]
        public ResourcesConfig Config { get { return (!ResourcesConfigs.Config.ContainsKey(Name)) ? DefaultConfig(Name) : ResourcesConfigs.Config[Name]; } }

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
            return new List<int> { (int)Name, Count, Index, Promo ? 1 : 0 };
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
            return true;
        }

        public override bool CanUse(ExtPlayer player)
        {
            return Config.CanUse;
        }

        public override string GetItemLogData()
        {
            return Promo ? "prm" : "";
        }
    }
}
