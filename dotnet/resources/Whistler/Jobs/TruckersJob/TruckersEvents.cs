using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Common;
using Whistler.Core;
using Whistler.Core.UserDialogs;
using Whistler.Docks;
using Whistler.Entities;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.Jobs.TruckersJob
{
    internal class TruckersEvents : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(TruckersEvents));
        public TruckersEvents()
        {
            TruckersJobSettings.Init();
            new TruckRentPark(new Vector3(1214.342, 1843.516, 77.8776), 200, new List<(Vector3, float)>
            {
                (new Vector3(1220.066, 1844.639, 77.95108), 219),
                (new Vector3(1226.219, 1845.732, 77.95667), 219),
                (new Vector3(1243.271, 1866.733, 77.96034), 219),
                (new Vector3(1244.461, 1863.932, 78.16086), 219),
                (new Vector3(1210.938, 1836.68, 77.84169), 219),
            }, new Vector3(1174.393, 1817.333, 73.78917), "Billy Bob");

            new TruckRentPark(new Vector3(845.7662, -902.5869, 24.25148), 275, new List<(Vector3, float)>
            {
                (new Vector3(852.5505, -906.0693, 24.17249), 269),
                (new Vector3(852.726, -902.2256, 24.17881), 269),
                (new Vector3(853.1176, -899.4863, 24.19202), 269),
                (new Vector3(853.1365, -895.9651, 24.19249), 269),
                (new Vector3(853.3661, -892.1281, 24.19823), 269),
                (new Vector3(853.7742, -882.5355, 24.20951), 200),
            }, new Vector3(941.8301, -909.9188, 41.36938), "Dud Clark");

            new TruckRentPark(new Vector3(581.3249, -2724.137, 6.116927), 182, new List<(Vector3, float)>
            {
                (new Vector3(563.5824, -2748.524, 4.936064), 334),
                (new Vector3(559.7679, -2746.273, 4.936261), 334),
                (new Vector3(546.5737, -2741.394, 4.93632), 334),
                (new Vector3(542.178, -2738.906, 4.936344), 334),
                (new Vector3(538.0768, -2736.408, 4.936287), 334),
                (new Vector3(533.7974, -2734.077, 4.936287), 334),
            }, new Vector3(603.7774, -2757.222, 4.94164), "Bobo Jacksonville");
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(ExtPlayer player, DisconnectionType type, string reason)
        {
            try
            {
                foreach (var park in TruckRentPark.RentParks.Values) park.OnPlayerDisconnected(player);
            }
            catch (Exception ex) {_logger.WriteError("TruckersError: " + ex);}
        }
        
        [RemoteEvent("truckers:playerEnteredCheckpoint")]
        public void OnPlayerEnteredCheckpoint(ExtPlayer player)
        {
            try
            {
                ExtVehicle vehicle = player.Vehicle as ExtVehicle;
                if (vehicle == null || vehicle.Data.OwnerType != OwnerType.Temporary || (vehicle.Data as TemporaryVehicle).Access != VehicleAccess.WorkTruck) return;
                var carPark = TruckRentPark.RentParks[vehicle.GetData<int>("RENTPARK_ID")];
                carPark.OnPlayerEnteredRouteColshape(player);
            }
            catch (Exception ex) {_logger.WriteError("TruckersError: " + ex);}
        }
                
        [RemoteEvent("truckers:setTruck")]
        public void OnPlayerSettedTruck(ExtPlayer player, int id)
        {
            try
            {
                TruckRentPark.RentParks[player.GetData<int>("TruckRentParkId")]
                    .OnPlayerSelectedTruck(player, id);
            }
            catch (Exception ex) {_logger.WriteError("TruckersError: " + ex);}
        }

        [Command("trucklevel")]
        public void SetTruckLevelCommand(ExtPlayer player, int id, int level)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "trucklevel")) return;
                ExtPlayer target = Trigger.GetPlayerByUuid(id);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "Данного игрока не существует.", 3000);
                    return;
                }

                if (!target.Character.ImprovableJobs.ContainsKey(ImpovableJobs.ImprovableJobType.ProductsLoader))
                {
                    target.Character.ImprovableJobStates.Add(new ImpovableJobs.ImprovableJobState(ImpovableJobs.ImprovableJobType.ProductsLoader));
                }

                target.Character.ImprovableJobs[ImpovableJobs.ImprovableJobType.ProductsLoader].CurrentLevel = 1;
                if (!TruckersJobSettings.Stages.ContainsKey(level))
                {
                    Notify.Send(player, NotifyType.Info, NotifyPosition.Bottom, "job:truck:1", 3000);
                    return;
                }
                target.Character.ImprovableJobs[ImpovableJobs.ImprovableJobType.ProductsLoader].StagesPassed = TruckersJobSettings.Stages[level].RequiredTransportations + 1;
                Notify.SendAlert(target, $"Ваш уровень дальнобойщика был изменён на {level}.");
            }
            catch (Exception ex) { _logger.WriteError("TruckersErrorCommand: " + ex); }
        }
    }
}