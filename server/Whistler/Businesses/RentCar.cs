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

namespace Whistler.Businesses
{
    public class RentCarBusiness : Script
    {
        public const int RespawnTime = 5; //in minutes

        [Command("delrentveh")]
        public static void DeleteRentCar(Player player)
        {
            if (!player.IsInVehicle || player.VehicleSeat != VehicleConstants.DriverSeat)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_141", 3000);
                return;
            }
            VehicleGo vehGo = player.Vehicle.GetVehicleGo();
            if (vehGo.Data.OwnerType != OwnerType.Rent)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_142", 3000);
                return;
            }
            VehicleManager.WarpPlayerOutOfVehicle(player);
            vehGo.Data.DeleteVehicle(player.Vehicle);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Biz_143", 3000);
        }

    }
}