using Whistler.Entities;

namespace Whistler.Core.Models.Rewards
{
    abstract class RewardBase
    {
        public string Name;
        public int Amount;

        public RewardBase(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }

        public abstract bool GiveReward(ExtPlayer player);
    }
}
