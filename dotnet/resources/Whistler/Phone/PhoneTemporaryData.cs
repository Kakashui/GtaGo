using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Domain.Phone.Contacts;
using Whistler.Domain.Phone.Messenger;
using Whistler.MoneySystem;
using Whistler.MoneySystem.Models;

namespace Whistler.Phone
{
    public class PhoneTemporaryData
    {
        public Domain.Phone.Phone Phone { get; set; }
        internal CheckingAccount GetPhoneBankAccount()
        {
            if (Phone == null || Phone.SimCard == null) return null;

            if (Phone.SimCard.BankNumber > 0)  return BankManager.GetAccount(Phone.SimCard.BankNumber);

            Phone.SimCard.BankNumber = PhoneLoader.CreateBankNumber(Phone.SimCard.Id);
            if (Phone.SimCard.BankNumber > 0) return BankManager.GetAccount(Phone.SimCard.BankNumber);

            return null;
        }
    }
}
