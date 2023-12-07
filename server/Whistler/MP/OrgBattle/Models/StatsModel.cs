using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Common.Interfaces;

namespace Whistler.MP.OrgBattle.Models
{
    class StatsModel
    {
        public int FamilyId { get; set; }
        public string FamilyName { get; set; }
        public int Kills { get; set; }
        public int Points { get; set; }
        public StatsModel(IOrganization org)
        {
            FamilyId = org.Id;
            Kills = 0;
            Points = 0;
            FamilyName = org.GetName();
        }

    }
}
