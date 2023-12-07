using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Common;
using Whistler.Common.Interfaces;
using Whistler.Core;
using Whistler.Domain.Phone.Bank;
using Whistler.Entities;
using Whistler.Fractions.Configurations;
using Whistler.Fractions.PDA;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.MoneySystem.DTO;
using Whistler.MoneySystem.Interface;
using Whistler.SDK;

namespace Whistler.Fractions.Models
{
    internal class Fraction : IBankAccount, IOrganization
    {

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Fraction));
        public int Id { get; set; }
        public int Money { get; private set; }
        public int FuelLimit { get; set; }
        public int FuelLeft { get; set; }
        public int LastHour { get; set; }
        public int LastDay { get; set; }
        public int MoneyLimit { get; set; }


        public Dictionary<int, FractionRank> Ranks = new Dictionary<int, FractionRank>();
        public Dictionary<string, int> Commands = new Dictionary<string, int>();


        public Dictionary<int, FractionMember> Members { get; set; }
        public Dictionary<int, ExtPlayer> OnlineMembers { get; set; }
        public int MoneyForEnterprise = 0;
        public FractionConfig Configuration => Configs.GetConfigOrDefault(Id);

        public long IMoneyBalance => Money;
        public TypeMoneyAcc ITypeMoneyAcc => TypeMoneyAcc.Fraction;
        public int IOwnerID => Id;
        public TypeBankAccount ITypeBank => TypeBankAccount.Fraction;
        OrganizationType IOrganization.TypeOrganization => OrganizationType.Fraction;
        public OrgActivityType OrgActiveType => Configuration.TypeFraction;
        public List<OrgPaymentDTO> PaymentHystory { get; set; }
        public Fraction(int id, int money, int fuelLimit, int fuelLeft)
        {
            Id = id;
            Money = money;
            FuelLeft = fuelLeft;
            FuelLimit = fuelLimit;
            UpdateInfo();
            Main.Payday += UpdateInfo;
            MoneyLimit = 0;
            OnlineMembers = new Dictionary<int, ExtPlayer>();

            var result = MySQL.QueryRead("SELECT * FROM `fractionranks` WHERE `fraction` = @prop0", Id);
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow Row in result.Rows)
                {
                    var rank = Convert.ToInt32(Row["rank"]);
                    var payday = Convert.ToInt32(Row["payday"]);
                    var name = Row["name"].ToString();

                    Ranks.Add(rank, new FractionRank(name, payday));
                }
            }
            int maxRank = Ranks.Max(item => item.Key);
            Members = new Dictionary<int, FractionMember>();
             result = MySQL.QueryRead(
                "SELECT `uuid`, `fractionlvl`" +
                "FROM `characters` " +
                "WHERE `fraction` = @prop0 and `deleted` = false",
                Id);
            if (result != null)
            {
                foreach (DataRow Row in result.Rows)
                {
                    int uuid = Convert.ToInt32(Row["uuid"]);
                    int fractionLevel = Convert.ToInt32(Row["fractionlvl"]);
                    var member = new FractionMember(uuid, fractionLevel);
                    if (member.Rank > maxRank)
                        member.ChangeRank(1);
                    Members.Add(uuid, member);
                }
            }

            result = MySQL.QueryRead("SELECT * FROM `fractionaccess` WHERE `fraction` = @prop0", Id);
            if (result != null || result.Rows.Count > 0)
                Commands = JsonConvert.DeserializeObject<Dictionary<string, int>>(result.Rows[0]["commands"].ToString());
        }

        public bool ChangeRank(int uuid, int newRank)
        {
            if (!Members.ContainsKey(uuid))
                return false;
            if (!Ranks.ContainsKey(newRank))
                return false;
            Members[uuid].ChangeRank( newRank);
            return true;
        }


        public void ConnectedMember(int uuid, ExtPlayer player)
        {
            if (OnlineMembers.ContainsKey(uuid))
                OnlineMembers[uuid] = player;
            else
                OnlineMembers.Add(uuid, player);
        }

        public void DisconnectedMember(int uuid)
        {
            if (OnlineMembers.ContainsKey(uuid))
                OnlineMembers.Remove(uuid);
        }

        public void NewMember(int uuid, int rank)
        {
            if (Members.ContainsKey(uuid))
                return;
            Members.Add(uuid, new FractionMember(this, uuid, rank));
        }
        public bool DeleteMember(int uuid)
        {
            if (!Members.ContainsKey(uuid))
                return false;
            Members.Remove(uuid);
            DisconnectedMember(uuid);
            SkinManager.TakePlayerCostumes(uuid, Inventory.Enums.ClothesOwn.Fraction);
            
            MySQL.Query("UPDATE characters SET fraction = 0, fractionlvl = 0 WHERE uuid = @prop0", uuid);
            ExtPlayer target = Trigger.GetPlayerByUuid(uuid);

            if (target != null)
            {
                target.Character.FractionID = 0;
                target.Character.FractionLVL = 0;
                target.Character.OnDuty = false;
                FractionSubscribeSystem.UnSubscribe(target);
                SafeTrigger.SetSharedData(target, "fraction", 0);
                GangsCapture.SetMemberInGangTeam(target, 3);
                PoliceCalls.SubHelperForAllCalls(target, true);
                PersonalDigitalAssistant.OnPlayerRemoveFromFraction(target);
                LSNewsManager.OnPlayerRemoveFromFraction(target);
                Manager.UpdateFracData(target);
                MainMenu.SendStats(target);
            }
            return true;
        }

        public FractionRank GetRank(ExtPlayer player)
        {
            int uuid = player.Character?.UUID ?? -1;
            return GetRank(uuid);
        }

        public FractionRank GetRank(int uuid)
        {
            if (!Members.ContainsKey(uuid))
                return null;
            if (!Ranks.ContainsKey(Members[uuid].Rank))
                Members[uuid].ChangeRank(1);
            return Ranks[Members[uuid].Rank];
        }

        public FractionRank GetRankForLevel(int fraclvl)
        {
            if (!Ranks.ContainsKey(fraclvl))
                return null;
            return Ranks[fraclvl];
        }

        public bool IsLeader(ExtPlayer player)
        {
            var uuid = player.Character?.UUID ?? -1;
            return IsLeader(uuid);
        }

        public bool IsLeader(int uuid)
        {
            if (!Members.ContainsKey(uuid))
                return false;
            return Ranks.Max(item => item.Key) == Members[uuid].Rank;
        }

        public bool IsLeaderOrSub(ExtPlayer player)
        {
            if (!player.Logged()) return false;

            return IsLeaderOrSub(player.Character.UUID);
        }

        public bool IsLeaderOrSub(int uuid)
        {
            if (uuid < 0) return false;
            if (!Members.ContainsKey(uuid)) return false;
            return Ranks.Max(item => item.Key) - 1 <= Members[uuid].Rank;
        }

        #region Money

        private bool ChangeMoney(int amount)
        {
            if (Money + amount < 0)
                return false;
            Money += amount;
            return true;
        }

        public bool MoneyAdd(int amount)
        {
            if (amount <= 0) return false;
            return ChangeMoney(amount);
        }

        public bool MoneySub(int amount, bool limit = false)
        {
            if (amount <= 0) return false;
            if (limit)
                MoneyLimit += amount;
            return ChangeMoney(-amount);
        }
        public async void UpdateInfo()
        {
            try
            {
                var result = await MySQL.QueryReadAsync($"SELECT SUM(`sum`) FROM `{GameLog.DB}`.`fractionmoney` WHERE `date` >  NOW() - INTERVAL 1 HOUR AND `fractionId`=@prop0", Id);
                if (result == null || result.Rows.Count == 0 || result.Rows[0].IsNull(0))
                    LastHour = 0;
                else
                    LastHour = Convert.ToInt32(result.Rows[0][0]);
                result = await MySQL.QueryReadAsync($"SELECT SUM(`sum`) FROM `{GameLog.DB}`.`fractionmoney` WHERE `date` >  NOW() - INTERVAL 1 DAY AND `fractionId`=@prop0", Id);
                if (result == null || result.Rows.Count == 0 || result.Rows[0].IsNull(0))
                    LastDay = 0;
                else
                    LastDay = Convert.ToInt32(result.Rows[0][0]);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"UpdateInfo:\n{ex}");
            }
        }
        #endregion

        public void Save()
        {

            MySQL.Query("UPDATE `fractions` SET `fuellimit` = @prop0, `fuelleft` = @prop1, `money` = @prop2 WHERE `id` = @prop3", FuelLimit, FuelLeft, Money, Id);
        }
        #region CommandAccess
        public bool AddCommand(string command, int level)
        {
            if (Commands.ContainsKey(command))
                return false;
            Commands.Add(command, level);
            SaveFracCommand();
            return true;
        }
        public bool RemoveCommand(string command)
        {
            if (!Commands.ContainsKey(command))
                return false;
            Commands.Remove(command);
            SaveFracCommand();
            return true;
        }
        public bool ChangeCommandAccess(string command, int level)
        {
            if (!Commands.ContainsKey(command))
                return false;
            Commands[command] = level;
            SaveFracCommand();
            return true;
        }

        public void SaveFracCommand()
        {
            MySQL.Query("UPDATE fractionaccess SET commands = @prop0 WHERE fraction = @prop1", JsonConvert.SerializeObject(Commands), Id);
        }

        #endregion
        string IOrganization.GetName()
        {
            return Configuration.Name;
        }
        public List<OrgPaymentDTO> GetMemberPayments()
        {
            if (PaymentHystory == null) PaymentHystory = BankManager.GetMemberPayments(Members.Values, this, "Премия (организация)", 30);
            return PaymentHystory;
        }
        public void UpdatePayments(OrgPaymentDTO payment)
        {
            if (PaymentHystory == null) PaymentHystory = BankManager.GetMemberPayments(Members.Values, this, "Премия (организация)", 30);
            OrgPaymentDTO existPayment = PaymentHystory.FirstOrDefault(item => item.uuid == payment.uuid);
            if (existPayment != null) PaymentHystory.Remove(existPayment);
            PaymentHystory.Add(payment);
        }
    }
}
