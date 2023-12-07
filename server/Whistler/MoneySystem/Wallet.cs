using System;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.Phone.Bank;
using Whistler.SDK;
using Whistler.MoneySystem.Models;
using Whistler.Domain.Phone.Bank;
using Whistler.MoneySystem.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Whistler.MoneySystem
{
    class Wallet : Script
    {
        public static bool TryChange(IMoneyOwner from, int amount)
        {
            return (from != null) && (from.IMoneyBalance + amount >= 0);
        }

        public static bool MoneyAdd(IMoneyOwner to, int amount, string desc, bool limit = false) => TransferMoney(MoneyManager.ServerMoney, to, amount, 0, desc, limit);

        public static bool MoneySub(IMoneyOwner from, int amount, string desc, bool limit = false) => TransferMoney(from, MoneyManager.ServerMoney, amount, 0, desc, limit);
        
        public static bool TransferMoney(IMoneyOwner from, IMoneyOwner to, int amount, int tax, string desc = null, bool limit = false)
        {
            if (from == null || to == null) return false;
            if (!from.MoneySub(amount, limit))
                return false;
            to.MoneyAdd(amount - tax);
            if (desc != null)
            {
                BankManager.SaveBankTransact(from as IBankAccount, to as IBankAccount, amount, tax, desc);
                GameLog.MoneyNew(from.ITypeMoneyAcc, from.IOwnerID, to.ITypeMoneyAcc, to.IOwnerID, amount - tax, tax, desc);
            }
            return true;
        }
        public static bool TransferMoney(IMoneyOwner from, List<(IMoneyOwner, int)> toList, string desc = null, bool limit = false)
        {
            if (from == null || toList == null || toList.Count == 0) return false;
            if (!from.MoneySub(toList.Sum(item => item.Item2), limit))
                return false;
            foreach (var to in toList)
            {
                if (to.Item1?.MoneyAdd(to.Item2) ?? false)
                {
                    if (desc != null)
                    {
                        BankManager.SaveBankTransact(from as IBankAccount, to.Item1 as IBankAccount, to.Item2, 0, desc);
                        GameLog.MoneyNew(from.ITypeMoneyAcc, from.IOwnerID, to.Item1.ITypeMoneyAcc, to.Item1.IOwnerID, to.Item2, 0, desc);
                    }
                }
            }
            return true;
        }

        public static bool SetBankMoney(int accountID, int amount)
        {
            return BankManager.GetAccount(accountID)?.SetMoney(amount) ?? false;
        }
        public static bool SetBankMoneyByUUID(int uuid, int amount)
        {
            return BankManager.GetAccountByUUID(uuid)?.SetMoney(amount) ?? false;
        }
    }
}