using System.Linq;
using Whistler.Entities;

namespace Whistler.Core.Models
{
    internal class ReferalCode
    {
        public int Level;
        public readonly string Name;
        public readonly int OwnerUuid;
        public ExtPlayer Owner = null;
        public int ActivatedCount;
        public int UsedCount;
        public readonly RewardData Reward;
        public bool Changed = false;

        public ReferalCode(int level, string name, int ownerUuid, int activatedCount, int usedCount, int money)
        {
            Level = level;
            Name = name;
            OwnerUuid = ownerUuid;
            ActivatedCount = activatedCount;
            UsedCount = usedCount;
            Reward = new RewardData(money);
        }
    }
}
