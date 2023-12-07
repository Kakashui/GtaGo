using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Whistler.Core;
using Whistler.Docks;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Jobs.ImpovableJobs;
using Whistler.NewDonateShop;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;

namespace Whistler.Jobs.TruckersJob
{
    /// <summary>
    /// Грузовик для аренды дальнобойщиками
    /// </summary>
    internal class Truck
    {
        public uint TruckHash { get; }
        private static Dictionary<ExtPlayer, ExtVehicle> _templates = new Dictionary<ExtPlayer, ExtVehicle>();
        public static Dictionary<ExtPlayer, DateTime> PlayersUnloadTime = new Dictionary<ExtPlayer, DateTime>();
        
        private static List<Vector3> _trailersLoadPool = new List<Vector3>();// Список точек загрузки где стоит и ждет трейлер  
        private TruckRoute _currentRoute;
        private float _vehicleHealthSnapshot;
        private int _selectedTruckerLevel;
        public bool IsOnWork { get; private set; }
        
        public Vector3 Position { get; private set; }
        private string _rentTimerId;
        public ExtPlayer Renter { get; private set; }
        private readonly Color _color;
        
        public Truck(TruckInfo info)
        {
            TruckHash = info.TruckHash;
            _color = info.Color;
        }
        
        public void Instantiate(ExtPlayer player, Vector3 position, float heading, int parkId, int payment)
        {
            Position = position;
            Renter = player;
            var vehicle = VehicleManager.CreateTemporaryVehicle(TruckHash, position, new Vector3(0, 0, heading), "TRUCK", VehicleAccess.WorkTruck, player);
            VehicleCustomization.SetPowerTorque(vehicle, 10, 1);
            vehicle.CustomPrimaryColor = _color;
            VehicleStreaming.SetEngineState(vehicle, true);
            VehicleStreaming.SetLockStatus(vehicle, false);
            MoneySystem.Wallet.MoneySub(player.Character, payment, "Оплата аренды");
            _rentTimerId = Timers.Start(3600000, () => // Раз в час
            {
                if(MoneySystem.Wallet.MoneySub(player.Character, payment, "Оплата аренды (почасовая)")) return;

                Notify.SendAlert(player, "У Вас не хватило денег на оплату аренды в час, поэтому договор аренды прекращён.");
                TruckRentPark.RentParks[parkId].StopRentForPlayer(player, false);
            });
            SafeTrigger.SetData(vehicle, "RENTPARK_ID", parkId);
            _templates.Add(player, vehicle);
        }

        /// <summary>
        /// Начать маршрут
        /// </summary>
        /// <param name="selectedLevel">Выбранный уровень</param>
        public void StartRoute(int selectedLevel)
        {
            if (!IsOnWork) IsOnWork = true;
            _selectedTruckerLevel = selectedLevel;

            if (selectedLevel > 2)
            {
                Order order = Dock.CurrentOrders.FirstOrDefault(o => o?.Taked == false);
                if (order != null && BusinessManager.BizList[order.Customer.ID].UnloadPoint != null
                                  && BusinessManager.BizList[order.Customer.ID].UnloadPoint != new Vector3(0, 0, 0))
                {
                    _currentRoute = new TruckRoute(TruckersJobSettings.DockLoadPoints.GetRandomElement().Item1, BusinessManager.BizList[order.Customer.ID].UnloadPoint, null, orderFromDock: order);
                    string bizName = string.IsNullOrEmpty(order.Customer.Name) ? order.Customer.TypeModel.TypeName : order.Customer.Name;
                    Notify.SendInfo(Renter, $"Поступил заказ от бизнеса {bizName}", 3000);
                    order.Taked = true;
                    SetLoadPoint();
                    return;
                }
            }

            if (!TruckersJobSettings.Stages.ContainsKey(selectedLevel))
            {
                Notify.SendError(Renter, "Ошибка получения заказа, свяжитесь с администрацией.");
                return;
            }

            List<(Vector3, float)> freeLoadPoints = null;
            if (TruckersJobSettings.Stages[selectedLevel].TrailerHash != null) freeLoadPoints = TruckersJobSettings.Stages[selectedLevel].LoadPoints.Where(p => _trailersLoadPool.All(l => l != p.Item1)).ToList();
            else freeLoadPoints = TruckersJobSettings.Stages[selectedLevel].LoadPoints;
            if (freeLoadPoints == null || !freeLoadPoints.Any())
            {
                Notify.SendError(Renter, "Ошибка получения точки загрузки, свяжитесь с администрацией.");
                return;
            }
            if (TruckersJobSettings.Stages[selectedLevel].UnloadPoints == null || !TruckersJobSettings.Stages[selectedLevel].UnloadPoints.Any())
            {
                Notify.SendError(Renter, "Ошибка получения точки разгрузки, свяжитесь с администрацией.");
                return;
            }

            (Vector3, float) load = freeLoadPoints.GetRandomElement();
            List<Vector3> unloadPointsWhereBigDistance = TruckersJobSettings.Stages[selectedLevel].UnloadPoints.Where(u => load.Item1.DistanceTo2D(u) > 2900).ToList();
            Vector3 unload = unloadPointsWhereBigDistance.GetRandomElement();
            if (TruckersJobSettings.Stages[selectedLevel].TrailerHash == null)
            {
                _currentRoute = new TruckRoute(load.Item1, unload, null);
                SetLoadPoint();
                return;
            }

            ExtVehicle trailer = VehicleManager.CreateTemporaryVehicle(NAPI.Util.GetHashKey(TruckersJobSettings.Stages[selectedLevel].TrailerHash), load.Item1, new Vector3(0f, 0f, load.Item2), "TRAILER", VehicleAccess.WorkTruck, Renter);
            if (trailer == null)
            {
                Notify.SendError(Renter, "Ошибка создания трейлера, свяжитесь с администрацией.");
                return;
            }

            VehicleCustomization.SetPowerTorque(trailer, 10, 1);
            trailer.CustomPrimaryColor = _color;
            VehicleStreaming.SetEngineState(trailer, true);
            VehicleStreaming.SetLockStatus(trailer, false);
            SafeTrigger.SetData(Renter.Vehicle as ExtVehicle, "VehicleTrailer", trailer);
            _currentRoute = new TruckRoute(load.Item1, unload, trailer);
            SetLoadPoint();
        }
        
        [ServerEvent(Event.VehicleTrailerChange)]
        public void OnVehicleTrailerChange(ExtVehicle vehicle, ExtVehicle trailer)
        {

        }


        public void Destroy(ExtPlayer player)
        {
            if (_currentRoute.Trailer != null)
            {
                _currentRoute.Trailer.CustomDelete();
                _currentRoute.Trailer = null;
            }

            var loadPointFromPool = _trailersLoadPool.FirstOrDefault(p => p == _currentRoute.LoadPoint);
            if (loadPointFromPool != null) _trailersLoadPool.Remove(loadPointFromPool);
            
            if (_currentRoute.OrderFromDock != null) _currentRoute.OrderFromDock.Taked = false;
            
            _currentRoute.CurrentRouteType = CurrentRouteType.None;
            _templates[player]?.CustomDelete();
            _templates.Remove(player);
            Timers.Stop(_rentTimerId);
        }

        private void SetLoadPoint()
        {
            _currentRoute.CurrentRouteType = CurrentRouteType.Load;
            SafeTrigger.ClientEvent(Renter, "truckers:setCheckPointRoutePath", JsonConvert.SerializeObject(_currentRoute.LoadPoint));
            _vehicleHealthSnapshot = _templates[Renter].Health;
            
            if (_currentRoute.Trailer != null){
                Notify.Send(Renter, NotifyType.Success, NotifyPosition.BottomCenter,
                   "Truckers_9_1".Translate(), 5000);
            }
               

            Notify.Send(Renter, NotifyType.Success, NotifyPosition.BottomCenter,
            "Truckers_9".Translate(), 5000);
        }
        
        private void SetUnLoadPoint()
        {
            _currentRoute.CurrentRouteType = CurrentRouteType.Unload;
            SafeTrigger.ClientEvent(Renter, "truckers:setCheckPointRoutePath", 
                JsonConvert.SerializeObject(_currentRoute.UnloadPoint));
        }
        
        public void EnteredCurrentRoutePath()//todo: отрефакторить
        {
            switch (_currentRoute.CurrentRouteType)
            {
                case CurrentRouteType.None: return;
                case CurrentRouteType.Load:
                {
                    if (_currentRoute.Trailer != null)
                    {
                       var loadPointFromPool = _trailersLoadPool.FirstOrDefault(p => p == _currentRoute.LoadPoint);
                       if (loadPointFromPool != null) _trailersLoadPool.Remove(loadPointFromPool);
                    }

                    SetUnLoadPoint();
                    break;
                }
                case CurrentRouteType.Unload:
                    if (_currentRoute.Trailer != null && Renter.Vehicle.GetData<ExtVehicle>("VehicleTrailer") != _currentRoute.Trailer)
                    {
                        Notify.Send(Renter, NotifyType.Error, NotifyPosition.BottomCenter, "Truckers_12".Translate(), 3000);
                        return;
                    }

                    if (!PlayersUnloadTime.ContainsKey(Renter)) PlayersUnloadTime.Add(Renter, DateTime.Now);
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

                    var levelBeforeIncrementation = TruckersJobSettings.GetLevel(Renter.Character.ImprovableJobs[ImprovableJobType.ProductsLoader].StagesPassed);
                    // Повышаем поездки только если на последнем уровне
                    if (levelBeforeIncrementation == _selectedTruckerLevel)
                        Renter.Character.ImprovableJobs[ImprovableJobType.ProductsLoader].StagesPassed++;
                    var newLevel = TruckersJobSettings.GetLevel(Renter.Character.ImprovableJobs[ImprovableJobType.ProductsLoader].StagesPassed);
                    if (levelBeforeIncrementation < newLevel)
                    {
                        // if (TruckersJobSettings.Stages[newLevel].MCoinsReward.HasValue)
                        // {
                        //     Renter.AddMCoins(TruckersJobSettings.Stages[newLevel].MCoinsReward.Value);
                        //     Notify.Alert(Renter, "Truckers_25".Translate(TruckersJobSettings.Stages[newLevel].MCoinsReward.Value));
                        // }
                        Notify.Alert(Renter,"Truckers_26");
                    }
                    var basePayment = DonateService.UseJobCoef(Renter, TruckersJobSettings.Stages[_selectedTruckerLevel].PaymentPerUnload, Renter.Character.IsPrimeActive());
                    var paymentWithLoseFactor = DonateService.UseJobKoef(Renter, basePayment - basePayment * healthLosePercents / 2, Renter.Character.IsPrimeActive());
                    
                    if (healthLosePercents > 5 && paymentWithLoseFactor < basePayment 
                        && (basePayment - paymentWithLoseFactor) > basePayment / 2)
                    {
                        Notify.Send(Renter, NotifyType.Success, NotifyPosition.BottomCenter, "Truckers_8".Translate(paymentWithLoseFactor), 3000);
                        MoneySystem.Wallet.MoneyAdd(Renter.Character, paymentWithLoseFactor, "Работа дальнобойщика");
                    }
                    
                    else 
                    {
                        Notify.Send(Renter, NotifyType.Success, NotifyPosition.BottomCenter, "Truckers_7".Translate(basePayment), 3000);
                        MoneySystem.Wallet.MoneyAdd(Renter.Character, basePayment, "Работа дальнобойщика");
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