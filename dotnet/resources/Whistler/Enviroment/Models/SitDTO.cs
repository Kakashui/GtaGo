using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Whistler.Enviroment.Models
{
    public class SitDTO
    {
        public SitDTO(int model, Vector3 position, Vector3 rotation, int place)
        {
            Model = model;
            Place = place;
            Position = position;
            Rotation = rotation;
        }
        [JsonProperty("model")]
        public int Model { get; set; }
        [JsonProperty("pos")]
        public Vector3 Position { get; set; }
        [JsonProperty("rot")]
        public Vector3 Rotation { get; set; }
        [JsonProperty("place")]
        public int Place { get; set; }
    }
}
