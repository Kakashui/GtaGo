using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Inventory.Models;

namespace Whistler.AlcoholBar.Configs
{
    class AlcoItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("img")]
        public string Image { get; set; }
        [JsonProperty("price")]
        public int Price { get; set; }
        [JsonProperty("sale")]
        public int Discount { get; set; }
        [JsonIgnore]
        public Alcohol Item { get; set; }

    }
}
