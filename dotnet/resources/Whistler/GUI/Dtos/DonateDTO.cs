using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Families.Models;

namespace Whistler.GUI
{
    class DonateDTO
    {
        public int premiumPrice { get; set; }
        public MoneyDonate money { get; set; }
        public ExclusiveDonate  exclusive { get; set; }
        public int exclusiveCount { get; set; }
        public int exclusiveMaxCount { get; set; }
        public int exclusivePrice { get; set; }
    }
    class MoneyDonate
    {
        public int id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int imgid { get; set; }

    }

    class ExclusiveDonate
    {
        public int id { get; set; }
        public int count { get; set; }
        public int maxcount { get; set; }
        public int price { get; set; }
    }
}
