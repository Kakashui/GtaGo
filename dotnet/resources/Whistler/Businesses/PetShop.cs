using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.SDK;

namespace Whistler.Core
{
    partial class BusinessManager : Script
    {
        public static List<string> PetNames = new List<string>() {
            "Husky",
            "Poodle",
            "Pug",
            "Retriever",
            "Rottweiler",
            "Shepherd",
            "Westy",
            "Cat",
            "Rabbit",
        };
        public static List<int> PetHashes = new List<int>() {
            1318032802, // Husky
            1125994524,
            1832265812,
            882848737, // Retriever
            -1788665315,
            1126154828,
            -1384627013,
            1462895032,
            -541762431,
        };

        public static void enterPetShop(ExtPlayer player, string prodname)
        {
        }
    }
}
