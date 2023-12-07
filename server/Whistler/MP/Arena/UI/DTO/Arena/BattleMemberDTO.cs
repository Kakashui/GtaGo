using Newtonsoft.Json;

namespace Whistler.MP.Arena.UI.DTO
{
    internal class BattleMemberDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("username")]
        public string Name { get; set; }
        
        [JsonProperty("kills")]
        public int Kills { get; set; }
        
        [JsonProperty("deaths")]
        public int Deaths { get; set; }
    }
}