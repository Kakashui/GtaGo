using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.MoneySystem.Interface
{
    interface IMoneyOwner
    {
        public int IOwnerID { get; }
        public long IMoneyBalance { get; }
        public TypeMoneyAcc ITypeMoneyAcc { get; }
        public bool MoneyAdd(int amount);
        public bool MoneySub(int amount, bool limit = false);
    }
}
