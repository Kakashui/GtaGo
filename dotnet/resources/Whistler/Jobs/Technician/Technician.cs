using GTANetworkAPI;
using Whistler.ClothesCustom;
using Whistler.Core;
using Whistler.Jobs.AbstractEntity;
using Newtonsoft.Json;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Whistler.Jobs.Technician.Work;
using Whistler.Helpers;
using Whistler.NewDonateShop;
using Whistler.Entities;

namespace Whistler.Jobs.Technician
{
    class Technician : AbstractWorker
    {
        public Technician(ExtPlayer client) : base(client) { }

        #region Properties
        public WorkStation WorkStation { get; set; } = null;
        public Vector3 RepairPoint { get; set; } = null;
        public JobStage JobStage { get; set; } = JobStage.ORDER_SEARCH;
        public Vector3 OrderGetPosition { get; set; } = null;
        public int EarnedMoney { get; set; } = 0;
        public DateTime StartWorkTime { get; set; } = DateTime.Now;
        public List<WorkStation> WorkStationList { get; set; } = Work.WorkStations.OrderBy(a => Guid.NewGuid()).ToList();
        public int CurrentWorkStationIndex { get; set; } = 0;

        #endregion

        #region Methods

        public void GiveWork()
        {
            try
            {
                if (!JobStage.Equals(JobStage.ORDER_SEARCH)) return;

                NAPI.Task.Run(() =>
                {
                    this.CurrentWorkStationIndex = CurrentWorkStationIndex < WorkStationList.Count ? CurrentWorkStationIndex : 0;
                    this.WorkStation = this.WorkStationList[this.CurrentWorkStationIndex++];
                    // Начинаем его рабочий квест на этой точке
                    this.JobStage = JobStage.DIAGNOSTIC;
                    StartWorkQuest();
                }, 10000);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.GiveWork(): {ex.ToString()}");
            }
        }

        #region Create/Destroy Quest

        public void StartWorkQuest()
        {
            try
            {
                if (WorkStation == null) return;

                CreateWaypoint(WorkStation.DiagnosticPoint);

                // Сохраняем позицию игрока для расчета З.П. в конце
                OrderGetPosition = Player.Position;

                //Вы получили новый заказ, отправляйтесь на {WorkStation.Name} и сделайте диагностику
                SendMessage("Technician_3".Translate( WorkStation.Name), 5000);
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
                this.JobStage = JobStage.ORDER_SEARCH;
                // Теперь игрок может получать новые заказы! Но он не должен попасть на той же точке!
                GiveWork(); 
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.EndWorkQuest(): {ex.ToString()}");
            }
        }

        #endregion

        #region Diagnostic selected WorkStation

        public void StartDiagnostic()
        {
            try
            {
                // Пользователь проводит диагностику
                //Начата диагностика участка {WorkStation.Name}
                SendMessage("Technician_4".Translate( WorkStation.Name), 3000);
                Main.OnAntiAnim(Player);
                Player.PlayAnimation("amb@medic@standing@timeofdeath@base", "base", 39);
                SafeTrigger.ClientEvent(Player,"WORK::TECHNICIAN::DIAGNOSTIC::SERVER");
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.StartDiagnostic(): {ex.ToString()}");
            }
        }

        public void EndDiagnostic(bool isSuccess)
        {
            try
            {
                Player.StopAnimation();
                Main.OffAntiAnim(Player);

                // Обрываем, если пользователь не прошел миниигру
                if (!isSuccess) return;
                SafeTrigger.SetData(Player, "lastWorkAction", DateTime.Now);

                this.JobStage = JobStage.SHOP;
                //int repairStacks = WorkManager.rnd.Next(1, 4); // От 1 до 3 комплектов
                // Диагностика участка {WorkStation.Name} окончена, отправляйтесь на склад и возьмите сумку с необходимыми материалами
                SendMessage("Technician_12".Translate( WorkStation.Name), 5000);
                CreateWaypoint(Work.ShopColShape);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.EndDiagnostic(): {ex.ToString()}");
            }
        }

        #endregion

        #region Shop - buy repair kits

        public void OpenShop()
        {
            try
            {
                CloseShop();
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.OpenShop(): {ex.ToString()}");
            }
        }

        public void CloseShop()
        {
            try
            {
                SafeTrigger.SetData(Player, "lastWorkAction", DateTime.Now);
                // Выдаем сумку

                // Выбираем рандомную точку починки на станции
                var repairPoint = WorkStation.RepairPoints.GetRandomElement();

                //Вы взяли сумку с материалами для ремонта, отправляйтесь на точку починки
                SendMessage("Technician_13".Translate(), 5000);

                this.RepairPoint = repairPoint;
                this.JobStage = JobStage.REPAIR;

                // Подсвечиваем точку ремонта
                CreateWaypoint(repairPoint);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.CloseShop(): {ex.ToString()}");
            }
        }

        #endregion

        #region Repair selected point on WorkStation

        /// <summary>
        /// Тут может быть дюп, его нужно как-то остановить, т.к. будет выдача денег!
        /// </summary>
        public void StartRepair()
        {
            try
            {
                //Начата диагностика участка {WorkStation.Name}
                SendMessage("Technician_4".Translate( WorkStation.Name), 3000);

                Main.OnAntiAnim(Player);
                Player.PlayAnimation("amb@medic@standing@kneel@base", "base", 39);
                SafeTrigger.ClientEvent(Player, "WORK::TECHNICIAN::DIAGNOSTIC::SERVER");
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.StartRepair(): {ex.ToString()}");
            }
        }

        public void EndRepair(bool isSuccess)
        {
            try
            {
                Player.StopAnimation();
                Main.OffAntiAnim(Player);
                var rotation = Player.Rotation;
                this.Player.ChangePosition(null);
                NAPI.Player.SpawnPlayer(this.Player, this.Player.Position);
                Player.Rotation = rotation;

                // Обрываем, если пользователь не прошел миниигру
                if (!isSuccess) return;
                SafeTrigger.SetData(Player, "lastWorkAction", DateTime.Now);
                DeleteWaypoint();

                // Выплата денег
                //Починка участка успешно окончена. Вы заработали {Work.Salary}$. Ожидайте новых заказов.
                SendSalaryToBank();

                EndWorkQuest();
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.EndRepair(): {ex.ToString()}");
            }
        }

        #endregion

        public void SendSalaryToBank()
        {
            try
            {
                // Calculate Salary
                int resultDistance = (int)Math.Round(
                                        OrderGetPosition.DistanceTo(WorkStation.DiagnosticPoint) +
                                        WorkStation.DiagnosticPoint.DistanceTo(ShopColShape)*2
                                        );
                int salary = DonateService.UseJobCoef(Player, Convert.ToInt32(resultDistance * PriceByDistance), Player.Character.IsPrimeActive());
                //if (salary >= ?) salary = ?; // Защита

                EarnedMoney += salary;
                MoneySystem.Wallet.MoneyAdd(Player.Character.BankModel, salary, "Работа техника");
                SendMessage("Technician_14".Translate( salary), 9000);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.EndRepair(): {ex.ToString()}");
            }
        }
        
        public int GetWorkedMinutes()
        {
            return (int)Math.Round(DateTime.Now.Subtract(this.StartWorkTime).TotalMinutes);
        }

        public override void CreateWaypoint(Vector3 point)
        {
            try
            {
                SafeTrigger.ClientEvent(Player, "createWaypoint", point.X, point.Y);
                SafeTrigger.ClientEvent(Player, "createCheckpoint", 15, 1, point, 1, 0, 255, 0, 0);
                SafeTrigger.ClientEvent(Player, "createWorkBlip", point);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"CreateWaypoint(): {ex.ToString()}");
            }
        }

        public void DeleteWaypoint()
        {
            try
            {
                SafeTrigger.ClientEvent(Player, "deleteCheckpoint", 15);
                SafeTrigger.ClientEvent(Player, "deleteWorkBlip");
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Worker.DeleteWaypoint(): {ex.ToString()}");
            }
        }

        public override string ToString()
        {
            return $"{Name}({Id}, {EarnedMoney}$, {GetWorkedMinutes()} mins) ";
        }

        

        #endregion
    }
}
