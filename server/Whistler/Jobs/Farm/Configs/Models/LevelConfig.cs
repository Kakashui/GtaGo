using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Jobs.Farm.Configs.Models
{
    class LevelConfig
    {
        public PitNames BestPits { get; set; }
        public int CountPits { get; set; }
        public LevelConfig(PitNames bestPits, int countPits)
        {
            BestPits = bestPits;
            CountPits = countPits;
        }
    }
}
