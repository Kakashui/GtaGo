using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.MP.Arena.Battles;
using Whistler.MP.Arena.Interfaces;
using Whistler.MP.Arena.Lobbies;
using Whistler.MP.Arena.UI.DTO;
using Whistler.SDK;

namespace Whistler.MP.Arena.UI
{
    internal class ArenaUiUpdateHandler : Script
    {
        public static List<ExtPlayer> Subscribers = new List<ExtPlayer>();
        
        public ArenaUiUpdateHandler()
        {
            LobbyManager.LobbyCreated += OnLobbyCreated;
            LobbyManager.LobbyJoined += OnLobbyJoined;
            LobbyManager.LobbyDeleted += OnLobbyDeleted;
            LobbyManager.LobbyLeaved += OnLobbyLeaved;
            LobbyManager.LobbyTeamSwitched += OnLobbyTeamSwitched;
            
            BattleManager.BattleCreated += OnBattleCreated;
            Main.PlayerPreDisconnect += UnsubscribePlayer;
        }

        public static void OpenMenuForPlayer(ExtPlayer player)
        {
            SubsribePlayer(player);
            
            var mapper = MapperManager.Get();

            var list = LobbyManager.Lobbies.Values
                .Select(lobby => mapper.Map<StrikeLobbyDTO>(lobby as StrikeLobby))
                .ToList();

            SafeTrigger.ClientEvent(player,"ARENA::OPEN::GUI::SERVER", JsonConvert.SerializeObject(list));
        }
        
        public static void SubsribePlayer(ExtPlayer player)
        {
            if (!Subscribers.Contains(player))
                Subscribers.Add(player);
        }

        public static void UnsubscribePlayer(ExtPlayer player)
        {
            if (Subscribers.Contains(player))
                Subscribers.Remove(player);
        }
        
        private static void OnBattleCreated(IArenaBattle battle)
        {
            var mapper = MapperManager.Get();
            
            Subscribers.ForEach(s => SafeTrigger.ClientEvent(s, "ARENA:DEACTIVATE::LOBBY::SERVER", battle.Lobby.Id));
            
            // battle.Members.ForEach(m => m.SafeTrigger.ClientEvent(player,"arena:battle:start", 
            //     JsonConvert.SerializeObject(mapper.Map<IEnumerable<BattleMemberDTO>>(battle.Members)), 
            //     JsonConvert.SerializeObject(mapper.Map<BattleMemberDTO>(m)),
            //     battle is StrikeDeathmatchBattle));
        }

        private static void OnLobbyTeamSwitched(IArenaLobbyMember member)
        {    
            var mapper = MapperManager.Get();

            Subscribers.ForEach(s => SafeTrigger.ClientEvent(s, "ARENA::PLAYER::SWITCH::TEAM::SERVER", 
                JsonConvert.SerializeObject(mapper.Map<LobbyMemberDTO>(member))));
        }

        private static void OnLobbyLeaved(IArenaLobbyMember member)
        {
            var mapper = MapperManager.Get();
            
            Subscribers.ForEach(s => SafeTrigger.ClientEvent(s, "ARENA::LOBBY::LEAVE::SERVER", 
                JsonConvert.SerializeObject(mapper.Map<LobbyMemberDTO>(member))));
        }

        private static void OnLobbyDeleted(IArenaLobby lobby)
        {
            Subscribers.ForEach(s => SafeTrigger.ClientEvent(s, "ARENA::REMOVE::LOBBY::SERVER", lobby.Id));
        }

        private static void OnLobbyJoined(IArenaLobbyMember member)
        {
            var mapper = MapperManager.Get();
            
            Subscribers.ForEach(s => SafeTrigger.ClientEvent(s, "ARENA::JOIN::LOBBY::SERVER", 
                JsonConvert.SerializeObject(mapper.Map<LobbyMemberDTO>(member))));
        }

        private static void OnLobbyCreated(IArenaLobby lobby)
        {
            var mapper = MapperManager.Get();
            
            switch (lobby)
            {
                case StrikeLobby concreteStrikeLobby:
                    Subscribers.ForEach(s => SafeTrigger.ClientEvent(s, "ARENA::ADD::LOBBY::SERVER", 
                        JsonConvert.SerializeObject(mapper.Map<StrikeLobbyDTO>(concreteStrikeLobby))));
                    SafeTrigger.ClientEvent(lobby.Leader.Player, "ARENA::OPEN::LOBBY::SERVER", lobby.Id);
                    break;
                case RacingLobby concreteRacingLobby:
                    throw new NotImplementedException();
            }
        }
    }
}