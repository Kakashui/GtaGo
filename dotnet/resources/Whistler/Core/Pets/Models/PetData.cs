using GTANetworkAPI;
using System.Collections.Generic;
using Whistler.Entities;

namespace Whistler.Core.Pets.Models
{
    public class PetData
    {
        public int OwnerUuid;
        public string Name;
        public uint Model;
        public Vector3 Position;
        public uint Dimension;
        public ExtPlayer Controller = null;
        public ExtPed Pet = null;
        public int Health = 100;
        public bool FreeRename = true;
        public Dictionary<string, string> Timers = new Dictionary<string, string>();
        public bool Dead
        {
            get
            {
                return Health <= 0;
            }
        }
        public PetConfig Config;

        public PetData(int ownerUuid, string name, uint model, int health, Vector3 position, uint dimension = 0, bool freeRename = true)
        {
            OwnerUuid = ownerUuid;
            Name = name;
            Model = model;
            Health = health;
            Position = position;
            Dimension = dimension;
            FreeRename = freeRename;
            Config = Pets.Config.PetsConfig.ContainsKey(model) ? Pets.Config.PetsConfig[model] : null;
        }
    }
}
