using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.VehicleSystem.Models
{
    public class VehConfig
    {
        /// <summary>
        /// Hash модели.
        /// </summary>
        public uint Model { get; set; }

        /// <summary>
        /// Название игровой модели машины (ex. bugattigo).
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Отображаемое игрокам название машины (ex. Bugatti Chiron).
        /// </summary>
        public string DisplayName { get; set; }
        public int PriceInCoins { get; set; }

        // Inventory properties

        /// <summary>
        /// Количество слотов в багажнике.
        /// </summary>
        public int Slots { get; set; }

        /// <summary>
        /// Максимальный вес предметов в багажинке (в условных единицах).
        /// </summary>
        public int MaxWeight { get; set; }

        // Fuel properties

        /// <summary>
        /// Потребление топлива (ед. в секунду).
        /// </summary>
        public int FuelConsumption { get; set; }

        /// <summary>
        /// Максимальное количество топлива (в литрах).
        /// </summary>
        public int MaxFuel { get; set; }

        public int fuelType { get; set; }
        public VehConfig(string modelName, string displayName, int price, int slots, int maxWeight, int fuelConsumption, int maxFuel, int ftype)
        {
            ModelName = modelName;
            Model = NAPI.Util.GetHashKey(modelName);
            DisplayName = displayName;
            PriceInCoins = price;
            Slots = slots;
            MaxWeight = maxWeight;
            FuelConsumption = fuelConsumption;
            MaxFuel = maxFuel;
            fuelType = ftype;
        }
        public VehConfig()
        {

        }
    }
}
