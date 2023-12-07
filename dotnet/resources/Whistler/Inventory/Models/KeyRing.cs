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
    public class KeyRing : BaseItem
    {
        public int InventoryId { get; set; }
        public KeyRing() : base() { }
        public KeyRing(ItemNames name, bool promo, bool temporary) : base(name, 1, promo, temporary)
        {
            var inventory = new InventoryModel(Config.MaxWeight, Config.Size, InventoryTypes.KeyRing);
            InventoryId = inventory.Id;
        }

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(KeyRing));
        private static KeyRingConfig DefaultConfig(ItemNames name)
        {
            _logger.WriteWarning($"No config for: {name}");
            return new KeyRingConfig
            {
                Weight = 500,
                Stackable = true,
                DisplayName = "bad_item",
                Disposable = true,
                DropOffsetPosition = new Vector3(),
                DropRotation = new Vector3(),
                Image = "bad_item",
                ModelHash = NAPI.Util.GetHashKey("prop_box_ammo07a"),
                Type = ItemTypes.KeyRing
            };
        }

        [JsonIgnore]
        public KeyRingConfig Config { get { return (!KeyRingConfigs.Config.ContainsKey(Name)) ? DefaultConfig(Name) : KeyRingConfigs.Config[Name]; } }

        public bool HasKey(int vehId, int KeyNum)
        {
            return InventoryService.GetById(InventoryId)?.Items.FirstOrDefault(item => item != null && item.Name == ItemNames.CarKey && item is VehicleKey && (item as VehicleKey).CheckTrueVehicle(vehId, KeyNum)) != null;
        }

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
            return new List<int> { (int)Name, Count, Index, Promo ? 1 : 0, InventoryId, GetWeight() };
        }


        public override uint GetModelHash()
        {
            return Config.ModelHash;
        }

        public override int GetWeight()
        {
            var inventory = InventoryService.GetById(InventoryId);
            if (inventory == null) return Config.Weight;
            return inventory.CurrentWeight + Config.Weight;
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
            return true;
        }

        public override string GetItemLogData()
        {
            return $"{InventoryId}" + (Promo ? ",prm" : "");
        }

    }
}
