using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.Core.Models.Rewards
{
    internal class MCoinsReward : RewardBase
    {
        public MCoinsReward(int amount) : base("MCoins", amount)
        {

        }

        public override bool GiveReward(ExtPlayer player)
        {
            player.AddMCoins(Amount);
            GameLog.Admin("system", $"referal_mcoins({Amount})", player.Name);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы получили {Amount} донат валюты за реферальную систему.", 5000);
            return true;
        }
    }
}
