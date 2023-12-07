using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.SDK.CustomColShape;

namespace Whistler.Core.SafeZone
{
    class SafeZoneManager : Script
    {
        
        private static Dictionary<int, SafeZoneModel> _safezones = new Dictionary<int, SafeZoneModel>();
        private static int zoneIndex = 1;
        private static List<Zone> _zones = new List<Zone>
        {
            new Zone(true, new Vector3(429.2, -437.6, 48.45614), new Vector3(197.4, -362.4, 47.94752), new Vector3(89, -555.5, 56), new Vector3(307.6072, -752.9468, 28.19113)), //EMS
            new Zone(true, new Vector3(307.6072, -752.9468, 28.19113), new Vector3(89, -555.5, 56), new Vector3(8.2, -779.8, 50.33475), new Vector3(263, -874.7, 41.89381), 0), //FIB
            new Zone(true, new Vector3(-428.092, -261.3784, 37.10266), new Vector3(-557.6434, -316.6519, 36.75684), new Vector3(-642.7313, -174.9576, 39.05326), new Vector3(-515.499, -120.9552, 40.09607), 0), //gov
            new Zone(true, new Vector3(1184.164, 303.4999, -52.1557), new Vector3(1184.164, 203.4999, -52.1557), new Vector3(1084.164, 203.4999, -52.1557), new Vector3(1084.164, 303.4999, -52.1557), 0), //casino
            new Zone(true, new Vector3(735.7003, 40.43241, 83.64057), new Vector3(610.2786, -131.7605, 69.92729), new Vector3(418.9232, -61.42164, 72.97512), new Vector3(512.1642, 111.9798, 96.4117), 0), //PD new
            //new Zone(true, new Vector3(536.174, -1121.348, 27.5319), new Vector3(342.4241, -1120.379, 28.27608), new Vector3(348.1317, -868.9663, 28.17158), new Vector3(537.8769, -870.4906, 24.09285)), //PD old
            new Zone(false, new Vector3(-1341.887, 825.1432, 213.7543), new Vector3(-1634.402, 635.9006, 156.9128), new Vector3(-1703.801, 815.9882, 153.6941), new Vector3(-1463.69, 1079.351, 212.3449), 0), //house 1
            new Zone(false, new Vector3(-1448.661, 38.72265, 52.55526), new Vector3(-1411.188, 206.0596, 58.41579), new Vector3(-1472.529, 266.1609, 61.55119), new Vector3(-1692.036, 93.1953, 64.77156), 0), //house 2
            new Zone(false, new Vector3(-1971.503, 2139.177, 138.5854), new Vector3(-1789.256, 2116.115, 131.4997), new Vector3(-1836.87, 1926.604, 176.3446), new Vector3(-2008.12, 1943.159, 186.4439), 0), //house 3
            new Zone(false, new Vector3(1297.429, 1212.818, 144.5305), new Vector3(1315.781, 1017.616, 117.4001), new Vector3(1537.513, 1002.891, 132.5319), new Vector3(1540.543, 1253.051, 127.1582), 0), //house 4
            new Zone(false, new Vector3(-1558.802, -157.0594, 77.43466), new Vector3(-1618.542, -71.22855, 66.3698), new Vector3(-1531.574, -34.98904, 90.56836), new Vector3(-1484.009, -94.86549, 59.21218), 0), //house 5
            new Zone(true, new Vector3(-1668.739, -1177.323, 17.98494), new Vector3(-1701.982, -1557.63, 14.74421), new Vector3(-2238.838, -1059.783, 21.00459), new Vector3(-1823.752, -1047.83, 20.4893), 0), //рыбалка
            new Zone(true, new Vector3(-643.2515, -2331.303, 19.51575), new Vector3(-793.9347, -2558.765, 18.20582), new Vector3(-878.0044, -2339.824, 25.74762), new Vector3(-767.9341, -2219.365, 19.68346), 0), //royal battle register
            new Zone(true, new Vector3(-681.9545, -1293.042, 17.12334), new Vector3(-856.5477, -1510.163, 10.4117), new Vector3(-920.9812, -1337.03, 10.583), new Vector3(-771.5197, -1176.754, 16.2254), 0), //autoschool
            new Zone(true, new Vector3(-242.0961, -2665.122, 6.00026), new Vector3(-281.459, -2706.352, 6.000264), new Vector3(-309.6744, -2680.203, 6.017814), new Vector3(-271.2957, -2641.892, 6.003123),  0), //weapon factory
            new Zone(true, new Vector3(-1654.9, -1005.306, 24.69979), new Vector3(-1753.81, -920.5529, 16.60217), new Vector3(-1637.208, -785.577, 18.32449), new Vector3(-1540.68, -866.0859, 17.85594),  0), //trade vehicle 1 
            new Zone(true, new Vector3(-2381.32, 304.8426, 181.385), new Vector3(-2348.066, 223.6838, 176.0626), new Vector3(-2273.953, 246.4202, 181.5843), new Vector3(-2320.882, 328.0229, 179.1149),  0), //trade vehicle 2
            new Zone(true, new Vector3(1436.089, 3739.989, 40.90155), new Vector3(1545.601, 3806.58, 49.84551), new Vector3(1573.584, 3783.736, 41.16996), new Vector3(1431.074, 3705.542, 39.16205),  0), //trade vehicle 3
            new Zone(true, new Vector3(150.1647, 6552.499, 37.25375), new Vector3(109.1522, 6609.918, 35.93734), new Vector3(193.1867, 6686.713, 47.13601), new Vector3(224.5817, 6585.793, 37.94696),  0), //trade vehicle 4
            new Zone(true, new Vector3(1569.2273, 2343.2722, 67.86336), new Vector3(1473.6167, 2713.3167, 38.140095), new Vector3(1844.3884, 2926.0483, 45.161777), new Vector3(1814.375, 2340.483, 56.211132),  0), //prison  
            new Zone(true, new Vector3(-3003.9778, -18.578375, 1.4637227), new Vector3(-2956.1292, 74.934944, 12.63215), new Vector3(-3052.1213, 151.56902, 11.572874), new Vector3(-3128.9075, 111.845215, -0.897361),  0), //spawn
            new Zone(true, new Vector3(-868.1797, -2052.275, 9.049984), new Vector3(-961.5772, -2144.1768, 8.684985), new Vector3(-1026.3741, -2077.8894, 13.637493), new Vector3(-932.21783, -1987.164, 11.52261),  0), //автошкола
            new Zone(true, new Vector3(-130.36829, -691.2988, 34.694885), new Vector3(-82.37065, -558.7032, 38.590466), new Vector3(-199.74063, -533.3818, 34.5872), new Vector3(-241.95912, -651.5749, 33.19464),  0), //автосалон
        };

        private static void CreateNewSafeZone(Zone zone)
        {
            int id = zoneIndex++;
            var colShape = ShapeManager.CreateQuadColShape(zone.Point1, zone.Point2, zone.Point3, zone.Point4, zone.Dimension);
            _safezones.Add(id, new SafeZoneModel(id, colShape, zone.Active));
        }

        [ServerEvent(Event.ResourceStart)]
        public void Event_onResourceStart()
        {
            foreach (var item in _zones)
            {
                CreateNewSafeZone(item);
            }
            Main.OnPlayerReady += PlayerReady;
        }

        private static void PlayerReady(ExtPlayer player)
        {
            var offZones =  _safezones.Where(item => !item.Value.Active).Select(item => item.Key).ToList();
            SafeTrigger.ClientEvent(player, "safeZones:setInActiveZones", offZones);
        }

        [Command("safezonechange")]
        public void CMD_safezonechange(ExtPlayer player, int id)
        {
            if (!player.IsLogged())
                return;
            if (!Group.CanUseAdminCommand(player, "safezonechange"))
                return;
            if (!_safezones.ContainsKey(id))
                return;
            _safezones[id].SetActive(!_safezones[id].Active);
            if (_safezones[id].Active)
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы включили зеленую зону #{id}", 3000);
            else
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы отключили зеленую зону #{id}", 3000);
            GameLog.Admin(player.Name, $"saveZone{(_safezones[id].Active ? "On" : "Off")}", "");
        }

        class Zone
        {
            public Vector3 Point1 { get; }
            public Vector3 Point2 { get; }
            public Vector3 Point3 { get; }
            public Vector3 Point4 { get; }
            public uint Dimension { get; }
            public bool Active { get; }
            public Zone(bool active, Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4, uint dimension = uint.MaxValue)
            {
                Active = active;
                Point1 = point1;
                Point2 = point2;
                Point3 = point3;
                Point4 = point4;
                Dimension = dimension;
            }
        }
    }
}
