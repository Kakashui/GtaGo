using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.Character;
using Whistler.Entities;
using Whistler.Families.Models;
using Whistler.PersonalEvents.Achievements;

namespace Whistler.PersonalEvents.Models.Rewards
{
    abstract class RewardBase
    {
        public string Desc { get; set; }
        public string Image { get; set; }
        public int Value { get; set; }
        public RewardBase(string desc, int value, string image)
        {
            Desc = desc;
            Value = value;
            Image = image;
        }

        public abstract bool GiveReward(ExtPlayer player, string commentParam);
        public abstract bool GiveReward(int playerUUID, string commentParam);
        public virtual bool GiveReward(Family family, string commentParam)
        {
            return false;
        }

    }
}
