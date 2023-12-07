using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Common;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.VehicleSystem;

namespace Whistler.NewDonateShop.Models
{
    class VehicleTiningDonateItem:BaseDonateItem
    {
        public VehicleTiningDonateItem(ModTypes type, int index)
        {
            Type = type;
            Index = index;
        }
        public ModTypes Type { get; set; }
        public int Index { get; set; }
        public override bool TryUse(Player player, int count, bool sell)
        {
            if (!player.IsInVehicle)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:veh:noveh".Translate(), 3000);
                return false;
            }
            var vehicle = player.Vehicle;
            var vehGo = vehicle.GetVehicleGo();
            if (vehGo.Data.OwnerType != OwnerType.Personal && vehGo.Data.OwnerType != OwnerType.Family)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:veh:bad".Translate(), 3000);
                return false;
            }
            if (vehGo.Data.VehCustomization.GetComponent(Type) >= Index)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:veh:more".Translate(), 3000);
                return false;
            }
            VehicleCustomization.SetMod(vehicle, Type, Index);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:item:veh:mod:ok".Translate(), 3000);
            return true;
        }
    }
}
