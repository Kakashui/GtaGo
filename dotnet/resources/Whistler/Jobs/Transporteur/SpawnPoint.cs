using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Jobs.Transporteur
{
    class SpawnPoint
    {
        public SpawnPoint(Vector3 position, float rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public Vector3 Position { get; set; }
        public float Rotation { get; set; }
        // Занята ли точка
        public bool IsOccupied { get; set; } = false;
    }
}
