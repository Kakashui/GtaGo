using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.GUI.Tips;
using Whistler.Helpers;
using Whistler.Phone.Taxi.Dtos;
using Whistler.SDK;

namespace Whistler.Phone.Taxi.Service.Models
{
    class TaxiOrderPlayer : TaxiOrderBase
    {
        public int PassengerUuid { get; set; }
        public TaxiOrderPlayer()
        {

        }

        public override void SendDriverData(ExtPlayer driver, DriverData driverData)
        {
            ExtPlayer client = Trigger.GetPlayerByUuid(PassengerUuid);
            if (client.IsLogged())
                client.TriggerCefEvent("smartphone/taxiPage/taxi_setDriverData", 
                    JsonConvert.SerializeObject(new DriverDataDto(driver.Character, driverData)));
        }

        public override bool CompleateOrder()
        {
            var passenger = Trigger.GetPlayerByUuid(PassengerUuid);
            if (!passenger.IsLogged())
                return false;

            if (passenger.Position.DistanceTo2D(Driver.Position) > 20)
                return false;
            Notify.Send(passenger, NotifyType.Success, NotifyPosition.Bottom, "taxi:service:1", 3000);
            passenger.TriggerCefAction("smartphone/taxiPage/taxi_goHome", true);
            return true;
        }

        public override void ArrivalToClient(DriverData driverData)
        {
            var passenger = Trigger.GetPlayerByUuid(PassengerUuid);
            passenger.TriggerCefAction("smartphone/taxiPage/taxi_setNotify", true);
            Tip.SendTipNotification(passenger, "taxi:service:2".Translate(driverData.CarModel, driverData.CarNumber));
        }
        public override void Destroy()
        {
            base.Destroy();
            Trigger.GetPlayerByUuid(PassengerUuid)?.TriggerCefAction("smartphone/taxiPage/taxi_goHome", true);
        }
        public override void DriverCancelOrder()
        {

            var passenger = Trigger.GetPlayerByUuid(PassengerUuid);

            var paymentType = IsCardPayment ? MoneySystem.PaymentsType.Card : MoneySystem.PaymentsType.Cash;
            MoneySystem.Wallet.MoneyAdd(passenger.GetMoneyPayment(paymentType), Sum, "Money_TaxiCancel");

            Notify.SendInfo(passenger, "taxi:service:3");
        }

    }
}
