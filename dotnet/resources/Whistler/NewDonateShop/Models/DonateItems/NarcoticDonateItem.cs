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
    class NarcoticDonateItem : BaseDonateItem
    {
        public NarcoticDonateItem(ItemNames name)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Narcotic) throw new Exception($"Donate item config: bad narcotic {name}");
            Name = name;
            Stackable = true;
        }
        public ItemNames Name { get; set; }
        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            var item = ItemsFabric.CreateNarcotic(Name, count, false);
            return TryAddToInventory(player, item);
        }
    }
}
