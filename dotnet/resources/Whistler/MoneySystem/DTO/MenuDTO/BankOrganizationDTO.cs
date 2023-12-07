using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Common;
using Whistler.Families.Models;
using Whistler.Fractions.Models;
using Whistler.Helpers;

namespace Whistler.MoneySystem.DTO.MenuDTO
{
    class BankOrganizationDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("balance")]
        public long Balance { get; set; }
        [JsonProperty("transfersList")]
        public List<TransactionDTO> TransfersList { get; set; }
        [JsonProperty("staffList")]
        public List<OrgPaymentDTO> StaffList { get; set; }
        public int houseTaxBalance { get; set; } = 0;
        public int houseTaxBalanceMax { get; set; } = 0;
        public BankOrganizationDTO(Family family, bool accessPay)
        {
            Name = family.Name;
            Balance = family.IMoneyBalance;
            TransfersList = BankManager.GetTransactionHistory(family);
            StaffList = accessPay ? family.GetMemberPayments() : new List<OrgPaymentDTO>();
            Houses.House house = Houses.HouseManager.GetHouse(family.Id, OwnerType.Family, true);
            if (house == null) return;

            houseTaxBalance = (int)house.BankModel.Balance;
            houseTaxBalanceMax = house.BankModel.MaxBalance(house.Price);
        }
        public BankOrganizationDTO(Fraction fraction, bool accessPay)
        {
            Name = fraction.Configuration.Name;
            Balance = fraction.IMoneyBalance;
            TransfersList = BankManager.GetTransactionHistory(fraction);
            StaffList = accessPay ? fraction.GetMemberPayments() : new List<OrgPaymentDTO>();
        }
    }
}
