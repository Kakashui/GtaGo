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
    class DrinkDonateItem : BaseDonateItem
    {
        public DrinkDonateItem(ItemNames name)
        {
            var type = Config.GetTypeByName(name);
            if (type != ItemTypes.Drink) throw new Exception($"Donate item config: bad drink {name}");
            Name = name;
            Stackable = true;
        }
        public ItemNames Name { get; set; }
        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            var item = ItemsFabric.CreateDrink(Name, count, false);
            return TryAddToInventory(player, item);
        }
    }
}
