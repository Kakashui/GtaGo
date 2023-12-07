using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.Phone.Taxi.Service.Models
{
    class TaxiOrderPed : TaxiOrderBase
    {
        public override void SendDriverData(Player driver, DriverData driverData)
        {
            driver.TriggerEvent("phone:taxijob:createPed", Start);
        }
        public override bool CompleateOrder()
        {
            Driver?.TriggerEvent("phone:taxijob:pedLeaveVehicle", Destination);        
            return true;
        }
        public override void ArrivalToClient(DriverData driverData)
        {
            Driver?.TriggerEvent("phone:taxijob:pedEnterVehicle");
        
        }
        public override void DriverCancelOrder()
        {
            Driver?.TriggerEvent("phone:taxijob:destroyPed");
        
        }

    }
}
