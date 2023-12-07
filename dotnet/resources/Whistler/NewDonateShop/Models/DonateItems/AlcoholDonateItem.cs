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
    class AlcoholDonateItem : BaseDonateItem
    {
        public AlcoholDonateItem(ItemNames name)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Alcohol) throw new Exception($"Donate item config: bad alcohol {name}");
            Name = name;
            Stackable = true;
        }
        public ItemNames Name { get; set; }
        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            var item = ItemsFabric.CreateAlcohol(Name, count, false);
            return TryAddToInventory(player, item);
        }
    }
}
