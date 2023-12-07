using System;
using System.Linq;
using GTANetworkAPI;
using Whistler.MP.Arena.Events;
using System.Collections.Generic;
using Whistler.MoneySystem;
using Whistler.MP.Arena.Battles;
using Whistler.MP.Arena.Enums;
using Whistler.MP.Arena.Helpers;
using Whistler.MP.Arena.Interfaces;
using Whistler.MP.Arena.Lobbies.Implementations;
using Whistler.SDK;
using Whistler.Helpers;
using Whistler.Entities;

namespace Whistler.MP.Arena.Lobbies
{
    internal static class LobbyManager
    {
        public static IReadOnlyDictionary<int, IArenaLobby> Lobbies => 
            _lobbies.ToDictionary(key => key.Id);
        private static List<IArenaLobby> _lobbies = new List<IArenaLobby>();

        private static int _idCounter;
 
        public static event Action<IArenaLobby> LobbyCreated;
        public static event Action<IArenaLobbyMember> LobbyJoined;
        public static event Action<IArenaLobby> LobbyDeleted;
        public static event Action<IArenaLobbyMember> LobbyLeaved;
        public static event Action<IArenaLobbyMember> LobbyTeamSwitched;
        
        static LobbyManager()
        {
            StrikeLobbyEvents.LobbyCreated += CreateStrikeLobby;
            StrikeLobbyEvents.LobbyJoined += JoinToLobby;
            StrikeLobbyEvents.LobbyLeaved += LeaveLobby;
            StrikeLobbyEvents.LobbyKicked += KickFromLobby;
            StrikeLobbyEvents.LobbyStarted += StartLobby;

            Main.PlayerPreDisconnect += p =>
            {
                var lobby = ArenaLobbyHelper.GetPlayersLobbyOrDefault(p);
                if (lobby != null)
                    LeaveLobby(p, lobby);
            };
        }

        private static void StartLobby(ExtPlayer starter, IArenaLobby lobby)
        {
            if (lobby.Members.Count <= 1)
            {
                Notify.SendError(starter, "arena_dm_36");
                return;
            }
            BattleManager.CreateBattle(lobby);
            lobby.CurrentState = LobbyState.InGame;
        }

        private static void KickFromLobby(ExtPlayer owner, string victimname, IArenaLobby lobby)
        {
            var victimClient = NAPI.Pools.GetAllPlayers().FirstOrDefault(p => p.Name == victimname);
            if (victimClient == null) return;
            if (owner == victimClient) return;
            
            LeaveLobby(victimClient as ExtPlayer, lobby);
        }

        public static void LeaveLobby(ExtPlayer leaver, IArenaLobby lobby)
        {
            var member = lobby.Members.FirstOrDefault(m => m.Player == leaver);
            if (member == null) return;
            
            lobby.DeleteMember(member);
            LobbyLeaved?.Invoke(member);
            
            if ((lobby.Leader?.Player == leaver && lobby.CurrentState != LobbyState.InGame) || (!lobby.AutoStarted && !lobby.Members.Any()))
                DeleteLobby(lobby);
        }

        public static void DeleteLobby(IArenaLobby lobby)
        {
            _lobbies.Remove(lobby);
            LobbyDeleted?.Invoke(lobby);
        }
        
        private static void JoinToLobby(ExtPlayer joiner, IArenaLobby lobby, TeamName team)
        {
            var existedMember = lobby.Members.FirstOrDefault(m => m.Player == joiner);
            if (existedMember != null)
            {
                existedMember.Team = team;

                LobbyTeamSwitched?.Invoke(existedMember);
                return;
            }
            
            if (!lobby.TryAddMember(joiner, team)) return;

            LobbyJoined?.Invoke(lobby.Members.FirstOrDefault(m => m.Player == joiner));

            var existedBattle = BattleManager.Battles
                .FirstOrDefault(b => b.Value.Lobby.Id == lobby.Id).Value;

            (existedBattle as StrikeBattle<DeathmatchBattleMember>)
                ?.LoadPlayer(lobby.Members.FirstOrDefault(m => m.Player == joiner));
        }
        public static void CreateStrikeLobby(ExtPlayer creator, StrikeLobbySettings settings)
        {
            if (ArenaLobbyHelper.IsPlayerInAnyLobby(creator))
            {
                Notify.SendError(creator, "arena_dm_42");
                return;
            }
            if (!Wallet.MoneySub(creator.Character, settings.EntryBet, "Money_Entrybet"))
            {
                Notify.SendError(creator, "arena_dm_43");
                return;
            }
            var member = new ArenaStrikeMember(creator);
            var lobby = new StrikeLobby(++_idCounter, member,  settings);
            member.CurrentLobby = lobby;
            
            _lobbies.Add(lobby);
            LobbyCreated?.Invoke(lobby);
        }
        public static StrikeLobby CreateStrikeLobby(StrikeLobbySettings settings)
        {
            var lobby = new StrikeLobby(++_idCounter, settings);

            _lobbies.Add(lobby);

            return lobby;
        }
        public static void CreateRacingLobby(RacingLobbySettings settings)
        {
            var lobby = new RacingLobby(++_idCounter, settings);
            
            _lobbies.Add(lobby);
            LobbyCreated?.Invoke(lobby);
        }
    }
}