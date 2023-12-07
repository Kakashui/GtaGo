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
        public Account Account { get; set; }
        public SimCard Simcard { get; set; }
        public Domain.Phone.Phone Phone { get; set; }
        internal CheckingAccount GetPhoneBankAccount()
        {
            if (Simcard == null)
                return null;
            if (Simcard.BankNumber > 0)
                return BankManager.GetAccount(Simcard.BankNumber);
            Simcard.BankNumber = PhoneLoader.CreateBankNumber(Simcard.Id);
            if (Simcard.BankNumber > 0)
                return BankManager.GetAccount(Simcard.BankNumber);
            return null;
        }
    }
}
