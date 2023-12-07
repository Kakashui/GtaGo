using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.Houses.Models
{
    class GarageCoordsModel
    {
        public GarageCoordsModel(Vector3 position, Vector3 outPosition, Vector3 robberyPos, List<Vector3> vehiclesPositions, List<Vector3> vehiclesRotations)
        {
            Position = position;
            OutPosition = outPosition;
            RobberyPos = robberyPos;
            VehiclesPositions = vehiclesPositions;
            VehiclesRotations = vehiclesRotations;
        }

        public Vector3 Position { get; }
        public Vector3 OutPosition { get; }
        public List<Vector3> VehiclesPositions { get; }
        public List<Vector3> VehiclesRotations { get; }
        public Vector3 RobberyPos { get; }

    }
}
