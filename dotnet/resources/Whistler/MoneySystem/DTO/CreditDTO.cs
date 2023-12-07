using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.MoneySystem.DTO.MenuDTO;
using Whistler.MoneySystem.Models;

namespace Whistler.MoneySystem.DTO
{
    class CreditDTO
    {
        public int id { get; set; }
        public int amount { get; set; }
        public string pledgeType { get; set; }
        public int pledgeId { get; set; }
        public int payment { get; set; }
        public string datePayment { get; set; }
        public int leftPayment { get; set; }
        public int payedAmount { get; set; }


        public List<CreditPaymentDTO> paymentsList { get; set; }
        public CreditDTO(CreditModel credit)
        {
            id = credit.ID;
            amount = credit.Indebtedness + credit.Percents + credit.GetPercent();
            paymentsList = credit.HistoryPayment.Select(item => new CreditPaymentDTO(item)).ToList();
            pledgeType = credit.PledgeType.ToString();
            pledgeId = credit.PledgeId;
            payment = credit.GetNextPayment();
            payedAmount = credit.PayedAmount;
            leftPayment = credit.LeftPayments;
            datePayment = credit.Create.AddDays((credit.HistoryPayment.Count + 1) * 5).ToShortDateString();
        }
    }
}
