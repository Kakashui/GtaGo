using System.Linq;
using GTANetworkAPI;
using Whistler.Entities;
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

        public static bool IsPlayerInAnyBattle(ExtPlayer player)
        {
            if (!BattleManager.Battles.Any()) return false;

            foreach (Interfaces.IArenaBattle battle in BattleManager.Battles.Values)
            {
                if (!battle.Members.Any()) continue;
                if (battle.Members.FirstOrDefault(m => m.Player == player) == null) continue;
                return true;
            }

            return false;
        }

        public static bool IsPlayersInSameBattle(ExtPlayer firstPlayer, ExtPlayer secondPlayer)
        {
            var firstBattle = BattleManager.GetPlayerBattle(firstPlayer, out var firstMemberModel);
            var secondBattle = BattleManager.GetPlayerBattle(secondPlayer, out var secondMemberModel);

            return firstBattle != null && secondBattle != null && firstBattle == secondBattle;
        }
    }
}