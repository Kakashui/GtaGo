using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.SDK;

namespace Whistler.Core.CustomSync
{
    public static class Particles
    {
        /// <summary>
        /// Включает эффекты на определенной позиции.
        /// </summary>
        /// <param name="ms">Время проигрывания в миллисекундах</param>
        public static void PlayParticlesAtPosition(Vector3 position, string assetName, string effectName, float scale, int ms)
        {
            const int RangeToSync = 50;
            SafeTrigger.ClientEventInRange(position, RangeToSync, "particles:playAtPosition", position, assetName, effectName, scale, ms);
        }
    }
}
