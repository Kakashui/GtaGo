using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.AlcoholBar.Models
{
    internal class Basket
    {
        public string Name;
        public int Count;

        public Basket(string name, int count)
        {
            Name = name;
            Count = count;
        }
    }
}
