using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Phone.Forbes.Models;
using Whistler.SDK;

namespace Whistler.Phone.Forbes
{
    class ForbesHandler : Script
    {
        private static List<ForbesPlace> ForbesList = new List<ForbesPlace>();
        private static List<int> _blockUUID = new List<int>
        {
            1001415,
        };
        public static void InitForbes()
        {
            var result = MySQL.QueryRead("SELECT ch.`uuid`, ch.`money` + IFNULL(m.`Balance`, 0) as amountmoney FROM `characters` ch LEFT JOIN `efcore_bank_account` m ON m.ID = ch.banknew WHERE ch.adminlvl = 0 AND `deleted` = false ORDER BY amountmoney DESC LIMIT 100");
            if (result != null && result.Rows.Count > 0)
                foreach (DataRow row in result.Rows)
                {
                    if (ForbesList.Count >= 20)
                        return;
                    int uuid = Convert.ToInt32(row["uuid"]);
                    if (_blockUUID.Contains(uuid))
                        continue;
                    ForbesList.Add(new ForbesPlace { 
                        UUID = uuid,
                        Money = Convert.ToUInt32(row["amountmoney"]),
                        Name = Main.PlayerNames.GetValueOrDefault(uuid, "Unknown")
                    });
                }
        }

        public static void PlayerLoadForbesList(ExtPlayer player)
        {
            player.TriggerCefEvent("smartphone/forbesPage/setForbesItems", JsonConvert.SerializeObject(ForbesList));
        }
    }
}