using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.MP.RoyalBattle.Models
{
    class StageModel
    {
        public int Range { get; set; }
        public int Time { get; set; }
        public int TimeForConstriction { get; set; }
        public StageModel(int range, int time, int timeForConstriction)
        {
            Range = range;
            Time = time;
            TimeForConstriction = timeForConstriction;
        }
    }
}
