using System;
using System.Collections.Generic;
using Whistler.MP.Arena.Locations;

namespace Whistler.MP.Arena.Interfaces
{
    internal interface IArenaBattle
    {
        int Id { get; }
        
        List<IBattleMember> Members { get; }

        IArenaLobby Lobby { get; }
        
        ArenaLocation Location { get; set; }
        
        event Action<IBattleMember, IBattleMember, uint> BattleMemberKilled; 
        
        void InvokeBattleMemberKilled(IBattleMember killer, IBattleMember victim, uint reason);
        
        void OnBattleMemberDisconnected(IBattleMember member);
    }
}