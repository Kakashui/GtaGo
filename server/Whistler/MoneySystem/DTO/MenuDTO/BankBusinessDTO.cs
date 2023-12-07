using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;

namespace Whistler.MoneySystem.DTO.MenuDTO
{
    class BankBusinessDTO
    {
        public string bizName { get; set; }
        public long bizBalance { get; set; }
        public int bizTaxBalance { get; set; }
        public int bizTaxMax { get; set; }
        public bool isCredit { get; set; }
        public List<TransactionDTO> transfersList { get; set; }
        public BankBusinessDTO(Business business, bool isPrime)
        {
            bizName = $"{business.TypeModel.TypeName} №{business.ID}";
            bizBalance = business.BankAccountModel.Balance;
            bizTaxBalance = (int)business.BankNalogModel.Balance;
            bizTaxMax = business.BankNalogModel.MaxBalance(business.SellPrice, isPrime);
            isCredit = business.Pledged;
            transfersList = BankManager.GetTransactionHistory(business.BankAccountModel);
        }
    }
}
