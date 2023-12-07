using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Common;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;

namespace Whistler.NewDonateShop.Models
{
    class VehBoostDonateItem: BaseDonateItem
    {
        public VehBoostDonateItem(int amount)
        {
            Amount = amount;
        }

        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            if (!player.IsInVehicle)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:veh:noveh".Translate(), 3000);
                return false;
            }
            ExtVehicle vehicle = player.Vehicle as ExtVehicle;
            if(vehicle.Data.OwnerType != OwnerType.Personal && vehicle.Data.OwnerType != OwnerType.Family)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:veh:bad".Translate(), 3000);
                return false;
            }
            if(vehicle.Data.EnginePower >= Amount)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:veh:more".Translate(), 3000);
                return false;
            }
            VehicleCustomization.SetPowerTorque(vehicle, Amount);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:item:veh:boost:ok".Translate(Amount), 3000);
            return true;
        }
    }
}
