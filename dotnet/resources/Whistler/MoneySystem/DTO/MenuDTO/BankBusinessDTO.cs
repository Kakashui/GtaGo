using System.Collections.Generic;
using Whistler.Businesses.Manager.DTOs;
using Whistler.Core;

namespace Whistler.MoneySystem.DTO.MenuDTO
{
    class BankBusinessDTO
    {
        public string bizName { get; private set; }
        public long bizBalance { get; private set; }
        public BizProfitDTO bizProfit { get; private set; }
        public int bizTaxBalance { get; private set; }
        public int bizTaxMax { get; private set; }
        public bool isCredit { get; private set; }
        public List<TransactionDTO> transfersList { get; private set; }
        public BankBusinessDTO(Business business)
        {
            bizName = $"{business.TypeModel.TypeName} №{business.ID}";
            bizBalance = business.BankAccountModel.Balance;
            bizProfit = business.ProfitData;
            bizTaxBalance = (int)business.BankNalogModel.Balance;
            bizTaxMax = business.BankNalogModel.MaxBalance(business.SellPrice);
            isCredit = business.Pledged;
            transfersList = BankManager.GetTransactionHistory(business.BankAccountModel);
        }
    }
}
