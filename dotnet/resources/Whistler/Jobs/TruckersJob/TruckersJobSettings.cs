using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Docks;
using Whistler.SDK;

namespace Whistler.Jobs.TruckersJob
{
    internal static class TruckersJobSettings
    {
        private static Dictionary<int, TruckerStage> _stages = new Dictionary<int, TruckerStage>();
        private static double _salaryIncrementCoefficient = 2;
        
        public static IReadOnlyDictionary<int, TruckerStage> Stages => _stages;
        
        private static Dictionary<int, TruckInfo> _trucks = new Dictionary<int, TruckInfo>();
        public static IReadOnlyDictionary<int, TruckInfo> Trucks => _trucks;
        private static (List<(Vector3, float)>, List<Vector3>) _trashPreset;
        private static (List<Vector3>, List<Vector3>) _constructionPreset;
        public static List<(Vector3, float)> DockLoadPoints;

        private static List<(Vector3, float)> _loadPointsFirstPart;// 3, 4, 5, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20
        private static List<Vector3> _unloadPointsFirstPart;

        private static List<(Vector3, float)> _loadPointsSecondPart;
        private static List<Vector3> _unloadPointsSecondPart;
        
        private static void LoadFirstPoints()// вызывать после инициализации портовых точек
        {
            _loadPointsFirstPart = new List<(Vector3, float)>
            {
                (new Vector3(-125.1457, 6216.647, 30.08143), 48),
                (new Vector3(-72.55775, 1907.603, 195.3946), 191),
                (new Vector3(-62.54316, 1904.287, 194.6666), 182),
                (new Vector3(-61.54913, 6276.924, 30.2107), 96),
                (new Vector3(-52.72404, 1874.994, 195.7637), 94),
                (new Vector3(14.11519, 6279.744, 30.11696), 119),
                (new Vector3(45.53838, 6298.027, 30.1163), 120),
                (new Vector3(494.8745, -1971.047, 23.80917), 0),
                (new Vector3(485.1688, -1997.99, 23.77276), 29),
                (new Vector3(569.9202, -1921.4, 23.60337), 36),
                (new Vector3(785.2703, -1621.086, 29.94308), 0),
                (new Vector3(796.3647, -1598.588, 30.15678), 0),
                (new Vector3(802.5596, -1620.761, 30.04909), 0),
                (new Vector3(836.6028, -1935.048, 27.8449), 170),
                (new Vector3(842.825, -1977.71, 28.17158), 79),
                (new Vector3(845.811, -1951.911, 27.83204), 83),
                (new Vector3(2292.271, 4890.153, 40.10825), 312),
                (new Vector3(2309.38, 4815.771, 38.82953), 300),
                (new Vector3(2405.266, 4983.108, 44.90929), 133),
                (new Vector3(2674.473, 3524.719, 51.44942), 332),
                (new Vector3(2738.209, 3414.174, 55.53788), 246),
                (new Vector3(2758.571, 3527.462, 51.97694), 77),
                (new Vector3(2910.486, 4391.361, 49.02826), 292),
                (new Vector3(2916.927, 4377.616, 49.28371), 300),
            };
            _loadPointsFirstPart.AddRange(DockLoadPoints);
            
            _unloadPointsFirstPart = new List<Vector3>
            {
                new Vector3(-1402.436, -642.4412, 27.55337),
                new Vector3(-802.0358, -2113.607, 7.691144),
                new Vector3(-570.8846, -452.1022, 33.15334),
                new Vector3(-563.3618, -159.8066, 37.00382),
                new Vector3(-411.2873, -2648.284, 4.88022),
                new Vector3(-351.621, 6125.064, 30.32008),
                new Vector3(-306.1174, -1170.319, 22.21539),
                new Vector3(-217.0356, -2389.656, 4.881404),
                new Vector3(80.8837, 6323.271, 30.11636),
                new Vector3(94.62099, -1279.363, 27.92837),
                new Vector3(288.2829, 2861.162, 42.52238),
                new Vector3(380.048, -598.2144, 27.63154),
                new Vector3(401.9816, -1393.645, 28.52566),
                new Vector3(422.4211, 227.4901, 102.0711),
                new Vector3(476.8307, -1961.542, 23.43054),
                new Vector3(494.8745, -1971.047, 23.80917),
                new Vector3(496.9891, -990.2465, 26.47024),
                new Vector3(785.2703, -1621.086, 29.94308),
                new Vector3(786.7185, 1278.406, 359.1765),
                new Vector3(827.7833, -1975.177, 28.13364),
                new Vector3(1003.786, -2911.465, 4.780604),
                new Vector3(1154.02, -1512.126, 33.57253),
                new Vector3(1160.862, -1492.167, 33.57257),
                new Vector3(1216.18, -2986.266, 4.769779),
                new Vector3(1520.823, 784.9827, 76.32033),
                new Vector3(1520.823, 784.9827, 76.32033),

            };
            _unloadPointsFirstPart.AddRange(BusinessManager.BizList
                .Where(b => b.Value.Type == 8)
                .Select(e => e.Value.UnloadPoint).ToList());
            _unloadPointsFirstPart.AddRange(BusinessManager.BizList
                .Where(b => b.Value.Type == 0)
                .Select(e => e.Value.UnloadPoint).ToList());
            _unloadPointsFirstPart.AddRange(BusinessManager.BizList
                .Where(b => b.Value.Type == 7)
                .Select(e => e.Value.UnloadPoint).ToList());
            _unloadPointsFirstPart.AddRange(BusinessManager.BizList
                .Where(b => b.Value.Type == 9)
                .Select(e => e.Value.UnloadPoint).ToList());
            _unloadPointsFirstPart.AddRange(BusinessManager.BizList
                .Where(b => b.Value.Type == 11)
                .Select(e => e.Value.UnloadPoint).ToList());
            _unloadPointsFirstPart.AddRange(BusinessManager.BizList
                .Where(b => b.Value.Type == 13)
                .Select(e => e.Value.UnloadPoint).ToList());
        }

        private static void LoadSecondPoints()// вызывать после LoadFirst
        {
            _loadPointsSecondPart = new List<(Vector3, float)>
            {
                (new Vector3(581.4086, 2798.833, 40.98441), 287),
                (new Vector3(610.9162, 2797.437, 40.84628), 8),
                //(new Vector3(1546.331, -2092.183, 75.9956), 3.2f), //на базе у синих
                (new Vector3(1708.337, -1637.431, 111.364), 192),
                (new Vector3(2676.959, 1396.024, 23.41633), 194),
                (new Vector3(2738.249, 1710.394, 23.50977), 175),
                (new Vector3(2772.405, 1404.892, 23.40326), 8.7f),
                (new Vector3(2792.45, 1709.436, 23.49774), 81),
            };
            
            _loadPointsSecondPart.AddRange(_loadPointsFirstPart);
            
            _unloadPointsSecondPart = new List<Vector3>
            {
                new Vector3(-507.3515, -1045.224, 22.43057),
                new Vector3(-505.5651, -941.642, 22.86756),
                new Vector3(-449.1065, -991.0843, 22.50447),
                new Vector3(-181.087, -1032.372, 26.15358),
                new Vector3(-165.3173, -1032.667, 26.15355),
                new Vector3(-133.1771, -1102.252, 20.56524),
                new Vector3(-98.53783, -954.1395, 27.33911),
                new Vector3(19.00052, -386.0247, 38.27295),
                new Vector3(58.83251, -411.1791, 38.8005),
                new Vector3(115.0345, -421.419, 39.94042),
                new Vector3(116.7576, -370.545, 41.3472),
                new Vector3(288.2829, 2861.162, 42.52238),
                new Vector3(835.418, 2356.99, 50.68313),
                new Vector3(863.7564, 2374.72, 53.35925),
                new Vector3(1015.683, 2312.392, 44.1624),
                new Vector3(1099.652, 2077.009, 52.52225),
                new Vector3(1107.841, 2213.859, 48.72133),
                new Vector3(1376.783, -717.6054, 65.62799),
                new Vector3(1395.658, -755.9365, 66.30567),
                new Vector3(1398.878, -735.8174, 66.19203),
            };
            _unloadPointsFirstPart.AddRange(BusinessManager.BizList
                .Where(b => b.Value.Type == 1)
                .Select(e => e.Value.UnloadPoint).ToList());
            
            _unloadPointsFirstPart.AddRange(BusinessManager.BizList
                .Where(b => b.Value.Type == 3)
                .Select(e => e.Value.UnloadPoint).ToList());
            
            _unloadPointsSecondPart.AddRange(_unloadPointsFirstPart);
        }
        private static void LoadTrashRoutePresets()
        {
            _trashPreset = (new List<(Vector3, float)>
                {
                    (new Vector3(-328.8619, -1318.749, 30.28042), 0),
                    (new Vector3(-28.67153, -1353.877, 28.03963), 0),
                    (new Vector3(-7.098591, -1082.354, 25.55207), 0),
                    (new Vector3(6.339994, -1031.873, 28.03525), 0),
                    (new Vector3(35.08377, -1010.604, 28.33896), 0),
                    (new Vector3(64.11087, -1394.142, 28.26165), 0),
                    (new Vector3(89.87859, 310.6112, 108.9022), 0),
                    (new Vector3(96.23852, -2208.258, 4.923095), 0),
                    (new Vector3(103.1414, 319.9641, 110.9782), 0),
                    (new Vector3(112.7906, -1761.545, 28.2072), 0),
                    (new Vector3(119.6066, -1327.078, 28.25), 0),
                    (new Vector3(126.3947, -2200.297, 4.91332), 0),
                    (new Vector3(128.2699, 275.6437, 108.8541), 0),
                    (new Vector3(161.7584, -777.6418, 30.61015), 0),
                    (new Vector3(163.8806, -1286.689, 28.1786), 0),
                    (new Vector3(170.0169, -1075.329, 28.07235), 0),
                    (new Vector3(176.2229, 305.1025, 104.2516), 0),
                    (new Vector3(187.1456, -1554.598, 28.09578), 0),
                    (new Vector3(196.6522, 335.7193, 104.42), 0),
                    (new Vector3(257.2106, 377.5049, 104.4061), 0),
                    (new Vector3(273.3105, -1498.448, 28.17159), 0),
                    (new Vector3(357.2013, -1809.251, 27.84018), 0),
                    (new Vector3(373.1029, 351.4996, 101.7608), 0),
                    (new Vector3(438.8638, -1064.334, 28.0928), 0),
                    (new Vector3(450.2422, -1972.954, 21.8813), 0),
                    (new Vector3(734.0682, -2031.975, 28.15437), 0),
                    (new Vector3(759.2723, -1229.334, 23.82188), 0),
                    (new Vector3(785.7256, -805.1984, 25.11838), 0),
                    (new Vector3(788.3659, -1323.558, 24.97535), 0),
                    (new Vector3(795.2894, -2529.751, 20.34344), 0),
                    (new Vector3(826.0351, -1062.779, 26.83134), 0),
                    (new Vector3(842.9975, -2251.345, 29.08687), 0),
                    (new Vector3(845.0007, -1410.145, 24.97919), 0),
                    (new Vector3(867.5823, -1611.666, 29.07764), 0),
                    (new Vector3(868.1993, -1575.755, 29.50755), 0),
                    (new Vector3(923.3915, -1519.44, 29.88765), 0),
                    (new Vector3(945.0676, -2106.112, 29.42103), 0),
                    (new Vector3(947.8364, -2174.996, 29.44155), 0),
                    (new Vector3(963.9334, -1533.459, 29.83497), 0),
                    (new Vector3(965.2005, -1466.109, 29.89719), 0),
                    (new Vector3(972.7986, -1873.747, 30.05418), 0),
                    (new Vector3(994.6057, -2532.911, 27.18198), 0),
                    (new Vector3(1070.609, -2385.848, 29.29741), 0),
                    (new Vector3(1179.548, -302.2406, 67.93264), 0),
                },
                new List<Vector3>
                {
                    new Vector3(2385.889, 3053.011, 47.0329),
                    new Vector3(2408.639, 3139.436, 47.04271),
                    new Vector3(2414.687, 3091.512, 47.03288),
                });
        }

        private static void LoadDockRoutePresets()
        {
            DockLoadPoints = new List<(Vector3, float)>
            {
                (new Vector3(-547.7343, -2810.069, 4.880381), 214),
                (new Vector3(-528.5093, -2869.353, 4.880381), 40),
                (new Vector3(-501.7807, -2842.434, 4.880388), 43),
                (new Vector3(-160.474, -2658.968, 4.88103), 267),
                (new Vector3(-160.1619, -2707.426, 4.890736), 273),
                (new Vector3(166.2635, -3075.246, 4.768763), 270),
                (new Vector3(208.0854, -3072.442, 4.6595), 275),
                (new Vector3(543.7641, -2738.789, 4.936147), 329),
                (new Vector3(582.7476, -2761.172, 4.936067), 327),
                (new Vector3(663.4078, -2672.688, 4.96118), 87),
                (new Vector3(663.0127, -2687.576, 4.961184), 89),
            };
        }
        
        public static void Init()
        {
            LoadDockRoutePresets();
            LoadFirstPoints();
            LoadSecondPoints();
            LoadTrashRoutePresets();
            _stages.Add(0,  new TruckerStage(0,    500,   Convert.ToInt32(_salaryIncrementCoefficient * 1200), _trashPreset.Item1,    _trashPreset.Item2));
            _stages.Add(1,  new TruckerStage(20,   1000,  Convert.ToInt32(_salaryIncrementCoefficient * 1400), _loadPointsFirstPart,  _unloadPointsFirstPart));            
            _stages.Add(2,  new TruckerStage(60,   1500,  Convert.ToInt32(_salaryIncrementCoefficient * 1600), _loadPointsSecondPart, _unloadPointsSecondPart));
            _stages.Add(3,  new TruckerStage(120,  2000,  Convert.ToInt32(_salaryIncrementCoefficient * 1800), _loadPointsFirstPart,  _unloadPointsFirstPart));
            _stages.Add(4,  new TruckerStage(200,  2500,  Convert.ToInt32(_salaryIncrementCoefficient * 1900), _loadPointsFirstPart,  _unloadPointsFirstPart));            
            _stages.Add(5,  new TruckerStage(300,  3000,  Convert.ToInt32(_salaryIncrementCoefficient * 2000), _loadPointsSecondPart, _unloadPointsSecondPart));
            _stages.Add(6,  new TruckerStage(420,  3500,  Convert.ToInt32(_salaryIncrementCoefficient * 2100), _loadPointsSecondPart, _unloadPointsSecondPart));
            _stages.Add(7,  new TruckerStage(560,  4000,  Convert.ToInt32(_salaryIncrementCoefficient * 2200), _loadPointsSecondPart, _unloadPointsSecondPart));
            _stages.Add(8,  new TruckerStage(720,  4500,  Convert.ToInt32(_salaryIncrementCoefficient * 2300), _loadPointsSecondPart, _unloadPointsSecondPart));

            /*
            _stages.Add(9,  new TruckerStage(900,  5000,  Convert.ToInt32(_salaryIncrementCoefficient * 2400), _loadPointsFirstPart,  _unloadPointsFirstPart, 200));
            _stages.Add(10, new TruckerStage(1100, 5300,  Convert.ToInt32(_salaryIncrementCoefficient * 2500), _loadPointsFirstPart,  _unloadPointsFirstPart));
            _stages.Add(11, new TruckerStage(1320, 5600,  Convert.ToInt32(_salaryIncrementCoefficient * 2600), _loadPointsSecondPart, _unloadPointsSecondPart));
            _stages.Add(12, new TruckerStage(1560, 5900,  Convert.ToInt32(_salaryIncrementCoefficient * 2700), _loadPointsSecondPart, _unloadPointsSecondPart, trailerHash: "trailers"));
            _stages.Add(13, new TruckerStage(1840, 6200,  Convert.ToInt32(_salaryIncrementCoefficient * 2800), _loadPointsSecondPart, _unloadPointsSecondPart, trailerHash: "trailers"));
            _stages.Add(14, new TruckerStage(2060, 6500,  Convert.ToInt32(_salaryIncrementCoefficient * 2900), _loadPointsSecondPart, _unloadPointsSecondPart, trailerHash: "trailers"));// Лесопилка
            _stages.Add(15, new TruckerStage(2360, 6800,  Convert.ToInt32(_salaryIncrementCoefficient * 3000), _loadPointsSecondPart, _unloadPointsSecondPart, trailerHash: "trailers"));
            _stages.Add(16, new TruckerStage(2680, 7100,  Convert.ToInt32(_salaryIncrementCoefficient * 3100), _loadPointsSecondPart, _unloadPointsSecondPart, trailerHash: "trailers"));
            _stages.Add(17, new TruckerStage(3020, 7400,  Convert.ToInt32(_salaryIncrementCoefficient * 3200), _loadPointsSecondPart, _unloadPointsSecondPart, trailerHash: "trailers"));
            _stages.Add(18, new TruckerStage(3380, 7700,  Convert.ToInt32(_salaryIncrementCoefficient * 3300), _loadPointsSecondPart, _unloadPointsSecondPart, trailerHash: "trailers2"));
            _stages.Add(19, new TruckerStage(3760, 8000,  Convert.ToInt32(_salaryIncrementCoefficient * 3400), _loadPointsSecondPart, _unloadPointsSecondPart, 400, "trailers2"));
            _stages.Add(20, new TruckerStage(3160, 8300,  Convert.ToInt32(_salaryIncrementCoefficient * 3500), _loadPointsSecondPart, _unloadPointsSecondPart, trailerHash: "trailers2"));
            _stages.Add(21, new TruckerStage(3580, 8600,  Convert.ToInt32(_salaryIncrementCoefficient * 3600), _loadPointsSecondPart, _unloadPointsSecondPart, trailerHash: "trailers2"));
            _stages.Add(22, new TruckerStage(4020, 8900,  Convert.ToInt32(_salaryIncrementCoefficient * 3700), _loadPointsSecondPart, _unloadPointsSecondPart, trailerHash: "trailers3"));
            _stages.Add(23, new TruckerStage(4480, 9200,  Convert.ToInt32(_salaryIncrementCoefficient * 3800), _loadPointsSecondPart, _unloadPointsSecondPart, trailerHash: "trailers3"));//82 прирост тут
            _stages.Add(24, new TruckerStage(4960, 9500,  Convert.ToInt32(_salaryIncrementCoefficient * 4000), _loadPointsSecondPart, _unloadPointsSecondPart, 500, "trailers3"));//82 прирост тут
            _stages.Add(25, new TruckerStage(5460, 9800,  Convert.ToInt32(_salaryIncrementCoefficient * 4200), _loadPointsSecondPart, _unloadPointsSecondPart, trailerHash: "trailers4"));
            _stages.Add(26, new TruckerStage(5980, 10100, Convert.ToInt32(_salaryIncrementCoefficient * 4500), _loadPointsSecondPart, _unloadPointsSecondPart, trailerHash: "trailers4"));
            _stages.Add(27, new TruckerStage(6520, 10400, Convert.ToInt32(_salaryIncrementCoefficient * 4600), _loadPointsSecondPart, _unloadPointsSecondPart, trailerHash: "trailers4"));
            _stages.Add(28, new TruckerStage(7080, 10700, Convert.ToInt32(_salaryIncrementCoefficient * 4800), _loadPointsSecondPart, _unloadPointsSecondPart, trailerHash: "trailers4"));
            */

            _trucks.Add(0, new TruckInfo(0xB527915C, false));
            _trucks.Add(1, new TruckInfo(0x2E19879, false));
            _trucks.Add(2, new TruckInfo(0x1C534995, false));
            _trucks.Add(3, new TruckInfo(0x35ED670B, false));
            _trucks.Add(4, new TruckInfo(0xC1632BEB, false));
            _trucks.Add(5, new TruckInfo(0x85A5B471, false));
            _trucks.Add(6, new TruckInfo(0x73F4110E, false, new Color(255, 211, 0)));
            _trucks.Add(7, new TruckInfo(0x7A61B330, false));
            _trucks.Add(8, new TruckInfo(0x6290F15B, false));

            /*
            _trucks.Add(9, new TruckInfo(0x73F4110E, false, new Color(0, 9, 255)));//mule4
            _trucks.Add(10, new TruckInfo(0x6290F15B, false));
            _trucks.Add(11, new TruckInfo(0x1C534995, false));
            // Packer
            _trucks.Add(12, new TruckInfo(0x21EEE87D, true));
            _trucks.Add(13, new TruckInfo(0x21EEE87D, true, new Color(255, 211, 0)));
            
            _trucks.Add(14, new TruckInfo(0x5A82F9AE, true));//hauler
            _trucks.Add(15, new TruckInfo(0x5A82F9AE, true, new Color(0, 9, 255)));//hauler
            _trucks.Add(16, new TruckInfo(0x5A82F9AE, true, new Color(255, 103, 0)));//hauler
            _trucks.Add(17, new TruckInfo(0x5A82F9AE, true, new Color(255, 255, 255)));//hauler
            
            _trucks.Add(18, new TruckInfo(0x809AA4CB, true));//phantom
            _trucks.Add(19, new TruckInfo(0x809AA4CB, true, new Color(63, 63, 63)));//phantom
            _trucks.Add(20, new TruckInfo(0x809AA4CB, true, new Color(255, 0, 5)));//phantom
            _trucks.Add(21, new TruckInfo(0x809AA4CB, true, new Color(255, 253, 0)));//phantom
            _trucks.Add(22, new TruckInfo(0x809AA4CB, true, new Color(255, 255, 255)));//phantom
            
            _trucks.Add(23, new TruckInfo(0xA90ED5C, true));//phantom3
            _trucks.Add(24, new TruckInfo(0xA90ED5C, true, new Color(73, 73, 73)));//phantom3
            _trucks.Add(25, new TruckInfo(0xA90ED5C, true, new Color(255, 0, 5)));//phantom3
            _trucks.Add(26, new TruckInfo(0xA90ED5C, true, new Color(255, 176, 0)));//phantom3
            
            _trucks.Add(27, new TruckInfo(0x171C92C4, true));//hauler2
            _trucks.Add(28, new TruckInfo(0x171C92C4, true, new Color(255, 255, 255)));//hauler2
            */
        }

        /// <summary>
        /// Получить уровень дальнобойщика
        /// </summary>
        /// <param name="stagesPassed">Количество совершенных перевозов</param>
        public static int GetLevel(int stagesPassed)
        {
            return _stages.LastOrDefault(s => s.Value.RequiredTransportations <= stagesPassed).Key;
        }
    }
}