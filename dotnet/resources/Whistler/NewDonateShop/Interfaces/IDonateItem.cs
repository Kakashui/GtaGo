using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;

namespace Whistler.NewDonateShop.Interfaces
{
    interface IDonateItem
    {
        public bool TryUse(ExtPlayer player);
    }
}
