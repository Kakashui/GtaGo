using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Common;
using Whistler.Helpers;
using Whistler.Phone.Taxi.Dtos;
using Whistler.Phone.Taxi.Service;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.Phone.Taxi.Job
{
    internal class StartWorkDayHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(StartWorkDayHandler));

        [RemoteEvent("phone::taxijob::startWorkDay")]
        public void HandleAcceptOrder(Player player)
        {
            try
            {
                if (!IsVehicleAvailableForTaxi(player))
                {
                    Notify.SendError(player, "taxi:job:1");
                    return;
                }

                var vehicle = player.Vehicle.GetVehicleGo();
                var driverData = new DriverData
                {
                    CarModel = vehicle.Config.DisplayName,
                    CarNumber = vehicle.Data.Number
                };

                TaxiService.SetDriverActive(player, driverData);
                player.TriggerCefAction("smartphone/taxiPage/taxijob_sendToOrders", true);

                LoadOrders(player);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Unhandled exception catched on phone::taxijob::startWorkDay ({player?.Name}) - " + e.ToString());
            }
        }

        private static void LoadOrders(Player player)
        {
            var mapper = MapperManager.Get();
            var orders = TaxiService.GetAvailableOrders()
                .Select(o => mapper.Map<TaxiOrderDto>(o));
            var ordersJson = JsonConvert.SerializeObject(orders);
            player.TriggerEvent("phone:taxijob:sendOrders", ordersJson);
        }

        private bool IsVehicleAvailableForTaxi(Player player)
        {
            if (!player.IsInVehicle)
                return false;

            if (player.VehicleSeat != VehicleConstants.DriverSeat)
                return false;

            var veh = player.Vehicle.GetVehicleGo();

            switch (veh.Data.OwnerType)
            {
                case OwnerType.Temporary:
                    var vehData = veh.Data as TemporaryVehicle;
                    if (vehData.Driver == player && vehData.Access == VehicleAccess.Rent && VehicleRent.Configs.RentVehicleConfig.CheckVehicleModelIsTaxi(player.Vehicle.Model))
                        return true;
                    break;
                case OwnerType.Personal:
                    if (veh.Data.ID == 23660 && player.GetCharacter().UUID == 1007478)
                        return true;
                    break;
                default:
                    break;
            }

            return false;
        }
    }
}
