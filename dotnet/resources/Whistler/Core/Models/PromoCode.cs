using System.Linq;
using Whistler.Entities;

namespace Whistler.Core.Models
{
    internal class PromoCode
    {
        public int Level;
        public readonly string Name;
        public readonly int OwnerUuid;
        public ExtPlayer Owner = null;
        public int ActivatedCount = 0;
        public int UsedCount = 0;
        public readonly RewardData Reward; 
        public bool Changed = false;

        public PromoCode(int level, string name, int ownerUuid, int activatedCount, int usedCount, int money, int donate)
        {
            Level = level;
            Name = name;
            OwnerUuid = ownerUuid;
            ActivatedCount = activatedCount;
            UsedCount = usedCount;
            Reward = new RewardData(money, donate);
        }
    }
}
