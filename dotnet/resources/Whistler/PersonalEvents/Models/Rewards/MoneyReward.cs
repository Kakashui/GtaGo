using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.Character;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.PersonalEvents.Achievements;
using Whistler.SDK;

namespace Whistler.PersonalEvents.Models.Rewards
{
    class MoneyReward : RewardBase
    {
        public MoneyReward(int money) : base("Money", money, "MoneyReward")
        {

        }
        public override bool GiveReward(ExtPlayer player, string commentParam)
        {
            return MoneySystem.Wallet.MoneyAdd(player.Character, Value, "Money_Achieve".Translate(commentParam));
        }
        public override bool GiveReward(Whistler.Families.Models.Family family, string commentParam)
        {
            return MoneySystem.Wallet.MoneyAdd(family, Value, "Money_Achieve".Translate(commentParam));
        }
        public override bool GiveReward(int playerUUID, string commentParam)
        {
            var player = Trigger.GetPlayerByUuid(playerUUID);
            if (player.IsLogged())
                return GiveReward(player, commentParam);
            else
            {
                return MoneySystem.Wallet.MoneyAdd(BankManager.GetAccountByUUID(playerUUID), Value, "Money_Achieve".Translate(commentParam));
            }
        }
    }
}
