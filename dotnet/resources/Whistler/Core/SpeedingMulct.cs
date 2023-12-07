using GTANetworkAPI;
using System;
using Whistler.SDK;
using Whistler.VehicleSystem.Models;
using Whistler.Helpers;
using Whistler.VehicleSystem;
using Whistler.Common;
using Whistler.Entities;
//using MySql.Data.MySqlClient;

namespace Whistler.Core
{
    class SpeedingMulct : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(SpeedingMulct));

        [RemoteEvent("speeding_mulct")]
        public void ClientEvent_OverSpeed(ExtPlayer player, int speed, int speedlimit, int sum, string reason)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!player.IsInVehicle || player.VehicleSeat != VehicleConstants.DriverSeat) return;
                if (player.Character.LVL <= 1) return;
                ExtVehicle extVehicle = player.Vehicle as ExtVehicle;
                if (extVehicle.Data.OwnerType == OwnerType.Fraction)
                {
                    switch (extVehicle.Data.GetHolderName())
                    {
                        case "CITY": //мэрия
                        case "POLICE": //полиция
                        case "EMS": //скорая
                        case "FIB": //фбр
                        case "ARMY": //армия
                            return;
                    }
                }
                Notify.Send(player, NotifyType.Alert, NotifyPosition.BottomCenter, $"Вы превысили скорость на {speed - speedlimit} км/ч! Штраф {sum}$", 3000);
                player.Character.Mulct += sum;
                player.TriggerCefEvent("smartphone/bankPage/setPenaltySum", player.Character.Mulct);

                //int mulct = player.Character.Mulct;

                //;
                //if (!Wallet.MoneySub(player, player.Character.Mulct, PaymentsType.Cash, "frac(6)", "paypdd") && 
                //    !Wallet.MoneySub(player, player.Character.Mulct, PaymentsType.Card, "frac(6)", "paypdd"))
                //{
                //    mulct = (int)player.Character.Money;
                //    player.Character.Mulct -= mulct;
                //    Wallet.MoneySub(player, mulct, PaymentsType.Cash, "frac(6)", "paypdd");
                //    if (player.Character.Mulct > 0)
                //        if (!Wallet.MoneySub(player, player.Character.Mulct, PaymentsType.Card, "frac(6)", "paypdd"))
                //        {
                //            var mulctBank = (int)MoneySystem.Bank.Accounts[player.Character.Bank].Balance;
                //            player.Character.Mulct -= mulctBank;
                //            Wallet.MoneySub(player, mulctBank, PaymentsType.Card, "frac(6)", "paypdd");
                //            mulct += mulctBank;
                //        }
                //        else
                //        {
                //            mulct += player.Character.Mulct;
                //            player.Character.Mulct = 0;
                //        }
                //}
                //else
                //{
                //    player.Character.Mulct = 0;
                //}
                //MySQL.Query("UPDATE characters SET mulct={player.Character.Mulct} WHERE uuid={player.Character.UUID}");
                //if (mulct > 0)
                //{
                //    Notify.Send(player, NotifyType.Alert, NotifyPosition.BottomCenter, "local_37".Translate( mulct), 3000);
                //    Stocks.DepositMoney(6, mulct);
                //}
            }
            catch (Exception ex)
            {
                _logger.WriteError($"speeding_mulct: {ex.ToString()}");
            };
        }
    }
}