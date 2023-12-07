using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.NewDonateShop.Models
{
    class ComplectGenderDonateItem : BaseDonateItem
    {
        public ComplectGenderDonateItem(int discount,  List<int> items, bool gender)
        {
            Items = items;
            Discount = discount;
            Gender = gender;
            Name = 0;
        }

        public int Name { get; set; }
        public int Discount { get; set; }
        public bool Gender { get; set; }
        public List<int> Items { get; set; }

        public override bool TryUse(Player player, int count, bool sell)
        {
            return false;
        }
    }
}
