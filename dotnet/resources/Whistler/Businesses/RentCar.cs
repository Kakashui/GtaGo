using System.Linq;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Whistler.Core;
using Whistler.MoneySystem;
using Newtonsoft.Json;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using Whistler.GarbageCollector;
using Whistler.VehicleSystem;
using Whistler.Helpers;
using Whistler.VehicleSystem.Models;
using Whistler.VehicleSystem.Models.VehiclesData;
using Whistler.GUI;
using Whistler.Common;
using Whistler.Entities;

namespace Whistler.Businesses
{
    public class RentCarBusiness : Script
    {
        public const int RespawnTime = 5; //in minutes

        [Command("delrentveh")]
        public static void DeleteRentCar(ExtPlayer player)
        {
            if (!player.IsInVehicle || player.VehicleSeat != VehicleConstants.DriverSeat)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы должны быть водителям", 3000);
                return;
            }
            ExtVehicle extVehicle = player.Vehicle as ExtVehicle;
            if (extVehicle.Data.OwnerType != OwnerType.Rent)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы должны быть в арендованном транспорте", 3000);
                return;
            }
            VehicleManager.WarpPlayerOutOfVehicle(player);
            extVehicle.Data.DeleteVehicle(extVehicle);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Машина для аренды удалена", 3000);
        }

    }
}