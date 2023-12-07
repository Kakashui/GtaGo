using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.SDK
{
    class SitPlaceDev
    {

        [JsonProperty("pos")]
        public Vector3 Position { get; set; }
        [JsonProperty("rot")]
        public Vector3 Rotation { get; set; }
    }
}
