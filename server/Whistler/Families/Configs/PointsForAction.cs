using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Families.Configs
{
    class PointsForAction
    {
        public static Dictionary<FamilyActions, int> ListPoints = new Dictionary<FamilyActions, int>
        {
            { FamilyActions.GoToDemorgan, -10 },
            { FamilyActions.AddMoney, 1 },
            { FamilyActions.SubMoney, -1 },
        };
    }
}
