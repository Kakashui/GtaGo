using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Whistler.Domain.Phone.Bank
{
    [Table("phones_bank_transact")]
    public class TransactionHistoryItem
    {
        [Key]
        public int Id { get; set; }
        public int From { get; set; }
        public int FromType { get; set; }
        public int To { get; set; }
        public int ToType { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
