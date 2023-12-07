using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.NewDonateShop.Enums;

namespace Whistler.NewDonateShop.Models
{
    class RouletteModel
    {
        public RouletteModel(int id, string name, int price, bool forCoins, string color, string effect, int minPrice, int maxPrice)
        {
            Id = id;
            Name = name;
            Price = price;
            ForCoins = forCoins;
            Color = color;
            Effect = effect;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
        }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "price")]
        public int Price { get; set; }
        [JsonProperty(PropertyName = "forCoins")]
        public bool ForCoins { get; set; }
        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }
        [JsonProperty(PropertyName = "effect")]
        public string Effect { get; set; }
        [JsonProperty(PropertyName = "minPrice")]
        public int MinPrice { get; set; }
        [JsonProperty(PropertyName = "maxPrice")]
        public int MaxPrice { get; set; }
        [JsonProperty(PropertyName = "items")]
        public List<int> Items { get; set; }
        [JsonIgnore]
        public List<ItemModel> ItemLinks { get; set; }

    }
}
