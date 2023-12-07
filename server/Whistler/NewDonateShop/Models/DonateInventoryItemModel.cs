using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.NewDonateShop.Models
{
    class DonateInventoryItemModel
    {
        public DonateInventoryItemModel(int id, int count, bool sell)
        {
            Id = id;
            Count = count;
            Sell = sell;
        }
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [JsonProperty(PropertyName = "sell")]
        public bool Sell { get; set; }
    }
}
