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
        public void AlcoBarOpen(ExtPlayer player, string json, bool cashPay)
        {
            if (!player.IsLogged()) return;
            BarService.BuyAlco(player, json, cashPay);
        }

        [Command("addbar")]
        public void AddBar(ExtPlayer player, int radius)
        {
            if (!Group.CanUseAdminCommand(player, "addbar")) return;
            BarService.AddNewBarpoint(player, radius);            
        }

        [Command("removebar")]
        public void RemoveBar(ExtPlayer player)
        {
            if (!Group.CanUseAdminCommand(player, "barid")) return;
            BarService.RemoveBarPoint(player);
        }
    }
}
