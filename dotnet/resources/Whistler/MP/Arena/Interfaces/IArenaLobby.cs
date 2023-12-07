using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.MP.Arena.Enums;

namespace Whistler.MP.Arena.Interfaces
{
    internal interface IArenaLobby
    {
        public int Id { get; set; }
        
        List<IArenaLobbyMember> Members { get; }
        
        LobbyState CurrentState { get; set; }
        
        LocationName LocationName { get; }

        IArenaLobbyMember Leader { get; set; }
        
        bool AutoStarted { get; set; }

        bool TryAddMember(ExtPlayer player, TeamName team);
        
        void DeleteMember(IArenaLobbyMember player);
    }
}