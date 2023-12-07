using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.Fractions.TransportAlcohol.Models;
using Whistler.Helpers;

namespace Whistler.Fractions.TransportAlcohol
{
    class TransportManager : Script
    {
        public static Dictionary<int, Models.BarPoint> _bars = new Dictionary<int, Models.BarPoint>
        {
            [1] = new Models.BarPoint(1, new Vector3(1037.01, 195.7188, 80.85583)),
            [2] = new Models.BarPoint(2, new Vector3(-1395.039, -582.0796, 30.19641)),
            [3] = new Models.BarPoint(3, new Vector3(291.9547, 176.2909, 104.1258)),
            [4] = new Models.BarPoint(4, new Vector3(133.8899, -1307.52, 29.03347)),
            [5] = new Models.BarPoint(5, new Vector3(-430.8336, 252.432, 82.97105)),
            [6] = new Models.BarPoint(6, new Vector3(1993.273, 3057.484, 47.05683)),
        };
        public const int MoneyForMoto = 2000;
        public const int MoneyForAuto = 6000;
        private static GiveAlcoholPoint _givePoint;
        private static TakePoint _takePoint;
        private static Blip _blip;
        public static void InitPoints(List<Vector3> points)
        {
            int i = _bars.Max(item => item.Key) + 1;
            foreach (var point in points)
                if (point != null)
                {
                    _bars.Add(i, new Models.BarPoint(i, point));
                    i++;
                }
            _givePoint = new GiveAlcoholPoint(new Vector3(1017.057, -2525.144, 27.30197));
            _takePoint = new TakePoint(new Vector3(-4.725906, 6272.966, 30.25226));
            _blip = NAPI.Blip.CreateBlip(749, new Vector3(1017.057, -2525.144, 27.30197), 1, 25, Main.StringToU16("Байкеры"), 255, 0, true, 0, 0);
        }

        public static void GetWaipoint(ExtPlayer player, int id)
        {
            if (_bars.ContainsKey(id))
            {
                player.CreateWaypoint(_bars[id].Position);
                player.CreateClientMarker(1699, 27, _bars[id].Position, 5, 0, new Color(182, 211, 0, 200), new Vector3());
            }
            else
            {
                player.CreateWaypoint(_takePoint.Position);
                player.CreateClientMarker(1699, 27, _takePoint.Position, 5, 0, new Color(182, 211, 0, 200), new Vector3());
            }
            SDK.Notify.SendInfo(player, "frac:transp:alco_1");
        }
    }
}
