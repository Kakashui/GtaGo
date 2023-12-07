using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.Core.Models.Rewards
{
    internal class EmptyReward : RewardBase
    {
        public EmptyReward(string name) : base(name, 0)
        {

        }

        public override bool GiveReward(ExtPlayer player)
        {
            GameLog.Admin("system", $"referal_empty({Name})", player.Name);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы получили награду ({Name}) за реферальную систему! Обратитесь к администрации для её получения.", 5000);
            return true;
        }
    }
}
