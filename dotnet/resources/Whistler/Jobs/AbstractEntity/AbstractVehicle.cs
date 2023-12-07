using GTANetworkAPI;
using Whistler.Jobs.Transporteur;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.VehicleSystem.Models;

namespace Whistler.Jobs.AbstractEntity
{
    abstract class AbstractVehicle
    {
        public ExtVehicle Vehicle { get; set; }
        public Vector3 Position { get; set; }
        public float Rotation { get; set; }
        public AbstractWorker Owner { get; set; }
        public string Model { get; set; }
        public string Number { get; set; }
        public bool IsOccupied { get; set; } = false;
        public SpawnPoint CarLocation { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public string GenerateRandomCarNumer(int vehNumberLength, string dictionary)
        {
            try
            {
                StringBuilder resultStringBuilder = new StringBuilder();
                for (int i = 0; i < vehNumberLength; i++)
                {
                    resultStringBuilder.Append(dictionary[WorkManager.rnd.Next(dictionary.Length)]);
                }
                return resultStringBuilder.ToString();
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Car.GenerateRandomCarNumer(): {ex.ToString()}");
                return "";
            }
        }
    }
}
