using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.NewDonateShop.Enums;
using Whistler.SDK;

namespace Whistler.NewDonateShop
{
    public abstract class Wallet
    {
       
        protected string _databaseName;
        public Wallet()
        {
            _databaseName = Main.ServerConfig.DonateConfig.Database;
            InitDatabase();
            //Test();
        }
        
        public void OrderCoinKit(ExtPlayer player, int id)
        {
            try
            {
                var kit = Main.ServerConfig.DonateConfig.CoinKits.FirstOrDefault(k => k.Id == id);
                if (kit == null) return;
                var account = player.Account;
                var link = CreateOrder(account.Login, OrderTypes.Kit, kit.Price, kit.Coins, account.PromoUsed, account.Email);
                SafeTrigger.ClientEvent(player,"dshop:wallet:redirect", link);
            }
            catch (Exception ex)
            {
                DonateLog.ErrorLog(ex.ToString());
            }
        }

        public void OrderCoins(ExtPlayer player, int count)
        {
            try
            {
                if (count < 1) return;
                var account = player.Account;
                var sum = count / Main.ServerConfig.DonateConfig.RubToCoin;
                var link = CreateOrder(account.Login, OrderTypes.Coins, sum, count, account.PromoUsed, account.Email);
                SafeTrigger.ClientEvent(player,"dshop:wallet:redirect", link);
            }
            catch (Exception ex)
            {
                DonateLog.ErrorLog(ex.ToString());
            }
        }

        public void ExchangeCoinsToMoney(ExtPlayer player, int amount)
        {
            if (amount < 1) return;
            var price = amount / Main.ServerConfig.DonateConfig.CoinToVirtual;
            if (amount > price * Main.ServerConfig.DonateConfig.CoinToVirtual) price++;
            if (player.Account.MCoins < price)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У вас недостаточно средств", 3000);
                player.UpdateCoins();
                return;
            }
            player.SubMCoins(price);
            MoneySystem.Wallet.MoneyAdd(player.Character, amount, $"Конвертация донат-валюты ({amount})");
            DonateLog.OperationLog(player, price, "coins to money");
        }

        public void BuyMoneyPack(ExtPlayer player, int amount, int price)
        {
            if (amount < 1) return;
            if (player.Account.MCoins < price)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У вас недостаточно средств", 3000);
                player.UpdateCoins();
                return;
            }
            player.SubMCoins(price);
            MoneySystem.Wallet.MoneyAdd(player.Character, amount, "Покупка упаковки денег");
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы приобрели ${amount}", 3000);
            DonateLog.OperationLog(player, price, "buy money pack");
        }
        protected string CreateOrder(string login, OrderTypes type, int sum, int coins, string promo, string email)
        {
            try
            {
                var data = MySQL.QueryRead("INSERT INTO `{_databaseName}`(`type`,`value`,`sum`,`date`,`login`,`promo`) VALUES(@prop0,@prop1,@prop2,@prop3,@prop4,@prop5); SELECT @@identity;", type, sum, coins, MySQL.ConvertTime(DateTime.Now), login, promo);
                var id = Convert.ToInt32(data.Rows[0][0]);
                return GetPayUrl(sum, id, email, $"{coins} GO-COINS");
            }
            catch (Exception e)
            {
                DonateLog.ErrorLog(e.ToString());
                return "error";
            }
        }

        protected class Result
        {
            public string status { get; set; }
            public string result { get; set; }
        }
        protected abstract string GetPayUrl(int sum, int orderId, string email, string comment);
        private void InitDatabase()
        {
            var query = $"CREATE TABLE IF NOT EXISTS `{_databaseName}`(" +
                $"`id` int(11) NOT NULL AUTO_INCREMENT," +
                $"`{_databaseName}_id` int(11) NOT NULL DEFAULT '0'," +
                $"`login` varchar(45) NOT NULL," +
                $"`type` varchar(15) NOT NULL," +
                $"`value` int(11) NOT NULL," +
                $"`sum` int(11) NOT NULL DEFAULT '0'," +
                $"`promo` varchar(45) NOT NULL DEFAULT 'noref'," +
                $"`date` datetime NOT NULL," +
                $"PRIMARY KEY(`id`)" +
                $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
            MySQL.Query(query);
            query = $"CREATE TABLE IF NOT EXISTS `{_databaseName}_errors`(" +
                $"`id` int(11) NOT NULL AUTO_INCREMENT," +
                $"`orderid` varchar(11) NOT NULL," +
                $"`error` TEXT NOT NULL," +
                $"`date` datetime NOT NULL," +
                $"PRIMARY KEY(`id`)" +
                $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
            MySQL.Query(query);
            query = $"CREATE TABLE IF NOT EXISTS `{_databaseName}_history`(" +
                $"`id` int(11) NOT NULL AUTO_INCREMENT," +
                $"`name` varchar(90) NOT NULL," +
                $"`operation` TEXT NOT NULL," +
                $"`sum` int(11) NOT NULL," +
                $"`date` datetime NOT NULL," +
                $"PRIMARY KEY(`id`)" +
                $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
            MySQL.Query(query);
            query = $"DELETE FROM `{_databaseName}` WHERE `date` < NOW() - INTERVAL 1 DAY AND `{_databaseName}_id` = 0;";
            MySQL.Query(query);
        }

        private void Test()
        {
            Console.WriteLine(GetPayUrl(1000, 9999999, "sireleot@gmail.com", "dwdqdw"));
        }
    }
}
