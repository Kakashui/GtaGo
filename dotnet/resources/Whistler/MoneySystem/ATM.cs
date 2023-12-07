using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Businesses;
using Whistler.Core;
using Whistler.Domain.Phone.Bank;
using Whistler.Entities;
using Whistler.Families.FamilyMenu;
using Whistler.Fractions;
using Whistler.Helpers;
using Whistler.Houses;
using Whistler.MoneySystem.DTO;
using Whistler.MoneySystem.DTO.MenuDTO;
using Whistler.MoneySystem.Models;
using Whistler.Phone;
using Whistler.SDK;

namespace Whistler.MoneySystem
{
    class ATM : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ATM));

        public static Dictionary<int, ColShape> ATMCols = new Dictionary<int, ColShape>();

        public static List<Vector3> ATMs = new List<Vector3>
        {
            new Vector3(-30.28312, -723.7054, 43.10828),
            new Vector3(-846.4784, -340.7381, 37.56028),
            new Vector3(-30.28312, -723.7054, 43.10828),
            new Vector3(-57.79301, -92.57375, 56.65908),
            new Vector3(-203.8796, -861.4044, 29.14762),
            new Vector3(-301.6998, -830.0975, 31.29726),
            new Vector3(-1315.741, -834.8119, 15.84172),
            new Vector3(-526.7958, -1222.796, 17.33497),
            new Vector3(-165.068, 232.6937, 93.80193),
            new Vector3(147.585, -1035.683, 28.22313),
            new Vector3(-2072.433, -317.1329, 12.19597),
            new Vector3(-2975.008, 380.1415, 13.87914),
            new Vector3(112.6747, -819.3305, 30.21771),
            new Vector3(111.1934, -775.319, 30.31857),
            new Vector3(-3043.924, 594.6759, 6.616974),
            new Vector3(-3241.165, 997.4967, 11.4304),
            new Vector3(-254.3221, -692.4096, 32.49045),
            new Vector3(-256.154, -716.0692, 32.39723),
            new Vector3(-258.849, -723.3128, 32.36183),
            new Vector3(-537.8723, -854.4181, 28.16625),
            new Vector3(-386.8388, 6046.073, 30.38172),
            new Vector3(155.811, 6642.846, 30.48126),
            new Vector3(-2958.9, 487.8209, 14.34391),
            new Vector3(-594.6927, -1161.374, 21.20427),
            new Vector3(-282.9406, 6226.058, 30.37295),
            new Vector3(-3144.312, 1127.521, 19.73535),
            new Vector3(1167.063, -456.2611, 65.6659),
            new Vector3(1138.276, -469.0832, 65.60734),
            new Vector3(-97.33072, 6455.452, 30.34733),
            new Vector3(-821.5346, -1081.945, 10.01243),
            new Vector3(527.2645, -161.3371, 55.95051),
            new Vector3(-1091.597, 2708.577, 17.82036),
            new Vector3(158.4433, 234.1823, 105.5114),
            new Vector3(1171.491, 2702.544, 37.05545),
            new Vector3(-953.75867, -2067.5562, 9.5257015),
            new Vector3(-2294.625, 356.5286, 173.4816),
            new Vector3(-56.88515, -1752.214, 28.30102),
            new Vector3(2564.523, 2584.744, 36.96311),
            new Vector3(2558.747, 350.9788, 107.5015),
            new Vector3(33.25563, -1348.147, 28.37702),
            new Vector3(1822.76, 3683.133, 33.15678),
            new Vector3(1703.047, 4933.534, 40.94364),
            new Vector3(1686.842, 4815.943, 40.88822),
            new Vector3(89.62029, 2.412876, 67.18955),
            new Vector3(-1410.304, -98.57402, 51.31698),
            new Vector3(288.7548, -1282.287, 28.52028),
            //new Vector3(-1212.692, -330.7367, 36.66656),
            new Vector3(-1205.556, -325.066, 36.73424),
            new Vector3(-611.844, -704.7563, 30.11593),
            new Vector3(-867.6541, -186.0634, 36.72196),
            new Vector3(289.0122, -1256.787, 28.32075),
            new Vector3(1968.167, 3743.618, 31.22374),
            new Vector3(-1305.292, -706.3788, 24.20243),
            new Vector3(-1570.267, -546.7006, 33.83642),
            new Vector3(1701.183, 6426.415, 31.64404),
            new Vector3(-1430.069, -211.1082, 45.37187),
            new Vector3(-1416.06, -212.0282, 45.38037),
            new Vector3(-1109.778, -1690.661, 3.255033),
            new Vector3(237.3561, 217.8394, 105.1667), // 56 gov
            new Vector3(940.8488, 46.70314, 79.1716),
            new Vector3(-2349.857, 3284.854, 31.70238),
            new Vector3(-262.0602, -2012.484, 30.1456)
        };
        private static Dictionary<int, BankPoint> _bankPoints = new Dictionary<int, BankPoint>();

        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            try
            {
                var atm = NAPI.Object.CreateObject(NAPI.Util.GetHashKey("prop_atm_03"), new Vector3(-2349.857, 3284.854, 32), new Vector3(0, 0, 146.6154));
                _logger.WriteInfo("Loading ATMs...");
                for (int i = 0; i < ATMs.Count; i++)
                {
                    if (i != 57) NAPI.Blip.CreateBlip(434, ATMs[i], 0.35f, 11, "Банкомат", shortRange: true, dimension: 0);

                    int atmNumber = i;

                    InteractShape.Create(ATMs[i], 1, 2)
                        .AddInteraction(OpenBankMenu, "interact_11");
                }
                DataTable result = MySQL.QueryRead("SELECT * FROM `bankpoints`");
                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        BankPoint bank = new BankPoint(row);
                        _bankPoints.Add(bank.Id, bank);
                    }
                }
            }
            catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
        }

        [Command("createbankpoint")]
        public static void CreateBankPoint(ExtPlayer player)
        {
            if (!Group.CanUseAdminCommand(player, "createbankpoint")) return;
            BankPoint bank = new BankPoint(player.Position, player.Dimension);
            _bankPoints.Add(bank.Id, bank);
        }
        [Command("deletebankpoint")]
        public static void CreateBankPoint(ExtPlayer player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "deletebankpoint")) return;
            if (_bankPoints.ContainsKey(id))
            {
                _bankPoints[id].Destroy();
                _bankPoints.Remove(id);
            }
        }

        public static void OpenBankMenu(ExtPlayer player)
        {
            BankDTO bankDto = new BankDTO(player);
            List<DepositDTO> depositsDto = DepositManager.GetDepositDTOs(player);

            Business business = player.GetBusiness();
            BankBusinessDTO businessDto = business != null ? new BankBusinessDTO(business) : null;

            BankCreditDTO creditDto = new BankCreditDTO(player);

            Families.Models.Family family = player.GetFamily();
            BankOrganizationDTO familyDto = family != null ? new BankOrganizationDTO(family, family.IsLeader(player)) : null;

            Fractions.Models.Fraction fraction = player.GetFraction();
            BankOrganizationDTO fractionDto = fraction != null ? new BankOrganizationDTO(fraction, fraction.IsLeaderOrSub(player)) : null;

            SafeTrigger.ClientEvent(player, "bankMenu:open", 
                JsonConvert.SerializeObject(bankDto), 
                JsonConvert.SerializeObject(depositsDto), 
                JsonConvert.SerializeObject(businessDto), 
                JsonConvert.SerializeObject(creditDto), 
                JsonConvert.SerializeObject(familyDto), 
                JsonConvert.SerializeObject(fractionDto));
        }

        [RemoteEvent("bank:topUp")]
        public static void BankRefillMoney(ExtPlayer player, int amount, string type, int depositId)
        {
            var acc = player.Character;
            switch (type)
            {
                case "deposit":
                    DepositManager.AddedMoneyToDeposit(player, depositId, amount);
                    break;
                case "family":
                    if (FamilyMenuManager.RemoteEvent_tryPutMoney(player, amount))
                    {
                        player.TriggerCefEvent("bank/family/updateFamilyBalance", player.GetFamily().IMoneyBalance);
                    }
                    else
                        Notify.SendError(player, "bank:menu:218");
                    break;
                case "personal":
                    if (Wallet.TransferMoney(acc, player.Character.BankModel, amount, 0, "Положил деньги на банк.счёт"))
                        Notify.SendSuccess(player, "bank:menu:230");
                    else
                        Notify.SendError(player, "bank:menu:218");
                    break;
                case "business":
                    if (BusinessManager.BusinessDepositMoney(player, amount))
                    {
                        Notify.SendSuccess(player, "bank:menu:230");
                        player.TriggerCefEvent("bank/business/updateBizBalance", player.GetBusiness().BankAccountModel.IMoneyBalance);
                    }
                    else
                        Notify.SendError(player, "bank:menu:218");
                    break;
                case "fraction":
                    if (Manager.FractionDepositMoney(player, amount))
                    {
                        player.TriggerCefEvent("bank/organization/updateFractionBalance", player.GetFraction().IMoneyBalance);
                    }
                    else
                        Notify.SendError(player, "bank:menu:218");
                    break;
                default:
                    break;
            }
        }
        [RemoteEvent("bank:withdraw")]
        public static void BankWithdrawMoney(ExtPlayer player, int amount, string type, int depositId)
        {
            var acc = player.Character;
            switch (type)
            {
                case "deposit":
                    DepositManager.WithdrawMoneyFromDeposit(player, depositId, amount);
                    break;
                case "family":
                    if (FamilyMenuManager.RemoteEvent_tryTakeMoney(player, amount))
                    {
                        player.TriggerCefEvent("bank/family/updateFamilyBalance", player.GetFamily().IMoneyBalance);
                    }
                    else
                        Notify.SendError(player, "bank:menu:218");
                    break;
                case "personal":
                    if (Wallet.TransferMoney(player.Character.BankModel, acc, amount, 0, "Снял деньги с банк.счёта"))
                        Notify.SendSuccess(player, "bank:menu:231");
                    else
                        Notify.SendError(player, "bank:menu:218");
                    break;
                case "business":
                    if (BusinessManager.BusinessWithdrawMoney(player, amount))
                    {
                        Notify.SendSuccess(player, "bank:menu:231");
                        player.TriggerCefEvent("bank/business/updateBizBalance", player.GetBusiness().BankAccountModel.IMoneyBalance);
                    }
                    else
                        Notify.SendError(player, "bank:menu:218");
                    break;
                case "fraction":
                    if (Manager.FractionWithdrawMoney(player, amount))
                    {
                        player.TriggerCefEvent("bank/organization/updateFractionBalance", player.GetFraction().IMoneyBalance);
                    }
                    else
                        Notify.SendError(player, "bank:menu:218");
                    break;
                default:
                    break;
            }
        }
        [RemoteEvent("bank:transfer")]
        public static void BankTransferMoney(ExtPlayer player, string targetNumber, int amount, string comment, string reason)
        {
            var target = Convert.ToInt64(targetNumber);
            MoneyManager.TransferMoneyToAccount(player, target, amount, comment, reason, true, true);
        }
        [RemoteEvent("bank:payHouseTax")]
        public static void BankPayHouseTax(ExtPlayer player, int amount, bool personalHouse)
        {
            if (personalHouse)
            {
                var house = HouseManager.GetHouse(player, true);
                if (MoneyManager.PayHouseTax(player, player.Character.BankModel, amount, house))
                    player.TriggerCefEvent("bank/updateHomeBalance", house.BankModel.IMoneyBalance);
            }
            else
            {
                House famHouse = HouseManager.GetHouseFamily(player);
                Families.Models.Family family = player.GetFamily();
                if (!MoneyManager.PayHouseTax(player, family, amount, famHouse)) return;

                player.TriggerCefEvent("bank/family/updateFamilyBalance", family.IMoneyBalance);
                player.TriggerCefEvent("bank/family/updateFamilyHomeBalance", famHouse.BankModel.IMoneyBalance);
                DateTime date = DateTime.Now.AddMinutes(-5);
                List<TransactionModel> transactions = EFCore.DBManager.GlobalContext.Transactions.Where(x => x.Date > date && x.To == famHouse.BankModel.ID).ToList();
                if (transactions == null || !transactions.Any()) return;

                TransactionModel transaction = transactions.Last();
                player.TriggerCefEvent("bank/family/addTransaction", JsonConvert.SerializeObject(new TransactionDTO(transaction, transaction.From)));
            }
        }
        [RemoteEvent("bank:payBizTax")]
        public static void BankPayBusinessTax(ExtPlayer player, int amount)
        {
            if (!MoneyManager.PayBusinessTax(player, amount)) return;

            Business business = player.GetBusiness();
            long nalogBalance = business.BankNalogModel.IMoneyBalance;
            long bankBalance = business.BankAccountModel.IMoneyBalance;
            player.TriggerCefEvent("bank/business/updateBizTaxBalance", nalogBalance);
            player.TriggerCefEvent("bank/business/updateBizBalance", bankBalance);
            player.TriggerCefEvent("optionsMenu/updateBizTaxCount", nalogBalance);
        }
        [RemoteEvent("bank:openDeposit")]
        public static void BankCreateDeposit(ExtPlayer player, int typeId, int amount, int days) => DepositManager.CreateDeposit(player, typeId, amount, days);
        
        [RemoteEvent("bank:closeDeposit")]
        public static void BankCreateDeposit(ExtPlayer player, int id) => DepositManager.CloseDeposit(player, id);

        [RemoteEvent("bank:checkStatusProperty")]
        public static void BankCheckStatusProperty(ExtPlayer player)
        {
        }
        [RemoteEvent("bank:calcCredit")]
        public static void BankCalculateCredit(ExtPlayer player, int amount, int days, string typePayment, bool isPledge)
        {
            if (!Enum.TryParse(typePayment, out CreditTypePayment typePay))
                return;
            CreditManager.CalculateCreditParams(player, amount, days, typePay, isPledge);
        }
        [RemoteEvent("bank:createCredit")]
        public static void BankCreateCredit(ExtPlayer player, int amount, int days, string typePayment, string pledgedType, int pledgedId)
        {
            
            if (!Enum.TryParse(pledgedType, out PropertyType pledgeType))
                return;
            if (!Enum.TryParse(typePayment, out CreditTypePayment typePay))
                return;
            CreditManager.CreateCredit(player, amount, days, typePay, pledgedId, pledgeType);
        }        
        [RemoteEvent("bank:payCredit")]
        public static void BankPayCredit(ExtPlayer player, int id, int amount) => CreditManager.PayCredit(player, id, amount);

        [RemoteEvent("bank:payPenalty")]
        public static void BankPayPenalty(ExtPlayer player, int id)
        {
            if (id == -1)
                MoneyManager.PayPenalty(player);
        }

        [RemoteEvent("bank:payPhoneBalance")]
        public static void BankPayPhone(ExtPlayer player, int amount)
        {
            var acc = player.Character.PhoneTemporary.GetPhoneBankAccount();
            if (Wallet.TransferMoney(player.Character.BankModel, acc, amount, 0, "Пополнение баланса телефона"))
                Notify.SendSuccess(player, "bank:menu:217");
            else
                Notify.SendError(player, "bank:menu:218");
        }

        [RemoteEvent("bank:payBonus")]
        public static void BankPayBonus(ExtPlayer player, int uuid, int amount, string type)
        {
            switch (type)
            {
                case "fraction":
                    Fractions.Models.Fraction fraction = player.GetFraction();
                    if (!fraction.IsLeaderOrSub(player))
                    {
                        Notify.SendError(player, "bank:menu:219");
                        return;
                    }
                    if (!fraction.Members.ContainsKey(uuid))
                    {
                        Notify.SendError(player, "bank:menu:220");
                        return;
                    }
                    var bankAccount = BankManager.GetAccountByUUID(uuid);
                    if (!Wallet.TransferMoney(fraction, bankAccount, amount, 0, "Премия (организация)"))
                    {
                        Notify.SendError(player, "bank:menu:221");
                        return;
                    }
                    OrgPaymentDTO payment = new OrgPaymentDTO(uuid, Main.PlayerNames.GetValueOrDefault(uuid), DateTime.Now.ToShortDateString(), amount);
                    fraction.UpdatePayments(payment);
                    player.TriggerCefEvent("bank/organization/updateStaffList", JsonConvert.SerializeObject(payment));
                    player.TriggerCefEvent("bank/organization/updateFractionBalance", fraction.IMoneyBalance);
                    Notify.SendSuccess(player, "bank:menu:222");
                    if (player.Character.UUID == uuid) break;

                    DateTime date = DateTime.Now.AddMinutes(-5);
                    List<TransactionModel> transactions = EFCore.DBManager.GlobalContext.Transactions.Where(x => x.Date > date && x.From == fraction.IOwnerID && x.To == bankAccount.ID).ToList();
                    if (transactions == null || !transactions.Any()) return;

                    TransactionModel transaction = transactions.Last();
                    player.TriggerCefEvent("bank/organization/addTransaction", JsonConvert.SerializeObject(new TransactionDTO(transaction, transaction.From)));
                    break;
                case "family":
                    Families.Models.Family family = player.GetFamily();
                    if (!family.IsLeader(player))
                    {
                        Notify.SendError(player, "bank:menu:219");
                        return;
                    }
                    if (!family.Members.ContainsKey(uuid))
                    {
                        Notify.SendError(player, "bank:menu:220");
                        return;
                    }
                    bankAccount = BankManager.GetAccountByUUID(uuid);
                    if (!Wallet.TransferMoney(family, bankAccount, amount, 0, "Премия (семья)"))
                    {
                        Notify.SendError(player, "bank:menu:223");
                        return;
                    }
                    payment = new OrgPaymentDTO(uuid, Main.PlayerNames.GetValueOrDefault(uuid), DateTime.Now.ToShortDateString(), amount);
                    family.UpdatePayments(payment);
                    player.TriggerCefEvent("bank/family/updateStaffList", JsonConvert.SerializeObject(payment));
                    player.TriggerCefEvent("bank/family/updateFamilyBalance", family.IMoneyBalance);
                    Notify.SendSuccess(player, "bank:menu:222");
                    if (player.Character.UUID == uuid) break;

                    date = DateTime.Now.AddMinutes(-5);
                    transactions = EFCore.DBManager.GlobalContext.Transactions.Where(x => x.Date > date && x.From == family.IOwnerID && x.To == bankAccount.ID).ToList();
                    if (transactions == null || !transactions.Any()) return;

                    transaction = transactions.Last();
                    player.TriggerCefEvent("bank/family/addTransaction", JsonConvert.SerializeObject(new TransactionDTO(transaction, transaction.From)));
                    break;
                default:
                    break;
            }
        }
    }
}
