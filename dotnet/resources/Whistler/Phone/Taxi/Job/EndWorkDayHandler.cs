using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.GarbageCollector;
using Whistler.Helpers;
using Whistler.Phone.Taxi.Service;
using Whistler.SDK;
using Whistler.VehicleSystem.Models;
using Whistler.VehicleSystem;

namespace Whistler.Phone.Taxi.Job
{
    internal class EndWorkDayHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(EndWorkDayHandler));

        [RemoteEvent("phone::taxijob::endWorkDay")]
        public void HandlerEndWorkDay(ExtPlayer player)
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

        public static void HandleEndWork(ExtPlayer player)
        {
            player.RemoveTempVehicle(VehicleAccess.Rent)?.CustomDelete();
            TaxiService.SetDriverUnactive(player);
            Service.Models.TaxiOrderBase order = TaxiService.GetDriverOrder(player);
            if (order == null) return;

            order.DriverCancelOrder();
            TaxiService.CancelOrder(order);
        }

        public static void HandleEndWorkDelayed(ExtPlayer player)
        {
            DriverData taxiDriverData = TaxiService.GetDriverData(player);
            if (taxiDriverData == null) return;

            ExtVehicle tempVehicle = taxiDriverData.Vehicle;
            if (tempVehicle == null || !tempVehicle.Exists) return;

            Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Если Вы не сядете в транспорт в течении 60 секунд, то рабочий день закончится", 3000);
            GarbageManager.Add(tempVehicle, 1, player);
        }
    }
}
