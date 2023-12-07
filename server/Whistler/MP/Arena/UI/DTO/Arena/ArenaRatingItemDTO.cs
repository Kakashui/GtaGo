using Newtonsoft.Json;

namespace Whistler.MP.Arena.UI.DTO
{
    internal class ArenaRatingItemDTO
    {
        [JsonProperty("username")]
        public string Name { get; set; }

        [JsonProperty("place")]
        public int Place { get; set; }

        [JsonProperty("points")]
        public int TotalPoints { get; set; }
    }
}