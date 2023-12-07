using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Domain.Phone.Bank;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Houses;
using Whistler.Infrastructure.DataAccess;
using Whistler.MoneySystem;
using Whistler.MoneySystem.Models;
using Whistler.Phone.Bank.Dtos;

namespace Whistler.Phone.Bank
{
    class BankGoHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(BankGoHandler));

        [RemoteEvent("phone::bank::acceptTransfer")]
        private static void AcceptTransferMoneyToBankAccount(ExtPlayer player, string bankTarget, int amount)
        {
            long bank = Convert.ToInt64(bankTarget);
            if (MoneyManager.TransferMoneyToAccount(player, bank, amount, null, null, false))
            {
                player.TriggerCefAction("smartphone/bankPage/setCompleateTrasfer", true);
            }
        }
        [RemoteEvent("phone::bank::payHouseTax")]
        private static void AcceptPayHouseTax(ExtPlayer player, int amount)
        {
            if (MoneyManager.PayHouseTax(player, player.Character.BankModel, amount, HouseManager.GetHouse(player, true)))
            {
                player.TriggerCefAction("smartphone/bankPage/setCompleateTaxPay", true);
            }
        }
        [RemoteEvent("phone::bank::payBusinessTax")]
        private static void AcceptPayBusinessTax(ExtPlayer player, int amount)
        {
            if (MoneyManager.PayBusinessTax(player, amount))
            {
                player.TriggerCefAction("smartphone/bankPage/setCompleateTaxPay", true);
            }
        }
        [RemoteEvent("phone::bank::payPenalty")]
        private static void AcceptPayPenalty(ExtPlayer player)
        {
            if (MoneyManager.PayPenalty(player))
            {
                player.TriggerCefAction("smartphone/bankPage/setCompleateTaxPay", true);
            }
        }
    }
}
