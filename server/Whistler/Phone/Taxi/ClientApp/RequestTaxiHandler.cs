using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.Phone.Taxi.Service;
using Whistler.Phone.Taxi.Service.Models;
using Whistler.SDK;

namespace Whistler.Phone.Taxi.ClientApp
{
    internal class RequestTaxiHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(RequestTaxiHandler));

        [RemoteEvent("phone:taxi:requestTaxi")]
        public static void HandleRequestTaxi(Player player, bool isCardPayment, float destinationX, float destinationY)
        {
            try
            {
                if (TaxiService.GetClientOrder(player) != null)
                {
                    Notify.SendError(player, "racing:7");
                    return;
                }

                if (IsInInteriors(player))
                {
                    Notify.SendError(player, "racing:8");
                    return;
                }

                if (IsInPrison(player))
                {
                    Notify.SendError(player, "racing:9");
                    return;
                }

                var character = player.GetCharacter();
                var destination = new Vector3(destinationX, destinationY, 0);
                var startPosition = player.IsInVehicle ? player.Vehicle.Position : player.Position;
                var sum = TaxiService.CalculatePrice(startPosition, destination);

                var paymentType = isCardPayment ? MoneySystem.PaymentsType.Card : MoneySystem.PaymentsType.Cash;

                if (!MoneySystem.Wallet.MoneySub(player.GetMoneyPayment(paymentType), sum, "Money_TaxiOrder"))
                {
                    Notify.SendError(player, "Core_355");
                    return;
                }

                var order = new TaxiOrderPlayer
                {
                    CreateDate = DateTime.Now,
                    PassengerUuid = character.UUID,
                    Start = startPosition,
                    Destination = destination,
                    IsCardPayment = isCardPayment,
                    Sum = sum
                };

                TaxiService.PullOrderToQueue(order);
                player.TriggerCefAction("smartphone/taxiPage/taxi_setSearch", true);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Unhandled exception catched on phone:taxi:requestTaxi ({player?.Name}, {isCardPayment}, {destinationX}, {destinationY}) - " + e.ToString());
            }
        }

        private static bool IsInPrison(Player player)
        {
            var character = player.GetCharacter();
            if (character.DemorganTime != 0 || 
                character.ArrestDate > DateTime.UtcNow || 
                character.ArrestiligalTime != 0 || 
                character.CourtTime != 0)
                return true;

            return false;
        }

        private static bool IsInInteriors(Player player)
        {
            var character = player.GetCharacter();
            if (character.InsideHouseID != -1 || 
                character.InsideGarageID != -1)
                return true;

            return false;
        }
    }
}
