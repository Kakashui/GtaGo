using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Core.ReportSystem
{
    class ReportConfigs
    {
        public static readonly byte adminLvL = 1;
        public static readonly byte adminLvLMoneyTransfer = 5;
        public static readonly DateTime StartDate = new DateTime(1970, 1, 1, 0, 0, 0);
        public static readonly float MinRatingForBest = 4.5F;
        public static readonly float MinNumberRatingForBest = 20;

    }
}
