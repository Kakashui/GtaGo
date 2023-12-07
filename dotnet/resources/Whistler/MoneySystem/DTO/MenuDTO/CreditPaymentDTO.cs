using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.MoneySystem.Models;

namespace Whistler.MoneySystem.DTO.MenuDTO
{
    class CreditPaymentDTO
    {
        public CreditPaymentDTO(CreditPayment creditPayment)
        {
            date = creditPayment.Date.ToShortDateString();
            payment = creditPayment.Amount;
            amount = creditPayment.Rest;
        }

        public string date { get; set; }
        public int payment { get; set; }
        public int amount { get; set; }
    }
}
