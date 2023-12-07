using Whistler.MP.Arena.Locations;

namespace Whistler.MP.Arena.Interfaces
{
    internal interface IArenaGame
    {
        IArenaLobby Lobby { get; }        
        ArenaLocation Location { get; }
    }
}