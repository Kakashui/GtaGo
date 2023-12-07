using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Enums;

namespace Whistler.MP.RoyalBattle.Models
{
    class BattleDropModel
    {
        public ItemNames Name { get; set; }
        public int MinCount { get; set; }
        public int MaxCount { get; set; }
        public int Probability { get; set; }
        private static Random _rnd = new Random();
        public BattleDropModel(ItemNames name, int probability, int minCount = 1, int maxCount = 1)
        {
            Name = name;
            MinCount = minCount;
            MaxCount = maxCount;
            if (MinCount < 1)
                MinCount = 1;
            Probability = probability;
        }

        public int GetCount()
        {
            if (MaxCount < MinCount)
                return 1;
            return _rnd.Next(MinCount, MaxCount + 1);
        }
    }
}
