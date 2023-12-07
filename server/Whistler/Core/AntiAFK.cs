using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Helpers;

namespace Whistler.Core
{
    class AntiAFK : Script
    {
        [RemoteEvent("antiafk:setAfk")]
        public static void SetAfk(Player player, bool isAfk)
        {
            if (!player.IsLogged())
                return;
            if (isAfk)
                player.GetCharacter().AfkMinuteInHours += 15;
            else
            {
                var addMinutes = player.GetCharacter().LastTriggerAFK.Minute > DateTime.Now.Minute ? DateTime.Now.Minute : DateTime.Now.Minute - player.GetCharacter().LastTriggerAFK.Minute; 
                player.GetCharacter().AfkMinuteInHours += addMinutes;
            }
            
            player.GetCharacter().IsAFK = isAfk;
            player.GetCharacter().LastTriggerAFK = DateTime.Now;
        }
    }
}
