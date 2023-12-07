using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;

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
        public List<int> Items { get; set; }

        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            return false;
        }
    }
}
