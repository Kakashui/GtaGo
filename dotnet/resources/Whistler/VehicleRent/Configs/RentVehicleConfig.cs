using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;

namespace Whistler.VehicleRent.Configs
{
    class RentVehicleConfig : Script
    {
        private static readonly List<Color> _allColors = new List<Color>
        {
            new Color(255, 247, 0),
            new Color(255, 0, 202),
            new Color(17, 0, 97),
            new Color(255, 255, 255),
            new Color(98, 98, 98),
            new Color(0, 0, 0),
            new Color(13, 87, 0),
            new Color(255, 134, 0),
            new Color(0, 213, 255),
        };
        public const int RespawnTime = 20; //in minutes
        public static readonly Dictionary<RentSection, CategoryModel> SectConfigs = new Dictionary<RentSection, CategoryModel>
        {
            {
                RentSection.Car, new CategoryModel("car", "vehicleRent_6", _allColors,
                    new List<RentModel>
                    {
                        new RentModel("asea", 1000),
                        new RentModel("chino2", 1000),
                        new RentModel("emperor", 1000),
                        new RentModel("primo", 1000),
                        new RentModel("seminole2", 1000),
                        new RentModel("virgo3", 1000),
                        new RentModel("yosemite3", 1000),


                        new RentModel("baller4", 5000),
                        new RentModel("cog55", 5000),
                        new RentModel("dubsta2", 5000),
                        new RentModel("issi2", 5000),
                        new RentModel("superd", 5000),


                        new RentModel("toros", 20000),
                        new RentModel("turismo2", 25000),
                    })
            },
            {
                RentSection.Moto, new CategoryModel("moto", "vehicleRent_9", _allColors,
                    new List<RentModel>
                    {
                        new RentModel("faggio", 500),
                        new RentModel("blazer2", 2000),
                        new RentModel("blazer4", 1500),
                        new RentModel("carbonrs", 3000),
                        new RentModel("outlaw", 20000),
                        new RentModel("sanchez2", 2000),
                        new RentModel("vagrant", 4000),
                        new RentModel("verus", 2000),
                        new RentModel("vindicator", 4000),
                    })
            },
            {
                RentSection.Special, new CategoryModel("special", "vehicleRent_11", _allColors,
                    new List<RentModel>
                    {
                        new RentModel("biff", 4000),
                        new RentModel("burrito3", 2500),
                        new RentModel("camper", 2000),
                        new RentModel("moonbeam2", 3000),
                        new RentModel("mule3", 7000),
                        new RentModel("pbus2", 10000),
                        new RentModel("pony", 5000),
                        new RentModel("pounder2", 10000),
                        new RentModel("rentalbus", 2000),
                        new RentModel("rumpo3", 15000),
                        new RentModel("surfer", 2000),
                        new RentModel("tiptruck", 4000),
                        new RentModel("wastelander", 50000),
                    })
            },
            {
                RentSection.Taxi, new CategoryModel("car", "vehicleRent_12",
                    new List<Color>
                    {
                        new Color(255, 247, 0),
                    },
                    new List<RentModel>
                    {
                        new RentModel("taxi", 1000),
                    })
            },
            {
                RentSection.Fly, new CategoryModel("air", "vehicleRent_14", _allColors,
                    new List<RentModel>
                    {
                        new RentModel("alphaz1", 10000),
                        new RentModel("duster", 10000),
                        new RentModel("luxor2", 100000),
                    })
            },
            {
                RentSection.Boath, new CategoryModel("water", "vehicleRent_15", _allColors,
                    new List<RentModel>
                    {
                        new RentModel("dinghy", 5000),
                        new RentModel("dinghy2", 5000),
                        new RentModel("marquis", 15000),
                        new RentModel("seashark", 1000),
                        new RentModel("seashark2", 2000),
                        new RentModel("squalo", 20000),
                        new RentModel("submersible2", 10000),
                        new RentModel("suntrap", 30000),
                        new RentModel("toro", 40000),
                    })
            },
            {
                RentSection.HeliCopter, new CategoryModel("air", "vehicleRent_14", _allColors,
                    new List<RentModel>
                    {
                        new RentModel("volatus", 50000),
                        new RentModel("maverick", 30000),
                        new RentModel("havok", 20000),
                        new RentModel("seasparrow", 10000),
                    })
            },
        };
        public RentVehicleConfig()
        {
            if (Directory.Exists("interfaces/gui/src/configs/vehicleRent"))
            {
                Dictionary<int, CategoryModel> configs = new Dictionary<int, CategoryModel>();
                foreach (var item in SectConfigs)
                {
                    configs.Add((int)item.Key, item.Value);
                }

                using (var w = new StreamWriter("interfaces/gui/src/configs/vehicleRent/categories.js"))
                {
                    w.Write($"export default {JsonConvert.SerializeObject(configs)}");
                }
            };
        }

        public static bool CheckVehicleModelIsTaxi(uint model)
        {
            return SectConfigs[RentSection.Taxi].Cars.FirstOrDefault(item => NAPI.Util.GetHashKey(item.Model) == model) != null;
        }
    }
}
