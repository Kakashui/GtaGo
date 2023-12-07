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
    class FoodDonateItem : BaseDonateItem
    {
        public FoodDonateItem(ItemNames name)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Food) throw new Exception($"Donate item config: bad food {name}");
            Name = name;
            Stackable = true;
        }
        public ItemNames Name { get; set; }

        public override bool TryUse(Player player, int count, bool sell)
        {
            var item = ItemsFabric.CreateFood(Name, count, false);
            return TryAddToInventory(player, item);
        }
    }
}
