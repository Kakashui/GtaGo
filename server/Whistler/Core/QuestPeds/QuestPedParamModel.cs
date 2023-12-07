using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.Core.QuestPeds
{
    class QuestPedParamModel
    {

        public string Name { get; }

        public PedHash Hash { get; }

        public Vector3 Position { get; }

        public float Heading { get; }
        public string Role { get; }

        public uint Dimension { get; }
        public int Range { get; }
        public QuestPedParamModel(PedHash hash, Vector3 position, string name, string role, float heading, uint dimension = 0, int range = 1)
        {
            Name = name;
            Hash = hash;
            Position = position;
            Heading = heading;
            Dimension = dimension;
            Role = role;
            Range = range;
        }
    }
}
