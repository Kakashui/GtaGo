using System;
using System.Collections.Generic;
using System.Text;
using Whistler.VehicleSystem.Models;

namespace Whistler.PersonalEvents.Contracts.Models
{
    class ContractItem : VehicleItemBase
    {
        public ContractProgress ContractProgress { get; set; }
        public DateTime ContractDate { get; set; }
        public ContractItem(ContractProgress cntractProgress, AbstractItemNames abstractItem, DateTime contractDate, int count)
        {
            AbstractItem = abstractItem;
            ContractProgress = cntractProgress;
            ContractDate = contractDate;
            Count = count;
        }
        public override bool IsActual()
        {
            return ContractProgress.InProgress && ContractProgress.ExpirationDate == ContractDate;
        }
        public bool IsEqual(ContractProgress contractProgress, AbstractItemNames abstractItem, DateTime expirationDate)
        {
            return ContractProgress == contractProgress && AbstractItem == abstractItem && ContractDate == expirationDate;
        }
        public override bool IsEqual(VehicleItemBase vehicleItemBase)
        {
            ContractItem contractItem = vehicleItemBase as ContractItem;
            if (contractItem != null)
                return IsEqual(contractItem.ContractProgress, contractItem.AbstractItem, contractItem.ContractDate);
            else
                return false;
        }

        public override int GetWeight()
        {
            return ContractSettings.GetAbstractItemConfig(AbstractItem).Weight * Count;
        }
    }
}
