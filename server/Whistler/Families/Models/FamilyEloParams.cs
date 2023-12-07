using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Families.FamilyWars
{
    public class FamilyEloParams
    {    
        public int id { get; set; }
        public string name { get; set; }
        public string owner { get; set; }
        public int victories { get; set; }
        public int buissCount { get; set; }
        public int membersCount { get; set; }
        public int rating { get; set; }
        public int rank { get; set; }
        public FamilyEloParams(int id, string name, string owner, int victories, int buissCount, int membersCount, int rating, int rank)
        {
            this.id = id;
            this.name = name;
            this.owner = owner;
            this.victories = victories;
            this.buissCount = buissCount;
            this.membersCount = membersCount;
            this.rating = rating;
            this.rank = rank;
        }
    }
}
