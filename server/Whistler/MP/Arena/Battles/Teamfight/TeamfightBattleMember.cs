using System;
using GTANetworkAPI;
using Whistler.Helpers;
using Whistler.MP.Arena.Lobbies;

namespace Whistler.MP.Arena.Battles.Teamfight
{
    internal class TeamfightBattleMember : StrikeBattleMember
    {
        public TeamfightBattleMemberState CurrentState { get; set; }
        
        public override void Spawn()
        {
            CurrentState = TeamfightBattleMemberState.Alive;
            Player.Health = 100;
            
            CurrentBattle.Location.DismissSpawnPoint(CurrentSpawn);
            CurrentSpawn = CurrentBattle.Location.RequestRandomFreeSpawnPoint(Team);
            
            Player.ChangePosition(null);
            NAPI.Player.SpawnPlayer(Player, CurrentSpawn.Position, CurrentSpawn.Heading);
            GiveWeapon((CurrentBattle.Lobby as StrikeLobby).Settings.AvailableWeapon);
        }
    }
}