using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Customization;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.NewDonateShop.Models
{
    class CharacterCreatorDonateItem : BaseDonateItem
    {
        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            CustomizationService.SendToCreator(player, -1);
            return true;
        }
    }
}
