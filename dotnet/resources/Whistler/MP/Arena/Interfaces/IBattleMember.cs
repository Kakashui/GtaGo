using System;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.MP.Arena.Enums;
using Whistler.MP.Arena.Locations;

namespace Whistler.MP.Arena.Interfaces
{
    internal interface IBattleMember
    {
        ExtPlayer Player { get; set; }

        int Kills { get; set; }

        int Deaths { get; set; }

        TeamName Team { get; set; }

        IArenaBattle CurrentBattle { get; set; }
        
        ArenaLocationSpawn CurrentSpawn { get; set; }
        
        event Action Leaved;

        void Spawn();

        void Delete();
    }
}