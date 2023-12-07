using GTANetworkAPI;
using Whistler.Entities;

namespace Whistler.MP.Arena.Racing
{
    internal static class GameEventsHelper
    {
        public static bool IsPlayerInAnyRace(ExtPlayer player)
        {
            if (RacingManager.CurrentMap == null) return false;

            return RacingManager.CurrentMap.Players.ContainsKey(player);
        }
    }
}