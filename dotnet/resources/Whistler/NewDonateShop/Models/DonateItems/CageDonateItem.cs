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
    class CageDonateItem : BaseDonateItem
    {
        public CageDonateItem(ItemNames name)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Cage) throw new Exception($"Donate item config: bad cage {name}");
            Name = name;
        }
        public ItemNames Name { get; set; }
        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            var item = ItemsFabric.CreateCage(Name, false);
            return TryAddToInventory(player, item);
        }
    }
}
