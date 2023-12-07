using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.DoorsSystem
{
    public class Door
    {
        public Door(uint hash, Vector3 position)
        {
            Position = position;
            Hash = (int)hash;
        }
        public Door(int hash, Vector3 position)
        {
            Position = position;
            Hash = hash;
        }
        public Door(string name, Vector3 position)
        {
            Position = position;
            Hash = (int)NAPI.Util.GetHashKey(name);
        }
        public Vector3 Position { get; set; }
        public int Hash { get; set; }
    }
}
