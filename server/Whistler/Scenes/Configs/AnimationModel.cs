using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Whistler.Scenes.Configs
{
    class AnimationModel
    {
        public AnimationModel(){}
        public AnimationModel(string dict, string name, int flag, float start, float stop)
        {
            Dictionary = dict;
            Name = name;
            Flag = flag;
            Start = start;
            Stop = stop;
        }
        [JsonProperty("dictionary")]
        public string Dictionary { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("flag")]
        public int Flag { get; set; }
        [JsonProperty("start")]
        public float Start { get; set; }
        [JsonProperty("stop")]
        public float Stop { get; set; }
    }
}
