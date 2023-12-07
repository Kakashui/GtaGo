using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.ClothesCustom;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.Inventory.Models
{
    public class DefaultClothes
    {
        public DefaultClothes(ClothesBase male, ClothesBase female)
        {
            _male = male;
            _female = female;
        }
              

        private ClothesBase _male;
        private ClothesBase _female;

        public int GetDrawable(ExtPlayer player)
        {
            var gender = player.GetGender();
            return gender ? _male.Drawable : _female.Drawable;
        }

        public int GetDrawable(bool gender)
        {
            return gender ? _male.Drawable : _female.Drawable;
        }

        public bool Set(ExtPlayer player)
        {
            return player.GetGender() ? _male.Equip(player) : _female.Equip(player);
        }

        internal int GetTexture(bool gender)
        {
            return gender ? _male.Texture : _female.Texture;
        }
    }
}
