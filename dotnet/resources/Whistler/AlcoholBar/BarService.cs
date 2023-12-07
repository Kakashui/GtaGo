using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Whistler.AlcoholBar.Configs;
using Whistler.AlcoholBar.Models;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.MoneySystem;
using Whistler.SDK;

namespace Whistler.AlcoholBar
{
    static class BarService
    {
        private static BarConfig _config;
        private static Dictionary<int, BarPoint> _points;
        public static void Init()
        {
            MySqlCommand query = new MySqlCommand(@"CREATE TABLE IF NOT EXISTS `alcobars`(" +
                    $"`id` int(11) NOT NULL AUTO_INCREMENT," +
                    $"`position` TEXT NOT NULL," +
                    $"`radius` INT(11) NOT NULL," +
                    $"PRIMARY KEY(`id`)" +
                    $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4");
            MySQL.Query(query);

            _config = new BarConfig();
            _config.Parse();
            //_config.SetRandomDoscount(10);
            //_config.SetRandomDoscount(20);
            //_config.SetRandomDoscount(25);
            InitEnterPoints();
        }

        internal static void BuyAlco(ExtPlayer player, string json, bool cashPay)
        {
            Basket[] basket = JsonConvert.DeserializeObject<Basket[]>(json);
            if (basket == null || basket.Length == 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы ничего не выбрали для покупки.", 3000);
                return;
            }

            Inventory.Models.InventoryModel inv = player.GetInventory();
            if (inv == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Ошибка инвентаря, обратитесь к администрации.", 3000);
                return;
            }

            AlcoItem alcoConfig;
            Inventory.Models.Alcohol alcoItem;
            string bought = "Вы купили ";
            MoneySystem.Interface.IMoneyOwner payMethod = player.GetMoneyPayment(cashPay ? PaymentsType.Cash : PaymentsType.Card);
            int price;
            int totalPrice = 0;
            foreach (Basket item in basket)
            {
                alcoConfig = _config.GetItemByName(item.Name);
                if (alcoConfig == null) continue;

                alcoItem = ItemsFabric.CreateAlcohol(alcoConfig.Item.Name, item.Count, false);
                if (!inv.CanAddItem(alcoItem))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно места в инвентаре для {item.Name} (x{item.Count})", 3000);
                    break;
                }

                price = alcoConfig.Price * item.Count;
                if (price < 0 || !Wallet.MoneySub(payMethod, price, "Покупка алкоголя"))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно денежных средств на покупку {item.Name} (x{item.Count})", 3000);
                    break;
                }

                inv.AddItem(alcoItem);
                bought += $"{item.Name} (x{item.Count}), ";
                totalPrice += price;
            }
            if (totalPrice == 0) return;

            bought = bought.Remove(bought.Length - 2, 2);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, bought, 5000);
        }

        private static void InitEnterPoints()
        {
            var responce = MySQL.QueryRead("SELECT * FROM `alcobars`;");
            _points = new Dictionary<int, BarPoint>();
            foreach (DataRow item in responce.Rows)
            {
                var bar = new BarPoint
                {
                    Id = Convert.ToInt32(item["id"]),
                    Position = JsonConvert.DeserializeObject<Vector3>(item["position"].ToString()),
                    Radius = Convert.ToInt32(item["radius"]),
                };
                _points.Add(bar.Id, bar);
            }
            foreach (var point in _points.Values)
            {
                point.Load(OpenAlcoShop);
            }
           
        }

        static int GetClosestBar(Vector3 position)
        {
            return (_points.Any(p => p.Value.Position.DistanceTo(position) < 10)) ? _points.First(p => p.Value.Position.DistanceTo(position) < 10).Key : -1;
        }

        internal static void AddNewBarpoint(ExtPlayer player, int radius)
        {
            var point = new BarPoint();
            point.Create(player.Position - new Vector3(0, 0, 1), radius, OpenAlcoShop);
            _points.Add(point.Id, point);
            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Точка добавлена c ID: {point.Id}", 3000);
        }
        internal static void RemoveBarPoint(ExtPlayer player)
        {
            var id = GetClosestBar(player.Position);
            if(id < 0)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Рядом нету баров", 3000);
            }
            else
            {
                _points[id].Destroy();
                _points.Remove(id);
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Бар {id} удален", 3000);
            }
        }
        static void OpenAlcoShop(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"alco:bar:open", _config.Discounts);
        }
    }
}
