using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Whistler.Helpers;
using Whistler.MP.Arena.Configs;
using Whistler.MP.Arena.Enums;

namespace Whistler.MP.Arena.Battles
{
    internal class GunGameBattleMember : StrikeBattleMember
    {
        private Queue<WeaponHash> _weapons;
        public WeaponHash CurrentWeapon => _weapons.Peek();
        
        public GunGameBattleMember()
        {
            _weapons = new Queue<WeaponHash>();
            
            foreach (var weapon in StrikeConfig.AvailableForSelectWeaponsForLoby) _weapons.Enqueue(weapon);
        }
        
        public override void Spawn()
        {
            CurrentBattle.Location.DismissSpawnPoint(CurrentSpawn);

            CurrentSpawn = CurrentBattle.Location.RequestRandomFreeSpawnPoint(Team);
            Player.Health = 100;
            Player.ChangePosition(null);
            NAPI.Player.SpawnPlayer(Player, CurrentSpawn.Position, CurrentSpawn.Heading);

            GiveWeapon(CurrentWeapon);
        }

        public void NewLevel()
        {
            Player.RemoveWeapon(CurrentWeapon);
            Player.Health = 100;
            GiveWeapon(_weapons.Dequeue());
        }

        public bool IsOnLastLevel() => CurrentWeapon == StrikeConfig.AvailableForSelectWeaponsForLoby.Last();
    }
}