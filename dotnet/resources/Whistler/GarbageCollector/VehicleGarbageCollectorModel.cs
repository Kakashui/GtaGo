using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.VehicleSystem.Models;

namespace Whistler.GarbageCollector
{
    public delegate void RespawnCarDelegate(ExtVehicle vehicle);
    class VehicleGarbageCollectorModel
    {
        public VehicleGarbageCollectorModel(ExtVehicle vehicle, int minutes, RespawnCarDelegate callback)
        {
            Vehicle = vehicle;
            Expiried = DateTime.Now.AddMinutes(minutes);
            Callback = callback;
        }
        RespawnCarDelegate Callback { get; set; }
        public DateTime Expiried { get; set; }
        public ExtVehicle Vehicle { get; set; }

        public void RespawnCar()
        {
            Callback.Invoke(Vehicle);
        }

        internal void Update(int minutes)
        {
            Expiried = DateTime.Now.AddMinutes(minutes);
        }
    }
}
