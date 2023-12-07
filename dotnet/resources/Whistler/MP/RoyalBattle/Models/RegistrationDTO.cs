using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.MP.RoyalBattle.Models
{
    internal class RegistrationDTO
    {
        [JsonProperty("isReg")]
        public bool IsRegistered;

        [JsonProperty("peopleCount")]
        public int PeopleCount;

        public RegistrationDTO(bool isRegistered, int peopleCount)
        {
            IsRegistered = isRegistered;
            PeopleCount = peopleCount;
        }
    }
}
