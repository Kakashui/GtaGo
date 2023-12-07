using System;
using System.Collections.Generic;
using System.Text;
using Whistler.GUI.Documents.Enums;
using Whistler.GUI.Documents.Models;

namespace Whistler.GUI.Documents
{
    class DocumentConfigs
    {

        private static readonly Dictionary<LicenseName, LicConfig> LicenseConfig = new Dictionary<LicenseName, LicConfig>
        {
            { LicenseName.Moto, new LicConfig(9999, 25000, "A")},
            { LicenseName.Auto, new LicConfig(9999, 35000, "B")},
            { LicenseName.Truck, new LicConfig(9999, 45000, "C")},
            { LicenseName.Both, new LicConfig(9999, 55000, "D")},
            { LicenseName.Helicopter, new LicConfig(9999, 65000, "E")},
            { LicenseName.Fly, new LicConfig(9999, 75000, "F")},
            { LicenseName.Weapon, new LicConfig(9999, 15000, "G")},
            { LicenseName.Medical, new LicConfig(30, 50000, "MED")},
            { LicenseName.Military, new LicConfig(9999, 50000, "MID")},
            { LicenseName.Taxi, new LicConfig(30, 5000, "TAXI")},
            { LicenseName.MakingWeapon, new LicConfig(30, 15000, "CW")},
            { LicenseName.MiningOre, new LicConfig(30, -1, "MO")},
            { LicenseName.Hunting, new LicConfig(30, -1, "H")},
            { LicenseName.Trucker, new LicConfig(30, 8000, "TR")},
            { LicenseName.Fishing, new LicConfig(30, 1500, "FS")},
            { LicenseName.MetalPlantWorker, new LicConfig(9999, -1, "MPW") }
        };

        public static int GetLicenseDuration(LicenseName name)
        {
            if (LicenseConfig.ContainsKey(name))
                return LicenseConfig[name].DayDuration;
            return 30;
        }

        public static string GetLicenseWord(LicenseName name)
        {
            if (LicenseConfig.ContainsKey(name))
                return LicenseConfig[name].Word;
            return $"ERR_{name}";
        }

        public static int GetLicensePrice(LicenseName name)
        {
            if (LicenseConfig.ContainsKey(name))
                return LicenseConfig[name].Price;
            return -1;
        }
    }
}
