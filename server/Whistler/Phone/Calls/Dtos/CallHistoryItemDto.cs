using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Phone.Calls.Dtos
{
    internal class CallHistoryItemDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("isSuccessful")]
        public bool IsSuccessful { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("date")]
        public int Date { get; set; }
    }
}
