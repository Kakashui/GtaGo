using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Inventory;
using Whistler.Inventory.Models;
using Whistler.SDK;

namespace Whistler.NewDonateShop.Models
{
    abstract class BaseDonateItem
    {
        public abstract bool TryUse(ExtPlayer player, int count, bool sell);
        private int _amount;
        public int Min { get; set; }
        public int Max { get; set; }
        public int Amount { get; set; }
        public bool Stackable { get; set; } = false;
        public bool Gender { get; set; }

        protected bool TryAddToInventory(ExtPlayer player, BaseItem item)
        {
            var inv = player.GetInventory();
            if (inv == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:error:unk".Translate(1001), 3000);
                return false;
            }
            if (!inv.CanAddItem(item))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:error:inv".Translate(), 3000);
                return false;
            }
            inv.AddItem(item);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:item:inv:add".Translate(), 3000);
            //player.UpdateCoins();
            return true;
        }

        protected int GetRandomInRange()
        {
            return new Random().Next(Min, Max + 1);
        }
    }
}
