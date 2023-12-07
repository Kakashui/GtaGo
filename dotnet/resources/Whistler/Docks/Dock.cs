using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Businesses;
using Whistler.Core;
using Whistler.Core.QuestPeds;
using Whistler.Domain.Phone.Messenger;
using Whistler.Entities;
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
                    .AddInteraction(OpenPortOrderMenuForPlayer, "Чтобы открыть терминал");
            }
            NAPI.Blip.CreateBlip(356, TerminalPositions[2], 1.2f, 68, Main.StringToU16("Терминал"), 255, 0, true, 0, 0);            
        }

        [RemoteEvent("dockOrder:playerOrdered")]
        public void OnPlayerOrderedProducts(ExtPlayer player, string data)
        {
            try
            {
                var items = JsonConvert.DeserializeObject<List<ConfirmedOrderDTO>>(data);
                if (items == null || !items.Any()) return;

                Business biz = player.GetBusiness();
                if (biz == null) return;

                if (biz.Orders.Any())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "В обработке может быть только 1 заказ от одного бизнеса. Дождитесь доставки предыдущего заказа.", 5000);
                    return;
                }
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
                        Notify.Send(player, NotifyType.Alert, NotifyPosition.Top, "Недостаточно места на складе", 3000);
                        return;
                    }
                    totalCost += product.OrderPrice * item.Count;
                    products.Add((product.Name, item.Count));
                }

                if (player.Character.Money < totalCost)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.Center, "Недостаточно денег", 3000);
                    return;
                }

                if (!Wallet.MoneySub(player.Character, totalCost, "Заказ товаров в бизнес")) return;
                biz.ProfitData.NewExpenses(totalCost);

                Order newOrder = new Order(products, biz);
                CreateOrder(newOrder);

                Notify.Send(player, NotifyType.Success, NotifyPosition.Bottom, "Вы заказали товар".Translate(), 3000);
                player.SendTip("Ожидайте доставки товаров дальнобойщиками");
            }
            catch (Exception ex) 
            { 
                _logger.WriteError("DockOrder" + ex);
            }
        }

        public static void CreateOrder(Order order)
        {
            if (order == null) return;

            if (!order.Customer.Orders.Contains(order)) order.Customer.Orders.Add(order);
            if (!CurrentOrders.Contains(order)) CurrentOrders.Add(order);
        }

        private static void OpenPortOrderMenuForPlayer(ExtPlayer player)
        {
            try
            {
                var biz = player.GetBusiness();
                if (biz == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.Top, "У вас нет бизнеса", 3000);
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
                SafeTrigger.ClientEvent(player,"dockOrder:openPage", JsonConvert.SerializeObject(data));
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