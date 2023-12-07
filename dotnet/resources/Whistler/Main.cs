using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using Whistler.Core.Character;
using Whistler.GUI;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;
using Whistler.Businesses;
using Whistler.Houses;
using Whistler.Families;
using Whistler.MoneySystem;
using ServerGo.Casino.Business;
using Whistler.Core.UserDialogs;
using Whistler.Fractions;
using Whistler.Helpers;
using Whistler.VehicleSystem;
using Whistler.Inventory;
using Whistler.VehicleSystem.Models.VehiclesData;
using Whistler.UpdateR;
using Whistler.SDK.StockSystem;
using Whistler.Possessions;
using Whistler.Core.CustomSync.Attachments;
using Whistler.Fractions.GOV;
using Whistler.ServerQuery;
using Whistler.NewDonateShop;
using Whistler.Core.Weather;
using Whistler.Phone.Forbes;
using Whistler.VehicleTrading;
using Whistler.Scenes;
using Whistler.Scenes.Configs;
using Whistler.NewJobs;
using System.Collections.Concurrent;
using Whistler.Entities;
using Whistler.ServerConfiguration;
using Whistler.Customization.Enums;
using Whistler.Core.nAccount;
using Whistler.Customization;
using Whistler.Common;
using Whistler.Gangs.WeedFarm;
using Whistler.Inventory.Enums;
using Whistler.PriceSystem;
using Whistler.Fractions.PDA;
using Whistler.VehicleSystem.Models;
using System.Diagnostics;
using Whistler.Core.LifeSystem;

namespace Whistler
{
    public delegate void PlayerPreDisconnectHandler(ExtPlayer player);
    public delegate void OnPlayerReadyDelegate(ExtPlayer player);

    public class SlotInfo
    {
        public SlotInfo(int lvl, int exp, int fraction, long money, bool gender, int hunger, int thirst)
        {
            Lvl = lvl;
            Exp = exp;
            Fraction = fraction;
            Money = money;
            Gender = gender;
            Hunger = hunger;
            Thirst = thirst;
        }

        public int Lvl { get; set; }
        public int Exp { get; set; }
        public int Fraction { get; set; }
        public long Money { get; set; }
        public bool Gender { get; set; }
        public int Hunger { get; set; }
        public int Thirst { get; set; }
    }
    public class Main : Script
    {
        private static AutoRestart _restart;
        public static ServerConfig ServerConfig { get; } = ServerConfig.Load();
        public static string Codename { get; } = "GSRP";
        public static string Version { get; } = "0.4"; // 2.2.4 r.i.p
        public static string Build { get; } = "Global"; // 1583 r.i.p

        public static bool IsEnviromentWinter = false;
        // // // //
        public static string Full { get; } = $"{Codename} {Version} {Build}";
        public static DateTime StartDate { get; } = DateTime.Now;
        public static DateTime CompileDate { get; } = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;

        public static event Action Payday;

        public static event Action DatabaseSave;
        public static PlayerPreDisconnectHandler PlayerPreDisconnect;
        public static event OnPlayerReadyDelegate OnPlayerReady;
        public static event Func<ExtPlayer, Character, Task> OnPlayerReadyAsync;
        public static Action<ExtPlayer> OnPlayerLevelUp;


        private static int Slots = NAPI.Server.GetMaxPlayers();
        //private static ServerQueryService _serverQuery;

        // All players, vehicles
        public static List<ExtPlayer> AllPlayers = new List<ExtPlayer>();
        public static Dictionary<ushort, ExtVehicle> AllVehicles = new Dictionary<ushort, ExtVehicle>();

        // Characters
        public static List<int> UUIDs = new List<int>(); // characters UUIDs
        public static Dictionary<int, string> PlayerNames = new Dictionary<int, string>(); // character uuid - character name
        public static Dictionary<string, int> PlayerUUIDs = new Dictionary<string, int>(); // character name - character uuid
        public static Dictionary<int, SlotInfo> PlayerSlotsInfo = new Dictionary<int, SlotInfo>(); // character uuid - lvl,exp,fraction,money


        // Accounts
        public static List<string> Usernames = new List<string>(); // usernames
        public static List<string> SocialClubs = new List<string>(); // socialclubnames
        public static List<ulong> SocialClubsID = new List<ulong>(); // socialclubids
        public static Dictionary<string, string> Emails = new Dictionary<string, string>(); // emails
        public static Dictionary<ExtPlayer, Tuple<int, string, string, string>> RestorePass = new Dictionary<ExtPlayer, Tuple<int, string, string, string>>(); // int code, string Login, string SocialClub, string Email

        public static char[] stringBlock = { '\'', '@', '[', ']', ':', '"', '[', ']', '{', '}', '|', '`', '%',  '\\' };

        public static string BlockSymbols(string check) {
            for (int i = check.IndexOfAny(stringBlock); i >= 0;)
            {
                check = check.Replace(check[i], ' ');
                i = check.IndexOfAny(stringBlock);
            }
            return check;
        }

        public static Random rnd = new Random();
              
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Main));

        public static void FIX_FUCKING_LASTVEH()
        {
            var count = 0;

            var result = MySQL.QueryRead("SELECT `login`,`character1`,`character2`,`character3` FROM `accounts`");
            foreach (DataRow Row in result.Rows)
            {
                var login = Row["login"].ToString();
                var characters = new int[] { Convert.ToInt32(Row["character1"]), Convert.ToInt32(Row["character2"]), Convert.ToInt32(Row["character3"]) };

                var index = 0;
                foreach (var character in characters)
                {
                    //WARNING: Гениальн омать вашу, а откуда PlayerNames возьмется при запуске сервера
                    if (character >= 0 && !PlayerNames.ContainsKey(character))
                    {
                        MySQL.Query("DELETE FROM customization WHERE uuid=@prop0", character);
                        MySQL.Query("DELETE FROM inventory WHERE uuid=@prop0", character);
                        _logger.WriteWarning($"Login {login} ");
                        MySQL.Query($"UPDATE accounts SET character{index + 1}=-1 WHERE login=@prop0", login);

                        _logger.WriteWarning($"CHARACTER {character} NOT FOUND. ALL DEPENDING ON IT UUID WAS DELETED");
                        count++;
                    }
                    index++;
                }
            }

            _logger.WriteWarning($"{count} CHARACTERS WAS DELETED");
        }

        private void DeletePromoWeapons()
        {
            DataTable result = MySQL.QueryRead("SELECT `id` FROM `inventories`");
            if (result != null)
            {
                foreach (DataRow Row in result.Rows)
                {
                    int id = Convert.ToInt32(Row["id"]);
                    var inventory = InventoryService.GetById(id);
                    if (inventory != null) inventory.RemoveItems(i => 
                        (i.Type == Inventory.Enums.ItemTypes.Weapon && i.Promo),
                        Inventory.Enums.LogAction.None
                     );
                }
            }
            result = MySQL.QueryRead("SELECT `id` FROM `equips`");
            if (result != null)
            {
                foreach (DataRow Row in result.Rows)
                {
                    int id = Convert.ToInt32(Row["id"]);
                    var equip = EquipService.GetById(id);
                    if (equip != null) equip.RemoveWeapons(i => i.Value != null && i.Value.Promo);
                }
            }
        }

        private void DeleteAllWeapons()
        {
            DataTable result = MySQL.QueryRead("SELECT `id` FROM `inventories`");
            if (result != null)
            {
                foreach (DataRow Row in result.Rows)
                {
                    int id = Convert.ToInt32(Row["id"]);
                    var inventory = InventoryService.GetById(id);
                    if (inventory != null) inventory.RemoveItems(i =>
                        (i.Type == Inventory.Enums.ItemTypes.Weapon),
                        Inventory.Enums.LogAction.None
                     );
                }
            }
            result = MySQL.QueryRead("SELECT `id` FROM `equips`");
            if (result != null)
            {
                foreach (DataRow Row in result.Rows)
                {
                    int id = Convert.ToInt32(Row["id"]);
                    var equip = EquipService.GetById(id);
                    if (equip != null) equip.RemoveWeapons(i => i.Value != null);
                }
            }
        }

        private void DeleteAllClothes()
        {
            DataTable result = MySQL.QueryRead("SELECT `id` FROM `inventories`");
            if (result != null)
            {
                foreach (DataRow Row in result.Rows)
                {
                    int id = Convert.ToInt32(Row["id"]);
                    var inventory = InventoryService.GetById(id);
                    if (inventory != null) inventory.RemoveItems(
                        i =>i.Type == Inventory.Enums.ItemTypes.Clothes || 
                        i.Type == Inventory.Enums.ItemTypes.Props || 
                        i.Type == Inventory.Enums.ItemTypes.Backpack ||
                        i.Type == Inventory.Enums.ItemTypes.Costume , Inventory.Enums.LogAction.None);
                }
            }
            result = MySQL.QueryRead("SELECT `id` FROM `equips`");
            if (result != null)
            {
                foreach (DataRow Row in result.Rows)
                {
                    int id = Convert.ToInt32(Row["id"]);
                    var equip = EquipService.GetById(id);
                    if (equip != null) equip.RemoveClothes( i =>
                        i.Value != null &&(
                            i.Value.Type == Inventory.Enums.ItemTypes.Clothes ||
                            i.Value.Type == Inventory.Enums.ItemTypes.Props ||
                            i.Value.Type == Inventory.Enums.ItemTypes.Backpack ||
                            i.Value.Type == Inventory.Enums.ItemTypes.Costume
                        )
                    );
                }
            }
        }

        private void FixMasks()
        {
            DataTable result = MySQL.QueryRead("SELECT `id` FROM `inventories`");
            if (result != null)
            {
                foreach (DataRow Row in result.Rows)
                {
                    int id = Convert.ToInt32(Row["id"]);
                    var inventory = InventoryService.GetById(id);
                    if (inventory != null) inventory.Items.ForEach(i =>
                    {
                        if (
                            i is Inventory.Models.Clothes &&
                            i.Name == ItemNames.Mask &&
                            (i as Inventory.Models.Clothes).Drawable > 505
                        )
                        {
                            (i as Inventory.Models.Clothes).Drawable++;
                            inventory.MarkAsChanged();
                        }
                    });
                }
            }
            result = MySQL.QueryRead("SELECT `id` FROM `equips`");
            if (result != null)
            {
                foreach (DataRow Row in result.Rows)
                {
                    int id = Convert.ToInt32(Row["id"]);
                    var equip = EquipService.GetById(id);
                    if (equip != null)
                    {
                        equip.Clothes.Values.ToList().ForEach(i => {
                            if (
                                i != null &&
                                i.Name == ItemNames.Mask &&
                                i.Drawable > 505
                            ) {
                                i.Drawable++;
                                equip.MarkAsChanged();
                            };
                        });
                    }
                }
            }
        }
               
        public void DeleteCharacters()
        {
            var query = $"CREATE TABLE IF NOT EXISTS `resetcharacters`(" +
              $"`id` int(11) NOT NULL AUTO_INCREMENT," +
              $"`compleete` int(11) NOT NULL," +
              $"`hours` int(11) NOT NULL," +
              $"PRIMARY KEY(`id`)" +
              $")ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
            MySQL.QuerySync(query);

            //TestDelete();
            //return;

            var responce = MySQL.QueryRead("SELECT `hours` FROM `resetcharacters` WHERE `compleete`=0");
            if (responce == null || responce.Rows.Count == 0) return;
            var hours = Convert.ToInt32(responce.Rows[0]["hours"]);
            responce = MySQL.QueryRead(
                "with hours as " + 
                "(SELECT `uuid`, sum(ifnull(TIMESTAMPDIFF(SECOND, `in`, `out`), 600)) as " +
                "times FROM whistlerlogs.connlog " +
                "where `out` > '2021-09-01 00:00:00' || `in` > '2021-09-01 00:00:00' " +
                "group by uuid) "+
                "SELECT ch.uuid FROM whistler.characters ch " +
                "left join hours on ch.uuid = hours.uuid " +
                $"WHERE ifnull(hours.times, 0) < {hours} * 60 * 60 AND ch.adminlvl = 0");
            if (responce != null && responce.Rows.Count > 0)
            {
                foreach (DataRow row in responce.Rows)
                {
                    var uuid = Convert.ToInt32(row["uuid"]);
                    var allVehicles = VehicleManager.getAllHolderVehicles(uuid, OwnerType.Personal);
                    foreach (var vehID in allVehicles)
                        VehicleManager.Remove(vehID);

                    var house = HouseManager.GetHouse(uuid, OwnerType.Personal);
                    if (house != null)
                    {
                        if (house.OwnerID == uuid)
                            house.SetOwner(-1, OwnerType.Personal);
                        else
                            house.RemoveRoommate(uuid);
                    }
                    BusinessManager.GetBusinessByOwner(uuid)?.SetOwner(-1);
                    Manager.GetFractionByUUID(uuid)?.DeleteMember(uuid);
                    FamilyManager.GetFamilyByUUID(uuid)?.DeleteMember(uuid);
                    MoneySystem.Wallet.SetBankMoneyByUUID(uuid, 0);
                    InventoryService.GetByUUID(uuid)?.Reset();
                    EquipService.GetByUUID(uuid)?.Reset();
                    DonateService.GetInventoryByUUID(uuid)?.Reset();
                    MySQL.QuerySync("UPDATE `characters` SET `money`=0, `biz`='[]', `chips`=null, `licenses`='[]', `adminlvl`=0 WHERE `uuid`=@prop0", uuid);

                    NAPI.Util.ConsoleOutput($"character whith uuid {uuid} has been reset");
                }
            }            
            MySQL.Query("UPDATE `resetcharacters` SET `compleete`=1;");
        }

        private void TestDelete()
        {
            var uuid = 1007422;
            var allVehicles = VehicleManager.getAllHolderVehicles(uuid, OwnerType.Personal);
            foreach (var vehID in allVehicles)
                VehicleManager.Remove(vehID);

            var house = HouseManager.GetHouse(uuid, OwnerType.Personal);
            if (house != null)
            {
                if (house.OwnerID == uuid)
                    house.SetOwner(-1, OwnerType.Personal);
                else
                    house.RemoveRoommate(uuid);
            }
            BusinessManager.GetBusinessByOwner(uuid)?.SetOwner(-1);
            Manager.GetFractionByUUID(uuid)?.DeleteMember(uuid);
            FamilyManager.GetFamilyByUUID(uuid)?.DeleteMember(uuid);
            MoneySystem.Wallet.SetBankMoneyByUUID(uuid, 0);
            InventoryService.GetByUUID(uuid)?.Reset();
            EquipService.GetByUUID(uuid)?.Reset();
            DonateService.GetInventoryByUUID(uuid)?.Reset();
            MySQL.QuerySync("UPDATE `characters` SET `money`=0, `biz`='[]', `chips`=null, `licenses`='[]', `adminlvl`=0 WHERE `uuid`=@prop0", uuid);

            NAPI.Util.ConsoleOutput($"character whith uuid {uuid} has been reset");
        }
        //static List<int> pizdec = new List<int>();
        //private static async void Pizdec()
        //{
        //    var t = pizdec[2];
        //    Pizdec();
        //}

        //[ServerEvent(Event.UnhandledException)]
        //public void OnException(Exception e)
        //{
        //    _logger.WriteUnhandled(e.ToString());
        //}
        [ServerEvent(Event.UnhandledException)]
        public void OnException(Exception e)
        {
            _logger.WriteUnhandled(e.ToString());
        }

        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            //Pizdec();
            _restart = new AutoRestart();
            //_serverQuery = new ServerQueryService();
            NAPI.Server.SetAutoRespawnAfterDeath(false);
            NAPI.Server.SetAutoSpawnOnConnect(false);
            NAPI.Task.Run(() =>
            {
                NAPI.Server.SetGlobalServerChat(false);
                NAPI.World.SetTime(DateTime.Now.Hour, 0, 0);
            });
            PromoCodesService.Initialize();
            Controller.Initialize();
            LifeActivity.Initialize();
            //DeletePromoWeapons();
            //DeleteAllClothes();
            //DeleteAllWeapons();
            //FixMasks();

            DataTable result = MySQL.QueryRead("SELECT IFNULL(MAX(`uuid`), 0) FROM `characters`");
            if (result != null && result.Rows.Count > 0)
            {
                Character.SetLastUUID(Convert.ToInt32(result.Rows[0][0]));
            }
            result = MySQL.QueryRead(
                "SELECT `uuid`,`firstname`,`lastname`,`sim`,`lvl`,`exp`,`fraction`,`money`,`adminlvl`,`gender`,`hungerlevel`,`thirstlevel`,`rbrating`,`promocode`,`promocodeLevel`,`promocodeActivated`,`promocodeUsed`, ac.login as login " +
                "FROM `characters` ch " +
                "LEFT JOIN `accounts` ac " +
                "ON ch.uuid = ac.character1 OR ch.uuid = ac.character2 OR ch.uuid = ac.character3 " +
                "WHERE `deleted`=0");
            if (result != null)//ctrl+f
            {
                foreach (DataRow Row in result.Rows)
                {
                    try
                    {
                        int uuid = Convert.ToInt32(Row["uuid"]);
                        if (Row["login"] == DBNull.Value)
                        {
                            MySQL.QuerySync("UPDATE `characters` SET `deleted` = 1, `deletedAt` = @prop0 WHERE `uuid` = @prop1", MySQL.ConvertTime(DateTime.Now), uuid);
                            _logger.WriteWarning($"Character with UUID {uuid} was deleted");
                            continue;
                        }
                        if (UUIDs.Contains(uuid)) continue;

                        string name = Convert.ToString(Row["firstname"]);
                        string lastname = Convert.ToString(Row["lastname"]);
                        int lvl = Convert.ToInt32(Row["lvl"]);
                        int exp = Convert.ToInt32(Row["exp"]);
                        int fraction = Convert.ToInt32(Row["fraction"]);
                        long money = Convert.ToInt64(Row["money"]);
                        int adminlvl = Convert.ToInt32(Row["adminlvl"]);
                        int gender = Convert.ToInt32(Row["gender"]);

                        int hunger = Convert.ToInt32(Row["hungerlevel"]);
                        int thirst = Convert.ToInt32(Row["thirstlevel"]);
                        int rbrating = Convert.ToInt32(Row["rbrating"]);

                        string mypromocode = DBNull.Value == Row["promocode"] ? null : Convert.ToString(Row["promocode"]);
                        int promocodeLevel = Convert.ToInt32(Row["promocodeLevel"]);
                        int usedCount = Convert.ToInt32(Row["promocodeUsed"]);
                        int activatedCount = Convert.ToInt32(Row["promocodeActivated"]);
                        if (mypromocode != null) PromoCodesService.AddReferalCode(mypromocode, uuid, promocodeLevel, activatedCount, usedCount);

                        UUIDs.Add(uuid);
                        PlayerNames.Add(uuid, $"{name}_{lastname}");//ходя должно отсюда, тяжко без отладчика
                        PlayerUUIDs.Add($"{name}_{lastname}", uuid);
                        PlayerSlotsInfo.Add(uuid, new SlotInfo(lvl, exp, fraction, money, gender != 0, hunger, thirst));
                        MP.RoyalBattle.RoyalBattleService.LoadBattleRating(uuid, $"{name}_{lastname}", rbrating);

                    }
                    catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
                }
            }
            else _logger.WriteWarning("DB `characters` return null result");                                     //
            
            //FIX_FUCKING_LASTVEH();

            string email;
            ulong socialClubId;
            string socialClubName;
            string login;
            result = MySQL.QueryRead("SELECT `login`,`socialclub`,`email`,`hwid`, `socialclubid` FROM `accounts`");
            if (result != null)
            {
                foreach (DataRow Row in result.Rows)
                {
                    try
                    {
                        login = Convert.ToString(Row["login"]);
                        login = login.ToLower();
                        Usernames.Add(login);

                        socialClubId = Convert.ToUInt64(Row["socialclubid"]);
                        if (socialClubId > 0 && SocialClubsID.Contains(socialClubId)) _logger.WriteError($"ResourceStart: sc contains {socialClubId}");
                        else SocialClubsID.Add(socialClubId);

                        socialClubName = Row["socialclub"] != DBNull.Value ? Convert.ToString(Row["socialclub"]) : null;
                        if (socialClubName != null && !SocialClubs.Contains(socialClubName)) SocialClubs.Add(socialClubName);

                        email = Convert.ToString(Row["email"]);
                        email = email.ToLower();
                        Emails.Add(email, login);
                    }
                    catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
                }
            }
            else _logger.WriteWarning("DB `accounts` return null result");

            Ban.Sync();

            int time = 3600 - (DateTime.Now.Minute * 60) - DateTime.Now.Second;
            NAPI.Task.Run(() =>
            {
                Timers.Start("payday", 3600000, () => 
                {
                    payDayTrigger();
                });
                payDayTrigger();
            }, time * 1000);
           
            Timers.StartTask(60000, DropSystem.CollectGarbage);
            Timers.StartTask("savedb", 300000, saveDatabase );
            NAPI.Task.Run(() => {
                UpdateCharacters();
                Timers.Start("updateCharacters", 300000, UpdateCharacters);
            }, 150000);
            Timers.Start("playedMins", 60000, playedMinutesTrigger);
            WeatherManager.WeatherInit();


            BusinessManager.Init();
            /* Loading families */
            FamilyManager.LoadFamilies();
            HouseManager.OnLoadHouses();
            GarageManager.OnLoadGarages();
            VehicleManager.Init();
            TradeManager.LoadTradePoints();
            ForbesHandler.InitForbes();
            Manager.onResourceStart();
            DonateService.LoadDonateDB();
            // Assembly information //
            _logger.WriteInfo(Full + " started at " + StartDate.ToString("s"));
            _logger.WriteInfo($"Assembly compiled {CompileDate.ToString("s")}");

            DeleteCharacters();
            Console.Title = "RageMP - " + ServerConfig.Main.ServerName;
        }

        private static void ServerPreload()
        {
            RAGE.Entities.Players.CreateEntity = netHandle => new ExtPlayer(netHandle);
            RAGE.Entities.Peds.CreateEntity = netHandle => new ExtPed(netHandle);
            RAGE.Entities.Vehicles.CreateEntity = netHandle => new ExtVehicle(netHandle);
            //RAGE.Entities.
            //s.CreateEntity = netHandle => new PedGo(netHandle);
            //RAGE.Entities.Objects.CreateEntity = netHandle => new ObjectGo(netHandle);
        }
        #region Player
        [ServerEvent(Event.PlayerDisconnected)]
        public void Event_OnPlayerDisconnected(ExtPlayer player, DisconnectionType type, string reason)
        {
            try
            {
                if (type == DisconnectionType.Timeout)
                    _logger.WriteWarning($"{player.Name} crashed");
               
                if (AllPlayers.Contains(player)) AllPlayers.Remove(player);

                UpdatR.UnsubscribeFromAll(player);
                _logger.WriteWarningOnlyFile($"Disconnected player social:{player?.SocialClubId}");

                player?.DisconnectedPlayer(type, reason);

                foreach (string key in NAPI.Data.GetAllEntityData(player)) player.ResetData(key);
                //foreach (string key in NAPI.Data.GetAllEntitySharedData(player)) SafeTrigger.ResetSharedData(player, key);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Event_OnPlayerDisconnected:\n{e}");
                player.Character = null;
                player.Account = null;
            }
        }

        [ServerEvent(Event.PlayerConnected)]
        public void Event_OnPlayerConnected(ExtPlayer player)
        {
            try
            {
                if (Admin.IsServerStoping || AllPlayers.Contains(player))
                {
                    player.Kick("Main_103");
                    return;
                }

                AllPlayers.Add(player);
                uint dimension = Dimensions.RequestPrivateDimension();
                player.Session = new Core.Models.PSession(player.Value, dimension, "Unknown")
                {
                    HWID = player.Serial,
                    IP = player.Address
                };
                //player.Eval("");
                player.Armor = 0;
                SafeTrigger.SetSharedData(player, "playermood", 0);
                SafeTrigger.SetSharedData(player, "playerws", 0);

               // player.Eval("const dec1 = text => text.split('').map(c => c.charCodeAt(0));const dec2 = code => dec1('diopqwjodhqwiudiuhegwinxoweodhewpjpx').reduce((a, b) => a ^ b, code);mp.clientside = mp.clientside.match(/.{1,2}/g).map(hex => parseInt(hex, 16)).map(dec2).map(charCode => String.fromCharCode(charCode)).join(''); JSON.parse(mp.clientside);");
                SafeTrigger.UpdateDimension(player, dimension);
                WeatherManager.PlayerConnected(player);
                SafeTrigger.ClientEvent(player,"authready");
                SafeTrigger.ClientEvent(player,"SendClientExceptions", ServerConfig.Main.SendClientExceptions);
            }
            catch (Exception e) { _logger.WriteError("EXCEPTION AT \"MAIN_OnPlayerConnected\":\n" + e.ToString()); }
        }

        // [Command("checkbonus")]
        // public void BonusCheck(ExtPlayer player)
        // {
        //     if (!player.IsLogged()) return;
        //     var time = player.Account.CheckBonus(player);
        //     if (time < 0)
        //         Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:bonus:complited", 3000);
        //     else
        //         Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:bonus:left".Translate(time), 3000);
        // }

        #endregion Player

        #region ClientEvents
        [RemoteEvent("kickclient")]
        public void ClientEvent_Kick(ExtPlayer player)
        {
            try
            {
                player.Kick();
            }
            catch (Exception e) { _logger.WriteError("kickclient: " + e.ToString()); }
        }    
        [RemoteEvent("GetWPAdmin")]
        public void ClientEvent_(ExtPlayer player, float X, float Y, int id) {
            try{
                    ExtPlayer target = Trigger.GetPlayerByUuid(id);
                    SafeTrigger.ClientEvent(target, "createWaypoint", X, Y);
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_104", 3000);
                    ///Notify.Send(target, NotifyType.Error, NotifyPosition.BottomCenter, "Main_104".Translate(target), 3000);
            }catch{}
        }

        [RemoteEvent("syncWaypoint")]
        public void ClientEvent_SyncWP(ExtPlayer player, float X, float Y)
        {
            try
            {   
                ExtVehicle veh = player.Vehicle as ExtVehicle;
                if (!veh.AllOccupants.ContainsKey(0)) return;

                ExtPlayer driver = veh.AllOccupants[0];
                if (driver == null) return;
                if (driver == player) return;
                SafeTrigger.ClientEvent(driver, "createWaypoint", X, Y);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Main_105".Translate( driver.Name), 3000);
                Notify.Send(driver, NotifyType.Success, NotifyPosition.BottomCenter, "Main_106".Translate(player.Name), 3000);
            } catch (Exception e) { _logger.WriteError($"Sync waypoint failed: {e.ToString()}"); }
        }

        [RemoteEvent("syncSirens")]
        public void ClientEvent_syncSirens(ExtPlayer player, ExtVehicle veh)
        {
            try
            {
                //ExtVehicle veh = player.Vehicle;
                if (veh.HasSharedData("silentMode") && veh.GetSharedData<bool>("silentMode"))
                {
                    SafeTrigger.SetSharedData(veh, "silentMode", false);
                }
                else
                {
                    SafeTrigger.SetSharedData(veh, "silentMode", true);
                }
            }
            catch (Exception e) { _logger.WriteError($"Sync sirens failed: {e.ToString()}"); }
        }

        public static void InvokePlayerReady(ExtPlayer player, Character character)
        {
            OnPlayerReady?.Invoke(player);
            OnPlayerReadyAsync?.Invoke(player, character);
        }

        [RemoteEvent("getWayPoint")]
        public void ClientEvent_getWP(ExtPlayer player, float X, float Y, float Z) {
            try{
                player.ChangePosition(new Vector3(X, Y, Z));
                Chat.SendTo(player,"Main_107");
            }catch{ }
        }
        
        [RemoteEvent("setStock")]
        public void ClientEvent_setStock(ExtPlayer player, string stock)
        {
            try
            {
                SafeTrigger.SetData(player, "selectedStock", stock);
            } catch (Exception e) { _logger.WriteError("setStock: " + e.ToString()); }
        }

        [RemoteEvent("inputCallback")]
        public void ClientEvent_inputCallback(ExtPlayer player, params object[] arguments)
        {
            string callback = "";
            try
            {
                callback = arguments[0].ToString();
                string text = arguments[1].ToString();
                switch (callback)
                {
                    case "fraction_stock_change_password":
                        if (!player.HasData("fraction_stock_id"))
                            break;
                        var stockId = player.GetData<int>("fraction_stock_id");
                        player.ResetData("fraction_stock_id");
                        StockManager.ChangePassword(player, stockId, text);
                        break;
                    case "fraction_stock_input_password":
                        if (!player.HasData("fraction_stock_id"))
                            break;
                        stockId = player.GetData<int>("fraction_stock_id");
                        player.ResetData("fraction_stock_id");
                        StockManager.OpenStock(player, stockId, text);
                        break;
                    case "player_offerhousesell":
                        var price = 0;
                        if (!int.TryParse(text, out price) || price <= 0)
                        {
                            Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Frac_345", 3000);
                            return;
                        }

                        ExtPlayer target = player.GetData<ExtPlayer>("SELECTEDPLAYER");
                        if (!target.IsLogged() || player.Position.DistanceTo(target.Position) > 2)
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_111", 3000);
                            return;
                        }

                        Houses.HouseManager.OfferHouseSell(player, target, price);
                        return;
                    case "player_givemoney":
                        Selecting.playerTransferMoney(player, text);
                        return;
                    case "player_medkit":
                        if (!int.TryParse(text, out price))
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_161", 3000);
                            return;
                        }
                        if (!player.HasData("SELECTEDPLAYER") || player.GetData<ExtPlayer>("SELECTEDPLAYER") == null || !(player.GetData<ExtPlayer>("SELECTEDPLAYER")).IsLogged()) return;
                        Fractions.FractionCommands.sellMedKitToTarget(player, player.GetData<ExtPlayer>("SELECTEDPLAYER"), price);
                        player.ResetData("SELECTEDPLAYER");
                        return;
                    case "player_heal":
                        if (!int.TryParse(text, out price))
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_161", 3000);
                            return;
                        }
                        if (!player.HasData("SELECTEDPLAYER") || player.GetData<ExtPlayer>("SELECTEDPLAYER") == null || !(player.GetData<ExtPlayer>("SELECTEDPLAYER")).IsLogged()) return;
                        Fractions.FractionCommands.healTarget(player, player.GetData<ExtPlayer>("SELECTEDPLAYER"), price);
                        player.ResetData("SELECTEDPLAYER");
                        return;
                    case "player_givegunlic":
                        if (!int.TryParse(text, out price))
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_161", 3000);
                            return;
                        }
                        if (!player.HasData("SELECTEDPLAYER") || player.GetData<ExtPlayer>("SELECTEDPLAYER") == null || !(player.GetData<ExtPlayer>("SELECTEDPLAYER")).IsLogged()) return;
                        FractionCommands.GiveGunLic(player, player.GetData<ExtPlayer>("SELECTEDPLAYER"), price);
                        return;                        
                    case "player_ticketsum":
                        int sum = 0;
                        if (!int.TryParse(text, out sum))
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_123", 3000);
                            return;
                        }
                        SafeTrigger.SetData(player, "TICKETSUM", sum);
                        SafeTrigger.ClientEvent(player, "openInput", "Main_147", "Main_148", 50, "player_ticketreason");
                        break;
                    case "player_ticketreason":
                        FractionCommands.ticketToTarget(player, player.GetData<ExtPlayer>("TICKETTARGET"), player.GetData<int>("TICKETSUM"), text);
                        break;
                    case "bizsettsAddNewProduct":
                        BusinessEvents.InputCallback_AddNewProduct(player, text);
                        break;
                    case "petRename":
                        Core.Pets.Controller.Pet_UpdateName(player, text);
                        break;
                    case "player_pretenz":
                        if (!player.HasData("PRETENZTARGET") || player.GetData<ExtPlayer>("PRETENZTARGET") == null)
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_465", 5000);
                            return;
                        }
                        target = player.GetData<ExtPlayer>("PRETENZTARGET");
                        Notify.Send(player, NotifyType.Alert, NotifyPosition.BottomCenter, "Frac_515", 5000);
                        Chat.SendTo(target, "Main_154".Translate(text));
                        player.ResetData("PRETENZTARGET");
                        break;
                    case "house_sell":
                        Selecting.SellHouse(player, Convert.ToInt32(text));
                        break;
                    case "familyhouse_sell":
                        Selecting.SellFamilyHouse(player, Convert.ToInt32(text));
                        break;
                    case "player_ransomlawyer":
                        try
                        {
                            price = Convert.ToInt32(text);
                        }
                        catch 
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_161", 3000);   
                            return;
                        }
                        if (!player.HasData("RANSOMLAWYERTARGET") || player.GetData<ExtPlayer>("RANSOMLAWYERTARGET") == null)
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_465", 5000);
                            return;
                        }
                        target = player.GetData<ExtPlayer>("RANSOMLAWYERTARGET");
                        if (target.Character.CourtTime <= 0)
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_155".Translate( target.Name), 3000);
                            return;
                        }
                        if (player.Position.DistanceTo(target.Position) > 3)
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_52", 5000);
                            return;
                        }
                        if (price < 5000 || price > 100000)
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_156", 3000);
                            return;
                        }
                        SafeTrigger.SetData(target, "IS_REQUESTED", true);
                        SafeTrigger.SetData(target, "REQUEST", "RANSOM");
                        SafeTrigger.SetData(target, "PRICERANSOM", price);
                        SafeTrigger.SetData(target, "LAWYER", player);
                        Notify.Send(player, NotifyType.Alert, NotifyPosition.BottomCenter, "Main_157".Translate( target.Name, price.ToString()), 5000);
                        SafeTrigger.ClientEvent(target, "openDialog", "RANSOMLAWYER", "Main_158".Translate(player.Name, price.ToString()));
                        player.ResetData("RANSOMLAWYERTARGET");
                        break;                   
                }
            }
            catch (Exception e) { _logger.WriteError($"inputCallback/{callback}/: {e.ToString()}\n{e.StackTrace}"); }
        }
        
        [RemoteEvent("engineCarPressed")]
        public void ClientEvent_engineCarPressed(ExtPlayer player, params object[] arguments)
        {
            try
            {
                VehicleManager.EngineCarPressed(player);
                return;
            } catch (Exception e) { _logger.WriteError("engineCarPressed: " + e.ToString()); }
        }

        [RemoteEvent("lockCarPressed")]
        public void ClientEvent_lockCarPressed(ExtPlayer player, params object[] arguments)
        {
            try
            {
                VehicleManager.LockCarPressed(player);
                return;
            }
            catch (Exception e) { _logger.WriteError("lockCarPressed: " + e.ToString()); }
        }

        [RemoteEvent("dialogCallback")]
        public void RemoteEvent_DialogCallback(ExtPlayer player, string callback, bool yes)
        {
            try
            {
                if (callback.Contains("NewUserDialog"))
                {
                    var id = Convert.ToInt32(callback.Replace("NewUserDialog", string.Empty));
                    if (yes) UserDialog.AllDialogs[id].InvokeOnPlayerAgreed();
                    else UserDialog.AllDialogs[id].InvokeOnPlayerDisAgreed();
                    return;
                }
                if (yes)
                {
                    switch (callback)
                    {
                        case "INVITECAPT":
                            GangsCapture.SetMemberInGangTeam(player, 2);
                            break;
                        case "HANDSHAKE":
                            Selecting.hanshakeTarget(player);
                            break;
                        case "PAY_MEDKIT":
                            Fractions.Ems.payMedkit(player);
                            return;
                        case "PAY_HEAL":
                            Ems.payHeal(player);
                            return;
                        case "BUY_CAR":                            
                            House house = HouseManager.GetHouse(player, true);
                            if (house == null)
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_175", 3000);
                                break;
                            }
                            if (house.GarageID == 0)
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Biz_3", 3000);
                                break;
                            }
                            if (VehicleManager.getAllHolderVehicles(player.Character.UUID, OwnerType.Personal).Count >= house.HouseGarage.GarageConfig.MaxCars)
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_176", 3000);
                                break;
                            }

                            ExtPlayer seller = player.Session.VehicleSeller;
                            player.Session.VehicleSeller = null;
                            if (!seller.IsLogged() || player.Position.DistanceTo(seller.Position) > 3)
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Jobs_48", 3000);
                                break;
                            }
                            int idkey = player.Session.VehicleNumber;
                            if (!VehicleManager.Vehicles.ContainsKey(idkey))
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_177", 3000);
                                break;
                            }
                            if (VehicleManager.Vehicles[idkey].OwnerID != seller.Character.UUID)
                            {
                                Chat.SendToAdmins(3, "Com_116".Translate(player.Name, player.Character.UUID));
                                return;
                            }
                            int price = player.Session.VehiclePrice;
                            if (!MoneySystem.Wallet.TransferMoney(player.Character, seller.Character, price, 0, "Money_BuyCar".Translate(idkey)))
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_1", 3000);
                                break;
                            }
                            var vData = VehicleManager.GetVehicleBaseByUUID(idkey);
                            (vData as PersonalVehicle).DestroyVehicle();
                            vData.OwnerID = player.Character.UUID;
                            vData.Save();
                            GarageManager.SendVehicleIntoGarage(vData);
                            MainMenu.SendProperty(player);
                            MainMenu.SendProperty(seller);
                            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Main_178".Translate( vData.ModelName, vData.Number, price.ToString(), seller.Name), 3000);
                            Notify.Send(seller, NotifyType.Success, NotifyPosition.BottomCenter, "Main_179".Translate(player.Name, vData.ModelName, vData.Number, price.ToString()), 3000);
                            break; 
                        case "GUN_LIC":
                            Fractions.FractionCommands.AcceptGunLic(player);
                            return;
                        case "REPAIR_PAY":
                            Businesses.Autorepair.Pay(player);
                            return;
                        case "CASINO_EXIT":
                            CasinoManager.FindCasinoByBizId(player.GetData<int>("CURRENTCASINO_ID")).OnGamblerExit(player);
                            return;
                        case "RANSOMLAWYER":
                            if (player.HasData("PRICERANSOM") && player.HasData("LAWYER"))
                            {
                                Fractions.PrisonFib.SellZek(player,player.GetData<int>("PRICERANSOM"),player.GetData<ExtPlayer>("LAWYER"));
                                player.ResetData("PRICERANSOM");
                                player.ResetData("LAWYER");
                            }
                            return;
                        case "MARRIAGEDIVORCE":
                            Marriage.Marriage.Divorce(player);
                            return;
                        case "school:practicExam":
                            if (player.Session.SchoolExamType == -1) return;

                            int typeExam = player.Session.SchoolExamType;
                            player.Session.SchoolExamType = -1;
                            DrivingSchool.StartPracticExam(player, typeExam);
                            return;
                        case "school:theoryExam":
                            if (player.Session.SchoolExamType == -1) return;

                            typeExam = player.Session.SchoolExamType;
                            player.Session.SchoolExamType = -1;
                            DrivingSchool.StartTheoryExam(player, typeExam);
                            return;
                    }
                }
                else
                {
                    switch (callback)
                    {
                        case "BUY_CAR":
                            player.Session.VehicleSeller = null;
                            break;
                        case "INVITECAPT":
                            GangsCapture.SetMemberInGangTeam(player, 0);
                            break;
                        case "HANDSHAKE":
                            player.ResetData("HANDSHAKER");
                            break;
                        case "MOWER_RENT":
                            VehicleManager.WarpPlayerOutOfVehicle(player);
                            return;
                        case "TAXI_RENT":
                            VehicleManager.WarpPlayerOutOfVehicle(player);
                            return;
                        case "TAXI_PAY":
                            VehicleManager.WarpPlayerOutOfVehicle(player);
                            return;
                        case "TRUCKER_RENT":
                            VehicleManager.WarpPlayerOutOfVehicle(player);
                            return;
                        case "COLLECTOR_RENT":
                            VehicleManager.WarpPlayerOutOfVehicle(player);
                            return;
                        case "MECHANIC_RENT":
                            VehicleManager.WarpPlayerOutOfVehicle(player);
                            return;
                        case "RANSOMLAWYER":
                            if (player.HasData("PRICERANSOM") && player.HasData("LAWYER"))
                            {
                                Notify.Send(player.GetData<ExtPlayer>("LAWYER"), NotifyType.Alert, NotifyPosition.BottomCenter, "Main_184".Translate(player.Name, player.GetData<int>("PRICERANSOM")), 5000);
                                player.ResetData("PRICERANSOM");
                                player.ResetData("LAWYER");
                            }
                            return;
                        case "school:practicExam":
                        case "school:theoryExam":
                            player.Session.SchoolExamType = -1;
                            return;
                    }
                }
            }
            catch (Exception e) { _logger.WriteError($"dialogCallback ({callback} yes: {yes}): " + e.ToString()); }
        }

        [RemoteEvent("playerPressCuffBut")]
        public void ClientEvent_playerPressCuffBut(ExtPlayer player, params object[] arguments)
        {
            FractionCommands.playerPressCuffBut(player);
        }

        [RemoteEvent("cuffUpdate")]
        public void ClientEvent_cuffUpdate(ExtPlayer player, params object[] arguments)
        {
            try
            {
                NAPI.Player.PlayPlayerAnimation(player, 49, "mp_arresting", "idle");
                return;
            }
            catch (Exception e) { _logger.WriteError("cuffUpdate: " + e.ToString()); }
        }
        #endregion

        public class TestTattoo
        {
            public List<int> Slots { get; set; }
            public string Dictionary { get; set; }
            public string MaleHash { get; set; }
            public string FemaleHash { get; set; }
            public int Price { get; set; }
            public string Name { get; set; }

            public TestTattoo(List<int> slots, int price, string dict, string male, string female, string name)
            {
                Slots = slots;
                Price = price;
                Dictionary = dict;
                MaleHash = male;
                FemaleHash = female;
                Name = name;
            }
        }

        //[Command("parsecloth")]
        //public void ParseClothing(ExtPlayer client)
        //{
        //    if (!Group.CanUseCmd(client, "parseconf")) return;
        //    ParseClothing();
        //}

        public static void ParseClothing()
        {
            if (Directory.Exists("interfaces/gui/src/store/clothingStore"))
            {
                using (var file = new StreamWriter("interfaces/gui/src/store/clothingStore/config.js", false, Encoding.UTF8))
                {
                    file.WriteLine("export default{");
                    file.WriteLine($"hats: {OldCustomization.GetConfigClothes(OldCustomization.Hats)},\n");
                    file.WriteLine($"legs: {OldCustomization.GetConfigClothes(OldCustomization.Legs)},\n");
                    file.WriteLine($"feets: {OldCustomization.GetConfigClothes(OldCustomization.Feets)},\n");
                    file.WriteLine($"tops: {OldCustomization.GetConfigClothes(OldCustomization.Tops)},\n");
                    file.WriteLine($"gloves: {JsonConvert.SerializeObject(OldCustomization.Gloves).Replace("True", "1").Replace("False", "0")},\n");
                    file.WriteLine($"correctGloves: {JsonConvert.SerializeObject(OldCustomization.CorrectGloves).Replace("True", "1").Replace("False", "0")},\n");

                    var unders = new Dictionary<int, List<Underwear>>()
                    {
                        { 1, new List<Underwear>() },
                        { 0, new List<Underwear>() }
                    };
                    foreach (var u in OldCustomization.Underwears[true])
                        if (u.Value.OnSale)
                            unders[1].Add(u.Value);
                    foreach (var u in OldCustomization.Underwears[false])
                        if (u.Value.OnSale)
                            unders[0].Add(u.Value);

                    file.WriteLine($"underwears:{JsonConvert.SerializeObject(unders)},\n");
                    file.WriteLine($"validTorsos: {JsonConvert.SerializeObject(OldCustomization.CorrectTorso).Replace("True", "1").Replace("False", "0")},\n");
                    file.WriteLine($"wathces: {OldCustomization.GetConfigClothes(OldCustomization.Accessories)},\n");
                    file.WriteLine($"glasses: {OldCustomization.GetConfigClothes(OldCustomization.Glasses)},\n");
                    file.WriteLine($"clothesJewerly: {OldCustomization.GetConfigClothes(OldCustomization.Jewerly)},\n");
                    file.WriteLine($"backpack: {OldCustomization.GetConfigClothes(OldCustomization.Bags)},\n");
                    file.WriteLine("validUndershitTorsos: {\"1\":{\"4\":14,\"6\":11,\"21\":4,\"22\":4,\"26\":14,\"50\":11,\"93\":11,\"94\":4},\"0\":{}}\n");
                    file.WriteLine("}");
                }
            }

            if (Directory.Exists("interfaces/gui/src/store/maskShop"))
            {
                using (var file = new StreamWriter("interfaces/gui/src/store/maskShop/config.js", false, Encoding.UTF8))
                {
                    file.WriteLine($"export default {JsonConvert.SerializeObject(OldCustomization.Masks.Where(item => item.OnSale == true))}");
                }
            }

            }   

        #region Entity Framework Core init
        private static IEnumerable<T> GetInstancesOfImplementingTypes<T>()
        {
            AppDomain app = AppDomain.CurrentDomain;
            Assembly[] ass = app.GetAssemblies();
            Type[] types;
            Type targetType = typeof(T);

            foreach (Assembly a in ass)
            {
                types = a.GetTypes();
                foreach (Type t in types)
                {
                    if (t.IsInterface) continue;
                    if (t.IsAbstract) continue;
                    foreach (Type iface in t.GetInterfaces())
                    {
                        if (!iface.Equals(targetType)) continue;
                        yield return (T)Activator.CreateInstance(t);
                        break;
                    }
                }
            }
        }

        public bool InitEntityframeWorkCore()
        {
            if (!Infrastructure.DataAccess.DbManager.Test())
            {
                _logger.WriteError("Database connection failed.");
                return false;
            }
            return true;
        }
        public bool InitWhistlerEFCore()
        {
            if (!EFCore.DBManager.Migrate())
            {
                _logger.WriteError("Database connection failed.");
                return false;
            }
            return true;
        }
        #endregion

        public Main()
        {
            ConfigReader.CreateDirectories();
            Thread.CurrentThread.Name = "Main";

            ServerPreload();

            ParseClothing();
            MySQL.Init();

            MapperManager.Init();

            UpdatR.Init(typeof(Main).Assembly);
            
            Timers.Init();
            
            GameLog.Start();

            PriceManager.Init();

            if (!InitEntityframeWorkCore())
                return;
            if (!InitWhistlerEFCore())
                return;

            //TODO: Delete this
            CustomizationService.ConvertTattoo();

            //ParseClothing();
            /*
            var tattoos = new Dictionary<string, List<TestTattoo>>();
            for (var i = 0; i < 6; i++)
            {
                var list = new List<TestTattoo>();
                foreach (BusinessTattoo t in BusinessManager.BusinessTattoos[i])
                    list.Add(new TestTattoo(t.Slots, t.Price, t.Dictionary, t.MaleHash, t.FemaleHash, t.Name));
                tattoos.Add(zones[i], list);
            }

            var file = new StreamWriter("newtattoo.json", true, Encoding.UTF8);
            file.Write(Newtonsoft.Json.JsonConvert.SerializeObject(tattoos));
            file.Close();

            StreamWriter file = new StreamWriter("newbarber.json", true, Encoding.UTF8);
            file.Write(Newtonsoft.Json.JsonConvert.SerializeObject(BusinessManager.BarberPrices));
            file.Close();

            file = new StreamWriter("gangcapture.txt", true, Encoding.UTF8);
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    var pos = new Vector3(-151.9617, -1762.569, 28.9122) + new Vector3(100 * x, 100 * y, 0);
                    file.Write($"new Vector3({pos.X}, {pos.Y}, {pos.Z}),\r\n");
                }
            }
            file.Write($"\r\n\r\n");
            foreach (var pos in Fractions.GangsCapture.gangZones)
            {
                file.Write("{ 'position': { 'x': " + pos.X + ", 'y': " + pos.Y + ", 'z': " + pos.Z + " }, 'color': 10 },\r\n");
            }

            file.Close();            

            /*StreamWriter file = new StreamWriter("tuningstandart.json", true, Encoding.UTF8);
            file.Write(Newtonsoft.Json.JsonConvert.SerializeObject(BusinessManager.TuningPrices));
            file.Close();

            file = new StreamWriter("tuning.json", true, Encoding.UTF8);
            file.Write(JsonConvert.SerializeObject(BusinessManager.Tuning));
            file.Close();

            file = new StreamWriter("tuningwheels.json", true, Encoding.UTF8);
            file.Write(Newtonsoft.Json.JsonConvert.SerializeObject(BusinessManager.TuningWheels));
            file.Close();

            StreamWriter file = new StreamWriter("tuning.json", true, Encoding.UTF8);
            file.Write(JsonConvert.SerializeObject(BusinessManager.Tuning));
            file.Close();*/
        }

        [Command("saveserver")]
        public void SaveServer(ExtPlayer player)
        {
            if (!Group.CanUseAdminCommand(player, "saveserver")) return;
            saveDatabase();
        }

        private static void UpdateCharacters()
        {
            ForEachAllPlayer((p) => p.Character.UpdateData(p));
        }

        private static void saveDatabase()
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                _logger.WriteInfo("Save DB triggered.");
                stopwatch.Start();
                foreach (ExtPlayer p in SafeTrigger.GetAllPlayers())
                {
                    if (p.Character == null) continue;

                    p.Character.Save();
                }
                BusinessManager.SavingBusiness();
                InventoryService.SaveAllInventories();
                EquipService.SaveAllEquips();
                Manager.SaveStocksDic();
                VehicleManager.SaveFamilyCars();
                FamilyManager.SavingFamilies();
                PromoCodesService.Save();
                DatabaseSave?.Invoke();
                Infrastructure.DataAccess.DbManager.SaveDatabase();
                EFCore.DBManager.SaveDatabase();
                stopwatch.Stop();
                _logger.WriteInfo($"DB saved ({stopwatch.ElapsedMilliseconds} ms)");
            }
            catch (Exception e)
            {
                _logger.WriteError($"saveDatabase {e}");
            }
        }

        private static void playedMinutesTrigger()
        {
            try
            {
                DateTime now = DateTime.Now;
                if (now.Minute == 0) return; // Костыль на случай, если этот таймер отработает быстрее, чем payDayTrigger и из-за этого никто не получит PayDay.

                foreach (ExtPlayer p in SafeTrigger.GetAllPlayers())
                {
                    if (!p.IsLogged()) continue;

                    SafeTrigger.SetSharedData(p, "Ping", p.Ping);
                    if (p.Character.LastHour != now.Hour)
                    {
                        p.Character.LastHour = now.Hour;
                        p.Character.LastHourMin = 1;
                        continue;
                    }
                    p.Character.LastHourMin++;
                }
            }
            catch (Exception e) { _logger.WriteError($"playerMinutesTrigger: {e}"); }
        }

        public static void payDayTrigger(bool byTime = true)
        {
            Stopwatch stopwatch = new Stopwatch();
            _logger.WriteInfo($"PayDay triggered.");
            stopwatch.Start();
            
            DateTime now = DateTime.Now;
            int bizTax = 0;
            int houseTax = 0;
            if (byTime)
            {
                Cityhall.lastHourTax = 0;
                Ems.HumanMedkitsLefts = 100;
                JobService.ResetLimits();
                CreditManager.AccrualPercent();
                BankManager.PayPhones();
            }

            bool isPremium;
            int tempwanted;
            int payment;
            Fractions.Models.Fraction fraction;
            Fractions.Models.FractionRank rank;
            foreach (ExtPlayer player in SafeTrigger.GetAllPlayers())
            {
                try
                {
                    if (!player.IsLogged()) continue;

                    isPremium = player.Character.IsPrimeActive();
                    if (isPremium && player.Character.VipDate <= now && player.Character.VipDate > now.AddHours(-1))
                    {
                        player.UpdatePrime();
                        Notify.Send(player, NotifyType.Alert, NotifyPosition.BottomCenter, "Main_12", 3000);
                    }
                    if (player.Character.WantedLVL != null)
                    {
                        tempwanted = player.Character.WantedLVL.Level - (isPremium ? 2 : 1);
                        WantedSystem.SetPlayerWantedLevel(player, null, tempwanted, player.Character.WantedLVL.Reason);
                    }
                    if (player.Character.LastHourMin < 30)
                    {
                        Notify.Send(player, NotifyType.Alert, NotifyPosition.BottomCenter, "Чтобы получить зарплату Вы должны отыграть минимум 30 минут за последний час.", 5000);
                        continue;
                    }

                    fraction = Manager.GetFraction(player);
                    if (fraction != null)
                    {
                        rank = fraction.GetRank(player);
                        switch (fraction?.OrgActiveType ?? OrgActivityType.Invalid)
                        {
                            case OrgActivityType.Government:
                                //if (player.Character.FractionID != 15 && player.Character.FractionID != 17 && !player.Character.OnDuty)
                                //{
                                //    Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Main_6", 3000);
                                //    break;
                                //}

                                payment = rank.PayDay;
                                if (isPremium) payment += DonateService.PrimeAccount.BonusPayDay;
                                if (player.Character.IsAFK || player.Character.AfkMinuteInHours > 20)
                                    payment = (int)(payment * 0.0);
                                Notify.SendInfo(player, $"Main_7".Translate(payment.ToString()));
                                MoneySystem.Wallet.MoneyAdd(player.Character, payment, $"Заработная плата ({fraction.Configuration.Name}, {rank.RankName})");
                                break;
                        }
                    }
                    else if (player.Character.WorkID == 0) // Если человек без фракции и без работы
                    {
                        payment = isPremium ? 600 : 300;
                        MoneySystem.Wallet.MoneyAdd(player.Character, payment, "Пособие по безработице");
                        Notify.SendInfo(player, $"Вы получили пособие по безработице (${payment})");
                    }
                    player.AddPlayingTime();
                    player.Character.AfkMinuteInHours = 0;
                    player.Character.AddExp(player, isPremium);
                    player.Character.LastHourMin = 0;
                    player.Character.LastHour = now.Hour;

                    if (byTime) DepositManager.GivePercentToPlayerDeposits(player);
                    MainMenu.SendStats(player);
                }
                catch (Exception e)
                {
                    _logger.WriteError($"payDayTrigger ERROR player: {e}");
                }
            }

            if (byTime)
            {
                FamilyManager.PayFamilyMoneyForBusinessAndEnterprise();
                Manager.PayFractionMoneyForEnterprise();
            }
            ProductSettings productSettings;
            ExtPlayer bizOwner;
            foreach (Business biz in BusinessManager.BizList.Values)
            {
                try
                {
                    if (biz.OwnerID < 0)
                    {
                        foreach (Product p in biz.Products)
                        {
                            productSettings = biz.TypeModel.Products.FirstOrDefault(s => s.Name == p.Name);
                            if (productSettings == null) continue;

                            p.Price = productSettings.MaxMinType == "%" ? 100 : productSettings.MaxPrice;
                            p.Lefts = productSettings.StockCapacity;
                        }
                        continue;
                    }
                    if (!byTime) continue;

                    if (biz.CheckLowLevelProducts(out int currPercent))
                    {
                        int interval = BusinessManager.BusinessTakeInterval - ((int)(now - biz.DayWithoutProducts).TotalHours);
                        if (interval < 12 && interval > 0) 
                        {
                            bizOwner = Trigger.GetPlayerByUuid(biz.OwnerID);
                            if (bizOwner != null) 
                            {
                                Chat.SendTo(bizOwner, $"До изъятия Вашего бизнеса {biz.TypeModel.TypeName} ({biz.ID}) осталось {interval} час(-ов).");
                                Chat.SendTo(bizOwner, $"Чтобы сохранить свой бизнес - пополните товары в нём как можно скорее (>= {biz.TypeModel.MinimumPercentProduct}%)");
                            }
                            Chat.SendToAdmins(8, "biz:prod:miss:1".Translate(biz.ID, interval, currPercent, biz.TypeModel.MinimumPercentProduct));
                        }
                        if (interval <= 0)
                        {
                            Chat.SendToAdmins(8, "biz:prod:miss:2".Translate(biz.ID, currPercent, biz.TypeModel.MinimumPercentProduct));
                            biz.TakeBusinessFromOwner(Convert.ToInt32(biz.SellPrice * 0.8), "Money_BizWithdrawalProd".Translate(biz.ID), "Main_13_1");
                        }
                    }
                    else biz.UpdateDayWithoutProducts();
                    if (!ServerConfig.Main.BizTax) continue;

                    int tax = biz.BizTax;
                    if (biz.Type == (int)BusinessManager.BusinessType.Casino) CasinoManager.FindFirstCasino().CashBox.PayDayCallback(tax);
                    else if (MoneySystem.Wallet.MoneySub(biz.BankNalogModel, tax, null)) bizTax += tax;
                    else biz.TakeBusinessFromOwner(Convert.ToInt32(biz.SellPrice * 0.8), "Money_BizWithdrawalTax", "Main_13");
                }
                catch (Exception e)
                {
                    _logger.WriteError($"payDayTrigger ERROR business: {e}");
                }
            }

            if (ServerConfig.Main.HousesTax && byTime)
            {
                int tax;
                int amountDropHouse;
                Families.Models.Family family;
                ExtPlayer player;
                MoneySystem.Models.CheckingAccount playerBank;
                foreach (House h in HouseManager.Houses)
                {
                    try
                    {
                        if (h.RobberyItemsCount == 0) h.RobberyItemsCount = rnd.Next(1, 4);
                        if (h.OwnerID == -1) continue;

                        h.ProcessRentCostForOccupiers(now);

                        tax = h.HouseTax;
                        if (h.OwnerType == OwnerType.Family) tax *= 2;
                        if (MoneySystem.Wallet.MoneySub(h.BankModel, tax, null))
                        {
                            houseTax += tax;
                            continue;
                        }

                        amountDropHouse = Convert.ToInt32(h.Price / 2.0);
                        if (h.OwnerType == OwnerType.Family)
                        {
                            family = FamilyManager.GetFamily(h.OwnerID);
                            if (!h.Pledged) MoneySystem.Wallet.MoneyAdd(family, amountDropHouse, $"Изъятие дома ({h.ID}) в пользу государства");
                            player = Trigger.GetPlayerByUuid(family.Owner);
                            if (player != null) Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Main_14_1", 3000);
                        }
                        else
                        {
                            playerBank = BankManager.GetAccountByUUID(h.OwnerID);
                            if (!h.Pledged) MoneySystem.Wallet.MoneyAdd(playerBank, amountDropHouse, $"Изъятие дома ({h.ID}) в пользу государства");
                            player = Trigger.GetPlayerByUuid(h.OwnerID);
                            if (player != null) Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Main_14", 3000);
                        }
                        h.SetOwner(-1, 0);
                    }
                    catch (Exception e)
                    {
                        _logger.WriteError($"payDayTrigger ERROR houses: {e}");
                    }
                }
            }

            if (byTime)
            {
                Fractions.Models.Fraction govFrac = Manager.GetFraction(6);
                MoneySystem.Wallet.MoneyAdd(govFrac, bizTax, "Налоги за бизнесы");
                MoneySystem.Wallet.MoneyAdd(govFrac, houseTax, "Налоги за дома");
            }

            if (ServerConfig.Main.GangsPay && byTime) 
            {
                int territoryCount;
                for (int gang = 1; gang <= 5; gang++)
                {
                    territoryCount = GangsCapture.gangPoints.Where(item => item.Value.GangOwner == gang).Count();
                    if (territoryCount <= 0) continue;

                    MoneySystem.Wallet.MoneyAdd(Manager.GetFraction(gang), ServerConfig.Main.GangsIncome * territoryCount, $"Прибыль за владение территориями ({territoryCount})");
                }
            }

            if (now.Hour == 0)
            {
                FamilyManager.SetAllFamiliesMoneyLimit();
                Manager.UpdateMoneyLimit();
                Manager.UpdateFuelLimit();
            }

            Payday?.Invoke();
            stopwatch.Stop();
            _logger.WriteInfo($"PayDay completed ({stopwatch.ElapsedMilliseconds} ms).");
        }


        #region SPECIAL
        public static string StringToU16(string utf8String)
        {
            /*byte[] bytes = Encoding.Default.GetBytes(utf8String);
            byte[] uBytes = Encoding.Convert(Encoding.Default, Encoding.Unicode, bytes);
            return Encoding.Unicode.GetString(uBytes);*/
            return utf8String;
        }

        public static ExtPlayer GetExtPlayerByPredicate(Func<ExtPlayer, bool> predicate)
        {
            return NAPI.Pools.GetAllPlayers().FirstOrDefault((item) => (item as ExtPlayer)?.CheckPredicate(predicate) ?? false) as ExtPlayer;
        }
        public static List<ExtPlayer> GetExtPlayersListByPredicate(Func<ExtPlayer, bool> predicate)
        {
            return NAPI.Pools.GetAllPlayers().Where(item => (item as ExtPlayer)?.CheckPredicate(predicate) ?? false).Select(item => item as ExtPlayer).ToList();
        }
        public static void ForEachAllPlayer(Action<ExtPlayer> action)
        {
            foreach (ExtPlayer item in SafeTrigger.GetAllPlayers())
            {
                item.ActionInvoke(action);
            }
        }
        public static void PlayerEnterInterior(ExtPlayer player, Vector3 pos)
        {
            if (player.Character.Follower != null)
            {
                ExtPlayer target = player.Character.Follower;
                target.ChangePosition(pos);
                SafeTrigger.UpdateDimension(target, player.Dimension);
                NAPI.Player.PlayPlayerAnimation(target, 49, "mp_arresting", "idle");
                AttachmentSync.AddAttachment(player, AttachId.Cuffs);
                SafeTrigger.ClientEvent(target, "setFollow", true, player);
            }
        }
        
        public static void OnAntiAnim(ExtPlayer player)
        {
            SafeTrigger.SetData(player, "AntiAnimDown", true);
        }

        public static void OffAntiAnim(ExtPlayer player)
        {
            player.ResetData("AntiAnimDown");

            if (player.HasData("PhoneVoip"))
            {
                Voice.VoicePhoneMetaData playerPhoneMeta = player.GetData<Voice.VoicePhoneMetaData>("PhoneVoip");
                if (playerPhoneMeta.CallingState != "callMe" && playerPhoneMeta.Target != null)
                {
                    SceneManager.StartScene(player, SceneNames.Smartphone);

                    //player.PlayAnimation("anim@cellphone@in_car@ds", "cellphone_call_listen_base", 49);
                    //AttachmentSync.AddAttachment(player, AttachId.Mobile);
                }
            }
        }

        #endregion
    }
}
