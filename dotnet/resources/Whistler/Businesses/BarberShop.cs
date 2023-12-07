using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Whistler.GUI;
using Whistler.MoneySystem;
using Whistler.SDK;
using Whistler.Businesses;
using Whistler.Houses;
using Whistler.Core;
using Whistler.Helpers;
using AutoMapper.Internal;
using Whistler.Businesses.Models;
using Whistler.Entities;

namespace Whistler.Core
{
    partial class BusinessManager : Script
    {
        private static Dictionary<string, int> BarberPrices = new Dictionary<string, int>()
        {
            { "hairstyle", 3000},
            { "eyebrows", 500},
            { "torso", 1000},
            { "lenses", 5000},
            { "pomade", 500},
            { "blush", 1500},
            { "shadows", 10000},
            { "beard", 5000}
        };


        private static int _defaultPrice = 10000;

        public static void OpenBarberShop(ExtPlayer player, Business biz)
        {
            if (player.Character.OnDuty || player.Session.OnWork)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы должны закончить рабочий день", 3000);
                return;
            }
            player.Character.BusinessInsideId = biz.ID;
            player.Character.ExteriorPos = player.Position;
            SafeTrigger.UpdateDimension(player,  Dimensions.RequestPrivateDimension());
            player.ChangePosition(new Vector3(138.3647, -1709.252, 29.5));
            SafeTrigger.ClientEvent(player, "openBarber", 
                biz.Products[0].Price, 
                JsonConvert.SerializeObject(new { money = player.GetMoneyPayment(PaymentsType.Cash).IMoneyBalance, bank = player.GetMoneyPayment(PaymentsType.Card).IMoneyBalance })
            );
        }

        [RemoteEvent("closeBarber")]
        public static void RemoteEvent_cancelBarber(ExtPlayer player)
        {
            try
            {
                Business biz = BizList.GetOrDefault(player.Character.BusinessInsideId);
                player.Character.BusinessInsideId = -1;
                player.Character.Customization.Apply(player);
                SafeTrigger.UpdateDimension(player,  0);
                player.ChangePosition(biz?.EnterPoint + new Vector3(0, 0, 1.12));
                player.Character.ExteriorPos = null;
            }
            catch (Exception e) { _logger.WriteError("closeBarber: " + e.ToString()); }
        }

        [RemoteEvent("buyBarber")]
        public static void RemoteEvent_buyBarber(ExtPlayer player, string type, int style, int color, bool cashPay)
        {
            try
            {
                // _logger.WriteDebug($"buyBarber: id - {type} | style - {style} | color - {color}");

                Business biz = BizList.GetOrDefault(player.Character.BusinessInsideId);

                //Console.WriteLine($"{type}/{style}/{color}");

                //if ((type == "pomade" || type == "blush" || type == "shadows") && player.Character.Gender && style != 255)
                //{
                //    SafeTrigger.ClientEvent(player,"buyBarberCallback", 1);
                //    return;
                //}
                var tempPrice = BarberPrices.ContainsKey(type) ? BarberPrices[type] : _defaultPrice;
                var prodModel = biz.GetProductPrice("Hairs", tempPrice);

                // bool cashPay = false;
                // if(cashtype == "cash") cashPay = true;
                // else cashPay = false;
                // if (player.Character.Money < prodModel.Price)
                // {
                //     SafeTrigger.ClientEvent(player,"buyBarberCallback", 2);
                //     return;
                // }
                //player.GetMoneyPayment(cashPay ? PaymentsType.Cash : PaymentsType.Card)

                // NAPI.Util.ConsoleOutput($"{cashtype} cashtype");

                if (!BusinessManager.TakeProd(player, biz, player.GetMoneyPayment(cashPay ? PaymentsType.Cash : PaymentsType.Card), new BuyModel("Hairs", prodModel.MaterialsAmount, true, 
                    (cnt) =>
                    {
                        switch (type)
                        {
                            case "hairstyle":
                                player.Character.Customization.UpdateHairsModel(player, style, color, color);//.Hair = new HairData(style, color, color);
                                break;
                            case "beard":
                                player.Character.Customization.EditHeadOverlay(player, 1, style, color);
                                break;
                            case "eyebrows":
                                player.Character.Customization.EditHeadOverlay(player, 2, style, color);
                                break;
                            case "torso":
                                player.Character.Customization.EditHeadOverlay(player, 10, style, color);
                                break;
                            case "lenses":
                                player.Character.Customization.EditEyes(player, style);
                                break;
                            case "pomade":
                                player.Character.Customization.EditHeadOverlay(player, 8, style, color);
                                break;
                            case "blush":
                                player.Character.Customization.EditHeadOverlay(player, 5, style, color);
                                break;
                            case "shadows":
                                player.Character.Customization.EditHeadOverlay(player, 4, style, color);
                                break;
                        }
                        SafeTrigger.ClientEvent(player,"buyBarberCallback", 4, Convert.ToInt32(prodModel.Price));
                        return cnt;
                    }), "Money_BuyBarber", null))
                {
                    SafeTrigger.ClientEvent(player,"buyBarberCallback", 3);
                }
            }
            catch (Exception e) {
                _logger.WriteError("BuyBarber: " + e.ToString());
                SafeTrigger.ClientEvent(player,"buyBarberCallback", 5);
            }

        }
    }
}
