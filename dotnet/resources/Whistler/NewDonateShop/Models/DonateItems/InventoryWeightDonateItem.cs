using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Inventory;
using Whistler.SDK;

namespace Whistler.NewDonateShop.Models
{
    class InventoryWeightDonateItem : BaseDonateItem
    {
        public InventoryWeightDonateItem(int max, int min)
        {
            Max = max;
            Min = min;
        }

        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            var inventory = player.GetInventory();
            if (inventory == null) return false;
            var amount = GetRandomInRange();
            inventory.ChangeMaxWeight(inventory.MaxWeight + amount * 1000);
            inventory.Update(player);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "dshop:item:iweight:ok".Translate(amount), 3000);
            return true;
        }
    }
}
