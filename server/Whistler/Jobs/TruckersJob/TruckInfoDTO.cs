using Newtonsoft.Json;

namespace Whistler.Jobs.TruckersJob
{
    internal class TruckInfoDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("available")]
        public bool Available { get; set; }

        [JsonProperty("rentCost")]
        public int RentCost { get; set; }
        
        [JsonProperty("availableLevel")]
        public int AvailableLevel { get; set; }
        
        [JsonProperty("reward")]
        public int? Reward { get; set; }
        
        [JsonProperty("payment")]
        public int Payment { get; set; }
    }
}