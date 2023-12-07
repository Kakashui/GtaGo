using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.VehicleSystem;
using System.Linq;
using Whistler.VehicleSystem.Models;
using Whistler.Helpers;
using Whistler.VehicleSystem.Models.VehiclesData;
using Whistler.MoneySystem;
using Whistler.Businesses.Models;
using Whistler.PriceSystem;

namespace Whistler.Businesses
{
    class Autorepair : Script
    {

        private const int _priceCheapCar = 1000000;
        private const int _priceExpensiveCar = 4000000;
        private static Dictionary<PriceType, string> _engineOilProduct = new Dictionary<PriceType, string>()
        {
            { PriceType.Cheap, "Engine Oil Cheap" },
            { PriceType.Medium, "Engine Oil Medium" },
            { PriceType.Expensive, "Engine Oil Expensive" },
        };
        private static Dictionary<PriceType, string> _transmissionOilProduct = new Dictionary<PriceType, string>()
        {
            { PriceType.Cheap, "Transmission Oil Cheap" },
            { PriceType.Medium, "Transmission Oil Medium" },
            { PriceType.Expensive, "Transmission Oil Expensive" },
        };
        private static Dictionary<PriceType, string> _brakesProduct = new Dictionary<PriceType, string>()
        {
            { PriceType.Cheap, "Brakes Cheap" },
            { PriceType.Medium, "Brakes Medium" },
            { PriceType.Expensive, "Brakes Expensive" },
        };

        private static Dictionary<string, Action<Vehicle>> _repairActions = new Dictionary<string, Action<Vehicle>>()
        {
            { "body", VehicleManager.RepairBody },
            { "engine", VehicleManager.RepairEngine },
            { "engineOil", VehicleManager.EngineService },
            { "transmisOil", VehicleManager.TransmissionService },
            { "break", VehicleManager.BrakeService },
        };


        internal static void Pay(Player client)
        {
            try
            {
                if (client.GetData<int>("BIZ_ID") == -1) return;
                Business biz = BusinessManager.BizList[client.GetData<int>("BIZ_ID")];

                if (client.IsInVehicle)
                {
                    if (client.VehicleSeat == VehicleConstants.DriverSeat)
                    {
                        if (BusinessManager.TakeProd(client, biz, client.GetCharacter(), new BuyModel("Auto parts", 1, true, 
                            (cnt) =>
                            {
                                VehicleManager.RepairCar(client.Vehicle);
                                return cnt;
                            }), "Money_Autorepair", null))
                            Notify.Send(client, NotifyType.Success, NotifyPosition.BottomCenter, "Biz_86".Translate(), 3000);

                    }
                    else
                        Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_87".Translate(), 3000);
                }
                return;
            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
                return;
            }
        }

        public static void OpenRepairCarMenu(Player player)
        {
            if (!player.IsLogged())
                return;
            if (player.GetData<int>("BIZ_ID") == -1) return;
            Business biz = BusinessManager.BizList[player.GetData<int>("BIZ_ID")];
            if (!player.IsInVehicle || player.Vehicle == null)
                return;

            Vehicle vehicle = player.Vehicle;
            VehicleGo vehGo = vehicle.GetVehicleGo();
            if (!vehGo.IsWearable())
                return;
            var vData = vehGo.Data as PersonalBaseVehicle;

            player.GetCharacter().BusinessInsideId = biz.ID;
            PriceType carPriceType = GetTypeCar(vData.ModelName);
            int pricePart = GetPriceByProductName(biz, "Auto parts");
            var config = VehicleConfiguration.GetConfig(vData.ModelName);
            string name = config.DisplayName;
            Trigger.ClientEvent(player, "repair:openMenu",
                name,
                (float)(vData.Mileage / 1000),
                (float)((vData.Mileage - vData.MileageOilChange) / 1000),
                (float)((vData.Mileage - vData.MileageTransmissionService) / 1000),
                (float)((vData.Mileage - vData.MileageBrakePadsChange) / 1000),
                pricePart,
                GetPartsForRepair(vData.ModelName),
                GetPriceByProductName(biz, _engineOilProduct[carPriceType]),
                GetPriceByProductName(biz, _transmissionOilProduct[carPriceType]),
                GetPriceByProductName(biz, _brakesProduct[carPriceType]),
                biz.Peds.FirstOrDefault()?.Position);
        }

        [RemoteEvent("repair:closemenu")]
        public static void RemoteEvent_CloseRepairMenu(Player player)
        {
            if (!player.IsLogged())
                return;
            player.GetCharacter().BusinessInsideId = -1;
        }
        [RemoteEvent("repair:buy")]
        public static void RemoteEvent_RepairBuy(Player player, string state)
        {
            if (!player.IsLogged())
                return;
            if (player.GetCharacter().BusinessInsideId < 0)
                return;
            Business biz = BusinessManager.BizList[player.GetCharacter().BusinessInsideId];
            if (!player.IsInVehicle || player.Vehicle == null)
                return;
            Vehicle vehicle = player.Vehicle;
            VehicleGo vehGo = vehicle.GetVehicleGo();
            if (!vehGo.IsWearable())
                return;
            var vData = vehGo.Data as PersonalBaseVehicle;
            PriceType carPriceType = GetTypeCar(vData.ModelName);
            int countProd = 1;
            string PartName = "Auto parts";
            bool actualRepair = true;
            switch (state)
            {
                case "body":
                    int i = vData.DoorBreak;
                    while (i > 0)
                    {
                        countProd += i % 2;
                        i >>= 1;
                    }
                    break;
                case "engine":
                    countProd = GetPartsForRepair(vData.ModelName) * (int)Math.Round((1000-vData.EngineHealth)/10);
                    if (vData.EngineHealth >= 1000)
                        actualRepair = false;
                    break;
                case "engineOil":
                    PartName = _engineOilProduct[carPriceType];
                    if (vData.MileageOilChange >= vData.Mileage)
                        actualRepair = false;
                    break;
                case "transmisOil":
                    PartName = _transmissionOilProduct[carPriceType];
                    if (vData.MileageTransmissionService >= vData.Mileage)
                        actualRepair = false;
                    break;
                case "break":
                    PartName = _brakesProduct[carPriceType];
                    if (vData.MileageBrakePadsChange >= vData.Mileage)
                        actualRepair = false;
                    break;
                default:
                    return;
            }
            if (countProd <= 0 || !actualRepair)
            {
                string message = "Biz_165";
                switch (state)
                {
                    case "engineOil":
                        message = "Biz_162";
                        break;
                    case "transmisOil":
                        message = "Biz_163";
                        break;
                    case "break":
                        message = "Biz_164";
                        break;
                }
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, message.Translate(), 3000);
                return;
            }

            BusinessManager.TakeProd(player, biz, player.GetCharacter(), new BuyModel(PartName, countProd, true, 
                (cnt) =>
                {
                    _repairActions[state](vehicle);
                    SendActualInfo(player, biz, vData);
                    return cnt;
                }), "Money_Repair".Translate(state), null);
        }

        private static void SendActualInfo(Player player, Business biz, PersonalBaseVehicle vData)
        {
            PriceType carPriceType = GetTypeCar(vData.ModelName);
            int pricePart = GetPriceByProductName(biz, "Auto parts");
            Trigger.ClientEvent(player, "repair:updateInfo",
                (float)((vData.Mileage - vData.MileageOilChange) / 1000),
                (float)((vData.Mileage - vData.MileageTransmissionService) / 1000),
                (float)((vData.Mileage - vData.MileageBrakePadsChange) / 1000),
                pricePart,
                GetPartsForRepair(vData.ModelName),
                GetPriceByProductName(biz, _engineOilProduct[carPriceType]),
                GetPriceByProductName(biz, _transmissionOilProduct[carPriceType]),
                GetPriceByProductName(biz, _brakesProduct[carPriceType]));
        }

        private static int GetPriceByProductName(Business biz, string name)
        {
            var product = biz.Products.FirstOrDefault(p => p.Name == name);
            if (product != null)
                return product.Price;
            else
                return 0;
        }

        private static PriceType GetTypeCar(string model)
        {
            int price = PriceManager.GetPriceInDollars(TypePrice.Car, model, 1000000);
            if (price < _priceCheapCar)
                return PriceType.Cheap;
            else if (price >= _priceExpensiveCar)
                return PriceType.Expensive;
            else
                return PriceType.Medium;
        }
        private static int GetPartsForRepair(string model)
        {
            int priceRepairOnePercent = PriceManager.GetPriceInDollars(TypePrice.Car, model, 1000000) / 1000;

            return priceRepairOnePercent / 100;
        }
    }

    enum PriceType
    {
        Cheap = 0,
        Medium = 1,
        Expensive = 2,
    }
}


/*
[
{"Name":"Auto parts","OrderPrice":100,"MaxMinType":"$","MaxPrice":250,"MinPrice":70,"StockCapacity":500},
{"Name":"Brakes Cheap","OrderPrice":1000,"MaxMinType":"$","MaxPrice":2000,"MinPrice":700,"StockCapacity":500},
{"Name":"Engine Oil Cheap","OrderPrice":1500,"MaxMinType":"$","MaxPrice":2500,"MinPrice":1200,"StockCapacity":500},
{"Name":"Transmission Oil Cheap","OrderPrice":1500,"MaxMinType":"$","MaxPrice":2500,"MinPrice":1200,"StockCapacity":500},
{"Name":"Brakes Medium","OrderPrice":4000,"MaxMinType":"$","MaxPrice":8000,"MinPrice":2800,"StockCapacity":500},
{"Name":"Engine Oil Medium","OrderPrice":6000,"MaxMinType":"$","MaxPrice":10000,"MinPrice":4800,"StockCapacity":500},
{"Name":"Transmission Oil Medium","OrderPrice":6000,"MaxMinType":"$","MaxPrice":10000,"MinPrice":4800,"StockCapacity":500},
{"Name":"Brakes Expensive","OrderPrice":10000,"MaxMinType":"$","MaxPrice":20000,"MinPrice":7000,"StockCapacity":500},
{"Name":"Engine Oil Expensive","OrderPrice":15000,"MaxMinType":"$","MaxPrice":25000,"MinPrice":12000,"StockCapacity":500},
{"Name":"Transmission Oil Expensive","OrderPrice":15000,"MaxMinType":"$","MaxPrice":25000,"MinPrice":12000,"StockCapacity":500}
]
*/