using GTANetworkAPI;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whistler.Helpers;
using Whistler.Entities;
using Whistler.GUI;

namespace Whistler.Jobs.Transporteur
{
    class Work : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Work));

        #region Settings

        public enum QuestStage { ORDER_SEARCH, GET_HELICOPTER, FLY_TO_CAR, DELIVER };
        public static List<Pilot> Workers { get; set; } = new List<Pilot>();
        public static List<Cargobob> Cargobobs { get; set; } = new List<Cargobob>();
        public static List<DeliverCar> ClientCars { get; set; } = new List<DeliverCar>();

        #region Locations
        public static Vector3 StartEndWorkPoint { get; set; } = new Vector3(-1189.708, -2933.827, 12.82469);

        public static List<SpawnPoint> CargobobSpawns { get; set; } = new List<SpawnPoint>()
        {
            new SpawnPoint( new Vector3(-1165.902, -3010.778, 14.58498), 150.3264f),
            new SpawnPoint( new Vector3(-1156.927, -3016.018, 14.585), 149.6591f),
            new SpawnPoint( new Vector3(-1147.823, -3021.508, 14.5852), 146.5089f),
            new SpawnPoint( new Vector3(-1138.624, -3026.785, 14.58545), 147.1575f),
            new SpawnPoint( new Vector3(-1129.299, -3032.115, 14.5852), 148.8802f),
            new SpawnPoint( new Vector3(-1119.873, -3037.5, 14.58542), 151.3211f),
            new SpawnPoint( new Vector3(-1110.615, -3042.767, 14.586), 151.6293f),
            new SpawnPoint( new Vector3(-1101.389, -3047.967, 14.5854), 148.0658f),
            new SpawnPoint( new Vector3(-1092.528, -3053.107, 14.58505), 148.1458f),
            new SpawnPoint( new Vector3(-1083.34, -3058.364, 14.58498), 148.2083f),
            new SpawnPoint( new Vector3(-1073.921, -3063.782, 14.58489), 147.8796f),
            new SpawnPoint( new Vector3(-1064.841, -3068.592, 14.5843), 147.501f),
            new SpawnPoint( new Vector3(-1218.056, -2980.611, 14.58515), 152.9517f),
            new SpawnPoint( new Vector3(-1227.482, -2975.486, 14.58564), 149.2663f),
            new SpawnPoint( new Vector3(-1236.523, -2970.76, 14.58593), 152.4522f),
            new SpawnPoint( new Vector3(-1246.476, -2965.039, 14.58632), 150.1542f),
            new SpawnPoint( new Vector3(-1255.583, -2960.104, 14.58654), 149.5798f),
            new SpawnPoint( new Vector3(-1264.956, -2954.913, 14.58536), 151.5419f),
            new SpawnPoint( new Vector3(-1273.858, -2949.75, 14.58599), 150.928f),
            new SpawnPoint( new Vector3(-1283.743, -2944.138, 14.58524), 150.1527f),
            new SpawnPoint( new Vector3(-1293.075, -2938.992, 14.58329), 149.7617f),
            new SpawnPoint( new Vector3(-1301.881, -2934.142, 14.58062), 152.0557f),
            new SpawnPoint( new Vector3(-1311.441, -2928.872, 14.57965), 151.2318f),
            new SpawnPoint( new Vector3(-1321.418, -2923.333, 14.57575), 147.8932f),

        };

        public static List<SpawnPoint> VehSpawns { get; set; } = new List<SpawnPoint>()
        {
            //new SpawnPoint(new Vector3(-526.5804, -2822.813, 4.880387),244.6886f),
            //new SpawnPoint(new Vector3(-405.4005, -2723.847, 4.880215),299.1496f),
            //new SpawnPoint(new Vector3(-246.436, -2493.387, 4.880637),187.2559f),
            //new SpawnPoint(new Vector3(1149.563, -3287.811, 4.780806),26.76968f),
            //new SpawnPoint(new Vector3(1209.755, -2990.498, 4.745362),357.6398f),
            //new SpawnPoint(new Vector3(1036.891, -2926.955, 4.780824),269.1537f),
            //new SpawnPoint(new Vector3(1153.669, -3088.867, 4.645979),0.6162059f),
            //new SpawnPoint(new Vector3(1021.748, -3186.968, 4.781036),1.057517f),
            //new SpawnPoint(new Vector3(1003.046, -3209.262, 4.78159),87.77519f),
            //new SpawnPoint(new Vector3(929.2101, -3210.028, 4.780663),88.35396f),
            //new SpawnPoint(new Vector3(908.8992, -3129.875, 4.780799),1.099453f),
            //new SpawnPoint(new Vector3(840.762, -3134.964, 4.780799),92.98142f),
            //new SpawnPoint(new Vector3(895.8009, -2918.296, 4.780585),88.61566f),
            //new SpawnPoint(new Vector3(-1204.768, -2610.796, 12.82494),146.5466f),
            //new SpawnPoint(new Vector3(-1113.514, -2411.974, 12.82514),328.3029f),
            //new SpawnPoint(new Vector3(-1188.276, -2372.824, 12.82515),240.9263f),
            //new SpawnPoint(new Vector3(-410.3928, -2473.222, 4.880782),226.5334f),
            //new SpawnPoint(new Vector3(2776.404, 2808.911, 40.37247),117.3862f),
            //new SpawnPoint(new Vector3(2681.164, 2805.659, 39.33611),182.4851f),
            //new SpawnPoint(new Vector3(2856.807, 4409.35, 47.8721),19.88842f),
            //new SpawnPoint(new Vector3(2946.38, 4225.909, 51.4885),293.967f),
            //new SpawnPoint(new Vector3(2268.97, 4924.616, 39.86076),222.2013f),
            //new SpawnPoint(new Vector3(2270.263, 4847.052, 39.4696),218.3282f),
            //new SpawnPoint(new Vector3(2159.13, 5038.865, 41.98524),331.4806f),
            //new SpawnPoint(new Vector3(2205.394, 4998.834, 41.62238),311.7711f),
            new SpawnPoint(new Vector3(904.6246, -3209.089, 5.873769), 2.782471f),
            new SpawnPoint(new Vector3(904.4077, -3185.473, 5.872877), 358.4086f),
            new SpawnPoint(new Vector3(904.2301, -3155.575, 5.875382), 1.568909f),
            new SpawnPoint(new Vector3(904.3851, -3130.967, 5.875012), 359.675f),
            new SpawnPoint(new Vector3(924.9393, -3130.489, 5.873738), 182.8769f),
            new SpawnPoint(new Vector3(925.1403, -3153.884, 5.873841), 179.267f),
            new SpawnPoint(new Vector3(924.9227, -3184.137, 5.873994), 180.7373f),
            new SpawnPoint(new Vector3(925.0852, -3209.594, 5.874142), 180.3216f),
            new SpawnPoint(new Vector3(945.3864, -3209.712, 5.873995), 359.5644f),
            new SpawnPoint(new Vector3(945.1021, -3186.858, 5.874282), 359.9017f),
            new SpawnPoint(new Vector3(945.1508, -3155.254, 5.874382), 359.9089f),
            new SpawnPoint(new Vector3(945.2208, -3128.535, 5.875134), 358.4247f),
            new SpawnPoint(new Vector3(965.6464, -3129.674, 5.874732), 178.8053f),
            new SpawnPoint(new Vector3(965.4783, -3153.631, 5.874691), 179.2307f),
            new SpawnPoint(new Vector3(965.7122, -3184.96, 5.874586), 178.1032f),
            new SpawnPoint(new Vector3(965.5405, -3210.097, 5.874513), 179.8859f),
            new SpawnPoint(new Vector3(993.0057, -3208.706, 5.875165), 359.7449f),
            new SpawnPoint(new Vector3(993.1216, -3183.587, 5.873799), 358.3342f),
            new SpawnPoint(new Vector3(1013.651, -3185.46, 5.87452), 179.638f),
            new SpawnPoint(new Vector3(1013.708, -3209.811, 5.852673), 180.1244f),
            new SpawnPoint(new Vector3(1033.986, -3208.99, 5.852996), 0.7671814f),
            new SpawnPoint(new Vector3(1033.874, -3184.826, 5.874685), 358.2582f),
            new SpawnPoint(new Vector3(1058.254, -3184.192, 5.875121), 181.8994f),
            new SpawnPoint(new Vector3(1058.354, -3209.144, 5.870398), 179.7f),
            new SpawnPoint(new Vector3(1123.331, -3159.124, 5.874468), 179.9675f),
            new SpawnPoint(new Vector3(1112.201, -3146.183, 5.875129), 1.13623f),
            new SpawnPoint(new Vector3(1122.774, -3125.971, 5.874695), 358.7864f),
            new SpawnPoint(new Vector3(1101.557, -3126.327, 5.874747), 358.782f),
            new SpawnPoint(new Vector3(1101.854, -3159.325, 5.873879), 180.4419f),
            new SpawnPoint(new Vector3(356.8214, -2459.152, 6.376312), 333.4238f),
            new SpawnPoint(new Vector3(511.0277, -2155.662, 5.92168), 170.5549f),
            new SpawnPoint(new Vector3(574.2397, -2343.484, 5.869985), 170.8512f),
            new SpawnPoint(new Vector3(556.5952, -2305.242, 5.898305), 243.1682f),
            new SpawnPoint(new Vector3(2754.464, 1331.592, 24.49825), 181.5936f),
            new SpawnPoint(new Vector3(2732.494, 1331.573, 24.49543), 178.3389f),
            new SpawnPoint(new Vector3(2743.425, 1346.732, 24.49698), 179.5132f),
            new SpawnPoint(new Vector3(2721.281, 1346.157, 24.49741), 359.0288f),
            new SpawnPoint(new Vector3(2706.971, 1330.87, 24.49651), 177.4498f),
            new SpawnPoint(new Vector3(2692.244, 1330.081, 24.49368), 178.1284f),
            new SpawnPoint(new Vector3(2676.196, 1340.251, 24.48544), 93.16928f),
            new SpawnPoint(new Vector3(2676.328, 1354.682, 24.49892), 85.48618f),
            new SpawnPoint(new Vector3(2675.581, 1369.497, 24.50527), 89.64423f),
            new SpawnPoint(new Vector3(2703.069, 1391.801, 24.50382), 355.2124f),
            new SpawnPoint(new Vector3(2717.839, 1391.524, 24.50704), 3.531921f),
            new SpawnPoint(new Vector3(2743.205, 1392.238, 24.50023), 351.4447f),
            new SpawnPoint(new Vector3(2673.252, 1719.065, 24.46246), 272.8564f),
            new SpawnPoint(new Vector3(2660.01, 1707.631, 24.46192), 272.2901f),
            new SpawnPoint(new Vector3(2673.229, 1693.426, 24.46209), 271.8378f),
            new SpawnPoint(new Vector3(2660.081, 1682.359, 24.46163), 268.3739f),
            new SpawnPoint(new Vector3(2673.301, 1660.475, 24.46276), 269.9388f),
        };

        public static List<Vector3> DeliverPoints = new List<Vector3>()
        {
            new Vector3(2651.802, 3273.543, 54.12591),
            new Vector3(2553.488, 341.4255, 107.342),
            new Vector3(533.4471, 2657.292, 41.16639),
            new Vector3(600.68, 2803.324, 40.79782),
            new Vector3(-1149.757, 2678.091, 16.97389),
            new Vector3(168.8267, 2755.466, 42.28166),
            new Vector3(-446.8608, 1586.302, 357.728),
            new Vector3(889.9977, 3645.769, 31.70572),
            new Vector3(1694.353, 4806.056, 40.73181),
            new Vector3(1292.395, 4331.15, 37.354)
        };


        #endregion

        #region Car Models

        public static List<string> CarModels { get; set; } = new List<string>()
        {
            "blista3",
            "prairie",
            "asterope",
            "issi2",
            "futo",
        };

        #endregion


        public static int WorkTimer { get; set; } = 5;
        public static int MinLVL { get; set; } = 10;
        public static int WorkID { get; set; } = 17;
        public static string JobName { get; set; } = "TRANSPORTEUR";
        public static WorkFactory WorkFactory { get; set; }
        public static uint BlipID { get; set; } = 481;
        public static byte BlipColor { get; set; } = 50;
        public static string HelicopterName { get; set; } = "cargobob2";
        public static string CarNumberDictionary { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static int CarNumberLength { get; set; } = 6;
        public static int Salary { get; set; } = 5000;

        #endregion

        #region Events

        [ServerEvent(Event.ResourceStart)]
        public void onResourceStartHandler()
        {
            try
            {
                // Создаем фабрику объектов работы, инициализируем метку и кулшейп начала работы 
                WorkFactory = new WorkFactory(MinLVL, WorkID, WorkTimer, JobName, StartEndWorkPoint, BlipID, BlipColor, InitWorkDay);

                //#region Change Numer Points

                foreach (Vector3 deliverPoint in DeliverPoints)
                {
                    var position = deliverPoint;
                    var colsh = NAPI.ColShape.CreatCircleColShape(position.X, position.Y, 9f);

                    colsh.OnEntityEnterColShape += (colshape, client) =>
                    {
                        try
                        {
                            if (!(client is ExtPlayer player)) return;

                            // Если игрок не в машине
                            if (!player.IsInVehicle) return;
                            // Если это не рабочий - прерываем событие
                            if (!(Workers.FirstOrDefault(e => e.Player == player) is Pilot worker)) return;
                            // Если рабочий не находится на стадии доставки транспорта - прерываем
                            if (!worker.JobStage.Equals(QuestStage.DELIVER)) return;

                            // Если рабочему дали другую точку - прерываем [MK]
                            if (worker.DeliverPoint != position) return;

                            // Посылаем событие на клиент для проверки!
                            SafeTrigger.ClientEvent(player,"WORK::TRANSPORTEUR::DELIVER::VEHICLE::SERVER");
                        }
                        catch (Exception e) { NAPI.Util.ConsoleOutput("shape.OnEntityEnterColshape: " + e.ToString()); }
                    };
                }
            }
            catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public static void onPlayerDisconnected(ExtPlayer player, DisconnectionType type, string reason)
        {
            try
            {
                // Если это не рабочий - прерываем событие
                if (!(Workers.FirstOrDefault(e => e.Player == player) is Pilot worker)) return;
                if (worker != null)
                {
                    //Work.Workers.Remove(worker);

                    worker.Dispose();

                    //// Уничтожаем машину (все проверки на клиенте)
                    //worker.SafeTrigger.ClientEvent(player,"WORK::TRANSPORTEUR::DELETE::VEHICLE::SERVER");

                    //// Уничтожаем Cargobob
                    //if (worker.Cargobob != null)
                    //{
                    //    worker.Cargobob.Vehicle.CustomDelete();
                    //    // Освобождаем точку для спауна другими игроками
                    //    worker.Cargobob.CarLocation.IsOccupied = false;
                    //    Work.Cargobobs.Remove(worker.Cargobob);
                    //    worker.Cargobob = null;
                    //}
                }

            }

            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Work.onPlayerDisconnected(pilot): {ex.ToString()}");
            }

        }

        [RemoteEvent("WORK::TRANSPORTEUR::DELIVER::END::CLIENT")]
        public static void PilotEndDeliver(ExtPlayer player)
        {
            try
            {
                // Если такого игрока нет на сервере - прерываем
                if (!player.IsLogged()) return;

                // Если он не пилот - прерываем
                if (!(Workers.FirstOrDefault(e => e.Player == player) is Pilot pilot)) return;

                // Проверяем, что пилот находится в транспорте
                if (!player.IsInVehicle) return;

                // Проверяем, что самолет, в котором находится полицейский привязан к нему
                if (pilot.Cargobob.Vehicle != player.Vehicle) return;

                // Если рабочий не находится на стадии доставки транспорта - прерываем
                if (!pilot.JobStage.Equals(QuestStage.DELIVER)) return;

                /*
                 * Если мы дошли до этого места - значит пилот:
                 * 1. На правильном вертолете
                 * 2. К его вертолету прикреплена машина, котороую он должен сдать
                 * 3. Он прилетел в радиус кулшейпа сдачи транспорта
                */
                // Останавливаем таймер и оканчиваем квест
                pilot.StopTimer();
                pilot.EndWorkQuest();
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Work.PilotEndDeliver(): {ex.ToString()}");
            }
        }

        [RemoteEvent("WORK::TRANSPORTEUR::DELIVER::START::CLIENT")]
        public static void PilotStartDeliver(ExtPlayer player)
        {
            try
            {
                // Если такого игрока нет на сервере - прерываем
                if (!player.IsLogged()) return;

                // Если он не пилот - прерываем
                if (!(Workers.FirstOrDefault(e => e.Player == player) is Pilot pilot)) return;

                // Проверяем, что пилот находится в транспорте
                if (!player.IsInVehicle) return;

                // Проверяем, что вертолет, в котором привязан пилот, привязан к нему
                if (pilot.Cargobob.Vehicle != player.Vehicle) return;

                /*
                 * Если мы дошли до этого места - значит пилот:
                 * 1. На правильном вертолете
                 * 2. К его вертолету прикреплена машина, котороую он должен сдать (уже проверено на клиенте)
                 * 3. Он прилетел в радиус машины.
                 Показываем ему точку сдачи машины
                */
                pilot.GiveDeliverPoint();
                // Освобождаем точку спауна транспорта для перевозки
                pilot.DeliverCar.CarLocation.IsOccupied = false;
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Work.PilotStartDeliver(): {ex.ToString()}");
            }
        }

        [RemoteEvent("WORK::TRANSPORTEUR::ENGINE::START::CLIENT")]
        public static void PilotStartEngine(ExtPlayer player)
        {
            try
            {
                // Если такого игрока нет на сервере - прерываем
                if (!player.IsLogged()) return;

                // Если он не пилот - прерываем
                if (!(Workers.FirstOrDefault(e => e.Player == player) is Pilot pilot)) return;

                // Проверяем, что пилот находится в транспорте
                if (!player.IsInVehicle) return;

                // Проверяем, что самолет, в котором находится пилот привязан к нему
                if (pilot.Cargobob.Vehicle != player.Vehicle) return;

                // Заводим и глушим двигатель
                player.Vehicle.EngineStatus = !player.Vehicle.EngineStatus;

                // Если двигатель уже был запущен первый раз - прерываем
                if (pilot.IsEngineAlreadyStarted) return;
                // Устанавливаем переменную первого запуска двигателя, чтобы код ниже больше не выполнялся
                pilot.IsEngineAlreadyStarted = true;

                // Устанавливаем стадию поиска заказов
                pilot.JobStage = QuestStage.ORDER_SEARCH;

                // Включаем магнит через 5 секунд после первого запуска двигателя
                SafeTrigger.ClientEvent(pilot.Player,"WORK::TRANSPORTEUR::MAGNET::VEHICLE::SERVER");

                // Выключаем таймер
                pilot.StopTimer();

                // Выдаем машину и указываем путь до нее
                //await Task.Delay(5000);

                // Пытаемся освободить точку сдачи вертолета
                pilot.FreeCargobobSpawnAsync();

                // Пытаемся выдать пилоту новую машину, каждые 5 секунд
                pilot.GiveCarAsync();

                // Освобождаем место для спауна другого вертолета через 10 секунд после успешной выдачи точки с автомобилем
                //NAPI.Task.Run(() =>
                //{
                //  // До тех пор, пока занята точка спауна вертолета
                //  while (pilot.Cargobob.CarLocation.IsOccupied)
                //  {
                //      if (pilot.Cargobob != null)
                //      {
                //          // Если игрок отлетел на вертолете более чем на 20 метров - прерываем
                //          if (pilot.Cargobob.Vehicle.AllOccupants.Contains(pilot.Player) && pilot.Player.Position.DistanceTo(pilot.Cargobob.CarLocation.Position) > 20)
                //              pilot.Cargobob.CarLocation.IsOccupied = false;
                //      }
                //  }
                //}, 10);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Work.PolicemanStartEngine(): {ex.ToString()}");
            }
        }

        #endregion

        #region Methods

        public static void InitWorkDay(ExtPlayer client)
        {
            try
            {
                // Устраиваемся на работу
                if (!Workers.Exists(e => e.Player == client))
                {
                    // Пытаемся создать пилота с сохранить его в список Workers
                    if (!(WorkFactory.CreateWorker(client) is Pilot worker)) return;
                    // Создаем вертолет 
                    if (!(WorkFactory.CreateVehicle(worker) is Cargobob cargobob)) return;
                    // Устанавливаем WorkID игроку, т.к. его вертолет успешно создался
                    client.Character.WorkID = Work.WorkID;
                    MainMenu.SendStats(client);
                    // Добавляем пилота в список рабочих
                    Work.Workers.Add(worker);
                    // Привязываем его к пилоту
                    worker.Cargobob = cargobob;
                    // Создаем метку на карте и путь к ней
                    worker.CreateCheckpoint(worker.Cargobob.Vehicle.Position);
                    worker.CreateWaypoint(worker.Cargobob.Vehicle.Position);
                    // Меняем переменную на клиенте, чтобы игрок мог завести вертолет
                    SafeTrigger.ClientEvent(worker.Player,"WORK::TRANSPORTEUR::CHANGE::STATE::SERVER", "isTransporteurWorker", true);
                    // Вы начали рабочий день пилота, садитесь в вертолет и запускайте двигатель <2>
                    worker.SendMessage("Transporteur_1".Translate(), 8000);
                    // Меняем стадию квеста
                    worker.JobStage = QuestStage.GET_HELICOPTER;

                    #region Timer to start engine

                    // Запускаем таймер, 3 минуты
                    int minutes = 3;
                    // Останавливаем существующий таймер, если он был запущен
                    worker.StopTimer();
                    // Запускаем таймер на клиенте
                    worker.StartClientTimer(minutes);
                    // Запускаем таймер на сервере
                    worker.TimerID = Timers.StartOnce(minutes * 60 * 1000, () => NAPI.Task.Run(() => worker.CheckTimer(QuestStage.GET_HELICOPTER)));
                    //worker.StartTimer(3, QuestStage.GET_HELICOPTER);

                    #endregion
                }
                // Оканчиваем работу
                else
                {
                    Pilot worker = Workers.Find(e => e.Player == client);
                    worker.Player.Character.WorkID = 0;
                    MainMenu.SendStats(client);
                    worker.Dispose();
                    // Вы закончили рабочий день пилота
                    worker.SendMessage("Transporteur_2".Translate(), 5000);
                }
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Work.InitWorkDay(ExtPlayer pilot): {ex.ToString()}");
            }
        }

        #endregion

        #region Commands

        [Command("tranworkers")]
        public static void CMD_GetLSPDWorkers(ExtPlayer player)
        {
            if (player.Character.AdminLVL == 0) return;
            try
            {
                string message = "";
                Work.Workers.ForEach(e => message += e);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, message, 15000);
            }
            catch (Exception ex)
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, ex.ToString(), 10000);
            }
        }

        [Command("workflies")]
        public static void CMD_GetCars(ExtPlayer player)
        {
            if (player.Character.AdminLVL == 0) return;
            try
            {
                string message = "";
                Work.Cargobobs.ForEach(e => message += e);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, message, 15000);
            }
            catch (Exception ex)
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, ex.ToString(), 10000);
            }
        }

        #endregion
    }
}
