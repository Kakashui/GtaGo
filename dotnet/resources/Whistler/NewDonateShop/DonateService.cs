using System;
using System.Data;
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

namespace Whistler.NewDonateShop
{
    static class DonateService
    {
        private static ConcurrentDictionary<int, DonateInventoryModel> _inventoryCache = new ConcurrentDictionary<int, DonateInventoryModel>();
        public static SlotConfig _slotConfig = new SlotConfig();

        public static PrimeAccountConfig PrimeAccount;
        public static ItemsConfig Items { get; private set; } = new ItemsConfig();
        public static Dictionary<int, RouletteGame> RouletteGames { get; private set; }

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(DonateService));

        public static List<Donate> DonateList = new List<Donate>();

        public static Wallet Wallet{ get; private set; }
        public static Shop Shop { get; private set; }
        public static void LoadConfig()
        {
            PrimeAccount = new PrimeAccountConfig();
            RouletteGames = RouletteGame.Init();
            Wallet = Main.ServerConfig.DonateConfig.PaymentProvider switch
            {
                "Enot" => new EnotWallet(),
                _ => new PrimeWallet(),
            };
            Shop = new Shop();
            Main.DatabaseSave += SaveAll;
            PriceManager.AddEvent(TypePrice.Car, Items.UpdateVehiclePrice);
        }

    // shop:{
    //   premium: {
    //     price: 1000,
    //   },
    //   money: [
    //     { id: 0, name: '$ 150.000', price: 100 },
    //     { id: 1, name: '$ 250.000', price: 100 },
    //     { id: 2, name: '$ 350.000', price: 100 },
    //     { id: 3, name: '$ 450.000', price: 100 },
    //     { id: 4, name: '$ 550.000', price: 100 },
    //     { id: 5, name: '$ 650.000', price: 100 },
    //   ],
    //   exclusive: {
    //     count: 1,
    //     maxcount: 500,
    //     price: 1000,
    //     pictures: [
    //       { id: 1, shirt: '/img/Shop/shirt-modal.png', sneacker: '/img/Shop/sneackers-modal.png', pants: '/img/Shop/pants-modal.png', title: 'first' },
    //       { id: 2, shirt: '/img/Shop/shirt-modal-2.png', sneacker: '/img/Shop/sneackers-modal-2.png', pants: '/img/Shop/pants-modal-2.png', title: 'second' },
    //       { id: 3, shirt: '/img/Shop/shirt-modal-3.png', sneacker: '/img/Shop/sneackers-modal-3.png', pants: '/img/Shop/pants-modal-3.png', title: 'third' },
    //     ]
    //   }
    // },

        public static void LoadDonateDB()
        {
            var result = MySQL.QueryRead("SELECT * FROM `donateitems`");

            if (result == null)
            {
                _logger.WriteError("DB `donateitems` return null result");
            }
            else
            {
                foreach (DataRow Row in result.Rows)
                {
                    Donate donate = new Donate(Row);
                    DonateList.Add(donate);
                }
            }
            // NAPI.Util.ConsoleOutput($"{DonateList}");
        }


        public static void UpdateExclusive(this ExtPlayer player, Donate exc){
            exc.Count = exc.Count + 1;
            if(exc.Count > exc.MaxCount) exc.Count = exc.MaxCount;
            NAPI.ClientEvent.TriggerClientEventForAll("mmenu:donateExclusive:update", exc.Price, exc.Count, exc.MaxCount);
            MySQL.Query("UPDATE donateitems SET count = @prop0 WHERE id = @prop1", exc.Count, exc.Id);
        }

        public static void UpdateCoins(this ExtPlayer player)
        {
            Account account = player.Account;
            SafeTrigger.ClientEvent(player,"dshop:coins:update", account.MCoins);
        }
        public static void UpdatePrime(this ExtPlayer player)
        {
            Core.Character.Character character = player.Character;
            SafeTrigger.ClientEvent(player,"dshop:prime:update", character.GetPrimeDays());
        }

        public static int UseJobCoef(ExtPlayer player, int sum, bool isPremium)
        {
            int multiplied = Main.ServerConfig.Jobs.PayMultipler * sum;
            if (isPremium) multiplied *= 2;
            return multiplied;
        }
        public static int UseJobKoef(ExtPlayer player, float sum, bool isPremium)
        {
            int multiplied = Main.ServerConfig.Jobs.PayMultipler * Convert.ToInt32(sum);
            if (isPremium) multiplied *= 2;
            return multiplied;
        }

        public static void ParseConfigs()
        {
            if (Directory.Exists("interfaces/gui/src/configs/newDonateShop"))
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
            var responce = MySQL.QueryRead("INSERT INTO `donate_inventories` (`items`) VALUES (@prop0);SELECT @@identity;", inventory.GetItemData());
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
        public static void CharacterSlot(ExtPlayer player, Account account, int index)
        {
            try
            {
                int coins = account.MCoins;
                if (coins < _slotConfig.Price)
                {
                    int count = _slotConfig.Price - coins;
                    Notify.SendError(player, $"У Вас не хватает {count} коинов. Пополните баланс и попробуйте ещё раз.");
                }
                else
                {
                    account.Characters[index] = -1;
                    MySQL.Query($"UPDATE accounts SET character{index + 1} = -1 WHERE login = @prop0", account.Login);
                    player.SubMCoins(_slotConfig.Price);
                    account.LoadSlots(player);
                    Notify.SendSuccess(player, $"Вы успешно оплатили открытие слота на аккаунте. Приятной игры!");
                }
            }
            catch (Exception ex)
            {
                DonateLog.ErrorLog(ex.ToString(), "Unwarn");
            }
        }

        internal static DonateInventoryModel GetInventoryByUUID(int uuid)
        {
            var result = MySQL.QueryRead("SELECT `donateInventoryId` FROM `characters` WHERE `uuid`=@prop0", uuid);
            return result.Rows.Count > 0 && !result.Rows[0].IsNull("donateInventoryId") ? GetInventoryById(Convert.ToInt32(result.Rows[0]["donateInventoryId"])) : null;
        }
    }
}
