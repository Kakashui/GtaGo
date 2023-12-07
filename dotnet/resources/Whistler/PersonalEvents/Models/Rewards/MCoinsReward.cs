using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.nAccount;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.PersonalEvents.Models.Rewards
{
    class MCoinsReward : RewardBase
    {
        public MCoinsReward(int coins) : base("Coins", coins, "CoinsReward")
        {

        }
        public override bool GiveReward(ExtPlayer player, string commentParam)
        {
            return player.AddMCoins(Value);
        }
        public override bool GiveReward(int playerUUID, string commentParam)
        {
            var player = Trigger.GetPlayerByUuid(playerUUID);
            if (player.IsLogged())
                return GiveReward(player, commentParam);
            else
            {
                return Account.AddOffMCoins(playerUUID, Value);
            }
        }
    }
}
