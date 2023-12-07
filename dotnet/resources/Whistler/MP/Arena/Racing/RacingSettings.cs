using System;
using System.Collections.Generic;
using System.Linq;

namespace Whistler.MP.Arena.Racing
{
    internal static class RacingSettings
    {
        /// <summary>
        /// Периодичность автоматического запуска гонки в мс
        /// </summary>
        public static int CreationDelayTime { get; } = 60 * 60 * 1000;

        /// <summary>
        /// Длительность регистрации
        /// </summary>
        public static int RegistrationTime { get; } = 10 * 60 * 1000;
        
        /// <summary>
        /// Стоимость регистрации
        /// </summary>
        public static int RegistrationPayment { get; } = 1000;

        public static Dictionary<RacingName, int> BestDailyTimes = LoadDictionary();

        private static Dictionary<RacingName, int> LoadDictionary() => Enum.GetValues(typeof(RacingName))
                .Cast<RacingName>()
                .ToDictionary(name => name, name => 0);
    }
}