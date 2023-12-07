using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Families.FamilyMP.Models
{
    class FamilyStatsModel
    {
        public int FamilyId { get; set; }
        public string FamilyName { get; set; }
        public int Kills { get; set; }
        public int Points { get; set; }
        public FamilyStatsModel(int familyId)
        {
            FamilyId = familyId;
            Kills = 0;
            Points = 0;
            FamilyName = FamilyManager.GetFamilyName(FamilyId);
        }

    }
}
