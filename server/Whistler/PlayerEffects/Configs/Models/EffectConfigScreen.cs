using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;

namespace Whistler.PlayerEffects.Configs
{
    class EffectConfigScreen: EffectConfigBase
    {
        public EffectConfigScreen(EffectNames id, string name)
        {
            Id = (int)id;
            Type = EffectTypes.Screen;
            Name = name;
        }     
      
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
