using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.MiniGames.Lockpik
{
    public static class LockpickService
    {
        public static void StartLockpickGame(ExtPlayer player, string callbackEvent)
        {
            SafeTrigger.ClientEvent(player,"mg:lockpick:open", callbackEvent);
        }
    }
}
