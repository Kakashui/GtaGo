using GTANetworkAPI;

namespace Whistler.MP.Arena.Racing
{
    internal static class GameEventsHelper
    {
        public static bool IsPlayerInAnyRace(Player player) =>
            RacingManager.CurrentMap != null && RacingManager.CurrentMap.Players.ContainsKey(player);
    }
}