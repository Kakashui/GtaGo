using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Whistler.SDK
{
    class SitConfigDev
    {
        [JsonProperty("model")]
        public int Model { get; set; }
        [JsonProperty("places")]
        public List<SitPlaceDev> Places  { get; set; }
        [JsonProperty("anim")]
        public int Animation { get; set; }
        [JsonIgnore]
        public bool InDev { get; set; }

    }
}
