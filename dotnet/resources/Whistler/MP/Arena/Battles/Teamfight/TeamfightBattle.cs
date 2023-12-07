using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.MP.Arena.Enums;
using Whistler.MP.Arena.Interfaces;
using Whistler.MP.Arena.Lobbies;
using Whistler.MP.Arena.Locations;
using Whistler.SDK;

namespace Whistler.MP.Arena.Battles.Teamfight
{
    internal sealed class TeamfightBattle : StrikeBattle<TeamfightBattleMember>
    {
        public Dictionary<TeamName, ushort> TeamPoints = new Dictionary<TeamName, ushort>();

        private TeamName _winnerTeam = TeamName.Unknown;
        public TeamfightBattle(StrikeLobby lobby, ArenaLocation location) : base(lobby, location)
        { }
        
        protected override void OnBattleStarted()
        {
            TeamPoints.Add(TeamName.Green, 0);
            TeamPoints.Add(TeamName.Red, 0);
        }

        protected override void StartForMember(IBattleMember member)
        {
            member.Leaved += () => OnMemberLeaved(member as TeamfightBattleMember);
            // Отправляем ID-шники тиммейтов
            SafeTrigger.ClientEvent(member.Player,"arena:tracking:start", 
                JsonConvert.SerializeObject(Members
                    .Where(e => e.Team == member.Team && e != member)
                    .Select(t => t.Player.Value)));
                
            member.Player.SendTip("tip_tf_started");
        }

        private void OnMemberLeaved(TeamfightBattleMember member)
        {
            if (!Members.Any()) return;

            // Ливнул последний живой - стартуем новый раунд
            if (member.CurrentState == TeamfightBattleMemberState.Alive 
                && ConcreteMembers
                    .Where(m => m.Team == member.Team)
                    .All(m => m.CurrentState == TeamfightBattleMemberState.Dead))
                HandleRoundFinish(member.Team == TeamName.Green ? TeamName.Red : TeamName.Green);
        }

        private void StartNextRound(TeamName winnerTeam)
        {
            Members.ForEach(m =>
            {
                m.Spawn();
                // Звук окончания раунда
                if (winnerTeam == TeamName.Red) SafeTrigger.ClientEvent(m.Player,"arena:endRound", 0);
                else SafeTrigger.ClientEvent(m.Player,"arena:endRound", 1);
                
                Notify.SendInfo(m.Player, $"Green [{TeamPoints[TeamName.Green]}] : [{TeamPoints[TeamName.Red]}] Red");
            });
        }

        private void HandleRoundFinish(TeamName winnerTeam)
        {
            TeamPoints[winnerTeam]++;
            
            if (TeamPoints[TeamName.Green] + TeamPoints[TeamName.Red] == (Lobby as StrikeLobby)?.Settings.TotalRounds)
            {
                _winnerTeam = TeamPoints[TeamName.Green] > TeamPoints[TeamName.Red] ? TeamName.Green : TeamName.Red; 
                FinishBattle();
            }
            else StartNextRound(winnerTeam);
        }
        
        protected override void OnBattleMemberKilled(TeamfightBattleMember killer, TeamfightBattleMember victim, uint reason)
        {
            victim.CurrentState = TeamfightBattleMemberState.Dead;

            if (ConcreteMembers
                .Where(m => m.Team == victim.Team)
                .All(m => m.CurrentState == TeamfightBattleMemberState.Dead))
            {
                if (killer == victim)
                    HandleRoundFinish(killer.Team == TeamName.Green ? TeamName.Red : TeamName.Green);
                else HandleRoundFinish(killer.Team);
            }
        }

        public override void CalculateWinning(List<IBattleMember> sorted)
        {
            // Доля каждого чувака из победившей команды
            int countMembers = Members.Count(m => m.Team == _winnerTeam);
            var shareFromWinning = 1 / (countMembers <= 0 ? 1 : countMembers);
            
            var averagePlayerWinning = Convert.ToInt32((Lobby as StrikeLobby)?.TotalContribution * shareFromWinning);

            foreach (var member in Members.ToList())
            {
                FinishPlayer(member);

                if (_winnerTeam == TeamName.Unknown) continue;
                
                if (member.Team == _winnerTeam)
                    Wallet.MoneyAdd(member.Player.Character, averagePlayerWinning, "Призовые за участие в соревнованиях на Арене");
            }
        }
    }
}