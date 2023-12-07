using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;

namespace Whistler.Phone.Calls
{
    // TODO: Remake it with CallsManager.
    internal class FakeCallsApi : Script
    {
        public static void MakeCall(Player player, string callerName)
        {
            player.TriggerEventSafe("phone:calls:setFakeCall", callerName);
        }

        public static void EndCall(Player player)
        {
            player.TriggerEventSafe("phone:calls:endFakeCall");
        }
    }
}
