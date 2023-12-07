using System;
using System.Collections.Generic;
using System.Text;
using Whistler.PersonalEvents.Contracts;
using Whistler.VehicleSystem.Models;

namespace Whistler.Jobs.HomeRobbery.Models
{
    class RobberyItem : VehicleItemBase
    {
        public RobberyItem(AbstractItemNames abstractItem, int count)
        {
            AbstractItem = abstractItem;
            Count = count;
        }
        public override int GetWeight()
        {
            return Count * 15000;
        }

        public override bool IsActual()
        {
            return true;
        }

        public override bool IsEqual(VehicleItemBase vehicleItemBase)
        {
            return vehicleItemBase is RobberyItem && vehicleItemBase.AbstractItem == AbstractItem;
        }
    }
}
