using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Phone.Contacts.Dtos
{
    internal class BlockDto
    {
        [JsonProperty("number")]
        public int TargetNumber { get; set; }
    }
}
