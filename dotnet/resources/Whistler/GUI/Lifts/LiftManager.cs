using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Entities;

namespace Whistler.GUI.Lifts
{
    class LiftManager : Script
    {
        [RemoteEvent("lift:callBack")]
        public static void LiftCallBack(ExtPlayer player, int liftId, int floor)
        {
            Lift.LiftPressButton(player, liftId, floor);
        }
    }
}
