using GTANetworkAPI;
using System;
using System.Linq;
using System.Collections.Generic;
using Whistler.Core;
using Whistler.SDK;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Whistler.Core.QuestPeds;
using Whistler.VehicleSystem;
using Whistler.Helpers;
using Whistler.VehicleSystem.Models.VehiclesData;
using Whistler.GUI;
using Whistler.Jobs.TruckersJob;
using Whistler.MoneySystem;
using Whistler.NewDonateShop;
using Whistler.Core.Character;
using Whistler.Common;
using Whistler.Entities;
using Whistler.VehicleSystem.Models;
using Whistler.GarbageCollector;

namespace Whistler.Jobs
{
    class Bus : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Bus));
        #region Bus Ways Creator
        [Command("startnewbusway")]
        public void StartCreatingBusWay(ExtPlayer player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "startnewbuway")) return;

                var checkpoints = new Stack<Tuple<Vector3, bool>>();
                SafeTrigger.SetData(player, "BUSWAY_CHECKPOINTS", checkpoints);

                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "local_142", 3000);
            }
            catch (Exception e) { _logger.WriteError("startnewbusway: " + e.ToString()); }
        }

        [Command("addbuspoint")]
        public void AddNewBusWayPoint(ExtPlayer player, bool isStop)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "addbuspoint")) return;

                Stack<Tuple<Vector3, bool>> checkpoints = player.GetData<Stack<Tuple<Vector3, bool>>>("BUSWAY_CHECKPOINTS");

                var position = player.Position - new Vector3(0, 0, 1.12);
                checkpoints.Push(new Tuple<Vector3, bool>(position, isStop));

                SafeTrigger.ClientEvent(player,"buswayscreator:pushMarker", position, isStop);
            }
            catch (Exception e) { _logger.WriteError("addbuspoint: " + e.ToString()); }
        }

        [Command("removebuspoint")]
        public void RemoveLastBusWayPoint(ExtPlayer player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "removebuspoint")) return;

                Stack<Tuple<Vector3, bool>> checkpoints = player.GetData<Stack<Tuple<Vector3, bool>>>("BUSWAY_CHECKPOINTS");
                checkpoints.Pop();

                SafeTrigger.ClientEvent(player,"buswayscreator:popMarker");
            }
            catch (Exception e) { _logger.WriteError("removebuspoint: " + e.ToString()); }
        }

        [Command("finishbusway")]
        public void FinishBusWay(ExtPlayer player, string busWayName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "finishbusway")) return;

                var checkpoints = player.GetData<Stack<Tuple<Vector3, bool>>>("BUSWAY_CHECKPOINTS");

                if (!Directory.Exists("busways"))
                {
                    Directory.CreateDirectory("busways");
                }

                using (StreamWriter file = new StreamWriter($"busways/{busWayName}.txt", true, Encoding.UTF8))
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

                    foreach (var point in checkpoints.Reverse())
                    {
                        file.WriteLine($"new BusCheck(new Vector3({point.Item1.X}, {point.Item1.Y}, {point.Item1.Z}), {point.Item2.ToString().ToLower()}),");
                    }

                    file.Close();
                }

                player.ResetData("BUSWAY_CHECKPOINTS");
                SafeTrigger.ClientEvent(player,"buswayscreator:clearMarkers");
            }
            catch (Exception e) { _logger.WriteError("finishbusway: " + e.ToString()); }
        }
        #endregion

        private static List<int> BuswaysPayments = new List<int>()
        {
            50, 4, 3, 4, 4, 8
        };
        
        private static int BusRentCost = 2000;
        private static List<string> BusWaysNames = new List<string>
        {
            "Fix_2",
        };
        private static List<List<BusCheck>> BusWays = new List<List<BusCheck>>()
        {
            new List<BusCheck>()
            {
                new BusCheck(new Vector3(422.1137, -674.1924, 27.76142), false),
                new BusCheck(new Vector3(377.168, -666.6127, 27.73141), false),
                new BusCheck(new Vector3(389.0001, -621.6503, 27.41046), true),
                new BusCheck(new Vector3(446.3656, -534.6471, 26.89148), false),
                new BusCheck(new Vector3(584.7208, -351.0631, 33.9251), false),
                new BusCheck(new Vector3(664.8224, -226.4948, 42.41568), false),
                new BusCheck(new Vector3(741.5232, -85.36739, 54.41381), false),
                new BusCheck(new Vector3(850.4318, 74.72253, 66.3352), true),
                new BusCheck(new Vector3(1022.331, 281.3962, 81.32581), false),
                new BusCheck(new Vector3(1120.426, 353.0294, 89.7627), true),
                new BusCheck(new Vector3(1173.101, 423.4496, 89.21178), false),
                new BusCheck(new Vector3(1280.317, 558.1822, 79.20348), false),
                new BusCheck(new Vector3(1455.135, 739.5914, 75.94089), false),
                new BusCheck(new Vector3(1489.793, 765.8294, 75.98931), true),
                new BusCheck(new Vector3(1542.216, 857.003, 75.97947), false),
                new BusCheck(new Vector3(1567.322, 943.0686, 76.63514), false),
                new BusCheck(new Vector3(1661.639, 1220.503, 83.71792), false),
                new BusCheck(new Vector3(1711.798, 1472.066, 83.65356), false),
                new BusCheck(new Vector3(1770.602, 1787.406, 79.55607), false),
                new BusCheck(new Vector3(1829.936, 2067.277, 72.36456), false),
                new BusCheck(new Vector3(1893.953, 2365.301, 53.59543), false),
                new BusCheck(new Vector3(2020.379, 2571.021, 53.17053), false),
                new BusCheck(new Vector3(2123.741, 2643.703, 49.47456), true),
                new BusCheck(new Vector3(2241.054, 2750.718, 43.311), false),
                new BusCheck(new Vector3(2400.276, 2843.588, 45.85299), false),
                new BusCheck(new Vector3(2433.168, 2882.619, 47.72298), false),
                new BusCheck(new Vector3(2349.731, 2974.418, 47.06969), false),
                new BusCheck(new Vector3(2167.351, 3011.645, 43.91059), false),
                new BusCheck(new Vector3(1993.592, 2986.433, 44.21371), false),
                new BusCheck(new Vector3(1831.192, 2945.005, 44.28941), false),
                new BusCheck(new Vector3(1693.051, 2878.133, 41.9178), false),
                new BusCheck(new Vector3(1583.554, 2806.614, 37.10222), false),
                new BusCheck(new Vector3(1481.229, 2742.331, 36.23688), true),
                new BusCheck(new Vector3(1382.253, 2693.619, 36.1668), false),
                new BusCheck(new Vector3(1249.964, 2684.144, 36.12576), false),
                new BusCheck(new Vector3(1175.764, 2690.425, 36.38087), true),
                new BusCheck(new Vector3(1033.224, 2692.606, 37.94114), false),
                new BusCheck(new Vector3(860.0817, 2700.226, 39.29581), false),
                new BusCheck(new Vector3(690.1392, 2703.069, 39.05276), false),
                new BusCheck(new Vector3(663.2747, 2725.306, 40.38007), false),
                new BusCheck(new Vector3(638.4131, 2742.663, 40.49842), false),
                new BusCheck(new Vector3(611.4587, 2740.24, 40.39569), true),
                new BusCheck(new Vector3(593.0718, 2709.723, 40.37864), false),
                new BusCheck(new Vector3(568.2053, 2693.588, 40.57887), false),
                new BusCheck(new Vector3(450.6233, 2678.33, 42.21728), false),
                new BusCheck(new Vector3(302.0734, 2643.294, 43.03107), false),
                new BusCheck(new Vector3(281.8667, 2656.439, 43.18356), false),
                new BusCheck(new Vector3(257.9612, 2773.926, 42.38515), false),
                new BusCheck(new Vector3(232.7696, 2951.853, 41.43577), false),
                new BusCheck(new Vector3(218.0024, 3201.049, 41.15731), false),
                new BusCheck(new Vector3(253.1276, 3357.806, 37.57611), false),
                new BusCheck(new Vector3(323.3152, 3437.089, 34.906), false),
                new BusCheck(new Vector3(375.6837, 3459.975, 33.73102), true),
                new BusCheck(new Vector3(436.8615, 3482.645, 33.14578), false),
                new BusCheck(new Vector3(604.309, 3510.626, 32.68782), false),
                new BusCheck(new Vector3(785.9597, 3529.131, 32.75726), false),
                new BusCheck(new Vector3(948.8748, 3534.243, 32.55853), false),
                new BusCheck(new Vector3(1197.715, 3534.93, 33.67043), false),
                new BusCheck(new Vector3(1311.182, 3550.836, 33.65437), false),
                new BusCheck(new Vector3(1432.639, 3590.374, 33.40628), true),
                new BusCheck(new Vector3(1503.047, 3621.344, 33.39943), false),
                new BusCheck(new Vector3(1633.067, 3685.121, 32.81652), false),
                new BusCheck(new Vector3(1725.692, 3732.288, 32.29106), true),
                new BusCheck(new Vector3(1799.153, 3780.166, 32.18549), false),
                new BusCheck(new Vector3(1833.081, 3761.999, 31.93128), false),
                new BusCheck(new Vector3(1869.732, 3732.22, 31.57533), false),
                new BusCheck(new Vector3(1936.819, 3748.734, 30.85148), false),
                new BusCheck(new Vector3(1984.349, 3730.26, 30.8822), true),
                new BusCheck(new Vector3(2045.508, 3746.534, 31.0816), false),
                new BusCheck(new Vector3(2088.806, 3732.829, 31.53677), false),
                new BusCheck(new Vector3(2301.229, 3855.461, 33.22334), false),
                new BusCheck(new Vector3(2421.206, 3955.572, 34.98779), false),
                new BusCheck(new Vector3(2481.641, 4063.636, 36.29506), false),
                new BusCheck(new Vector3(2504.359, 4103.646, 36.78699), true),
                new BusCheck(new Vector3(2489.042, 4148.241, 36.42716), false),
                new BusCheck(new Vector3(2450.294, 4284.021, 35.36266), false),
                new BusCheck(new Vector3(2487.36, 4477.79, 33.57435), false),
                new BusCheck(new Vector3(2420.899, 4628.125, 35.44973), false),
                new BusCheck(new Vector3(2286.071, 4727.165, 36.18642), false),
                new BusCheck(new Vector3(2141.199, 4754.938, 39.71914), false),
                new BusCheck(new Vector3(2068.262, 4692.568, 39.71882), false),
                new BusCheck(new Vector3(1955.145, 4602.187, 38.50766), false),
                new BusCheck(new Vector3(1819.899, 4577.195, 34.70077), false),
                new BusCheck(new Vector3(1779.687, 4578.597, 36.00731), true),
                new BusCheck(new Vector3(1742.542, 4592.32, 38.89871), false),
                new BusCheck(new Vector3(1698.661, 4668.604, 41.84652), false),
                new BusCheck(new Vector3(1685.708, 4749.121, 40.54785), false),
                new BusCheck(new Vector3(1665.814, 4913.117, 40.61788), false),
                new BusCheck(new Vector3(1688.186, 4947.898, 41.0952), true),
                new BusCheck(new Vector3(1748.103, 4994.789, 47.74355), false),
                new BusCheck(new Vector3(1811.3, 5055.494, 57.48277), false),
                new BusCheck(new Vector3(1933.266, 5143.654, 42.79694), false),
                new BusCheck(new Vector3(1987.88, 5147.517, 43.17025), false),
                new BusCheck(new Vector3(2101.278, 5216.664, 55.26764), false),
                new BusCheck(new Vector3(2226.155, 5198.722, 60.04853), false),
                new BusCheck(new Vector3(2301.243, 5185.379, 58.36971), false),
                new BusCheck(new Vector3(2379.894, 5216.588, 55.04584), false),
                new BusCheck(new Vector3(2416.272, 5140.506, 45.42236), false),
                new BusCheck(new Vector3(2565.936, 5093.583, 43.09349), false),
                new BusCheck(new Vector3(2625.739, 5108.813, 43.36451), false),
                new BusCheck(new Vector3(2632.962, 5159.007, 43.31228), false),
                new BusCheck(new Vector3(2571.829, 5365.183, 43.08956), false),
                new BusCheck(new Vector3(2454.632, 5686.263, 43.68531), false),
                new BusCheck(new Vector3(2305.708, 5928.445, 47.02079), false),
                new BusCheck(new Vector3(2202.139, 6048.026, 50.63028), false),
                new BusCheck(new Vector3(2046.688, 6148.426, 47.54199), false),
                new BusCheck(new Vector3(1944.409, 6320.596, 42.28114), false),
                new BusCheck(new Vector3(1764.25, 6371.594, 35.81409), false),
                new BusCheck(new Vector3(1702.642, 6398.289, 31.30288), false),
                new BusCheck(new Vector3(1644.388, 6419.591, 26.9312), true),
                new BusCheck(new Vector3(1590.327, 6435.932, 23.75172), false),
                new BusCheck(new Vector3(1507.819, 6453.688, 21.10286), false),
                new BusCheck(new Vector3(1287.973, 6496.337, 18.79633), false),
                new BusCheck(new Vector3(1029.958, 6493.33, 19.51353), false),
                new BusCheck(new Vector3(743.4399, 6511.593, 25.03288), false),
                new BusCheck(new Vector3(521.9761, 6559.735, 25.9344), false),
                new BusCheck(new Vector3(309.9625, 6577.896, 27.92869), false),
                new BusCheck(new Vector3(188.5342, 6555.142, 30.5195), false),
                new BusCheck(new Vector3(136.0985, 6549.138, 30.14827), false),
                new BusCheck(new Vector3(91.86416, 6594.121, 30.06025), false),
                new BusCheck(new Vector3(58.81058, 6597.96, 29.95148), false),
                new BusCheck(new Vector3(-51.56122, 6486.815, 29.91941), false),
                new BusCheck(new Vector3(-129.1997, 6405.966, 29.91172), false),
                new BusCheck(new Vector3(-168.609, 6376.741, 29.85526), true),
                new BusCheck(new Vector3(-213.3955, 6324.054, 29.98795), false),
                new BusCheck(new Vector3(-291.3896, 6246.947, 29.94721), false),
                new BusCheck(new Vector3(-289.4024, 6213.118, 30.00023), false),
                new BusCheck(new Vector3(-242.2207, 6164.964, 30.04718), false),
                new BusCheck(new Vector3(-255.256, 6128.175, 29.70757), false),
                new BusCheck(new Vector3(-322.5765, 6067.483, 29.77437), true),
                new BusCheck(new Vector3(-426.4638, 5951.044, 30.39734), false),
                new BusCheck(new Vector3(-499.5351, 5849.947, 32.51718), false),
                new BusCheck(new Vector3(-617.7625, 5612.447, 37.56888), false),
                new BusCheck(new Vector3(-750.9531, 5512.888, 34.13403), false),
                new BusCheck(new Vector3(-783.9711, 5529.69, 32.64415), false),
                new BusCheck(new Vector3(-774.1569, 5532.541, 32.00642), true),
                new BusCheck(new Vector3(-756.7371, 5534.721, 32.01211), false),
                new BusCheck(new Vector3(-780.2253, 5553.57, 32.02015), false),
                new BusCheck(new Vector3(-785.2789, 5516.792, 32.95012), false),
                new BusCheck(new Vector3(-799.6119, 5482.352, 32.58126), false),
                new BusCheck(new Vector3(-885.7205, 5435.227, 34.1617), false),
                new BusCheck(new Vector3(-1030.677, 5364.33, 41.46011), false),
                new BusCheck(new Vector3(-1202.453, 5264.57, 49.97256), false),
                new BusCheck(new Vector3(-1308.966, 5236.641, 52.87657), false),
                new BusCheck(new Vector3(-1347.649, 5131.015, 60.20387), false),
                new BusCheck(new Vector3(-1529.17, 4984.212, 60.99313), false),
                new BusCheck(new Vector3(-1686.829, 4824.352, 59.15472), false),
                new BusCheck(new Vector3(-1863.012, 4662.216, 55.55002), false),
                new BusCheck(new Vector3(-2043.042, 4484.882, 55.67565), false),
                new BusCheck(new Vector3(-2154.691, 4452.682, 61.9355), false),
                new BusCheck(new Vector3(-2215.26, 4333.958, 48.19844), false),
                new BusCheck(new Vector3(-2335.559, 4116.586, 34.35604), false),
                new BusCheck(new Vector3(-2423.325, 3863.862, 22.67658), false),
                new BusCheck(new Vector3(-2484.172, 3674.456, 12.41984), false),
                new BusCheck(new Vector3(-2506.086, 3595.267, 13.01756), true),
                new BusCheck(new Vector3(-2564.326, 3406.206, 11.77501), false),
                new BusCheck(new Vector3(-2601.449, 3115.016, 13.5974), false),
                new BusCheck(new Vector3(-2623.181, 2914.721, 15.20452), false),
                new BusCheck(new Vector3(-2659.8, 2669.667, 15.2187), false),
                new BusCheck(new Vector3(-2707.754, 2347.988, 15.50599), false),
                new BusCheck(new Vector3(-2809.22, 2212.209, 26.54499), false),
                new BusCheck(new Vector3(-2950.499, 2123.006, 39.70521), false),
                new BusCheck(new Vector3(-2993.517, 2016.99, 32.66048), false),
                new BusCheck(new Vector3(-3045.481, 1838.579, 30.6661), false),
                new BusCheck(new Vector3(-3040.129, 1701.411, 34.88239), false),
                new BusCheck(new Vector3(-3003.011, 1480.497, 26.30058), false),
                new BusCheck(new Vector3(-3080.757, 1366.031, 18.82938), false),
                new BusCheck(new Vector3(-3121.773, 1322.556, 18.26006), false),
                new BusCheck(new Vector3(-3173.708, 1270.625, 10.95019), false),
                new BusCheck(new Vector3(-3192.435, 1151.655, 8.181454), false),
                new BusCheck(new Vector3(-3232.567, 1023.419, 10.42156), false),
                new BusCheck(new Vector3(-3234.955, 965.7787, 11.57702), true),
                new BusCheck(new Vector3(-3194.784, 909.9579, 12.95235), false),
                new BusCheck(new Vector3(-3145.65, 859.2975, 14.00483), false),
                new BusCheck(new Vector3(-3089.493, 767.8923, 18.55804), false),
                new BusCheck(new Vector3(-3093.92, 709.438, 18.36559), false),
                new BusCheck(new Vector3(-3038.995, 607.0548, 6.120207), true),
                new BusCheck(new Vector3(-3022.789, 550.0478, 6.057031), false),
                new BusCheck(new Vector3(-3043.68, 442.993, 4.832955), false),
                new BusCheck(new Vector3(-3095.845, 266.0446, 9.083327), false),
                new BusCheck(new Vector3(-3054.637, 224.4894, 14.6487), false),
                new BusCheck(new Vector3(-3033.477, 183.7093, 14.49724), false),
                new BusCheck(new Vector3(-2933.075, 76.87959, 12.13697), false),
                new BusCheck(new Vector3(-2741.807, -0.02542996, 14.0105), false),
                new BusCheck(new Vector3(-2593.813, -159.5641, 19.66697), false),
                new BusCheck(new Vector3(-2378.463, -280.5488, 12.96682), false),
                new BusCheck(new Vector3(-2223.149, -354.076, 11.86032), false),
                new BusCheck(new Vector3(-2067.39, -392.2213, 10.13055), false),
                new BusCheck(new Vector3(-1995.072, -398.7397, 11.71592), false),
                new BusCheck(new Vector3(-1845.232, -504.0619, 25.94534), true),
                new BusCheck(new Vector3(-1746.2, -562.3613, 35.94725), false),
                new BusCheck(new Vector3(-1685.563, -505.3715, 36.32445), false),
                new BusCheck(new Vector3(-1599.012, -395.38, 41.52195), false),
                new BusCheck(new Vector3(-1511.625, -279.2353, 48.51748), false),
                new BusCheck(new Vector3(-1453.88, -221.5374, 47.42611), true),
                new BusCheck(new Vector3(-1395.647, -158.2711, 46.01888), false),
                new BusCheck(new Vector3(-1396.415, -91.8192, 51.10231), false),
                new BusCheck(new Vector3(-1355.54, -67.00564, 49.61102), false),
                new BusCheck(new Vector3(-1249.692, -93.16431, 42.49834), false),
                new BusCheck(new Vector3(-1086.152, -187.8951, 36.20369), false),
                new BusCheck(new Vector3(-1113.958, -218.4124, 36.23117), true),
                new BusCheck(new Vector3(-1185, -268.8321, 36.23151), false),
                new BusCheck(new Vector3(-1281.664, -323.592, 35.23913), false),
                new BusCheck(new Vector3(-1367.148, -369.3068, 35.23518), false),
                new BusCheck(new Vector3(-1448.876, -420.9687, 34.24647), false),
                new BusCheck(new Vector3(-1525.658, -465.6512, 33.80186), true),
                new BusCheck(new Vector3(-1595.769, -521.4067, 33.89774), false),
                new BusCheck(new Vector3(-1637.781, -561.0109, 32.01431), false),
                new BusCheck(new Vector3(-1631.519, -602.9203, 31.75641), false),
                new BusCheck(new Vector3(-1555.049, -656.2282, 27.58021), false),
                new BusCheck(new Vector3(-1517.71, -653.8568, 27.64604), false),
                new BusCheck(new Vector3(-1477.332, -632.3749, 29.02439), true),
                new BusCheck(new Vector3(-1397.297, -576.5508, 28.83052), false),
                new BusCheck(new Vector3(-1317.751, -521.0546, 31.50322), false),
                new BusCheck(new Vector3(-1205.933, -426.746, 32.2895), false),
                new BusCheck(new Vector3(-1108.76, -404.1614, 35.16742), false),
                new BusCheck(new Vector3(-1049.167, -389.8268, 36.0987), true),
                new BusCheck(new Vector3(-1026.674, -361.9534, 36.30374), false),
                new BusCheck(new Vector3(-905.7436, -304.5258, 38.19185), false),
                new BusCheck(new Vector3(-848.2498, -300.7811, 37.59847), false),
                new BusCheck(new Vector3(-736.5396, -352.5283, 33.89955), false),
                new BusCheck(new Vector3(-687.4252, -374.8421, 32.72592), true),
                new BusCheck(new Vector3(-662.7519, -370.0682, 33.29288), false),
                new BusCheck(new Vector3(-606.9859, -334.5951, 33.25802), false),
                new BusCheck(new Vector3(-535.6707, -294.5771, 33.70389), false),
                new BusCheck(new Vector3(-508.828, -287.2889, 33.88255), true),
                new BusCheck(new Vector3(-464.307, -259.0699, 34.50599), false),
                new BusCheck(new Vector3(-426.8457, -238.196, 34.85578), false),
                new BusCheck(new Vector3(-424.2488, -202.8477, 34.7712), false),
                new BusCheck(new Vector3(-468.1155, -126.8589, 37.2958), false),
                new BusCheck(new Vector3(-515.7504, -76.6132, 38.29764), false),
                new BusCheck(new Vector3(-543.2444, -64.68332, 40.27182), false),
                new BusCheck(new Vector3(-558.4138, -30.10728, 41.9073), false),
                new BusCheck(new Vector3(-591.034, 5.794554, 42.18276), false),
                new BusCheck(new Vector3(-692.1293, -5.501464, 36.68905), true),
                new BusCheck(new Vector3(-723.6676, 0.05986705, 36.3344), false),
                new BusCheck(new Vector3(-743.86, 75.21359, 53.47216), false),
                new BusCheck(new Vector3(-744.8654, 126.8043, 55.76779), false),
                new BusCheck(new Vector3(-753.2667, 195.6581, 73.81053), false),
                new BusCheck(new Vector3(-727.637, 218.7837, 75.4173), false),
                new BusCheck(new Vector3(-670.2212, 246.0302, 79.86584), false),
                new BusCheck(new Vector3(-562.2875, 254.8227, 81.59653), false),
                new BusCheck(new Vector3(-498.9071, 239.8395, 81.46297), true),
                new BusCheck(new Vector3(-394.3439, 230.7515, 82.13838), false),
                new BusCheck(new Vector3(-296.7199, 250.8802, 87.04388), false),
                new BusCheck(new Vector3(-242.6159, 254.194, 90.54556), false),
                new BusCheck(new Vector3(-228.6544, 238.452, 89.86967), false),
                new BusCheck(new Vector3(-228.4724, 143.535, 68.51198), false),
                new BusCheck(new Vector3(-237.6465, 83.0108, 66.48647), false),
                new BusCheck(new Vector3(-255.0188, -20.09055, 48.14644), false),
                new BusCheck(new Vector3(-271.7445, -88.18818, 46.67962), false),
                new BusCheck(new Vector3(-288.2511, -152.8016, 40.22398), false),
                new BusCheck(new Vector3(-291.016, -232.8055, 34.27121), false),
                new BusCheck(new Vector3(-276.075, -357.8597, 28.61252), false),
                new BusCheck(new Vector3(-233.3652, -408.2876, 29.08522), false),
                new BusCheck(new Vector3(-150.4553, -371.7691, 32.28588), false),
                new BusCheck(new Vector3(-109.9639, -253.4634, 42.82576), false),
                new BusCheck(new Vector3(-93.24649, -209.4327, 43.83962), false),
                new BusCheck(new Vector3(-78.90691, -135.4405, 56.22635), false),
                new BusCheck(new Vector3(-57.1304, -46.1536, 61.42284), false),
                new BusCheck(new Vector3(-42.19652, 14.22627, 70.55457), false),
                new BusCheck(new Vector3(-27.27642, 71.4772, 72.05705), false),
                new BusCheck(new Vector3(8.367899, 172.358, 97.25153), false),
                new BusCheck(new Vector3(28.84819, 229.0464, 108.0074), false),
                new BusCheck(new Vector3(62.32481, 232.6494, 107.7054), true),
                new BusCheck(new Vector3(160.1185, 200.6805, 104.8946), false),
                new BusCheck(new Vector3(243.6284, 170.4688, 103.5439), false),
                new BusCheck(new Vector3(336.3764, 136.7479, 101.7089), false),
                new BusCheck(new Vector3(413.8889, 108.6538, 99.48848), false),
                new BusCheck(new Vector3(485.0956, 81.87645, 95.23555), false),
                new BusCheck(new Vector3(490.1277, 53.43594, 93.01765), false),
                new BusCheck(new Vector3(448.2257, -26.22086, 78.37286), false),
                new BusCheck(new Vector3(396.4343, -95.23776, 65.60006), false),
                new BusCheck(new Vector3(343.6031, -99.50961, 66.00061), false),
                new BusCheck(new Vector3(314.9341, -85.27055, 67.8704), true),
                new BusCheck(new Vector3(261.7988, -72.13683, 68.51499), false),
                new BusCheck(new Vector3(140.4693, -27.71109, 66.07436), false),
                new BusCheck(new Vector3(113.2004, -52.761, 66.07517), false),
                new BusCheck(new Vector3(75.80226, -137.9787, 53.53299), false),
                new BusCheck(new Vector3(58.98939, -195.0773, 52.87923), false),
                new BusCheck(new Vector3(34.62973, -256.5393, 46.28682), false),
                new BusCheck(new Vector3(16.12885, -303.2974, 45.33957), false),
                new BusCheck(new Vector3(-35.21459, -427.8877, 38.62657), false),
                new BusCheck(new Vector3(-76.99221, -556.1051, 37.67161), false),
                new BusCheck(new Vector3(-97.56776, -591.6045, 34.61317), false),
                new BusCheck(new Vector3(-111.8981, -626.0052, 34.58101), true),
                new BusCheck(new Vector3(-116.1631, -685.2446, 33.52488), false),
                new BusCheck(new Vector3(-156.6196, -808.7966, 30.15969), false),
                new BusCheck(new Vector3(-177.1451, -868.1951, 27.88256), false),
                new BusCheck(new Vector3(-157.5236, -908.425, 27.8703), false),
                new BusCheck(new Vector3(-63.42694, -943.4898, 27.94486), false),
                new BusCheck(new Vector3(6.376304, -968.714, 27.93455), false),
                new BusCheck(new Vector3(79.4799, -990.0222, 27.93595), false),
                new BusCheck(new Vector3(120.8944, -974.7212, 27.94062), false),
                new BusCheck(new Vector3(144.9024, -909.0728, 28.77307), false),
                new BusCheck(new Vector3(168.3736, -844.0269, 29.63822), false),
                new BusCheck(new Vector3(186.0824, -796.2076, 29.92), false),
                new BusCheck(new Vector3(223.5013, -695.497, 34.75163), false),
                new BusCheck(new Vector3(275.5211, -590.476, 41.74975), true),
                new BusCheck(new Vector3(297.9715, -521.0364, 41.78933), false),
                new BusCheck(new Vector3(332.1337, -446.3433, 42.38042), false),
                new BusCheck(new Vector3(396.7256, -403.4986, 45.18819), false),
                new BusCheck(new Vector3(484.5174, -337.6916, 43.96102), false),
                new BusCheck(new Vector3(560.6848, -363.6989, 42.06549), false),
                new BusCheck(new Vector3(623.2785, -392.5558, 41.85311), false),
                new BusCheck(new Vector3(670.1066, -418.5154, 40.19962), false),
                new BusCheck(new Vector3(751.8777, -486.5208, 34.49876), false),
                new BusCheck(new Vector3(767.8539, -586.0959, 28.86414), false),
                new BusCheck(new Vector3(767.9216, -711.2336, 26.80487), false),
                new BusCheck(new Vector3(773.4387, -796.6069, 24.97273), false),
                new BusCheck(new Vector3(773.7111, -891.5729, 23.66598), false),
                new BusCheck(new Vector3(770.6981, -939.9253, 24.12187), true),
                new BusCheck(new Vector3(767.5419, -985.5596, 24.70717), false),
                new BusCheck(new Vector3(696.9651, -1004.539, 31.42245), false),
                new BusCheck(new Vector3(613.5073, -1015.354, 35.36834), false),
                new BusCheck(new Vector3(516.7377, -1027.861, 35.555), false),
                new BusCheck(new Vector3(415.5643, -1039.406, 27.98213), false),
                new BusCheck(new Vector3(401.6037, -995.3913, 27.93539), false),
                new BusCheck(new Vector3(406.755, -938.1567, 27.89118), false),
                new BusCheck(new Vector3(410.6093, -908.18, 27.86936), true),
                new BusCheck(new Vector3(408.5784, -876.0842, 27.85665), false),
                new BusCheck(new Vector3(409.0273, -807.9806, 27.80561), false),
                new BusCheck(new Vector3(408.3296, -693.8121, 27.77983), false),
                new BusCheck(new Vector3(435.8326, -680.1879, 27.50885), false),
                new BusCheck(new Vector3(464.4304, -654.5659, 26.379), false),
                new BusCheck(new Vector3(469.0848, -601.9102, 27.02626), false),
                new BusCheck(new Vector3(453.5795, -581.0454, 27.02448), false),
                new BusCheck(new Vector3(423.8653, -593.1102, 27.02611), false),
                new BusCheck(new Vector3(419.2924, -627.9025, 27.02603), false),
                new BusCheck(new Vector3(429.5045, -645.6686, 27.02718), true),
            }
        };

        #region BusStations
        private static List<Vector3> BusStations = new List<Vector3>()
        {
            new Vector3(389.0001, -621.6503, 27.41046),
            new Vector3(850.4318, 74.72253, 66.3352),
            new Vector3(1120.426, 353.0294, 89.7627),
            new Vector3(1489.793, 765.8294, 75.98931),
            new Vector3(2123.741, 2643.703, 49.47456),
            new Vector3(1175.764, 2690.425, 36.38087),
            new Vector3(1481.229, 2742.331, 36.23688),
            new Vector3(611.4587, 2740.24, 40.39569),
            new Vector3(375.6837, 3459.975, 33.73102),
            new Vector3(1432.639, 3590.374, 33.40628),
            new Vector3(1725.692, 3732.288, 32.29106),
            new Vector3(1984.349, 3730.26, 30.8822),
            new Vector3(2504.359, 4103.646, 36.78699),
            new Vector3(1779.687, 4578.597, 36.00731),
            new Vector3(1688.186, 4947.898, 41.0952),
            new Vector3(1644.388, 6419.591, 26.9312),
            new Vector3(-168.609, 6376.741, 29.85526),
            new Vector3(-322.5765, 6067.483, 29.77437),
            new Vector3(-774.1569, 5532.541, 32.00642),
            new Vector3(-2506.086, 3595.267, 13.01756),
            new Vector3(-3234.955, 965.7787, 11.57702),
            new Vector3(-3038.995, 607.0548, 6.120207),
            new Vector3(-1845.232, -504.0619, 25.94534),
            new Vector3(-1113.958, -218.4124, 36.23117),
            new Vector3(-1525.658, -465.6512, 33.80186),
            new Vector3(-1477.332, -632.3749, 29.02439),
            new Vector3(-1049.167, -389.8268, 36.0987),
            new Vector3(-687.4252, -374.8421, 32.72592),
            new Vector3(-508.828, -287.2889, 33.88255),
            new Vector3(-692.1293, -5.501464, 36.68905),
            new Vector3(-498.9071, 239.8395, 81.46297),
            new Vector3(62.32481, 232.6494, 107.7054),
            new Vector3(314.9341, -85.27055, 67.8704),
            new Vector3(-111.8981, -626.0052, 34.58101),
            new Vector3(275.5211, -590.476, 41.74975),
            new Vector3(770.6981, -939.9253, 24.12187),
            new Vector3(410.6093, -908.18, 27.86936),
            new Vector3(429.5045, -645.6686, 27.02718)
        };
        #endregion

        [ServerEvent(Event.ResourceStart)]
        public void onResourceStartHandler()
        {
            try
            {
                for (int a = 0; a < BusWays.Count; a++)
                {
                    for (int x = 0; x < BusWays[a].Count; x++)
                    {
                        var col = NAPI.ColShape.CreateCylinderColShape(BusWays[a][x].Pos, 4, 3, 0);
                        col.OnEntityEnterColShape += busCheckpointEnterWay;
                        col.SetData("WORKWAY", a);
                        col.SetData("NUMBER", x);
                    }
                }

                CreateBusQuestPed();
                //foreach (var stationPos in BusStations)
                //    NAPI.TextLabel.CreateTextLabel("Bus Station", stationPos + new Vector3(0, 0, 0.5), 30f, 0.1f, 0, new Color(255, 255, 255), true, NAPI.GlobalDimension);

            } catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
        }

        private static void CreateBusQuestPed()
        {
            var questPed = new QuestPed(PedHash.Postal01SMM, new Vector3(435.3048, -645.8337, 28.71639), "Dom Beasley", "", 110);
            questPed.PlayerInteracted += (player, ped) =>
            {
                try
                {
                    var descriptionPage = new DialogPage("qp:bus:q:1", ped.Name, ped.Role)
                        .AddAnswer("qp:bus:a:1:1", StartWork)
                        .AddCloseAnswer("");
                    var introPage = new DialogPage("", ped.Name, ped.Role);
                    if (player.Character.WorkID == 4)
                        introPage.AddAnswer("qp:bus:a:2:1", StopWork);
                    else 
                        introPage.AddAnswer("qp:bus:a:2:2", StartWork);

                    introPage.AddAnswer("qp:bus:a:2:3", descriptionPage);
                    introPage.AddCloseAnswer("qp:bus:a:2:4");
                    introPage.OpenForPlayer(player);
                }
                catch (Exception e)
                {
                    _logger.WriteError("BusQuestPed: " + e.ToString());
                }
            };
        }

        public static void StopWork(ExtPlayer player)
        {
            player.Character.WorkID = 0;
            player.Session.OnWork = false;
            player.RemoveTempVehicle(VehicleAccess.WorkBus)?.CustomDelete();

            if (player.Session.RentBus != null)
            {
                player.Session.RentBus.CustomDelete();
                player.Session.RentBus = null;
            }
            SafeTrigger.ClientEvent(player,"bus:clear");
            MainMenu.SendStats(player);
        }

        public static void StopWorkDelayed(ExtPlayer player)
        {
            ExtVehicle tempVehicle = player.GetTempVehicle(VehicleAccess.WorkBus);
            if (tempVehicle == null) return;

            Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Если Вы не сядете в транспорт в течении 60 секунд, то рабочий день закончится", 3000);
            player.Session.InWorkCar = false;
            GarbageManager.Add(tempVehicle, 1, player);
        }

        private static List<TruckRentSpot> _busSpawnPoints = new List<TruckRentSpot>
        {
            new TruckRentSpot(new Vector3(399.3899, -658.5911, 27.38206), 270),
            new TruckRentSpot(new Vector3(400.5659, -646.8042, 27.38027), 270),
            new TruckRentSpot(new Vector3(401.8925, -636.9082, 27.38014), 270),
            new TruckRentSpot(new Vector3(401.6971, -629.9773, 27.3802), 270),
            new TruckRentSpot(new Vector3(421.0982, -626.9404, 27.37995), 270),
            new TruckRentSpot(new Vector3(420.4388, -639.1947, 27.38012), 270),
            new TruckRentSpot(new Vector3(421.6132, -647.4777, 27.38028), 270),
        };
            
        public static uint[] BusHash = new uint[3] { NAPI.Util.GetHashKey("coach"), NAPI.Util.GetHashKey("bus"), NAPI.Util.GetHashKey("airbus") };
        private static Random rnd = new Random();

        private static void StartWork(ExtPlayer player)
        {
            if (!player.CheckLic(GUI.Documents.Enums.LicenseName.Truck))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Jobs_166", 3000);
                VehicleManager.WarpPlayerOutOfVehicle(player);
                return;
            }

            if (player.Character.LVL < 3)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Для этой работы нужно быть как минимум 3м уровнем.", 3000);
                VehicleManager.WarpPlayerOutOfVehicle(player);
                return;
            }

            if (player.Character.WorkID == 4)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "qp:bus:q:3", 3000);
                return;
            }

            if (player.Session.RentBus == null)
            {
                var firstUnusedSpot = _busSpawnPoints.FirstOrDefault(s => s.Busy == false);
                if (firstUnusedSpot == null)
                {
                    _busSpawnPoints.ForEach(p => p.Busy = false);
                    firstUnusedSpot = _busSpawnPoints.FirstOrDefault();
                }
                firstUnusedSpot.Busy = true;
                uint busHash = BusHash[rnd.Next(BusHash.Length)];
                ExtVehicle vehicle = VehicleManager.CreateTemporaryVehicle(busHash, firstUnusedSpot.Position, new Vector3(0, 0, firstUnusedSpot.Heading), "BUS", VehicleAccess.WorkBus, player);
                player.Session.RentBus = vehicle;
                SafeTrigger.ClientEvent(player, "bus:vehicleLoaded", JsonConvert.SerializeObject(vehicle.Position));
            }

            player.Character.WorkID = 4;
            Notify.SendInfo(player, "qp:bus:q:4");
            MainMenu.SendStats(player);
        }

        public static Vector3 GetNearestStation(Vector3 position)
        {
            Vector3 station = BusStations[0];
            foreach (var pos in BusStations)
            {
                if (position.DistanceTo(pos) < position.DistanceTo(station))
                    station = pos;
            }
            return station;
        }

        #region BusWays

        private static void busCheckpointEnterWay(ColShape shape, Player client)
        {
            try
            {
                if (!(client is ExtPlayer player)) return;

                if (!NAPI.Player.IsPlayerInAnyVehicle(player)) return;
                ExtVehicle vehicle = player.Vehicle as ExtVehicle;

                if (player.GetTempVehicle(VehicleAccess.WorkBus) != vehicle) return;
                if (player.Character.WorkID != 4 || !player.Session.OnWork || player.GetData<int>("WORKWAY") != shape.GetData<int>("WORKWAY")) return;
                var way = player.GetData<int>("WORKWAY");

                if (shape.GetData<int>("NUMBER") != player.GetData<int>("WORKCHECK")) return;
                var check = player.GetData<int>("WORKCHECK");

                if (player.GetData<bool>("BUS_ONSTOP")) return;
                if (!BusWays[way][check].IsStop)
                {
                    if (player.GetData<int>("WORKCHECK") != check) return;
                    var payment = DonateService.UseJobKoef(player, BuswaysPayments[way], player.Character.IsPrimeActive());
                    if (check + 1 != BusWays[way].Count) 
                        check++;
                    else {
                        check = 0;
                    }

                    var color = (BusWays[way][check].IsStop) ? new Color(255, 255, 255) : new Color(0, 200, 0);
                    SafeTrigger.ClientEvent(player, "bus:createCheckpoint", 
                        JsonConvert.SerializeObject(BusWays[way][check].Pos - new Vector3(0, 0, 1.12)),
                        color.Red, color.Green, color.Blue);

                    SafeTrigger.SetData(player, "WORKCHECK", check);
                    MoneySystem.Wallet.MoneyAdd(player.Character, payment, "Оплата за работу (водитель автобуса)");
                }
                else
                {
                    if (player.GetData<int>("WORKCHECK") != check) return;
                    SafeTrigger.ClientEvent(player, "bus:clear", 3, 0);

                    SafeTrigger.SetData(player, "BUS_ONSTOP", true);
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Jobs_160", 3000);

                    player.Session.Timers.BusTimer = Timers.StartOnce(10000, () => timer_busStop(player, way, check));
                }
            }
            catch (Exception ex) { _logger.WriteError("busCheckpointEnterWay: " + ex.Message); }
        }

        private static void timer_busStop(ExtPlayer player, int way, int check)
        {
            if (way < 0) return;
            if (way >= BuswaysPayments.Count) return;

            SafeTrigger.SetData(player, "BUS_ONSTOP", false);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Jobs_162", 3000);

            int payment = DonateService.UseJobCoef(player, BuswaysPayments[way], player.Character.IsPrimeActive());
            MoneySystem.Wallet.MoneyAdd(player.Character, payment, "Оплата за работу (водитель автобуса)");
            if (way >= BusWays.Count) return;

            if (++check >= BusWays[way].Count) check = 0;
            Color color = (BusWays[way][check].IsStop) ? new Color(255, 255, 255) : new Color(255, 0, 0);

            SafeTrigger.ClientEvent(player, "bus:createCheckpoint", 
                JsonConvert.SerializeObject(BusWays[way][check].Pos - new Vector3(0, 0, 1.12)),
                color.Red, color.Green, color.Blue);

            SafeTrigger.SetData(player, "WORKCHECK", check);

            if (player.Session.Timers.BusTimer != null) Timers.Stop(player.Session.Timers.BusTimer);
            player.Session.Timers.BusTimer = null;
        }
        #endregion


        public static void OnStartWork(ExtPlayer player)
        {
            if (player.Character.Money >= BusRentCost)
            {
                DialogUI.Open(player, $"Jobs_167".Translate(BusRentCost), new List<DialogUI.ButtonSetting>
                {
                    new DialogUI.ButtonSetting
                    {
                        Name = "House_58",// yes
                        Icon = "confirm",
                        Action = acceptBusRent
                    },

                    new DialogUI.ButtonSetting
                    {
                        Name = "House_59",// no
                        Icon = "cancel",
                        Action = StopWork
                    }
                });
            }
            else
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Jobs_168".Translate(BusRentCost), 3000);
                VehicleManager.WarpPlayerOutOfVehicle(player);
            }
        }

        public static void acceptBusRent(ExtPlayer player)
        {
            if (NAPI.Player.IsPlayerInAnyVehicle(player) && player.VehicleSeat == VehicleConstants.DriverSeat)
            {
                ExtVehicle vehicle = player.Vehicle as ExtVehicle;
                if (vehicle.Data.OwnerType != OwnerType.Temporary || (vehicle.Data as TemporaryVehicle).Access != VehicleAccess.WorkBus || (vehicle.Data as TemporaryVehicle).Driver != player) 
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Jobs_176", 3000);
                    return;
                }
                int way = 0;
                player.Session.InWorkCar = true;
                player.Session.OnWork = true;
                player.Session.RentBus = null;
                MoneySystem.Wallet.MoneySub(player.Character, BusRentCost, "Money_BusRentCost");

                VehicleStreaming.SetEngineState(vehicle, true);


                NAPI.Data.SetEntityData(player, "WORKWAY", way);
                NAPI.Data.SetEntityData(player, "WORKCHECK", 0);
                SafeTrigger.ClientEvent(player, "bus:createCheckpoint", JsonConvert.SerializeObject(BusWays[way][0].Pos - new Vector3(0, 0, 1.12)), 0, 255, 0);
                player.AddTempVehicle(vehicle, VehicleAccess.WorkBus);
            }
            else
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Jobs_176", 3000);
            }
        }

        internal class BusCheck
        {
            public Vector3 Pos { get; }
            public bool IsStop { get; }

            public BusCheck(Vector3 pos, bool isStop = false)
            {
                Pos = pos;
                IsStop = isStop;
            }
        }
    }
}
