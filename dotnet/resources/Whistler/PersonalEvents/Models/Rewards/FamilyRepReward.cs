using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.Character;
using Whistler.Entities;
using Whistler.Families;
using Whistler.PersonalEvents.Achievements;

namespace Whistler.PersonalEvents.Models.Rewards
{
    class FamilyRepReward : RewardBase
    {
        public FamilyRepReward(int rep) : base("", rep, "")
        {

        }
        public override bool GiveReward(ExtPlayer player, string commentParam)
        {
            FamilyManager.GetFamily(player.Character.FamilyID)?.ChangePoints(Value);
            return true;
        }
        public override bool GiveReward(Families.Models.Family family, string commentParam)
        {
            family?.ChangePoints(Value);
            return true;
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
