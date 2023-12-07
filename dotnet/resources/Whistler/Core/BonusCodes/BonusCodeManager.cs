using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.SDK;

namespace Whistler.Core.BonusCodes
{
    class BonusCodeManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(BonusCodeManager));
        public static List<BonusModel> BonusList = new List<BonusModel>();
        public BonusCodeManager()
        {
            try
            {
                var result = MySQL.QueryRead("SELECT * FROM `bonuscodes` WHERE `dateoff` > @prop0", MySQL.ConvertTime(DateTime.UtcNow));
                if (result == null || result.Rows.Count == 0)
                    return;
                foreach (DataRow row in result.Rows)
                {
                    BonusList.Add(new BonusModel(
                        Convert.ToInt32(row["id"]),
                        row["bonusname"].ToString(),
                        Convert.ToDateTime(row["dateoff"]),
                        Convert.ToInt32(row["money"]),
                        Convert.ToInt32(row["coins"]),
                        Convert.ToInt32(row["prime"])
                        ));
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError($"BonusCodeManager constructor {ex}");
            }
        }

        [Command("newbonuscode")]
        private static void CreateBonusCode(ExtPlayer player, string name, int money, int coins, int prime, int length)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "newbonuscode")) return;
                name = name.ToLower();
                if (BonusList.FirstOrDefault(item => item.Name == name) != null)
                {
                    Notify.SendError(player, "bcode:exists");
                    return;
                }
                if (length <= 0)
                {
                    Notify.SendError(player, "bcode:more:0");
                    return;
                }
                var result = MySQL.QueryRead("INSERT INTO `bonuscodes` (`bonusname`, `money`, `coins`, `prime`, `dateoff`) VALUES (@prop0, @prop1, @prop2, @prop3, @prop4); SELECT @@identity;", name, money, coins, prime, MySQL.ConvertTime(DateTime.UtcNow.AddDays(length)));
                if (result != null && result.Rows.Count > 0)
                {
                    BonusList.Add(new BonusModel(Convert.ToInt32(result.Rows[0][0]), name, DateTime.UtcNow.AddDays(length), money, coins, prime));
                    Notify.SendSuccess(player, "bcode:add");
                }
                else
                    Notify.SendError(player, "bcode:err");
            }
            catch (Exception ex)
            {
                _logger.WriteError($"CreateBonusCode {ex}");
                Notify.SendError(player, "bcode:err:exc");
            }
        }

        [Command("deletebonuscode")]
        private static void DeleteBonusCode(ExtPlayer player, string name)
        {
            if (!Group.CanUseAdminCommand(player, "deletebonuscode")) return;
            name = name.ToLower();
            var bonus = BonusList.FirstOrDefault(item => item.Name == name && item.Timeout > DateTime.UtcNow);
            if (bonus == null)
            {
                Notify.SendError(player, "bcode:nofound");
                return;
            }
            bonus.Timeout = DateTime.UtcNow;
            MySQL.Query("UPDATE `bonuscodes` SET `dateoff` = @prop1 WHERE `id` = @prop0", bonus.ID, MySQL.ConvertTime(bonus.Timeout));
            Notify.SendSuccess(player, "bcode:end");
        }

        [Command("bonuscode")]
        private static void UseBonusCode(ExtPlayer player, string name)
        {
            if (!player.IsLogged()) return;
            var accout = player.Account;
            name = name.ToLower();
            var bonus = BonusList.FirstOrDefault(item => item.Name == name && item.Timeout > DateTime.UtcNow);
            if (bonus == null)
            {
                Notify.SendError(player, "bonuscode_1");
                return;
            }
            if (accout.UsedBonuses.Contains(name))
            {
                Notify.SendError(player, "bonuscode_2");
                return;
            }
            accout.UsedBonuses.Add(bonus.Name);

            if (bonus.Coins > 0)
            {
                player.AddMCoins(bonus.Coins);
                NewDonateShop.DonateLog.OperationLog(player, bonus.Coins, $"bonuscode({bonus.Name})");
            }
            if (bonus.Money > 0)
            {
                Wallet.MoneyAdd(player.Character, bonus.Money, "Money_Bonuscode".Translate(bonus.Name));
            }
            if (bonus.PrimeDays > 0)
            {
                player.AddPrime(bonus.PrimeDays);
            }
            MySQL.Query("UPDATE accounts SET usedbonuses = @prop1 WHERE idkey = @prop0", accout.Id, Newtonsoft.Json.JsonConvert.SerializeObject(accout.UsedBonuses));
            MySQL.Query("UPDATE `bonuscodes` SET `countuse` = `countuse` + 1 WHERE `id` = @prop0", bonus.ID);
            Notify.SendSuccess(player, "bonuscode_3");

        }

    }
}
