using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Phone.Taxi.Service;
using Whistler.SDK;
using Whistler.Helpers;
using Whistler.Phone.Taxi.Dtos;
using Newtonsoft.Json;
using Whistler.GUI.Tips;
using Whistler.Phone.Taxi.Service.Models;
using Whistler.NewDonateShop;
using Whistler.Entities;
using Whistler.VehicleSystem.Models;

namespace Whistler.Phone.Taxi.Job
{
    internal class AcceptOrderHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(AcceptOrderHandler));

        [RemoteEvent("phone::taxijob::acceptOrder")]
        public void HandleAcceptOrder(ExtPlayer driver, int orderID)
        {
            try
            {
                var order = TaxiService.SetDriverToOrder(driver, orderID);
                if (order == null)
                    return;
                var driverData = TaxiService.GetDriverData(driver);

                if (driverData == null)
                    return;

                order.SendDriverData(driver, driverData);
                SafeTrigger.ClientEvent(driver, "phone:taxijob:setCurrentOrder", "accepted", order.Start, order.Sum, order.IsCardPayment);

                if (driver.Position.DistanceTo2D(order.Start) <= 60)
                {
                    Tip.SendTipNotification(driver, "jobs:taxi:pass:wait");
                    order.ArrivalToClient(driverData);
                }
                else
                {
                    order.Colshape?.Delete();
                    var colshape = NAPI.ColShape.CreatCircleColShape(order.Start.X, order.Start.Y, 50);
                    colshape.OnEntityEnterColShape += (shape, player) => HandleEntityEnterOrderColshape((ExtPlayer)player, order);

                    order.Colshape = colshape;
                }

            }
            catch (Exception e)
            {
                _logger.WriteError($"Unhandled exception catched on phone::taxijob::acceptOrder ({driver?.Name}, {orderID}) - " + e.ToString());
            }
        }

        private static void HandleEntityEnterOrderColshape(ExtPlayer driver, TaxiOrderBase order)
        {
            try
            {
                if (driver != order.Driver|| !driver.IsInVehicle)
                    return;

                var driverData = TaxiService.GetDriverData(driver);

                ExtVehicle driverVehicle = driver.Vehicle as ExtVehicle;
                if (driverData == null || driverVehicle.Data.Number != driverData.CarNumber)
                    return;

                Tip.SendTipNotification(driver, "jobs:taxi:pass:wait");
                order.ArrivalToClient(driverData);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Unhandled exception catched on HandleEntityEnterOrderColshape ({driver?.Name}, {order?.ID}) - " + e.ToString());
            }
        }

        [ServerEvent(Event.PlayerEnterVehicle)]
        public void HandlePlayerEnterDriverVehicle(ExtPlayer client, ExtVehicle vehicle, sbyte seatId)
        {
            try
            {
                var order = TaxiService.GetClientOrder(client);
                if (order == null)
                    return;

                var driver = order.Driver;
                if (!driver.IsInVehicle || driver.Vehicle != vehicle)
                    return;

                ExtVehicle driverVehicle = driver.Vehicle as ExtVehicle;
                var driverData = TaxiService.GetDriverData(driver);

                if (driverData == null || driverVehicle.Data.Number != driverData.CarNumber)
                    return;

                SafeTrigger.ClientEvent(driver, "phone:taxijob:setCurrentOrder", "inProgress", order.Destination, order.Sum, order.IsCardPayment);

                order.Colshape?.Delete();

                order.Colshape = NAPI.ColShape.CreatCircleColShape(order.Destination.X, order.Destination.Y, 80);
                order.Colshape.OnEntityEnterColShape += (shape, sPlayer) => HandleDriverEnterFinishPoint(sPlayer as ExtPlayer, order);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Unhandled exception catched on Event.PlayerEnterVehicle ({client?.Name}, {seatId}) - " + e.ToString());
            }
        }
        [RemoteEvent("phone::taxijob::pedEnterVehicle")]
        public void HandlePedEnterDriverVehicle(ExtPlayer client)
        {
            try
            {
                var order = TaxiService.GetDriverOrder(client);
                if (order == null)
                    return;

                var driver = order.Driver;
                if (!driver.IsInVehicle)
                    return;

                var driverData = TaxiService.GetDriverData(driver);

                ExtVehicle driverVehicle = driver.Vehicle as ExtVehicle;
                if (driverData == null || driverVehicle.Data.Number != driverData.CarNumber)
                    return;

                SafeTrigger.ClientEvent(driver, "phone:taxijob:setCurrentOrder", "inProgress", order.Destination, order.Sum, order.IsCardPayment);

                order.Colshape?.Delete();

                order.Colshape = NAPI.ColShape.CreatCircleColShape(order.Destination.X, order.Destination.Y, 80);
                order.Colshape.OnEntityEnterColShape += (shape, sPlayer) => HandleDriverEnterFinishPoint(sPlayer as ExtPlayer, order);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Unhandled exception catched on phone::taxijob::pedEnterVehicle ({client?.Name}) - " + e.ToString());
            }
        }

        private static void HandleDriverEnterFinishPoint(ExtPlayer driver, TaxiOrderBase order)
        {
            try
            {
                if (driver != order.Driver)
                    return;
                var driverData = TaxiService.GetDriverData(driver);

                ExtVehicle driverVehicle = driver.Vehicle as ExtVehicle;
                if (driverData == null || driverVehicle.Data.Number != driverData.CarNumber)
                    return;
                if (!order.CompleateOrder())
                    return;

                var paymentType = order.IsCardPayment ? MoneySystem.PaymentsType.Card : MoneySystem.PaymentsType.Cash;
                order.Sum = DonateService.UseJobCoef(driver, order.Sum, driver.Character.IsPrimeActive());
                MoneySystem.Wallet.MoneyAdd(driver.GetMoneyPayment(paymentType), order.Sum, "Money_TaxiOrder");

                driver.CreatePlayerAction(PersonalEvents.PlayerActions.CompleteTaxiCarry, 1);

                Tip.SendTipNotification(driver, "jobs:taxi:compl".Translate(order.Sum));

                driver.TriggerCefAction("smartphone/taxiPage/taxijob_sendToOrders", true);
                driver.TriggerCefEvent("smartphone/taxiPage/taxijob_addOrderToStats", order.Sum);

                TaxiService.DeleteOrder(order.ID, true);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Unhandled exception catched on HandleDriverEnterFinishPoint ({driver?.Name}, {order?.ID}) - " + e.ToString());
            }
        }
    }
}
