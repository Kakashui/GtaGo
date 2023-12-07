using GTANetworkAPI;
using System;
using Whistler.Entities;
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

        private void HandleDisconnect(ExtPlayer player)
        {
            TaxiService.SetDriverUnactive(player);

            Service.Models.TaxiOrderBase order = TaxiService.GetDriverOrder(player);
            if (order == null) return;

            order.DriverCancelOrder();
            TaxiService.CancelOrder(order);
        }

        [RemoteEvent("phone::taxijob::cancel")]
        public static void HandleCancelOrder(ExtPlayer player)
        {
            try
            {
                var order = TaxiService.GetDriverOrder(player);
                if (order == null) return;

                DateTime endDate = order.CreateDate.AddMinutes(2);
                DateTime now = DateTime.Now;
                if (endDate > now)
                {
                    TimeSpan diff = endDate - now;
                    Notify.SendError(player, $"Вы сможете отменить заказ через {Math.Round(diff.TotalSeconds, MidpointRounding.AwayFromZero)} секунд.");
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
