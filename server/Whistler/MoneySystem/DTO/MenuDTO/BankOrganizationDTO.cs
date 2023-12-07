using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Common;
using Whistler.Families.Models;
using Whistler.Fractions.Models;

namespace Whistler.MoneySystem.DTO.MenuDTO
{
    class BankOrganizationDTO
    {
        public string name { get; set; }
        public long balance { get; set; }
        public List<TransactionDTO> transfersList { get; set; }
        public List<OrgPaymentDTO> staffList { get; set; }
        public int houseTaxBalance { get; set; }
        public int houseTaxBalanceMax { get; set; }
        public BankOrganizationDTO(Family family, bool accessPay)
        {
            name = family.Name;
            balance = family.IMoneyBalance;
            transfersList = BankManager.GetTransactionHistory(family);
            var house = Whistler.Houses.HouseManager.GetHouse(family.Id, OwnerType.Family, true);
            if (house != null)
            {
                houseTaxBalance = (int)house.BankModel.Balance;
                houseTaxBalanceMax = house.BankModel.MaxBalance(house.Price, true);
            }
            if (accessPay)
                staffList = family.GetMemberPayments();
            else
                staffList = new List<OrgPaymentDTO>();
        }
        public BankOrganizationDTO(Fraction fraction, bool accessPay)
        {
            name = fraction.Configuration.Name;
            balance = fraction.IMoneyBalance;
            transfersList = BankManager.GetTransactionHistory(fraction);
            if (accessPay)
                staffList = fraction.GetMemberPayments();
            else
                staffList = new List<OrgPaymentDTO>();
        }
    }
}
