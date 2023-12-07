using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using AutoMapper.Internal;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Domain.Phone.Bank;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Houses;
using Whistler.MoneySystem.DTO;
using Whistler.MoneySystem.Interface;
using Whistler.NewDonateShop;
using Whistler.Phone.Bank.Dtos;

namespace Whistler.MoneySystem.Models
{
    [Table("efcore_bank_account")]
    class CheckingAccount : IBankAccount
    {
        [Key]
        public int ID { get; set; }
        public int UUID { get; set; }
        public int BankId { get; set; }
        public long Number { get; set; }
        public TypeBankAccount OwnerType { get; set; }
        public long Balance { get; set; }
        public long IMoneyBalance => Balance;

        public TypeMoneyAcc ITypeMoneyAcc { get { return TypeMoneyAcc.BankAccount; } }
        public int IOwnerID => ID;
        public TypeBankAccount ITypeBank => OwnerType;

        private ExtPlayer _subscriber = null;
        [NotMapped]
        private int _max = -1;
        [NotMapped]
        private static Dictionary<TypeBankAccount, string> _setData = new Dictionary<TypeBankAccount, string>
            {
                { TypeBankAccount.Player, "smartphone/bankPage/setBalance" },
                { TypeBankAccount.House, "smartphone/bankPage/setHomeBalance" },
                { TypeBankAccount.BusinessNalog, "smartphone/bankPage/setBusinessBalance" },
                { TypeBankAccount.Phone, "smartphone/setPhoneBalance" },
            };

        [NotMapped]
        private static Dictionary<TypeBankAccount, string> _setMaxData = new Dictionary<TypeBankAccount, string>
            {
                {TypeBankAccount.House, "setMaxHomeBalance" },
                {TypeBankAccount.BusinessNalog, "setMaxBusinessBalance" },
            };

        public CheckingAccount()
        {

        }
        public CheckingAccount(TypeBankAccount ownerType, int bankId, long balance = 0, int uuid = 0)
        {
            OwnerType = ownerType;
            BankId = bankId;
            Balance = balance;
            UUID = uuid;
        }

        public int MaxBalance(int price)
        {
            if (_max < 0) _max = GetTaxInOneDay(price);
            return _max * 14;
        }

        private int GetTaxInOneDay(int price)
        {
            return OwnerType switch
            {
                TypeBankAccount.House => Convert.ToInt32(price * MoneyConstants.PayTaxCoeffHouseForDay),
                TypeBankAccount.BusinessNalog => Convert.ToInt32(price * MoneyConstants.PayTaxCoeffBusinessForDay),
                _ => price
            };
        }

        public bool MoneyAdd(int amount)
        {
            if (amount <= 0) return false;
            return SetMoney(Balance + amount);
        }

        public bool MoneySub(int amount, bool limit = false)
        {
            if (amount <= 0) return false;
            return SetMoney(Balance - amount);
        }

        public bool SetMoney(long money)
        {
            if (money < 0)
                return false;
            Balance = money;
            SendData();
            return true;
        }

        private void SendData()
        {
            if (!_subscriber.IsLogged()) return;

            _subscriber.TriggerCefEvent($"{_setData.GetOrDefault(OwnerType)}", Balance);
        }

        public void SendTransaction(TransactionModel transaction)
        {
            if (OwnerType != TypeBankAccount.Player && OwnerType != TypeBankAccount.BusinessNalog) return;
            if (_subscriber == null) return;
            if (!_subscriber.IsLogged()) return;

            TransactionDTO transactionData = new TransactionDTO(transaction, ID);
            int transactionPerspectiveId = transaction.To == ID ? transaction.From : transaction.To;
            if (OwnerType == TypeBankAccount.BusinessNalog)
            {
                if (transaction.FromType == (int)TypeBankAccount.Business) _subscriber.TriggerCefEvent("bank/business/addTransaction", JsonConvert.SerializeObject(new TransactionDTO(transaction, transactionPerspectiveId)));
                return;
            }

            switch (transactionData.recipientType)
            {
                case (int)TypeBankAccount.Business:
                    _subscriber.TriggerCefEvent("bank/business/addTransaction", JsonConvert.SerializeObject(new TransactionDTO(transaction, transactionPerspectiveId)));
                    break;
                case (int)TypeBankAccount.Family:
                    _subscriber.TriggerCefEvent("bank/family/addTransaction", JsonConvert.SerializeObject(new TransactionDTO(transaction, transactionPerspectiveId)));
                    break;
                case (int)TypeBankAccount.Fraction:
                    _subscriber.TriggerCefEvent("bank/organization/addTransaction", JsonConvert.SerializeObject(new TransactionDTO(transaction, transactionPerspectiveId)));
                    break;
                default:
                    break;
            }
            string serializedTransaction = JsonConvert.SerializeObject(transactionData);
            _subscriber.TriggerCefEvent("bank/addTransaction", serializedTransaction);
            _subscriber.TriggerCefEvent("smartphone/bankPage/addHistoryItems", serializedTransaction);
        }

        public void Subscribe(ExtPlayer player, int price = 0)
        {
            UnSubscribe();
            _subscriber = player;
            _subscriber.TriggerCefEvent($"{_setData.GetOrDefault(OwnerType)}", Balance);
            switch (OwnerType)
            {
                case TypeBankAccount.Player:
                    _subscriber.TriggerCefEvent($"smartphone/bankPage/setBankAccount", ID);
                    break;
                case TypeBankAccount.House:
                case TypeBankAccount.BusinessNalog:
                    _subscriber.TriggerCefEvent($"smartphone/bankPage/{_setMaxData.GetOrDefault(OwnerType)}", GetTaxInOneDay(price));
                    break;
            }
        }

        public void UnSubscribe()
        {
            if (_subscriber.IsLogged())
            {
                switch (OwnerType)
                {
                    case TypeBankAccount.House:
                    case TypeBankAccount.BusinessNalog:
                        _subscriber.TriggerCefEvent($"smartphone/bankPage/{_setMaxData.GetOrDefault(OwnerType)}", -1);
                        break;
                }
            }
            _subscriber = null;
        }
    }
}
