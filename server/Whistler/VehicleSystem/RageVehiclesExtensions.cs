using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;

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
        public static int FillFuel(this Vehicle vehicle, int fuelAmount)
        {
            var vehicleGo = vehicle.GetVehicleGo();
            if (vehicleGo == null)
                return fuelAmount;

            var needFuelAmount = Math.Min(fuelAmount, vehicleGo.Config.MaxFuel - vehicleGo.Data.Fuel);

            vehicleGo.Data.Fuel += needFuelAmount;
            vehicle.SetSharedData("PETROL", vehicleGo.Data.Fuel);

            return needFuelAmount;
        }
    }
}
