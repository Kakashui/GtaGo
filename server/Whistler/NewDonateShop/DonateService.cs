using System;
using System.Collections.Generic;
using System.Text;
using Whistler.NewDonateShop.Configs;
using Whistler.NewDonateShop.Models;
using Newtonsoft.Json;
using System.IO;
using GTANetworkAPI;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.Core.nAccount;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Whistler.PriceSystem;
using Whistler.Entities;
using Whistler.NewDonateShop.Interfaces;

namespace Whistler.NewDonateShop
{
    static class DonateService
    {
        private static ConcurrentDictionary<int, DonateInventoryModel> _inventoryCache = new ConcurrentDictionary<int, DonateInventoryModel>();
        private static SlotConfig _slotConfig = new SlotConfig();

        public static PrimeAccountConfig PrimeAccount;
        public static ItemsConfig Items { get; private set; } = new ItemsConfig();
        public static Dictionary<int, RouletteGame> RouletteGames { get; private set; }

        public static Wallet Wallet{ get; private set; }
        public static Shop Shop { get; private set; }
        public static void LoadConfig()
        {
            PrimeAccount = new PrimeAccountConfig();
            RouletteGames = RouletteGame.Init();
            switch (Main.ServerConfig.DonateConfig.PaymentProvider)
            {
                case "Enot":
                    Wallet = new EnotWallet();
                    break;
                default:
                    Wallet = new PrimeWallet();
                    break;
            }
            
            Shop = new Shop();
            Main.DatabaseSave += SaveAll;
            PriceManager.AddEvent(TypePrice.Car, Items.UpdateVehiclePrice);
        }

        public static void UpdateCoins(this Player player){
            var account = player.GetAccount();
            player.TriggerEvent("dshop:coins:update", account.GoCoins);
        }
        public static void UpdatePrime(this Player player)
        {
            var account = player.GetAccount();
            player.TriggerEvent("dshop:prime:update", account.GetPrimeDays());
        }

        public static int UseJobCoef(Player player, int sum)
        {
            return Main.ServerConfig.Jobs.PayMultipler * (player.GetAccount().IsPrimeActive() ? Convert.ToInt32(sum * PrimeAccount.JobsPaymentsMultipler) : sum);
        }
        public static int UseJobKoef(Player player, float sum)
        {
            return Main.ServerConfig.Jobs.PayMultipler * (player.GetAccount().IsPrimeActive() ? Convert.ToInt32(sum * PrimeAccount.JobsPaymentsMultipler) : Convert.ToInt32(sum));
        }

        public static void ParseConfigs()
        {
            if (Directory.Exists("interfaces"))
            {
                Items.ParseConfigs();
                RouletteGame.ParseConfigs();
                using var r1 = new StreamWriter("interfaces/gui/src/configs/newDonateShop/primeAccount.js");
                r1.Write($"export default {JsonConvert.SerializeObject(PrimeAccount)}");
            }
        }

        public static DonateInventoryModel GetInventoryById(int id)
        {
            return _inventoryCache.GetOrAdd(id, LoadInventoryFromDB);           
        }
        private static DonateInventoryModel LoadInventoryFromDB(int id)
        {
            if (id < 1) return null;
            var responce = MySQL.QueryRead("SELECT `items` FROM `donate_inventories` WHERE `id`=@prop0", id);
            if (responce == null || responce.Rows.Count == 0) return null;
            var row = responce.Rows[0];
            var inventory = new DonateInventoryModel(row["items"].ToString());
            inventory.Id = id;
            return inventory;
        }
        public static DonateInventoryModel CrateInventory()
        {
            var inventory = new DonateInventoryModel();
            string itemdata = inventory.GetItemData();
            var responce = MySQL.QueryRead("INSERT INTO `donate_inventories` (`items`) VALUES (@prop0);SELECT @@identity;", itemdata);
            inventory.Id = Convert.ToInt32(responce.Rows[0][0]);
            _inventoryCache.TryAdd(inventory.Id, inventory);
            return inventory;
        }

        public static void SaveAll()
        {
            Task.Run(() =>
            {
                foreach (var inventory in _inventoryCache)
                {
                    inventory.Value?.Save();
                }
                foreach (var roulette in RouletteGames)
                {
                    roulette.Value?.Save();
                }
            });            
        }
        public static void CharacterSlot(PlayerGo player, Account account)
        {
            try
            {
                var coins = account.GoCoins;
                if (coins < _slotConfig.Price)
                {
                    var count = _slotConfig.Price - coins;
                    Wallet.OrderCoins(player, count);
                }
                else
                {
                    account.AddSlot();
                    player.SubGoCoins(_slotConfig.Price);
                    account.LoadSlots(player);
                }
            }
            catch (Exception ex)
            {
                DonateLog.ErrorLog(ex.ToString(), "Unwarn");
            }
        }

        internal static DonateInventoryModel GetInventoryByUUID(int uuid)
        {
            var characterInCache = Main.GetCharacterByUUID(uuid);
            if (characterInCache != null) return characterInCache.DonateInventory;

            var result = MySQL.QueryRead("SELECT `donateInventoryId` FROM `characters` WHERE `uuid`=@prop0", uuid);
            return result.Rows.Count > 0 && !result.Rows[0].IsNull("donateInventoryId") ? GetInventoryById(Convert.ToInt32(result.Rows[0]["donateInventoryId"])) : null;
        }
    }
}
