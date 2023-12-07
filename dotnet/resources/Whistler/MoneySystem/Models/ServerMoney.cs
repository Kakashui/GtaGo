using System;
using System.Collections.Generic;
using System.Text;
using Whistler.MoneySystem.Interface;

namespace Whistler.MoneySystem.Models
{
    class ServerMoney : IMoneyOwner
    {
        public long IMoneyBalance { get { return long.MaxValue; } }

        public TypeMoneyAcc ITypeMoneyAcc { get; }

        public int IOwnerID { get; }
        public ServerMoney(TypeMoneyAcc iTypeMoneyAcc, int iOwnerID)
        {
            ITypeMoneyAcc = iTypeMoneyAcc;
            IOwnerID = iOwnerID;
        }

        public bool MoneyAdd(int amount)
        {
            return amount > 0;
        }

        public bool MoneySub(int amount, bool limit = false)
        {
            return amount > 0;
        }
    }
}
