using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.Character;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.PersonalEvents.Models.Rewards
{
    class BonusPointReward : RewardBase
    {
        public BonusPointReward(int count) : base("Bonus Point", count, "BonusReward")
        {

        }
        public override bool GiveReward(ExtPlayer player, string commentParam)
        {
            return true;//player.ChangeBonusPoints(Value);
        }
        public override bool GiveReward(int playerUUID, string commentParam)
        {
            var player = Trigger.GetPlayerByUuid(playerUUID);
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
