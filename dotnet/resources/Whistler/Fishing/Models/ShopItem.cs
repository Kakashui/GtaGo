using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Whistler.Fishing.Models
{
    class ShopItem
    {
        float _kof = .5f;
        public ShopItem(int id, string name, int value, int maxPrice, int minPrice = -1, bool donate = false)
        {
            Id = id;
            Name = name;
            _maxPrice = maxPrice;
            _minPrice = minPrice;
            _value = value;
            Name = name;
            Donate = donate;
            UpdatePrice();
        }
        public ShopItem(Fish id, string name, int value, int maxPrice, int minPrice = -1, bool donate = false)
        {
            Id = (int)id;
            Name = name;
            _maxPrice = maxPrice;
            _minPrice = minPrice;
            _value = value;
            Name = name;
            Donate = donate;
            UpdatePrice();
        }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price")]
        public int Price { get; set; }
        
        [JsonProperty("donate")]
        public bool Donate { get; set; }
        private int _minPrice;
        private int _maxPrice;
        private int _value;
        [JsonIgnore]
        public DateTime lastUpdate { get; private set; }
        private void UpdatePrice()
        {
            var price = _minPrice < 0 ? _maxPrice : new Random(DateTime.Now.Millisecond).Next(_minPrice, _maxPrice);
            Price = (int)(price * _kof);
        }

        internal bool HasData()
        {
            return _value > 0;
        }

        internal int GetData()
        {
            return _value;
        }
    }
}
