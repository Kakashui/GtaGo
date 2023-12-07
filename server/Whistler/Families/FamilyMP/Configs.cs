using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Families.FamilyMP.Models;

namespace Whistler.Families.FamilyMP
{
    class Configs
    {
        public static readonly Dictionary<FamilyMPType, ConfigMP> ConfigMPList = new Dictionary<FamilyMPType, ConfigMP>
        {
            { FamilyMPType.IslandCapture, new ConfigMP("fam:mp:1", "fam:mp:2", "fam:mp:3")},
            { FamilyMPType.BusinessWar, new ConfigMP("fam:mp:4", "fam:mp:5", "fam:mp:6")},
        };
    }
}
