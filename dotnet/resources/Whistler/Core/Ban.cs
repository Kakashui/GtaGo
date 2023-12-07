using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Core
{
    public class BanData
    {
        public int UUID;
        public string Name;
        public string Account;
        public DateTime Time;
        public DateTime Until;
        public bool isHard;
        public string IP;
        public string SocialClub;
        public string HWID;
        public string Reason;
        public string ByAdmin;
    }
    class Ban : BanData
    {
        public static List<Ban> Banned = new List<Ban>();
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Ban));
        public static void Sync()
        {
            lock (Banned)
            {
                Banned.Clear();
                DataTable result = MySQL.QueryRead("select * from banned");
                if (result == null || result.Rows.Count == 0) return;
                foreach (DataRow row in result.Rows)
                {
                    Banned.Add(new Ban()
                    {
                        UUID = Convert.ToInt32(row["uuid"]),
                        Name = Convert.ToString(row["name"]),
                        Account = Convert.ToString(row["account"]),
                        Time = Convert.ToDateTime(row["time"]),
                        Until = Convert.ToDateTime(row["until"]),
                        isHard = Convert.ToBoolean(row["ishard"]),
                        IP = Convert.ToString(row["ip"]),
                        SocialClub = Convert.ToString(row["socialclub"]),
                        HWID = Convert.ToString(row["hwid"]),
                        Reason = Convert.ToString(row["reason"]),
                        ByAdmin = Convert.ToString(row["byadmin"])
                    });
                }
            }
        }

        #region
        public static Ban Get1(ExtPlayer player)
        {
            lock (Banned)
            {
                var ban = Banned.FirstOrDefault(x => x.SocialClub == player.SocialClubName || x.HWID == player.Serial);
                return ban;
            }
        }

        public static Ban Get2(int UUID)
        {
            lock (Banned)
            {
                return Banned.FirstOrDefault(x => x.UUID == UUID);
            }
        }

        public bool CheckDate()
        {
            if (DateTime.Now <= Until)
            {
                return true;
            }
            else
            {
                MySQL.Query("DELETE FROM banned WHERE uuid = @prop0", this.UUID);
                lock (Banned)
                {
                    Banned.Remove(this);
                }
                return false;
            }
        }
        #endregion

        #region 
        public static void Online(ExtPlayer client, DateTime until, bool ishard, string reason, string admin)
        {
            var acc = client.Character;
            if (acc == null) {
                _logger.WriteError($"Can't ban player {client.Name}");
                return;
            }
            Ban ban = new Ban()
            {
                UUID = acc.UUID,
                Name = acc.FirstName + "_" + acc.LastName,
                Account = client.Account.Login,
                Time = DateTime.Now,
                Until = until,
                isHard = ishard,
                IP = client.Address,
                SocialClub = client.SocialClubName,
                HWID = client.Session.HWID,
                Reason = reason,
                ByAdmin = admin
            };            
            Banned.Add(ban); 
            MySQL.Query("INSERT INTO `banned`(`uuid`,`name`,`account`,`time`,`until`,`ishard`,`ip`,`socialclub`,`hwid`,`reason`,`byadmin`) VALUES (@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6, @prop7, @prop8, @prop9, @prop10)",
                 ban.UUID, ban.Name, ban.Account, MySQL.ConvertTime(ban.Time), MySQL.ConvertTime(ban.Until), ban.isHard, ban.IP, ban.SocialClub, ban.HWID, ban.Reason, ban.ByAdmin);
        }

        public static void UpdateBan(int uuid)
        {
            var ban = Banned.FirstOrDefault(b => b.UUID == uuid);
            if (ban == null) return;

            MySQL.Query("UPDATE `banned` SET `account` = @prop0,`hwid` = @prop1 WHERE `uuid` = @prop2", ban.Account, ban.HWID, uuid);
        }
        public static bool Offline(string nickname, DateTime until, bool ishard, string reason, string admin)
        {
            if (Banned.FirstOrDefault(b => b.Name == nickname) != null) return false;

            if (!Main.PlayerUUIDs.ContainsKey(nickname)) return false;

            var uuid = Main.PlayerUUIDs[nickname];
            if (uuid == -1) return false;

            var ip = "";
            var socialclub = "";
            var account = "";
            var hwid = "";

            if (ishard)
            {
                DataTable result = MySQL.QueryRead("SELECT `hwid`,`socialclub`,`ip`,`login` FROM `accounts` WHERE `character1` = @prop0 OR `character2` = @prop0 OR `character3` = @prop0", uuid);
                var row = result.Rows[0];
                if (result == null || result.Rows.Count == 0) return false;
                ip = row["ip"].ToString();
                socialclub = row["socialclub"].ToString();
                account = row["login"].ToString();
                hwid = row["hwid"].ToString();
            }

            Ban ban = new Ban()
            {
                UUID = uuid,
                Name = nickname,
                Account = account,
                Time = DateTime.Now,
                Until = until,
                isHard = ishard,
                IP = ip,
                SocialClub = socialclub,
                HWID = hwid,
                Reason = reason,
                ByAdmin = admin
            };
            MySQL.Query("INSERT INTO `banned`(`uuid`,`name`,`account`,`time`,`until`,`ishard`,`ip`,`socialclub`,`hwid`,`reason`,`byadmin`) VALUES (@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6, @prop7, @prop8, @prop9, @prop10)",
                ban.UUID, ban.Name, ban.Account, MySQL.ConvertTime(ban.Time), MySQL.ConvertTime(ban.Until), ban.isHard, ban.IP, ban.SocialClub, ban.HWID, ban.Reason, ban.ByAdmin);
            Banned.Add(ban);
            return true;
        }
        #endregion

        #region 
        public static bool PardonHard(string nickname)
        {
            lock (Banned)
            {
                int index = Banned.FindIndex(x => x.Name == nickname);
                if (index < 1) return false;

                Banned[index].isHard = false;
                MySQL.Query("UPDATE banned SET ishard=false WHERE name = @prop0", nickname);
                return true;
            }
        }
        public static bool PardonHard(int uuid)
        {
            lock (Banned)
            {
                int index = Banned.FindIndex(x => x.UUID == uuid);
                if (index < 1) return false;

                Banned[index].isHard = false;
                MySQL.Query("UPDATE banned SET ishard = false WHERE uuid = @prop0", uuid);
                return true;
            }
        }
        #endregion
        #region
        public static bool Pardon(string nickname)
        {
            lock (Banned)
            {
                int index = Banned.FindIndex(x => x.Name == nickname);
                if (index < 0) return false;

                Banned.RemoveAt(index);
                MySQL.Query("DELETE FROM banned WHERE name = @prop0", nickname);
                return true;
            }
        }
        public static bool Pardon(int uuid)
        {
            lock (Banned)
            {
                int index = Banned.FindIndex(x => x.UUID == uuid);
                if (index < 0) return false;

                Banned.RemoveAt(index);
                MySQL.Query("DELETE FROM banned WHERE uuid = @prop0", uuid);
                return true;
            }
        }
        #endregion
    }
}
