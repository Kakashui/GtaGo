using System;
using System.Collections.Generic;
using GTANetworkAPI;
using System.Data;
using System.Linq;
using Whistler.GUI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.Core.nAccount;
using Whistler.Core;
using MySql.Data.MySqlClient;

namespace Whistler.SDK
{
    public class AccountData
    {
        public int Id { get; protected set; }
        public string Login { get; protected set; }
        public string Email { get; protected set; }
        public string Password { get; protected set; }

        public string HWID { get; protected set; }
        public string IP { get; protected set; }
        public string SocialClub { get; protected set; }
        public ulong SocialClubId { get; protected set; }
        public int AvailableSlots { get; protected set; }
        public int TotalPlyed { get; protected set; }
        public DateTime BonusBegineAt { get; protected set; }
        public bool BonusCompleete { get; protected set; }
        public string PromoUsed { get; set; }
        public List<string> UsedBonuses { get; set; }

        private int _gocoins;
        public int GoCoins {
            get {
                DataTable result = MySQL.QueryRead($"SELECT `gocoins` FROM `accounts` WHERE `login` = @prop0", Login);
                if (result == null || result.Rows.Count == 0)
                    return 0;
                _gocoins = Convert.ToInt32(result.Rows[0]["gocoins"]);
                return _gocoins;
            }
        }
        public DateTime VipDate { get; protected set; } = DateTime.UtcNow;

        public List<int> Characters { get; protected set; } // characters uuids
        public int LastCharacter { get; protected set; }
        public bool SubGoCoins(int count)
        {
            if (count < 1) return false;
            MySQL.QuerySync($"UPDATE `accounts` SET `gocoins` = `gocoins` - @prop0 WHERE `login` = @prop1", count, Login);
            return true;
        }
        public bool AddGoCoins(int count)
        {
            if (count < 1) return false;
            MySQL.QuerySync($"UPDATE `accounts` SET `gocoins` = `gocoins` + @prop0 WHERE `login` = @prop1", count, Login);
            return true;
        }
        public static bool AddOffGoCoins(int uuid, int count)
        {
            if (count < 1 || !Main.UUIDs.Contains(uuid)) return false;
            MySQL.QuerySync($"UPDATE `accounts` SET `gocoins` = `gocoins` + @prop0 WHERE `character1` = @prop1 OR `character2` = @prop1 OR `character3` = @prop1", count, uuid);
            return true;
        }
        public AccountData(string email, string login, string pass, ulong socialClubId, string hwid, string ip)
        {

            Password = Account.GetSha256(pass);
            Login = login;
            Email = email;

            Characters = new List<int>() { -1, -1, -2 }; // -1 - empty slot, -2 - non-purchased slot

            HWID = hwid;
            IP = ip;
            SocialClubId = socialClubId;
            LastCharacter = 0;
            AvailableSlots = 2;
            BonusBegineAt = DateTime.Now;
            VipDate = DateTime.UtcNow;
            //MySqlCommand cmd = new MySqlCommand
            //{
            //    CommandText = "INSERT INTO `othervehicles`(`type`, `number`, `model`, `position`, `rotation`, `color1`, `color2`, `price`) VALUES (@type, @number, @model, @pos, @rot, @c1, @c2, @price);"
            //};
            MySqlCommand command = new MySqlCommand(@"INSERT INTO `accounts` 
            (`socialclub`, `login`, `email`, `password`, `hwid`, `ip`, `socialclubid`, `gocoins`, `vipdate`, `character1`, `character2`, `character3`, `lang`, `availableSlots`, `lastCharacter`, `usedbonuses`, `bonusbegine`) 
            VALUES (@socialclub, @login, @email, @password, @hwid, @ip, @socialclubid, @gocoins, @vipdate, @character1, @character2, @character3, @lang, @availableSlots, @lastCharacter, @usedbonuses, @bonusbegine );");

            command.Parameters.AddWithValue("@socialclub", "123");
            command.Parameters.AddWithValue("@login", Login);
            command.Parameters.AddWithValue("@email", Email);
            command.Parameters.AddWithValue("@password", Password);
            command.Parameters.AddWithValue("@hwid", HWID);
            command.Parameters.AddWithValue("@ip", IP);
            command.Parameters.AddWithValue("@socialclubid", SocialClubId);
            command.Parameters.AddWithValue("@gocoins", 0);
            command.Parameters.AddWithValue("@vipdate", MySQL.ConvertTime(VipDate));
            command.Parameters.AddWithValue("@character1", -1);
            command.Parameters.AddWithValue("@character2", -1);
            command.Parameters.AddWithValue("@character3", -2);
            command.Parameters.AddWithValue("@lang", "ru");
            command.Parameters.AddWithValue("@availableSlots", 2);
            command.Parameters.AddWithValue("@lastCharacter", 0);
            command.Parameters.AddWithValue("@usedbonuses", JsonConvert.SerializeObject(UsedBonuses));
            command.Parameters.AddWithValue("@bonusbegine", MySQL.ConvertTime(BonusBegineAt));
            MySQL.Query(command);
            //MySQL.Query("INSERT INTO `accounts` (`login`, `email`, `password`, `hwid`, `ip`, `socialclubid`, `gocoins`, `vipdate`,`character1`, `character2`, `character3`, `lang`, `availableSlots`, `lastCharacter`, `usedbonuses`, `bonusbegine`) " +
            //    "VALUES (login = @prop0, email = @prop1, password = @prop2, hwid = @prop3, ip = @prop4,socialclubid = @prop5, gocoins = '0', vipdate = @prop6, character1 = '-1', character2 = '-1', character3 = '-2',lang = @prop7, availableSlots = 2, lastCharacter = 0, usedbonuses = @prop8, bonusbegine = @prop9)",
            //    Login, Email, Password, HWID, IP, SocialClubId, MySQL.ConvertTime(VipDate), "ru", JsonConvert.SerializeObject(UsedBonuses), MySQL.ConvertTime(BonusBegineAt));//а новой базы не прелагается?

        }
        public AccountData(DataRow row, string hwid, string ip)
        {
            Id = Convert.ToInt32(row["idkey"]);
            Login = Convert.ToString(row["login"]);
            Email = Convert.ToString(row["email"]);
            Password = Convert.ToString(row["password"]);
            HWID = hwid;
            IP = ip;
            if (row.IsNull("bonusbegine"))
            {
                TotalPlyed = 0;
                BonusCompleete = false;
            }
            else
            {
                BonusBegineAt = Convert.ToDateTime(row["bonusbegine"]);
                TotalPlyed = Convert.ToInt32(row["totalplayed"]);
                BonusCompleete = Convert.ToBoolean(row["bonuscompleete"]);
            }

            SocialClubId = Convert.ToUInt64(row["socialclubid"].ToString());
            PromoUsed = row["promoused"].ToString();
            VipDate = (DateTime)row["vipdate"];

            UsedBonuses = JsonConvert.DeserializeObject<List<string>>(row["usedbonuses"].ToString()) ?? new List<string>();
            var char1 = Convert.ToInt32(row["character1"]);
            var char2 = Convert.ToInt32(row["character2"]);
            var char3 = Convert.ToInt32(row["character3"]);
            AvailableSlots = Convert.ToInt32(row["availableSlots"]);
            Characters = new List<int>() { char1, char2, char3 };
            LastCharacter = Convert.ToInt32(row["lastCharacter"]);
            if (LastCharacter < 0 || LastCharacter > 2)
                LastCharacter = 0;
        }

    }
}