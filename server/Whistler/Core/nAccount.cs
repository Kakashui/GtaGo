using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GTANetworkAPI;
using Whistler.SDK;
using Whistler.GUI;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using Whistler.VehicleSystem;
using System.Linq;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.NewDonateShop;
using Whistler.Entities;
using Whistler.MoneySystem;
using Whistler.Common;

namespace Whistler.Core.nAccount
{
    public class Account : AccountData
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Account));
        public Account(string email, string login, string pass, ulong socialClubId, string hwid, string ip) : base(email, login, pass, socialClubId, hwid, ip)
        {

        }
        public Account(DataRow row, string hwid, string ip) : base(row, hwid, ip)
        {

        }

        public void InitBonus()
        {
            if (!Main.ServerConfig.Bonus.OneInDay)
            {
                BonusCompleete = false;
            }
            else
            {
                if (BonusBegineAt.DayOfYear != DateTime.Now.DayOfYear)
                {
                    TotalPlyed = 0;
                    BonusCompleete = false;
                }
            }
            BonusBegineAt = DateTime.Now;
        }

        public int CheckBonus(PlayerGo player, bool needMessage = true)
        {
            if (BonusCompleete) return -1;
            var minutes = (int)(DateTime.Now - BonusBegineAt).TotalMinutes;
            TotalPlyed += minutes;
            if (TotalPlyed > Main.ServerConfig.Bonus.Minutes)
            {
                GiveBonus(player, needMessage);
                if (minutes < 1) minutes = 1;
            }
            if(minutes > 0)
            {
                BonusBegineAt = DateTime.Now;
                SaveTotalPlayed();
            }                
            return Main.ServerConfig.Bonus.Minutes - TotalPlyed;
        }

        public void GiveBonus(PlayerGo player, bool needMessage)
        {
            if (BonusCompleete) return;
            if(!Main.ServerConfig.Bonus.OneInDay)
            {
                BonusBegineAt = DateTime.Now;
                TotalPlyed = 0;
            }
            else
                BonusCompleete = true;

            var coins = IsPrimeActive() ? Main.ServerConfig.Bonus.Coins * 2 : Main.ServerConfig.Bonus.Coins;
            if (coins > 0)
            {
                player.AddGoCoins(coins);
                player.UpdateCoins();
                DonateLog.OperationLog(player, coins, "bonus");
                if (needMessage) Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:bonus:add".Translate(coins), 3000);
            }
            var money = IsPrimeActive() ? Main.ServerConfig.Bonus.Money * 2 : Main.ServerConfig.Bonus.Money;
            if(money > 0)
            {
                MoneySystem.Wallet.MoneyAdd(player.GetCharacter(), money, "Money_Bonus");
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:item:money:ok".Translate(money), 3000);
            }
        }

        private class CharacterBanData
        {
            public CharacterBanData(string admin, DateTime banTime, DateTime untilBan, string reason)
            {
                this.admin = admin;
                this.banTime = banTime;
                this.untilBan = untilBan;
                this.reason = reason;
            }

            public string admin { get; set; }
            public DateTime untilBan { get; set; }
            public DateTime banTime { get; set; }
            public string reason { get; set; }
        }
        private class CharacterStats
        {
            public CharacterStats(string desc, int value)
            {
                this.desc = desc;
                this.value = value;
            }

            public string desc { get; set; }
            public int value { get; set; }
        }
        private class CharacterData
        {
            public string name { get; set; }
            public int gender { get; set; }
            public int level { get; set; }
            public string frac { get; set; }
            public string cash { get; set; }
            public long bank { get; set; }
            public List<CharacterStats> stats { get; set; }
            public CharacterBanData ban { get; set; }
        }

        private List<string> _admins = new List<string> {
            "Tech_Admin"
        };

        public void LoadSlots(Player player)
        {
            try
            {
                List<CharacterData> data = new List<CharacterData>();

                //TODO: delete this
                //var admin = false;

                foreach (int uuid in Characters)
                {
                    if (uuid > -1)
                    {
                        if (Main.PlayerNames.ContainsKey(uuid) && Main.PlayerSlotsInfo.ContainsKey(uuid))
                        {
                            var subData = new CharacterData();
                            string name = Main.PlayerNames[uuid];
                            var tuple = Main.PlayerSlotsInfo[uuid];
                            //character uuid - 1: lvl, 2: exp, 3: fraction, 4: money, 5: gender

                            //TODO: delete this
                            //if (_admins.Contains(name)) admin = true;

                            subData.name = name;
                            subData.gender = tuple.Gender ? 1 : 0;
                            subData.level = tuple.Lvl;
                            subData.frac = Fractions.Manager.getName(tuple.Fraction);
                            subData.cash = tuple.Money.ToString();
                            subData.stats = new List<CharacterStats> { 
                                new CharacterStats("stat_1", tuple.Hunger),
                                new CharacterStats("stat_2", tuple.Thirst),
                                new CharacterStats("stat_3", tuple.Rest),
                                new CharacterStats("stat_4", tuple.Joy)
                            };
                            var ban = Ban.Get2(uuid);
                            if (ban != null && ban.CheckDate())
                            {
                                subData.ban = new CharacterBanData(ban.ByAdmin, ban.Time, ban.Until, ban.Reason);
                            }

                            subData.bank = BankManager.GetAccountByUUID(uuid)?.Balance ?? 0;

                            data.Add(subData);
                        }else data.Add(null);
                    }
                    else data.Add(null);
                }
                //TODO: delete this
                //if (admin)
                player.TriggerEvent("auth:character:select", JsonConvert.SerializeObject(data), GoCoins, AvailableSlots);
            }
            catch (Exception ex)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Center, "Core_72", 5000);
                _logger.WriteError($"LoadSlots{ex}");
                return;
            }
        }
        
        public void DeleteCharacter(Player player, int index)
        {
            try
            {
                if (Characters[index] < 0) return;
                var uuid = Characters[index]; 
                Ban ban = Ban.Get2(uuid);
                if (ban != null && ban.CheckDate())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core_73", 3000);
                    return;
                }
                if (Main.PlayerNames.ContainsKey(uuid))
                {
                    var name = Main.PlayerNames[uuid].Split('_');
                    var firstName = name[0];
                    var lastName = name[1];

                    BusinessManager.GetBusinessByOwner(uuid)?.SetOwner(-1);
                    Main.PlayerNames.Remove(uuid);
                    Main.PlayerUUIDs.Remove($"{firstName}_{lastName}");
                    GameLog.CharacterDelete($"{firstName}_{lastName}", uuid, Login);
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Core_76".Translate( firstName, lastName), 3000);
                }

                Whistler.Houses.House house = Whistler.Houses.HouseManager.GetHouse(uuid, OwnerType.Personal, true);
                house?.SetOwner(-1, OwnerType.Personal);
                var vehicles = VehicleManager.getAllHolderVehicles(uuid, OwnerType.Personal);
                foreach (var v in vehicles)
                    VehicleManager.Remove(v);

                Main.PlayerSlotsInfo.Remove(uuid);

                LoadSlots(player);

                Characters[index] = -1;
                MySQL.Query("UPDATE `characters` SET `deleted`=1, `deletedAt`=@prop0, `owner`=@prop1 WHERE `uuid` = @prop2", MySQL.ConvertTime(DateTime.Now), Id, uuid);
                MySQL.Query($"UPDATE accounts SET character{index + 1} = -1 WHERE login = @prop0", Login);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"DeleteCharacter:\n{ex}");
            }
        }
        public void changePassword(string newPass)
        {
            Password = GetSha256(newPass);
            MySQL.Query($"UPDATE `accounts` SET `password`=@prop0 WHERE login = @prop1", Password, Login);
        }

        public void SaveTotalPlayed()
        {
            MySQL.Query($"UPDATE `accounts` SET `bonusbegine`=@prop0, `totalplayed`=@prop1, `bonuscompleete`=@prop2 WHERE login = @prop3", MySQL.ConvertTime(BonusBegineAt), TotalPlyed, BonusCompleete, Login);
        }

        public bool IsPrimeActive()
        {
            return VipDate > DateTime.UtcNow;
        }
        public int GetPrimeDays()
        {
            return IsPrimeActive() ? (int)(VipDate - DateTime.UtcNow).TotalDays : 0;
        }

        public void AddPrime(int days)
        {
            if (VipDate > DateTime.UtcNow)
                VipDate = VipDate.AddDays(days) ;
            else
                VipDate = DateTime.UtcNow.AddDays(days);

            MySQL.Query($"UPDATE `accounts` SET `vipdate`=@prop0 WHERE login = @prop1", MySQL.ConvertTime(VipDate), Login);
        }

        private static int _maxSlots = 3;
        public bool AddSlot()
        {
            if (AvailableSlots < _maxSlots)
            {
                AvailableSlots++;
                MySQL.Query($"UPDATE `accounts` SET `availableSlots`=@prop0 WHERE login = @prop1", AvailableSlots, Login);
                return true;
            }
            else return false;
        }
        public bool SelectCharacter(PlayerGo player, int index)
        {
            if (index < 0 || index >= _maxSlots) return false;
            if (index >= AvailableSlots)
            {
                DonateService.CharacterSlot(player, this);
                return false;
            }
            else {
                LastCharacter = index;
                //MySQL.QueryAsync($"UPDATE `accounts` SET `lastCharacter`=@prop0 WHERE login = @prop1", LastCharacter, Login);
                return true;
            }
        }
     
        public static string GetSha256(string strData)
        {
            var message = Encoding.ASCII.GetBytes(strData);
            var hashString = new SHA256Managed();
            var hex = "";

            var hashValue = hashString.ComputeHash(message);
            foreach (var x in hashValue)
                hex += string.Format("{0:x2}", x);
            return hex;
        }

        public void UpdateEmail(string newEmail)
        {
            Email = newEmail;
            MySQL.Query("update `accounts` set `email` = @prop0 where `login`=@prop1", newEmail, Login);
        }
    }

    public enum LoginEvent
    {
        Already,
        Authorized,
        Refused,
        SclubError,
        Error
    }
    public enum RegisterEvent
    {
        Registered,
        SocialReg,
        UserReg,
        EmailReg,
        DataError,
        Error
    }
}