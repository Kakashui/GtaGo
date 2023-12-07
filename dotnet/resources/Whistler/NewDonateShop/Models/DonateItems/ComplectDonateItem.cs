using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;

namespace Whistler.NewDonateShop.Models
{
    class ComplectDonateItems: BaseDonateItem
    {
        public ComplectDonateItems(int discount,  List<int> items)
        {
            Items = items;
            Discount = discount;
            Name = 0;
        }

        public int Name { get; set; }
        public int Discount { get; set; }
        public List<int> Items { get; set; }

        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            return false;
        }
    }
}
