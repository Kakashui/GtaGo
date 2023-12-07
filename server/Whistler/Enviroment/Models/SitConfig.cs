using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Whistler.Enviroment.Models
{
    class SitConfig
    {
        [JsonProperty("model")]
        public int Model { get; set; }
        [JsonProperty("places")]
        public List<SitPlace> SitPlaces { get; set; }
        [JsonProperty("anim")]
        public int Animation { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; } = "sit";
        public int TakePlace(uint hash)
        {
            for (int i = 0; i < SitPlaces.Count; i++)
            {
                if (SitPlaces[i].TakePlace(hash))
                    return i;
            }
            return -1;
        }
        public void FreePlace(uint hash, int position)
        {
            if (position < 0 || SitPlaces.Count <= position) return;
            SitPlaces[position].FrePlace(hash);
        }
    }
}
