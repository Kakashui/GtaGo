using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.ParkingSystem
{
    class PosAndRot
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public PosAndRot(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}
