using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.MoneySystem.Models
{
    class CreditPayment
    {
        public CreditPayment(DateTime date, int amount, int rest)
        {
            Date = date;
            Amount = amount;
            Rest = rest;
        }

        public DateTime Date { get; set; }
        public int Amount { get; set; }
        public int Rest { get; set; }
    }
}
