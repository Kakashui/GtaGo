using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Businesses.Models
{
    class BuyModel
    {
        public string ProductName { get; set; }
        public int CountProduct { get; set; }
        public bool OnlyAllProduct { get; set; }
        public Func<int, int> BuyFunc { get; set; }
        public BuyModel(string productName, int countProduct, bool onlyAllProduct, Func<int, int> buyFunc)
        {
            ProductName = productName;
            CountProduct = countProduct;
            OnlyAllProduct = onlyAllProduct;
            BuyFunc = buyFunc;
        }
    }
}
