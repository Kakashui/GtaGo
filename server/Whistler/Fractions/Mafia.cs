using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.VehicleSystem;

namespace Whistler.Fractions
{
    class Mafia : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Mafia));
        public static Dictionary<int, Vector3> EnterPoints = new Dictionary<int, Vector3>()
        {
            //{ 13, new Vector3() },
        };
        public static Dictionary<int, Vector3> ExitPoints = new Dictionary<int, Vector3>()
        {
            //{ 13, new Vector3() },
        };

        public static Dictionary<int, Vector3> EndCol = new Dictionary<int, Vector3>()
        {
            { 0, new Vector3(484.2206, -1305.217, 29.27462)  },
            { 1, new Vector3(735.8679, -1071.442, 22.23295)  },
            { 2, new Vector3(2132.932, 4775.882, 40.97028)   },
            { 3, new Vector3(102.974, 6623.538, 31.82852)    },
        };

        private static int RandomNumber(int max, int min = 0)
        {
            try
            {
                Random rdm = new Random();
                return rdm.Next(min, max);
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT \"RandomNumber\":\n" + e.ToString());
                return 0;
            }

        }

        [ServerEvent(Event.ResourceStart)]
        public void Event_ResourceStart()
        {
            try
            {
                PrisonFib.StartWork();

                foreach (var point in EnterPoints)
                {
                    var fraction = point.Key;

                    InteractShape.Create(point.Value, 1.2f, 2)
                        .AddDefaultMarker()
                        .AddInteraction((player) =>
                        {
                            Manager.enterInterier(player, fraction);
                        }, "interact_28");
                }

                foreach (var point in ExitPoints)
                {
                    var fraction = point.Key;

                    InteractShape.Create(point.Value, 1.2f, 2)
                        .AddDefaultMarker()
                        .AddInteraction((player) =>
                        {
                            Manager.enterInterier(player, fraction);
                        }, "interact_29");
                }
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"GetLastTask\":\n" + e.ToString()); }
        }
    }
}
