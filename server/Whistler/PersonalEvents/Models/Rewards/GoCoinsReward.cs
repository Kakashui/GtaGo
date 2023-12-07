using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.nAccount;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.PersonalEvents.Models.Rewards
{
    class GoCoinsReward : RewardBase
    {
        public GoCoinsReward(int coins) : base("Coins", coins, "CoinsReward")
        {

        }
        public override bool GiveReward(PlayerGo player, string commentParam)
        {
            return player.AddGoCoins(Value);
        }
        public override bool GiveReward(int playerUUID, string commentParam)
        {
            var player = Main.GetPlayerByUUID(playerUUID);
            if (player.IsLogged())
                return GiveReward(player, commentParam);
            else
            {
                return Account.AddOffGoCoins(playerUUID, Value);
            }
        }
    }
}
