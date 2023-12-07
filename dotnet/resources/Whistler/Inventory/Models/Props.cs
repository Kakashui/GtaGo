using GTANetworkAPI;
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
    public class Props : ClothesBase
    {
        public Props() : base() { } 
        public Props(ItemNames name, bool promo, bool temporary) : base(name, promo, temporary)
        {
        }
        public Props(ItemNames name, bool gender, int drawable, int texture, bool promo, bool temporary) : base(name, promo, temporary)
        {
            Drawable = drawable;
            Texture = texture;
            Gender = gender;
        }

        public override bool Equip(ExtPlayer player)
        {
            if (!player.IsLogged() || player.GetGender() != Gender) return false;
            var ComponentId = Config.ComponentId;
            player.SetWhistlerProps(ComponentId, Drawable, Texture);
            return true;
        }
        public override string GetItemLogData()
        {
            return $"{Drawable},{Texture},{(Gender ? 1 : 0)}" + (Promo ? ",prm" : "");
        }
    }
}