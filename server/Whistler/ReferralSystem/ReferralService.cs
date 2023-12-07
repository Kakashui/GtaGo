using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;

namespace Whistler.ReferralSystem
{
    public static class ReferralService
    {
        public static void Init()
        {
            Main.OnPlayerReady += OnPlayerReady;
            Main.OnPlayerLevelUp += OnPlayerLevelUp;
        }

        private static void OnPlayerLevelUp(PlayerGo player)
        {
            if (player.Referrals == null) return;
        }

        private static void OnPlayerReady(PlayerGo player)
        {
            if (player.Referrals == null) return;
            player.TriggerEvent("mmenu:referals:set", player.Referrals.ReferralUUIDs.Count, player.Referrals.Code);
        }
    }
}
