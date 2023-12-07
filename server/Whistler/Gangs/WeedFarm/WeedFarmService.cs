using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Whistler.Core;
using Whistler.Gangs.WeedFarm.Models;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models.VehiclesData;
using System.Linq;
using System.Threading.Tasks;
using Whistler.Fractions.Models;
using Whistler.Scenes;
using Whistler.Scenes.Configs;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.MoneySystem;
using Whistler.MP.OrgBattle;
using Whistler.Common;
using Whistler.Entities;
using Whistler.Fractions;

namespace Whistler.Gangs.WeedFarm
{
    public static class WeedFarmService
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(WeedFarmService));

        private static Vector3 _exitPoint = new Vector3(1065.894, -3183.636, -40.16363);
        private static Vector3 _menuPoint = new Vector3(1044.588, -3194.938, -39.1971);
        private static Vector3 _stockPoint = new Vector3(1040.892, -3192.929, -39.1971);
        private static DeliveryPointsConfig _deliveryPoints;
        private static int _currentFarmBattle = 0;
        private static int _farmBattleToday = 0;
        private const int ComponentsLoadTime = 10; //seconds
        private const int DeliveryJobTariff = 5000; //$
        public const int FarmBattlePeriod = 3;//days
        public const int PlanstOnSpot = 10;
        public const int GrowTime= 30; //minutes 30;
        public const int CountPlantsOnDryStep = 3; //count in minutes
        public const int FarmStockWeight = 50; //kg
        public const int FarmBattleTime = 22; //in hours
        public const int NoyifiacationTime = 60; //in minutes

        internal static void RequestPackAction(Player player)
        {
            player.CurrentFarm()?.RequestPackAction(player);
        }

        private static Dictionary<int, Vector3> _componentTraders = new Dictionary<int, Vector3>
        {
            {0, new Vector3(3639.513, 3770.751, 27.51574)}, //ventilation
            {1, new Vector3(487.6162, -3153.362, 5.070059)}, //security
            {2, new Vector3(-541.0387, -2806.228, 5.000377)}, //light
            {3, new Vector3(-1276.491, -3393.032, 12.94014)}, //chairs
            {4, new Vector3(46.67241, 6300.452, 30.23207)}, //seeds
            {5, new Vector3(1337.779, 4376.645, 43.34803)}  //drying
        };

        private static List<WeedFarmInstance> _farms = new List<WeedFarmInstance>();

        private static List<WeedFarmInstance> _baseFarms = new List<WeedFarmInstance>
        {
            new WeedFarmInstance(new Vector3(2224.576, 5604.627, 53.92282), new Vector3(2194.419, 5602.047, 53.63015), new Vector3(0, 0, 335.5751)),
            new WeedFarmInstance(new Vector3(-264.0295, 2196.459, 129.3986), new Vector3(-271.9196, 2196.045, 129.855), new Vector3(0, 0, 229.3263)),
            new WeedFarmInstance(new Vector3(281.657, 6789.407, 14.86391), new Vector3(280.4149, 6801.626, 15.69547), new Vector3(0, 0, 275.7165))
        };

        internal static void StartFarmBattleByAdmin(Player player, int farmId)
        {
            var farm = _farms.FirstOrDefault(f => f.Id == farmId);
            if(farm == null)
            {
                player.SendError("weedfarm:owner:err:1");
                return;
            }

            if(_currentFarmBattle != 0)
            {
                player.SendError("weedfarm:battle:already".Translate(_currentFarmBattle));
                return;
            }

            if(farm.BattleTimerKey != null)
            {
                Timers.Stop(farm.BattleTimerKey);
                farm.BattleTimerKey = null;
            }
            
            _currentFarmBattle = farmId;
            OrgBattleManager.CreateOrgBattleWithPoints(OrganizationType.Fraction, Families.BattleLocation.StabCity, OrgActivityType.Crime, 20, 1200, 300)
                .AddEndAction(EndFarmBattle)
                .BattleStart();
        }

        internal static void RespawnFarmCar(Player player, int id)
        {
            var farm = _farms.FirstOrDefault(f=>f.Id == id);
            if (farm == null) {
                player.SendError("weedfarm:car:err");
                return;
            }
            farm.RespawnFarmCar();
        }

        private static Dictionary<int,Vector3> _packPoints = new Dictionary<int,Vector3>
        {
            {0,  new Vector3(1039.20642, -3205.952, -40.1) },
            {1,  new Vector3(1037.34717, -3205.80322, -40.1) }
        };

        public static List<Vector3> PlacePoints = new List<Vector3>
        {
            new Vector3(1052.307, -3196.589, -40.03001),
            new Vector3(1051.421, -3192.046, -40.13181),
            new Vector3(1055.705, -3190.121, -40.1299),
            new Vector3(1062.29, -3193.734, -40.04634),
            new Vector3(1062.725, -3197.706, -40.14037),
            new Vector3(1062.937, -3204.01, -40.12094),
            new Vector3(1057.699, -3205.127, -40.03535),
            new Vector3(1057.746, -3199.27, -40.12271),
            new Vector3(1051.688, -3201.953, -40.11529)
        };

        public async static void Init()
        {
            ParseConfig();
            InitDatabase();
            while (!Fractions.Manager.FractionsReady)
            {
                await Task.Delay(100);
            }
            WhistlerTask.Run(() =>
            {
                _deliveryPoints = new DeliveryPointsConfig();
                InitFarms();
                InitPoints();
                InitTraders();
                InitDrying();
            }, 100);
        }      

        private static void ParseConfig()
        {
            if (Directory.Exists("interfaces"))
            {
                using (var w = new StreamWriter("interfaces/gui/src/configs/weedfarm/traders.js"))
                {
                    w.Write($"export default {JsonConvert.SerializeObject(_componentTraders, Formatting.Indented)}");
                }
                using (var w = new StreamWriter("interfaces/gui/src/configs/weedfarm/maxOnStady.js"))
                {
                    w.Write($"export default {JsonConvert.SerializeObject(new List<int> { PlacePoints.Count, PlacePoints.Count * PlanstOnSpot, PlacePoints.Count * PlanstOnSpot, PlacePoints.Count * PlanstOnSpot }, Formatting.Indented)}");
                }
            };
        }

        private static void InitDatabase()
        {
            var query = $"CREATE TABLE IF NOT EXISTS `weedfarm` (" +
                $"  `id` INT NOT NULL AUTO_INCREMENT," +
                $"  `ownerId` INT(11) NOT NULL DEFAULT 0," +
                $"  `occupationDate` DATETIME(4) NOT NULL DEFAULT '2021-01-01 00:00:00'," +
                $"  `components` TEXT NOT NULL," +
                $"  `onDrying` INT(11) NOT NULL DEFAULT 0," +
                $"  `onPacking` INT(11) NOT NULL DEFAULT 0," +
                $"  `onDelivery` INT(11) NOT NULL DEFAULT 0," +
                $"  `irrigationSystem` INT(11) NOT NULL DEFAULT 100," +
                $"  `lightSystem` INT(11) NOT NULL DEFAULT 100," +
                $"  `dyringSystem` INT(11) NOT NULL DEFAULT 100," +
                $"  `ventilationSystem` INT(11) NULL DEFAULT 100," +
                $"  `enterPosition` TEXT NULL," +
                $"  `vehPosition` TEXT NULL," +
                $"  `vehRotation` TEXT NULL," +
                $"  PRIMARY KEY (`id`));";
            MySQL.QuerySync(query);
        }

        private static void InitFarms()
        {          
            var responce = MySQL.QueryRead("SELECT `id` FROM `weedfarm`");
            if (responce.Rows.Count == 0)
                SeedDatabase();
            else
                LoadFarms();
            LoadFarmEnterance();
            LoadFarmVehicles();
            InitFarmBattles();
        }

        private static void InitFarmBattles()
        {
            var dayCounter = 1;
            _farms.ForEach(farm =>
            {
                var nowDate = DateTime.Now;
                var battleTime = farm.OccupationDate.AddDays(FarmBattlePeriod);
                if(battleTime < nowDate)
                {
                    var date = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day, FarmBattleTime, 00, 00);
                    farm.UpdateOccupationDate(date.AddDays(dayCounter++));
                }
                else if (battleTime.Year == nowDate.Year && battleTime.DayOfYear == nowDate.DayOfYear)
                {
                    farm.BattleTimerKey = Timers.StartOnce((int)(battleTime - nowDate).TotalMilliseconds, () => StartFarmBattle(farm.Id));
                    _farmBattleToday = farm.Id;
                    Timers.StartOnce(NoyifiacationTime * 60 * 1000, FarmBattleNotification);
                    return;
                }
            });
        }

        private static void FarmBattleNotification()
        {
            if (_farmBattleToday == 0 || DateTime.Now.Hour > FarmBattleTime) return;
            var msg = "weedfarm:battle:noty".Translate(FarmBattleTime, _farmBattleToday);
            foreach (var player in NAPI.Pools.GetAllPlayers())
            {
                if (player.IsLogged() && (player.IsAdmin() || (player.GetFraction()?.OrgActiveType ?? OrgActivityType.Invalid) == OrgActivityType.Crime))
                    Chat.SendTo(player, msg);
            }
            //Chat.AdminToAll("weedfarm:battle:noty".Translate(FarmBattleTime, _farmBattleToday));
            Timers.StartOnce(NoyifiacationTime * 60 * 1000, FarmBattleNotification);
        }

        private static void StartFarmBattle(int farmId)
        {
            if(_currentFarmBattle != 0)
            {
                var farm = _farms.FirstOrDefault(f => f.Id == farmId);
                if (farm == null) return;
                farm.BattleTimerKey = Timers.StartOnce(60000, () => StartFarmBattle(farmId));
                return;
            }
            _currentFarmBattle = farmId;
            OrgBattleManager.CreateOrgBattleWithPoints(OrganizationType.Fraction, Families.BattleLocation.StabCity, OrgActivityType.Crime, 20, 1200, 300)
                .AddEndAction(EndFarmBattle)
                .BattleStart();
        }

        private static void EndFarmBattle(int winnerId)
        {
            if (winnerId == 0) return;
            var farm = _farms.FirstOrDefault(f => f.Id == _currentFarmBattle);
            _currentFarmBattle = 0;
            var winner = Fractions.Manager.GetFraction(winnerId);
            if (farm == null || winner == null) return;
            farm.SetOwner(winner);
        }

        private static void InitPoints()
        {
            for (int i = 0; i < PlacePoints.Count; i++)
            {
                var place = PlacePoints[i];
                InteractShape.Create(place, 2.5f, 2, uint.MaxValue)
                 .AddEnterPredicate((csh, player) => AllComponentsCollected(player.CurrentFarm()))
                 .AddInteraction((player) => PlacePointAction(player, PlacePoints.IndexOf(place)), "interact_51");
            }

            foreach (var point in _packPoints)
            {
                InteractShape.Create(point.Value, 1f, 2, uint.MaxValue)
                 .AddEnterPredicate((csh, player) => AllComponentsCollected(player.CurrentFarm()) && player.CurrentFarm().OnPacking > 0 && !player.CurrentFarm().SeatPlaces[point.Key].IsLogged())
                 .AddInteraction((player) => player.CurrentFarm()?.PackPointAction(player, point.Key), "interact_53");
            }
        }       

        private static void InitTraders()
        {
            foreach (var trader in _componentTraders)
            {
                InteractShape.Create(trader.Value, 2, 3)
                 .AddMarker(27, trader.Value, 2, InteractShape.DefaultMarkerColor)
                 .AddEnterPredicate((csh, player) => player.IsInVehicle && (player.Vehicle.GetVehicleGo().Data as TemporaryVehicle)?.Access == VehicleAccess.WeedFarm && !player.Vehicle.HasData("weedFarmComp"))
                 .AddInteraction(LoadComponentToVehicle, "interact_47");
            }
        }
        private static void InitDrying()
        {
            Timers.Start(60 * 1000, DryingLoop);
        }

        private static void DryingLoop()
        {
            foreach (var farm in _farms)
            {
                farm.DryingLoop();
            }
        }

        private static void SeedDatabase()
        {
            var days = 1;
            foreach (var farm in _baseFarms)
            {
                var newFarm = new WeedFarmInstance(farm.EnterPoint, farm.VehiclePosition, farm.VehicleRotation);
                newFarm.Components = new List<int>();
                newFarm.Places = new List<int>();
                newFarm.OnDrying = 0;
                newFarm.OnDelivery = 0;
                newFarm.OnPacking = 0;
                newFarm.LightSystemHealth = 100;
                newFarm.IrrigationSystemHealth = 100;
                newFarm.DryingSystemHealth = 100;
                newFarm.VentilationSystemHealth = 100;
                foreach (var place in PlacePoints)
                {
                    newFarm.Places.Add(0);
                }
                var time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, FarmBattleTime, 00, 00);
                var responce = MySQL.QueryRead("INSERT INTO `weedfarm` (`components`, `occupationDate`, `enterPosition`, `vehPosition`, `vehRotation`) VALUES(@prop0, @prop1, @prop2, @prop3, @prop4);  SELECT @@identity;", 
                    JsonConvert.SerializeObject(newFarm.Components), 
                    MySQL.ConvertTime(time.AddDays(days++)),
                    JsonConvert.SerializeObject(newFarm.EnterPoint),
                    JsonConvert.SerializeObject(newFarm.VehiclePosition),
                    JsonConvert.SerializeObject(newFarm.VehicleRotation)
                );
                newFarm.Id = Convert.ToInt32(responce.Rows[0][0]);
                _farms.Add(newFarm);
            }
        }

        private static void LoadFarms()
        {
            var responce = MySQL.QueryRead("SELECT * FROM `weedfarm`");
            foreach (DataRow row in responce.Rows)
            {
                var farm = new WeedFarmInstance(
                    JsonConvert.DeserializeObject<Vector3>(row["enterPosition"].ToString()),
                    JsonConvert.DeserializeObject<Vector3>(row["vehPosition"].ToString()),
                    JsonConvert.DeserializeObject<Vector3>(row["vehRotation"].ToString())
                );
                farm.Id = Convert.ToInt32(row["id"]);
                farm.Owner = Fractions.Manager.GetFraction(Convert.ToInt32(row["ownerId"]));
                farm.IrrigationSystemHealth = Convert.ToInt32(row["irrigationSystem"]);
                farm.LightSystemHealth = Convert.ToInt32(row["lightSystem"]);
                farm.DryingSystemHealth = Convert.ToInt32(row["dyringSystem"]);
                farm.VentilationSystemHealth = Convert.ToInt32(row["ventilationSystem"]);
                farm.OnDrying = Convert.ToInt32(row["onDrying"]);
                farm.OnPacking = Convert.ToInt32(row["onPacking"]);
                farm.OnDelivery = Convert.ToInt32(row["onDelivery"]);
                farm.Components = JsonConvert.DeserializeObject<List<int>>(row["components"].ToString());
                farm.OccupationDate = Convert.ToDateTime(row["occupationDate"].ToString());
                var places = new List<int>();
                foreach (var place in PlacePoints)
                {
                    places.Add(0);
                }
                farm.Places = places;
                _farms.Add(farm);
            }           
        }

        private static void LoadFarmEnterance()
        {
            foreach (var farm in _farms)
            {
                InteractShape.Create(farm.EnterPoint, 2, 3)
                .AddDefaultMarker()
                .AddEnterPredicate((csh, player) => !player.IsInVehicle && farm.Owner != null && player.GetFraction() == farm.Owner)
                .AddInteraction((player) => {
                    player.Dimension = farm.Dimension;
                    player.ChangePosition(_exitPoint);
                    farm.Subscribe(player);
                }, "interact_48");
                farm.Blip = NAPI.Blip.CreateBlip(140, farm.EnterPoint, 1.2f, 69, $"Weed farm {farm.Id}", 255, 0, true, 0, 0);
            }

            InteractShape.Create(_exitPoint, 2, 3, uint.MaxValue)
               .AddDefaultMarker()
               .AddInteraction((player) => {
                   var farm = player.CurrentFarm();
                   if (farm == null)
                   {
                       _logger.WriteWarning("Bad dimension in weed farm");
                       player.ChangePosition(new Vector3(0,0,73));
                       return;
                   }
                   farm.Unsubscribe(player);
                   player.Dimension = 0;
                   player.ChangePosition(farm.EnterPoint);
                  
                   if (player.HasData("weedfarm:comp:path"))
                   {
                       WhistlerTask.Run(() =>
                       {
                           Vector3 pos = player.GetData<Vector3>("weedfarm:comp:path");
                           player.TriggerEvent("phone::gps::setWaypoint", pos.X, pos.Y);
                       }, 500);                      
                   } else if (player.HasData("weedfarm:delivery:point"))
                   {
                       WhistlerTask.Run(() =>
                       {
                           Vector3 nextPoint = player.GetData<Vector3>("weedfarm:delivery:point");
                           player.TriggerEvent("weedfarm:delivery:next", nextPoint.X, nextPoint.Y, nextPoint.Z);
                       }, 500);
                   }
                   player.TriggerEvent("weedfarm:instance:reset");
               }, "interact_49");

            InteractShape.Create(_menuPoint, 2, 3, uint.MaxValue)
               .AddDefaultMarker()
               .AddEnterPredicate((csh, player)=> player.CurrentFarm() != null)
               .AddInteraction((player) => { 
                    player.TriggerEvent("weedfarm:menu:open", player.CurrentFarm().OnDelivery);
               }, "interact_50");

            InteractShape.Create(_stockPoint, 1.5f, 3, uint.MaxValue)
              .AddInteraction((player) => {
                  var farm = player.CurrentFarm();
                  if (farm == null) return;
                  InventoryService.OpenStock(player, farm.Inventory.Id, StockTypes.Default);
              }, "interact_54");
        }

        private static void LoadFarmVehicles()
        {
            foreach (var farm in _farms)
            {
                farm.FarmVehicle = VehicleManager.CreateTemporaryVehicle(VehicleHash.Sandking2, farm.VehiclePosition, farm.VehicleRotation, VehicleManager.GenerateNumber(), VehicleAccess.WeedFarm).GetVehicleGo();
                farm.FarmVehicle.Data.OwnerID = farm.Id;
                var pos = farm.VehiclePosition + new Vector3(0, 0, -1);
                InteractShape.Create(pos, 2, 3)
                   .AddMarker(27, pos, 2, InteractShape.DefaultMarkerColor)
                   .AddEnterPredicate((csh, player) => player.IsInVehicle && (player.Vehicle.GetVehicleGo().Data as TemporaryVehicle)?.Access == VehicleAccess.WeedFarm && player.Vehicle.HasData("weedFarmComp"))
                   .AddInteraction((player) => {
                       if (!player.Vehicle.HasData("weedFarmComp")) return;
                       if (farm.Owner == null || player.GetFraction() != farm.Owner)
                       {
                           player.SendError("weedfarm:comp:err");
                           return;
                       }
                       int component = player.Vehicle.GetData<int>("weedFarmComp");
                       farm.AddComponent(component);
                       player.SendSuccess("weedfarm:comp:ok");
                       player.Vehicle.ResetData("weedFarmComp");
                   }, "interact_52");
            }                
        }

        private static void LoadComponentToVehicle(Player player)
        {
            var veh = player.Vehicle;
            player.ResetData("weedfarm:comp:path");
            if(veh.HasData("weedFarmComp"))
            {
                player.SendError("weedfarm:comp:already");
                return;
            }
            player.TriggerEvent("weedfarm:veh:load", ComponentsLoadTime);
            var component = 0;
            float lastDist = 100;
            foreach (var c in _componentTraders)
            {
                var dist = player.Position.DistanceTo(c.Value);
                if(lastDist > dist)
                {
                    lastDist = dist;
                    component = c.Key;
                }
            }
            WhistlerTask.Run(() => {
                veh.SetData("weedFarmComp", component);
            }, ComponentsLoadTime);
        }

        internal static void SetNewFarmOwner(Player player, int farmId, int fracId)
        {
            var farm = _farms.FirstOrDefault(f => f.Id == farmId);
            if (farm == null)
            {
                player.SendError("weedfarm:owner:err:1".Translate(farmId));
                return;
            }
            var fraction = Fractions.Manager.GetFraction(fracId);
            if(fraction == null || fraction.OrgActiveType != OrgActivityType.Crime)
            {
                player.SendError("weedfarm:owner:err:2");
                return;
            }
            farm.SetOwner(fraction);
            player.SendSuccess("weedfarm:owner:ok");
        }

        internal static WeedFarmInstance FarmByFraction(Fraction fraction)
        {
            if (fraction == null) return null;
            return _farms.FirstOrDefault(f => f.Owner == fraction);
        }

        public static bool HasVehicleKey(Player player, int ownerId)
        {
            var farm = _farms.FirstOrDefault(f => f.Id == ownerId);
            if (farm == null || farm.Owner == null) return false;
            return farm.Owner == player.GetFraction();            
        }

        private static void PlacePointAction(PlayerGo player, int palceIndex)
        {
            if (player.HasSharedData("scene:current") && player.GetSharedData<int>("scene:current") != (int)SceneNames.NoAction) return;
            var farm = player.CurrentFarm();
            if (!AllComponentsCollected(farm))
            {
                player.SendError("weedfarm:comp:miss");
                return;
            }
            switch (farm.Places[palceIndex])
            {
                case 3:
                    if (farm.OnDrying + PlanstOnSpot <= PlacePoints.Count * PlanstOnSpot)
                    {
                        player.SetData("weed:place:index", palceIndex);
                        SceneManager.StartScene(player, SceneNames.WeedGrow);
                    }
                    else player.SendError("weedfarm:dry:err");
                    break;
                case 0:
                    if(DateTime.Now.AddHours(1).Hour == AutoRestart.RestartInHour)
                    {
                        player.SendError("weedfarm:grow:restart");
                        return;
                    }
                    player.SetData("weed:place:index", palceIndex);
                    SceneManager.StartScene(player, SceneNames.WeedSeed);
                    break;
                default:
                    player.SendInfo("weedfarm:grow:wait");
                    break;
            }
        }

        internal static void DisconnectHandle(Player player)
        {
            foreach (var farm in _farms)
            {
                farm.Unsubscribe(player);
            }
        }

        private static WeedFarmInstance CurrentFarm(this Player player)
        {
            return _farms.FirstOrDefault(f => f.Dimension == player.Dimension);
        }

        private static bool AllComponentsCollected(WeedFarmInstance farm)
        {
            return farm?.Components.Count >= _componentTraders.Count;
        }

        internal static void LoadFarmOnStart(Player player)
        {
            var fraction = player.GetFraction();
            if (fraction?.OrgActiveType != OrgActivityType.Crime)
                player.ChangePosition(new Vector3(0, 0, 73));
            else
            {
                var farm = _farms.FirstOrDefault(f => f.Owner == fraction);
                if (farm == null)
                    player.ChangePosition(new Vector3(0, 0, 73));
                else
                    player.ChangePosition(farm.EnterPoint);
            }
            player.Dimension = 0;
        }

        internal static bool PlaceSeedAction(Player player)
        {
            if (player.HasData("weed:place:index"))
            {
                player.CurrentFarm().PlantSeed(player.GetData<int>("weed:place:index"));
                player.SendSuccess("weedfarm:grow:start");
                player.ResetData("weed:place:index");
                return true;
            }
            else return false;
        }

        internal static bool PlaceGrowAction(Player player)
        {
            if (player.HasData("weed:place:index"))
            {
                var farm = player.CurrentFarm();
                farm.MoveToDrying();
                farm.UpdatePlace(player.GetData<int>("weed:place:index"), 0);
                player.SendSuccess("weedfarm:grow:ok");
                return true;
            }
            else return false;
        }

        internal static bool DeliveryJobAction(Player player)
        {
            var inventory = player.GetInventory();
            if (inventory == null) return false;
            var item = inventory.SubItemByName(ItemNames.Marijuana, 1, LogAction.Delete);
            if (item == null)
            {
                player.ResetData("weedfarm:delivery:point");
                player.TriggerEvent("weedfarm:delivery:cancel");
                return false;
            }
            var sum = DeliveryJobTariff / 2;
            Wallet.MoneyAdd(player.GetCharacter(), sum, "weedDelivery");
            Wallet.MoneyAdd(player.GetFraction(), sum, "weedDelivery");
            if (inventory.HasItem(ItemNames.Marijuana))
            {
                var nextPoint = _deliveryPoints.Next(new Vector3());
                player.SetData("weedfarm:delivery:point", nextPoint); 
                player.TriggerEvent("weedfarm:delivery:next", nextPoint.X, nextPoint.Y, nextPoint.Z);
            }
            else
            {
                player.ResetData("weedfarm:delivery:point");
                player.SendError("weedfarm:delivery:miss");
                player.TriggerEvent("weedfarm:delivery:cancel");
            }
            return true;
        }

        internal static void ActionWeedDeliveryJob(PlayerGo player)
        {
            if (!player.HasData("weedfarm:delivery:point")) return;
            Vector3 point = player.GetData<Vector3>("weedfarm:delivery:point");
            if (point.DistanceTo(player.Position) > 5) return;
            SceneManager.StartScene(player, SceneNames.WeedDelivery);
        }

        internal static void BegineWeedDeliveryJob(Player player)
        {
            if (player.HasData("weedfarm:delivery:point")) return;
            var inventory = player.GetInventory();
            if (inventory == null || !inventory.HasItem(ItemNames.Marijuana))
            {
                player.ResetData("weedfarm:delivery:point");
                player.SendError("weedfarm:delivery:miss");
                return;
            }
            var nextPoint = _deliveryPoints.Next(new Vector3());
            player.SetData("weedfarm:delivery:point", nextPoint);
            player.SendSuccess("weedfarm:delivery:get");
            //player.TriggerEvent("weedfarm:delivery:next", nextPoint.X, nextPoint.Y, nextPoint.Z);
        }


        internal static void EndWeedDeliveryJob(Player player)
        {
            player.ResetData("weedfarm:delivery:point");
            player.SendSuccess("weedfarm:delivery:end");
            player.TriggerEvent("weedfarm:delivery:cancel");
        }

        internal static void CancelSortJob(Player player)
        {
            player.CurrentFarm()?.ReleasePackPoint(player);
        }
    }
}
