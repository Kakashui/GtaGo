using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Core;
using Whistler.Entities;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.Phone.Taxi.Service;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.GarbageCollector
{
    class GarbageManager : Script
    {
        static WhistlerLogger _logger = new WhistlerLogger(typeof(GarbageManager));
        const int _checkTime = 60000;

        [ServerEvent(Event.ResourceStart)]
        public void StartGarbageCollector()
        {
            Timers.Start(_checkTime, CheckVehicleList);
        }

        private void CheckVehicleList()
        {
            DateTime now = DateTime.Now;
            ExtPlayer responsible;
            foreach (ExtVehicle vehicle in SafeTrigger.GetAllVehicles())
            {
                if (vehicle.Session.RespawnTime == null) continue;
                if (vehicle.Session.RespawnTime > now) continue;

                responsible = vehicle.Session.RespawnResponsible;
                if (responsible != null) ResponsibleAction(responsible, vehicle);
                vehicle.Session.RespawnTime = null;
                vehicle.Session.RespawnResponsible = null;
                RespawnVehicle(vehicle);
            }
        }

        private static void ResponsibleAction(ExtPlayer player, ExtVehicle vehicle)
        {
            if (!(vehicle.Data is TemporaryVehicle tempVehicle)) return;

            switch (tempVehicle.Access)
            {
                case VehicleAccess.WorkBus:
                    player.Character.WorkID = 0;
                    if (player.Session.RentBus != null)
                    {
                        player.Session.RentBus.CustomDelete();
                        player.Session.RentBus = null;
                    }
                    SafeTrigger.ClientEvent(player, "bus:clear");
                    MainMenu.SendStats(player);
                    return;
                case VehicleAccess.Rent:
                    TaxiService.SetDriverUnactive(player);
                    Phone.Taxi.Service.Models.TaxiOrderBase order = TaxiService.GetDriverOrder(player);
                    if (order == null) return;

                    order.DriverCancelOrder();
                    TaxiService.CancelOrder(order);
                    return;
                default:
                    return;
            }
        }

        private static void RespawnVehicle(ExtVehicle vehicle)
        {
            vehicle.Data.RespawnVehicle();
        }

        internal static void Add(ExtVehicle vehicle, int min, ExtPlayer responsible = null)
        {
            vehicle.Session.RespawnTime = DateTime.Now.AddMinutes(min);
            vehicle.Session.RespawnResponsible = responsible;
        }
        internal static void Remove(ExtVehicle vehicle)
        {
            vehicle.Session.RespawnTime = null;
            vehicle.Session.RespawnResponsible = null;
        }
        internal static bool InGarbage(ExtVehicle vehicle)
        {
            return vehicle.Session.RespawnTime != null;
        }
    }
}
