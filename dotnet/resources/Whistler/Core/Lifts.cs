using GTANetworkAPI;
using System;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Core
{
    internal class Lifts : Script
    {
        //private static WhistlerLogger _logger = new WhistlerLogger(typeof(Lifts));

        private static Vector3 GhettoMallFloor1 = new Vector3(68.85433, -1728, 28.62979);
        private static Vector3 GhettoMallFloor2 = new Vector3(71.08394, -1725.969, 34.61863);
        private static Vector3 GhettoMallFloor3 = new Vector3(71.09315, -1725.969, 40.73378);

        [ServerEvent(Event.ResourceStart)]
        public void CreateGhettoMallLift()
        {
            GUI.Lifts.Lift.Create()
                .AddFloor("1 этаж", GhettoMallFloor1, marker: false)
                .AddFloor("2 этаж", GhettoMallFloor2, marker: false)
                .AddFloor("3 этаж", GhettoMallFloor3, marker: false);
        }

    }
}
