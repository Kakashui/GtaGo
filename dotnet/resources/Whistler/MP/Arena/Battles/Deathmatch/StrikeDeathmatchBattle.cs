using System.Linq;
using Newtonsoft.Json;
using Whistler.MP.Arena.Enums;
using Whistler.MP.Arena.Interfaces;
using Whistler.MP.Arena.Lobbies;
using Whistler.MP.Arena.Locations;
using Whistler.SDK;

namespace Whistler.MP.Arena.Battles
{
    internal sealed class StrikeDeathmatchBattle : StrikeBattle<DeathmatchBattleMember>
    {
        public const int BattleDurationInMilliseconds = 5 * 60 * 1000; 
        
        public StrikeDeathmatchBattle(StrikeLobby lobby, ArenaLocation location) : base(lobby, location)
        { }

        protected override void OnBattleStarted()
        {
            if (!Lobby.AutoStarted) Timers.StartOnce(BattleDurationInMilliseconds, FinishBattle);
        }

        protected override void StartForMember(IBattleMember member)
        {
            member.Team = TeamName.Unknown;
            // Отправляем ID-шники врагов
            SafeTrigger.ClientEvent(member.Player,"arena:tracking:start", 
                JsonConvert.SerializeObject(Members
                    .Where(e => e.Team == member.Team && e != member)
                    .Select(t => t.Player.Value)));
        }

        protected override void OnBattleMemberKilled(DeathmatchBattleMember killer, DeathmatchBattleMember victim, uint reason)
        {
            victim.Spawn();
            killer.Player.Health = 100;
        }
    }
}