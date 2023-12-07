using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.Entities;

namespace Whistler.Jobs.AbstractEntity
{
    abstract class AbstractFactory
    {
        public AbstractFactory(int minLVL, int workID, int workTimer, string jobName, Vector3 startEndWorkPoint, uint blipID, byte colorID, Action<ExtPlayer> startWorkAction)
        {
            this.MinLVL = minLVL;
            this.WorkID = workID;
            this.WorkTimer = workTimer;
            this.JobName = jobName;
            this.StartEndWorkPoint = startEndWorkPoint;

            #region Start Work ColShape

            NAPI.Blip.CreateBlip(blipID, startEndWorkPoint, 1.2f, colorID, $"Work {jobName.ToLower()}", 255, 2, true);

            var position = startEndWorkPoint;

            InteractShape.Create(position, 1.5f, 2)
                .AddDefaultMarker()
                .AddInteraction(startWorkAction);

            #endregion
        }

        public int MinLVL { get; set; }
        public int WorkID { get; set; }
        public int WorkTimer { get; set; }
        public string JobName { get; set; }
        public Vector3 StartEndWorkPoint { get; set; }
        public int InteractionNumber { get; set; }

        public abstract AbstractWorker CreateWorker(ExtPlayer client);

        public abstract AbstractVehicle CreateVehicle(AbstractWorker abstractWorker);

        public bool CheckWorker(ExtPlayer client)
        {
            try
            {
                if (client.Character.LVL < MinLVL)
                {
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, $"Jobs_73".Translate(MinLVL), 3000);
                    return false;
                }
                if (client.Character.WorkID != 0 && client.Character.WorkID != WorkID)
                {
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, $"Jobs_64".Translate(), 3000);
                    return false;
                }
                if (client.HasData($"{JobName}::WORK::LEAVE::RECENTLY") && client.GetData<bool>($"{JobName}::WORK::LEAVE::RECENTLY"))
                {
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Technician_10".Translate( WorkTimer), 3000);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"AbstractFactory.CheckWorker(): {ex}");
                return false;
            }
        }
    }
}
