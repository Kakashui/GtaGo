using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Enums;

namespace Whistler.NewDonateShop.Models
{
    class GasCanDonateItem : BaseDonateItem
    {
        public GasCanDonateItem(ItemNames name)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.GasCan) throw new Exception($"Donate item config: bad gascan {name}");
            Name = name;
        }
        public ItemNames Name { get; set; }

        public override bool TryUse(Player player, int count, bool sell)
        {
            var item = ItemsFabric.CreateGasCan(Name, 1, false);
            return TryAddToInventory(player, item);
        }
    }
}
