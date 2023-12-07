using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.NewDonateShop.Configs
{
    public class WalletConfig
    {
        //public bool Debug { get; set; }
        //public string UnitUrl { get; set; }
        //public string UnitPublicyKey { get; set; }
        //public string UnitSecretKey { get; set; }
        //public string UnitUri { get; set; }
        //public string UnitCurrency { get; set; }
        //public int UnitPort { get; set; }

        public string Url { get; set; }
        public int Port { get; set; }
        public int Project { get; set; }
        public string Secret1 { get; set; }
        public string Currency { get; set; }
    }
}
