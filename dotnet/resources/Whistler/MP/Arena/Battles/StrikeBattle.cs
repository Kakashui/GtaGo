using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.MP.Arena.Enums;
using Whistler.MP.Arena.Helpers;
using Whistler.MP.Arena.Interfaces;
using Whistler.MP.Arena.Lobbies;
using Whistler.MP.Arena.Locations;
using Whistler.MP.Arena.UI.DTO;
using Whistler.SDK;

namespace Whistler.MP.Arena.Battles
{
    internal abstract class StrikeBattle<TMember> : IArenaBattle where TMember : class, IBattleMember, new()
    {
        public int Id { get; }
        public List<IBattleMember> Members { get; }
        public List<TMember> ConcreteMembers => Members.Cast<TMember>().ToList();
        
        public IArenaLobby Lobby { get; }
        
        public ArenaLocation Location { get; set; }
        
        public event Action<IBattleMember, IBattleMember, uint> BattleMemberKilled;
        
        protected bool ShowRatingChange => Members.Count >= 4;

        public void InvokeBattleMemberKilled(IBattleMember killer, IBattleMember victim, uint reason)
        {
            BattleMemberKilled?.Invoke(killer, victim, reason);
        }

        private void UpdateKillog(IBattleMember killer, IBattleMember victim, uint reason)
        {
            var mapper = MapperManager.Get();

            // Если убил себя
            if (killer == victim)
            {
                killer.Deaths++;
            }
            // Если убил своего
            else if (killer.Team == victim.Team && killer.Team != TeamName.Unknown)
            {
                Notify.SendAlert(killer.Player, "strike:battle:1");
                killer.Kills--;
                victim.Deaths++;
            }
            else
            {
                killer.Kills++;
                victim.Deaths++;                
            }

            Members.ForEach(m =>
            {
                CaptureUI.AddKillogItem(m.Player, killer.Player, ArenaBattleHelper.GetKillogColorId(killer.Team),
                    victim.Player, ArenaBattleHelper.GetKillogColorId(victim.Team), reason);
                
                SafeTrigger.ClientEvent(m.Player,"ARENA::KILLOG::UPDATE::SERVER",
                    JsonConvert.SerializeObject(mapper.Map<IEnumerable<BattleMemberDTO>>(Members)));
            });
        }

        protected abstract void OnBattleStarted();
        protected abstract void StartForMember(IBattleMember member);

        protected void FinishBattle()
        {
            // Фильтруем по занятому месту 
            var sortedMembers = Members
                .OrderBy(member => member as StrikeBattleMember, new StrikeKillsToPlaceComparer())
                .ToList();
            
            if (ShowRatingChange) CalculateRating(sortedMembers);

            CalculateWinning(sortedMembers);
            
            LobbyManager.DeleteLobby(Lobby);
        }

        public virtual void CalculateWinning(List<IBattleMember> sorted)
        {
            Members.ToList().ForEach(m =>
            {
                var percentsFromWinning = sorted.IndexOf(m) switch
                {
                    0 => 0.5,// 50% банка за 1 место
                    1 => 0.3,
                    2 => 0.2,
                    _ => 0
                };
                
                var totalPayment = Convert.ToInt32((Lobby as StrikeLobby).TotalContribution * percentsFromWinning);
                if (totalPayment != 0)
                    Wallet.MoneyAdd(m.Player.Character, totalPayment, "Призовые за участие в соревнованиях на Арене");

                FinishPlayer(m);
            });
        }

        protected void FinishPlayer(IBattleMember member)
        {
            CaptureUI.DisableKillog(member.Player);
            if (!ShowRatingChange) Notify.SendAlert(member.Player, "arena_dm_37");
            BattleManager.PlayerLeave(member);
        }

        private void CalculateRating(List<IBattleMember> sorted)
        {
            Members.ForEach(m =>
            {
                var additionalPoints = sorted.IndexOf(m) switch
                {
                    0 => 3,// 3 очка за 1 место
                    1 => 2,
                    2 => 1,
                    _ => 0
                };
                
                m.Player.Character.ArenaPoints += additionalPoints;
                
                Notify.Send(m.Player, NotifyType.Info, NotifyPosition.Top, 
                    additionalPoints == 0 
                    ? "arena_dm_40"
                    : "arena_dm_41", 
                    5000);
            });
        }
        
        protected abstract void OnBattleMemberKilled(TMember killer, TMember victim, uint reason);
        
        public void OnBattleMemberDisconnected(IBattleMember member)
        {
            (member as StrikeBattleMember)?.ForceInvokeOnLeaved();
            Members.Remove(member);
        }

        public StrikeBattle(StrikeLobby lobby, ArenaLocation location)
        {
            Id = BattleManager.IdCounter++;
            Lobby = lobby;
            Location = location;
            
            Members = new List<IBattleMember>();
            
            foreach (var member in lobby.Members)
            {
                LoadPlayer(member);
            }

            BattleMemberKilled += UpdateKillog;
            BattleMemberKilled += (k, v, r) =>
            {
                OnBattleMemberKilled(k as TMember, v as TMember, r);
            };
            
            OnBattleStarted();
        }

        public void LoadPlayer(IArenaLobbyMember member)
        {
            var battleMember = new TMember
            {
                Player = member.Player,
                CurrentBattle = this,
                Team = (Lobby as StrikeLobby).Settings.BattleMode == StrikeBattleMode.TeamFight ? member.Team : TeamName.Unknown,
                //CurrentSpawn = Location.RequestRandomFreeSpawnPoint(member.Team)
            };
                
            SafeTrigger.SetData(member.Player, "dm:default:spawn", member.Player.Position);
                
            Members.Add(battleMember);
            
            CaptureUI.EnableKillLog(member.Player);

            var mapper = MapperManager.Get();
            SafeTrigger.ClientEvent(member.Player,"arena:battle:start",
                JsonConvert.SerializeObject(mapper.Map<IEnumerable<BattleMemberDTO>>(Members)),
                JsonConvert.SerializeObject(mapper.Map<BattleMemberDTO>(battleMember)),
                this is StrikeDeathmatchBattle && !(Lobby as StrikeLobby).AutoStarted);
            
            SafeTrigger.UpdateDimension(battleMember.Player,  Location.Dimension);
            battleMember.Spawn();
            
            StartForMember(battleMember);
        }
    }
}