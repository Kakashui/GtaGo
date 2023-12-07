using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.ClothesCustom;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Models
{
    public class Backpack : ClothesBase
    {
        public int InventoryId { get; set; }

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Backpack));
        private static BackpackConfig DefaultConfig(ItemNames name)
        {
            _logger.WriteWarning($"No config for: {name}");
            return new BackpackConfig
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
                Type = ItemTypes.Backpack,
                MaxWeight = 0,
                Size = 1,
                Slots = new List<ClothesSlots> { ClothesSlots.Bag },
                ComponentId = 5
            };

        }

        [JsonIgnore]
        public new BackpackConfig Config { get { return (!BackpackConfigs.Config.ContainsKey(Name)) ? DefaultConfig(Name) : BackpackConfigs.Config[Name]; } }

        public Backpack(ItemNames name, bool promo, bool temporary) : base(name, promo, temporary)
        {
            var inventory = new InventoryModel(Config.MaxWeight, Config.Size, InventoryTypes.BackPack);
            InventoryId = inventory.Id;
        }

        public Backpack() : base() { }

        public Backpack(ItemNames name, bool gender, int drawable, int texture, bool promo, bool withInventory = true, bool temporary = false) : base(name, promo, temporary)
        {           
            Gender = gender;
            Drawable = drawable;
            Texture = texture;
            if (withInventory)
            {
                var inventory = new InventoryModel(Config.MaxWeight, Config.Size, InventoryTypes.BackPack);
                InventoryId = inventory.Id;
            }
        }

        public override bool Equip(ExtPlayer player)
        {
            player.SetWhistlerClothes(Config.ComponentId, Drawable, Texture);
            return true;
        }

        public override bool Use(ExtPlayer player)
        {
            return false;
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

        public override List<int> GetItemData()
        {
            return new List<int> { (int)Name, Count, Index, Promo ? 1 : 0, InventoryId, GetWeight() };
        }
        public override string GetItemLogData()
        {
            return $"{InventoryId},{Drawable},{Texture},{(Gender ? 1 : 0)}" + (Promo ? ",prm" : "");
        }
    }
}
