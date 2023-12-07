using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.VehicleSystem.Models;

namespace Whistler.Houses.Models
{
    class VehicleGaragePlaces
    {
        public ExtVehicle Vehicle { get; set; }
        public int Place { get; set; }
        public VehicleGaragePlaces(ExtVehicle vehicle, int place)
        {
            Vehicle = vehicle;
            Place = place;
        }

    }
}
