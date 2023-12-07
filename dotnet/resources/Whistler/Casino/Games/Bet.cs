using System.Collections.Generic;
using ServerGo.Casino.ChipModels;

namespace ServerGo.Casino.Games
{
    public class Bet
    {
        public Bet(string betName, IEnumerable<Chip> cost)
        {
            BetName = betName;
            Cost = new ChipList(cost);
        }
        public string BetName { get; }
        public ChipList Cost { get; }
    }
}