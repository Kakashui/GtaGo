using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Businesses.Models
{
    class CarWashConfProd
    {
        public CarWashConfProd(int countProds, int minuteEffect, bool isWash)
        {
            CountProds = countProds;
            MinuteEffect = minuteEffect;
            IsWash = isWash;
        }

        public int CountProds { get; set; }
        public int MinuteEffect { get; set; }
        public bool IsWash { get; set; }
        
    }
}
