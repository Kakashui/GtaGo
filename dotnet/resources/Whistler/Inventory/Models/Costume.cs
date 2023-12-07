using GTANetworkAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.ClothesCustom;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Models
{
    public class Costume : ClothesBase
    {
        public Costume() : base() { }
        public Costume(ItemNames name, bool promo, bool temporary) : base(name, promo, temporary)
        {
        }
        public Costume(ItemNames name, CostumeNames costumeModel, ClothesOwn costumeOwner, bool gender, bool promo, bool temporary) : base(name, promo, temporary)
        {
            Gender = gender;
            CostumeModel = costumeModel;
            CostumeOwner = costumeOwner;
        }
        public ClothesOwn CostumeOwner { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CostumeNames CostumeModel { get; set; }

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Costume));
        private static CostumeConfig DefaultConfig(ItemNames name)
        {
            _logger.WriteWarning($"No config for: {name}");
            return new CostumeConfig
            {
                Weight = 3000,
                CanUse = true,
                Stackable = true,
                DisplayName = "bad_item",
                Disposable = true,
                DropOffsetPosition = new Vector3(),
                DropRotation = new Vector3(),
                Image = "bad_item",
                ModelHash = NAPI.Util.GetHashKey("prop_cs_tshirt_ball_01"),
                Type = ItemTypes.Costume
            };

        }

        [JsonIgnore]
        public new CostumeConfig Config { get { return (!CostumeConfigs.Config.ContainsKey(Name)) ? DefaultConfig(Name) : CostumeConfigs.Config[Name]; } }

        public override bool Equip(ExtPlayer player)
        {
            CostumeModel config = SkinCostumeConfigs.GetConfig(CostumeModel);
            if (config == null)
                return false;
            var equip = player.GetEquip();
            if (player.GetGender() != config.Gender)
                return false;

            player.SetWhistlerCostume(CostumeModel);
            //foreach (CostumeClothesSlots slot in Enum.GetValues(typeof(CostumeClothesSlots)))
            //{
            //    var cloth = config.GetSlotClothes(slot);
            //    if (slot != CostumeClothesSlots.BodyArmor)
            //        player.SetWhistlerClothes((int)slot, cloth.Drawable, cloth.Texture);
            //}
            //foreach (CostumePropsSlots slot in Enum.GetValues(typeof(CostumePropsSlots)))
            //{
            //    var cloth = config.GetSlotProp(slot);
            //    player.SetWhistlerProps((int)slot, cloth.Drawable, cloth.Texture);
            //}
            equip.CorrectArmor(player);
            return true;
        }
        public bool SetArmor(ExtPlayer player, ClothesBase armor, int variationCycle)
        {
            CostumeModel config = SkinCostumeConfigs.GetConfig(CostumeModel);
            if (config == null)
                return false;
            if (player.GetGender() != config.Gender)
                return false;

            var cloth = config.GetSlotClothes(CostumeClothesSlots.BodyArmor);
            if (config.TypeArmor == -1)
                player.SetWhistlerClothes((int)CostumeClothesSlots.BodyArmor, cloth.Drawable, cloth.Texture);
            else
                player.SetWhistlerClothes((int)CostumeClothesSlots.BodyArmor, (armor.Drawable * variationCycle) + config.TypeArmor, armor.Texture);

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

        public override List<int> GetItemData()
        {
            return new List<int> { (int)Name, Count, Index, Promo ? 1 : 0, Gender ? 1 : 0, (int)CostumeModel };
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
        public override string GetItemLogData()
        {
            return $"{(int)CostumeModel},{(Gender ? 1 : 0)}" + (Promo ? ",prm" : "");
        }
    }
}
