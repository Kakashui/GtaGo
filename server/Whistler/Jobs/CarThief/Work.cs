using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Helpers;
using Whistler.Fractions;
using Whistler.Common;
using Whistler.Entities;

namespace Whistler.Jobs.CarThief
{
    class Work : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Work));
        public enum QuestStage { ORDER_SEARCH, AGENT_SEARCH, CAR_SEARCH, PICKLOCK, CAR_NUMER_CHANGE, CAR_DELIVERY };
        public static Vector3 StartEndWorkPoint { get; set; } = new Vector3(1417.378, 6343.877, 24.00321);
        public static int MinLVL { get; set; } = 0;
        public static int WorkID { get; set; } = 16;
        public static int Salary { get; set; } = 4000;
        public static int MapZoneRadius { get; set; } = 100;
        public static int MapZoneColor { get; set; } = 1; // RED
        public static double PriceByDistance { get; set; } = 0.4;
        public static Vector3 PoliceDeliverCarPoint { get; set; } = new Vector3(467.0472, -1154.857, 28.17177);
        public static int PolicemanSalary { get; set; } = 10000;
        public static Dictionary<string, int> AgentData { get; set; } = new Dictionary<string, int>()
        {
            // Male
            ["Daniel"] = 71501447,
            ["William"] = -459818001,
            ["Henry"] = -396800478,
            // Female
            ["Madison"] = 1943971979,
            ["Hannah"] = -44746786
        };
        public static List<string> CarModels { get; set; } = new List<string>()
        {
            // Compacts
            "brioso", 
            // Coupes
            "windsor", 
            // Motorcycles
            "bati",
            "hakuchou",
            "hexer",
            // Muscle
            "tampa",
            "dominator",
            "buccaneer2",
            // Off-Road
            "caracara2",
            "brawler",
            // Sports Classic
            "casco",
            "btype",
            "peyote"
        };
        public static List<Color> CarColors { get; set; } = new List<Color>()
        {
            new Color(178, 34, 34),
            new Color(0, 255, 127),
            new Color(0, 0, 128),
            new Color(255, 255, 255),
            new Color(75, 0, 130)
        };
        public static List<Thief> Workers { get; set; } = new List<Thief>();
        public static List<Policeman> Policemen { get; set; } = new List<Policeman>();
        public static List<Car> Cars { get; set; } = new List<Car>();
        public static int WorkGetTimerMinutes { get; set; } = 5;

        /// <summary>
        /// PlayerDisconnected
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        /// <param name="reason"></param>
        [ServerEvent(Event.PlayerDisconnected)]
        public static void onPlayerDisconnected(Player player, DisconnectionType type, string reason)
        {
            try
            {
                // Если это не рабочий - прерываем событие
                Thief worker = Workers.Find(e => e.Player == player);
                Policeman policeman = Policemen.Find(e => e.Player == player);
                if (worker != null)
                {
                    // Пробуем удалить машину
                    worker.DeleteCar();

                    // Удаляем его из списка рабочих
                    Workers.Remove(worker);

                    // Обнуляем ID
                    //player.GetCharacter().WorkID = 0;
                    worker.DeleteExistedMapZone();
                    worker.DeleteWaypoint();
                }
                else if (policeman != null)
                {
                    policeman.DeleteCar();
                    Policemen.Remove(policeman);
                    policeman.DeleteWaypointAndCheckPoint();
                }
                
            }

            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Work.onPlayerDisconnected: {ex.ToString()}");
            }

        }

        /// <summary>
        /// Init Base Resources
        /// </summary>
        [ServerEvent(Event.ResourceStart)]
        public void onResourceStartHandler()
        {
            try
            {
                NAPI.Blip.CreateBlip(488, StartEndWorkPoint, 1.2f, 21, "Угонщик авто", 255, 2, true);

                // Точка создается для всех - люди начинают/заканчивают рабочий день

                InteractShape.Create(StartEndWorkPoint, 1.5f, 2)
                    .AddInteraction(InitWorkDay);

                foreach (Vector3 numberPoint in CarThiefConfigs.NumberReplacements)
                {
                    InteractShape.Create(numberPoint, 1.5f, 2)
                    .AddInteraction((player) =>
                    {
                        if (!player.IsInVehicle) return;
                        // Если это не рабочий - прерываем событие
                        Thief worker = Workers.Find(e => e.Player == player);
                        if (worker == null || player.Vehicle != worker.Car.Vehicle || worker.NumberReplacement != numberPoint)
                            return;
                        worker.StartNumberChange();
                    })
                    .AddEnterPredicate((shape, player) =>
                    {
                        if (!player.IsInVehicle) return false;
                        Thief worker = Workers.Find(e => e.Player == player);
                        return worker != null && player.Vehicle == worker.Car.Vehicle && worker.NumberReplacement == numberPoint;
                    });
                }

                foreach (Vector3 garagePoint in CarThiefConfigs.Garages)
                {

                    InteractShape.Create(garagePoint, 1.5f, 2)
                        .AddInteraction((player) =>
                        {
                            if (!player.IsInVehicle) return;
                            // Если это не рабочий - прерываем событие
                            Thief worker = Workers.Find(e => e.Player == player);
                            if (worker == null || player.Vehicle != worker.Car.Vehicle || worker.Garage != garagePoint || 
                                !worker.CheckAccessNextAction() || worker.JobStage != QuestStage.CAR_DELIVERY)
                                return;
                            worker.EndWorkQuest();
                        });
                }

                InteractShape.Create(PoliceDeliverCarPoint, 1.5f, 2)
                        .AddInteraction((player) =>
                        {
                            Policeman policeman = Policemen.Find(e => e.Player == player);
                            if (policeman == null || policeman.JobStage != QuestStage.CAR_DELIVERY) return;
                            policeman.DeliverCar();
                        });

                // Инициализируем точки починки
                foreach (var agentPed in CarThiefConfigs.AgentPeds)
                {
                    InteractShape.Create(agentPed.Position, 1.5f, 2)
                        .AddInteraction((player) =>
                        {
                            if (player.IsInVehicle) return;
                            Thief worker = Work.Workers.Find(e => e?.Player == player);
                            // Если рабочий не найден
                            if (worker == null) return;
                            // Если он не относится к этой рабочей станции или находится не на стадии диагностики
                            if (worker.Agent != agentPed || worker.JobStage != QuestStage.AGENT_SEARCH || !worker.CheckAccessNextAction()) return;

                            worker.StartCarSearch();
                        })
                        .AddEnterPredicate((c, player) =>
                        {
                            if (player.IsInVehicle) return false;
                            Thief searchedWorker = Work.Workers.Find(e => e?.Player == player);
                            if (searchedWorker == null) return false;
                            if (searchedWorker.Agent != agentPed || searchedWorker.JobStage != QuestStage.AGENT_SEARCH) return false;
                            return true;
                        });
                }

            }
            catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
        }


        /// <summary>
        /// Start task form Player
        /// </summary>
        /// <param name="player"></param>
        /// <param name="keyPressed"></param>
        [RemoteEvent("WORK::CARTHIEF::GAME::START::CLIENT")]
        public static void CMD_StartGame(Player player, string keyPressed)
        {
            try
            {
                // Если это не рабочий - прерываем событие
                Thief worker = Workers.Find(e => e.Player == player);
                if (worker == null) return;

                if (worker.JobStage.Equals(QuestStage.CAR_SEARCH))
                {
                    if (!keyPressed.Equals("O")) return;
                    if (worker.Player.Position.DistanceTo(worker.Car.Vehicle.Position) < 2)
                    {
                        //worker.Car.Vehicle.Locked = false;
                        //Машина успешно взломана
                        worker.Player.TriggerEvent("WORK::CARTHIEF::CHANGE::STATE::SERVER", true);
                        worker.Player.TriggerEvent("WORK::CARTHIEF::DIAGNOSTIC::SERVER");
                    }
                    else
                    {
                        //Необходимо подойти ближе к машине
                        worker.SendMessage("AutoThief_2", 10000);
                    }
                }
                else if (worker.JobStage.Equals(QuestStage.CAR_NUMER_CHANGE) || worker.JobStage.Equals(QuestStage.CAR_DELIVERY))
                {
                    if (worker.Car.Vehicle.Occupants.Contains(worker.Player))
                    {
                        if (!keyPressed.Equals("I")) return;

                        // Заводим автомобиль, если он заглушен
                        if (!VehicleSystem.VehicleStreaming.GetEngineState(worker.Car.Vehicle))
                        {
                            worker.Player.TriggerEvent("WORK::CARTHIEF::CHANGE::STATE::SERVER", true);
                            worker.Player.TriggerEvent("WORK::CARTHIEF::DIAGNOSTIC::SERVER");
                        }
                        else
                        {
                            
                            //Автомобиль заглушен
                            VehicleSystem.VehicleStreaming.SetEngineState(worker.Car.Vehicle, false);
                            worker.SendMessage("AutoThief_3", 3000);
                        }

                    }
                    else
                    {
                        //Необходимо находиться в машине
                        worker.SendMessage("AutoThief_4", 10000);
                    }
                }


            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Work.CMD_PickLockCar({player}): {ex.ToString()}");
            }

        }

        /// <summary>
        /// Task result from VUE
        /// </summary>
        /// <param name="player"></param>
        /// <param name="isSuccess"></param>
        [RemoteEvent("WORK::CARTHIEF::GAME::RESULT::CLIENT")]
        public static void MiniGameFinished(Player player, bool isSuccess)
        {
            // Если такого игрока нет на сервере - прерываем
            if (!player.IsLogged()) return;

            // Если это не рабочий - прерываем событие
            Thief worker = Workers.Find(e => e.Player == player);
            if (worker == null) return;
            // Если результат неудачный
            if (!isSuccess)
            {
                //Попробуйте еще раз
                worker.SendMessage("AutoThief_5", 5000);
                return;
            }

            switch (worker.JobStage)
            {
                case QuestStage.CAR_SEARCH:
                    worker.EndCarSearch();
                    break;
                case QuestStage.CAR_NUMER_CHANGE:
                    if (!VehicleSystem.VehicleStreaming.GetEngineState(worker.Car.Vehicle)) worker.StartEngine();
                    if (!worker.Car.IsNumberChanged) worker.EndNumerChange();
                    break;
                case QuestStage.CAR_DELIVERY:
                    if (!VehicleSystem.VehicleStreaming.GetEngineState(worker.Car.Vehicle)) worker.StartEngine();
                    break;
            }
        }

        /// <summary>
        /// Policeman Start Car
        /// </summary>
        /// <param name="player"></param>
        [RemoteEvent("WORK::CARTHIEF::POLICEMAN::START::ENGINE")]
        public static void PolicemanStartEngine(PlayerGo player)
        {
            try
            {
                // Если такого игрока нет на сервере - прерываем
                if (!player.IsLogged()) return;

                // Проверяем, что полицейский в машине еще раз
                if (!player.IsInVehicle) return;

                // Если игрок не является полицейским
                if (player.GetCharacter().FractionID != 7) return;
                // Если полицейский не на службе
                if (!player.GetCharacter().OnDuty) return;

                // Если машина закрыта - обрываем метод!
                if (player.Vehicle.Locked) return;

                // Проверяем, что машина, в которой находится полицейский, числится как угнанная
                if (!Cars.Select(e => e.Vehicle).Contains(player.Vehicle)) return;
                

                Policeman policeman;
                // Если полицейский в первый раз заводит ее - записываем его в список полицейских
                if (!Policemen.Select(e => e.Player).Contains(player))
                {
                    // Создаем полицейского
                    policeman = new Policeman(player);
                    // Добавляем в список
                    Policemen.Add(policeman);
                    // Устанавливаем стандию квеста доставки
                    policeman.JobStage = QuestStage.CAR_DELIVERY;
                    // Доставьте машину в полицейский участок
                    policeman.SendMessage("AutoThief_18", 5000);
                }
                else policeman = Policemen.Find(e => e.Player == player); // Или находим его

                // Был ли создан или найден полицейский
                if (policeman == null) return;

                // Привязываем к нему машину всегда (т.к. есть вероятность, что он пересядет в другую)
                Car findedCar = Cars.Find(e => e.Vehicle == player.Vehicle);

                // Была ли найдена машина
                if (findedCar == null) return;

                // Связываем полицейского и машину, запускаем двигатель
                policeman.Car = findedCar;
                policeman.StartEngine();

                // Устанавливаем точку сдачи автомобиля (нужно сделать один раз - обработать позже!)
                policeman.CreateCheckpoint(policeman.DeliverPoint, 7);
                policeman.CreateWaypoint(policeman.DeliverPoint);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Work.PolicemanStartEngine(): {ex.ToString()}");
            }
        }


        /// <summary>
        /// InitWorkDay
        /// </summary>
        /// <param name="client"></param>
        public static void InitWorkDay(PlayerGo client)
        {
            try
            {
                if (!client.IsLogged())
                    return;
                if (client.GetCharacter().FractionID == 7)
                {
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "AutoThief_17", 3000);
                    return;
                }
                if ((client.GetFraction()?.OrgActiveType ?? OrgActivityType.Invalid) != OrgActivityType.Crime && (client.GetFamily()?.OrgActiveType ?? OrgActivityType.Unknown) != OrgActivityType.Crime)
                {
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "AutoThief_19", 3000);
                    return;
                }
                // Устраиваемся на работу
                if (!Workers.Exists(e => e.Player == client))
                {
                    if (client.GetCharacter().LVL < MinLVL)
                    {
                        Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, $"Jobs_73".Translate(MinLVL), 3000);
                        return;
                    }
                    if (client.GetCharacter().WorkID != 0 && client.GetCharacter().WorkID != WorkID)
                    {
                        Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, $"Jobs_64".Translate(), 3000);
                        return;
                    }
                    if (client.HasData("CARTHIEF::WORK::LEAVE::RECENTLY") && (client.GetData<DateTime>("CARTHIEF::WORK::LEAVE::RECENTLY")).AddMinutes(WorkGetTimerMinutes) > DateTime.Now)
                    {
                        Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Technician_10".Translate(WorkGetTimerMinutes), 3000);
                        return;
                    }

                    client.SetData("lastWorkAction", DateTime.Now);
                    Thief worker = new Thief(client);
                    Workers.Add(worker);
                    //worker.SetSkin();
                    //Вы начали рабочий день техника, ожидайте поступления заказов
                    worker.SendMessage("AutoThief_6".Translate(), 5000); // ОК!
                    client.GetCharacter().WorkID = WorkID;
                    worker.GiveWork();
                }
                // Оканчиваем работу
                else
                {
                    Thief worker = Workers.Find(e => e.Player == client);
                    
                    //Вы закончили рабочий день техника
                    worker.SendMessage("AutoThief_7".Translate(), 5000); // ОК

                    worker.DeleteWaypoint();
                    worker.DeleteExistedMapZone();
                    client.GetCharacter().WorkID = 0;

                    CMD_DeletePed(client); // Удаляем педа
                    worker.ChangeNameColor(false); // Удаляем красный ник

                    Workers.Remove(worker);
                    // Выставляем стандартные переменные на клиенте
                    client.TriggerEvent("WORK::CARTHIEF::CHANGE::STATE::SERVER", false);
                    client.TriggerEvent("WORK::CARTHIEF::CHANGE::LOCK::SERVER", false);

                    // Игрок окончил работу, сохраняем состояние
                    client.SetData("CARTHIEF::WORK::LEAVE::RECENTLY", DateTime.Now);

                    // Пытаемся удалить машину
                    worker.DeleteCar();
                }
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Work.InitWorkDay(Player client): {ex.ToString()}");
            }
        }

        #region Debug Commands

        //[Command("deleteped")]
        public static void CMD_DeletePed(Player player)
        {
            // Если это не рабочий - прерываем событие
            Thief worker = Workers.Find(e => e.Player == player);
            if (worker == null) return;

            worker.Player.TriggerEvent("WORK::CARTHIEF::PED::DELETE");
        }

        [Command("autoworkers")]
        public static void CMD_GetAutoWorkers(Player player)
        {
            if (player.GetCharacter().AdminLVL == 0) return;
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

        [Command("copworkers")]
        public static void CMD_GetLSPDWorkers(Player player)
        {
            if (player.GetCharacter().AdminLVL == 0) return;
            try
            {
                string message = "";
                Work.Policemen.ForEach(e => message += e);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, message, 15000);
            }
            catch (Exception ex)
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, ex.ToString(), 10000);
            }
        }

        [Command("workcars")]
        public static void CMD_GetCars(Player player)
        {
            if (player.GetCharacter().AdminLVL == 0) return;
            try
            {
                string message = "";
                Work.Cars.ForEach(e => message += e);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, message, 15000);
            }
            catch (Exception ex)
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, ex.ToString(), 10000);
            }
        }

        #region Goto mark test

        [Command("gotomark")]
        public static void CMD_GotoMarkTest(Player player)
        {
            try
            {
                if (!player.IsInVehicle || !Group.CanUseAdminCommand(player, "gotomark")) return;
                player.ChangePosition(null);
                player.TriggerEvent("GOTOMARK::SEND");
            }
            catch (Exception ex)
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, ex.ToString(), 3000);
            }
        }

        [RemoteEvent("GOTOMARK::GET")]
        public static void GetEvent(Player player, float x, float y, float z, bool isSend1)
        {
            try
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"X:{x}, Y{y}, Z:{z}", 3000);
                if (!isSend1) return;
                WhistlerTask.Run(() =>
                {
                    player.TriggerEvent("GOTOMARK::SEND2");
                }, 2000);
            }
            catch (Exception ex)
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, ex.ToString(), 10000);
            }
        }

        #endregion
        #endregion
    }
}