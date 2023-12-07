using Newtonsoft.Json;

namespace Whistler.MP.Arena.UI.DTO
{
    public class LobbyMemberDTO
    {
        
        [JsonProperty("playerName")]
        public string PlayerName { get; set; }

        [JsonProperty("battleId")]
        public int LobbyId { get; set; }

        [JsonProperty("team")]
        public string TeamName { get; set; }
    }
}