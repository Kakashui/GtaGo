using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.MoneySystem.Models.Settings
{
    class DepositSettings
    {
        public DepositTypes Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MinDays { get; set; }
        public int MaxDays { get; set; }
        public int MinMoney { get; set; }
        public int MaxMoney { get; set; }
        public float AnnualRate { get; set; }
        public bool IsRefill { get; set; }
        public bool IsWithdraw { get; set; }
        public bool Capitalization { get; set; }
        public string Image { get; set; }
        public bool Actual { get; set; }
    }
}
