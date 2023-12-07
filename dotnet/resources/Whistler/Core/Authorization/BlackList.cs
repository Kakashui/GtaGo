using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.Core.Authorization
{
    class BlackListItem
    {
        public BlackListItem(string serial, string socialclub)
        {
            Serial = serial;
            Socialclub = socialclub;
        }
        public string Serial { get; set; }
        public string Socialclub { get; set; }
    }
    class BlackList : Script
    {
        private static List<BlackListItem> _list;
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            var query = $"CREATE TABLE IF NOT EXISTS `blacklist`(" +
               $"`id` int(11) NOT NULL AUTO_INCREMENT," +
               $"`serial` TEXT NOT NULL," +
               $"`socialclub` TEXT NOT NULL," +
               $"`admin` TEXT NOT NULL," +
               $"`date` DATETIME NOT NULL," +
               $"PRIMARY KEY(`id`)" +
               $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
            MySQL.Query(query);
            _list = new List<BlackListItem>();
            var responce = MySQL.QueryRead("SELECT * FROM `blacklist`;");
            if (responce != null && responce.Rows.Count > 0)
            {
                foreach (DataRow item in responce.Rows)
                {
                    _list.Add(new BlackListItem(item["serial"].ToString(), item["socialclub"].ToString()));
                }
            }
        }

        [Command("blacklist")]
        public void AddToBlackList(ExtPlayer player, string name)
        {
            if (!Group.CanUseAdminCommand(player, "blacklist")) return;
            var online = Trigger.GetPlayerByName(name);
            if (online != null)
            {
                _list.Add(new BlackListItem(online.Serial, online.SocialClubName));
                ByeBye(online);
                MySQL.Query("INSERT INTO `blacklist` (`serial`, `socialclub`, `admin`, `date`) VALUES(@prop0, @prop1, @prop2, @prop3);", online.Serial, online.SocialClubName, player.Name, MySQL.ConvertTime(DateTime.Now));
            }
            else
            {
                var names = name.Split('_');
                if (names.Length < 2)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Неправильно указано имя", 3000);
                    return;
                }
                var responce = MySQL.QueryRead("SELECT `uuid` FROM `characters` WHERE `firstname`=@prop0 and `lastname`=@prop1;", names[0], names[1]);
                if (responce == null || responce.Rows.Count == 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "", 3000);
                    return;
                }
                var uuid = Convert.ToInt32(responce.Rows[0]["uuid"]);
                responce = MySQL.QueryRead("SELECT `hwid`, `socialclub` FROM `accounts` WHERE `character1`=@prop0 OR `character2`=@prop0 OR `character3`=@prop0;", uuid);
                if (responce == null || responce.Rows.Count == 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок найден", 3000);
                    return;
                }
                var serial = responce.Rows[0]["hwid"].ToString();
                var socialclub = responce.Rows[0]["socialclub"].ToString();
                _list.Add(new BlackListItem(serial, socialclub));
                MySQL.Query("INSERT INTO `blacklist` (`serial`, `socialclub`, `admin`, `date`) VALUES(@prop0, @prop1, @prop2, @prop3);", serial, socialclub, player.Name, MySQL.ConvertTime(DateTime.Now));
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Игрок добавлен в blacklist", 3000);
            }
        }
        [Command("blacklistupdate")]
        public void BlacklistUpdate(ExtPlayer player)
        {
            if (!Group.CanUseAdminCommand(player, "blacklistr")) return;
            _list = new List<BlackListItem>();
            var responce = MySQL.QueryRead("SELECT * FROM `blacklist`;");
            if (responce != null && responce.Rows.Count > 0)
            {
                foreach (DataRow item in responce.Rows)
                {
                    _list.Add(new BlackListItem(item["serial"].ToString(), item["socialclub"].ToString()));
                }
            }

        }

        public static bool Exists(ExtPlayer player)
        {
            if (_list.Any(c => c.Serial == player.Serial || c.Socialclub == player.SocialClubName))
            {
                ByeBye(player);
                return true;
            }
            else return false;
        }

        private static void ByeBye(ExtPlayer player)
        {
            SafeTrigger.UpdateDimension(player,  Dimensions.RequestPrivateDimension());
            player.Eval("while (true) {mp.game.wait(0)}");
            NAPI.Task.Run(() =>
            {
                player.Kick();
            }, 3000);
        }
    }
}
