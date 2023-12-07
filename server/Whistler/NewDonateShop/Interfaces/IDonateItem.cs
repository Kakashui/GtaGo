using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.NewDonateShop.Interfaces
{
    interface IDonateItem
    {
        public bool TryUse(Player player);
    }
}
