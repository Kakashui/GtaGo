using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Fractions.PDA;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.NewDonateShop.Models
{
    class PrisonReleaseDonateItem : BaseDonateItem
    {
        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            var character = player.Character;
            if(character.ArrestDate <= DateTime.Now)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:arrest:no".Translate(), 3000);
                return false;
            }
            PoliceArrests.ReleasePlayer(player as ExtPlayer, null, 0);
            return true;

        }
    }
}
