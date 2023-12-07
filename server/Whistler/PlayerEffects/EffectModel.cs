using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.PlayerEffects
{
    public class EffectModel
    {
        public EffectModel(EffectNames name, int delay, int time)
        {
            Name = name;
            Delay = delay;
            Time = time;
        }

        public EffectNames Name { get; set; }
        public int Delay { get; set; }
        public int Time { get; set; }
    }
}
