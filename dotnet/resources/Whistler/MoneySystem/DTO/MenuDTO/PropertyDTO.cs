using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Common;
using Whistler.Common.Interfaces;
using Whistler.Core;
using Whistler.Houses;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.MoneySystem.DTO.MenuDTO
{
    class PropertyDTO
    {
        public string type { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public bool familyProperty { get; set; }
        public int price { get; set; }
        public bool isPledged { get; set; }
        public PropertyDTO(IWhistlerProperty property)
        {
            id = property.ID;
            price = property.CurrentPrice;
            isPledged = property.Pledged;
            familyProperty = property.OwnerType == OwnerType.Family;
            type = property.PropertyType.ToString();
            name = property.PropertyName;
        }
    }
}
