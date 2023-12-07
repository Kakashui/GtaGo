using GTANetworkAPI;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.AlcoholBar
{
    class BarEvents: Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            BarService.Init();
        }

        [RemoteEvent("alco:bar:buy")]
        public void AlcoBarOpen(PlayerGo player, int id)
        {
            if (!player.IsLogged()) return;
            BarService.BuyAlco(player, id);
        }

        [Command("addbar")]
        public void AddBar(PlayerGo player, int radius)
        {
            if (!Group.CanUseAdminCommand(player, "addbar")) return;
            BarService.AddNewBarpoint(player, radius);            
        }

        [Command("removebar")]
        public void RemoveBar(PlayerGo player)
        {
            if (!Group.CanUseAdminCommand(player, "barid")) return;
            BarService.RemoveBarPoint(player);
        }
    }
}
