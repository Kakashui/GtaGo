using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Fishing.Models
{
    class FishingSpot
    {
        public FishingSpot(int id, Vector3 pos, Vector3 rot, bool inSea = false)
        {
            Id = id;
            Position = pos;
            Rotation = rot;
            InSea = inSea;

        }
        public FishingSpot(Vector3 pos, Vector3 rot, bool inSea = false)
        {
            Position = pos;
            Rotation = rot;
            InSea = inSea;
        }
        public int Id { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public bool InSea { get; set; }
    }
}
