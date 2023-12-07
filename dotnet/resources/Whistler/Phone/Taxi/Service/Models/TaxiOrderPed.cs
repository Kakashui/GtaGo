using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.Phone.Taxi.Service.Models
{
    class TaxiOrderPed : TaxiOrderBase
    {
        public override void SendDriverData(ExtPlayer driver, DriverData driverData)
        {
            SafeTrigger.ClientEvent(driver, "phone:taxijob:createPed", Start);
        }
        public override bool CompleateOrder()
        {
            SafeTrigger.ClientEvent(Driver, "phone:taxijob:pedLeaveVehicle", Destination);        
            return true;
        }
        public override void ArrivalToClient(DriverData driverData)
        {
            SafeTrigger.ClientEvent(Driver, "phone:taxijob:pedEnterVehicle");
        
        }
        public override void DriverCancelOrder()
        {
            SafeTrigger.ClientEvent(Driver, "phone:taxijob:destroyPed");
        
        }

    }
}
