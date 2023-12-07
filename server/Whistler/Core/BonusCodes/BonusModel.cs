using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Core.BonusCodes
{
    class BonusModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Timeout { get; set; }
        public int Money { get; set; }
        public int Coins { get; set; }
        public int PrimeDays { get; set; }
        public BonusModel(int id, string name, DateTime timeout, int money, int coins, int primeDays)
        {
            ID = id;
            Name = name;
            Timeout = timeout;
            Money = money;
            Coins = coins;
            PrimeDays = primeDays;
        }
    }
}
