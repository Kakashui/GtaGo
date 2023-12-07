using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.MiniGames.MakeWeapon
{
    public class WeaponModel
    {
        public WeaponModel(int id, string name, int price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "price")]
        public int Price { get; set; }
    }
}
