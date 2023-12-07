using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Helpers;
using Whistler.VehicleSystem.Models;

namespace Whistler.VehicleTrading
{
    class SellVehicleManager
    {
        private static Vector3 SellPosition = new Vector3(144.1654, -3001.976, 6.061024);
        public static void Init()
        {
            Whistler.Core.InteractShape.Create(SellPosition, 2, 2, 0)
                .AddEnterPredicate((shape, player) => player.IsInVehicle)
                .AddInteraction(player => 
                {
                    if (player.Vehicle == null) return;
                    VehicleSystem.VehicleOperations.SellVeh(player, (player.Vehicle as ExtVehicle).Data.ID);
                }, "veh:sell:mngr")
                .AddMarker(27, SellPosition, 4, Core.InteractShape.DefaultMarkerColor);
            NAPI.Blip.CreateBlip(672, SellPosition, 1, 24, Main.StringToU16("Продажа авто"), 255, 0, true, 0, 0);
        }
    }
}
