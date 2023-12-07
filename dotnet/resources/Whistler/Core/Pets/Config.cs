using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.Pets.Models;
using Whistler.Helpers;

namespace Whistler.Core.Pets
{
    internal class Config
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Config));
        public static Dictionary<uint, PetConfig> PetsConfig = new Dictionary<uint, PetConfig>();

        public static void Initialize()
        {
            PetsConfig.Add(2971380566, new PetConfig(2971380566, "Свинья", 100, price: 3500, immuneToPlayerDamage: true));
            PetsConfig.Add(3462393972, new PetConfig(3462393972, "Кабан", 100, price: 3500, immuneToPlayerDamage: true));

            PetsConfig.Add(1462895032, new PetConfig(1462895032, "Кот", 40, 0, 1, 2000, true));

            PetsConfig.Add(2910340283, new PetConfig(2910340283, "Йоркширский терьер", 150, price: 2000));
            PetsConfig.Add(1832265812, new PetConfig(1832265812, "Мопс", 150, price: 2000));
            PetsConfig.Add(1125994524, new PetConfig(1125994524, "Пудель", 150, price: 2000));
            PetsConfig.Add(2506301981, new PetConfig(2506301981, "Ротвейлер", 150, 25, price: 6000));
            PetsConfig.Add(1318032802, new PetConfig(1318032802, "Хаски", 150, 20, price: 6500));
            PetsConfig.Add(882848737, new PetConfig(882848737, "Ретривер", 150, 20, price: 6000));
            PetsConfig.Add(1126154828, new PetConfig(1126154828, "Овчарка", 150, 20, price: 5500));

            PetsConfig.Add(1682622302, new PetConfig(1682622302, "Койот", 150, price: 0));
            PetsConfig.Add(307287994, new PetConfig(307287994, "Горная львица", 150, 30, price: 0));
            PetsConfig.Add(3877461608, new PetConfig(3877461608, "Пума", 150, 30, price: 0));

            _logger.WriteInfo($"Pets Config ({PetsConfig.Count}) successfully loaded.");
        }
    }
}
