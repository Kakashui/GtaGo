using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;

namespace Whistler.PlayerEffects.Configs
{
    class EffectConfigShot: EffectConfigBase
    {
        public EffectConfigShot(EffectNames id, string asset, string name, float scale = 1.0f)
        {
            Id = (int)id;
            Type = EffectTypes.Shot;
            Asset = asset;
            Name = name;
            Scale = scale;
        }
        [JsonProperty("asset")]
        public string Asset { get; set; }
        [JsonProperty("scale")]
        public float Scale { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
