using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Fractions.Models
{
    public class FractionRank
    {
        public string RankName { get; set; }
        public int PayDay { get; set; }
        public FractionRank(string rankName, int payDay)
        {
            RankName = rankName;
            PayDay = payDay;
        }
    }
}
