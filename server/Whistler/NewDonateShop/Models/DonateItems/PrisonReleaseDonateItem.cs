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
        public override bool TryUse(Player player, int count, bool sell)
        {
            var character = player.GetCharacter();
            if(character.ArrestDate <= DateTime.UtcNow)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:arrest:no".Translate(), 3000);
                return false;
            }
            PoliceArrests.ReleasePlayer(player as PlayerGo, null, 0);
            return true;

        }
    }
}
