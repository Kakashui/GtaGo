using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.MoneySystem.Interface
{
    interface IBankAccount : IMoneyOwner
    {
        public TypeBankAccount ITypeBank { get; }
    }
}
