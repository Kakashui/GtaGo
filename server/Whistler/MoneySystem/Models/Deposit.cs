using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GTANetworkAPI;
using Whistler.Helpers;
using Whistler.MoneySystem.Interface;
using Whistler.MoneySystem.Models.Settings;
using Whistler.MoneySystem.Settings;

namespace Whistler.MoneySystem.Models
{
    [Table("efcore_bank_deposit")]
    class Deposit : IMoneyOwner
    {
        [Key]
        public int ID { get; set; }
        public int UUID { get; set; }
        public int BankId { get; set; }
        /// <summary>
        /// Денежные средства на счету
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// Суммарные проценты, начисленные на аккаунт
        /// </summary>
        public int Profit { get; set; }
        /// <summary>
        /// Первоначальная сумма (неснимаемый минимум)
        /// </summary>
        public int DepositMoney { get; set; }
        /// <summary>
        /// Пополнение счета
        /// </summary>
        public int AddedMoney { get; set; }
        /// <summary>
        /// Тип счета
        /// </summary>
        public DepositTypes DepositTypes { get; set; }
        /// <summary>
        /// Количество пройденных часов с открытия вклада (отыгранных на сервере)
        /// </summary>
        public int HoursInInterval { get; set; }
        /// <summary>
        /// Полное время депозита
        /// </summary>
        public int DepositFullTime { get; set; }
        /// <summary>
        /// Статус вклада
        /// </summary>
        public bool ClosedDeposit { get; set; }

        [NotMapped]
        public long IMoneyBalance { get { return Amount + AddedMoney; } }

        [NotMapped]
        public TypeMoneyAcc ITypeMoneyAcc { get { return TypeMoneyAcc.Deposit; } }

        [NotMapped]
        public int IOwnerID => ID;

        [NotMapped]
        internal DepositSettings Config
        {
            get
            {
                return BankSettings.GetDepositSetting(DepositTypes);
            }
        }
        [NotMapped]
        private double GivenPercent
        {
            get
            {
                return Config.AnnualRate / 100 * (BankSettings.IntervalCalculatePercent / 360.0);
            }
        }
        public Deposit()
        {

        }
        public Deposit(int uuid, int bankId, int amount, DepositTypes depositTypes, int days)
        {
            UUID = uuid;
            BankId = bankId;
            Amount = amount;
            Profit = 0;
            DepositMoney = Amount;
            AddedMoney = 0;
            DepositTypes = depositTypes;
            HoursInInterval = 0;
            DepositFullTime = days * 24;
            ClosedDeposit = false;
        }
        public void GivePercent()
        {
            HoursInInterval++;
            if (HoursInInterval % (BankSettings.IntervalCalculatePercent * 24) != 0)
                return;
            if (Config.Capitalization)
                Profit += (int)((Amount + Profit) * GivenPercent);
            else
                Profit += (int)(Amount * GivenPercent);
            Amount += AddedMoney;
            AddedMoney = 0;
            if (DepositFullTime <= HoursInInterval)
            {
                PayPercent();
                DepositTypes = DepositTypes.Default;
            }
        }

        public bool MoneyAdd(int amount)
        {
            if (amount <= 0)
                return false;
            if (!Config.IsRefill)
                return false;
            AddedMoney += amount;
            return true;
        }

        public bool MoneySub(int amount, bool limit = false)
        {
            if (amount <= 0)
                return false;
            if ((IMoneyBalance - amount != 0) && (IMoneyBalance - amount < DepositMoney || !Config.IsWithdraw))
                return false;
            if (AddedMoney > amount)
            {
                AddedMoney -= amount;
                return true;
            }
            else
            {
                amount -= AddedMoney;
                AddedMoney = 0;
            }
            if (Profit > amount)
            {
                Profit -= amount;
                return true;
            }
            else
            {
                amount -= Profit;
                Profit = 0;
            }
            if (Amount > amount)
            {
                Amount -= amount;
            }
            else
            {
                Amount = 0;
                CloseDeposit();
            }
            return true;

        }

        public void PayPercent()
        {
            Amount += Profit;
            Profit = 0;
        }

        public void CloseDeposit()
        {
            Wallet.TransferMoney(this, BankManager.GetAccountByUUID(UUID), (int)IMoneyBalance, 0, "Money_CloseDeposit");
            ClosedDeposit = true;
        }
    }
}
