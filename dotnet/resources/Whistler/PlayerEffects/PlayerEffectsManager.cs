using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.PlayerEffects.Configs;
using Whistler.PlayerEffects.Configs.Models;

namespace Whistler.PlayerEffects
{
    public class PlayerEffectsManager: Script
    {

        private static EffectsConfig _config;        
        public PlayerEffectsManager()
        {
            _config = new EffectsConfig();
        }
        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnect(ExtPlayer player, DisconnectionType type, string reason)
        {
            if (player.HasData("effect:heal"))
            {
                HealTimer timer = player.GetData<HealTimer>("effect:heal");
                timer.Destroy();
            }
        }
        public static void AddEffect(ExtPlayer player, EffectNames name, int time)
        {
            _config[name]?.Use(player, time);            
        }
    }
}
