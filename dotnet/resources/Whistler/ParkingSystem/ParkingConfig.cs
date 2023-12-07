using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.ParkingSystem
{
    class ParkingConfig
    {
        public Vector3 BlipPosition { get; set; }
        public List<PosAndRot> Position { get; set; }
        public ParkingConfig(Vector3 blipPosition, List<PosAndRot> position)
        {
            BlipPosition = blipPosition;
            Position = position;
        }
    }
}
