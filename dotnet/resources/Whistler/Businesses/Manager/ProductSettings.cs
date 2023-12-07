using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Businesses
{
    public class ProductSettings
    {
        public string Name { get; set; }
        public int OrderPrice { get; set; }
        public string MaxMinType { get; set; }
        public int MaxPrice { get; set; }
        public int MinPrice { get; set; }
        public int StockCapacity { get; set; }
    }
}
