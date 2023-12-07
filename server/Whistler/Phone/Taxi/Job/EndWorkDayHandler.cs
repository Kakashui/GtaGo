using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.Phone.Taxi.Service;
using Whistler.SDK;

namespace Whistler.Phone.Taxi.Job
{
    internal class EndWorkDayHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(EndWorkDayHandler));

        [RemoteEvent("phone::taxijob::endWorkDay")]
        public void HandlerEndWorkDay(Player player)
        {
            try
            {
                TaxiService.SetDriverUnactive(player);
                CancelHandler.HandleCancelOrder(player);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Unhandled exception catched phone::taxijob::endWorkDay ({player?.Name}) - " + e.ToString());
            }
        }
    }
}
