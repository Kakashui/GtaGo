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
    public abstract class ClothesBase : BaseItem
    {
        public bool Gender { get; set; }
        public int Drawable { get; set; }
        public int Texture { get; set; }
        public int Armor { get; set; } = 0;

        public ClothesBase(ItemNames name, bool promo, bool temporary) : base(name, 1, promo, temporary)
        {

        }
        public ClothesBase() : base(){ }

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ClothesBase));
        private static ClothesConfig DefaultConfig(ItemNames name)
        {
            _logger.WriteWarning($"No config for: {name}");
            return new ClothesConfig
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
                Type = ItemTypes.Clothes,
                ComponentId = 0,
                Slots = new List<ClothesSlots>()
            };
        }

        [JsonIgnore]
        public ClothesConfig Config { get { return (!ClothesConfigs.Config.ContainsKey(Name)) ? DefaultConfig(Name) : ClothesConfigs.Config[Name]; } }

        public override bool Equip(ExtPlayer player)
        {
            return false;
        }

        public bool AvailableForGender(bool gender)
        {
            if (Name == ItemNames.Mask || Name == ItemNames.BodyArmor || Name == ItemNames.BackpackLarge || Name == ItemNames.BackpackLight || Name == ItemNames.BackpackMedium) return true;
            return gender == Gender;
        }

        public void CorrectTorso(ExtPlayer player)
        {

        }

        public override List<int> GetItemData()
        {
            return new List<int> { (int)Name, Count, Index, Promo ? 1 : 0, Gender ? 1 : 0};
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
        public override uint GetModelHash()
        {
            return Config.ModelHash;
        }
    }
}
