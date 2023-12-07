using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;

namespace Whistler.Core.Models
{
    public class VSession
    {
        public int Id { get; set; } = -1;
        public uint Dimension { get; set; } = 0;
        public Vector3 Position { get; set; } = new Vector3(0f, 0f, 0f);
        public uint ModelHash { get; set; } = 0;

        public DateTime? RespawnTime = null;
        public ExtPlayer RespawnResponsible = null;

        public VSession(uint modelHash)
        {
            ModelHash = modelHash;
        }

        public VSession(int id, uint dimension, uint modelHash)
        {
            Id = id;
            Dimension = dimension;
            ModelHash = modelHash;
        }
    }
}
