using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Phone.Taxi.Service;

namespace Whistler.Phone.Taxi.ClientApp
{
    internal class PlayerReadyHandler : Script
    {
        public PlayerReadyHandler()
        {
            Main.OnPlayerReady += HandlePlayerReady;
        }

        private void HandlePlayerReady(ExtPlayer player)
        {
            player.TriggerCefEvent("smartphone/taxiPage/taxi_setPricePerKm", TaxiService.PricePerKm);
        }
    }
}
