using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Businesses.Models
{
    public class BuyProductModel
    {
        public int Price { get; set; }
        public int MaterialsAmount { get; set; }
        public BuyProductModel(int price, int materialsAmount)
        {
            Price = price;
            MaterialsAmount = materialsAmount;
        }
    }
}
