using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Common;

namespace Whistler.Fractions.Configurations
{
    public class FractionConfig
    {
        public string Name { get; set; }
        public Color FracColor { get; set; }
        public OrgActivityType TypeFraction { get; set; }
        public FractionConfig(string name, Color fracColor, OrgActivityType typeFraction)
        {
            Name = name;
            FracColor = fracColor;
            TypeFraction = typeFraction;
        }
    }
}
