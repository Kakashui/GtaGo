using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.ReferralSystem
{
    public static class ReferralService
    {
        public static void Init()
        {
            Main.OnPlayerReady += OnPlayerReady;
            Main.OnPlayerLevelUp += OnPlayerLevelUp;
        }

        private static void OnPlayerLevelUp(ExtPlayer player)
        {
            if (player.Referrals == null) return;
        }

        private static void OnPlayerReady(ExtPlayer player)
        {
            if (player.Referrals == null) return;
            SafeTrigger.ClientEvent(player,"mmenu:referals:set", player.Referrals.ReferralUUIDs.Count, player.Referrals.Code);
        }
    }
}
