using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Phone.Contacts.Dtos
{
    public class ContactDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }
    }
}
