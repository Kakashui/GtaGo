using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Whistler.PersonalEvents.Achievements.AchieveModels
{
    class AchieveProgress
    {
        public int CurrentLevel { get; set; }
        public bool GivenReward { get; set; }
        public DateTime DateCompleted { get; set; }
        public bool _isChanged = false;
        public AchieveProgress(int currLevel)
        {
            CurrentLevel = currLevel;
            GivenReward = false;
            DateCompleted = DateTime.MaxValue;
        }
        public AchieveProgress(DataRow row)
        {
            CurrentLevel = Convert.ToInt32(row["currentLevel"]);
            GivenReward = Convert.ToBoolean(row["givenReward"]);
            DateCompleted = Convert.ToDateTime(row["dateCompleted"]);
        }

        public void AddProgress(int progress)
        {
            _isChanged = true;
            CurrentLevel += progress;
        }
    }
}
