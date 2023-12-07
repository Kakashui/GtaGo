using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Businesses.Models
{
    public class ProductPrice
    {
        public int OrderPrice { get; set; }
        public int CurrentPrice { get; set; }
        public int Lefts { get; set; }
        public ProductPrice(int orderPrice, int currentPrice, int lefts)
        {
            OrderPrice = orderPrice;
            CurrentPrice = currentPrice;
            Lefts = lefts;
        }
    }
}
