using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Text;
using Whistler.Common;
using Whistler.Common.Interfaces;
using Whistler.Core;
using Whistler.Houses;
using Whistler.MoneySystem.Settings;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.MoneySystem.Models
{
    [Table("efcore_bank_credit")]
    class CreditModel
    {
        [Key]
        public int ID { get; set; }
        public int UUID { get; set; }
        public int BankId { get; set; }
        /// <summary>
        /// Тип платежей
        /// </summary>
        public CreditTypePayment TypePayment { get; set; }
        /// <summary>
        /// Задолженность по основной сумме
        /// </summary>
        public int Indebtedness { get; set; }
        /// <summary>
        /// Количество оставшихся платежей
        /// </summary>
        public int LeftPayments { get; set; }
        /// <summary>
        /// Заложенное имущество
        /// </summary>
        public int PledgeId { get; set; }
        /// <summary>
        /// Тип заложенного имущества
        /// </summary>
        public PropertyType PledgeType { get; set; }
        /// <summary>
        /// Сумма погашения следующего платежа
        /// </summary>
        public int PayedAmount { get; set; }
        /// <summary>
        /// Задолженность по начисленным процентам
        /// </summary>
        public int Percents { get; set; }
        /// <summary>
        /// Статус кредита
        /// </summary>
        public bool ClosedCredit { get; set; }
        /// <summary>
        /// Годовая процентная ставка
        /// </summary>
        public float InterestRate { get; set; }
        public DateTime Create { get; set; }
        public List<CreditPayment> HistoryPayment { get; set; }
        public CreditModel()
        {

        }

        public CreditModel(int uuid, int bankId, CreditTypePayment typePayment, int indebtedness, int leftPayments, int pledgeId, PropertyType pledgeType, float interestRate)
        {
            UUID = uuid;
            BankId = bankId;
            TypePayment = typePayment;
            Indebtedness = indebtedness;
            LeftPayments = leftPayments;
            PledgeId = pledgeId;
            PledgeType = pledgeType;
            PayedAmount = 0;
            Percents = 0;
            ClosedCredit = false;
            InterestRate = interestRate;
            Create = DateTime.Now;
            HistoryPayment = new List<CreditPayment>();
        }

        public static int GetPercent(int amount, float yearRate)
        {
            return (int)(amount * yearRate * (BankSettings.IntervalCalculatePercent / 36000.0));
        }
        public int GetPercent()
        {
            return (int)((Indebtedness + Percents) * InterestRate * (BankSettings.IntervalCalculatePercent / 36000.0));
        }
        internal static int GetNextPayment(int amount, int percents, float yearRate, CreditTypePayment typePayment, int countPayment)
        {
            var rate = yearRate * BankSettings.IntervalCalculatePercent / 36000.0;
            switch (typePayment)
            {
                case CreditTypePayment.Annuity:
                    if (countPayment > 0)
                        return (int)((amount + percents) * (rate + rate / (Math.Pow(1 + rate, countPayment) - 1))) + percents;
                    else
                        return (int)((amount + percents) * rate) + percents;
                case CreditTypePayment.Differentiated:
                    return (amount + percents) / (countPayment > 0 ? countPayment : 1) + GetPercent(amount + percents, yearRate) + percents;
                default:
                    return 0;
            }
        }
        internal int GetNextPayment()
        {
            var rate = InterestRate * BankSettings.IntervalCalculatePercent / 36000.0;

            switch (TypePayment)
            {
                case CreditTypePayment.Annuity:
                    if (LeftPayments > 0)
                        return (int)((Indebtedness + Percents) * (rate + rate / (Math.Pow(1 + rate, LeftPayments) - 1))) + Percents;
                    else
                        return (int)((Indebtedness + Percents) * rate) + Percents;
                case CreditTypePayment.Differentiated:
                    return (Indebtedness + Percents) / (LeftPayments > 0 ? LeftPayments : 1) + GetPercent() + Percents;
                default:
                    return 0;
            }
        }

        public void CalculatePercent()
        {
            var days = (DateTime.Now - Create).TotalDays;
            var newPercent = GetPercent();
            if (((days / BankSettings.IntervalCalculatePercent) < HistoryPayment.Count + 1) && (PayedAmount < Percents + Indebtedness + newPercent))
                return;
            if ((PayedAmount < newPercent + Percents) && Percents > 0 && PledgeType != PropertyType.Invalid)
            {
                Percents += newPercent;
                CloseCreditWithSellPledge();
                return;
            }
            int currentPay = PayedAmount;
            if (Percents > 0)
            {
                if (currentPay > Percents)
                {
                    currentPay -= Percents;
                    Percents = 0;
                }
                else
                {
                    Percents -= currentPay;
                    currentPay = 0;
                }
            }
            if (currentPay > newPercent)
            {
                currentPay -= newPercent;
                newPercent = 0;
            }
            else
            {
                newPercent -= currentPay;
                currentPay = 0;
            }
            if (newPercent > 0)
                Percents += newPercent;
            if (currentPay > 0)
                Indebtedness -= currentPay;
            HistoryPayment.Add(new CreditPayment(DateTime.Now, PayedAmount, Indebtedness + Percents));
            EFCore.DBManager.GlobalContext.Update(this);
            PayedAmount = 0;
            LeftPayments--;
            if (Indebtedness + Percents >= 10 && LeftPayments < 0 && PledgeType != PropertyType.Invalid)
                CloseCreditWithSellPledge();
            if (Indebtedness + Percents < 10)
                CloseCredit();
        }

        private void CloseCreditWithSellPledge()
        {
            int price = 0;
            IWhistlerProperty property = PropertyMethods.GetWhistlerPropertyByName(PledgeType, PledgeId);
            if (property != null)
            {
                property.SetPledged(false);
                price = property.CurrentPrice;
                property.DeletePropertyFromMember();
            }
            price /= 2;
            int duty = GetFullAmountCredit(Indebtedness, Percents, InterestRate, LeftPayments, TypePayment);
            price -= duty;
            if (price < 1)
                price = 1;
            Wallet.MoneyAdd(BankManager.GetAccountByUUID(UUID), price, $"Продажа имущества за неоплаченный кредит ({PledgeType}, {PledgeId})");
            CloseCredit();
        }
        public void CloseCredit()
        {
            Indebtedness = 0;
            Percents = 0;
            ClosedCredit = true;
            IWhistlerProperty property = PropertyMethods.GetWhistlerPropertyByName(PledgeType, PledgeId);
            property?.SetPledged(false);
        }

        public void PayCredit(int amount)
        {
            PayedAmount += amount;
        }

        public static int GetFullAmountCredit(int amount, int percents, float yearRate, int countPayments, CreditTypePayment typePayment)
        {
            switch (typePayment)
            {
                case CreditTypePayment.Annuity:
                    return GetNextPayment(amount, percents, yearRate, typePayment, countPayments) * (countPayments > 0 ? countPayments : 1);
                case CreditTypePayment.Differentiated:
                    var rate = yearRate * BankSettings.IntervalCalculatePercent / 36000.0;
                    return amount + percents + (countPayments > 0 ? (int)(rate * (amount + percents) * ((countPayments + 1) / 2)) : (int)(rate * (amount + percents)));
            }
            return 0;
        }

        public int GetMaxPay()
        {
            return Indebtedness + Percents + GetPercent() - PayedAmount;
        }
    }
}
