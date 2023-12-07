using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.VehicleSystem.Models;

namespace Whistler.VehicleSystem
{
    public static class RageVehiclesExtensions
    {
        /// <summary>
        /// Заправляет т/с на <paramref name="fuelAmount"/> или до полного бака,
        /// если при заправлении на <paramref name="fuelAmount"/> в итоге топлива будет больше, чем в конфиге.
        /// </summary>
        /// <remarks>Использует <see cref="NAPI"/>-методы.</remarks>
        /// <returns>Количество заправленного топлива</returns>
        public static int FillFuel(this ExtVehicle vehicle, int fuelAmount)
        {
            var needFuelAmount = Math.Min(fuelAmount, vehicle.Config.MaxFuel - vehicle.Data.Fuel);

            vehicle.Data.Fuel += needFuelAmount;
            SafeTrigger.SetSharedData(vehicle, "PETROL", vehicle.Data.Fuel);

            return needFuelAmount;
        }
    }
}
