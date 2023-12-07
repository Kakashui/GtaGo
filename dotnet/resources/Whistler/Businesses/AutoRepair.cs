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
using Whistler.Entities;

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

        private static Dictionary<string, Action<ExtVehicle>> _repairActions = new Dictionary<string, Action<ExtVehicle>>()
        {
            { "body", VehicleManager.RepairBody },
            { "engineOil", VehicleManager.EngineService },
            { "transmisOil", VehicleManager.TransmissionService },
            { "break", VehicleManager.BrakeService },
        };


        internal static void Pay(ExtPlayer client)
        {
            try
            {
                if (client.GetData<int>("BIZ_ID") == -1) return;
                Business biz = BusinessManager.BizList[client.GetData<int>("BIZ_ID")];

                if (client.IsInVehicle)
                {
                    if (client.VehicleSeat == VehicleConstants.DriverSeat)
                    {
                        if (BusinessManager.TakeProd(client, biz, client.Character, new BuyModel("Auto parts", 1, true, 
                            (cnt) =>
                            {
                                VehicleManager.RepairCar(client.Vehicle as ExtVehicle);
                                return cnt;
                            }), "Ремонт авто", null))
                            Notify.Send(client, NotifyType.Success, NotifyPosition.BottomCenter, "Ваша машина отремонтирована", 3000);

                    }
                    else
                        Notify.Send(client, NotifyType.Error, NotifyPosition.BottomCenter, "Починить транспорт может только водитель", 3000);
                }
                return;
            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput(e.ToString());
                return;
            }
        }

        public static void OpenRepairCarMenu(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            if (player.GetData<int>("BIZ_ID") == -1) return;
            Business biz = BusinessManager.BizList[player.GetData<int>("BIZ_ID")];
            if (!player.IsInVehicle || player.Vehicle == null)
                return;

            ExtVehicle vehicle = player.Vehicle as ExtVehicle;
            if (vehicle == null) return;
            if (!vehicle.IsWearable()) return;

            var vData = vehicle.Data as PersonalBaseVehicle;

            player.Character.BusinessInsideId = biz.ID;
            PriceType carPriceType = GetTypeCar(vData.ModelName);
            int pricePart = GetPriceByProductName(biz, "Auto parts");
            var config = VehicleConfiguration.GetConfig(vData.ModelName);
            string name = config.DisplayName;
            SafeTrigger.ClientEvent(player, "repair:openMenu",
                name,
                (float)(vData.Mileage / 1000),
                (float)((vData.Mileage - vData.MileageOilChange) / 1000),
                (float)((vData.Mileage - vData.MileageTransmissionService) / 1000),
                (float)((vData.Mileage - vData.MileageBrakePadsChange) / 1000),
                pricePart,
                GetPricePerPercentForRepair(vData.ModelName),
                GetPriceByProductName(biz, _engineOilProduct[carPriceType]),
                GetPriceByProductName(biz, _transmissionOilProduct[carPriceType]),
                GetPriceByProductName(biz, _brakesProduct[carPriceType]),
                biz.Peds.FirstOrDefault()?.Position);
        }

        [RemoteEvent("repair:closemenu")]
        public static void RemoteEvent_CloseRepairMenu(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            player.Character.BusinessInsideId = -1;
        }
        [RemoteEvent("repair:buy")]
        public static void RemoteEvent_RepairBuy(ExtPlayer player, string state)
        {
            if (!player.IsLogged())
                return;
            if (player.Character.BusinessInsideId < 0)
                return;
            Business biz = BusinessManager.BizList[player.Character.BusinessInsideId];
            if (!player.IsInVehicle || player.Vehicle == null)
                return;
            ExtVehicle vehicle = player.Vehicle as ExtVehicle;
            if (vehicle == null) return;

            if (!vehicle.IsWearable()) return;
            var vData = vehicle.Data as PersonalBaseVehicle;
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
                    if (vData.EngineHealth >= 1000) actualRepair = false;
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
                string message = "Двигатель не нуждается в ремонте";
                switch (state)
                {
                    case "engineOil":
                        message = "Моторное масло не нуждается в замене";
                        break;
                    case "transmisOil":
                        message = "Трансмиссионное масло не нуждается в замене";
                        break;
                    case "break":
                        message = "Тормозные колодки не нуждается в замене";
                        break;
                }
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, message, 3000);
                return;
            }

            if (state == "engine")
            {
                int engineHealth = (int)Math.Round(vData.EngineHealth / 10);
                int price = GetPricePerPercentForRepair(vData.ModelName) * (100 - engineHealth);
                if (price <= 0)
                {
                    Notify.SendError(player, "Произошла ошибка при подсчёте цены за обслуживание двигателя. Обратитесь к администрации.");
                    return;
                }
                if (Wallet.TransferMoney(player.Character, MoneyManager.ServerMoney, price, 0, $"Обслуживание авто (Двигатель)"))
                {
                    VehicleManager.RepairEngine(vehicle);
                    SendActualInfo(player, biz, vData);
                    Notify.SendSuccess(player, $"Вы успешно обслужили двигатель за ${price}.");
                    return;
                }
                Notify.SendError(player, $"К сожалению, у Вас не хватает денег на руках для починки двигателя (${price}).");
                return;
            }

            BusinessManager.TakeProd(player, biz, player.Character, new BuyModel(PartName, countProd, true, 
            (cnt) =>
            {
                _repairActions[state](vehicle);
                SendActualInfo(player, biz, vData);
                return cnt;
            }), $"Обслуживание авто ({state})", null);
        }

        private static void SendActualInfo(ExtPlayer player, Business biz, PersonalBaseVehicle vData)
        {
            PriceType carPriceType = GetTypeCar(vData.ModelName);
            int pricePart = GetPriceByProductName(biz, "Auto parts");
            SafeTrigger.ClientEvent(player, "repair:updateInfo",
                (float)((vData.Mileage - vData.MileageOilChange) / 1000),
                (float)((vData.Mileage - vData.MileageTransmissionService) / 1000),
                (float)((vData.Mileage - vData.MileageBrakePadsChange) / 1000),
                pricePart,
                GetPricePerPercentForRepair(vData.ModelName),
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

            if (price < _priceCheapCar) return PriceType.Cheap;
            else if (price >= _priceExpensiveCar) return PriceType.Expensive;
            return PriceType.Medium;
        }

        private static int GetPricePerPercentForRepair(string model)
        {
            int vehiclePrice = PriceManager.GetPriceInDollars(TypePrice.Car, model, 0);
            if (vehiclePrice <= 0) return 0;

            return vehiclePrice / 1000;
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