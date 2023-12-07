using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.Phone.Calls
{
    // TODO: Remake it with CallsManager.
    internal class FakeCallsApi : Script
    {
        public static void MakeCall(ExtPlayer player, string callerName)
        {
            player.TriggerEventSafe("phone:calls:setFakeCall", callerName);
        }

        public static void EndCall(ExtPlayer player)
        {
            player.TriggerEventSafe("phone:calls:endFakeCall");
        }
    }
}
