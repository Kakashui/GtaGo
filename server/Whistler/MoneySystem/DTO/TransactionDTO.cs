using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.MoneySystem.Models;

namespace Whistler.MoneySystem.DTO
{
    class TransactionDTO
    {
        public int date { get; set; }
        public int value { get; set; }
        public int recipient { get; set; }
        public bool directFrom { get; set; }
        public int recipientType { get; set; }
        public int tax { get; set; }
        public string comment { get; set; }
        public TransactionDTO(TransactionModel transaction, int myNumber)
        {
            date = transaction.Date.GetTotalSeconds();
            value = transaction.Amount;
            recipient = transaction.From == myNumber ? transaction.To : transaction.From;
            directFrom = transaction.From == myNumber;
            recipientType = transaction.From == myNumber ? transaction.ToType : transaction.FromType;
            tax = transaction.Tax;
            comment = transaction.Comment;
        }
    }
}
