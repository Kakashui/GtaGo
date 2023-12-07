using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;

namespace Whistler.PlayerEffects.Configs
{
    public abstract class EffectConfigBase
    {

        [JsonIgnore]
        public int Id { get; set; }
        [JsonProperty("type")]
        public EffectTypes Type { get; set; }

        public virtual void Use(Player player, int time)
        {
            player.TriggerEvent("effect:add", Id, time);
        }
    }
}
