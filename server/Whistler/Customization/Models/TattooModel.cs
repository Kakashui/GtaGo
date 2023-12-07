using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Customization.Models
{
    public class TattooModel
    {
        public string Dictionary { get; set; }
        public string Hash { get; set; }
        public List<int> Slots { get; set; }

        public TattooModel(string dictionary, string hash, List<int> slots)
        {
            Dictionary = dictionary;
            Hash = hash;
            Slots = slots;
        }
    }
}
