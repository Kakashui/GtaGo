using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.MP.RoyalBattle.Models
{
    class VehiclePositionModel
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public VehiclePositionModel(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}
