using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.ServerConfiguration
{
    public class MainConfig
    {
        public int ServerNumber { get; set; }
        public string Weather { get; set; }
        public bool SocialClubCheck { get; set; }
        public string ServerName { get; set; }
        public bool VOIPEnabled { get; set; }
        public bool RemoteControl { get; set; }
        public bool DonateChecker { get; set; }
        public bool Donation_Sale { get; set; }
        public int PaydayMultiplier { get; set; }
        public int ExpMultiplier { get; set; }
        public bool SCLog { get; set; }
        public bool BizTax { get; set; }
        public bool HousesTax { get; set; }
        public bool GangsPay { get; set; } 
        public int GangsIncome { get; set; }
        public int MafiaIncome { get; set; }
        public bool SendClientExceptions { get; set; }

    }
}
