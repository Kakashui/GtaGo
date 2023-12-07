using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.Core
{
    class AntiAFK : Script
    {
        [RemoteEvent("antiafk:setAfk")]
        public static void SetAfk(ExtPlayer player, bool isAfk)
        {
            if (!player.IsLogged())
                return;
            if (isAfk)
                player.Character.AfkMinuteInHours += 15;
            else
            {
                var addMinutes = player.Character.LastTriggerAFK.Minute > DateTime.Now.Minute ? DateTime.Now.Minute : DateTime.Now.Minute - player.Character.LastTriggerAFK.Minute; 
                player.Character.AfkMinuteInHours += addMinutes;
            }
            
            player.Character.IsAFK = isAfk;
            player.Character.LastTriggerAFK = DateTime.Now;
        }
    }
}
