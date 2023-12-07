using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.NewDonateShop.Models
{
    class PhoneNumberDonateItem: BaseDonateItem
    {
        public PhoneNumberDonateItem(int length, int count)
        {
            NumberLength = length;
            CountDigits = count;
        }
        public int NumberLength { get; set; }
        public int CountDigits { get; set; }

        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "dshop:item:admin:temp".Translate(), 3000);
            return false;
        }
    }
}
