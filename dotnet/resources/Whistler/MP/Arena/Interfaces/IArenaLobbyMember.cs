using GTANetworkAPI;
using Whistler.Entities;
using Whistler.MP.Arena.Enums;

namespace Whistler.MP.Arena.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IArenaLobbyMember
    {
        ExtPlayer Player { get; }
        
        TeamName Team { get; set; }
        
        IArenaLobby CurrentLobby { get; }
    }
}