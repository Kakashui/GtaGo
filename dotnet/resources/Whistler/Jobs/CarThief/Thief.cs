using GTANetworkAPI;
using Whistler.Jobs.AbstractEntity;
using System;
using System.Linq;
using static Whistler.Jobs.CarThief.Work;
using Whistler.NewDonateShop;
using Whistler.Helpers;
using Whistler.Fractions;
using Whistler.Common;
using Whistler.Entities;
using Whistler.SDK;
using Whistler.GUI;

namespace Whistler.Jobs.CarThief
{
    class Thief : AbstractWorker
    {
        public Thief(ExtPlayer client) : base(client)
        {
        }

        #region Properties

        public QuestStage JobStage { get; set; } = QuestStage.ORDER_SEARCH;
        public Agent Agent { get; set; } = null;
        public Car Car { get; set; } = null;
        public Vector3 NumberReplacement { get; set; } = null;
        public Vector3 Garage { get; set; } = null;
        public Vector3 OrderGetPosition { get; set; } = null;
        public int EarnedMoney { get; set; } = 0;
        public DateTime StartWorkTime { get; set; } = DateTime.Now;

        #endregion


        #region Methods

        public void GiveWork()
        {
            try
            {
                if (!JobStage.Equals(QuestStage.ORDER_SEARCH)) return;

                NAPI.Task.Run(() =>
                {
                    Agent randomAgent = CarThiefConfigs.AgentPeds.GetRandomElement();
                    this.Agent = randomAgent;
                    this.JobStage = QuestStage.AGENT_SEARCH;
                    StartWorkQuest();
                }, 10000);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.GiveWork(): {ex.ToString()}");
            }
        }

        public void StartWorkQuest()
        {
            try
            {
                if (this.Agent == null) return;

                // Сохраняем позицию игрока
                this.OrderGetPosition = Player.Position;

                // Создаем педа на клиенте
                SafeTrigger.ClientEvent(Player, "WORK::CARTHIEF::PED::CRETATE", Agent.Hash, Agent.Position.X, Agent.Position.Y, Agent.Position.Z, Agent.Rotation, 0);
                // Создаем точку на карте
                // Создаем рандомную зону, включающую агента и ставим в центр этой зоны маршрут
                CreateRandomMapZone(Agent.Position, 10);
                //Найдите и поговорите с агентом {Agent.Name}
                SendMessage("AutoThief_8".Translate( Agent.Name), 4000);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.StartWorkQuest(): {ex.ToString()}");
            }
        }

        public void EndWorkQuest()
        {
            try
            {
                //Вы успешно справились с заданием
                // Выдача денег
                SendSalaryToBank();
                ChangeNameColor(false); // Удаляем красный ник
                DeleteWaypoint(); // Удаляем путь
                DeleteExistedMapZone(); // Удаляем зону
                CMD_DeletePed(this.Player); // Удаляем педа
                // Выставляем стандартные переменные на клиенте
                SafeTrigger.ClientEvent(Player, "WORK::CARTHIEF::CHANGE::STATE::SERVER", false);
                SafeTrigger.ClientEvent(Player, "WORK::CARTHIEF::CHANGE::LOCK::SERVER", false);

                // Меняем статус
                this.JobStage = QuestStage.ORDER_SEARCH;

                // Завершаем работу
                onPlayerDisconnected(this.Player, DisconnectionType.Left, $"timeleft Quest Ended");
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.EndWorkQuest(): {ex.ToString()}");
            }
        }

        public void SendSalaryToBank()
        {
            try
            {
                // Calculate Salary
                int resultDistance = (int)Math.Round(
                                        OrderGetPosition.DistanceTo(Agent.Position) +
                                        Agent.Position.DistanceTo(Car.CarLocation.Position) +
                                        Car.CarLocation.Position.DistanceTo(NumberReplacement) +
                                        NumberReplacement.DistanceTo(Garage));

                int salary = DonateService.UseJobCoef(Player, Convert.ToInt32(Work.PriceByDistance * resultDistance), Player.Character.IsPrimeActive());
                //if (salary >= ?) salary = ?; // Защита
                EarnedMoney += salary;
                MoneySystem.Wallet.MoneyAdd(Player.Character.BankModel, salary, "Угон авто");
                if ((Player.GetFraction()?.OrgActiveType ?? OrgActivityType.Invalid) == OrgActivityType.Crime)
                    MoneySystem.Wallet.MoneyAdd(Player.GetFraction(), salary, "Прибыль от работы членов организации");
                else
                {
                    if ((Player.GetFamily()?.OrgActiveType ?? OrgActivityType.Unknown) == OrgActivityType.Crime)
                        MoneySystem.Wallet.MoneyAdd(Player.GetFamily(), salary, "Прибыль от работы членов организации");
                }
                SendMessage("AutoThief_9".Translate(salary), 9000);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.EndRepair(): {ex.ToString()}");
            }
        }

        public void StartCarSearch()
        {
            try
            {
                Car randomCarLocation = CarThiefConfigs.RandomCarPositions.Where(item => !item.IsOccupied).GetRandomElement();
                if (randomCarLocation == null)
                {
                    //"В данный момомент у {Agent.Name} нет закзов, подождите некоторое время и попробуйте поговорить с ним снова!
                    this.SendMessage("AutoThief_10".Translate(Agent.Name), 9000);
                    return;
                }

                SafeTrigger.SetData(Player, "lastWorkAction", DateTime.Now);
                Car randomSpawnedCar = new Car(this, randomCarLocation);
                Work.Cars.Add(randomSpawnedCar); // Записываем машину в общий список машин
                this.Car = randomSpawnedCar; // Привязываем машину к игроку
                //Найдите автомобиль {this.Car.Model} с номером {this.Car.Number}. Взлом замка <O>
                SendMessage("AutoThief_11".Translate(this.Car.Model, this.Car.Number), 10000);
                DeleteExistedMapZone(); // Удаляем текущую зону
                // Создаем рандомную зону, включающую машину для угона и ставим в центр этой зоны маршрут
                CreateRandomMapZone(randomSpawnedCar.Position, Work.MapZoneRadius);
                SafeTrigger.ClientEvent(Player, "WORK::CARTHIEF::CHANGE::STATE::SERVER", true); // Запускаем, чтобы срабатывала кнопка O
                SafeTrigger.ClientEvent(Player, "WORK::CARTHIEF::CHANGE::LOCK::SERVER", false); // Замок автомобиля закрыт, чтобы не могли завести
                this.JobStage = QuestStage.CAR_SEARCH;
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.StartCarSearch(): {ex.ToString()}");
            }
        }

        public void EndCarSearch()
        {
            try
            {
                DeleteExistedMapZone(); // Удаляем текущую зону

                Vector3 randomNumberReplacement = CarThiefConfigs.NumberReplacements.GetRandomElement();
                CreateWaypoint(randomNumberReplacement);
                CreateCheckpoint(randomNumberReplacement);
                this.NumberReplacement = randomNumberReplacement;
                //Замок успешно взломан, заводите машину и отправляйтесь на точку смены номера. <O>
                SendMessage("AutoThief_12".Translate(), 10000);

                // Машина успешно взломана
                this.Car.Vehicle.Locked = false; // Открываем ее
                this.Car.Vehicle.IsLocked = false;
                // Сообщаем клиентской части, что замок взломан, таким образом машину открыть второй раз не получится
                SafeTrigger.ClientEvent(Player, "WORK::CARTHIEF::CHANGE::LOCK::SERVER", true);
                this.JobStage = QuestStage.CAR_NUMER_CHANGE;

                // Подсвечиваем ник игрока красным, чтобы полицейские могли их арестовать и сдать машину
                this.ChangeNameColor(true);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.EndCarSearch(): {ex.ToString()}");
            }
        }

        public void StartNumberChange()
        {
            try
            {
                if (this.JobStage.Equals(QuestStage.CAR_NUMER_CHANGE))
                {
                    // Запускаем смену номеров
                    this.SendMessage("AutoThief_13".Translate(), 10000);

                    SafeTrigger.ClientEvent(Player, "WORK::CARTHIEF::DIAGNOSTIC::SERVER");
                }
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.StartNumberChange(): {ex.ToString()}");
            }
        }

        public void EndNumerChange()
        {
            try
            {
                if (this.NumberReplacement.DistanceTo(this.Player.Position) > 3)
                    return;

                SafeTrigger.SetData(Player, "lastWorkAction", DateTime.Now);
                this.Car.UpdateRandomNumber();
                this.Car.IsNumberChanged = true;

                Vector3 randomGarage = CarThiefConfigs.Garages.GetRandomElement();
                CreateWaypoint(randomGarage);
                CreateCheckpoint(randomGarage);

                this.JobStage = QuestStage.CAR_DELIVERY;
                this.Garage = randomGarage;

                // Снимаем у человека красный ник, больше полицейские не могут сдать его машину
                this.ChangeNameColor(false);

                //Номера успешно изменены, отправляйтесь на точку сдачи транспорта
                SendMessage("AutoThief_14".Translate(), 10000);

            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.StartNumberChange(): {ex.ToString()}");
            }
        }

        public void ChangeNameColor(bool isRed)
        {
            try
            {
                SafeTrigger.SetSharedData(Player, "REDNAME", isRed);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.ChangeNameColor: {ex.ToString()}");
            }
        }

        public void StartEngine()
        {
            try
            {
                VehicleSystem.VehicleStreaming.SetEngineState(this.Car.Vehicle, true);
                //Машина успешно заведена
                this.SendMessage("AutoThief_15".Translate(), 5000);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.StartEngine(): {ex.ToString()}");
            }
        }

        public void CreateRandomMapZone(Vector3 location, int radius)
        {
            try
            {
                var random = location.GetRandomPointInRange(radius * 0.65);
                SafeTrigger.ClientEvent(Player, "WORK::CARTHIEF::CREATE::RADIUS", random, radius, Work.MapZoneColor);
                CreateWaypoint(random);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.CreateRandomMapZone: {ex.ToString()}");
            }

        }

        public void DeleteExistedMapZone()
        {
            try
            {
                SafeTrigger.ClientEvent(Player, "WORK::CARTHIEF::DELETE::RADIUS");
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.DeleteExistedMapZone(): {ex.ToString()}");
            }

        }

        public void DeleteWaypoint()
        {
            try
            {
                Player.DeleteClientMarker(15);
                SafeTrigger.ClientEvent(Player, "deleteWorkBlip");
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.DeleteWaypoint(): {ex.ToString()}");
            }
        }

        public int GetWorkedMinutes()
        {
            return (int)Math.Round(DateTime.Now.Subtract(this.StartWorkTime).TotalMinutes);
        }

        public override string ToString()
        {
            return $"{Name}({Id}, {EarnedMoney}$, {GetWorkedMinutes()} mins) ";
        }

        /// <summary>
        /// Отключаем квест, т.к. полицейский сдал машину
        /// </summary>
        public void TurnOffWorkQuest()
        {
            try
            {
                DeleteWaypoint();
                DeleteExistedMapZone();
                Player.Character.WorkID = 0;
                MainMenu.SendStats(Player);

                CMD_DeletePed(Player); // Удаляем педа

                ChangeNameColor(false);

                Workers.Remove(this);
                // Выставляем стандартные переменные на клиенте
                SafeTrigger.ClientEvent(Player, "WORK::CARTHIEF::CHANGE::STATE::SERVER", false);
                SafeTrigger.ClientEvent(Player, "WORK::CARTHIEF::CHANGE::LOCK::SERVER", false);

                // Задание провалено. Транспорт был доставлен в полицейский участок
                SendMessage("AutoThief_16".Translate(), 5000);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Thief.TurnOffWorkQuest(): {ex.ToString()}");
            }

        }

        public void DeleteCar()
        {
            // Удаляем машину, если она есть и освобождаем точку спауна
            if (this.Car != null)
            {
                if (this.Car.CarLocation != null) this.Car.CarLocation.IsOccupied = false;
                if (this.Car.Vehicle != null)
                {
                    // Если к машине привязан полицейский
                    if (Work.Policemen.Find(e => e.Car?.Vehicle == this.Car?.Vehicle) != null) return;

                    this.Car.Vehicle.CustomDelete();
                    this.Car.Vehicle = null;
                    // Удаляем машину из списка машин
                    Work.Cars.Remove(this.Car);
                }
            }
        }

        #endregion
    }
}
