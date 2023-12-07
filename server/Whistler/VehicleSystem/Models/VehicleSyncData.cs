using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.VehicleSystem.Models
{
    class VehicleSyncData
    {        //This is the data object which will be synced to vehicles
        public bool Locked { get; set; } = false;
        public bool Engine { get; set; } = false;
        public int TurnSignal { get; set; } = 0;
        public float Dirt { get; set; } = 0.0f;
        public bool IsFreezed { get; set; } = false;
        public int DoorState { get; set; } = 0;
    }
}
