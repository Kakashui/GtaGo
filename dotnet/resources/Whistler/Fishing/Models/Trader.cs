using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Fishing.Models
{
    class Trader
    {
        public Trader(string model, string name, string role, uint sprite, byte color, string bName, Vector3 pos, Vector3 rot)
        {
            Hash = (PedHash)NAPI.Util.GetHashKey(model);
            Name = name;
            Position = pos;
            Rotation = rot;
            BlipSprite = sprite;
            BlipColor = color;
            BlipName = bName;
            Role = role;
        }
        public Trader(PedHash hash, string name, string role, uint sprite, byte color, string bName, Vector3 pos, Vector3 rot)
        {
            Hash = hash;
            Name = name;
            BlipSprite = sprite;
            BlipColor = color;
            BlipName = bName;
            Position = pos;
            Rotation = rot;
            Role = role;
        }
        public string Name { get; set; }
        public string Role { get; set; }
        public uint BlipSprite { get; set; }
        public byte BlipColor { get; set; }
        public string BlipName { get; set; }
        public PedHash Hash { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
    }
}
