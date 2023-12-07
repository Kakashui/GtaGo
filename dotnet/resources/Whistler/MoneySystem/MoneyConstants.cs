using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.MoneySystem
{
    class MoneyConstants
    {
        public static double PayTaxCoeffBusinessForHour = 0.0002;
        public static double PayTaxCoeffHouseForHour = 0.00014;
        public static double PayTaxCoeffBusinessForDay = PayTaxCoeffBusinessForHour * 24;
        public static double PayTaxCoeffHouseForDay = PayTaxCoeffHouseForHour * 24;
    }
}
