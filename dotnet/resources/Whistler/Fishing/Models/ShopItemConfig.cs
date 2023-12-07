using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Fishing.Models
{
    class ShopItemConfig
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public bool Donate { get; set; }
    }
}
