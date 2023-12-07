using System.Collections.Generic;
using Whistler.MP.Arena.Battles;

namespace Whistler.MP.Arena.Helpers
{
    internal class StrikeKillsToPlaceComparer : IComparer<StrikeBattleMember>
    {
        public int Compare(StrikeBattleMember x, StrikeBattleMember y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            
            var killsComparison = x.Kills.CompareTo(y.Kills);
            if (killsComparison != 0) return killsComparison;
            
            var deathsComparison = x.Deaths.CompareTo(y.Deaths);
            if (deathsComparison != 0) return deathsComparison;
            
            return x.Player.Value.CompareTo(y.Player.Value);
        }
    }
}