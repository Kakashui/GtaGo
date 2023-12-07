using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.Phone.Taxi.Dtos;
using Whistler.SDK;
using System.Threading.Tasks;
using Whistler.Domain.Phone.Taxi;
using Whistler.Infrastructure.DataAccess;
using Whistler.GUI.Tips;
using Whistler.Phone.Taxi.Service.Models;
using Whistler.Entities;
using Whistler.VehicleSystem.Models;

namespace Whistler.Phone.Taxi.Service
{
    internal class DriverData
    {
        public string CarNumber { get; set; }

        public string CarModel { get; set; }
        public ExtVehicle Vehicle { get; set; }
    }

    internal class TaxiService : Script
    {
        private static int LastOrderID = 0;
        public const int PricePerKm = 150;

        private static readonly List<TaxiOrderBase> ActiveOrders = new List<TaxiOrderBase>();

        private static readonly Dictionary<ExtPlayer, DriverData> ActiveDrivers = new Dictionary<ExtPlayer, DriverData>();

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(TaxiService));
        public TaxiService()
        {

        }

        public static void SetDriverActive(ExtPlayer player, DriverData driverData)
        {
            if (ActiveDrivers.ContainsKey(player)) return;

            ActiveDrivers.Add(player, driverData);
        }

        public static DriverData GetDriverData(ExtPlayer player)
            => ActiveDrivers.GetValueOrDefault(player, null);

        public static TaxiOrderBase GetClientOrder(ExtPlayer player)
        {
            var driverUuid = player.Character.UUID;
            return ActiveOrders.FirstOrDefault(order => order is TaxiOrderPlayer && (order as TaxiOrderPlayer).PassengerUuid == driverUuid);
        }

        public static TaxiOrderBase GetDriverOrder(ExtPlayer player)
        {
            var driverUuid = player.Character.UUID;
            return ActiveOrders.FirstOrDefault(o => o.DriverUuid == driverUuid);
        }

        public static IEnumerable<TaxiOrderBase> GetAvailableOrders()
        {
            return ActiveOrders
                .Where(o => o.DriverUuid == null);
        }

        public static void SetDriverUnactive(ExtPlayer player)
        {
            if (!ActiveDrivers.ContainsKey(player)) return;

            ActiveDrivers.Remove(player);
        }

        public static void PullOrderToQueue(TaxiOrderBase order)
        {
            order.ID = ++LastOrderID;
            ActiveOrders.Add(order);

            var mapper = MapperManager.Get();
            var dto = mapper.Map<TaxiOrderDto>(order);
            var dtoJson = JsonConvert.SerializeObject(dto);

            foreach (var driver in ActiveDrivers)
            {
                driver.Key.TriggerEventSafe("phone:taxijob:sendOrder", dtoJson);
                Tip.SendTipNotification(driver.Key, "taxi:service:4");
            }
        }

        public static TaxiOrderBase SetDriverToOrder(ExtPlayer player, int orderID)
        {
            var order = ActiveOrders
                .Find(o => o.ID == orderID);

            if (order == null)
                return null;

            if (order.DriverUuid != null)
                return null;

            order.DriverUuid = player.Character.UUID;

            foreach (var driver in ActiveDrivers)
            {
                driver.Key.TriggerCefEvent("smartphone/taxiPage/taxijob_removeOrder", orderID);
            }

            return order;
        }

        public static void CancelOrder(TaxiOrderBase order)
        {
            if (order.DriverUuid != null)
            {
                var driver = order.Driver;
                driver.TriggerCefAction("smartphone/taxiPage/taxijob_sendToOrders", true);
                driver.TriggerCefEvent("smartphone/taxiPage/taxijob_setStateWork", JsonConvert.SerializeObject("working"));

            }
            DeleteOrder(order.ID, false);
        }

        public static void DeleteOrder(int orderID, bool isSuccess)
        {
            var order = ActiveOrders
                .Find(o => o.ID == orderID);
            if (order == null)
                return;

            if (isSuccess)
            {
                SaveOrderHistoryItem(new OrderHistoryItem
                    {
                        Date = DateTime.Now,
                        DriverUuid = (int)order.DriverUuid,
                        TotalPrice = order.Sum
                    });
            }

            order.Destroy();

            if (order.DriverUuid == null)
            {
                foreach (var driver in ActiveDrivers.Keys)
                    driver?.TriggerCefEvent("smartphone/taxiPage/taxijob_removeOrder", order.ID);
            }

            ActiveOrders.Remove(order);
        }

        public static int CalculatePrice(Vector3 start, Vector3 destination)
        {
            return Convert.ToInt32(start.DistanceTo2D(destination) / 1000 * PricePerKm);
        }

        public static async Task SaveOrderHistoryItem(OrderHistoryItem orderHistoryItem)
        {
            try
            {
                using (var context = DbManager.TemporaryContext)
                {
                    context.TaxiOrdersHistory.Add(orderHistoryItem);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() => _logger.WriteError($"SaveOrderHistoryItem: {e.ToString()}"));
            }
        }
    }
}
