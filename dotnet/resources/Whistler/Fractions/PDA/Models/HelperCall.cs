using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.Character;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.Fractions.PDA.Models
{
    class HelperCall
    {
        public int id { get; }
        public int UUID { get; }
        public string name { get; }
        public HelperCall(ExtPlayer player)
        {
            id = player.Value;
            UUID = player.Character.UUID;
            name = player.Character.FullName;
        }
    }
}
