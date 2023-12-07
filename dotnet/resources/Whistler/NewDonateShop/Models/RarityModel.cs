using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Whistler.NewDonateShop.Models
{
    class RarityModel
    {
        public RarityModel(int chance)
        {
            Chance = chance;
        }
        public RarityModel()
        {
        }

        public int Chance { get; set; }
    }
}
