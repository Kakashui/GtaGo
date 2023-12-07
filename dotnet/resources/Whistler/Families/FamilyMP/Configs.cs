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
            { FamilyMPType.IslandCapture, new ConfigMP("Битва за остров", "2000$ каждому выжившему члену победившей семьи на острове и ящики оружием", "Захват острова")},
            { FamilyMPType.BusinessWar, new ConfigMP("Битва за бизнес", "Бизнес для семьи", "Захват бизнеса")},
        };
    }
}
