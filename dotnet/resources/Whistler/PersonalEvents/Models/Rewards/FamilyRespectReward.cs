using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Families;
using Whistler.Families.FamilyWars;
using Whistler.Helpers;

namespace Whistler.PersonalEvents.Models.Rewards
{
    class FamilyRespectReward : RewardBase
    {
        public FamilyRespectReward(int count) : base("Family Respect", count, "FamilyRespectReward")
        {

        }
        public override bool GiveReward(ExtPlayer player, string commentParam)
        {
            return player.GetFamily()?.ChangeRespectPoints(Value) ?? false;
        }
        public override bool GiveReward(Families.Models.Family family, string commentParam)
        {
            return family.ChangeRespectPoints(Value);
        }
        public override bool GiveReward(int playerUUID, string commentParam)
        {
            var family = FamilyManager.GetFamilyByUUID(playerUUID);
            if (family != null)
                return GiveReward(family, commentParam);
            return false;
        }
    }
}
