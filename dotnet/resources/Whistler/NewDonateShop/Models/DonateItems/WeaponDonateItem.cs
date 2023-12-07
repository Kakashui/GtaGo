using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Inventory;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Enums;

namespace Whistler.NewDonateShop.Models
{
    class WeaponDonateItem: BaseDonateItem
    {
        public WeaponDonateItem(ItemNames name)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Weapon) throw new Exception($"Donate item config: bad weapon {name}");
            Name = name;
        }
        public ItemNames Name { get; set; }

        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            var item = ItemsFabric.CreateWeapon(Name, false); 
            return TryAddToInventory(player, item);
        }
    }
}
