using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Inventory;
using Whistler.Inventory.Enums;

namespace Whistler.NewDonateShop.Models
{
    class AmmoDonateItem : BaseDonateItem
    {
        public AmmoDonateItem(ItemNames name)
        {
            Name = name;
            Stackable = true;
        }
        public ItemNames Name { get; set; }

        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            var item = ItemsFabric.CreateAmmo(Name, count, false);
            return TryAddToInventory(player, item);
        }
    }
}
