using System;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.Fractions;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.MP.Arena.Enums;
using Whistler.MP.Arena.Interfaces;
using Whistler.MP.Arena.Lobbies;
using Whistler.MP.Arena.Locations;
using Whistler.SDK;

namespace Whistler.MP.Arena.Battles
{
    internal abstract class StrikeBattleMember : IBattleMember
    {
        public ExtPlayer Player { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        
        public TeamName Team { get; set; }
        
        public IArenaBattle CurrentBattle { get; set; }
        
        public MatchSignificantPlace Place { get; set; }
        
        public ArenaLocationSpawn CurrentSpawn { get; set; }

        public abstract void Spawn();

        public event Action Leaved;

        public void ForceInvokeOnLeaved()
        {
            Leaved?.Invoke();
        }
        
        public void Delete()
        {
            if (Player.HasData("dm:default:spawn"))
            {
                NAPI.Task.Run(() =>
                {
                    Vector3 oldPosition = Player.GetData<Vector3>("dm:default:spawn");
                    Ems.RevivePlayer(Player, oldPosition, 100);
                }, 1000);
            }
            SafeTrigger.UpdateDimension(Player,  0);
            Player.ResetData("kl:enabled");

            NAPI.Player.RemoveAllPlayerWeapons(Player);
            CaptureUI.DisableKillog(Player, true);
            
            CurrentBattle.Location.Unload(this);
            CurrentBattle.Members.Remove(this);
            SafeTrigger.ClientEvent(Player, "arena:battle:stop");
            
            Leaved?.Invoke();
        }

        protected void GiveWeapon(WeaponHash weaponHash)
        {
            NAPI.Player.RemoveAllPlayerWeapons(Player);
            NAPI.Player.GivePlayerWeapon(Player, weaponHash, 2000);
        }
    }
}