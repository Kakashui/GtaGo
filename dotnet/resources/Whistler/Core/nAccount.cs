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

        // public void InitBonus()
        // {
        //     if (!Main.ServerConfig.Bonus.OneInDay)
        //     {
        //         BonusCompleete = false;
        //     }
        //     else
        //     {
        //         if (BonusBegineAt.DayOfYear != DateTime.Now.DayOfYear)
        //         {
        //             TotalPlyed = 0;
        //             BonusCompleete = false;
        //         }
        //     }
        //     BonusBegineAt = DateTime.Now;
        // }

        // public int CheckBonus(ExtPlayer player, bool needMessage = true)
        // {
        //     if (BonusCompleete) return -1;
        //     var minutes = (int)(DateTime.Now - BonusBegineAt).TotalMinutes;
        //     TotalPlyed += minutes;
        //     if (TotalPlyed > Main.ServerConfig.Bonus.Minutes)
        //     {
        //         GiveBonus(player, needMessage);
        //         if (minutes < 1) minutes = 1;
        //     }
        //     if(minutes > 0)
        //     {
        //         BonusBegineAt = DateTime.Now;
        //         SaveTotalPlayed();
        //     }                
        //     return Main.ServerConfig.Bonus.Minutes - TotalPlyed;
        // }

        // public void GiveBonus(ExtPlayer player, bool needMessage)
        // {
        //     if (BonusCompleete) return;
        //     if(!Main.ServerConfig.Bonus.OneInDay)
        //     {
        //         BonusBegineAt = DateTime.Now;
        //         TotalPlyed = 0;
        //     }
        //     else
        //         BonusCompleete = true;

        //     var coins = IsPrimeActive() ? Main.ServerConfig.Bonus.Coins * 2 : Main.ServerConfig.Bonus.Coins;
        //     if (coins > 0)
        //     {
        //         player.AddMCoins(coins);
        //         player.UpdateCoins();
        //         DonateLog.OperationLog(player, coins, "bonus");
        //         if (needMessage) Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы получили бонус в размере {coins} Donate Coins.", 3000);
        //     }
        //     var money = IsPrimeActive() ? Main.ServerConfig.Bonus.Money * 2 : Main.ServerConfig.Bonus.Money;
        //     if(money > 0)
        //     {
        //         MoneySystem.Wallet.MoneyAdd(player.Character, money, $"Бонускод ");
        //         Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вам добавлено {money} $", 3000);
        //     }
        // }

        private class CharacterBanData
        {
            public CharacterBanData(string admin, DateTime banTime, DateTime untilBan, string reason)
            {
                this.Administrator = admin;
                this.BannedAt = banTime;
                this.BannedUntil = untilBan;
                this.Reason = reason;
            }

            public string Administrator { get; set; }
            public DateTime BannedUntil { get; set; }
            public DateTime BannedAt { get; set; }
            public string Reason { get; set; }
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

        private class CharacterSpawns
        {
            public CharacterSpawns(string key, string name, string subname, int x, int y)
            {
                this.key = key;
                this.name = name;
                this.subname = subname;
                this.x = x;
                this.y = y;
            }

            public string key { get; set; }
            public string name { get; set; }
            public string subname { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            // public int value { get; set; }
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
            public List<CharacterSpawns> spawnPoints { get; set; }
            public CharacterBanData ban { get; set; }
        }

        public void LoadSlots(ExtPlayer player)
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
                            //ExtPlayer character = 
                            //character uuid - 1: lvl, 2: exp, 3: fraction, 4: money, 5: gender

                            //TODO: delete this
                            //if (_admins.Contains(name)) admin = true;

                            subData.name = name;
                            subData.gender = tuple.Gender ? 1 : 0;
                            subData.level = tuple.Lvl;
                            subData.frac = Fractions.Manager.getName(tuple.Fraction);
                            subData.cash = tuple.Money.ToString();
                            // subData.stats = new List<CharacterStats> { 
                            //     new CharacterStats("Голод", tuple.Hunger),
                            //     new CharacterStats("Жажда", tuple.Thirst),
                            //     new CharacterStats("Усталость", tuple.Rest),
                            //     new CharacterStats("Счастье", tuple.Joy)
                            // };
                            var ban = Ban.Get2(uuid);
                            if (ban != null && ban.CheckDate())
                            {
                                subData.ban = new CharacterBanData(ban.ByAdmin, ban.Time, ban.Until, ban.Reason);
                            }

                            subData.bank = BankManager.GetAccountByUUID(uuid)?.Balance ?? 0;

                            // subData.spawnPoints = new List<CharacterSpawns> { 
                            //     new CharacterSpawns("s1", "Тест", "Тест2", 1000, 1000),
                            //     new CharacterSpawns("s2", "Тест2", "Тест22", 1500, 1000),
                            //     new CharacterSpawns("s3", "Тест3", "Тест23", 1000, 1500),
                            //     new CharacterSpawns("s4", "Тест4", "Тест24", 2000, 2000)
                            // };

                            data.Add(subData);
                        }else data.Add(null);
                    }
                    else data.Add(null);
                }
                //TODO: delete this
                //if (admin) , GoCoins, AvailableSlots
                SafeTrigger.ClientEvent(player,"auth:character:select", JsonConvert.SerializeObject(data));
            }
            catch (Exception ex)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Center, "К сожалению, невозможно получить данные о персонаже с номером паспорта, обратитесь в тех.раздел на форуме.", 5000);
                _logger.WriteError($"LoadSlots{ex}");
                return;
            }
        }
        
        public void DeleteCharacter(ExtPlayer player, int index)
        {
            try
            {
                if (Characters[index] < 0) return;
                var uuid = Characters[index]; 
                Ban ban = Ban.Get2(uuid);
                if (ban != null && ban.CheckDate())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Невозможно удалить персонажа, который находится в бане", 3000);
                    return;
                }
                if (Main.PlayerNames.ContainsKey(uuid))
                {
                    var name = Main.PlayerNames[uuid].Split('_');
                    var firstName = name[0];
                    var lastName = name[1];

                    BusinessManager.GetBusinessByOwner(uuid)?.SetOwner(-1);
                    Main.UUIDs.Remove(uuid);
                    Main.PlayerNames.Remove(uuid);
                    Main.PlayerUUIDs.Remove($"{firstName}_{lastName}");
                    GameLog.CharacterDelete($"{firstName}_{lastName}", uuid, Login);
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Персонаж {firstName} {lastName} успешно удален", 3000);
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
            MySQL.Query("UPDATE `accounts` SET `password`=@prop0 WHERE login = @prop1", Password, Login);
        }

        public void SaveTotalPlayed()
        {
            MySQL.Query("UPDATE `accounts` SET `bonusbegine`=@prop0, `totalplayed`=@prop1, `bonuscompleete`=@prop2 WHERE login = @prop3", MySQL.ConvertTime(BonusBegineAt), TotalPlyed, BonusCompleete, Login);
        }

        private static int _maxSlots = 3;
        public bool SelectCharacter(ExtPlayer player, int index)
        {
            if (index < 0 || index >= _maxSlots) return false;
            if (index >= Characters.Count) return false;
            if (Characters[index] == -2)
            {
                DialogUI.Open(player, $"Вы действительно хотите приобрести слот за {DonateService._slotConfig.Price}?", new List<DialogUI.ButtonSetting>
                {
                    new DialogUI.ButtonSetting
                    {
                        Name = "Подтвердить",
                        Icon = null,
                        Action = (p) =>
                        {
                            DonateService.CharacterSlot(p, this, index);
                        }
                    },
                    new DialogUI.ButtonSetting
                    {
                        Name = "Отменить",
                        Icon = null,
                        Action = { }
                    }
                });
                return false;
            }
            else 
            {
                LastCharacter = index;
                //MySQL.QueryAsync("UPDATE `accounts` SET `lastCharacter`=@prop0 WHERE login = @prop1", LastCharacter, Login);
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