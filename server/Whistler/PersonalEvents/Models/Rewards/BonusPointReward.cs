using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.Character;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.PersonalEvents.Models.Rewards
{
    class BonusPointReward : RewardBase
    {
        public BonusPointReward(int count) : base("Bonus Point", count, "BonusReward")
        {

        }
        public override bool GiveReward(PlayerGo player, string commentParam)
        {
            return player.ChangeBonusPoints(Value);
        }
        public override bool GiveReward(int playerUUID, string commentParam)
        {
            var player = Main.GetPlayerByUUID(playerUUID);
            if (player.IsLogged())
                return GiveReward(player, commentParam);
            else
            {
                Character.AddOfflineBonusPoint(playerUUID, Value);
                return true;
            }
        }
    }
}
