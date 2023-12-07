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
        PlayerGo Player { get; }
        
        TeamName Team { get; set; }
        
        IArenaLobby CurrentLobby { get; }
    }
}