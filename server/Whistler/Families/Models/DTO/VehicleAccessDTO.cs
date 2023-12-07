using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.VehicleSystem;

namespace Whistler.Families.Models.DTO
{
    class VehicleAccessDTO
    {
        public int key { get; }
        public int access { get; }
        public VehicleAccessDTO(int key, int access)
        {
            this.key = key;
            this.access = access;
        }
    }
}
