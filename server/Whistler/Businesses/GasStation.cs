using GTANetworkAPI;
using System;
using Whistler.MoneySystem;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.Helpers;
using Whistler.VehicleSystem.Models;
using Whistler.Fractions;
using Whistler.Domain.Phone.Bank;
using Whistler.MoneySystem.Interface;
using Whistler.Businesses.Models;
using Whistler.Common;

namespace Whistler.Core
{
    partial class BusinessManager : Script
    {
        [RemoteEvent("gasStation:buypetrol")]
        public static void FillCar(Player player, string fuelType, int liters, int paymentType)
        {
            try
            {
                if (!player.IsLogged() || player.Vehicle == null) return;
                Vehicle vehicle = player.Vehicle;
                if (player.VehicleSeat != VehicleConstants.DriverSeat) return;
                if (!Enum.IsDefined(typeof(PaymentsType), paymentType))
                    return;
                var payType = (PaymentsType)paymentType;
                if (liters <= 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Biz_161", 3000);
                    return;
                }
                if (VehicleStreaming.GetEngineState(vehicle))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_105", 3000);
                    return;
                }
                VehicleGo vehGo = vehicle.GetVehicleGo();

                var config = VehicleConfiguration.GetConfig(vehicle.Model);

                if (vehGo.Data.Fuel >= config.MaxFuel)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_106", 3000);
                    return;
                }
                if (fuelType != "standart") //TODO: add fuel types
                    return;

                int tfuel = vehGo.Data.Fuel + liters;
                if (tfuel > config.MaxFuel)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Main_92".Translate( config.MaxFuel), 3000);
                    return;
                }
                Business biz = BusinessManager.GetBusiness(player.GetData<int>("BIZ_ID"));
                if (payType == PaymentsType.Gov)
                {
                    var fraction = Manager.GetFraction(player);
                    if (fraction == null || fraction.OrgActiveType != OrgActivityType.Government)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_107", 3000);
                        return;
                    }
                    if (vehGo.Data.OwnerType != OwnerType.Fraction || vehGo.Data.OwnerID != fraction.Id)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_108", 3000);
                        return;
                    }
                    if (fraction.FuelLeft < liters * biz.Products[0].Price)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_109", 3000);
                        return;
                    }
                }

                BusinessManager.TakeProd(player, biz, player.GetMoneyPayment(payType, Manager.GetFraction(6)), 
                    new BuyModel("Petrol", liters, false,
                    (cnt) =>
                    {
                        VehicleStreaming.SetVehicleFuel(vehicle, vehGo.Data.Fuel + cnt);
                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Biz_112", 3000);
                        Chat.Action(player, "Biz_113");
                        if (payType == PaymentsType.Gov)
                        {
                            Manager.GetFraction(player).FuelLeft -= liters * biz.Products[0].Price;
                        }
                        return cnt;
                    }), 
                    "Money_BuyPetrol", null);
            }
            catch (Exception e) { _logger.WriteError("Petrol: " + e.ToString()); }
        }

        public static void OpenPetrolMenu(Player player)
        {
            Business biz = BizList[player.GetData<int>("BIZ_ID")];
            Product prod = biz.Products[0];

            Trigger.ClientEvent(player, "openPetrol", prod.Price);
        }
    }
}
