using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Phone.Taxi.Dtos
{
    internal class TaxiOrderDto
    {
        public int ID { get; set; }

        [JsonProperty("Destination")]
        public Vector3 Start { get; set; }
    }
}
