using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.ServerConfiguration
{
    public class BonusConfig
    {
        public int Minutes { get; set; }
        public bool OneInDay { get; set; }
        public int Coins { get; set; }
        public int Money { get; set; }
    }
}
