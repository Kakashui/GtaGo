using System;
using System.Collections.Generic;
using System.Text;
using Whistler.PersonalEvents.Contracts;

namespace Whistler.VehicleSystem.Models
{
    abstract class VehicleItemBase
    {
        public AbstractItemNames AbstractItem { get; set; }
        public int Count { get; set; }
        public abstract int GetWeight();
        public abstract bool IsActual();
        public abstract bool IsEqual(VehicleItemBase vehicleItemBase);

    }
}
