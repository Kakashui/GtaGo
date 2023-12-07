using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Whistler.MoneySystem.Interface;

namespace Whistler.MoneySystem.Models
{
    [Table("efcore_bank_transact")]
    internal class TransactionModel
    {
        [Key]
        public int Id { get; set; }
        public int From { get; set; }
        public int FromType { get; set; }
        public int To { get; set; }
        public int ToType { get; set; }
        public int Amount { get; set; }
        public int Tax { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public TransactionModel()
        {

        }
        public TransactionModel(IBankAccount from, IBankAccount to, int amount, int tax, string comment)
        {
            Amount = amount;
            From = from?.IOwnerID ?? -1;
            FromType = (int)(from?.ITypeBank ?? TypeBankAccount.invalid);
            To = to?.IOwnerID ?? -1;
            ToType = (int)(to?.ITypeBank ?? TypeBankAccount.invalid);
            Date = DateTime.Now;
            Tax = tax;
            Comment = comment;
        }
    }
}
