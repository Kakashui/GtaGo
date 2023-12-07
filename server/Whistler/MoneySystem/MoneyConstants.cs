using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.MoneySystem
{
    class MoneyConstants
    {
        public static double PayTaxCoeffForHour = 0.013 * 1.9 * 0.01;
        public static double PayTaxCoeffForDay = PayTaxCoeffForHour * 24;
    }
}
