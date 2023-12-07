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
using Whistler.Entities;

namespace Whistler.Core
{
    partial class BusinessManager : Script
    {
        public enum PetrolType
        {
            Invalid = 0,
            Standart,
            StandartPlus,
            Diesel,
            Deluxe,
            Electro
        };

        [RemoteEvent("gasStation:buypetrol")]
        public static void FillCar(ExtPlayer player, int fuelType, int liters, int paymentType)
        {
            try
            {
                if (!player.IsLogged() || player.Vehicle == null) return;
                ExtVehicle vehicle = player.Vehicle as ExtVehicle;
                if (player.VehicleSeat != VehicleConstants.DriverSeat) return;
                if (!Enum.IsDefined(typeof(PaymentsType), paymentType)) return;
                var payType = (PaymentsType)paymentType;
                if (liters <= 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Введены неправилные данные", 3000);
                    return;
                }
                VehicleStreaming.SetEngineState(vehicle, false);
                
                if (VehicleStreaming.GetEngineState(vehicle))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Чтобы начать заправляться - заглушите транспорт", 3000);
                    return;
                }

                var config = VehicleConfiguration.GetConfig(vehicle.Model);
                if (vehicle.Data.Fuel >= config.MaxFuel)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У транспорта полный бак", 3000);
                    return;
                }
                // Chat.AdminToAll($"fueltype: {fuelType} / fuel: {config.fuelType}-1");
                if (fuelType != config.fuelType)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У транспорта другой тип топлива", 3000);
                    return;
                } 

                int tfuel = vehicle.Data.Fuel + liters;
                if (tfuel > config.MaxFuel)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Введите правильные данные (до {config.MaxFuel})", 3000);
                    return;
                }
                Business biz = GetBusiness(player.GetData<int>("BIZ_ID"));
                int fuelProduct = fuelType - 1;
                if (fuelProduct >= biz.Products.Count)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Эта заправка не продаёт данный тип топлива.", 3000);
                    return;
                }

                int bizPricePerLiter = biz.Products[fuelProduct].Price;
                if (payType == PaymentsType.Gov)
                {
                    var fraction = Manager.GetFraction(player);
                    if (fraction == null || fraction.OrgActiveType != OrgActivityType.Government)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Чтобы заправить транспорт за гос. счет, Вы должны состоять в гос. организации", 3000);
                        return;
                    }
                    if (vehicle.Data.OwnerType != OwnerType.Fraction || vehicle.Data.OwnerID != fraction.Id)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы не можете заправить за государственный счет не государственный транспорт", 3000);
                        return;
                    }
                    if (fraction.FuelLeft < liters * bizPricePerLiter)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Лимит на заправку гос. транспорта за день исчерпан", 3000);
                        return;
                    }
                }
                string petrolName = Enum.GetName(typeof(PetrolType), fuelType);
                if (string.IsNullOrEmpty(petrolName))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Ошибка определения топлива, свяжитесь с администрацией.", 3000);
                    return;
                }
                TakeProd(player, biz, player.GetMoneyPayment(payType, Manager.GetFraction(6)), 
                    new BuyModel(petrolName, liters, false,
                    (cnt) =>
                    {
                        VehicleStreaming.SetVehicleFuel(vehicle, vehicle.Data.Fuel + cnt);
                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Транспорт заправлен", 3000);
                        Chat.Action(player, "заправил транспортное средство");
                        if (payType == PaymentsType.Gov)
                        {
                            Manager.GetFraction(player).FuelLeft -= liters * bizPricePerLiter;
                        }
                        biz.UpdateBlip();
                        return cnt;
                    }),
                    $"Покупка топлива {petrolName}", null);
            }
            catch (Exception e) { _logger.WriteError("Petrol: " + e.ToString()); }
        }

        public static void OpenPetrolMenu(ExtPlayer player)
        {
            Business biz = BizList[player.GetData<int>("BIZ_ID")];
            SafeTrigger.ClientEvent(player, "openPetrol", biz.Products[0].Price, biz.Products[1].Price, biz.Products[2].Price, biz.Products[3].Price, biz.Products[4].Price);
        }
    }
}
