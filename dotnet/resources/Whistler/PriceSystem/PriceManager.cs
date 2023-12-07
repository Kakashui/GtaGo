using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.NewDonateShop;
using Whistler.SDK;

namespace Whistler.PriceSystem
{
    class PriceManager : Script
    {
        private static Dictionary<string, int> _prices = new Dictionary<string, int>();
        private static readonly Dictionary<TypePrice, Action<string, int, int>> _subscribes = new Dictionary<TypePrice, Action<string, int, int>>();

        public static void AddEvent(TypePrice action, Action<string, int, int> handler)
        {
            if (!_subscribes.ContainsKey(action))
                _subscribes.Add(action, handler);
            else
                _subscribes[action] += handler;
        }
        public static void RemoveEvent(TypePrice action, Action<string, int, int> handler)
        {
            if (_subscribes.TryGetValue(action, out Action<string, int, int> d))
            {
                if (d != null)
                    d -= handler;
            }
        }
        private static void InvokeEvent(TypePrice action, string name, int priceInCoins)
        {
            _subscribes.TryGetValue(action, out Action<string, int, int> d);
            d?.Invoke(name, priceInCoins, priceInCoins * Main.ServerConfig.DonateConfig.CoinToVirtual);
        }

        public static void Init()
        {
            var result = MySQL.QueryRead("SELECT * FROM `propsprices`");
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    var name = row["name"].ToString();
                    var price = Convert.ToInt32(row["price"]);
                    if (!_prices.ContainsKey(name) && price > 0)
                        _prices.Add(name, price);
                    else
                        MySQL.Query("DELETE FROM `propsprices` WHERE `id` = @prop0", Convert.ToInt32(row["id"]));
                }
            }
        }

        public static bool AddItem(TypePrice type, string name, int price)
        {
            if (type == TypePrice.Car)
                name = name.ToLower();
            string newName = $"{type}::{name.ToLower()}";
            if (_prices.ContainsKey(newName))
            {
                return false;
            }
            _prices.Add(newName, price);
            MySQL.Query("INSERT INTO `propsprices`(`name`, `price`) VALUES(@prop0, @prop1)", newName, price);
            InvokeEvent(type, name.ToLower(), price);
            return true;
        }

        private static bool UpdateItem(TypePrice type, string name, int price)
        {
            if (type == TypePrice.Car)
                name = name.ToLower();
            string fullName = $"{type}::{name}";
            if (_prices.ContainsKey(fullName))
            {
                _prices[fullName] = price;
                MySQL.Query("UPDATE `propsprices` SET `price` = @prop0 WHERE `name` = @prop1", price, fullName);
                InvokeEvent(type, name.ToLower(), price);
                return true;
            }
            return false;
        }

        private static bool DeleteItem(string name)
        {
            if (_prices.ContainsKey(name))
            {
                _prices.Remove(name);
                MySQL.Query("DELETE FROM `propsprices` WHERE `name` = @prop0", name);
                return true;
            }
            return false;
        }


        public static int GetPrice(TypePrice type, string name, int defaultPrice = 0)
        {
            var price = _prices.GetValueOrDefault($"{type}::{name}", 0);
            return price <= 0 ? defaultPrice : price;
        }

        public static int GetPriceInDollars(TypePrice type, string name, int defaultPrice = 0)
        {
            var price = _prices.GetValueOrDefault($"{type}::{name}", 0);
            return price <= 0 ? defaultPrice : price * Main.ServerConfig.DonateConfig.CoinToVirtual;
        }
        
        [Command("openpricemenu")]
        public static void OpenPriceMenu(ExtPlayer player)
        {
            if (!Group.CanUseAdminCommand(player, "openpricemenu")) return;
            var cats = JsonConvert.SerializeObject(Enum.GetNames(typeof(TypePrice)));
            SafeTrigger.ClientEvent(player,"priceMenu:open", JsonConvert.SerializeObject(_prices), cats);
        }
        
        [RemoteEvent("priceMenu:addItem")]
        public static void PriceMenuAddItem(ExtPlayer player, string name, int price)
        {
            if (!Group.CanUseAdminCommand(player, "openpricemenu")) return;
            var split = name.Split("::");
            if (split.Length != 2 || !Enum.IsDefined(typeof(TypePrice), split[0]) || price <= 0)
            {
                Notify.SendError(player, "price:menu:1");
                return;
            }
            if (AddItem(Enum.Parse<TypePrice>(split[0]), split[1], price))
            {
                Notify.SendSuccess(player, "price:menu:2");
                UpdateItemInMenu(player, name, price);
            }
            else
                Notify.SendError(player, "price:menu:3");
        }

        [RemoteEvent("priceMenu:changePrice")]
        public static void PriceMenuChangePrice(ExtPlayer player, string name, int price)
        {
            if (!Group.CanUseAdminCommand(player, "openpricemenu")) return;
            var split = name.Split("::");
            if (split.Length != 2 || !Enum.IsDefined(typeof(TypePrice), split[0]) || price <= 0)
            {
                Notify.SendError(player, "price:menu:1");
                return;
            }
            if (UpdateItem(Enum.Parse<TypePrice>(split[0]), split[1], price))
            {
                Notify.SendSuccess(player, "price:menu:4");
                UpdateItemInMenu(player, name, price);
            }
            else
                Notify.SendError(player, "price:menu:5");
        }

        [RemoteEvent("priceMenu:deleteItem")]
        public static void PriceMenuDeleteItem(ExtPlayer player, string name)
        {
            if (!Group.CanUseAdminCommand(player, "openpricemenu")) return;
            if (DeleteItem(name))
            {
                Notify.SendSuccess(player, "price:menu:6");
                UpdateItemInMenu(player, name, -1);
            }
            else
                Notify.SendError(player, "price:menu:5");
        }

        private static void UpdateItemInMenu(ExtPlayer player, string name, int price)
        {
            player.TriggerCefEvent("priceMenu/updatePriceItem", JsonConvert.SerializeObject(new { Name = name, Price = price }));
        }
    }
}
