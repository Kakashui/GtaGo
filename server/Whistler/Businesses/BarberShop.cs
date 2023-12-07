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

        public static void OpenBarberShop(Player player, Business biz)
        {
            if (player.GetCharacter().OnDuty || player.GetData<bool>("ON_WORK"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Biz_25", 3000);
                return;
            }
            player.GetCharacter().BusinessInsideId = biz.ID;
            player.GetCharacter().ExteriorPos = player.Position;
            player.Dimension = Dimensions.RequestPrivateDimension();
            player.ChangePosition(new Vector3(138.3647, -1709.252, 29.5));
            Trigger.ClientEvent(player, "openBarber", biz.Products[0].Price);
        }

        [RemoteEvent("closeBarber")]
        public static void RemoteEvent_cancelBarber(Player player)
        {
            try
            {
                Business biz = BizList.GetOrDefault(player.GetCharacter().BusinessInsideId);
                player.GetCharacter().BusinessInsideId = -1;
                player.GetCustomization().Apply(player);
                player.Dimension = 0;
                player.ChangePosition(biz?.EnterPoint + new Vector3(0, 0, 1.12));
                player.GetCharacter().ExteriorPos = null;
            }
            catch (Exception e) { _logger.WriteError("closeBarber: " + e.ToString()); }
        }

        [RemoteEvent("buyBarber")]
        public static void RemoteEvent_buyBarber(Player player, string type, int style, int color)
        {
            try
            {
                _logger.WriteDebug($"buyBarber: id - {type} | style - {style} | color - {color}");

                Business biz = BizList.GetOrDefault(player.GetCharacter().BusinessInsideId);

                //Console.WriteLine($"{type}/{style}/{color}");

                //if ((type == "pomade" || type == "blush" || type == "shadows") && player.GetCharacter().Gender && style != 255)
                //{
                //    player.TriggerEvent("buyBarberCallback", 1);
                //    return;
                //}
                var tempPrice = BarberPrices.ContainsKey(type) ? BarberPrices[type] : _defaultPrice;
                var prodModel = biz.GetProductPrice("Hairs", tempPrice);

                if (player.GetCharacter().Money < prodModel.Price)
                {
                    player.TriggerEvent("buyBarberCallback", 2);
                    return;
                }

                if (!BusinessManager.TakeProd(player, biz, player.GetCharacter(), new BuyModel("Hairs", prodModel.MaterialsAmount, true, 
                    (cnt) =>
                    {
                        switch (type)
                        {
                            case "hairstyle":
                                player.GetCustomization().UpdateHairsModel(player, style, color, color);//.Hair = new HairData(style, color, color);
                                break;
                            case "beard":
                                player.GetCustomization().EditHeadOverlay(player, 1, style, color);
                                break;
                            case "eyebrows":
                                player.GetCustomization().EditHeadOverlay(player, 2, style, color);
                                break;
                            case "torso":
                                player.GetCustomization().EditHeadOverlay(player, 10, style, color);
                                break;
                            case "lenses":
                                player.GetCustomization().EditEyes(player, style);
                                break;
                            case "pomade":
                                player.GetCustomization().EditHeadOverlay(player, 8, style, color);
                                break;
                            case "blush":
                                player.GetCustomization().EditHeadOverlay(player, 5, style, color);
                                break;
                            case "shadows":
                                player.GetCustomization().EditHeadOverlay(player, 4, style, color);
                                break;
                        }
                        player.TriggerEvent("buyBarberCallback", 4, Convert.ToInt32(prodModel.Price));
                        return cnt;
                    }), "Money_BuyBarber", null))
                {
                    player.TriggerEvent("buyBarberCallback", 3);
                }
            }
            catch (Exception e) {
                _logger.WriteError("BuyBarber: " + e.ToString());
                player.TriggerEvent("buyBarberCallback", 5);
            }

        }
    }
}
