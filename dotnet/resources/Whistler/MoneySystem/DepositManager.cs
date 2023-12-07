using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.MoneySystem.DTO;
using Whistler.MoneySystem.Models;
using Whistler.MoneySystem.Settings;

namespace Whistler.MoneySystem
{
    class DepositManager
    {
        public static void CreateDeposit(ExtPlayer player, int typeId, int amount, int days)
        {
            var character = player.Character;
            if (character == null) return;
            if (!Enum.IsDefined(typeof(DepositTypes), typeId))
            {
                return;
            }
            DepositTypes type = (DepositTypes)typeId;
            var depositConf = BankSettings.GetDepositSetting(type);
            if (depositConf == null || !depositConf.Actual)
            {
                return;
            }
            if (!Wallet.MoneySub(player.Character.BankModel, amount, "Money_DepositCreate"))
            {
                return;
            }
            var deposit = new Deposit(character.UUID, BankManager.HeadBank.ID, amount, type, days);
            using (var context = EFCore.DBManager.TemporaryContext)
            {
                context.Deposits.Add(deposit);
                context.SaveChanges();
                EFCore.DBManager.GlobalContext.Deposits.Attach(deposit);
            }
            UpdateDeposit(player, deposit);
        }
        public static void CloseDeposit(ExtPlayer player, int id)
        {
            var deposit = GetDepositByID(id);
            if (deposit == null)
                return;
            deposit.CloseDeposit();
            player.TriggerCefEvent("bank/deposit/deleteDeposit", JsonConvert.SerializeObject(deposit.ID));
        }
        public static bool AddedMoneyToDeposit(ExtPlayer player, int id, int amount)
        {
            var deposit = GetDepositByID(id);
            if (deposit == null)
            {
                return false;
            }
            if (!deposit.Config.IsRefill)
            {
                return false;
            }
            if (!Wallet.TransferMoney(player.Character.BankModel, deposit, amount, 0, "Money_RefillDeposit"))
            {
                return false;
            }
            UpdateDeposit(player, deposit);
            return true;
        }
        public static bool WithdrawMoneyFromDeposit(ExtPlayer player, int id, int amount)
        {
            var deposit = GetDepositByID(id);
            if (deposit == null)
            {
                return false;
            }
            if (!deposit.Config.IsWithdraw)
            {
                return false;
            }
            if (!Wallet.TransferMoney(deposit, player.Character.BankModel, amount, 0, "Money_WithdrawDeposit"))
            {
                return false;
            }
            UpdateDeposit(player, deposit);
            return true;
        }
        public static void GivePercentToPlayerDeposits(ExtPlayer player)
        {
            List<Deposit> deposits = EFCore.DBManager.GlobalContext.Deposits.Where(item => item.UUID == player.Character.UUID && !item.ClosedDeposit).ToList();
            if (!deposits.Any()) return;

            foreach (Deposit deposit in deposits)
            {
                if (deposit == null) continue;

                deposit.GivePercent();
            }
        }
        public static Deposit GetDepositByID(int id)
        {
            return EFCore.DBManager.GlobalContext.Deposits.Find(id);
        }
        public static List<DepositDTO> GetDepositDTOs(ExtPlayer player)
        {
            return EFCore.DBManager.GlobalContext.Deposits.Where(item => item.UUID == player.Character.UUID && !item.ClosedDeposit).ToList().Where(item => !item.ClosedDeposit).Select(item => new DepositDTO(item)).ToList();
        }

        private static void UpdateDeposit(ExtPlayer player, Deposit deposit)
        {
            player.TriggerCefEvent("bank/deposit/updateDeposit", JsonConvert.SerializeObject(new DepositDTO(deposit)));
        }
    }
}
