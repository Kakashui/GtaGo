using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Whistler.Businesses.Models;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.SDK;
using Whistler.VehicleSystem;

namespace Whistler.Core
{
    class CarWash : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(CarWash));
        private static Dictionary<string, CarWashConfProd> _configs = new Dictionary<string, CarWashConfProd>
        {
            ["wash"]    = new CarWashConfProd(1, 0, true),
            ["wax"]     = new CarWashConfProd(5, 180, false),
            ["ceramic"] = new CarWashConfProd(10, 600, false),
        };
        public static void OpenCarWashMenu(Player player, Business biz)
        {
            if (!player.IsInVehicle)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_29", 3000);
                return;
            }
            var character = player.GetCharacter();
            if (character == null) return;
            character.BusinessInsideId = biz.ID;
            var price = biz.Products[0].Price;
            player.TriggerEvent("carwash::openMenu", biz.ID, _configs["wash"].CountProds * price, _configs["wax"].CountProds * price, _configs["ceramic"].CountProds * price);

        }
        [RemoteEvent("carwash:buy")]
        public static void Carwash_Pay(Player player, int payment, string keys)
        {
            var character = player.GetCharacter();
            if (character == null || character.BusinessInsideId == -1) return;
            Business biz = BusinessManager.GetBusiness(character.BusinessInsideId);
            character.BusinessInsideId = -1;
            if (!player.IsInVehicle)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_29", 3000);
                return;
            }
            if (player.VehicleSeat != VehicleConstants.DriverSeat)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_100", 3000);
                return;
            }
            var prods = JsonConvert.DeserializeObject<List<string>>(keys);
            bool isWash = false;
            int minuteDirtSafe = 0;


            List<BuyModel> buyModels = new List<BuyModel>();
            foreach (var prod in prods)
            {
                var confModel = _configs.GetValueOrDefault(prod);
                if (confModel == null)
                    continue;
                buyModels.Add(new BuyModel("Detergent", confModel.CountProds, true, 
                    (cnt) =>
                    {
                        isWash = isWash || confModel.IsWash;
                        minuteDirtSafe += confModel.MinuteEffect;
                        return cnt;
                    }));
            }

            if (BusinessManager.TakeProd(player, biz, player.GetMoneyPayment((PaymentsType)payment), buyModels, "Money_Carwash", null))
            {
                if (isWash)
                    VehicleStreaming.SetVehicleDirt(player.Vehicle, 0);
                if (minuteDirtSafe > 0 || isWash)
                    VehicleStreaming.SetVehicleDirtClear(player.Vehicle, minuteDirtSafe);
                Trigger.ClientEventInRange(player.Vehicle.Position, 20, "carwash::startEffect", player.Vehicle.Position);
            }
        }
    }
}
