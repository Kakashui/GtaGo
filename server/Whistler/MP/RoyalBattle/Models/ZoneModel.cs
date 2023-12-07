using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.MP.RoyalBattle.Models
{
    class ZoneModel
    {
        public Vector3 Center { get; set; }
        public int Range { get; set; }
        public ZoneModel(Vector3 center, int range)
        {
            Center = center;
            Range = range;
        }
        public ZoneModel(ZoneModel zone)
        {
            Center = zone.Center;
            Range = zone.Range;
        }
    }
}
