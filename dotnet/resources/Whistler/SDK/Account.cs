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
        public int TotalPlyed { get; protected set; }
        public DateTime BonusBegineAt { get; protected set; }
        public bool BonusCompleete { get; protected set; }
        public string PromoUsed { get; set; }
        public bool PromoReceived { get; set; }
        public List<string> UsedBonuses { get; set; }
        public int MCoins { get; protected set; } = 0;

        public List<int> Characters { get; protected set; } // characters uuids
        public int LastCharacter { get; protected set; }
        public bool SubMCoins(int count)
        {
            if (count < 1) return false;
            if (MCoins < count) return false;

            MCoins -= count;
            MySQL.QuerySync("UPDATE `accounts` SET `mcoins` = `mcoins` - @prop0 WHERE `login` = @prop1", count, Login);
            return true;
        }
        public bool AddMCoins(int count)
        {
            if (count < 1) return false;

            MCoins += count;
            MySQL.QuerySync("UPDATE `accounts` SET `mcoins` = `mcoins` + @prop0 WHERE `login` = @prop1", count, Login);
            return true;
        }
        public static bool AddOffMCoins(int uuid, int count)
        {
            if (count < 1 || !Main.UUIDs.Contains(uuid)) return false;

            MySQL.QuerySync("UPDATE `accounts` SET `mcoins` = `mcoins` + @prop0 WHERE `character1` = @prop1 OR `character2` = @prop1 OR `character3` = @prop1", count, uuid);
            return true;
        }
        public AccountData(string email, string login, string pass, ulong socialClubId, string hwid, string ip)
        {

            Password = Account.GetSha256(pass);
            Login = login;
            Email = email;

            Characters = new List<int>() { -1, -2, -2 }; // -1 - empty slot, -2 - non-purchased slot

            HWID = hwid;
            IP = ip;
            SocialClubId = socialClubId;
            LastCharacter = 0;
            BonusBegineAt = DateTime.Now;
            PromoReceived = false;
            MySQL.Query("INSERT INTO `accounts` (`login`, `email`, `password`, `hwid`, `ip`, `socialclubid`, `mcoins`,`character1`, `character2`, `character3`, `lang`, `lastCharacter`, `usedbonuses`, `bonusbegine`) " +
                "VALUES (@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, '0', '-1', '-2', '-2', @prop6, 0, @prop7, @prop8)",
                Login, Email, Password, HWID, IP, SocialClubId, "ru", JsonConvert.SerializeObject(UsedBonuses), MySQL.ConvertTime(BonusBegineAt));

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
            PromoUsed = row["promoused"] == DBNull.Value ? null : row["promoused"].ToString();
            PromoReceived = string.IsNullOrEmpty(PromoUsed) ? false : Convert.ToBoolean(row["promoreceived"]);

            UsedBonuses = JsonConvert.DeserializeObject<List<string>>(row["usedbonuses"].ToString()) ?? new List<string>();
            var char1 = Convert.ToInt32(row["character1"]);
            var char2 = Convert.ToInt32(row["character2"]);
            var char3 = Convert.ToInt32(row["character3"]);
            Characters = new List<int>() { char1, char2, char3 };
            LastCharacter = Convert.ToInt32(row["lastCharacter"]);
            if (LastCharacter < 0 || LastCharacter > 2) LastCharacter = 0;

            MCoins = Convert.ToInt32(row["mcoins"]);
        }

    }
}