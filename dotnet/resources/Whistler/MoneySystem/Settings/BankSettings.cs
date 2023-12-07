using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.MoneySystem.Models.Settings;

namespace Whistler.MoneySystem.Settings
{
    class BankSettings : Script
    {
        public const int IntervalCalculatePercent = 5;
        public const int CreditFastMaxAmount = 200000;
        public const int CreditFastMinLevel = 10;
        public const int PhonePayed = 50;
        public const int TransferLimitInDay = 1000000;
        private static Dictionary<int, float> _percentByDays = new Dictionary<int, float>
        {
            [0] = 30,
            [10] = 25,
            [20] = 24F,
            [30] = 23F,
            [40] = 22F,
            [50] = 21F,
            [60] = 20.5F,
            [70] = 20F,
            [80] = 20F,
            [90] = 19.5F,
            [100] = 19F,
            [110] = 18.5F,
            [120] = 18F,
        };
        private static Dictionary<int, float> _percentByAmount = new Dictionary<int, float>
        {
            [0] = 30,
            [100000] = 25,
            [500000] = 24.5F,
            [1000000] = 24F,
            [2000000] = 23.5F,
            [4000000] = 23F,
            [6000000] = 22.5F,
            [8000000] = 22F,
            [10000000] = 21.5F,
            [12000000] = 21F,
            [15000000] = 20.5F,
            [20000000] = 20F,
            [30000000] = 19.5F,
        };
        private static readonly Dictionary<DepositTypes, DepositSettings> _depositSetts = new Dictionary<DepositTypes, DepositSettings>
        {
            [DepositTypes.Default] = new DepositSettings
            {
                Id = DepositTypes.Default,
                Title = "bank:deposit:name0",
                Description = "bank:deposit:desc0",
                MinDays = 1,
                MaxDays = 99999,
                MinMoney = 1000,
                MaxMoney = 2000000000,
                AnnualRate = 0.1F,
                IsRefill = true,
                IsWithdraw = true,
                Capitalization = false,
                Image = "deposit-0",
                Actual = false,
            },
            [DepositTypes.BlessAndSave] = new DepositSettings
            {
                Id = DepositTypes.BlessAndSave,
                Title = "bank:deposit:name1",
                Description = "bank:deposit:desc1",
                MinDays = 30,
                MaxDays = 360,
                MinMoney = 100000,
                MaxMoney = 20000000,
                AnnualRate = 6.2F,
                IsRefill = true,
                IsWithdraw = false,
                Capitalization = false,
                Image = "deposit-1",
                Actual = true,
            },
            [DepositTypes.Profitable] = new DepositSettings
            {
                Id = DepositTypes.Profitable,
                Title = "bank:deposit:name2",
                Description = "bank:deposit:desc2",
                MinDays = 60,
                MaxDays = 360,
                MinMoney = 1000000,
                MaxMoney = 50000000,
                AnnualRate = 8.2F,
                IsRefill = false,
                IsWithdraw = false,
                Capitalization = false,
                Image = "deposit-2",
                Actual = true,
            },
            [DepositTypes.Universal] = new DepositSettings
            {
                Id = DepositTypes.Universal,
                Title = "bank:deposit:name3",
                Description = "bank:deposit:desc3",
                MinDays = 60,
                MaxDays = 360,
                MinMoney = 500000,
                MaxMoney = 50000000,
                AnnualRate = 4F,
                IsRefill = true,
                IsWithdraw = true,
                Capitalization = false,
                Image = "deposit-3",
                Actual = true,
            },
        };
        public BankSettings()
        {
            if (Directory.Exists("interfaces/gui/src/configs/bank"))
            {
                using var file = new StreamWriter("interfaces/gui/src/configs/bank/deposits.js");
                {
                    file.Write($"export default {JsonConvert.SerializeObject(_depositSetts.Values)}");
                }
            };
        }
        public static DepositSettings GetDepositSetting(DepositTypes type)
        {
            return _depositSetts.GetValueOrDefault(type);
        }

        public static float GetPercentByAmount(int amount)
        {
            return _percentByAmount.Where(item => item.Key <= amount).Last().Value;
        }
        public static float GetPercentByDays(int days)
        {
            return _percentByDays.Where(item => item.Key <= days).Last().Value;
        }
        public static float GetPercentByParams(int days, int amount, bool pledge, CreditTypePayment creditTypePayment)
        {
            return GetPercentByAmount(amount) + GetPercentByDays(days) + (!pledge ? 2 : 0) + (creditTypePayment == CreditTypePayment.Differentiated ? 0.5F : 0);
        }
    }
}
