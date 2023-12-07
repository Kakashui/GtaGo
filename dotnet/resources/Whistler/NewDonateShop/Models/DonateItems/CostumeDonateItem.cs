using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Inventory;
using Whistler.Inventory.Enums;

namespace Whistler.NewDonateShop.Models
{
    class CostumeDonateItem : BaseDonateItem
    {
        public CostumeDonateItem(CostumeNames costume, bool gender)
        {
            Costume = costume;
            Gender = gender;
        }
        public CostumeNames Costume { get; set; }

        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            var item = ItemsFabric.CreateCostume(ItemNames.StandartCostume, Costume, ClothesOwn.Donate, Gender, false);
            return TryAddToInventory(player, item);
        }
    }
}
