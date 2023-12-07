using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.MiniGames.Lockpik
{
    public static class LockpickService
    {
        public static void StartLockpickGame(Player player, string callbackEvent)
        {
            player.TriggerEvent("mg:lockpick:open", callbackEvent);
        }
    }
}
