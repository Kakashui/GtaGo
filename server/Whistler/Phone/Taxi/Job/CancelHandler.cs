using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.Phone.Taxi.Service;
using Whistler.SDK;

namespace Whistler.Phone.Taxi.Job
{
    internal class CancelHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(CancelHandler));

        public CancelHandler()
        {
            Main.PlayerPreDisconnect += HandleDisconnect;
        }

        private void HandleDisconnect(Player player)
        {
            TaxiService.SetDriverUnactive(player);
            HandleCancelOrder(player);
        }

        [RemoteEvent("phone::taxijob::cancel")]
        public static void HandleCancelOrder(Player player)
        {
            try
            {
                var order = TaxiService.GetDriverOrder(player);
                if (order == null)
                    return;
                if (order.CreateDate.AddMinutes(5) > DateTime.Now)
                {
                    Notify.SendError(player, "taxi:order:canc:err:2");
                    return;
                }
                order.DriverCancelOrder();
                TaxiService.CancelOrder(order);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Unhandled exception catched on phone::taxijob::cancel ({player?.Name}) - " + e.ToString());
            }
        }
    }
}
