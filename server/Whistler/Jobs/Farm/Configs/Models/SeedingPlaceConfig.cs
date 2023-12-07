using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.Jobs.Farm.Configs.Models
{
    public class SeedingPlaceConfig
    {
        public int ID { get; set; }
        public Vector3 Position { get; set; }
        public PitNames PitType { get; set; }
    }
}
