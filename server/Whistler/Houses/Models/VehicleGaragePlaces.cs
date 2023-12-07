using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.Houses.Models
{
    class VehicleGaragePlaces
    {
        public Vehicle Vehicle { get; set; }
        public int Place { get; set; }
        public VehicleGaragePlaces(Vehicle vehicle, int place)
        {
            Vehicle = vehicle;
            Place = place;
        }

    }
}
