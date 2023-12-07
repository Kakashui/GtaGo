using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Businesses;
using Whistler.Core;
using Whistler.Core.QuestPeds;
using Whistler.Domain.Phone.Messenger;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.SDK;
using Account = Whistler.Core.nAccount.Account;

namespace Whistler.Docks
{
    internal class Dock : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Dock));
        public static List<Order> CurrentOrders = new List<Order>();
        public static List<Vector3> TerminalPositions = new List<Vector3>
        {
            new Vector3(798.7647, -2959.05, 4.943324),
            new Vector3(798.6766, -2957.161, 4.943322),
            new Vector3(805.4302, -2965.01, 4.943324),
            new Vector3(805.4901, -2966.806, 4.943324),
            new Vector3(812.0761, -2971.42, 4.943325),
            new Vector3(814.7571, -2968.782, 4.943324),
            new Vector3(811.5248, -2960.333, 4.943324)
        };
        
        public Dock()
        {
            foreach (var terminalPosition in TerminalPositions)
            {
                InteractShape.Create(terminalPosition, 1f, 3)
                    .AddInteraction(OpenPortOrderMenuForPlayer, "questpeds_2");
            }
            NAPI.Blip.CreateBlip(356, TerminalPositions[2], 1.2f, 68, Main.StringToU16("Терминал"), 255, 0, true, 0, 0);            
        }

        [RemoteEvent("dockOrder:playerOrdered")]
        public void OnPlayerOrderedProducts(Player player, string data)
        {
            try
            {
                var items = JsonConvert.DeserializeObject<List<ConfirmedOrderDTO>>(data);
                if (items == null || !items.Any()) return;
                var biz = player.GetBusiness();
                var bizSettings = biz.TypeModel.Products;
                var totalCost = 0;
                var products = new List<(string, int)>();
                foreach (var item in items)
                {
                    var product = bizSettings[item.Id];
                    if (item.Count + 
                        biz.Products.FirstOrDefault(p => string.Equals(p.Name, product.Name, StringComparison.CurrentCultureIgnoreCase))?.Lefts + 
                        biz.Orders.Sum(item => item.Products.Sum(prod => string.Equals(prod.Item1, product.Name, StringComparison.CurrentCultureIgnoreCase) ? prod.Item2 : 0))
                        > product.StockCapacity)
                    {
                        Notify.Send(player, NotifyType.Alert, NotifyPosition.Top, "Gui_21", 3000);
                        return;
                    }
                    totalCost += product.OrderPrice * item.Count;
                    products.Add((product.Name, item.Count));
                }

                if (player.GetCharacter().Money < totalCost)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.Center, "Core_178", 3000);
                    return;
                }
                
                var newOrder = new Order(products, biz);
                CreateOrder(newOrder);
                Wallet.MoneySub(player.GetCharacter(), totalCost, "Money_DockOrder");
                Notify.Send(player, NotifyType.Success, NotifyPosition.Bottom, "PortOrders_24".Translate(), 3000);
                player.SendTip("tip_order_prods");
            }
            catch (Exception ex) 
            { 
                _logger.WriteError("DockOrder" + ex);
            }
        }

        public static void CreateOrder(Order order)
        {
            order.Customer.Orders.Add(order);
            CurrentOrders.Add(order);
        }

        private static void OpenPortOrderMenuForPlayer(Player player)
        {
            try
            {
                var biz = player.GetBusiness();
                if (biz == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.Top, "Money_7", 3000);
                    return;
                }

                var bizSettings = biz.TypeModel.Products;
                var list = bizSettings.Select(setting =>
                    new DockOrderDTO
                    {
                        Id = bizSettings.IndexOf(setting),
                        Cost = setting.OrderPrice,
                        Name = setting.Name,
                        Description = ""
                    }).ToList();
                var data = new
                {
                    productsList = list,
                    buissName = biz.Name,
                    userName = player.Name
                };
                player.TriggerEvent("dockOrder:openPage", JsonConvert.SerializeObject(data));
            }
            catch (Exception ex)
            {
                _logger.WriteError("DockOrderOpen" + ex);
            }
        }
    }

    internal struct DockOrderDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("desc")]
        public string Description { get; set; }
        [JsonProperty("cost")]
        public int Cost { get; set; }
    }

    internal struct ConfirmedOrderDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}