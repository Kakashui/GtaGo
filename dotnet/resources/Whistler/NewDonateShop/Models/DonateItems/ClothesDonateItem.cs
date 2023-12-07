using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Enums;
using Whistler.NewDonateShop.Interfaces;

namespace Whistler.NewDonateShop.Models
{
    class ClothesDonateItem: BaseDonateItem
    {
        public ClothesDonateItem(ItemNames name, int drawable, int texture, bool gender)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Clothes && type != ItemTypes.Backpack && type != ItemTypes.Props) throw new Exception($"Donate item config: bad clothes {name}");
            Name = name;
            Drawable = drawable;
            Texture = texture;
            Gender = gender;
        }

        public ItemNames Name { get; set; }
        public int Drawable { get; set; }
        public int Texture { get; set; }

        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {            
            var item = ItemsFabric.CreateClothes(Name, Gender, Drawable, Texture, false);
            return TryAddToInventory(player, item);
        }
    }
}
