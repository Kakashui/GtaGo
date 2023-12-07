using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.Fractions.GOV.Models
{
    class GovernmentLiftModel
    {        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public uint Dimension { get; set; }
        public int Access { get; set; }
        public GovernmentLiftModel(string name, Vector3 position, Vector3 rotation, uint dimension, int access)
        {
            Position = position;
            Rotation = rotation;
            Dimension = dimension;
            Access = access;
            Name = name;
        }
    }
}
