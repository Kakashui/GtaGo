using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Houses;

namespace Whistler.MoneySystem.DTO.MenuDTO
{
    class BankDTO
    {
        public int account { get; set; }
        public string cardNumber { get; set; }
        public int houseBalance { get; set; }
        public int houseBalanceMax { get; set; }
        public int phoneNumber { get; set; }
        public int transfersPerDay { get; set; }
        public int dailyTransferLimit { get; set; }
        public List<TransactionDTO> transactionsList { get; set; }
        public List<PenaltyDTO> finesList { get; set; }
        public BankDTO(ExtPlayer player)
        {
            var bank = player.Character.BankModel;
            account = bank.ID;
            cardNumber = bank.Number.ToString();
            var house = HouseManager.GetHouse(player, true);
            var houseBank = house?.BankModel;
            if (houseBank != null)
            {
                houseBalance = (int)houseBank.Balance;
                houseBalanceMax = houseBank.MaxBalance(house.Price);
            }
            transactionsList = BankManager.GetTransactionHistory(bank);
            finesList = new List<PenaltyDTO>();
            phoneNumber = player.GetPhone()?.Phone?.SimCard?.Number ?? 0;
            var mulct = player.Character.Mulct;
            if (mulct > 0)
                finesList.Add(new PenaltyDTO(-1, "bank:menu:1", player.Character.Mulct, "-"));
            transfersPerDay = MoneyManager.GetLimit(player);
            dailyTransferLimit = Whistler.MoneySystem.Settings.BankSettings.TransferLimitInDay;

        }
    }
}
