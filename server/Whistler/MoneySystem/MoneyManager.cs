using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Common;
using Whistler.Core;
using Whistler.Core.Character;
using Whistler.Core.ReportSystem;
using Whistler.Fractions;
using Whistler.Helpers;
using Whistler.Houses;
using Whistler.MoneySystem.Interface;
using Whistler.MoneySystem.Models;
using Whistler.Phone.Bank;
using Whistler.SDK;

namespace Whistler.MoneySystem
{
    class MoneyManager : Script
    {
        public static readonly ServerMoney ServerMoney = new ServerMoney(TypeMoneyAcc.Server, 0);
        private static Dictionary<ulong, int> _transferMoneyLimit = new Dictionary<ulong, int>();

        public static bool PayHouseTax(Player player, IMoneyOwner from, int amount, House house)
        {
            if (from == null || house == null) return false;
            var maxMoney = house.BankModel.MaxBalance(house.Price, house.OwnerType == OwnerType.Personal ? player.GetAccount().IsPrimeActive() : true);

            if (house.BankModel.Balance + amount > maxMoney)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Money_8", 3000);
                return false;
            }

            if (Wallet.TransferMoney(from, house.BankModel, amount, 0, "Money_AtmHouse"))
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Money_10", 3000);
                return true;
            }
            else
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Money_18", 3000);
                return false;
            }
        }

        public static bool PayBusinessTax(Player player, int amount)
        {
            var playerGo = player.GetPlayerGo();
            var biz = player.GetBusiness();
            if (biz == null)
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Money_11", 3000);
                return false;
            }
            var maxMoney = biz.BankNalogModel.MaxBalance(biz.SellPrice, playerGo.Account.IsPrimeActive());
            if (biz.BankNalogModel.Balance + amount > maxMoney)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Money_9", 3000);
                return false;
            }

            if (Wallet.TransferMoney(biz.BankAccountModel, biz.BankNalogModel, amount, 0, "Money_AtmBiz"))
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Money_10", 3000);
                return true;
            }
            else
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Money_18", 3000);
                return false;
            }
        }
        public static bool PayPenalty(Player player)
        {
            if (!player.IsLogged())
                return false;
            var playerGo = player.GetPlayerGo();
            int mulct = playerGo.Character.Mulct;
            if (mulct <= 0)
                return false;
            if (!Wallet.TransferMoney(player.GetBankAccount(), Manager.GetFraction(6), mulct, 0, "Money_Paypdd"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_1", 3000);
                return false;
            }
            player.GetCharacter().Mulct = 0;
            player.TriggerCefEvent("smartphone/bankPage/setPenaltySum", 0);
            MySQL.Query($"UPDATE characters SET mulct = 0 WHERE uuid=@prop0", playerGo.Character.UUID);
            Notify.SendAlert(player, "local_37".Translate(mulct));
            return true;
        }

        public static bool TransferMoneyToAccount(Player player, long targetAccount, int amount, string comment, string reason, bool createRequest, bool updateATM = false)
        {
            var acc = player.GetCharacter();
            var targetAcc = BankManager.GetAccountByNumber(targetAccount);
            if (targetAcc == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Money_12", 3000);
                return false;
            }
            if (acc.BankNew == targetAcc.ID)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Money_12", 3000);
                return false;
            }
            if (acc.AdminLVL < 8)
            {
                if (acc.LVL < 1) //передача денег
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Перевод денег доступен после 1 уровня", 3000);
                    return false;
                }

                if (targetAcc.OwnerType != TypeBankAccount.Player)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Money_11", 3000);
                    return false;
                }
            }
            if (!CheckLimit(player, amount))
            {
                if (createRequest)
                    CreateRequestTransferMoney(player, acc.BankModel, BankManager.GetAccountByNumber(targetAccount), amount, comment, reason);
                else
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Money_28", 3000);
                return false;
            }
            if (Wallet.TransferMoney(acc.BankModel, BankManager.GetAccountByNumber(targetAccount), amount, 0, comment != null ? "Money_AtmTransCom".Translate(comment) : "Money_AtmTrans"))
            {
                UpdateTransgerLimit(player.SocialClubId, amount);
                if (updateATM)
                    player.TriggerCefEvent("bank/updateBalanceWithTransfer", _transferMoneyLimit.GetValueOrDefault(player.SocialClubId, 0));
                return true;
            }
            else
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Money_18", 3000);
                return false;
            }
        }
        public static int GetLimit(Player player)
        {
            return _transferMoneyLimit.GetValueOrDefault(player.SocialClubId, 0);
        }
        public static bool CheckLimit(Player player, int amount)
        {
            return (player.GetCharacter().AdminLVL >= 8) || (_transferMoneyLimit.GetValueOrDefault(player.SocialClubId, 0) + amount <= Whistler.MoneySystem.Settings.BankSettings.TransferLimitInDay);
        }
        public static void UpdateTransgerLimit(ulong socialClub, int amount)
        {
            if (_transferMoneyLimit.ContainsKey(socialClub))
                _transferMoneyLimit[socialClub] += amount;
            else
                _transferMoneyLimit.Add(socialClub, amount);
        }
        private static void CreateRequestTransferMoney(Player player, CheckingAccount from, CheckingAccount to, int amount, string comment, string reason)
        {
            if (ReportManager.CreateTransfer(player.SocialClubId, player.Name, Main.PlayerNames.GetValueOrDefault(to.UUID), from, to, amount, comment, reason))
            {
                Notify.SendSuccess(player, "Money_30");
            }
            else
                Notify.SendError(player, "Money_31");

        }

        public static void SubscribePlayerToBankAccounts(Player player, Character character)
        {
            player.GetBankAccount()?.Subscribe(player);
            var house = HouseManager.GetHouse(character.UUID, OwnerType.Personal, true);
            if (house != null)
                house.BankModel.Subscribe(player, house.Price);
            var biz = BusinessManager.GetBusinessByOwner(character.UUID);
            if (biz != null)
                biz.BankNalogModel.Subscribe(player, biz.SellPrice);
            player.TriggerCefEvent("smartphone/bankPage/setPenaltySum", character.Mulct);
            player.TriggerCefEvent("smartphone/bankPage/setHistoryItems", JsonConvert.SerializeObject(BankManager.GetTransactionHistory(player.GetBankAccount())));
        }
        public static void UnsubscribePlayerFromBankAccounts(Character character)
        {
            character.BankModel?.UnSubscribe();
            HouseManager.GetHouse(character.UUID, OwnerType.Personal, true)?.BankModel?.UnSubscribe();
            BusinessManager.GetBusinessByOwner(character.UUID)?.BankNalogModel?.UnSubscribe();
            character.PhoneTemporary?.GetPhoneBankAccount()?.UnSubscribe();
        }
    }
}
