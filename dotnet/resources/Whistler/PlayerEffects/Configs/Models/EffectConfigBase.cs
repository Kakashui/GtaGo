using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.PlayerEffects.Configs
{
    public abstract class EffectConfigBase
    {

        [JsonIgnore]
        public int Id { get; set; }
        [JsonProperty("type")]
        public EffectTypes Type { get; set; }

        public virtual void Use(ExtPlayer player, int time)
        {
            SafeTrigger.ClientEvent(player,"effect:add", Id, time);
        }
    }
}
