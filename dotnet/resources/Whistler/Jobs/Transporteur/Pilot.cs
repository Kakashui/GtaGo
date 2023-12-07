using GTANetworkAPI;
using Whistler.Core;
using Whistler.Jobs.AbstractEntity;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Whistler.Jobs.Transporteur.Work;
using Whistler.Helpers;
using Whistler.NewDonateShop;
using Whistler.Entities;
using Whistler.GUI;
using Whistler.VehicleSystem.Models;
using Whistler.VehicleSystem;

namespace Whistler.Jobs.Transporteur
{
    class Pilot : AbstractWorker, IDisposable
    {
        public Pilot(ExtPlayer client) : base(client)
        {
            
        }

        public QuestStage JobStage { get; set; } = QuestStage.ORDER_SEARCH;
        public Cargobob Cargobob { get; set; }
        public DeliverCar DeliverCar { get; set; }
        public bool IsEngineAlreadyStarted { get; set; } = false;
        public Vector3 DeliverPoint { get; set; }
        public string TimerID { get; set; } = null;
        public DateTime StartWorkTime { get; set; } = DateTime.Now;
        public int EarnedMoney { get; set; } = 0;

        #region Properties



        #endregion

        public async void GiveCarAsync()
        {
            try
            {
                // Пытаемся выдать пилоту новую машину, каждые 5 секунд
                // Если он вышел за это время 
                bool resultOfCreateCar = false;
                while (!resultOfCreateCar)
                {
                    await Task.Delay(5000);
                    
                    if (Work.Workers.Contains(this))
                    {
                        NAPI.Task.Run(()=> resultOfCreateCar = this.GiveCar());
                    }
                    else return; // Иначе игрок либо вышел из игры, либо ушел с работы
                }
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Pilot.GiveCarAsync(): {ex.ToString()}");
            }
        }

        public async void FreeCargobobSpawnAsync()
        {
            try
            {
                // Пытаемся освободить точку сдачи вертолета
                while (this.Cargobob.CarLocation.IsOccupied)
                {
                    await Task.Delay(5000);
                    if (this != null && Work.Workers.Contains(this) && this.Cargobob != null)
                    {
                        // Если игрок отлетел на вертолете более чем на 20 метров - прерываем
                        NAPI.Task.Run(() => {
                            if (this.Cargobob.Vehicle.Position.DistanceTo(this.Cargobob.CarLocation.Position) > 20)
                                this.Cargobob.CarLocation.IsOccupied = false;
                        });
                        
                    }
                    else return; // Иначе игрок либо вышел из игры, либо ушел с работы
                }
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Pilot.FreeCargobobSpawnAsync(): {ex.ToString()}");
            }
        }

        public bool GiveCar()
        {
            try
            {
                if (!JobStage.Equals(QuestStage.ORDER_SEARCH)) return false;

                // Создаем машину на клиенте и прописываем к ней путь
                /*
                 * !!! Тут может быть проблема, что другой игрок может утащить его машину, сделанную на клиенте.
                 * Желательно сделать как с спауном каргобоба!!!
                */ 
                SpawnPoint randomCarLocation = Work.VehSpawns.Where(item => !item.IsOccupied).GetRandomElement();

                // Если точка занята
                if (randomCarLocation == null)
                {
                    // Сейчас нет свободных заказов на доставку транспорта, ожидайте...
                    SendMessage("Transporteur_5".Translate(), 5000);
                    return false;
                }

                // Создаем машину
                DeliverCar deliverCar = new DeliverCar(this, randomCarLocation);

                this.DeliverCar = deliverCar;
                SafeTrigger.ClientEvent(Player, "WORK::TRANSPORTEUR::CREATE::VEHICLE::SERVER", randomCarLocation.Position.X, randomCarLocation.Position.Y, randomCarLocation.Position.Z,
                                                                                         deliverCar.Model, deliverCar.Number);

                this.JobStage = QuestStage.FLY_TO_CAR;
                this.CreateCheckpoint(randomCarLocation.Position);
                this.CreateWaypoint(randomCarLocation.Position);
                // Меняем переменную на клиенте, чтобы сработал рендер, и мы могли получить точку сдачи машины, когда подлетим к ней
                SafeTrigger.ClientEvent(Player, "WORK::TRANSPORTEUR::CHANGE::STATE::SERVER", "isFlyToCar", true);

                SendMessage("Transporteur_3".Translate(this.DeliverCar.Model, this.DeliverCar.Number), 5000);

                #region Timer to get Vehicle

                // Запускаем таймер, 20 минут
                //this.StartTimer(20, QuestStage.FLY_TO_CAR);
                int minutes = 20;
                // Останавливаем существующий таймер, если он был запущен
                this.StopTimer();
                // Запускаем таймер на клиенте
                this.StartClientTimer(minutes);
                // Запускаем таймер на сервере
                this.TimerID = Timers.StartOnce(minutes * 60 * 1000, () => NAPI.Task.Run(() => this.CheckTimer(QuestStage.FLY_TO_CAR)));

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Pilot.GiveCar(): {ex.ToString()}");
                return false;
            }
        }


        public async void EndWorkQuest()
        {
            try
            {
                // Если рабочий не находится на стадии доставки транспорта - прерываем
                if (!this.JobStage.Equals(QuestStage.DELIVER)) return;
                // Удаляем сданную им машину на клиенте
                NAPI.Task.Run(() => 
                {
                    SafeTrigger.ClientEvent(Player, "WORK::TRANSPORTEUR::DELETE::VEHICLE::SERVER");
                    // Платим зарплату
                    SendSalaryToBank();
                });
                // Меняем статус, чтобы не было дюпов
                this.JobStage = QuestStage.ORDER_SEARCH;

                // Выдаем новую машину, через 5 секунд
                // Чтобы пользователь успел увидеть уведомление о том, что ему поступили деньги на банковский счет
                await Task.Delay(5000);

                // Пытаемся выдать пилоту новую машину, каждые 5 секунд
                // Если он вышел за это время 
                this.GiveCarAsync();
                    


                //NAPI.Task.Run(() =>
                //{
                //  while (!GiveCar()) ;
                //}, 5000);


            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Pilot.EndWorkQuest(): {ex.ToString()}");
            }
        }

        public void GiveDeliverPoint()
        {
            try
            {
                Vector3 randomDeliverPoint = Work.DeliverPoints.GetRandomElement();
                CreateWaypoint(randomDeliverPoint);
                CreateCheckpoint(randomDeliverPoint, 9);
                this.DeliverPoint = randomDeliverPoint;
                // Доставьте транспорт на точку сдачи
                SendMessage("Transporteur_7".Translate(this.DeliverCar.Model, this.DeliverCar.Number), 5000);
                // Убираем работу рендера
                SafeTrigger.ClientEvent(Player, "WORK::TRANSPORTEUR::CHANGE::STATE::SERVER", "isFlyToCar", false);
                this.JobStage = QuestStage.DELIVER;

                #region Timer to get deliver vehicle

                // Запускаем таймер, 20 минут
                //this.StartTimer(20, QuestStage.DELIVER);
                int minutes = 20;
                // Останавливаем существующий таймер, если он был запущен
                this.StopTimer();
                // Запускаем таймер на клиенте
                this.StartClientTimer(minutes);
                // Запускаем таймер на сервере
                this.TimerID = Timers.StartOnce(minutes * 60 * 1000, () => NAPI.Task.Run(() => this.CheckTimer(QuestStage.DELIVER)));

                #endregion
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Pilot.GiveDeliverPoint(): {ex.ToString()}");
            }
        }

        public void SendSalaryToBank()
        {
            try
            {
                int salary = DonateService.UseJobCoef(Player, Salary, Player.Character.IsPrimeActive());
                MoneySystem.Wallet.MoneyAdd(Player.Character.BankModel, salary, "Работа пилота");
                this.EarnedMoney += salary;
                SendMessage("AutoThief_9".Translate(salary), 5000);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Pilot.SendSalaryToBank(): {ex.ToString()}");
            }
        }

        public void CreateCheckpoint(Vector3 point, int scale = 1)
        {
            try
            {
                SafeTrigger.ClientEvent(Player, "createCheckpoint", 15, 1, point, scale, 0, 255, 0, 0);
                SafeTrigger.ClientEvent(Player, "createWorkBlip", point);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Pilot.CreateCheckpoint(): {ex.ToString()}");
            }
        }

        public void CheckTimer(QuestStage questStageCheck)
        {
            try
            {
                //int timerSeconds = timerMinutes * 60;
                // Отключаем старый таймер
                //if (!this.TimerID.Equals("")) this.StopTimer();

                // Запускаем таймер
                //this.SafeTrigger.ClientEvent(player,"WORK::TRANSPORTEUR::TIMER::SET::SERVER", timerSeconds);

                // Асинхронная пауза, перед проверкой
                //await Task.Delay(timerSeconds * 1000);

                // Если игрок еще на стадии проверки - оканчиваем квест!
                if (this.JobStage.Equals(questStageCheck))
                {
                    this.Player.Character.WorkID = 0;
                    MainMenu.SendStats(this.Player);
                    this.Dispose();
                    this.SendMessage("Transporteur_8".Translate(), 8000);
                }

                //// Запускаем таймер проверки
                //NAPI.Task.Run(() =>
                //{
                //    // Если игрок еще на стадии проверки - оканчиваем квест!
                //    if (this.JobStage.Equals(questStageCheck))
                //    {
                //        this.Dispose();
                //        this.SendMessage("Transporteur_8".Translate(), 8000);
                //    }

                //}, timerSeconds * 1000);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Pilot.CheckTimer(): {ex.ToString()}");
            }
        }

        public void StartClientTimer(int timerMinutes)
        {
            try
            {
                int timerSeconds = timerMinutes * 60;
                SafeTrigger.ClientEvent(Player, "WORK::TRANSPORTEUR::TIMER::SET::SERVER", timerSeconds);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Pilot.StartClientTimer(): {ex.ToString()}");
            }
        }

        public void StopTimer()
        {
            try
            {
                // Отключаем время на клиенте над картой
                SafeTrigger.ClientEvent(Player, "WORK::TRANSPORTEUR::TIMER::STOP::SERVER");
                if (string.IsNullOrEmpty(TimerID)) return;
                // Отключаем таймер, чтобы он не сработал
                Timers.Stop(TimerID);
                TimerID = null;
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Pilot.StopTimer(): {ex.ToString()}");
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

        public void Dispose()
        {
            try
            {
                this.StopTimer();
                // Удаляем waypoint и checkpoint
                DeleteWaypointAndCheckPoint();
                // Устанавливаем стандартную работу
                //client.Character.WorkID = 0; 
                // Удаляем из списка рабочих
                Work.Workers.Remove(this);
                // Выставляем стандартные переменные на клиенте
                SafeTrigger.ClientEvent(Player, "WORK::TRANSPORTEUR::CHANGE::STATE::SERVER", "isTransporteurWorker", false);
                SafeTrigger.ClientEvent(Player, "WORK::TRANSPORTEUR::CHANGE::STATE::SERVER", "isFlyToCar", false);
                // Уничтожаем машину (все проверки на клиенте)
                SafeTrigger.ClientEvent(Player, "WORK::TRANSPORTEUR::DELETE::VEHICLE::SERVER");
                // Освобождаем точку для спауна машины
                if (DeliverCar != null)
                {
                    if (DeliverCar.CarLocation != null) DeliverCar.CarLocation.IsOccupied = false;
                }
                // Уничтожаем Cargobob
                if (Cargobob != null)
                {
                    Cargobob.Vehicle.CustomDelete();
                    // Освобождаем точку для спауна другими игроками
                    Cargobob.CarLocation.IsOccupied = false;
                    Work.Cargobobs.Remove(Cargobob);
                    Cargobob = null;
                }
                // Отключаем таймер
                //this.StopTimer();
                // Игрок окончил работу, сохраняем состояние
                SafeTrigger.SetData(Player, $"{Work.JobName}::WORK::LEAVE::RECENTLY", true);
                // Даем возможность устроиться на работу через указанное в настройках время
                NAPI.Task.Run(() =>
                {
                    // Даем возможность устроиться
                    SafeTrigger.SetData(Player, $"{Work.JobName}::WORK::LEAVE::RECENTLY", false);
                }, 60 * 1000 * Work.WorkTimer);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Pilot.Dispose(): {ex.ToString()}");
            }
        }
    }
}
