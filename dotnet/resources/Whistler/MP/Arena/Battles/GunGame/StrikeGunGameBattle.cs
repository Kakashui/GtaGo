using System.Linq;
using Newtonsoft.Json;
using Whistler.Helpers;
using Whistler.MP.Arena.Enums;
using Whistler.MP.Arena.Interfaces;
using Whistler.MP.Arena.Lobbies;
using Whistler.MP.Arena.Locations;
using Whistler.SDK;

namespace Whistler.MP.Arena.Battles
{
    internal sealed class StrikeGunGameBattle : StrikeBattle<GunGameBattleMember>
    {
        public StrikeGunGameBattle(StrikeLobby lobby, ArenaLocation location) : base(lobby, location)
        { }

        protected override void OnBattleStarted()
        {
            
        }

        protected override void StartForMember(IBattleMember member)
        {
            member.Team = TeamName.Unknown;
            // Отправляем ID-шники врагов
            SafeTrigger.ClientEvent(member.Player,"arena:tracking:start", 
                JsonConvert.SerializeObject(Members
                    .Where(e => e != member)
                    .Select(t => t.Player.Value)));
        }

        protected override void OnBattleMemberKilled(GunGameBattleMember killer, GunGameBattleMember victim, uint reason)
        {
            killer.Player.SendTip("tip_gg_killed");
            if (killer.IsOnLastLevel()) FinishBattle();
            else
            {
                victim.Spawn();
                if (killer != victim) killer.NewLevel();
            }
        }
    }
}