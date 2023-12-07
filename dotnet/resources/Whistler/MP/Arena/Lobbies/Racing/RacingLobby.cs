using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.MP.Arena.Enums;
using Whistler.MP.Arena.Interfaces;

namespace Whistler.MP.Arena.Lobbies
{
    internal class RacingLobby : IArenaLobby
    {
        public int Id { get; set; }
        public RacingLobbySettings Settings { get; set; }

        public List<IArenaLobbyMember> Members { get; }

        public LobbyState CurrentState { get; set; }
        
        //todo: из настроек
        public LocationName LocationName { get; }
        public IArenaLobbyMember Leader { get; set; }
        public bool AutoStarted { get; set; }

        public bool TryAddMember(ExtPlayer player, TeamName team)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteMember(IArenaLobbyMember player)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Я вам запрещаю использовать этот конструктор!
        /// Используйте <see cref="LobbyManager"/>
        /// </summary>
        public RacingLobby(int id, RacingLobbySettings settings)
        {
            Id = id;
            Settings = settings;
        }
        
        public void AddMember(ExtPlayer client)
        {
            throw new System.NotImplementedException();
        }
    }
}