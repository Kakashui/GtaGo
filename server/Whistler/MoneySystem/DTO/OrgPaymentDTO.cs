using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.MoneySystem.Models;

namespace Whistler.MoneySystem.DTO
{
    internal class OrgPaymentDTO
    {
        public int uuid { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public int money { get; set; }
        public OrgPaymentDTO(int uuid, string name, TransactionModel transact)
        {
            this.uuid = uuid;
            this.name = name;
            this.date = transact?.Date.ToShortDateString();
            this.money = transact?.Amount ?? 0;
        }
        public OrgPaymentDTO(int uuid, string name, string date, int money)
        {
            this.uuid = uuid;
            this.name = name;
            this.date = date;
            this.money = money;
        }
    }
}
