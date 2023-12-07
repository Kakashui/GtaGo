using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Common;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Phone.Taxi.Dtos;
using Whistler.Phone.Taxi.Service;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.Phone.Taxi.Job
{
    internal class StartWorkDayHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(StartWorkDayHandler));

        [RemoteEvent("phone::taxijob::startWorkDay")]
        public void HandleAcceptOrder(ExtPlayer player)
        {
            try
            {
                if (player.Character.LVL < 2)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Для этой работы нужно быть как минимум 2м уровнем.", 3000);
                    return;
                }

                if (TaxiService.GetDriverData(player) != null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы уже начали рабочий день в такси.", 3000);
                    return;
                }

                GUI.Documents.Models.License license = player.Character.Licenses.FirstOrDefault(item => item.Name == GUI.Documents.Enums.LicenseName.Taxi);
                if (license == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вам нужно приобрести лицензию таксиста, чтобы работать в такси", 5000);
                    return;
                }

                if (!IsVehicleAvailableForTaxi(player))
                {
                    Notify.SendError(player, "taxi:job:1");
                    return;
                }

                ExtVehicle vehicle = player.Vehicle as ExtVehicle;
                DriverData driverData = new DriverData
                {
                    CarModel = vehicle.Config.DisplayName,
                    CarNumber = vehicle.Data.Number,
                    Vehicle = vehicle
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

        private static void LoadOrders(ExtPlayer player)
        {
            var mapper = MapperManager.Get();
            var orders = TaxiService.GetAvailableOrders()
                .Select(o => mapper.Map<TaxiOrderDto>(o));
            var ordersJson = JsonConvert.SerializeObject(orders);
            SafeTrigger.ClientEvent(player,"phone:taxijob:sendOrders", ordersJson);
        }

        private bool IsVehicleAvailableForTaxi(ExtPlayer player)
        {
            if (!player.IsInVehicle)
                return false;

            if (player.VehicleSeat != VehicleConstants.DriverSeat)
                return false;

            ExtVehicle veh = player.Vehicle as ExtVehicle;

            switch (veh.Data.OwnerType)
            {
                case OwnerType.Temporary:
                    var vehData = veh.Data as TemporaryVehicle;
                    if (vehData.Driver == player && vehData.Access == VehicleAccess.Rent && VehicleRent.Configs.RentVehicleConfig.CheckVehicleModelIsTaxi(player.Vehicle.Model))
                        return true;
                    break;
                case OwnerType.Personal:
                    if (veh.Data.ID == 23660 && player.Character.UUID == 1007478)
                        return true;
                    break;
                default:
                    break;
            }

            return false;
        }
    }
}
