using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory;
using Whistler.Inventory.Enums;

namespace Whistler.NewDonateShop.Models
{
    class PetDonateItem: BaseDonateItem
    {
        public PetDonateItem(int hash)
        {
            Hash = hash;
        }
        public int Hash { get; set; }
        public override bool TryUse(Player player, int count, bool sell)
        {
            var animal = ItemsFabric.CreateAnimal(ItemNames.Pet, Hash, false);
            return TryAddToInventory(player, animal);
        }

    }
}
