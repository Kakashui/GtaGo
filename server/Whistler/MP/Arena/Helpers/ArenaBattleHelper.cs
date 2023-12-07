using System.Linq;
using GTANetworkAPI;
using Whistler.MP.Arena.Battles;
using Whistler.MP.Arena.Enums;

namespace Whistler.MP.Arena.Helpers
{
    internal static class ArenaBattleHelper
    {
        public static int GetKillogColorId(TeamName team) => team switch
            {
                TeamName.Green => 1,
                TeamName.Red => 5,
                _ => 4
        };

        public static bool IsPlayerInAnyBattle(Player player) => 
            BattleManager.Battles.Any(l => l.Value.Members.Any(m => m.Player == player));

        public static bool IsPlayersInSameBattle(Player firstPlayer, Player secondPlayer)
        {
            var firstBattle = BattleManager.GetPlayerBattle(firstPlayer, out var firstMemberModel);
            var secondBattle = BattleManager.GetPlayerBattle(secondPlayer, out var secondMemberModel);

            return firstBattle != null && secondBattle != null && firstBattle == secondBattle;
        }
    }
}