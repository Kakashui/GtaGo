using GTANetworkAPI;
using Whistler.Core;
using Whistler.Jobs.AbstractEntity;
using System;
using System.Collections.Generic;
using System.Text;
using static Whistler.Jobs.CarThief.Work;
using Whistler.Helpers;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.Jobs.CarThief
{
    class Policeman : AbstractWorker
    {
        public Policeman(ExtPlayer client) : base(client) { }
        public Car Car { get; set; } = null;
        public Vector3 DeliverPoint { get; set; } = Work.PoliceDeliverCarPoint;
        public QuestStage JobStage { get; set; }
        public DateTime StartWorkTime { get; set; } = DateTime.Now;

        public void DeliverCar()
        {
            try
            {
                // Если полицейский не в машине 
                // или не в той машине, которую он угнать
                // или расстояние до точки сдачи от его машины, которую он должен сдать меньше трех метров
                if (!this.Player.IsInVehicle || !this.Car.Vehicle.AllOccupants.ContainsValue(this.Player) || this.Car.Vehicle.Position.DistanceTo(DeliverPoint) > 3) return;

                // Удаляем путь и маркер
                DeleteWaypointAndCheckPoint();

                // Выплачиваем зарплату
                SendSalaryToBank();

                // Меняем статус (доп защита от дюпа)
                this.JobStage = QuestStage.ORDER_SEARCH;

                // Завершаем квест у вора данной машины, если он еще в игре
                #region End carthief's quest
                Thief thief = Workers.Find(e => e.Car == this.Car);
                if (thief != null) // Если он в игре
                {
                    thief.TurnOffWorkQuest();
                }
                #endregion

                // Удаляем машину, если она есть и освобождаем точку спауна
                DeleteCar();

                // Удаляем из списка рабочих
                Policemen.Remove(this);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Policeman.DeliverCar(): {ex.ToString()}");
            }
        }

        public void SendSalaryToBank()
        {
            try
            {
                int salary = Work.PolicemanSalary;
                MoneySystem.Wallet.MoneyAdd(Player.Character.BankModel, salary, "Награда за возвращение угнанного авто");
                SendMessage("AutoThief_9".Translate( salary), 9000);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.EndRepair(): {ex.ToString()}");
            }
        }

        public void CreateCheckpoint(Vector3 point, int scale = 1)
        {
            try
            {
                Player.CreateClientCheckpoint(15, 1, point, scale, 0, new Color(255, 0, 0));
                SafeTrigger.ClientEvent(Player, "createWorkBlip", point);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Policeman.CreateCheckpoint(): {ex.ToString()}");
            }
        }

        public void StartEngine()
        {
            try
            {
                if (this.Car == null || this.Car.Vehicle == null) return;
                this.Car.Vehicle.EngineStatus = true;
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Policeman.StartEngine(): {ex.ToString()}");
            }
        }

        public void DeleteCar()
        {
            if (this.Car != null)
            {
                if (this.Car.CarLocation != null) this.Car.CarLocation.IsOccupied = false;
                if (this.Car.Vehicle != null)
                {
                    // Если к машине привязан угонщик - прерываем
                    if (Work.Workers.Find(e => e.Car?.Vehicle == this.Car?.Vehicle) != null) return;

                    this.Car.Vehicle.CustomDelete();
                    this.Car.Vehicle = null;
                    // Удаляем машину из списка машин
                    Work.Cars.Remove(this.Car);
                }
            }
        }

        public int GetWorkedMinutes()
        {
            return (int)Math.Round(DateTime.Now.Subtract(this.StartWorkTime).TotalMinutes);
        }

        public override string ToString()
        {
            return $"{Name}({Id}, {GetWorkedMinutes()} mins) ";
        }
    }
}
