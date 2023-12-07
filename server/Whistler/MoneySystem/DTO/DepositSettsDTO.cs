using System;
using System.Collections.Generic;
using System.Text;
using Whistler.MoneySystem.Models.Settings;

namespace Whistler.MoneySystem.DTO
{
    class DepositSettsDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int minDays { get; set; }
        public int maxDays { get; set; }
        public int minMoney { get; set; }
        public int maxMoney { get; set; }
        public float annualRate { get; set; }
        public bool isRefill { get; set; }
        public bool isWithdraw { get; set; }

        /// <summary>
        /// interval in hours
        /// </summary>
        public int IntervalGiveProcent { get; set; }
        public bool capitalization { get; set; }
        public string image { get; set; }
        public DepositSettsDTO(DepositSettings depositSettings)
        {
            id = (int)depositSettings.Id;
            title = depositSettings.Title;
            description = depositSettings.Description;
            minDays = depositSettings.MinDays;
            maxDays = depositSettings.MaxDays;
            minMoney = depositSettings.MinMoney;
            maxMoney = depositSettings.MaxMoney;
            annualRate = depositSettings.AnnualRate;
            isRefill = depositSettings.IsRefill;
            isWithdraw = depositSettings.IsWithdraw;
            capitalization = depositSettings.Capitalization;
            image = depositSettings.Image;

        }
    }
}
