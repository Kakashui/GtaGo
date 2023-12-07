using Newtonsoft.Json;

namespace Whistler.MP.Arena.UI.DTO
{
    public class CurrentRacingDto
    {
        [JsonProperty("eventId")]
        public int Id { get; set; }

        [JsonProperty("players")]
        public int TotalPlayers { get; set; }
        
        [JsonProperty("countStartTime")]
        public string CountStartTime { get; set; }
        
        [JsonProperty("bestTime")]
        public string BestTime { get; set; }
        
        [JsonProperty("startTime")]
        public string StartTime { get; set; }
    }
}