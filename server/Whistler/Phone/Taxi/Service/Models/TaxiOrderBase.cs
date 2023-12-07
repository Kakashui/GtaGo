using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.Phone.Taxi.Service.Models
{
    abstract class TaxiOrderBase
    {        
        public int ID { get; set; }

        public DateTime CreateDate { get; set; }

        public int? DriverUuid { get; set; }

        private Player _driver;
        public Player Driver => _driver ?? (_driver = Main.GetPlayerByUUID((int)DriverUuid));

        public Vector3 Start { get; set; }

        public Vector3 Destination { get; set; }

        public ColShape Colshape { get; set; }
        public bool IsCardPayment { get; set; }

        public int Sum { get; set; }

        public virtual void Destroy() 
        {
            if (Colshape != null)
            {
                Colshape.Delete();
            }
        }

        public abstract void SendDriverData(Player driver, DriverData driverData);
        public abstract bool CompleateOrder();
        public abstract void ArrivalToClient(DriverData driverData);
        public abstract void DriverCancelOrder();

    }
}
