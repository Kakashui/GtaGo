using System;
using System.Collections.Generic;
using System.Text;
using Whistler.MoneySystem.Models;

namespace Whistler.MoneySystem.DTO
{
    class DepositDTO
    {
        public int id { get; set; }
        public int balance { get; set; }
        public int profit { get; set; }
        public int depositTypes { get; set; }
        public int time { get; set; }
        public DepositDTO(Deposit deposit)
        {
            id = deposit.ID;
            balance = deposit.Amount + deposit.AddedMoney;
            profit = deposit.Profit;
            depositTypes = (int)deposit.DepositTypes;
            time = deposit.DepositFullTime - deposit.HoursInInterval;
        }
    }
}
