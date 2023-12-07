using System.Collections.Generic;
using Newtonsoft.Json;

namespace Whistler.MP.Arena.UI.DTO
{
    internal class StrikeLobbyDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("owner")]
        public string OwnerName { get; set; }
        
        [JsonProperty("isMapChange")]
        public bool IsMapChanging { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("maxPlayers")]
        public int MaxPlayers { get; set; }
        
        [JsonProperty("started")]
        public bool IsStarted { get; set; }
        
        [JsonProperty("rate")]
        public int EntryBet { get; set; }
        
        [JsonProperty("weapons")]
        public string WeaponName { get; set; }
        
        [JsonProperty("rounds")]
        public int TotalRounds { get; set; }
        
        [JsonProperty("redTeam")]
        public List<string> RedTeamPlayerNames { get; set; }
        
        [JsonProperty("greenTeam")]
        public List<string> GreenTeamPlayerNames { get; set; }
    }
}