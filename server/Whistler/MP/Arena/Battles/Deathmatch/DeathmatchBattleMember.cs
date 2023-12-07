using GTANetworkAPI;
using Whistler.Helpers;
using Whistler.MP.Arena.Lobbies;

namespace Whistler.MP.Arena.Battles
{
    internal class DeathmatchBattleMember : StrikeBattleMember
    {
        public override void Spawn()
        {
            Player.Health = 100;
            CurrentBattle.Location.DismissSpawnPoint(CurrentSpawn);

            CurrentSpawn = CurrentBattle.Location.RequestRandomFreeSpawnPoint(Team);
            Player.ChangePosition(null);
            NAPI.Player.SpawnPlayer(Player, CurrentSpawn.Position, CurrentSpawn.Heading);
            GiveWeapon((CurrentBattle.Lobby as StrikeLobby).Settings.AvailableWeapon);
        }
    }
}