using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Helpers;
using Whistler.Entities;
using Whistler.GUI;

namespace Whistler.Jobs.Technician
{
    class Work : Script
    {
        #region Settings

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Work));
        public static List<Technician> Workers { get; set; } = new List<Technician>();
        public static List<WorkStation> WorkStations { get; set; } = new List<WorkStation>();
        public static string SkinName { get; set; } = "Dockwork01SMM";
        public static int WorkSendInterval { get; set; } = 1 * 60 * 1000;
        public static Vector3 StartEndWorkPoint { get; set; } = new Vector3(2754.504, 3480.337, 54.52693);
        public static Vector3 ShopColShape { get; set; } = new Vector3(2748.272, 3472.617, 54.55492);
        public enum JobStage { ORDER_SEARCH, DIAGNOSTIC, SHOP, REPAIR };
        public static int MinLVL { get; set; } = 1;
        public static int WorkID { get; set; } = 15;
        public static int WorkGetTimerMinutes { get; set; } = 5;
        /// <summary>
        /// Коэффициент прибыли, привязанный к расстоянию
        /// На быстрой машине получается приблизительно 22800, если точки легкие, без сложных гор
        /// На медленной машине получается приблизительно 12000, если точки легкие, без сложных гор
        /// 
        /// Скорее всего, в среднем, если машина едет хотя бы 250 км/ч, можно получать около 19000-20000
        /// </summary>
        public static double PriceByDistance { get; set; } = 0.3;

        [ServerEvent(Event.PlayerDisconnected)]
        public static void onPlayerDisconnected(ExtPlayer player, DisconnectionType type, string reason)
        {
            try
            {
                // Если это не рабочий - прерываем событие
                Technician worker = Workers.Find(e => e.Player == player);
                if (worker == null) return;

                Workers.Remove(worker);
            }

            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Work.onPlayerDisconnected: {ex.ToString()}");
            }

        }

        [ServerEvent(Event.ResourceStart)]
        public void onResourceStartHandler()
        {
            try
            {
                NAPI.Blip.CreateBlip(620, StartEndWorkPoint, 1.5f, 60, Main.StringToU16("Technician Work"), 255, 2, true);

                //Start Work ColShape
                // Точка создается для всех - люди начинают/заканчивают рабочий день

                InteractShape.Create(StartEndWorkPoint, 1.5f, 2)
                    .AddInteraction(InitWorkDay);

                //Shop ColShape

                InteractShape.Create(ShopColShape, 1.5f, 2)
                    .AddInteraction((player) =>
                    {
                        if (player.IsInVehicle) return;
                        Technician worker = Work.Workers.Find(e => e?.Player == player);
                        // Если он не находится на стадии квеста - магазин
                        if (worker == null || !worker.JobStage.Equals(JobStage.SHOP) || !worker.CheckAccessNextAction()) return;

                        worker.OpenShop();
                    });

                #region Init Work Stations

                string workStationName = "Hill Vinewood";
                Vector3 diagnosticPoint = new Vector3(766.1343, 1273.833, 359.1766);//new Vector3(0, 0, 87.77041)
                List<Vector3> repairPoints = new List<Vector3>()
                {
	                new Vector3(760.35, 1281.1, 360.1766),//new Vector3(0, 0, 179.9034)
	                new Vector3(739.2563, 1275.947, 359.1766),//new Vector3(0, 0, 179.5239)
	                new Vector3(723.166, 1273.292, 359.1762),//new Vector3(0, 0, 152.0881)
	                new Vector3(713.5675, 1285.648, 359.1762),//new Vector3(0, 0, 148.7009)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Fire Department";
                diagnosticPoint = new Vector3(-368.1509, 6105.925, 38.34237);//new Vector3(0, 0, 200.3411)
                repairPoints = new List<Vector3>()
                {
                    new Vector3(-375.9403, 6111.786, 34.31964),//new Vector3(0, 0, 233.0909)
	                new Vector3(-368.9323, 6102.283, 34.31964), //new Vector3(0, 0, 325.032)
                    new Vector3(-360.6685, 6103.7, 30.37485),//new Vector3(0, 0, 35.35978)
	                new Vector3(-380.0432, 6110.505, 30.37283),//new Vector3(0, 0, 321.5366)
	                new Vector3(-369.3036, 6099.271, 30.37307),//new Vector3(0, 0, 315.1881)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Sandy Shores";
                diagnosticPoint = new Vector3(995.9535, 3582.677, 32.55095);//new Vector3(0, 0, 254.8283)
                repairPoints = new List<Vector3>()
                {
                    new Vector3(998.0602, 3575.639, 32.58849),//new Vector3(0, 0, 146.9766)
	                new Vector3(970.6295, 3607.641, 31.78758),//new Vector3(0, 0, 0.9525667)
	                new Vector3(997.0974, 3575.24, 33.49134),//new Vector3(0, 0, 228.386)
	                //new Vector3(987.5136, 3579.429, 37.44641),//new Vector3(0, 0, 45.96787)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Merryweather";
                diagnosticPoint = new Vector3(1407.686, 2118.777, 103.9221);//new Vector3(0, 0, 184.2876)
                repairPoints = new List<Vector3>()
                {
                    new Vector3(1404.209, 2122.933, 103.9397),//new Vector3(0, 0, 5.542112)
	                new Vector3(1401.035, 2123.115, 103.9053),//new Vector3(0, 0, 8.485661)
	                new Vector3(1397.98, 2117.414, 103.8972),//new Vector3(0, 0, 128.6739)

                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Oil pump";
                diagnosticPoint = new Vector3(603.0462, 2784.231, 41.17879);//new Vector3(0, 0, 156.585)
                repairPoints = new List<Vector3>()
                {
                    //new Vector3(541.7751, 2870.297, 50.90854),//new Vector3(0, 0, 61.18491)
	                new Vector3(568.9337, 2798.659, 40.8983),//new Vector3(0, 0, 96.72485)
	                new Vector3(636.6075, 2807.054, 40.91185),//new Vector3(0, 0, 186.494)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Stoner Cement Works";
                diagnosticPoint = new Vector3(306.1676, 2857.451, 48.70316);//new Vector3(0, 0, 169.9197)
                repairPoints = new List<Vector3>()
                {
                    //new Vector3(268.2276, 2862.193, 64.84898),//new Vector3(0, 0, 42.81493)
	                new Vector3(266.4867, 2871.437, 42.49012),//new Vector3(0, 0, 233.4591)
	                new Vector3(287.748, 2843.751, 43.58414),//new Vector3(0, 0, 296.5518)
	                new Vector3(306.5525, 2862.507, 43.47621),//new Vector3(0, 0, 168.2667)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Satellite dish";
                diagnosticPoint = new Vector3(2045.559, 2944.593, 49.90215);//new Vector3(0, 0, 346.9205)
                repairPoints = new List<Vector3>()
                {
                    new Vector3(2049.665, 2945.838, 56.39732),//new Vector3(0, 0, 242.1163)
	                new Vector3(2044.127, 2944.846, 58.90338),//new Vector3(0, 0, 352.5515)
	                new Vector3(2061.07, 2954.48, 46.13139),//new Vector3(0, 0, 270.5103)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Wind Power Station";
                //diagnosticPoint = new Vector3(2136.95, 1934.757, 92.81229);//new Vector3(0, 0, 265.9474)
                diagnosticPoint = new Vector3(2125.189, 2369.423, 97.94868);
                repairPoints = new List<Vector3>()
                {
                    new Vector3(1988.386, 2197.039, 103.9626),//new Vector3(0, 0, 359.1062)
                    new Vector3(2000.602, 1927.121, 91.32063),//new Vector3(0, 0, 6.590328)
	                new Vector3(2044.063, 2116.505, 92.37817),//new Vector3(0, 0, 7.018063)
	                new Vector3(2116.515, 2397.293, 99.63172),//new Vector3(0, 0, 2.804034)
	                new Vector3(2191.637, 2094.244, 126.5431),//new Vector3(0, 0, 357.3745)
	                new Vector3(2200.383, 2485.877, 87.26723),//new Vector3(0, 0, 359.9133)
	                new Vector3(2200.715, 1490.589, 81.55099),//new Vector3(0, 0, 0.08617856)
	                new Vector3(2236.593, 2042.563, 129.7011),//new Vector3(0, 0, 6.08593)
	                new Vector3(2279.995, 1567.431, 64.68876),//new Vector3(0, 0, 353.5428)
	                new Vector3(2280.472, 1408.027, 73.86548),//new Vector3(0, 0, 359.9306)
	                new Vector3(2301.204, 1852.462, 105.8787),//new Vector3(0, 0, 4.769694)
	                new Vector3(2318.61, 1608.13, 56.82455),//new Vector3(0, 0, 0.2647737)
	                new Vector3(2368.992, 2182.988, 101.8558),//new Vector3(0, 0, 357.7012)
	                new Vector3(2396.598, 2031.277, 90.21875),//new Vector3(0, 0, 358.7142)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Fowl Factory";
                diagnosticPoint = new Vector3(-76.34041, 6256.05, 29.97002);//new Vector3(0, 0, 123.3372)
                repairPoints = new List<Vector3>()
                {
                    new Vector3(-85.29607, 6238.498, 29.97033),//new Vector3(0, 0, 14.92581)
	                new Vector3(-73.2002, 6224.363, 29.96985),//new Vector3(0, 0, 305.9213)
	                new Vector3(-98.31669, 6220.286, 29.90505),//new Vector3(0, 0, 36.40879)
	                new Vector3(-147.319, 6163.647, 30.08619),//new Vector3(0, 0, 48.27267)
	                new Vector3(-88.61233, 6231.4, 29.96989),//new Vector3(0, 0, 305.224)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Power Line";
                diagnosticPoint = new Vector3(2602.708, 5038.672, 43.76349);//new Vector3(0, 0, 196.7089)
                repairPoints = new List<Vector3>()
                {
                    new Vector3(2589.729, 5057.067, 43.79933),//new Vector3(0, 0, 20.80326)
	                new Vector3(2595.366, 5059.321, 43.78946),//new Vector3(0, 0, 18.48907)
	                new Vector3(2591.222, 5063.667, 43.79933),//new Vector3(0, 0, 284.9682)
	                new Vector3(2584.647, 5061.542, 43.79933),//new Vector3(0, 0, 291.4258)
	                new Vector3(2585.886, 5065.452, 43.79933),//new Vector3(0, 0, 195.0988)
	                new Vector3(2591.997, 5067.344, 43.79933),//new Vector3(0, 0, 198.2303)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Levee Bottom";
                diagnosticPoint = new Vector3(1928.425, 592.7147, 174.5467);//new Vector3(0, 0, 63.64476)
                repairPoints = new List<Vector3>()
                {
                    new Vector3(1930.218, 597.4164, 174.5901),//new Vector3(0, 0, 67.93882)
                    new Vector3(1932.507, 602.0151, 174.7766),//new Vector3(0, 0, 63.59257)
                    new Vector3(1917.526, 586.0858, 175.2474),//new Vector3(0, 0, 63.64278)
                    new Vector3(1923.805, 601.847, 180.2483),//new Vector3(0, 0, 67.05892)
                    new Vector3(1924.492, 595.8527, 180.2483)//new Vector3(0, 0, 186.6609)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                //workStationName = "Levee Top";
                //diagnosticPoint = new Vector3(1663.181, -24.89683, 172.6547);//new Vector3(0, 0, 12.8359)
                //repairPoints = new List<Vector3>()
                //{
                //    new Vector3(1664.904, -25.10931, 181.6496),//new Vector3(0, 0, 199.4848)
                //    new Vector3(1665.94, -44.91439, 167.1925),//new Vector3(0, 0, 290.2168)
                //    new Vector3(1667.985, -26.62472, 183.6491),//new Vector3(0, 0, 284.7618)
                //    new Vector3(1662.26, -57.75267, 179.0446)//new Vector3(0, 0, 255.8859)
                //};
                //WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Grand Senora Tower";
                diagnosticPoint = new Vector3(2320.223, 2948.077, 46.03909);//new Vector3(0, 0, 262.1349)
                repairPoints = new List<Vector3>()
                {
                    new Vector3(2329.428, 2952.939, 46.03909),//new Vector3(0, 0, 93.95486)
                    new Vector3(2328.943, 2949.511, 46.03909)//new Vector3(0, 0, 87.85422)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Calafia Road";
                diagnosticPoint = new Vector3(-198.9923, 3653.436, 50.61248);//new Vector3(0, 0, 184.0637)
                repairPoints = new List<Vector3>()
                {
                    new Vector3(-222.7579, 3657.751, 63.29826),//new Vector3(0, 0, 260.5631)
                    new Vector3(-204.4477, 3636.1, 63.32484),//new Vector3(0, 0, 88.73292)
                    new Vector3(-216.4853, 3676.57, 50.63247)//new Vector3(0, 0, 123.2288)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                //workStationName = "Mount Josiah Railway";
                //diagnosticPoint = new Vector3(-425.1871, 4013.789, 80.49154);//new Vector3(0, 0, 280.8764)
                //repairPoints = new List<Vector3>()
                //{
                //    new Vector3(-425.0139, 3984.893, 80),//new Vector3(0, 0, 280.8764)
                //    new Vector3(-432.4097, 4015.435, 88.122),//new Vector3(0, 0, 192.8792),
                //    new Vector3(-417.2795, 4006.477, 80.15009)//new Vector3(0, 0, 8.868973)
                //};
                //WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                //workStationName = "Senora Highway Railway 1";
                //diagnosticPoint = new Vector3(1617.106, 6356.816, 39.5031);//new Vector3(0, 0, 155.9577)
                //repairPoints = new List<Vector3>()
                //{
                //    new Vector3(1619.003, 6355.739, 39.60103),//new Vector3(0, 0, 150.5853)
                //    new Vector3(1619.736, 6357.535, 47.23697)//new Vector3(0, 0, 248.793)
                //};
                //WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                //workStationName = "Senora Highway Railway 2";
                //diagnosticPoint = new Vector3(2489.513, 5729.504, 59.36031);//new Vector3(0, 0, 120.0716)
                //repairPoints = new List<Vector3>()
                //{
                //    new Vector3(2492.118, 5726.159, 59.51712),//new Vector3(0, 0, 120.0716)
                //    new Vector3(2497.068, 5731.33, 66.8606)//new Vector3(0, 0, 209.8256)
                //};
                //WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                //workStationName = "Mount Gordo";
                //diagnosticPoint = new Vector3(2793.809, 5995.838, 355.1485);//new Vector3(0, 0, 82.09976)
                //repairPoints = new List<Vector3>()
                //{
                //    new Vector3(2797.634, 5991.444, 354.2474),//new Vector3(0, 0, 34.79764)
                //    new Vector3(2807.972, 5976.924, 349.5832)//new Vector3(0, 0, 314.0712)
                //};
                //WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Great Ocean";
                diagnosticPoint = new Vector3(1359.507, 6396.368, 32.29013);//new Vector3(0, 0, 105.9097)
                repairPoints = new List<Vector3>()
                {
                    new Vector3(1343.72, 6381.989, 32.29012),//new Vector3(0, 0, 86.75952)
                    new Vector3(1352.405, 6380.466, 32.08919),//new Vector3(0, 0, 258.9829)
                    new Vector3(1352.275, 6386.853, 32.08919),//new Vector3(0, 0, 261.0803)
                    new Vector3(1343.825, 6388.059, 32.29015)//new Vector3(0, 0, 83.30291)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                //workStationName = "Mount Chiliad";
                //diagnosticPoint = new Vector3(455.9885, 5571.762, 780.0645);//new Vector3(0, 0, 267.8196)
                //repairPoints = new List<Vector3>()
                //{
                //    new Vector3(450.0552, 5566.408, 795.2438),//new Vector3(0, 0, 270.2075)
                //    new Vector3(451.9846, 5580.032, 795.231)//new Vector3(0, 0, 13.33764)
                //};
                //WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                //workStationName = "Grapeseed";
                //diagnosticPoint = new Vector3(2137.105, 4766.858, 40.34821);//new Vector3(0, 0, 37.89338)
                //repairPoints = new List<Vector3>()
                //{
                //    new Vector3(2120.879, 4816.529, 40.1417),//new Vector3(0, 0, 76.5775)
                //    new Vector3(2050.569, 4755.602, 39.91669),//new Vector3(0, 0, 203.932)
                //    new Vector3(1918.921, 4708.588, 40.07332)//new Vector3(0, 0, 109.0714)
                //};
                //WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Paleto Bay";
                diagnosticPoint = new Vector3(-1583.27, 5191.472, 2.864216);//new Vector3(0, 0, 146.1905)
                repairPoints = new List<Vector3>()
                {
                    new Vector3(-1585.814, 5211.215, 2.858171),//new Vector3(0, 0, 298.2768)
                    new Vector3(-1603.064, 5235.094, 2.854102),//new Vector3(0, 0, 115.0307)
                    new Vector3(-1609.392, 5262.308, 2.879313)//new Vector3(0, 0, 295.4035)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Route 68 Church";
                diagnosticPoint = new Vector3(-324.5564, 2818.241, 58.32983);//new Vector3(0, 0, 233.4089)
                repairPoints = new List<Vector3>()
                {
                    new Vector3(-293.5266, 2784.73, 63.37751)//new Vector3(0, 0, 243.4471)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Quarry";
                diagnosticPoint = new Vector3(2834.347, 2806.709, 56.28305);//new Vector3(0, 0, 235.3652)
                repairPoints = new List<Vector3>()
                {
                    new Vector3(2833.833, 2803.255, 59.40245),//new Vector3(0, 0, 4.150116)
                    new Vector3(2760.048, 2799.161, 37.50214),//new Vector3(0, 0, 115.9521)
                    new Vector3(2654.9, 2801.199, 33.04512)//new Vector3(0, 0, 347.9854)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Sawmill";
                diagnosticPoint = new Vector3(-552.9003, 5348.615, 73.62341);//new Vector3(0, 0, 247.4557)
                repairPoints = new List<Vector3>()
                {
                    new Vector3(-545.0904, 5353.17, 72.46953),//new Vector3(0, 0, 226.4134)
                    //new Vector3(-557.412, 5308.653, 81.97986),//new Vector3(0, 0, 73.66793)
                    new Vector3(-564.9139, 5283.036, 69.13063),//new Vector3(0, 0, 330.6429)
                    new Vector3(-502.2169, 5321.89, 79.11984),//new Vector3(0, 0, 71.26393)
                    new Vector3(-573.4576, 5309.052, 69.1402)//new Vector3(0, 0, 250.4586)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                workStationName = "Reserve";
                diagnosticPoint = new Vector3(-999.6207, 4852.63, 273.4861);//new Vector3(0, 0, 333.0378)
                repairPoints = new List<Vector3>()
                {
                    new Vector3(-1064.823, 4913.694, 211.5764),//new Vector3(0, 0, 197.6124)
                    new Vector3(-1099.742, 4909.751, 214.7236),//new Vector3(0, 0, 4.729312)
                    new Vector3(-1133.623, 4902.394, 218.2424),//new Vector3(0, 0, 335.3311)
                    new Vector3(-1156.658, 4930.005, 221.4862),//new Vector3(0, 0, 35.94315)
                };
                WorkStations.Add(new WorkStation(workStationName, diagnosticPoint, repairPoints));

                #endregion
            }
            catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
        }
        #endregion

        #region Events

        [RemoteEvent("WORK::TECHNICIAN::DIAGNOSTIC::RESULT::CLIENT")]
        public static void MiniGameFinished(ExtPlayer player, bool isSuccess)
        {
            // Если такого игрока нет на сервере - прерываем
            if (!player.IsLogged()) return;

            // Если это не рабочий - прерываем событие
            Technician worker = Workers.Find(e => e.Player == player);
            if (worker == null) return;

            switch (worker.JobStage)
            {
                case JobStage.DIAGNOSTIC:
                    worker.EndDiagnostic(isSuccess);
                    break;
                case JobStage.REPAIR:
                    worker.EndRepair(isSuccess);
                    break;
            }
        }


        #endregion

        #region Methods

        public static void InitWorkDay(ExtPlayer client)
        {
            try
            {
                if (client.IsInVehicle) return;

                if (client.Character.LVL < 2)
                {
                    Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Для этой работы нужно быть как минимум 2м уровнем.", 3000);
                    return;
                }

                // Устраиваемся на работу
                if (!Workers.Exists(e => e.Player == client))
                {
                    #region Check Player Work and LVL

                    // Если игрок меньше третьего уровня
                    if (client.Character.LVL < MinLVL)
                    {
                        Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, $"Jobs_73".Translate(MinLVL), 3000);
                        return;
                    }
                    // Если игрок на другой работе
                    if (client.Character.WorkID != 0 && client.Character.WorkID != Work.WorkID)
                    {
                        Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, $"Jobs_64".Translate(), 3000);
                        return;
                    }
                    // Если игрок недавно увольнялся с работы
                    if (client.HasData("TECHNICIAN::WORK::LEAVE::RECENTLY") && client.GetData<bool>("TECHNICIAN::WORK::LEAVE::RECENTLY"))
                    {
                        //На работу техника можно устроиться раз в {WorkGetTimerMinutes} минут
                        Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Technician_10".Translate( WorkGetTimerMinutes), 3000);
                        return;
                    }

                    #endregion

                    Technician worker = new Technician(client);
                    Workers.Add(worker);
                    //Вы начали рабочий день техника, ожидайте поступления заказов;
                    worker.SendMessage("Technician_1".Translate(), 5000);
                    client.Character.WorkID = WorkID;
                    MainMenu.SendStats(client);
                    worker.GiveWork();
                }
                // Оканчиваем работу
                else
                {
                    Technician worker = Workers.Find(e => e.Player == client);
                    Workers.Remove(worker);
                    //Вы закончили рабочий день техника
                    worker.SendMessage("Technician_11".Translate(), 5000);
                    worker.DeleteWaypoint();
                    client.Character.WorkID = 0;
                    MainMenu.SendStats(client);

                    // Игрок окончил работу, сохраняем состояние
                    SafeTrigger.SetData(client, "TECHNICIAN::WORK::LEAVE::RECENTLY", true);
                    // Даем возможность устроиться на работу через указанное в настройках время
                    NAPI.Task.Run(() =>
                    {
                        // Даем возможность устроиться
                        SafeTrigger.SetData(client, "TECHNICIAN::WORK::LEAVE::RECENTLY", false);
                    }, 60 * 1000 * WorkGetTimerMinutes);
                }
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Work.InitWorkDay(ExtPlayer client): {ex.ToString()}");
            }
        }

        #endregion

        #region Debug Commands

        [Command("techworkers")]
        public static void CMD_GetLobbiesActive(ExtPlayer player)
        {
            if (player.Character.AdminLVL == 0) return;
            try
            {
                string message = "";
                Work.Workers.ForEach(e => message += e);
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, message, 15000);
            }
            catch (Exception ex)
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, ex.ToString(), 10000);
            }
        }

        #endregion
    }
}
