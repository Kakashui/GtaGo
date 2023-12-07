using GTANetworkAPI;
using Whistler.Jobs.AbstractEntity;
using Whistler.SDK;
using System;
using System.Linq;
using Whistler.Helpers;
using Whistler.Entities;

namespace Whistler.Jobs.Transporteur
{
    class WorkFactory : AbstractFactory
    {
        public WorkFactory(int minLVL, int workID, int workTimer, string jobName, Vector3 startEndWorkPoint, uint blipID, byte colorID, Action<ExtPlayer> startWorkAction) 
            : base(minLVL, workID, workTimer, jobName, startEndWorkPoint, blipID, colorID, startWorkAction)
        {
        }

        public override AbstractWorker CreateWorker(ExtPlayer client)
        {
            try
            {
                // Если рабочий не может работать на этой работе
                if (!CheckWorker(client)) return null;
                /* !!! Проверяем лицензию пилота !!! */
                if (!client.CheckLic(GUI.Documents.Enums.LicenseName.Helicopter))
                {
                    // Для работы вам необходима лицензия пилота
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, $"Transporteur_6".Translate(), 3000);
                    return null;
                }
                // Создаем пилота, при успешной проверке и добавляем его в список рабочих 
                // + меняем у него WorkID
                return new Pilot(client);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Transporteur.WorkFactory.CreateWorker(): {ex.ToString()}");
                return null;
            }
        }

        public override AbstractVehicle CreateVehicle(AbstractWorker abstractWorker)
        {
            try
            {
                if (!(abstractWorker is Pilot pilot)) return null;

                SpawnPoint randomCarLocation = Work.CargobobSpawns.Where(item => !item.IsOccupied).GetRandomElement();

                if (randomCarLocation == null)
                {
                    // Извините, но сейчас все вертолеты заняты. Попробуйте позже!
                    abstractWorker.SendMessage("Transporteur_4", 9000);
                    return null;
                }

                Cargobob randomSpawnedCar = new Cargobob(pilot, randomCarLocation);
                return randomSpawnedCar;
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Transporteur.WorkFactory.CreateVehicle(): {ex.ToString()}");
                return null;
            }
        }
    }
}
