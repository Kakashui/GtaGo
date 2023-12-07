using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Common;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Families.Models;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.NewDonateShop;
using Whistler.NewDonateShop.Configs;
using Whistler.PriceSystem;
using Whistler.SDK;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.VehicleSystem
{
    class VehicleOperations
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(VehicleOperations));
        public static bool CheckCorrectVehiclePrice(ExtPlayer player, string model, int price, bool notify = true)
        {
            var modelPrice = PriceManager.GetPriceInDollars(TypePrice.Car, model, 0);
            if (modelPrice == 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_89", 3000);
                return false;
            }
            if (price < modelPrice / 2 || price > modelPrice * 3)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_92".Translate(modelPrice / 2, modelPrice * 3), 3000);
                return false;
            }
            return true;
        }

        public static void SellVeh(ExtPlayer player, int vehId)
        {
            PersonalBaseVehicle vData = VehicleManager.GetVehicleBaseByUUID(vehId) as PersonalBaseVehicle;
            if (vData == null) return;
            if (vData.TradePoint > 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "veh:operation:1", 3000);
                return;
            }

            if (vData.CanAccessVehicle(player, AccessType.SellDollars))
                SellVehForMoney(player, vData, PriceManager.GetPriceInDollars(TypePrice.Car, vData.ModelName, 0));
            else if (vData.CanAccessVehicle(player, AccessType.SellRouletteCar))
                SellVehForDonate(player, vData);
            else if (vData.CanAccessVehicle(player, AccessType.SellZero))
                SellVehForMoney(player, vData, 0);
            else
                Notify.Send(player, NotifyType.Info, NotifyPosition.Bottom, "House_146", 3000);
        }

        private static void SellVehForMoney(ExtPlayer player, PersonalBaseVehicle vData, int price)
        {
            price = VehicleConstants.GetCorrectSalePrice(price, false);

            DialogUI.Open(player, "House_90".Translate(vData.ModelName, vData.Number, price), new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "dialog_0",
                    Icon = null,
                    Action = (p) =>
                    {
                        switch (vData.OwnerType)
                        {
                            case OwnerType.Personal:
                                MoneySystem.Wallet.MoneyAdd(player.Character, price, $"Продажа авто ({vData.ID}) в гос");
                                break;
                            case OwnerType.Family:
                                MoneySystem.Wallet.MoneyAdd(player.GetFamily(), price, $"Продажа авто ({vData.ID}) в гос");
                                break;
                            default:
                                return;
                        }
                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Main_182".Translate( vData.ModelName, vData.Number, price), 3000);
                        VehicleManager.Remove(vData.ID);
                        MainMenu.SendProperty(player);
                    }
                },
                new DialogUI.ButtonSetting
                {
                    Name = "dialog_1",
                    Icon = null,
                    Action = { }
                }
            });
        }
        private static void SellVehForDonate(ExtPlayer player, PersonalBaseVehicle vData)
        {
            try
            {
                if (vData.OwnerID != player.Character.UUID)
                    return;

                var priceInCoins = PriceManager.GetPriceInDollars(TypePrice.Car, vData.ModelName.ToLower(), 0);
                if (priceInCoins == 0)
                {
                    Notify.Send(player, NotifyType.Info, NotifyPosition.Bottom, "House_146", 3000);
                    return;
                }
                int price = (int)(priceInCoins * 0.6);

                DialogUI.Open(player, "House_90".Translate(vData.ModelName, vData.Number, price), new List<DialogUI.ButtonSetting>
                {
                    new DialogUI.ButtonSetting
                    {
                        Name = "dialog_0",
                        Icon = "confirm",
                        Action = (p) =>
                        {
                            MoneySystem.Wallet.MoneyAdd(player.Character, price, $"Продажа авто ({vData.ID}) в гос");
                            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Main_182".Translate(vData.ModelName, vData.Number, price), 3000);
                            VehicleManager.Remove(vData.ID);
                            MainMenu.SendProperty(player);
                            return;
                        }
                    },
                    new DialogUI.ButtonSetting
                    {
                        Name = "dialog_1",
                        Icon = "cancel",
                        Action = { }
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.WriteError($"SellVehForDonate:\n {ex}");
            }
        }
    }
}
