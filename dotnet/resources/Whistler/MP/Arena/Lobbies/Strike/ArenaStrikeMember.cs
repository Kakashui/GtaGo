using GTANetworkAPI;
using Whistler.Entities;
using Whistler.MP.Arena.Enums;
using Whistler.MP.Arena.Interfaces;

namespace Whistler.MP.Arena.Lobbies.Implementations
{
    internal class ArenaStrikeMember : IArenaLobbyMember
    {
        public ExtPlayer Player { get; }
        
        public TeamName Team { get; set; }
        
        public IArenaLobby CurrentLobby { get; set; }

        public ArenaStrikeMember(ExtPlayer client, IArenaLobby lobby, TeamName team)
        {
            Team = team;
            Player = client;
            CurrentLobby = lobby;
        }

        public ArenaStrikeMember(ExtPlayer client)
        {
            Player = client;
        }
    }
}