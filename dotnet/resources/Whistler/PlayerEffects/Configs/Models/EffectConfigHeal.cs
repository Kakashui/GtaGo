using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.PlayerEffects.Configs.Models;
using Whistler.SDK;

namespace Whistler.PlayerEffects.Configs
{
    class EffectConfigHeal: EffectConfigBase
    {
        public EffectConfigHeal(EffectNames id, int amount)
        {
            Id = (int)id;
            Type = EffectTypes.Heal;
            Amount = amount;
        }
        [JsonProperty("amount")]
        public int Amount { get; set; }

        public override void Use(ExtPlayer player, int time)
        {
            if (player.HasData("effect:heal"))
            {
                HealTimer timer = player.GetData<HealTimer>("effect:heal");
                timer.ChangeEffect(time, Amount);
            }else
                SafeTrigger.SetData(player, "effect:heal", new HealTimer(player, time, Amount));
        }
    }
}
