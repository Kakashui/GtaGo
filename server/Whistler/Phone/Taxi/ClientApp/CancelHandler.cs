using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.GUI.Tips;
using Whistler.Helpers;
using Whistler.Phone.Taxi.Service;
using Whistler.SDK;

namespace Whistler.Phone.Taxi.ClientApp
{
    internal class CancelHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(CancelHandler));
        
        public CancelHandler()
        {
            Main.PlayerPreDisconnect += HandleCancelOrder;
        }

        [RemoteEvent("phone::taxi::cancel")]
        public void HandleCancelOrder(Player player)
        {
            try
            {
                var order = TaxiService.GetClientOrder(player);
                if (order == null)
                    return;

                if (order.DriverUuid != null)
                {
                    var playerPosition = player.IsInVehicle ? player.Vehicle.Position : player.Position;
                    if (playerPosition.DistanceTo2D(order.Destination) < 200)
                    {
                        Notify.SendError(player, "taxi:order:canc:err:1");
                        return;
                    }
                }

                var paymentType = order.IsCardPayment ? MoneySystem.PaymentsType.Card : MoneySystem.PaymentsType.Cash;
                MoneySystem.Wallet.MoneyAdd(player.GetMoneyPayment(paymentType), order.Sum, "Money_TaxiCancel");

                if (order.DriverUuid != null)
                {
                    var driver = order.Driver;
                    Tip.SendTipNotification(driver, "taxi:order:canc");
                }

                TaxiService.CancelOrder(order);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Unhandled exception catched on phone::taxijob::acceptOrder ({player?.Name}) - " + e.ToString());
            }
        }
    }
}
