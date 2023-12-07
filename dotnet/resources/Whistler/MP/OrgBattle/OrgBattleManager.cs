using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.Common;
using Whistler.Entities;
using Whistler.Families;
using Whistler.MP.OrgBattle.BattleModels;

namespace Whistler.MP.OrgBattle
{
    class OrgBattleManager
    {
        private static List<BattleBase> _battles = new List<BattleBase>();
        internal static OrgBattleWithPoints CreateOrgBattleWithPoints(OrganizationType type, BattleLocation location, OrgActivityType orgActiveType, int maxPlayers = 10, int timeLength = 1200, int waitPlayersTime = 300)
        {
            var battle = new OrgBattleWithPoints(maxPlayers, timeLength, waitPlayersTime, type, location, orgActiveType);
            _battles.Add(battle);
            return battle;
        }
        internal static void RemoveBattle(BattleBase battle)
        {
            if (_battles.Contains(battle))
                _battles.Remove(battle);
        }
        internal static bool PlayerDeath(ExtPlayer player, ExtPlayer killer, uint weapon)
        {
            bool res = false;
            foreach (var item in _battles)
            {
                res = item.PlayerDeath(player, killer, weapon) || res;
            }
            return res;
        }
        internal static void OnPlayerDisconnected(ExtPlayer player)
        {
            foreach (var item in _battles)
            {
                item.PlayerDisconnected(player);
            }
        }
    }
}
