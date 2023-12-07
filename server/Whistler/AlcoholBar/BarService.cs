using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Whistler.AlcoholBar.Configs;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.MoneySystem;
using Whistler.SDK;
using Newtonsoft.Json;
using System.Linq;

namespace Whistler.AlcoholBar
{
    static class BarService
    {
        private static BarConfig _config;
        private static Dictionary<int, BarPoint> _points;
        public static void Init()
        {
            var query = $"CREATE TABLE IF NOT EXISTS `alcobars`(" +
                    $"`id` int(11) NOT NULL AUTO_INCREMENT," +
                    $"`position` TEXT NOT NULL," +
                    $"`radius` INT(11) NOT NULL," +
                    $"PRIMARY KEY(`id`)" +
                    $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
            MySQL.Query(query);

            _config = new BarConfig();
            _config.Parse();
            //_config.SetRandomDoscount(10);
            //_config.SetRandomDoscount(20);
            //_config.SetRandomDoscount(25);
            InitEnterPoints();
        }

        internal static void BuyAlco(Player player, int id)
        {
            var alco = _config[id];
            var inv = player.GetInventory();
            if (!inv.CanAddItem(alco.Item))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно места в инвентаре", 3000);
                return;
            }

            if (Wallet.MoneySub(player.GetCharacter(), alco.Price, "Money_BuyAlco"))
            {
                var item = ItemsFabric.CreateAlcohol(alco.Item.Name, 1, false);
                inv.AddItem(item);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы купили {alco.Name}", 3000);
            }
            else
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно денежных средств", 3000);
                return;
            }
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

        internal static void AddNewBarpoint(Player player, int radius)
        {
            var point = new BarPoint();
            point.Create(player.Position - new Vector3(0, 0, 1), radius, OpenAlcoShop);
            _points.Add(point.Id, point);
            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Точка добавлена c ID: {point.Id}", 3000);
        }
        internal static void RemoveBarPoint(Player player)
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
        static void OpenAlcoShop(Player player)
        {
            player.TriggerEvent("alco:bar:open", _config.Discounts);
        }
    }
}
