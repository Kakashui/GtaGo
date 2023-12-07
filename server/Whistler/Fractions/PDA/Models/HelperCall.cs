using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.Character;
using Whistler.Helpers;

namespace Whistler.Fractions.PDA.Models
{
    class HelperCall
    {
        public int id { get; }
        public int UUID { get; }
        public string name { get; }
        public HelperCall(Player player)
        {
            id = player.Value;
            UUID = player.GetCharacter().UUID;
            name = player.GetCharacter().FullName;
        }
    }
}
