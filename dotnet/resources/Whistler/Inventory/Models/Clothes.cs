using System;
using System.Collections.Generic;
using Whistler.ClothesCustom;
using Whistler.Entities;
using Whistler.Inventory.Enums;
using Whistler.SDK;

namespace Whistler.Inventory.Models
{
    public class Clothes : ClothesBase
    {
        public Clothes() : base() {
        }
        public Clothes(ItemNames name, bool promo, bool temporary) : base(name, promo, temporary)
        {
            if(name == ItemNames.BodyArmor)
            {
                Armor = 100;
            }
        }
        public Clothes(ItemNames name, bool gender, int drawable, int texture, bool promo, bool temporary) : base(name, promo, temporary)
        {
            if (name == ItemNames.BodyArmor)
            {
                Armor = 100;
            }
            Drawable = drawable;
            Texture = texture;
            Gender = gender;
        }

        public override List<int> GetItemData()
        {
            return new List<int> { (int)Name, Count, Index, Promo ? 1 : 0, Gender ? 1 : 0, Drawable, Texture};
        }

        public override bool Equip(ExtPlayer player)
        {
            var ComponentId = Config.ComponentId;
            Equip equip = player.GetEquip();
            if (Name == ItemNames.Shirt || Name == ItemNames.Top || Name == ItemNames.Gloves)
            {
                equip.CorrectClothes(player);
            }
            else if (Name == ItemNames.BodyArmor)
            {
                if (equip.Clothes[ClothesSlots.BodyArmor] != null)
                {
                    player.Session.NextArmor = DateTime.Now.AddSeconds(30);
                    player.Armor = Armor;
                }
                else player.Armor = 0;
                SafeTrigger.SetData(player, "armour:last", this);
                equip.CorrectArmor(player);
            }
            else
            {
                player.SetWhistlerClothes(ComponentId, Drawable, Texture);
            }

            if (Name == ItemNames.Mask && (Drawable < 500 || Drawable > 506))
            {
                var state = equip.Clothes[ClothesSlots.Mask] != null;
                if(!player.HasSharedData("IS_MASK") || player.GetSharedData<bool>("IS_MASK") != state)
                {
                    SafeTrigger.SetSharedData(player, "IS_MASK", state);
                    player.Character.Customization?.SetMaskFace(player, state);
                }               
            }
           
            return true;
        }
        public override string GetItemLogData()
        {
            return $"{Drawable},{Texture},{(Gender ? 1 : 0)}" + (Promo ? ",prm" : "");
        }

    }
}
