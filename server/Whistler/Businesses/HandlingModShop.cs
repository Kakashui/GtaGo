using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Businesses.Models;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.PriceSystem;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;

namespace Whistler.Businesses
{
    class HandlingModShop : Script
    {
        /// <summary>
        /// tuning prices
        /// </summary>
        public static Dictionary<HandlingKeys, TuningPartsFix> HandlingParts = new Dictionary<HandlingKeys, TuningPartsFix>()
        {
            {HandlingKeys.fMass,                           new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fInitialDragCoeff,               new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fDriveBiasFront,                 new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.nInitialDriveGears,              new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fInitialDriveForce,              new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fDriveInertia,                   new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fClutchChangeRateScaleUpShift,   new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fClutchChangeRateScaleDownShift, new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fInitialDriveMaxFlatVel,         new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fBrakeForce,                     new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fBrakeBiasFront,                 new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fHandBrakeForce,                 new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fSteeringLock,                   new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fTractionCurveMax,               new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fTractionCurveMin,               new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fTractionCurveLateral,           new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fTractionSpringDeltaMax,         new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fLowSpeedTractionLossMult,       new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fTractionBiasFront,              new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fTractionLossMult,               new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fSuspensionForce,                new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fSuspensionCompDamp,             new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fSuspensionReboundDamp,          new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fSuspensionUpperLimit,           new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fSuspensionLowerLimit,           new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fSuspensionRaise,                new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fSuspensionBiasFront,            new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fAntiRollBarForce,               new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fAntiRollBarBiasFront,           new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fRollCentreHeightFront,          new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.fRollCentreHeightRear,           new TuningPartsFix(2500, 5000, 0, 0.3F)},
            {HandlingKeys.ReduceGrip,                      new TuningPartsFix(2500, 5000, 0, 0.5F)},
        };
        public HandlingModShop()
        {
            if (Directory.Exists("interfaces"))
            {
                Dictionary<int, TuningPartsFix> handlingParts = new Dictionary<int, TuningPartsFix>();
                foreach (var part in HandlingParts)
                {
                    handlingParts.Add((int)part.Key, part.Value);
                }
                using (var w = new StreamWriter("interfaces/gui/src/configs/shops/handlingParts.js"))
                {
                    w.Write($"export default {JsonConvert.SerializeObject(handlingParts)}");
                }
            }
        }
        [Command("custmenu")]
        public static void RemoteEvent_testcars(PlayerGo player)
        {
            if (!Group.CanUseAdminCommand(player, "custmenu"))
                return;
            OpenShop(player, true);
        }
        public static void OpenShop(PlayerGo player, bool IsAdmin)
        {
            if (player.GetData<int>("BIZ_ID") == -1 && !IsAdmin)
                return;
            if (!player.IsInVehicle)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_123", 3000);
                return;
            }

            var veh = player.Vehicle;
            VehicleGo vehGo = veh.GetVehicleGo();

            int pricePart = 100;
            if (IsAdmin)
            {
                player.Character.BusinessInsideId = -2;
            }
            else
            {
                if (!vehGo.Data.CanAccessVehicle(player, AccessType.Tuning))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_123", 3000);
                    return;
                }
                Business biz = BusinessManager.GetBusiness(player.GetData<int>("BIZ_ID"));

                player.Character.BusinessInsideId = biz.ID;
                pricePart = biz.GetPriceByProductId(0).CurrentPrice;
                player.Vehicle.Dimension = Dimensions.RequestPrivateDimension();
            }
            var modelPrice = PriceManager.GetPriceInDollars(TypePrice.Car, vehGo.Data.ModelName, 5000000);
            player.TriggerEvent("handlingShop:openMenu", pricePart, modelPrice, IsAdmin && player.Character.AdminLVL >= 9);
        }

        [RemoteEvent("handlingShop:closeMenu")]
        public static void CloseHandlingMod(PlayerGo player)
        {
            if (player.Character.BusinessInsideId > 0)
            {
                Business biz = BusinessManager.GetBusiness(player.Character.BusinessInsideId);
                if (biz != null && biz.Type == 40)
                {
                    if (player.IsInVehicle)
                    {
                        player.Vehicle.Dimension = 0;
                        player.ChangePositionWithCar(biz.UnloadPoint + new Vector3(0, 0, 0.9), null, 1000);
                    }
                    else
                    {
                        player.Dimension = 0;
                        player.ChangePosition(biz.UnloadPoint + new Vector3(0, 0, 1));
                    }
                }
            }
            player.Character.BusinessInsideId = -1;
        }


        [RemoteEvent("handlingShop:buy")]
        public static void BuyHandlingMod(PlayerGo player, int key, object value) => BuyOrSellHandlingMod(player, key, value);
        [RemoteEvent("handlingShop:sell")]
        public static void BuyHandlingMod(PlayerGo player, int key) => BuyOrSellHandlingMod(player, key, null);
        public static void BuyOrSellHandlingMod(PlayerGo player, int key, object value)
        {
            if (!player.IsInVehicle || player.VehicleSeat != VehicleConstants.DriverSeat)
                return;
            if (!Enum.IsDefined(typeof(HandlingKeys), key))
                return;
            HandlingKeys handlingKey = (HandlingKeys)key;
            int bizID = player.Character.BusinessInsideId;
            var vehicle = player.Vehicle;
            var vehData = vehicle.GetVehicleGo().Data;
            if (vehData.VehCustomization.GetHandling(handlingKey) == value)
            {
                Notify.SendError(player, "shmodshop:isCurrValue");
                return;
            }
            if (bizID != -2)
            {
                if (!HandlingParts.ContainsKey(handlingKey))
                    return;
                string vehModel = BusinessManager.GetVehicleModel(vehicle);
                int priceInPart = HandlingParts[handlingKey].GetPrice(value == null ? -1 : 0, vehModel);
                Business biz = BusinessManager.GetBusiness(bizID);
                if (!BusinessManager.TakeProd(player, biz, player.Character,
                    new BuyModel("Parts", priceInPart, true, (cnt) => cnt), "Money_BuyTuning".Translate(vehData.ID), null))
                    return;
            }
            else if (!Group.CanUseAdminCommand(player, "custmenu")) return;
            VehicleCustomization.SetHandlingMod(vehicle, handlingKey, value);
            Notify.SendSuccess(player, "Biz_128");
            player.TriggerEvent("handlingShop:update", (int)handlingKey, value);
        }
    }
}
