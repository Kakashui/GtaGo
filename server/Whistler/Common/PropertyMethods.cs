using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Common.Interfaces;
using Whistler.Core;
using Whistler.Houses;
using Whistler.MoneySystem;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.Common
{
    class PropertyMethods
    {
        public static IWhistlerProperty GetWhistlerPropertyByName(PropertyType propertyType, int id)
        {
            switch (propertyType)
            {
                case PropertyType.Vehicle:
                    return VehicleManager.GetVehicleBaseByUUID(id) as PersonalBaseVehicle;
                case PropertyType.Business:
                    return BusinessManager.GetBusiness(id);
                case PropertyType.House:
                    return HouseManager.GetHouseById(id);
                default:
                    return null;
            }
        }
    }
}
