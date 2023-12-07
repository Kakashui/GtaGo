using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.GarbageCollector
{
    public delegate void RespawnCarDelegate(Vehicle vehicle);
    class VehicleGarbageCollectorModel
    {
        public VehicleGarbageCollectorModel(Vehicle vehicle, int minutes, RespawnCarDelegate callback)
        {
            Vehicle = vehicle;
            Expiried = DateTime.Now.AddMinutes(minutes);
            Callback = callback;
        }
        RespawnCarDelegate Callback { get; set; }
        public DateTime Expiried { get; set; }
        public Vehicle Vehicle { get; set; }

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
