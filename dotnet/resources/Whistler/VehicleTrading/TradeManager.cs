using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using AutoMapper.Internal;
using GTANetworkAPI;
using Whistler.Common;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;
using Whistler.VehicleSystem.Models.VehiclesData;
using Whistler.VehicleTrading.Models;

namespace Whistler.VehicleTrading
{
    class TradeManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(TradeManager));

        private static Dictionary<int, VehicleTrade> _tradePoints = new Dictionary<int, VehicleTrade>();
        private static List<Vector3> _blipPoints = new List<Vector3>
        {
            new Vector3(-1634.272, -878.5118, 9.13976),
            new Vector3(-2341.6, 290.8, 175.5),
            new Vector3(138.1, 6602.3, 31.42),
            new Vector3(1485.4, 3755.8, 45.2),
        };

        public static void LoadTradePoints()
        {
            try
            {
                SellVehicleManager.Init();
                foreach (var point in _blipPoints)
                {
                    NAPI.Blip.CreateBlip(669, point, 1, 24, Main.StringToU16("Обмен автомобилей"), 255, 0, true, 0, 0);
                }
                DataTable result = MySQL.QueryRead("SELECT * FROM `vehicletrading`");
                if (result == null || result.Rows.Count == 0)
                {
                    return;
                }
                foreach (DataRow row in result.Rows)
                {
                    var trade = new VehicleTrade(row);
                    _tradePoints.Add(trade.Id, trade);
                    if (trade.CurrVehicle > 0)
                        SpawnTradeCar(trade.CurrVehicle);
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($" OnResourceStart: {e.ToString()}");
            }
        }

        public static void SpawnTradeCar(PersonalBaseVehicle vehicle)
        {
            if (!_tradePoints.ContainsKey(vehicle.TradePoint))
                return;
            var tradePoint = _tradePoints[vehicle.TradePoint];
            tradePoint.SpawnVehicle(vehicle);
        }

        public static void SpawnTradeCar(int vehId)
        {
            var vehData = VehicleManager.Vehicles.GetOrDefault(vehId);
            if (vehData != null && vehData is PersonalBaseVehicle)
                SpawnTradeCar(vehData as PersonalBaseVehicle);
        }

        public static void CancelSellVehicle(int point)
        {
            if (!_tradePoints.ContainsKey(point))
                return;
            var tradePoint = _tradePoints[point];
            tradePoint.UnSetVehicle();

        }
        public static void OpenBuyMenu(ExtPlayer player, int point)
        {
            if (!_tradePoints.ContainsKey(point))
                return;
            var tradePoint = _tradePoints[point];
            tradePoint.OpenBuyMenu(player);
        }

        [RemoteEvent("vehTrade:setVehicleOnPoint")]
        public static void SetVehicleOnTradePoint(ExtPlayer player, int point, int price)
        {
            if (!player.IsLogged())
                return;
            if (!_tradePoints.ContainsKey(point))
                return;
            if (!player.IsInVehicle)
            {
                Notify.SendError(player, "trade:mgr:1");
                return;
            }
            ExtVehicle extVehicle = player.Vehicle as ExtVehicle;
            if (extVehicle.Data.OwnerType != OwnerType.Personal || !extVehicle.Data.CanAccessVehicle(player, VehicleSystem.AccessType.SellDollars))
            {
                Notify.SendError(player, "trade:mgr:2");
                return;
            }
            if (!VehicleOperations.CheckCorrectVehiclePrice(player, extVehicle.Data.ModelName, price))
                return;
            var tradePoint = _tradePoints[point];
            if (tradePoint.SetVehicle(extVehicle.Data as PersonalBaseVehicle, price))
            {
                Notify.SendSuccess(player, "trade:mgr:3");
            }
            else
            {
                Notify.SendError(player, "trade:mgr:4");
            }
        }

        [RemoteEvent("vehicleTrade:buyVehicle")]
        public static void BuyVehicle(ExtPlayer player, int point)
        {
            if (!_tradePoints.ContainsKey(point))
                return;
            var tradePoint = _tradePoints[point];
            tradePoint?.BuyVehicle(player);
        }

        [Command("newtradepoint")]
        public static void CreateTradePoint(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            if (!Group.CanUseAdminCommand(player, "newtradepoint"))
                return;
            if (!player.IsInVehicle)
            {
                Notify.SendError(player, "trade:mgr:1");
                return;
            }
            var trade = new VehicleTrade(player.Vehicle.Position, player.Vehicle.Rotation);
            _tradePoints.Add(trade.Id, trade);
        }

        [Command("deltradepoint")]
        public static void DeleteTradePoint(ExtPlayer player, int id)
        {
            if (!player.IsLogged())
                return;
            if (!Group.CanUseAdminCommand(player, "deltradepoint"))
                return;
            if (!_tradePoints.ContainsKey(id))
            {
                return;
            }

            var trade = _tradePoints[id];
            if (trade.CurrVehicle > 0)
            {
                return;
            }
            trade.Destroy();
            _tradePoints.Remove(trade.Id);
        }
    }
}
