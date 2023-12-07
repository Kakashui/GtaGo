using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Core.QuestPeds;
using Whistler.Docks;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Jobs.ImpovableJobs;
using Whistler.SDK;
using Whistler.VehicleSystem;

namespace Whistler.Jobs.TruckersJob
{
    internal class TruckRentPark
    {
        public int Id { get; set; }
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(TruckRentPark));
        private static List<TruckRentPark> _rentParks = new List<TruckRentPark>();
        public static Dictionary<int, TruckRentPark> RentParks => _rentParks.ToDictionary(key => key.Id);
        public Vector3 Position { get; }

        /// <summary>
        /// Точки спавна грузовиков на стоянке
        /// </summary>
        public List<TruckRentSpot> TruckSpawnPositions { get; }

        private const int TruckRentDelayInHours = 1;
        private Dictionary<Player, Truck> _playersOnWork = new Dictionary<Player, Truck>();
        private Dictionary<Player, int> _truckersSelectedLevels = new Dictionary<Player, int>();
        private Dictionary<Player, string> _waitingTimers = new Dictionary<Player, string>();
        private QuestPed _ped;

        public TruckRentPark(Vector3 position, float pedHeading, List<(Vector3, float)> truckSpawnPoints, Vector3 exitPoint, string pedName)
        {
            Position = position;
            NAPI.Blip.CreateBlip(477, position, 1, 4, Main.StringToU16("Дальнобойщик"), 255, 0, true, 0, 0);
            
            TruckSpawnPositions = new List<TruckRentSpot>();

            foreach (var (spawn, heading) in truckSpawnPoints)
                TruckSpawnPositions.Add(new TruckRentSpot(spawn, heading));

            _ped = new QuestPed(PedHash.Zimbor, Position + new Vector3(0, 0, 1), pedName, "Truckers_13", heading: pedHeading);
            _ped.PlayerInteracted += (player, ped) =>
            {
                try
                {
                    // Если до этого арендовал в другом парке
                    if (RentParks.Values.FirstOrDefault(p => p.IsPlayerInPark(player) && p.Id != Id) != null)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.Top, "Truckers_27".Translate(), 4000);
                        return;
                    }
                    if (_truckersSelectedLevels.ContainsKey(player))
                    {
                        var introPage = new DialogPage("Truckers_14", ped.Name, ped.Role)
                            .AddAnswer("House_58", StopRentForPlayer)//Да
                            .AddCloseAnswer();
                        introPage.OpenForPlayer(player);
                    }
                    else
                    {
                        var rewardDescriptionPage = new DialogPage("Truckers_15" , ped.Name,
                                ped.Role)
                            .AddAnswer("Truckers_16", OnPlayerAgreedToRent)
                            .AddCloseAnswer("Truckers_17");

                        var jobDescriptionPage = new DialogPage("Truckers_18", ped.Name,
                                ped.Role)
                            .AddAnswer("Truckers_16", OnPlayerAgreedToRent)
                            .AddAnswer("Truckers_20", rewardDescriptionPage)
                            .AddCloseAnswer("Truckers_17");
                        
                        var introPage = new DialogPage("Truckers_19",
                                ped.Name, ped.Role)
                            .AddAnswer("Truckers_21", jobDescriptionPage)
                            .AddAnswer("Truckers_22", OnPlayerAgreedToRent)
                            .AddAnswer("Truckers_23", rewardDescriptionPage)
                            .AddCloseAnswer("Truckers_17");
                        
                        introPage.OpenForPlayer(player);
                    }
                }
                catch (Exception e) { _logger.WriteError("TruckRent: " + e.ToString()); }
            };
            Id = _rentParks.Count;
            _rentParks.Add(this);
        }

        public bool IsPlayerInPark(PlayerGo player)
        {
            return _truckersSelectedLevels.ContainsKey(player);
        }

        public void StopRentForPlayer(PlayerGo player)
        {
            if (_playersOnWork.TryGetValue(player, out var workingTruck))
            {
                workingTruck.Destroy(player);
                _playersOnWork.Remove(player);
            }
            if (_truckersSelectedLevels.TryGetValue(player, out var level)) _truckersSelectedLevels.Remove(player);
            ProcessWaitingTimer(player);
            player.TriggerEvent("truckers:resetCurrentRoutePath");
            Notify.Send(player, NotifyType.Info, NotifyPosition.Top, "Truckers_6".Translate(), 4000);
        }

        public void OnPlayerEnteredRouteColshape(PlayerGo player)
        {
            if (!_playersOnWork.TryGetValue(player, out var workingPlayer)) return;
            workingPlayer.EnteredCurrentRoutePath();
        }
        
        public void StartWaitingTimer(PlayerGo player)
        {
            if (_waitingTimers.ContainsKey(player)) return;
            Notify.Send(player, NotifyType.Info, NotifyPosition.Top, "Truckers_5".Translate(), 4000);
            _waitingTimers.Add(player, Timers.StartOnce(5 * 60 * 1000, () => StopRentForPlayer(player)));
            player.TriggerEvent("truckers:startWorkTimer", 300);
        }

        private void OnPlayerAgreedToRent(PlayerGo player)
        {
            if (!player.GetCharacter().ImprovableJobs.TryGetValue(ImprovableJobType.ProductsLoader, out var loaderStage))
            {
                var page = new DialogPage("Truckers_11", _ped.Name, _ped.Role).AddCloseAnswer();
                Trigger.ClientEvent(player, "createWaypoint", DockLoaderJob.StartWorkPosition.X, DockLoaderJob.StartWorkPosition.Y);
                page.OpenForPlayer(player);
                return;
            }
            // Если игрок не дорос до дальнобойщика

            if (player.GetCharacter().ImprovableJobs[ImprovableJobType.ProductsLoader]
                .GetCurrentLevel<ProductsLoaderJobStages>() < ProductsLoaderJobStages.Trucker)
            {
                var page = new DialogPage("Truckers_11", _ped.Name, _ped.Role).AddCloseAnswer();
                Trigger.ClientEvent(player, "createWaypoint", DockLoaderJob.StartWorkPosition.X, DockLoaderJob.StartWorkPosition.Y);
                page.OpenForPlayer(player);
                return;
            }
            if (!player.CheckLic(GUI.Documents.Enums.LicenseName.Truck))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Jobs_99".Translate(), 3000);
                return;
            }

            var level = TruckersJobSettings.GetLevel(player.GetCharacter()
                .ImprovableJobs[ImprovableJobType.ProductsLoader].StagesPassed);
            var list = new List<TruckInfoDTO>();
            for (var i = 0; i < TruckersJobSettings.Stages.Values.Count(); i++)
            {
                list.Add(new TruckInfoDTO
                {
                    Id = i,
                    Name = VehicleManager.GetModelName(TruckersJobSettings.Trucks[i].TruckHash),
                    RentCost = TruckersJobSettings.Stages[i].RentCost,
                    AvailableLevel = i + 1,
                    Available = level >= i,
                    Payment = TruckersJobSettings.Stages[i].PaymentPerUnload,
                    Reward = TruckersJobSettings.Stages[i].GoCoinsReward
                });
            }

            var data = new
            {
                rankName = player.GetCharacter().FullName,
                level = level + 1,
                currentExp = player.GetCharacter().ImprovableJobs[ImprovableJobType.ProductsLoader].StagesPassed,
                requiredExp = TruckersJobSettings.Stages.GetValueOrDefault(level + 1)?.RequiredTransportations ?? 0,
                trucks = list
            };
            player.SetData("TruckRentParkId", Id);
            player.TriggerEvent("truckers:openRentPage", JsonConvert.SerializeObject(data));
        }

        public void OnPlayerSelectedTruck(PlayerGo player, int truckId)
        {
            if (player.GetCharacter().Money < TruckersJobSettings.Stages[truckId].RentCost)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Center, "Core_178", 3000);
                return;
            }
            SpawnTruckOrClearSpots(player, truckId);
        }

        private void SpawnTruckOrClearSpots(PlayerGo player, int selectedLevel)
        {
            var firstUnusedSpot = TruckSpawnPositions.FirstOrDefault(s => s.Busy == false);

            if (firstUnusedSpot == null)
            {
                TruckSpawnPositions.ForEach(p => p.Busy = false);
                firstUnusedSpot = TruckSpawnPositions.FirstOrDefault();
            }
            
            _truckersSelectedLevels.Add(player, selectedLevel);
            SpawnTruckForPlayer(player, new Truck(TruckersJobSettings.Trucks[selectedLevel]),
                        firstUnusedSpot.Position, firstUnusedSpot.Heading,
                        TruckersJobSettings.Stages[selectedLevel].RentCost);
            Notify.Send(player, NotifyType.Alert, NotifyPosition.Top, "Truckers_3".Translate(), 3000);
            firstUnusedSpot.Busy = true;
        }

        private void SpawnTruckForPlayer(PlayerGo player, Truck truck, Vector3 position, float heading, int payment)
        {
            truck.Instantiate(player, position, heading, Id, payment);
            player.TriggerEvent("truckers:vehicleLoaded", JsonConvert.SerializeObject(truck.Position));
            _playersOnWork.Add(player, truck);
        }

        public void ProcessWaitingTimer(PlayerGo player)
        {
            if (!_waitingTimers.TryGetValue(player, out var timer)) return;
            
            Timers.Stop(timer);
            player.TriggerEvent("truckers:stopWorkTimer");
            _waitingTimers.Remove(player);
        }

        public void OnPlayerDisconnected(PlayerGo player)
        {
            if (_waitingTimers.TryGetValue(player, out var timer))
            {
                Timers.Stop(timer);
                _waitingTimers.Remove(player);
            }
            
            if (Truck.PlayersUnloadTime.ContainsKey(player)) Truck.PlayersUnloadTime.Remove(player);

            if (_playersOnWork.TryGetValue(player, out var workingTruck))
            {
                workingTruck.Destroy(player);
                _playersOnWork.Remove(player);
            }
            
            if (_truckersSelectedLevels.TryGetValue(player, out var level)) _truckersSelectedLevels.Remove(player);
        }

        public void OnPlayerEnterVehicle(PlayerGo player)
        {
            if (!_playersOnWork.TryGetValue(player, out var truck)) return;
            // Первый раз после выдачи сел в грузовик
            if (!truck.IsOnWork) truck.StartRoute(_truckersSelectedLevels[player]);
        }
    }
    
    internal class TruckRentSpot
    {
        public Vector3 Position { get; }

        public bool Busy { get; set; }
        
        public float Heading { get; }

        public TruckRentSpot(Vector3 position, float heading)
        {
            Position = position;
            Heading = heading;
        }
    }
}