using GTANetworkAPI;
using Whistler.Core;
using System;
using System.Collections.Generic;
using System.Text;
using static Whistler.Jobs.Technician.Work;

namespace Whistler.Jobs.Technician
{
    class WorkStation
    {
        #region Constructor

        public WorkStation(string name, Vector3 diagnosticPoint, List<Vector3> repairPoints)
        {
            try
            {
                this.Name = name;
                this.DiagnosticPoint = diagnosticPoint;
                this.RepairPoints = repairPoints;

                InteractShape.Create(DiagnosticPoint, 1, 2)
                    .AddInteraction((player) =>
                    {
                        if (player.IsInVehicle) return;
                        Technician worker = Work.Workers.Find(e => e?.Player == player);
                        // Если он не относится к этой рабочей станции или находится не на стадии диагностики
                        if (worker?.WorkStation != this || !worker.JobStage.Equals(JobStage.DIAGNOSTIC) || !worker.CheckAccessNextAction()) return;
                        worker?.StartDiagnostic();
                    })
                    .AddEnterPredicate((colshape, player) =>
                    {
                        Technician worker = Work.Workers.Find(e => e?.Player == player);
                        // Если он не относится к этой рабочей станции или находится не на стадии диагностики
                        if (worker?.WorkStation != this || !worker.JobStage.Equals(JobStage.DIAGNOSTIC))
                            return false;
                        return true;
                    });

                // Инициализируем точки починки
                foreach (var repairPoint in RepairPoints)
                {
                    var currentPoint = repairPoint;

                    InteractShape.Create(currentPoint, 1, 2)
                        .AddInteraction((player) =>
                        {
                            if (player.IsInVehicle) return;
                            Technician worker = Work.Workers.Find(e => e?.Player == player);
                            // Если он не относится к этой рабочей станции или находится не на стадии диагностики
                            if (worker?.WorkStation != this || worker?.RepairPoint != currentPoint || !worker.JobStage.Equals(JobStage.REPAIR) || !worker.CheckAccessNextAction()) return;
                            worker?.StartRepair();
                        })
                        .AddEnterPredicate((colshape,player) =>
                        {
                            Technician worker = Work.Workers.Find(e => e?.Player == player);
                            // Если он не относится к этой рабочей станции или находится не на стадии диагностики
                            if (worker?.WorkStation != this || worker?.RepairPoint != currentPoint || !worker.JobStage.Equals(JobStage.REPAIR)) 
                                return false;
                            return true;
                        });

                }
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"WorkStation.Constructor(): {ex.ToString()}");
            }
        }

        #endregion

        public string Name { get; set; }
        public Vector3 DiagnosticPoint { get; set; }
        public List<Vector3> RepairPoints { get; set; }
        public ColShape DiagnosticColShape { get; set; }

    }
}
