using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.NewDonateShop.Models;
using Whistler.Helpers;
using Whistler.Entities;

namespace Whistler.NewDonateShop
{
    class DonateLog
    {   internal static void ErrorLog(string error, string orderId = "no info")
        {           
            NAPI.Util.ConsoleOutput($"DONATE ERROR: ${error}");
            MySQL.Query($"INSERT INTO `{Main.ServerConfig.DonateConfig.Database}_errors` (`orderid`,`error`,`date`) VALUES(@prop0,@prop1,@prop2);", orderId, error, MySQL.ConvertTime(DateTime.Now));    
        }
        internal static void OperationLog(ExtPlayer client, int sum, string operation)
        {
            MySQL.Query($"INSERT INTO `{Main.ServerConfig.DonateConfig.Database}_history`(`login`,`name`,`operation`,`date`,`sum`) VALUES(@prop0, @prop1, @prop2, @prop3, @prop4)", client.Account.Login, $"{client.Character.FirstName}_{client.Character.LastName}", operation, MySQL.ConvertTime(DateTime.Now), sum);
        }

        internal static void DonateItemlog(ExtPlayer player, ItemModel item,  string operation)
        {
            MySQL.Query("INSERT INTO `donate_items_log`(`login`,`uuid`,`itemid`, `itemname`, `sum`, `operation`, `date`) VALUES(@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6)", player.Account.Login, player.Character.UUID, item.Id, item.Name, item.Price, operation, MySQL.ConvertTime(DateTime.Now));
        }
        internal static void DonateItemlog(ExtPlayer player, ItemModel item, int price, string operation)
        {
            MySQL.Query("INSERT INTO `donate_items_log`(`login`,`uuid`,`itemid`, `itemname`, `sum`, `operation`, `date`) VALUES(@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6)", player.Account.Login, player.Character.UUID, item.Id, item.Name, price, operation, MySQL.ConvertTime(DateTime.Now));
        }
        internal static void DebugLog(string log)
        {
            //if (Donate.Config.DonateDebug) NAPI.Util.ConsoleOutput($"DEBUG DONAT LOG: {log}");
            //if (Donate.Config.Debug) NAPI.Util.ConsoleOutput($"DEBUG DONAT LOG: {log}");
        }
    }
}
