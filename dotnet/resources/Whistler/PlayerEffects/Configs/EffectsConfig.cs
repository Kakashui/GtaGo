using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Whistler.PlayerEffects.Configs
{
    public class EffectsConfig
    {
        private Dictionary<EffectNames, EffectConfigBase> _configs;
        public EffectsConfig()
        {
            _configs = new Dictionary<EffectNames, EffectConfigBase>();
            Add(new EffectConfigCamera(EffectNames.CameraShake));
            Add(new EffectConfigShot(EffectNames.ShotPole, "core", "ent_sht_telegraph_pole", .8f));
            Add(new EffectConfigShot(EffectNames.ShotBubble, "core", "ent_dst_gen_gobstop", 1.5f));
            Add(new EffectConfigShot(EffectNames.ShotElectro, "core", "ent_dst_elec_fire", 2.0f));
            Add(new EffectConfigShot(EffectNames.ShotBlood, "core", "blood_stab", 2.5f));
            Add(new EffectConfigShot(EffectNames.ShotMetal, "core", "ent_brk_metal_frag", 2.5f));
            Add(new EffectConfigShot(EffectNames.ShotFlashlight, "core", "ent_anim_paparazzi_flash",3f));
            Add(new EffectConfigShot(EffectNames.ShotDust, "core", "mel_carmetal", 2.5f));
            Add(new EffectConfigScreen(EffectNames.ScreenDragMichael, "DrugsMichaelAliensFightIn"));
            Add(new EffectConfigScreen(EffectNames.ScreenDrugBiker, "BikerFilter"));
            Add(new EffectConfigScreen(EffectNames.ScreenBeastLaunch, "BeastLaunch"));
            Add(new EffectConfigScreen(EffectNames.ScreenDrugGanja, "CrossLine"));
            Add(new EffectConfigHeal(EffectNames.HealLight, 1));
            Add(new EffectConfigHeal(EffectNames.HealMedium, 2));
            Add(new EffectConfigHeal(EffectNames.HealHight, 3));

            Parse();
        }
        public EffectConfigBase this[EffectNames name]
        {
            get
            {
                var effect = _configs[name] ?? null;
                if (effect == null)
                {
                    Console.WriteLine($"No config for {name}");
                }
                return effect;
            }
        }
        private void Add(EffectConfigBase config)
        {
            _configs.Add((EffectNames)config.Id, config);
        }
        private void Parse()
        {
            if (Directory.Exists("client/configs"))
            {
                using var w = new StreamWriter("client/configs/effects.js");
                w.Write($"module.exports = {JsonConvert.SerializeObject(_configs.ToDictionary(e=>(int)e.Key, e=>e.Value), Formatting.Indented)}");
            }
        }
    }
}
