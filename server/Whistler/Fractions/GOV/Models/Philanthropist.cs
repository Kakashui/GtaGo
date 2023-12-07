using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Fractions.GOV.Models
{
    class Philanthropist
    {
        public int Uuid { get; set; }
        public long Amount { get; set; }
        public Philanthropist(int uuid, long amount)
        {
            Uuid = uuid;
            Amount = amount;
        }

    }
    class CompPhilanthropist<T> : IComparer<T> //sort by val asc
        where T : Philanthropist
    {
        public int Compare(T x, T y)
        {
            if (x.Amount > y.Amount)  //sorted desc
                return -1;
            else if (x.Amount < y.Amount)
                return 1;
            return 0;
        }
    }
}
