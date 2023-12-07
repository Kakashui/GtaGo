using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Domain.Phone.Bank;
using Whistler.Helpers;
using Whistler.MoneySystem.Models;

namespace Whistler.Phone.Bank.Dtos
{
    class BankTransactionDto
    {
        public int date { get; set; }
        public int value { get; set; }
        public int recipient { get; set; }
        public string type { get; set; }
        public int recipientType { get; set; }
        public int tax { get; set; }
        public string comment { get; set; }
        public BankTransactionDto(TransactionModel transaction, int myNumber)
        {
            date = transaction.Date.GetTotalSeconds();
            value = transaction.Amount;
            recipient = transaction.From == myNumber ? transaction.To : transaction.From;
            type = transaction.From == myNumber ? "out" : "in";
            recipientType = transaction.From == myNumber ? transaction.ToType : transaction.FromType;
            tax = transaction.Tax;
            comment = transaction.Comment;
        }
    }
}
