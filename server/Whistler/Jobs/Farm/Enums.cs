using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Jobs.Farm
{
    public enum PitNames
    {
        Standart,
        Medium,
        Fast,
        Big,
        Fertilized,
        BigFertilized,
        FastFertilized,
    }
    public enum FertilizerType
    {
        None,
        Standart,
        Big,
    }

    public enum PlantType
    {
        /// <summary>
        /// Овощ
        /// </summary>
        Vegetable,
        /// <summary>
        /// Ягода
        /// </summary>
        Berry,
        /// <summary>
        /// Фрукт
        /// </summary>
        Fruit,
    }

    public enum PlantStatus
    {
        Growing,
        Ripe,
        Withered,
    }
}
