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
        public void ClientEvent_OverSpeed(PlayerGo player, int speed, int speedlimit, int sum, string reason)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!player.IsInVehicle || player.VehicleSeat != VehicleConstants.DriverSeat) return;
                if (player.Character.LVL <= 1) return;
                VehicleGo vehGo = player.Vehicle.GetVehicleGo();
                if (vehGo.Data.OwnerType == OwnerType.Fraction)
                {
                    switch (vehGo.Data.GetHolderName())
                    {
                        case "CITY": //мэрия
                        case "POLICE": //полиция
                        case "EMS": //скорая
                        case "FIB": //фбр
                        case "ARMY": //армия
                            return;
                    }
                }
                Notify.Send(player, NotifyType.Alert, NotifyPosition.BottomCenter, $"Core_70".Translate(speed - speedlimit, sum), 3000);
                player.GetCharacter().Mulct += sum;
                player.TriggerCefEvent("smartphone/bankPage/setPenaltySum", player.GetCharacter().Mulct);

                //int mulct = player.GetCharacter().Mulct;

                //;
                //if (!Wallet.MoneySub(player, player.GetCharacter().Mulct, PaymentsType.Cash, "frac(6)", "paypdd") && 
                //    !Wallet.MoneySub(player, player.GetCharacter().Mulct, PaymentsType.Card, "frac(6)", "paypdd"))
                //{
                //    mulct = (int)player.GetCharacter().Money;
                //    player.GetCharacter().Mulct -= mulct;
                //    Wallet.MoneySub(player, mulct, PaymentsType.Cash, "frac(6)", "paypdd");
                //    if (player.GetCharacter().Mulct > 0)
                //        if (!Wallet.MoneySub(player, player.GetCharacter().Mulct, PaymentsType.Card, "frac(6)", "paypdd"))
                //        {
                //            var mulctBank = (int)MoneySystem.Bank.Accounts[player.GetCharacter().Bank].Balance;
                //            player.GetCharacter().Mulct -= mulctBank;
                //            Wallet.MoneySub(player, mulctBank, PaymentsType.Card, "frac(6)", "paypdd");
                //            mulct += mulctBank;
                //        }
                //        else
                //        {
                //            mulct += player.GetCharacter().Mulct;
                //            player.GetCharacter().Mulct = 0;
                //        }
                //}
                //else
                //{
                //    player.GetCharacter().Mulct = 0;
                //}
                //MySQL.Query($"UPDATE characters SET mulct={player.GetCharacter().Mulct} WHERE uuid={player.GetCharacter().UUID}");
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