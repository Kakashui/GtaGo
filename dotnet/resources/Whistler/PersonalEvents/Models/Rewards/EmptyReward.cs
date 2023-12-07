using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;

namespace Whistler.PersonalEvents.Models.Rewards
{
    class EmptyReward : RewardBase
    {
        public EmptyReward(string desc, string image) : base(desc, 0, image)
        {

        }
        public override bool GiveReward(ExtPlayer player, string commentParam)
        {
            return true;
        }

        public override bool GiveReward(int playerUUID, string commentParam)
        {
            return true;
        }
    }
}
