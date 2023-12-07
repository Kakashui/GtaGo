using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;

namespace Whistler.PlayerEffects.Configs
{
    public class EffectConfigCamera: EffectConfigBase
    {
        public EffectConfigCamera(EffectNames id)
        {
            Id = (int)id;
            Type = EffectTypes.Camera;
        }
    }
}
