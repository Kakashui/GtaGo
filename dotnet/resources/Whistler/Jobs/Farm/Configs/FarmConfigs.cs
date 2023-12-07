using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Inventory.Enums;
using Whistler.Jobs.Farm.Configs.Models;
using Whistler.Jobs.Farm.Models;

namespace Whistler.Jobs.Farm.Configs
{
    class FarmConfigs : Script
    {
        public const float CellCoef = 1.0f;
        public FarmConfigs()
        {
            Dictionary<int, PlantConfig> plants = new Dictionary<int, PlantConfig>();
            foreach (var item in PlantConfigsList)
            {
                plants.Add((int)item.Key, item.Value);
            }

            Dictionary<int, BonusConfig> pits = new Dictionary<int, BonusConfig>();
            foreach (var item in PitConfigsList)
            {
                pits.Add((int)item.Key, item.Value);
            }

            Dictionary<int, BonusConfig> fertilizers = new Dictionary<int, BonusConfig>();
            foreach (var item in FertilizerConfigsList)
            {
                fertilizers.Add((int)item.Key, item.Value);
            }
            if (Directory.Exists("client/farms"))
            {
                using (var w = new StreamWriter("client/farms/config.js"))
                {
                    w.WriteLine($"const pointsConfig = JSON.parse(`{JsonConvert.SerializeObject(_configsByFarmId)}`);");
                    w.WriteLine($"const plantsConfig = JSON.parse(`{JsonConvert.SerializeObject(plants)}`);");
                    w.WriteLine($"const pitsConfig = JSON.parse(`{JsonConvert.SerializeObject(pits)}`);");
                    w.WriteLine($"const fertilizersConfig = JSON.parse(`{JsonConvert.SerializeObject(fertilizers)}`);");
                    w.WriteLine($"const productConfigs = JSON.parse(`{JsonConvert.SerializeObject(ProductConfigs)}`);");
                    w.WriteLine($"module.exports.pointsConfig = pointsConfig; ");
                    w.WriteLine($"module.exports.plantsConfig = plantsConfig; ");
                    w.WriteLine($"module.exports.pitsConfig = pitsConfig; ");
                    w.WriteLine($"module.exports.fertilizersConfig = fertilizersConfig; ");
                    w.WriteLine($"module.exports.productConfigs = productConfigs; ");
                }
            };
        }

        public static Dictionary<int, List<SeedingPlaceConfig>> _configsByFarmId = new Dictionary<int, List<SeedingPlaceConfig>>
        {
            [0] = new List<SeedingPlaceConfig>
            {
                new SeedingPlaceConfig { ID = 0,   PitType = PitNames.Standart, Position = new Vector3(0, 0, -1) + new Vector3(2045.9579, 4966.468, 41.08101) },
                new SeedingPlaceConfig { ID = 1,   PitType = PitNames.Standart,  Position = new Vector3(0, 0, -1) + new Vector3(2050.1282, 4962.3745, 41.054592) },
                new SeedingPlaceConfig { ID = 2,   PitType = PitNames.Standart,  Position = new Vector3(0, 0, -1) + new Vector3(2054.1465, 4958.3325, 41.040672) },
                new SeedingPlaceConfig { ID = 3,   PitType = PitNames.Standart,     Position = new Vector3(0, 0, -1) + new Vector3(2058.1013, 4954.363, 41.026352) },
                new SeedingPlaceConfig { ID = 4,   PitType = PitNames.Standart,     Position = new Vector3(0, 0, -1) + new Vector3(2061.9998, 4950.464, 41.06165) },
                new SeedingPlaceConfig { ID = 5,   PitType = PitNames.Standart,            Position = new Vector3(0, 0, -1) + new Vector3(2065.9426, 4946.527, 41.04632) },
                new SeedingPlaceConfig { ID = 6,   PitType = PitNames.Standart,            Position = new Vector3(0, 0, -1) + new Vector3(2068.9043, 4943.768, 41.07652) },
                new SeedingPlaceConfig { ID = 7,   PitType = PitNames.Standart,            Position = new Vector3(0, 0, -1) + new Vector3(2065.9802, 4941.055, 41.109406) },
                new SeedingPlaceConfig { ID = 8,   PitType = PitNames.Standart,           Position = new Vector3(0, 0, -1) + new Vector3(2061.9172, 4944.951, 41.07528) },
                new SeedingPlaceConfig { ID = 9,   PitType = PitNames.Standart,           Position = new Vector3(0, 0, -1) + new Vector3(2057.9648, 4948.9385, 41.08867) },
                new SeedingPlaceConfig { ID = 10,  PitType = PitNames.Standart,           Position = new Vector3(0, 0, -1) + new Vector3(2053.9285, 4952.968, 41.094124) },
                new SeedingPlaceConfig { ID = 11,  PitType = PitNames.Standart,           Position = new Vector3(0, 0, -1) + new Vector3(2049.9321, 4956.938, 41.099133) },
                new SeedingPlaceConfig { ID = 12,  PitType = PitNames.Standart,         Position = new Vector3(0, 0, -1) + new Vector3(2045.9905, 4960.877, 41.095623) },
                new SeedingPlaceConfig { ID = 13,  PitType = PitNames.Standart,         Position = new Vector3(0, 0, -1) + new Vector3(2043.3317, 4963.407, 41.11742) },
                new SeedingPlaceConfig { ID = 14,  PitType = PitNames.Standart,         Position = new Vector3(0, 0, -1) + new Vector3(2040.3007, 4960.701, 41.123363) },
                new SeedingPlaceConfig { ID = 15,  PitType = PitNames.Standart,         Position = new Vector3(0, 0, -1) + new Vector3(2044.1658, 4956.8135, 41.100468) },
                new SeedingPlaceConfig { ID = 16,  PitType = PitNames.Standart,         Position = new Vector3(0, 0, -1) + new Vector3(2048.8274, 4952.205, 41.076626) },
                new SeedingPlaceConfig { ID = 17,  PitType = PitNames.Standart,       Position = new Vector3(0, 0, -1) + new Vector3(2052.87, 4948.248, 41.064487) },
                new SeedingPlaceConfig { ID = 18,  PitType = PitNames.Standart,       Position = new Vector3(0, 0, -1) + new Vector3(2057.5642, 4943.648, 41.07362) },
                new SeedingPlaceConfig { ID = 19,  PitType = PitNames.Standart,       Position = new Vector3(0, 0, -1) + new Vector3(2061.711, 4939.7446, 41.104225) },
                new SeedingPlaceConfig { ID = 20,  PitType = PitNames.Standart,       Position = new Vector3(0, 0, -1) + new Vector3(2060.357, 4935.6416, 41.104225) },
                new SeedingPlaceConfig { ID = 21,  PitType = PitNames.Standart,       Position = new Vector3(0, 0, -1) + new Vector3(2055.572, 4940.0835, 41.1101) },
                new SeedingPlaceConfig { ID = 22,  PitType = PitNames.Standart,       Position = new Vector3(0, 0, -1) + new Vector3(2051.0918, 4944.4634, 41.11083) },
                new SeedingPlaceConfig { ID = 23,  PitType = PitNames.Standart,       Position = new Vector3(0, 0, -1) + new Vector3(2046.2723, 4949.235, 41.086346) },
                new SeedingPlaceConfig { ID = 24,  PitType = PitNames.Standart,       Position = new Vector3(0, 0, -1) + new Vector3(2042.0282, 4953.424, 41.076782) },
                new SeedingPlaceConfig { ID = 25,  PitType = PitNames.Standart,       Position = new Vector3(0, 0, -1) + new Vector3(2037.9633, 4957.511, 41.094036) },
                new SeedingPlaceConfig { ID = 26,  PitType = PitNames.Standart,       Position = new Vector3(0, 0, -1) + new Vector3(2034.6786, 4955.05, 41.07003) },
                new SeedingPlaceConfig { ID = 27,  PitType = PitNames.Standart, Position = new Vector3(0, 0, -1) + new Vector3(2039.3677, 4950.4634, 41.072086) },
                new SeedingPlaceConfig { ID = 28,  PitType = PitNames.Standart,  Position = new Vector3(0, 0, -1) + new Vector3(2043.2197, 4946.578, 41.09676) },
                new SeedingPlaceConfig { ID = 29,  PitType = PitNames.Standart,  Position = new Vector3(0, 0, -1) + new Vector3(2047.2227, 4942.582, 41.052048) },
                new SeedingPlaceConfig { ID = 30,  PitType = PitNames.Standart,     Position = new Vector3(0, 0, -1) + new Vector3(2051.2568, 4938.4956, 41.0572) },
                new SeedingPlaceConfig { ID = 31,  PitType = PitNames.Standart,     Position = new Vector3(0, 0, -1) + new Vector3(2056.331, 4933.8623, 41.07718) },
                // 2
                new SeedingPlaceConfig { ID = 32,  PitType = PitNames.Medium,            Position = new Vector3(0, 0, -1) + new Vector3(2054.66, 4929.8037, 41.07502) },
                new SeedingPlaceConfig { ID = 33,  PitType = PitNames.Medium,            Position = new Vector3(0, 0, -1) + new Vector3(2050.6233, 4933.626, 41.07579) },
                new SeedingPlaceConfig { ID = 34,  PitType = PitNames.Medium,            Position = new Vector3(0, 0, -1) + new Vector3(2046.5568, 4937.686, 41.09151) },
                new SeedingPlaceConfig { ID = 35,  PitType = PitNames.Medium,           Position = new Vector3(0, 0, -1) + new Vector3(2042.1152, 4942.221, 41.101265) },
                new SeedingPlaceConfig { ID = 36,  PitType = PitNames.Medium,           Position = new Vector3(0, 0, -1) + new Vector3(2037.3999, 4946.709, 41.106884) },
                new SeedingPlaceConfig { ID = 37,  PitType = PitNames.Medium,           Position = new Vector3(0, 0, -1) + new Vector3(2032.9407, 4951.2617, 41.070206) },
                new SeedingPlaceConfig { ID = 38,  PitType = PitNames.Medium,           Position = new Vector3(0, 0, -1) + new Vector3(2029.0059, 4949.6323, 41.117973) },
                new SeedingPlaceConfig { ID = 39,  PitType = PitNames.Medium,         Position = new Vector3(0, 0, -1) + new Vector3(2032.792, 4945.5767, 41.070053) },
                new SeedingPlaceConfig { ID = 40,  PitType = PitNames.Medium,         Position = new Vector3(0, 0, -1) + new Vector3(2036.7434, 4941.638, 41.05331) },
                new SeedingPlaceConfig { ID = 41,  PitType = PitNames.Medium,         Position = new Vector3(0, 0, -1) + new Vector3(2041.3188, 4937.0806, 41.076263) },
                new SeedingPlaceConfig { ID = 42,  PitType = PitNames.Medium,         Position = new Vector3(0, 0, -1) + new Vector3(2046.0917, 4932.435, 41.09097) },
                new SeedingPlaceConfig { ID = 43,  PitType = PitNames.Medium,         Position = new Vector3(0, 0, -1) + new Vector3(2050.611, 4928.1836, 41.123955) },
                new SeedingPlaceConfig { ID = 44,  PitType = PitNames.Medium,       Position = new Vector3(0, 0, -1) + new Vector3(2048.3777, 4924.856, 41.09983) },
                new SeedingPlaceConfig { ID = 45,  PitType = PitNames.Medium,       Position = new Vector3(0, 0, -1) + new Vector3(2043.599, 4929.3726, 41.106586) },
                new SeedingPlaceConfig { ID = 46,  PitType = PitNames.Medium,       Position = new Vector3(0, 0, -1) + new Vector3(2039.047, 4933.9707, 41.10515) },
                new SeedingPlaceConfig { ID = 47,  PitType = PitNames.Medium,       Position = new Vector3(0, 0, -1) + new Vector3(2034.9465, 4937.849, 41.116737) },
                new SeedingPlaceConfig { ID = 48,  PitType = PitNames.Medium,       Position = new Vector3(0, 0, -1) + new Vector3(2031.1135, 4941.843, 41.116413) },
                new SeedingPlaceConfig { ID = 49,  PitType = PitNames.Medium,       Position = new Vector3(0, 0, -1) + new Vector3(2027.1349, 4945.782, 41.09519) },
                new SeedingPlaceConfig { ID = 50,  PitType = PitNames.Medium,       Position = new Vector3(0, 0, -1) + new Vector3(2023.5579, 4943.697, 41.096706) },
                new SeedingPlaceConfig { ID = 51,  PitType = PitNames.Medium,       Position = new Vector3(0, 0, -1) + new Vector3(2028.6934, 4938.5415, 41.08454) },
                new SeedingPlaceConfig { ID = 52,  PitType = PitNames.Medium,       Position = new Vector3(0, 0, -1) + new Vector3(2033.1434, 4933.9746, 41.083256) },
                new SeedingPlaceConfig { ID = 53,  PitType = PitNames.Medium,       Position = new Vector3(0, 0, -1) + new Vector3(2037.3481, 4929.773, 41.075504) },
                new SeedingPlaceConfig { ID = 54,  PitType = PitNames.Medium, Position = new Vector3(0, 0, -1) + new Vector3(2041.9724, 4925.2295, 41.096912) },
                new SeedingPlaceConfig { ID = 55,  PitType = PitNames.Medium,  Position = new Vector3(0, 0, -1) + new Vector3(2046.125, 4921.4487, 41.119694) },
                new SeedingPlaceConfig { ID = 56,  PitType = PitNames.Medium,  Position = new Vector3(0, 0, -1) + new Vector3(2043.2302, 4918.29, 41.05004) },
                new SeedingPlaceConfig { ID = 57,  PitType = PitNames.Medium,     Position = new Vector3(0, 0, -1) + new Vector3(2038.5356, 4922.7407, 41.100483) },
                new SeedingPlaceConfig { ID = 58,  PitType = PitNames.Medium,     Position = new Vector3(0, 0, -1) + new Vector3(2033.899, 4927.395, 41.103878) },
                new SeedingPlaceConfig { ID = 59,  PitType = PitNames.Medium,            Position = new Vector3(0, 0, -1) + new Vector3(2029.7898, 4931.357, 41.106842) },
                new SeedingPlaceConfig { ID = 60,  PitType = PitNames.Medium,            Position = new Vector3(0, 0, -1) + new Vector3(2025.3899, 4935.7773, 41.081646) },
                new SeedingPlaceConfig { ID = 61,  PitType = PitNames.Medium,            Position = new Vector3(0, 0, -1) + new Vector3(2021.2819, 4939.8105, 41.055367) },
                //3
                new SeedingPlaceConfig { ID = 62,  PitType = PitNames.Fast,           Position = new Vector3(0, 0, -1) + new Vector3(2006.239, 4927.3022, 42.866634) },
                new SeedingPlaceConfig { ID = 63,  PitType = PitNames.Fast,           Position = new Vector3(0, 0, -1) + new Vector3(2010.0854, 4923.529, 42.846317) },
                new SeedingPlaceConfig { ID = 64,  PitType = PitNames.Fast,           Position = new Vector3(0, 0, -1) + new Vector3(2016.015, 4917.592, 42.839203) },
                new SeedingPlaceConfig { ID = 65,  PitType = PitNames.Fast,           Position = new Vector3(0, 0, -1) + new Vector3(2019.6636, 4913.9634, 42.83429) },
                new SeedingPlaceConfig { ID = 66,  PitType = PitNames.Fast,         Position = new Vector3(0, 0, -1) + new Vector3(2022.4208, 4911.305, 42.82801) },
                new SeedingPlaceConfig { ID = 67,  PitType = PitNames.Fast,         Position = new Vector3(0, 0, -1) + new Vector3(2026.4872, 4907.431, 42.841305) },
                new SeedingPlaceConfig { ID = 68,  PitType = PitNames.Fast,         Position = new Vector3(0, 0, -1) + new Vector3(2029.2698, 4904.7627, 42.860165) },
                new SeedingPlaceConfig { ID = 69,  PitType = PitNames.Fast,         Position = new Vector3(0, 0, -1) + new Vector3(2026.6539, 4901.659, 42.89565) },
                new SeedingPlaceConfig { ID = 70,  PitType = PitNames.Fast,         Position = new Vector3(0, 0, -1) + new Vector3(2023.2455, 4904.9775, 42.84778) },
                new SeedingPlaceConfig { ID = 71,  PitType = PitNames.Fast,       Position = new Vector3(0, 0, -1) + new Vector3(2020.1295, 4907.966, 42.856873) },
                new SeedingPlaceConfig { ID = 72,  PitType = PitNames.Fast,       Position = new Vector3(0, 0, -1) + new Vector3(2016.9094, 4911.052, 42.865704) },
                new SeedingPlaceConfig { ID = 73,  PitType = PitNames.Fast,       Position = new Vector3(0, 0, -1) + new Vector3(2013.3455, 4914.565, 42.877922) },
                new SeedingPlaceConfig { ID = 74,  PitType = PitNames.Fast,       Position = new Vector3(0, 0, -1) + new Vector3(2011.0704, 4916.9473, 42.888664) },
                new SeedingPlaceConfig { ID = 75,  PitType = PitNames.Fast,       Position = new Vector3(0, 0, -1) + new Vector3(2008.0098, 4919.9355, 42.88351) },
                new SeedingPlaceConfig { ID = 76,  PitType = PitNames.Fast,       Position = new Vector3(0, 0, -1) + new Vector3(2004.6763, 4923.216, 42.906277) },
                //4
                new SeedingPlaceConfig { ID = 77,  PitType = PitNames.Big,       Position = new Vector3(0, 0, -1) + new Vector3(2000.9719, 4921.3735, 42.90132) },
                new SeedingPlaceConfig { ID = 78,  PitType = PitNames.Big,       Position = new Vector3(0, 0, -1) + new Vector3(2004.0791, 4918.176, 42.88528) },
                new SeedingPlaceConfig { ID = 79,  PitType = PitNames.Big,       Position = new Vector3(0, 0, -1) + new Vector3(2007.1746, 4915.104, 42.882324) },
                new SeedingPlaceConfig { ID = 80,  PitType = PitNames.Big,       Position = new Vector3(0, 0, -1) + new Vector3(2010.8647, 4911.4146, 42.86722) },
                new SeedingPlaceConfig { ID = 81,  PitType = PitNames.Big, Position = new Vector3(0, 0, -1) + new Vector3(2014.7291, 4907.696, 42.873135) },
                new SeedingPlaceConfig { ID = 82,  PitType = PitNames.Big,  Position = new Vector3(0, 0, -1) + new Vector3(2018.3009, 4904.2554, 42.865097) },
                new SeedingPlaceConfig { ID = 83,  PitType = PitNames.Big,  Position = new Vector3(0, 0, -1) + new Vector3(2022.1069, 4900.5054, 42.88456) },
                new SeedingPlaceConfig { ID = 84,  PitType = PitNames.Big,     Position = new Vector3(0, 0, -1) + new Vector3(2020.2144, 4896.6274, 42.88635) },
                new SeedingPlaceConfig { ID = 85,  PitType = PitNames.Big,     Position = new Vector3(0, 0, -1) + new Vector3(2016.8469, 4899.946, 42.896084) },
                new SeedingPlaceConfig { ID = 86,  PitType = PitNames.Big,            Position = new Vector3(0, 0, -1) + new Vector3(2013.5388, 4903.306, 42.895657) },
                new SeedingPlaceConfig { ID = 87,  PitType = PitNames.Big,            Position = new Vector3(0, 0, -1) + new Vector3(2010.2712, 4906.3804, 42.895412) },
                new SeedingPlaceConfig { ID = 88,  PitType = PitNames.Big,            Position = new Vector3(0, 0, -1) + new Vector3(2007.6941, 4909.025, 42.882294) },
                new SeedingPlaceConfig { ID = 89,  PitType = PitNames.Big,           Position = new Vector3(0, 0, -1) + new Vector3(2004.4532, 4912.2104, 42.862003) },
                new SeedingPlaceConfig { ID = 90,  PitType = PitNames.Big,           Position = new Vector3(0, 0, -1) + new Vector3(2001.719, 4914.9443, 42.859825) },
                new SeedingPlaceConfig { ID = 91,  PitType = PitNames.Big,           Position = new Vector3(0, 0, -1) + new Vector3(1999.0376, 4917.638, 42.891655) },
                //5
                new SeedingPlaceConfig { ID = 92,  PitType = PitNames.Fertilized,           Position = new Vector3(0, 0, -1) + new Vector3(1994.2881, 4913.844, 42.825314) },
                new SeedingPlaceConfig { ID = 93,  PitType = PitNames.Fertilized,         Position = new Vector3(0, 0, -1) + new Vector3(1996.965, 4911.2295, 42.850525) },
                new SeedingPlaceConfig { ID = 94,  PitType = PitNames.Fertilized,         Position = new Vector3(0, 0, -1) + new Vector3(2000.1547, 4907.9673, 42.880898) },
                new SeedingPlaceConfig { ID = 95,  PitType = PitNames.Fertilized,         Position = new Vector3(0, 0, -1) + new Vector3(2003.5662, 4904.496, 42.8731) },
                new SeedingPlaceConfig { ID = 96,  PitType = PitNames.Fertilized,         Position = new Vector3(0, 0, -1) + new Vector3(2006.6465, 4901.4385, 42.861176) },
                new SeedingPlaceConfig { ID = 97,  PitType = PitNames.Fertilized,         Position = new Vector3(0, 0, -1) + new Vector3(2010.1312, 4898.0415, 42.8488) },
                new SeedingPlaceConfig { ID = 98,  PitType = PitNames.Fertilized,       Position = new Vector3(0, 0, -1) + new Vector3(2013.2239, 4895.0176, 42.83304) },
                new SeedingPlaceConfig { ID = 99,  PitType = PitNames.Fertilized,       Position = new Vector3(0, 0, -1) + new Vector3(2016.0968, 4892.1387, 42.835106) },
                new SeedingPlaceConfig { ID = 100, PitType = PitNames.Fertilized,       Position = new Vector3(0, 0, -1) + new Vector3(2013.5039, 4889.31, 42.90578) },
                new SeedingPlaceConfig { ID = 101, PitType = PitNames.Fertilized,       Position = new Vector3(0, 0, -1) + new Vector3(2010.3176, 4892.5127, 42.90404) },
                new SeedingPlaceConfig { ID = 102, PitType = PitNames.Fertilized,       Position = new Vector3(0, 0, -1) + new Vector3(2007.0627, 4895.6885, 42.904705) },
                new SeedingPlaceConfig { ID = 103, PitType = PitNames.Fertilized,       Position = new Vector3(0, 0, -1) + new Vector3(2003.7253, 4898.5864, 42.896603) },
                new SeedingPlaceConfig { ID = 104, PitType = PitNames.Fertilized,       Position = new Vector3(0, 0, -1) + new Vector3(1999.9126, 4902.464, 42.890438) },
                new SeedingPlaceConfig { ID = 105, PitType = PitNames.Fertilized,       Position = new Vector3(0, 0, -1) + new Vector3(1996.5701, 4905.851, 42.880215) },
                new SeedingPlaceConfig { ID = 106, PitType = PitNames.Fertilized,       Position = new Vector3(0, 0, -1) + new Vector3(1993.9105, 4908.584, 42.88233) },
                new SeedingPlaceConfig { ID = 107, PitType = PitNames.Fertilized,       Position = new Vector3(0, 0, -1) + new Vector3(1991.2965, 4911.2104, 42.90548) },
                //6
                new SeedingPlaceConfig { ID = 108, PitType = PitNames.BigFertilized, Position = new Vector3(0, 0, -1) + new Vector3(1986.4926, 4907.376, 42.884995) },
                new SeedingPlaceConfig { ID = 109, PitType = PitNames.BigFertilized,  Position = new Vector3(0, 0, -1) + new Vector3(1989.616, 4904.2197, 42.865074) },
                new SeedingPlaceConfig { ID = 110, PitType = PitNames.BigFertilized,  Position = new Vector3(0, 0, -1) + new Vector3(1992.9467, 4900.874, 42.908184) },
                new SeedingPlaceConfig { ID = 111, PitType = PitNames.BigFertilized,     Position = new Vector3(0, 0, -1) + new Vector3(1995.9137, 4897.921, 42.900043) },
                new SeedingPlaceConfig { ID = 112, PitType = PitNames.BigFertilized,     Position = new Vector3(0, 0, -1) + new Vector3(1999.3593, 4894.492, 42.8942) },
                new SeedingPlaceConfig { ID = 113, PitType = PitNames.BigFertilized,            Position = new Vector3(0, 0, -1) + new Vector3(2002.7681, 4891.1978, 42.888565) },
                new SeedingPlaceConfig { ID = 114, PitType = PitNames.BigFertilized,            Position = new Vector3(0, 0, -1) + new Vector3(2006.2665, 4887.982, 42.880676) },
                new SeedingPlaceConfig { ID = 115, PitType = PitNames.BigFertilized,            Position = new Vector3(0, 0, -1) + new Vector3(2009.041, 4885.3423, 42.87697) },
                //7
                new SeedingPlaceConfig { ID = 116, PitType = PitNames.FastFertilized,           Position = new Vector3(0, 0, -1) + new Vector3(2006.4282, 4881.8696, 42.891464) },
                new SeedingPlaceConfig { ID = 117, PitType = PitNames.FastFertilized,           Position = new Vector3(0, 0, -1) + new Vector3(2004.153, 4884.2114, 42.878914) },
                new SeedingPlaceConfig { ID = 118, PitType = PitNames.FastFertilized,           Position = new Vector3(0, 0, -1) + new Vector3(2001.2201, 4886.931, 42.88287) },
                new SeedingPlaceConfig { ID = 119, PitType = PitNames.FastFertilized,           Position = new Vector3(0, 0, -1) + new Vector3(1998.5043, 4889.581, 42.88358) },
                new SeedingPlaceConfig { ID = 120, PitType = PitNames.FastFertilized,         Position = new Vector3(0, 0, -1) + new Vector3(1995.2264, 4892.8687, 42.887917) },
                new SeedingPlaceConfig { ID = 121, PitType = PitNames.FastFertilized,         Position = new Vector3(0, 0, -1) + new Vector3(1991.9457, 4896.1816, 42.88384) },
                new SeedingPlaceConfig { ID = 122, PitType = PitNames.FastFertilized,         Position = new Vector3(0, 0, -1) + new Vector3(1989.2347, 4898.872, 42.86807) },
                new SeedingPlaceConfig { ID = 123, PitType = PitNames.FastFertilized,         Position = new Vector3(0, 0, -1) + new Vector3(1986.1273, 4901.9106, 42.852654) },
                new SeedingPlaceConfig { ID = 124, PitType = PitNames.FastFertilized,         Position = new Vector3(0, 0, -1) + new Vector3(1983.7963, 4904.15, 42.841778) },
            }
        };

        public static readonly Dictionary<ItemNames, PlantConfig> PlantConfigsList = new Dictionary<ItemNames, PlantConfig>
        {
            { ItemNames.CabbageSeed, new PlantConfig(
                new List<PlantStages>
                {
                    new PlantStages(0, null),
                    new PlantStages(0.2, "bkr_prop_weed_bud_pruned_01a"),
                    new PlantStages(0.6, "bkr_prop_weed_bud_01a"),
                    new PlantStages(1, "bkr_prop_weed_bud_02a"),
                    new PlantStages(10, "prop_veg_crop_03_cab"),
                    new PlantStages(999999, "bkr_prop_weed_bud_pruned_01a"),
                }, 
                secondBeforeWatering: 60, 
                ripeningTime: 180,
                witheringTime: 180, 
                PlantType.Vegetable, 
                exp: 12,
                fetus: ItemNames.Cabbage,
                countFetus: 5,
                name: ItemNames.CabbageSeed,
                price: 50) }, //Капуста

            { ItemNames.PumpkinSeed, new PlantConfig(
                new List<PlantStages>
                {
                    new PlantStages(0, null),
                    new PlantStages(0.2, "bkr_prop_weed_bud_pruned_01a"),
                    new PlantStages(0.6, "bkr_prop_weed_bud_01a"),
                    new PlantStages(1, "bkr_prop_weed_bud_02a"),
                    new PlantStages(10, "prop_veg_crop_03_pump"),
                    new PlantStages(999999, "bkr_prop_weed_bud_pruned_01a"),
                },
                secondBeforeWatering: 60,
                ripeningTime: 180,
                witheringTime: 180,
                PlantType.Vegetable,
                exp: 12,
                fetus: ItemNames.Pumpkin,
                countFetus: 5,
                name: ItemNames.PumpkinSeed,
                price: 50) }, //Тыква

            //{ ItemNames.ZucchiniSeed, new PlantConfig(
            //    new List<PlantStages>
            //    {
            //        new PlantStages(0, null),
            //        new PlantStages(0.2, "bkr_prop_weed_bud_pruned_01a"),
            //        new PlantStages(0.4, "bkr_prop_weed_bud_01a"),
            //        new PlantStages(0.6, "bkr_prop_weed_bud_02a"),
            //        new PlantStages(1, "prop_weed_02"),
            //        new PlantStages(10, "prop_weed_01"),
            //        new PlantStages(999999, "bkr_prop_weed_bud_pruned_01a"),
            //    },
            //    secondBeforeWatering: 120,
            //    ripeningTime: 600,
            //    witheringTime: 600,
            //    PlantType.Vegetable,
            //    exp: 5,
            //    fetus: ItemNames.Zucchini,
            //    countFetus: 5,
            //    name: ItemNames.ZucchiniSeed) }, //Кабачок

            //{ ItemNames.WatermelonSeed, new PlantConfig(
            //    new List<PlantStages>
            //    {
            //        new PlantStages(0, null),
            //        new PlantStages(0.2, "bkr_prop_weed_bud_pruned_01a"),
            //        new PlantStages(0.4, "bkr_prop_weed_bud_01a"),
            //        new PlantStages(0.6, "bkr_prop_weed_bud_02a"),
            //        new PlantStages(1, "prop_weed_02"),
            //        new PlantStages(10, "prop_weed_01"),
            //        new PlantStages(999999, "bkr_prop_weed_bud_pruned_01a"),
            //    },
            //    secondBeforeWatering: 120,
            //    ripeningTime: 600,
            //    witheringTime: 600,
            //    PlantType.Vegetable,
            //    exp: 5,
            //    fetus: ItemNames.Watermelon,
            //    countFetus: 5,
            //    name: ItemNames.WatermelonSeed) }, //Арбуз

            { ItemNames.TomatoSeed, new PlantConfig(
                new List<PlantStages>
                {
                    new PlantStages(0, null),
                    new PlantStages(0.2, "bkr_prop_weed_bud_pruned_01a"),
                    new PlantStages(0.6, "bkr_prop_weed_bud_01a"),
                    new PlantStages(1, "prop_farmgo_tomato_1"),
                    new PlantStages(10, "prop_farmgo_tomato_2"),
                    new PlantStages(999999, "bkr_prop_weed_bud_pruned_01a"),
                },
                secondBeforeWatering: 60,
                ripeningTime: 180,
                witheringTime: 180,
                PlantType.Vegetable,
                exp: 12,
                fetus: ItemNames.Tomato,
                countFetus: 5,
                name: ItemNames.TomatoSeed,
                price: 50) }, //Помидор

            //{ ItemNames.StrawberrySeed, new PlantConfig(
            //    new List<PlantStages>
            //    {
            //        new PlantStages(0, null),
            //        new PlantStages(0.2, "bkr_prop_weed_bud_pruned_01a"),
            //        new PlantStages(0.4, "bkr_prop_weed_bud_01a"),
            //        new PlantStages(0.6, "bkr_prop_weed_bud_02a"),
            //        new PlantStages(1, "prop_weed_02"),
            //        new PlantStages(10, "prop_weed_01"),
            //        new PlantStages(999999, "bkr_prop_weed_bud_pruned_01a"),
            //    },
            //    secondBeforeWatering: 120,
            //    ripeningTime: 600,
            //    witheringTime: 600,
            //    PlantType.Vegetable,
            //    exp: 5,
            //    fetus: ItemNames.Strawberry,
            //    countFetus: 5,
            //    name: ItemNames.StrawberrySeed) }, //Клубника

            //{ ItemNames.RaspberriesSeed, new PlantConfig(
            //    new List<PlantStages>
            //    {
            //        new PlantStages(0, null),
            //        new PlantStages(0.2, "bkr_prop_weed_bud_pruned_01a"),
            //        new PlantStages(0.4, "bkr_prop_weed_bud_01a"),
            //        new PlantStages(0.6, "bkr_prop_weed_bud_02a"),
            //        new PlantStages(1, "prop_weed_02"),
            //        new PlantStages(10, "prop_weed_01"),
            //        new PlantStages(999999, "bkr_prop_weed_bud_pruned_01a"),
            //    },
            //    secondBeforeWatering: 120,
            //    ripeningTime: 600,
            //    witheringTime: 600,
            //    PlantType.Vegetable,
            //    exp: 5,
            //    fetus: ItemNames.Raspberries,
            //    countFetus: 5,
            //    name: ItemNames.RaspberriesSeed) }, //Малина

            //{ ItemNames.RadishSeed, new PlantConfig(
            //    new List<PlantStages>
            //    {
            //        new PlantStages(0, null),
            //        new PlantStages(0.2, "bkr_prop_weed_bud_pruned_01a"),
            //        new PlantStages(0.4, "bkr_prop_weed_bud_01a"),
            //        new PlantStages(0.6, "bkr_prop_weed_bud_02a"),
            //        new PlantStages(1, "prop_weed_02"),
            //        new PlantStages(10, "prop_weed_01"),
            //        new PlantStages(999999, "bkr_prop_weed_bud_pruned_01a"),
            //    },
            //    secondBeforeWatering: 120,
            //    ripeningTime: 600,
            //    witheringTime: 600,
            //    PlantType.Vegetable,
            //    exp: 5,
            //    fetus: ItemNames.Radish,
            //    countFetus: 5,
            //    name: ItemNames.RadishSeed) }, //Редис

            { ItemNames.PotatoesSeed, new PlantConfig(
                new List<PlantStages>
                {
                    new PlantStages(0, null),
                    new PlantStages(0.2, "bkr_prop_weed_bud_pruned_01a"),
                    new PlantStages(0.6, "bkr_prop_weed_bud_01a"),
                    new PlantStages(1, "prop_farmgo_potato_1"),
                    new PlantStages(10, "prop_farmgo_potato_2"),
                    new PlantStages(999999, "bkr_prop_weed_bud_pruned_01a"),
                },
                secondBeforeWatering: 75,
                ripeningTime: 240,
                witheringTime: 240,
                PlantType.Vegetable,
                exp: 16,
                fetus: ItemNames.Potatoes,
                countFetus: 5,
                name: ItemNames.PotatoesSeed,
                price: 60) }, //Картофель

            { ItemNames.OrangeSeed, new PlantConfig(
                new List<PlantStages>
                {
                    new PlantStages(0, null),
                    new PlantStages(0.2, "bkr_prop_weed_bud_pruned_01a"),
                    new PlantStages(0.6, "bkr_prop_weed_bud_01a"),
                    new PlantStages(1, "bkr_prop_weed_bud_02a"),
                    new PlantStages(10, "prop_veg_crop_orange"),
                    new PlantStages(999999, "prop_tree_stump_01"),
                },
                secondBeforeWatering: 90,
                ripeningTime: 300,
                witheringTime: 300,
                PlantType.Fruit,
                exp: 20,
                fetus: ItemNames.Orange,
                countFetus: 10,
                name: ItemNames.OrangeSeed,
                price: 75) }, //Апельсин

            //{ ItemNames.CucumberSeed, new PlantConfig(
            //    new List<PlantStages>
            //    {
            //        new PlantStages(0, null),
            //        new PlantStages(0.2, "bkr_prop_weed_bud_pruned_01a"),
            //        new PlantStages(0.4, "bkr_prop_weed_bud_01a"),
            //        new PlantStages(0.6, "bkr_prop_weed_bud_02a"),
            //        new PlantStages(1, "prop_weed_02"),
            //        new PlantStages(10, "prop_weed_01"),
            //        new PlantStages(999999, "bkr_prop_weed_bud_pruned_01a"),
            //    },
            //    secondBeforeWatering: 120,
            //    ripeningTime: 600,
            //    witheringTime: 600,
            //    PlantType.Vegetable,
            //    exp: 5,
            //    fetus: ItemNames.Cucumber,
            //    countFetus: 5,
            //    name: ItemNames.CucumberSeed) }, //Огурец

            { ItemNames.CarrotSeed, new PlantConfig(
                new List<PlantStages>
                {
                    new PlantStages(0, null),
                    new PlantStages(0.2, "bkr_prop_weed_bud_pruned_01a"),
                    new PlantStages(0.5, "bkr_prop_weed_bud_02b"),
                    new PlantStages(1, "bkr_prop_weed_bud_01a"),
                    new PlantStages(10, "bkr_prop_weed_bud_02a"),
                    new PlantStages(999999, "bkr_prop_weed_bud_pruned_01a"),
                },
                secondBeforeWatering: 60,
                ripeningTime: 180,
                witheringTime: 180,
                PlantType.Vegetable,
                exp: 12,
                fetus: ItemNames.Carrot,
                countFetus: 5,
                name: ItemNames.CarrotSeed,
                price: 50) }, //Морковь

            { ItemNames.BananaSeed, new PlantConfig(
                new List<PlantStages>
                {
                    new PlantStages(0, null),
                    new PlantStages(0.2, "bkr_prop_weed_bud_pruned_01a"),
                    new PlantStages(0.4, "bkr_prop_weed_bud_01a"),
                    new PlantStages(0.6, "bkr_prop_weed_bud_02a"),
                    new PlantStages(1, "prop_plant_01a"),
                    new PlantStages(10, "prop_veg_crop_tr_02"),
                    new PlantStages(999999, "prop_tree_stump_01"),
                },
                secondBeforeWatering: 90,
                ripeningTime: 300,
                witheringTime: 300,
                PlantType.Fruit,
                exp: 20,
                fetus: ItemNames.Banana,
                countFetus: 10,
                name: ItemNames.BananaSeed,
                price: 75) }, //Банан

            { ItemNames.AppleSeed, new PlantConfig(
                new List<PlantStages>
                {
                    new PlantStages(0, null),
                    new PlantStages(0.2, "bkr_prop_weed_bud_pruned_01a"),
                    new PlantStages(0.6, "bkr_prop_weed_bud_01a"),
                    new PlantStages(1, "bkr_prop_weed_bud_02a"),
                    new PlantStages(10, "prop_tree_birch_05"),
                    new PlantStages(999999, "prop_tree_stump_01"),
                },
                secondBeforeWatering: 90,
                ripeningTime: 300,
                witheringTime: 300,
                PlantType.Fruit,
                exp: 20,
                fetus: ItemNames.Apple,
                countFetus: 10,
                name: ItemNames.AppleSeed,
                price: 75) }, //Яблоко
        };

        public static readonly Dictionary<PitNames, BonusConfig> PitConfigsList = new Dictionary<PitNames, BonusConfig>
        {
            { PitNames.Standart, new BonusConfig(0, 1, 0) },
            { PitNames.Medium, new BonusConfig(0.05, 2, 1) },
            { PitNames.Fast, new BonusConfig(0.3, 2, 1) },
            { PitNames.Big, new BonusConfig(0.2, 3, 2) },
            { PitNames.Fertilized, new BonusConfig(0.15, 4, 2) },
            { PitNames.BigFertilized, new BonusConfig(0.25, 5, 3) },
            { PitNames.FastFertilized, new BonusConfig(0.3, 4, 2) },
        };

        public static readonly Dictionary<FertilizerType, BonusConfig> FertilizerConfigsList = new Dictionary<FertilizerType, BonusConfig>
        {
            { FertilizerType.None, new BonusConfig(0, 0, 0) },
            { FertilizerType.Standart, new BonusConfig(0.05, 1, 1) },
            { FertilizerType.Big, new BonusConfig(0.1, 2, 2) },
        };

        public static readonly Dictionary<ItemNames, ProductConfig> ProductConfigs = new Dictionary<ItemNames, ProductConfig>
        {
            { ItemNames.FertilizerStandVegetable, new ProductConfig("Fert", null, null, FertilizerType.Standart) },
            { ItemNames.FertilizerStandBerry, new ProductConfig("Fert", null, null, FertilizerType.Standart) },
            { ItemNames.FertilizerStandFruit, new ProductConfig("Fert", null, null, FertilizerType.Standart) },
            { ItemNames.FertilizerBigVegetable, new ProductConfig("Fert", null, null, FertilizerType.Big) },
            { ItemNames.FertilizerBigBerry, new ProductConfig("Fert", null, null, FertilizerType.Big) },
            { ItemNames.FertilizerBigFruit, new ProductConfig("Fert", null, null, FertilizerType.Big) },
            { ItemNames.WateringBig, new ProductConfig("Other", "Большая лейка", "water.png", 0) },
            { ItemNames.WateringMedium, new ProductConfig("Other", "Средняя лейка", "water.png", 0) },
            { ItemNames.WateringLow, new ProductConfig("Other", "Маленькая лейка", "water.png", 0) },
            { ItemNames.FoodBox, new ProductConfig("Other", null, null, 0) },
        };

        public static readonly List<Vector3> WaterPoints = new List<Vector3>
        {
            new Vector3(1960.116, 5048.679, 40.29717),
            new Vector3(1956.221, 5053.82, 40.38471),
            new Vector3(1951.916, 5058.558, 40.6528),
            new Vector3(2792.303, 4621.866, 44.67227),
            new Vector3(472.5044, 6529.063, 28.34974),
            new Vector3(466.5665, 6529.107, 28.0485),
        };

        public static readonly Dictionary<int, LevelConfig> Levels = new Dictionary<int, LevelConfig>
        {
            [1] = new LevelConfig(PitNames.Standart, 3),
            [2] = new LevelConfig(PitNames.Medium, 4),
            [3] = new LevelConfig(PitNames.Fast, 5),
            [4] = new LevelConfig(PitNames.Big, 6),
            [5] = new LevelConfig(PitNames.Fertilized, 7),
            [6] = new LevelConfig(PitNames.BigFertilized, 8),
            [7] = new LevelConfig(PitNames.FastFertilized, 9),
        };
    }
}
