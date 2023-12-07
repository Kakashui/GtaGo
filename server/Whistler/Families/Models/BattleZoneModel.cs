using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.Families.Models
{
    class BattleZoneModel
    {
        public BattleZoneModel(Vector3 position, int range, bool enable, int blipType, byte blipColor, uint dimension)
        {
            Position = position;
            Range = range;
            Enable = enable;
            BlipType = blipType;
            BlipColor = blipColor;
            Dimension = dimension;
        }

        public Vector3 Position { get; set; }
        public int Range { get; set; }
        public bool Enable { get; set; }
        public int BlipType { get; set; }
        public byte BlipColor { get; set; }
        public uint Dimension { get; set; }
    }
}
