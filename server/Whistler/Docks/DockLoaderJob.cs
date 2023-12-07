using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Core.QuestPeds;
using Whistler.Core.UserDialogs;
using Whistler.Helpers;
using Whistler.Jobs.ImpovableJobs;
using Whistler.MoneySystem;
using Whistler.NewDonateShop;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Object = GTANetworkAPI.Object;

namespace Whistler.Docks
{
    internal class DockLoaderJob : Script
    {
        public const int JobId = 7;
        public const int Payment = 800;
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(DockLoaderJob));
        public static Vector3 StartWorkPosition = new Vector3(1196.765, -3253.958, 6.117299);

        private static readonly Dictionary<Player, DockLoader> _currentlyWorkingPlayers = new Dictionary<Player, DockLoader>();
        private static readonly Queue<Player> _waitingForWorkPlayers = new Queue<Player>();
        private static readonly List<DockCrate> _crates = new List<DockCrate>();
        private static QuestPed _workerPed;
        public static Dictionary<Player, Vehicle> OccupiedVehicles = new Dictionary<Player, Vehicle>();
        private static readonly List<DockRentPoint> _forkliftsSpots = new List<DockRentPoint>
        {
            new DockRentPoint(new Vector3(1190.624, -3236.569, 4.908762), 90),
            new DockRentPoint(new Vector3(1190.624, -3239.549, 4.908763), 90),
            new DockRentPoint(new Vector3(1190.624, -3242.981, 4.908763), 90),
            new DockRentPoint(new Vector3(1190.624, -3245.774, 4.908763), 90),
            new DockRentPoint(new Vector3(1190.624, -3249.351, 4.908764), 90),
        };

        private static readonly List<(Vector3, float)> _cratesPositions = new List<(Vector3, float)>
        {
           (new Vector3(893.1829, -2975.547, 4.780777), 233),
           (new Vector3(892.8143, -3025.354, 4.781332), 233),
           (new Vector3(968.7137, -3050.437, 4.782042), 143),
           (new Vector3(988.5504, -2965.386, 4.781137), 162),
           (new Vector3(1089.415, -2965.698, 4.779738), 162),
           (new Vector3(1066.701, -2998.558, 4.780994), 273),
           (new Vector3(1168.951, -2976.721, 4.782102), 273),
           (new Vector3(874.6943, -3106.577, 4.776416), 273),
           (new Vector3(1063.771, -3088.344, 4.781046), 273),
           (new Vector3(1033.096, -3107.59, 4.776196), 273),
           (new Vector3(1198.474, -2942.739, 4.777205), 273),
           (new Vector3(1199.384, -2926.024, 4.77737), 273),
           (new Vector3(968.9349, -3105.116, 4.777856), 273),
           (new Vector3(901.4993, -3086.717, 4.777863), 273),
           (new Vector3(865.5057, -3026.041, 4.723681), 273),
           (new Vector3(1129.873, -3085.039, 4.684203), 273),
           (new Vector3(1067.822, -3086.325, 4.776445), 273),
           (new Vector3(949.1141, -3284.886, 4.777288), 273),
           (new Vector3(1064.762, -3301.659, 4.770942), 273), 
           (new Vector3(952.2217, -3259.52, 4.768867), 273),
           (new Vector3(960.3692, -3129.228, 4.780751), 273),
           (new Vector3(1018.091, -3129.095, 4.77549), 273),
           (new Vector3(1215.447, -3054.885, 4.741466), 273),
           (new Vector3(1215.681, -2905.814, 4.744663), 208),
           (new Vector3(1125.4, -3311.119, 4.783059), 180),
           (new Vector3(946.758, -3285.433, 4.77863), 180),
           (new Vector3(852.2266, -3277.266, 4.780234), 102),
           (new Vector3(807.8425, -3278.623, 4.775817), 200),
           (new Vector3(762.2357, -3228.155, 4.94051), 270),
        };

        private static List<Vector3> _unloadPoints = new List<Vector3>
        {
            new Vector3(827.3211, -3205.038, 5.8),
            new Vector3(834.8186, -3205.689, 5.8),
            new Vector3(765.8806, -3188.101, 5.8),
            new Vector3(764.6663, -3201.99, 5.8),
            new Vector3(1190.248, -3328.622, 5.8),
            new Vector3(1205.208, -3328.99, 5.8),
            new Vector3(1232.749, -3327.333, 5.8),
            new Vector3(1189.188, -3108.802, 5.8),
            new Vector3(1219.478, -3202.66, 5.8),
            new Vector3(1235.325, -3201.514, 5.8)
        };
        
        public DockLoaderJob()
        {
            foreach (var position in _cratesPositions)
                _crates.Add(new DockCrate(NAPI.Object.CreateObject(153748523, position.Item1, new Vector3(0,0,position.Item2))));

            InteractShape.Create(StartWorkPosition, 1, 2)
                .AddInteraction(StartWorkingDay, "interact_13");

            NAPI.Blip.CreateBlip(356, StartWorkPosition, 1, 62, Main.StringToU16("Работа грузчика"), 255, 0, true, 0, 0);

            foreach (var unloadPoint in _unloadPoints)
            {
                var colshape = NAPI.ColShape.CreatCircleColShape(unloadPoint.X, unloadPoint.Y, 4);
                colshape.OnEntityEnterColShape += (shape, player) =>
                {
                    try
                    {
                        var crate = _crates.FirstOrDefault(c => c.PlayerWorking == player);
                        if (crate == null) return;
                        player.TriggerEvent("dockLoader:playerUnloaded");    
                    }
                    catch (Exception e) { _logger.WriteError("DockLoaderColshape: " + e.ToString()); }
                };
            }
            Main.PlayerPreDisconnect += OnPlayerPreDisconnect;
            InitPedWithDialog();
        }

        private static void InitPedWithDialog()
        {
            
            _workerPed = new QuestPed(PedHash.Dockwork01SMY, StartWorkPosition + new Vector3(0, 0, 1), "John Rios", "DockLoader_10", 100);
            _workerPed.PlayerInteracted += (player, ped) =>
            {
                var workDescriptionPage = new DialogPage("DockLoader_11", ped.Name, ped.Role);
                DialogPage startDialogPage;
                DialogPage workCurrentStage;
                if (_currentlyWorkingPlayers.ContainsKey(player))
                {
                    startDialogPage = new DialogPage("DockLoader_12", ped.Name, ped.Role)
                        .AddAnswer("QuestDialog_1".Translate( _currentlyWorkingPlayers[player].CurrentPayment), StopWorkingDayForPlayer);
                }
                else
                {
                    startDialogPage = new DialogPage("DockLoader_13",
                            ped.Name, ped.Role)
                        .AddAnswer("QuestDialog_1_1", StartWorkingDayForPlayer);
                }
                workDescriptionPage
                    .AddAnswer("QuestDialog_3", startDialogPage)
                    .AddCloseAnswer();
                if (!player.GetCharacter().ImprovableJobs.ContainsKey(ImprovableJobType.ProductsLoader))
                    player.GetCharacter().ImprovableJobStates.Add(new ImprovableJobState(ImprovableJobType.ProductsLoader));
                if (player.GetCharacter().ImprovableJobs[ImprovableJobType.ProductsLoader]
                    .GetCurrentLevel<ProductsLoaderJobStages>() == ProductsLoaderJobStages.DockLoader)
                {
                    workCurrentStage = new DialogPage("DockLoader_15".Translate(
                            30 - player.GetCharacter().ImprovableJobs[ImprovableJobType.ProductsLoader].StagesPassed)
                        , ped.Name, ped.Role);
                }
                else
                {
                    workCurrentStage = new DialogPage("DockLoader_17", ped.Name, ped.Role);
                }
                workCurrentStage
                    .AddAnswer("QuestDialog_3", startDialogPage)
                    .AddCloseAnswer();
                startDialogPage
                    .AddAnswer("QuestDialog_2", workDescriptionPage)
                    .AddAnswer("DockLoader_16", workCurrentStage)
                    .AddCloseAnswer();

                startDialogPage.OpenForPlayer(player);
            };
        }
        
        private static void OnPlayerPreDisconnect(Player player) 
        {
            try
            {
                if (_currentlyWorkingPlayers.TryGetValue(player, out var workingPlayer))
                {
                    workingPlayer.StopWorkingDay();
                    _currentlyWorkingPlayers.Remove(player);
                    OccupiedVehicles[player].CustomDelete();
                    OccupiedVehicles.Remove(player);
                }
                if (player.GetCharacter().WorkID == 7) 
                    player.GetCharacter().WorkID = 0; 
                var crate = _crates.FirstOrDefault(c => c.PlayerWorking == player);
                if (crate != null) DismissCrate(crate);    
            }
            catch (Exception e) 
            {
                _logger.WriteError("DockLoader: " + e); 
            }
        }

        [RemoteEvent("playerUnloadedDockCrate")]
        public void OnPlayerUnloaded(Player player)
        {
            try
            {
                //MoneySystem.Wallet.Change(player, Payment);
                var additionalPayment = DonateService.UseJobCoef(player, Payment);                 
                _currentlyWorkingPlayers[player].CurrentPayment += additionalPayment;
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter,
                    "DockLoader_7".Translate( additionalPayment), 3000);

                var jobState = player.GetCharacter().ImprovableJobs[ImprovableJobType.ProductsLoader];
                if (jobState.GetCurrentLevel<ProductsLoaderJobStages>() == ProductsLoaderJobStages.DockLoader)
                {
                    jobState.StagesPassed++;
                    if (jobState.StagesPassed >=
                        ImprovableJobsSettings.ProductLoaderActionsToNewLevelCount[ProductsLoaderJobStages.DockLoader])
                    {
                        jobState.SetCurrentLevel(ProductsLoaderJobStages.Trucker);
                        Notify.Alert(player, "DockLoader_9");
                    }
                }
                var crate = _crates.FirstOrDefault(c => c.PlayerWorking == player);
                if (crate != null) DismissCrate(crate);
                GiveCrateToPlayer(player);
                player.SendTip("tip_docker_progress");
            }
            catch (Exception e) 
            { 
                _logger.WriteError("DockLoader: " + e.ToString()); 
            }
        }

        [ServerEvent(Event.PlayerConnected)]
        public void OnPlayerConnected(Player player)
        {
            try
            {
                player.TriggerEvent("dockLoader:init", JsonConvert.SerializeObject(_crates.Select(e => e.Id)),
                    JsonConvert.SerializeObject(_unloadPoints));
            }
            catch (Exception e) 
            {
                _logger.WriteError("DockLoader_PlayerConnected: " + e.ToString()); 
            }
        }

        public static void StartWorkingDay(Player player)
        {
            if (!_currentlyWorkingPlayers.ContainsKey(player))
            {
                var dialog = new UserDialog(player, "DockLoader_3".Translate());
                dialog.PlayerAgreed += () => StartWorkingDayForPlayer(player);
            }
            else
            {
                var dialog = new UserDialog(player, "DockLoader_8".Translate());
                dialog.PlayerAgreed += () => StopWorkingDayForPlayer(player);
            }
        }

        private static void StartWorkingDayForPlayer(Player player)
        {
            try
            {
                if (!player.GetCharacter().ImprovableJobs.ContainsKey(ImprovableJobType.ProductsLoader))
                    player.GetCharacter().ImprovableJobStates.Add(new ImprovableJobState(ImprovableJobType.ProductsLoader));
                Notify.Send(player, NotifyType.Info, NotifyPosition.Top, "DockLoader_14", 3000);
                var spot = _forkliftsSpots.FirstOrDefault(f => f.Occupied == false);
                if (spot == null)
                {
                    _forkliftsSpots.ForEach(f => f.Occupied = false);
                    spot = _forkliftsSpots.FirstOrDefault();
                }
                var vehicle = VehicleManager.CreateTemporaryVehicle("forklift", spot.Position, new Vector3(0, 0, spot.Heading), "DOCK", VehicleAccess.DockLoader, player);
                
                VehicleManager.GoVehicles[vehicle].Data.VehCustomization.PrimColor = new Color(0, 73, 254);
                VehicleManager.ApplyCustomization(vehicle);
                
                VehicleStreaming.SetLockStatus(vehicle, false);
                player.TriggerEvent("dockLoader:vehicleLoaded", JsonConvert.SerializeObject(spot.Position));
                if (OccupiedVehicles.ContainsKey(player))
                {
                    OccupiedVehicles[player]?.CustomDelete();
                    OccupiedVehicles.Remove(player);
                }
                OccupiedVehicles.Add(player, vehicle);
                _currentlyWorkingPlayers.Add(player, new DockLoader(player));
                _currentlyWorkingPlayers[player].StartWorkingDay();
                GiveCrateToPlayer(player);
                player.SendTip("tip_docker_start");
            }
            catch (Exception e)
            {
                _logger.WriteError("StartWorkingDayForPlayer: " + e.ToString());
            }
            
        }       
        
        private static void StopWorkingDayForPlayer(Player player)
        {
            MoneySystem.Wallet.MoneyAdd(player.GetCharacter(), _currentlyWorkingPlayers[player].CurrentPayment, "Money_DockPayment");
            _currentlyWorkingPlayers[player].StopWorkingDay();
            _currentlyWorkingPlayers.Remove(player);
            OccupiedVehicles[player]?.CustomDelete();
            OccupiedVehicles.Remove(player);
            var crate = _crates.FirstOrDefault(c => c.PlayerWorking == player);
            if (crate != null) DismissCrate(crate);
        }

        private static void GiveCrateToPlayer(Player player)
        {
            var crate = RequestCrate(player);
            if (crate == null)
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.Top, "DockLoader_5".Translate(), 3000);
                if  (!_waitingForWorkPlayers.Contains(player))
                    _waitingForWorkPlayers.Enqueue(player);
            }
            else
            {
                crate.Claim(player);
                player.TriggerEvent("dockLoader:CrateObjectsRequested", crate.Id);
            }
        }

        private static DockCrate RequestCrate(Player player)
        {
            var unusedCrates = _crates.Where(c => c.IsFree);
            var randomCrateIndex = new Random().Next(0, unusedCrates.Count());

            return unusedCrates.ToList()[randomCrateIndex];
        }

        private static void DismissCrate(DockCrate crate)
        {
            crate.Reset();
            if (!_waitingForWorkPlayers.Any()) return;
            var firstPlayerInQueue = _waitingForWorkPlayers.Dequeue();
            if (firstPlayerInQueue == null) return;
            GiveCrateToPlayer(firstPlayerInQueue);
        }
    }
}