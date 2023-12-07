using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.ClothesCustom;
using Whistler.Core;
using Whistler.Customization;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Configs.Models;
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

        public override bool Equip(PlayerGo player)
        {
            var ComponentId = Config.ComponentId;
            var equip = player.GetEquip();
            if (Name == ItemNames.Shirt || Name == ItemNames.Top || Name == ItemNames.Gloves)
            {
                equip.CorrectClothes(player);
            }
            else if (Name == ItemNames.BodyArmor)
            {
                player.Armor = (equip.Clothes[ClothesSlots.BodyArmor] != null) ? Armor : 0;
                player.SetData("armour:last", this);
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
                    player.SetSharedData("IS_MASK", state);
                    player.GetCustomization()?.SetMaskFace(player, state);
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
