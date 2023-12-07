using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.PersonalEvents.Achievements;
using Whistler.PersonalEvents.Achievements.AchieveModels;

namespace Whistler.PersonalEvents.DTO
{
    class AchieveProgressDTO
    {
        public int CurrentLevel { get; set; }
        public bool GivenReward { get; set; }
        public int DateCompleted { get; set; }
        public int AchieveName { get; set; }
        public AchieveProgressDTO(AchieveProgress achieve, AchieveNames achieveName)
        {
            CurrentLevel = achieve.CurrentLevel;
            GivenReward = achieve.GivenReward;
            DateCompleted = achieve.DateCompleted.GetTotalSeconds();
            AchieveName = (int)achieveName;
        }
    }
}
