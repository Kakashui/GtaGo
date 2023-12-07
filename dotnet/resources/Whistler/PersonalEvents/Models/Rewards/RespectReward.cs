using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.Character;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.PersonalEvents.Models.Rewards
{
    class RespectReward : RewardBase
    {
        public RespectReward(int count) : base("Respect", count, "RespectReward")
        {

        }
        public override bool GiveReward(ExtPlayer player, string commentParam)
        {
            return player.Character.ChangeRespectPoint(Value);
        }
        public override bool GiveReward(int playerUUID, string commentParam)
        {
            var player = Trigger.GetPlayerByUuid(playerUUID);
            if (player.IsLogged())
                return GiveReward(player, commentParam);
            else
            {
                Character.AddOfflineRespectPoint(playerUUID, Value);
                return true;
            }
        }
    }
}
