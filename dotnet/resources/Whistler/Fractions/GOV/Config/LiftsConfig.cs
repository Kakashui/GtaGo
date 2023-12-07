using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Fractions.GOV.Models;

namespace Whistler.Fractions.GOV.Config
{
    class LiftsConfig
    {
        public static Vector3 LeftInputLift = new Vector3(2511.223, -426.6018, 93.16309);
        public static Vector3 RightInputLift = new Vector3(2511.122, -341.4175, 93.16382);
        public static Vector3 LeftExitLift = new Vector3(2512.909, -425.1044, 93.11605);
        public static Vector3 RightExitLift = new Vector3(2512.849, -343.2283, 93.11026);
        public static Vector3 LeftExitLiftRot = new Vector3(0, 0, 312.1711);
        public static Vector3 RightExitLiftRot = new Vector3(0, 0, 229.1917);
        public static Vector3 LeftInputLiftUp = new Vector3(2510.868, -426.8039, 105.7869);
        public static Vector3 RightInputLiftUp = new Vector3(2510.723, -341.459, 105.7842);
        public static List<GovernmentLiftModel> LeftExitLiftUp = new List<GovernmentLiftModel>
        {
            new GovernmentLiftModel("2 floor", new Vector3(2512.673, -425.0628, 105.7387), new Vector3(0, 0, 311.9064), 101, 1),
            new GovernmentLiftModel("3 floor", new Vector3(2512.673, -425.0628, 105.7387), new Vector3(0, 0, 311.9064), 102, 2),
            new GovernmentLiftModel("4 floor", new Vector3(2512.673, -425.0628, 105.7387), new Vector3(0, 0, 311.9064), 103, 3),
            new GovernmentLiftModel("5 floor", new Vector3(2512.673, -425.0628, 105.7387), new Vector3(0, 0, 311.9064), 104, 4),
        };
        public static List<GovernmentLiftModel> RightExitLiftUp = new List<GovernmentLiftModel>
        {
            new GovernmentLiftModel("2 floor", new Vector3(2512.681, -343.3703, 105.7362), new Vector3(0, 0, 239.5885), 101, 1),
            new GovernmentLiftModel("3 floor", new Vector3(2512.681, -343.3703, 105.7362), new Vector3(0, 0, 239.5885), 102, 2),
            new GovernmentLiftModel("4 floor", new Vector3(2512.681, -343.3703, 105.7362), new Vector3(0, 0, 239.5885), 103, 3),
            new GovernmentLiftModel("5 floor", new Vector3(2512.681, -343.3703, 105.7362), new Vector3(0, 0, 239.5885), 104, 4),
        };
    }
}
