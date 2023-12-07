using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.NewDonateShop.Models
{
    class PrimeDonateItem:BaseDonateItem
    {
        public PrimeDonateItem(int days){
            Days = days;
        }
        public int Days { get; set; }

        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            (player as ExtPlayer).AddPrime(Days);
            return true;
        }
    }
}
