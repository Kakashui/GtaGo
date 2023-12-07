using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.SDK;
using Whistler.Fractions.Models;
using Whistler.Fractions.Configurations;
using Whistler.Helpers;
using Whistler.Core;
using Whistler.Common;

namespace Whistler.Fractions
{
    class Configs : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Configs));
        private static FractionConfig _defaultConfig = new FractionConfig("Unknown", InteractShape.DefaultMarkerColor, OrgActivityType.Invalid);
        public static readonly Dictionary<int, FractionConfig> FracConfigs = new Dictionary<int, FractionConfig>
        {
            { 1,  new FractionConfig("FAMILY", new Color(56, 102, 56), OrgActivityType.Crime)},
            { 2,  new FractionConfig("BALLAS", new Color(67, 57, 111), OrgActivityType.Crime)},
            { 3,  new FractionConfig("VAGOS", new Color(206, 169, 13), OrgActivityType.Crime)},
            { 4,  new FractionConfig("MARABUNTA", new Color(93, 182, 229), OrgActivityType.Crime)},
            { 5,  new FractionConfig("BLOOD", new Color(224, 50, 50), OrgActivityType.Crime)},

            { 6,  new FractionConfig("CITY", InteractShape.DefaultMarkerColor, OrgActivityType.Government)},
            { 7,  new FractionConfig("POLICE", InteractShape.DefaultMarkerColor, OrgActivityType.Government)},
            { 8,  new FractionConfig("EMS", InteractShape.DefaultMarkerColor, OrgActivityType.Government)},
            { 9,  new FractionConfig("FIB", InteractShape.DefaultMarkerColor, OrgActivityType.Government)},

            { 10, new FractionConfig("LCN", new Color(240, 200, 80), OrgActivityType.Mafia)},
            { 11, new FractionConfig("RUSSIAN", new Color(216, 215, 216), OrgActivityType.Mafia)},
            { 12, new FractionConfig("YAKUZA", new Color(99, 15, 12), OrgActivityType.Mafia)},
            { 13, new FractionConfig("Georgian", InteractShape.DefaultMarkerColor, OrgActivityType.Mafia)},

            { 14, new FractionConfig("ARMY", InteractShape.DefaultMarkerColor, OrgActivityType.Government)},
            { 15, new FractionConfig("LSNEWS", InteractShape.DefaultMarkerColor, OrgActivityType.Government)},

            { 16, new FractionConfig("THELOST", InteractShape.DefaultMarkerColor, OrgActivityType.Crime)},
            { 17, new FractionConfig("GOVERNMENT", InteractShape.DefaultMarkerColor, OrgActivityType.Government)},
        };
        public static FractionConfig GetConfigOrDefault(int fractionId)
        {
            if (FracConfigs.ContainsKey(fractionId)) return FracConfigs[fractionId];
            return _defaultConfig;
        }
    }
}
