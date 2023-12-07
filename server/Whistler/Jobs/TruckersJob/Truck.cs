using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Docks;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Jobs.ImpovableJobs;
using Whistler.MoneySystem;
using Whistler.NewDonateShop;
using Whistler.SDK;
using Whistler.VehicleSystem;

namespace Whistler.Jobs.TruckersJob
{
    /// <summary>
    /// Грузовик для аренды дальнобойщиками
    /// </summary>
    internal class Truck
    {
        public uint TruckHash { get; }
        private static Dictionary<Player, Vehicle> _templates = new Dictionary<Player, Vehicle>();
        public static Dictionary<PlayerGo, DateTime> PlayersUnloadTime = new Dictionary<PlayerGo, DateTime>();
        //private static List<Vector3> _trailersLoadPool = new List<Vector3>();// Список точек загрузки где стоит и ждет трейлер  
        private TruckRoute _currentRoute;
        private float _vehicleHealthSnapshot;
        private int _selectedTruckerLevel;
        public bool IsOnWork { get; private set; }
        
        public Vector3 Position { get; private set; }
        private string _rentTimerId;
        public PlayerGo Renter { get; private set; }
        private readonly Color _color;
        
        public Truck(TruckInfo info)
        {
            TruckHash = info.TruckHash;
            _color = info.Color;
        }
        
        public void Instantiate(PlayerGo player, Vector3 position, float heading, int parkId, int payment)
        {
            Position = position;
            Renter = player;
            var vehicle = VehicleManager.CreateTemporaryVehicle(TruckHash, position, new Vector3(0, 0, heading), "TRUCK", VehicleAccess.WorkTruck, player);
            VehicleCustomization.SetPowerTorque(vehicle, 10, 1);
            vehicle.CustomPrimaryColor = _color;
            VehicleStreaming.SetEngineState(vehicle, true);
            VehicleStreaming.SetLockStatus(vehicle, false);
            MoneySystem.Wallet.MoneySub(player.GetCharacter(), payment, "Money_RentPayment");
            _rentTimerId = Timers.Start(3600000, () => // Раз в час
            {
                MoneySystem.Wallet.MoneySub(player.GetCharacter(), payment, "Money_RentPayment");
            });
            vehicle.SetData("RENTPARK_ID", parkId);
            _templates.Add(player, vehicle);
        }

        /// <summary>
        /// Начать маршрут
        /// </summary>
        /// <param name="selectedLevel">Выбранный уровень</param>
        public void StartRoute(int selectedLevel)
        {
            var random = new Random();
            if (!IsOnWork) IsOnWork = true;
            _selectedTruckerLevel = selectedLevel;
            var order = Dock.CurrentOrders.FirstOrDefault(o => o?.Taked == false);
            if (order != null && BusinessManager.BizList[order.Customer.ID].UnloadPoint != null 
                              && BusinessManager.BizList[order.Customer.ID].UnloadPoint != new Vector3(0, 0, 0))
            {
                _currentRoute = new TruckRoute(TruckersJobSettings.DockLoadPoints.GetRandomElement().Item1, BusinessManager.BizList[order.Customer.ID].UnloadPoint, orderFromDock: order);
                Notify.Send(Renter, NotifyType.Info, NotifyPosition.Top, "Truckers_24".Translate(order.Customer.Name), 3000);
                order.Taked = true;
                SetLoadPoint();
                return;
            }

            //List<(Vector3, float?)> freeLoadPoints;
            //if (TruckersJobSettings.Stages[selectedLevel].TrailerHash.HasValue)
            //    freeLoadPoints = TruckersJobSettings.Stages[selectedLevel].LoadPoints
            //        .Where(p => _trailersLoadPool.All(l => l != p.Item1)).ToList();
            //freeLoadPoints = TruckersJobSettings.Stages[selectedLevel].LoadPoints;
            
            var load = TruckersJobSettings.Stages[selectedLevel].LoadPoints.GetRandomElement();
            
            var unloadPointsWhereBigDistance = TruckersJobSettings.Stages[selectedLevel].UnloadPoints
                .Where(u => load.Item1.DistanceTo2D(u) > 2900).ToList(); 
            
            var unload = unloadPointsWhereBigDistance.GetRandomElement();

            _currentRoute = new TruckRoute(load.Item1, unload);
           // LoadTrailerIfPossible(TruckersJobSettings.Stages[selectedLevel], load, unload);
            SetLoadPoint();
        }
        
        public void Destroy(Player player)
        {
            if (_currentRoute.Trailer != null)
            {
                _currentRoute.Trailer.CustomDelete();
                _currentRoute.Trailer = null;
            }

            //var loadPointFromPool = _trailersLoadPool.FirstOrDefault(p => p == _currentRoute.LoadPoint);
            //if (loadPointFromPool != null) _trailersLoadPool.Remove(loadPointFromPool);
            
            if (_currentRoute.OrderFromDock != null) _currentRoute.OrderFromDock.Taked = false;
            
            _currentRoute.CurrentRouteType = CurrentRouteType.None;
            _templates[player]?.CustomDelete();
            _templates.Remove(player);
            Timers.Stop(_rentTimerId);
        }

        private void SetLoadPoint()
        {
            _currentRoute.CurrentRouteType = CurrentRouteType.Load;
            Renter.TriggerEvent("truckers:setCheckPointRoutePath", 
                JsonConvert.SerializeObject(_currentRoute.LoadPoint));
            _vehicleHealthSnapshot = _templates[Renter].Health;
            //if (_currentRoute.Trailer != null)
            //    Notify.Send(Renter, NotifyType.Success, NotifyPosition.BottomCenter,
            //        "Truckers_9_1".Translate(Renter), 5000);
            Notify.Send(Renter, NotifyType.Success, NotifyPosition.BottomCenter,
            "Truckers_9".Translate(), 5000);
        }
        
        private void SetUnLoadPoint()
        {
            _currentRoute.CurrentRouteType = CurrentRouteType.Unload;
            Renter.TriggerEvent("truckers:setCheckPointRoutePath", 
                JsonConvert.SerializeObject(_currentRoute.UnloadPoint));
        }
        
        public void EnteredCurrentRoutePath()//todo: отрефакторить
        {
            switch (_currentRoute.CurrentRouteType)
            {
                case CurrentRouteType.None: return;
                case CurrentRouteType.Load:
                {
                    //if (_currentRoute.Trailer != null)
                    //{
                    //    var loadPointFromPool = _trailersLoadPool.FirstOrDefault(p => p == _currentRoute.LoadPoint);
                    //    if (loadPointFromPool != null) _trailersLoadPool.Remove(loadPointFromPool);
                    //}
                    SetUnLoadPoint();
                    break;
                }
                case CurrentRouteType.Unload:
                    if (_currentRoute.Trailer != null && Renter.Vehicle.Trailer != _currentRoute.Trailer)
                    {
                        Notify.Send(Renter, NotifyType.Error, NotifyPosition.BottomCenter,
                            "Truckers_12".Translate(), 3000);
                        return;
                    }

                    if (!PlayersUnloadTime.ContainsKey(Renter))
                    {
                        PlayersUnloadTime.Add(Renter, DateTime.Now);
                    }
                    else
                    {
                        if (DateTime.Now.Subtract(PlayersUnloadTime[Renter]).TotalMinutes < 5)
                        {
                            Notify.SendError(Renter, "Frac_63");
                            return;
                        }
                        PlayersUnloadTime[Renter] = DateTime.Now;
                    }
                    
                    if (_currentRoute.Trailer != null)
                    {
                        _currentRoute.Trailer.CustomDelete();
                    }
                    _currentRoute.OrderFromDock?.Customer.ComplyOrder(_currentRoute.OrderFromDock);
                    
                    var currentHealth = _templates[Renter].Health;
                    var healthLosePercents = 1 - currentHealth / _vehicleHealthSnapshot;

                    var levelBeforeIncrementation = TruckersJobSettings.GetLevel(Renter.GetCharacter().ImprovableJobs[ImprovableJobType.ProductsLoader].StagesPassed);
                    // Повышаем поездки только если на последнем уровне
                    if (levelBeforeIncrementation == _selectedTruckerLevel)
                        Renter.GetCharacter().ImprovableJobs[ImprovableJobType.ProductsLoader].StagesPassed++;
                    var newLevel = TruckersJobSettings.GetLevel(Renter.GetCharacter().ImprovableJobs[ImprovableJobType.ProductsLoader].StagesPassed);
                    if (levelBeforeIncrementation < newLevel)
                    {
                        if (TruckersJobSettings.Stages[newLevel].GoCoinsReward.HasValue)
                        {
                            Renter.AddGoCoins(TruckersJobSettings.Stages[newLevel].GoCoinsReward.Value);
                            Notify.Alert(Renter, "Truckers_25".Translate(TruckersJobSettings.Stages[newLevel].GoCoinsReward.Value));
                        }
                        else Notify.Alert(Renter,"Truckers_26");
                    }
                    var basePayment = DonateService.UseJobCoef(Renter, TruckersJobSettings.Stages[_selectedTruckerLevel].PaymentPerUnload);
                    var paymentWithLoseFactor = DonateService.UseJobKoef(Renter, basePayment - basePayment * healthLosePercents / 2);
                    
                    if (healthLosePercents > 5 && paymentWithLoseFactor < basePayment 
                        && (basePayment - paymentWithLoseFactor) > basePayment / 2)
                    {
                        Notify.Send(Renter, NotifyType.Success, NotifyPosition.BottomCenter, "Truckers_8".Translate(paymentWithLoseFactor), 3000);
                        MoneySystem.Wallet.MoneyAdd(Renter.Character, paymentWithLoseFactor, "Money_PaymentForDelivery");
                    }
                    
                    else 
                    {
                        Notify.Send(Renter, NotifyType.Success, NotifyPosition.BottomCenter, "Truckers_7".Translate(basePayment), 3000);
                        MoneySystem.Wallet.MoneyAdd(Renter.Character, basePayment, "Money_PaymentForDelivery");
                    }
                    Renter.CreatePlayerAction(PersonalEvents.PlayerActions.CompleteTruckCarry, 1);
                    _vehicleHealthSnapshot = _templates[Renter].Health;
                    StartRoute(_selectedTruckerLevel);
                    Renter.SendTip("tip_truck_newlvl");
                    break;
            }
        }

        internal enum CurrentRouteType
        {
            None,
            Load,
            Unload,
        }
    }
}