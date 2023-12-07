using GTANetworkAPI;
using System.Collections.Generic;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.MiniGames.MakeWeapon
{
    class MakeWeaponEvents: Script
    {
       

        [ServerEvent(Event.ResourceStart)]
        public void OnStart()
        {
            // MakeWeaponService.Init();            
        }

        [RemoteEvent("mg:makeweapon:result")]
        public void HandleResult(ExtPlayer player, int id, int result, int bonus)
        {
            if (!player.IsLogged()) return;
            MakeWeaponService.HandleGameResult(player, id, result, bonus);            
        }

        [RemoteEvent("mg:makeweapon:quit")]
        public void QuitJob(ExtPlayer player)
        {
            MakeWeaponService.QuitJob(player);
        }

        [Command("mwlimit")]
        public void SetLimit(ExtPlayer player, int limit)
        {
            if (!player.IsLogged()) return;
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            MakeWeaponService.SetLimit(player, limit);
        }

    }
}
