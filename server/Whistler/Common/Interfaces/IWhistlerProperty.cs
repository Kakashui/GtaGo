using System;
using System.Collections.Generic;
using System.Text;
using Whistler.MoneySystem;

namespace Whistler.Common.Interfaces
{
    interface IWhistlerProperty
    {
        public int ID { get; }
        public bool Pledged { get; }
        public void SetPledged(bool status);
        public OwnerType OwnerType { get; }
        public PropertyType PropertyType { get; }
        public int CurrentPrice { get; }
        public int OwnerID { get; }
        public string PropertyName { get; }
        public void DeletePropertyFromMember();
    }
}
