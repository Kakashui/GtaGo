using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Whistler.Common.Interfaces;
using Whistler.Core;
using Whistler.Core.Character;
using Whistler.Entities;
using Whistler.Fractions;
using Whistler.Helpers;
using Whistler.Houses;
using Whistler.MoneySystem.DTO;
using Whistler.MoneySystem.Interface;
using Whistler.MoneySystem.Models;
using Whistler.MoneySystem.Settings;
using Whistler.Phone.Bank;
using Whistler.SDK;

namespace Whistler.MoneySystem
{
    class BankManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(BankManager));
        public static HeadBank HeadBank { get; set; }
        public BankManager()
        {
            HeadBank = new HeadBank(1);
        }

        public static CheckingAccount CreateAccount(TypeBankAccount type, long money = 0, int uuid = 0)
        {
            if (uuid > 0)
            {
                var acc = GetAccountByUUID(uuid);
                if (acc != null)
                    return acc;
            }
            var account = new CheckingAccount(type, HeadBank.ID, money, uuid);
            using (var context = EFCore.DBManager.TemporaryContext)
            {
                context.CheckingAccounts.Add(account);
                context.SaveChanges();
                // {HeadBank.ID} {(int)(account.OwnerType):d2}{HeadBank.ID:d2}{account.ID}
                // account.Number = Convert.ToInt64($"4{HeadBank.ID:d4}{(int)(account.OwnerType):d2}{account.ID:d7}");
                account.Number = Convert.ToInt64($"{HeadBank.ID:d2}{(int)(account.OwnerType):d2}{account.ID}");
                context.SaveChanges();
                EFCore.DBManager.GlobalContext.CheckingAccounts.Attach(account);
            }
            return account;
        }

        public static void RemoveAccount(CheckingAccount account)
        {
            try
            {
                EFCore.DBManager.GlobalContext.CheckingAccounts.Remove(account);
            }
            catch (Exception e)
            {
                _logger.WriteError($"RemoveAccount:\n{e}");
            }
        }

        public static CheckingAccount GetAccount(int id)
        {
            try
            {
                return EFCore.DBManager.GlobalContext.CheckingAccounts.Find(id);
            }
            catch (Exception e)
            {
                _logger.WriteError($"GetAccount:\n{e}");
                return null;
            }
        }
        public static CheckingAccount GetAccountByUUID(int uuid)
        {
            try
            {
                return EFCore.DBManager.GlobalContext.CheckingAccounts.Where(acc => acc.UUID == uuid).ToList().LastOrDefault();
            }
            catch (Exception e)
            {
                _logger.WriteError($"GetAccountByUUID:\n{e}");
                return null;
            }
        }
        public static CheckingAccount GetAccountByNumber(long number)
        {
            try
            {
                return EFCore.DBManager.GlobalContext.CheckingAccounts.Where(acc => acc.Number == number).ToList().LastOrDefault();
            }
            catch (Exception e)
            {
                _logger.WriteError($"GetAccountByNumber:\n{e}");
                return null;
            }
        }

        public static List<CheckingAccount> GetAccountsByPredicate(Func<CheckingAccount, bool> predicate)
        {
            try
            {
                return EFCore.DBManager.GlobalContext.CheckingAccounts.Where(item => predicate(item)).ToList();
            }
            catch (Exception e)
            {
                _logger.WriteError($"GetAccountsByPredicate:\n{e}");
                return new List<CheckingAccount>();
            }
        }
        public static void SaveBankTransact(IBankAccount from, IBankAccount to, int amount, int tax, string comment)
        {
            try
            {
                if (from == null && to == null) return;
                TransactionModel transaction = new TransactionModel(from, to, amount, tax, comment);

                if (from is CheckingAccount checkingFromAccount) checkingFromAccount.SendTransaction(transaction);
                if (to is CheckingAccount checkingToAccount) checkingToAccount.SendTransaction(transaction);

                using (var context = EFCore.DBManager.TemporaryContext)
                {
                    context.Transactions.Add(transaction);
                    context.SaveChanges();
                    EFCore.DBManager.GlobalContext.Transactions.Attach(transaction);
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() => _logger.WriteError($"SaveBankTransact: {e.ToString()}"));
            }
        }

        public static List<TransactionDTO> GetTransactionHistory(IBankAccount bank, int count = 25)
        {
            try
            {
                return EFCore.DBManager.GlobalContext.Transactions
                    .Where(item => item.Date > DateTime.Now.AddDays(-2) && (item.From == bank.IOwnerID && item.FromType == (int)bank.ITypeBank || item.To == bank.IOwnerID && item.ToType == (int)bank.ITypeBank))
                    .ToList()
                    .TakeLast(count)
                    .Select(item => new TransactionDTO(item, bank.IOwnerID))
                    .ToList();
            }
            catch (Exception e)
            {
                _logger.WriteError($"LoadTransactionHistory: {e.ToString()}");
                return new List<TransactionDTO>();
            }
        }

        public static List<OrgPaymentDTO> GetMemberPayments(IEnumerable<IMember> members, IMoneyOwner from, string comment, int days)
        {
            try
            {
                var uuids = members.Select(item => item.PlayerUUID);
                var accounts = EFCore.DBManager.GlobalContext.CheckingAccounts.Where(account => account.OwnerType == TypeBankAccount.Player && uuids.Contains(account.UUID)).ToDictionary(item => item.ID);
                var payments = EFCore.DBManager.GlobalContext.Transactions.Where(item => item.Date > DateTime.Now.AddDays(-days) && item.From == from.IOwnerID && item.ToType == (int)TypeBankAccount.Player && item.Comment == comment).ToList();
                return accounts.Select(item => new OrgPaymentDTO(item.Value.UUID, Main.PlayerNames.GetValueOrDefault(item.Value.UUID), payments.LastOrDefault(payment => payment.To == item.Key))).ToList();
            }
            catch (Exception e)
            {
                _logger.WriteError($"GetMemberPayments: {e.ToString()}");
                return new List<OrgPaymentDTO>();
            }
        }

        public static void PayPhones()
        {
            EFCore.DBManager.GlobalContext.CheckingAccounts
                .Where(item => item.OwnerType == TypeBankAccount.Phone)
                .ToList()
                .ForEach(item =>
                {
                    Wallet.MoneySub(item, BankSettings.PhonePayed, "Оплата за телефон");
                });
        }

        [Command("fixdoubleaccount")]
        public static void FixDoubleAccount(ExtPlayer player)
        {
            if (player.Character.AdminLVL < 10) return;
            var result = MySQL.QueryRead(
                "SELECT acc.ID " +
                "FROM `efcore_bank_account` acc " +
                "   LEFT JOIN `characters` ch " +
                "       ON ch.banknew = acc.ID " +
                "WHERE acc.OwnerType = 1 AND ch.uuid is null");
            if (result != null && result.Rows.Count > 0)
            {
                List<int> listAccs = new List<int>();
                foreach (DataRow row in result.Rows)
                {
                    listAccs.Add(Convert.ToInt32(row["ID"]));
                }
                EFCore.DBManager.GlobalContext.CheckingAccounts.RemoveRange(EFCore.DBManager.GlobalContext.CheckingAccounts.Where(item => listAccs.Contains(item.ID)).ToList());
            }
        }
    }
}
