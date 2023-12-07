using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GTANetworkAPI;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Whistler.Houses;
using Whistler.GUI;
using Whistler.SDK;
using Whistler.Families;
using ServerGo.Casino.Business;
using Whistler.Helpers;
using Whistler.VehicleSystem;
using Whistler.Phone;
using Whistler.Inventory.Models;
using Whistler.Inventory;
using Whistler.Jobs.ImpovableJobs;
using Whistler.Core.ReportSystem;
using Whistler.Inventory.Enums;
using Whistler.Fractions;
using Whistler.DoorsSystem;
using System.Text.RegularExpressions;
using Whistler.Fractions.PDA;
using Whistler.NewDonateShop;
using Whistler.MoneySystem;
using Whistler.Phone.Forbes;
using Whistler.Customization;
using Whistler.Customization.Models;
using Whistler.MoneySystem.Models;
using Whistler.Common;
using Whistler.PersonalEvents;
using System.Threading;
using Whistler.Entities;

namespace Whistler.Core.Character
{
    public class Character : CharacterData
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Character));
        private static Random Rnd = new Random();
        private static Regex regex = new Regex(@"^[A-Za-z]{3,25}$");
        private class CharacterSpawns
        {
            public CharacterSpawns(string key, string name, string subname, float x, float y, int spawnType)
            {
                this.key = key;
                this.name = name;
                this.subname = subname;
                this.x = x;
                this.y = y;
                this.spawnType = spawnType;
            }

            public string key { get; set; }
            public string name { get; set; }
            public string subname { get; set; }
            public float x { get; set; }
            public float y { get; set; }
            public int spawnType { get; set; }
        }
        public static Action<ExtPlayer> OnPlayerSpawned;
        public PhoneTemporaryData PhoneTemporary { get; } = new PhoneTemporaryData();

        public Character(string firstName, string lastName, int accountId, int customizationId, ClothesDTO clothes) : base(firstName, lastName, accountId, customizationId, clothes)
        {

        }
        public Character(DataRow row) : base(row)
        {

        }

        public static void SetLastUUID(int uuid)
        {
            _lastUUID = uuid;
        }

        internal void UpdateCustomization(int id)
        {
            CustomizationId = id;
            MySQL.Query("UPDATE `characters` SET `customizationid`=@prop0 WHERE `uuid`=@prop1", CustomizationId, UUID);
        }

        private void CreateNewDonateInventroy(ExtPlayer player)
        {
            DonateInventory = DonateService.CrateInventory();
            MySQL.Query("UPDATE `characters` SET `donateInventoryId`=@prop0 WHERE `uuid`=@prop1", DonateInventory.Id, UUID);
            DonateInventory.Subscribe(player);
        }

        public bool IsPrimeActive()
        {
            return VipDate > DateTime.UtcNow;
        }
        public int GetPrimeDays()
        {
            return IsPrimeActive() ? (int)(VipDate - DateTime.UtcNow).TotalDays : 0;
        }

        public void AddPrime(int days)
        {
            if (VipDate > DateTime.UtcNow) VipDate = VipDate.AddDays(days);
            else VipDate = DateTime.UtcNow.AddDays(days);

            MySQL.Query("UPDATE `characters` SET `vipdate`=@prop0 WHERE uuid = @prop1", MySQL.ConvertTime(VipDate), UUID);
        }

        public void LoadSpawnPoints(ExtPlayer player, int index)
        {
            // var points = new List<string>();
            List<CharacterSpawns> spawnPoints = new List<CharacterSpawns>();

            House familyHose = HouseManager.GetHouse(FamilyID, OwnerType.Family);
            string familyName = familyHose == null ? "None" : FamilyManager.GetFamilyName(FamilyID);
            if(familyHose != null) spawnPoints.Add(new CharacterSpawns("s1", "Семья", familyName == "None" ? "" : familyName, familyHose.Position.X, familyHose.Position.Y, 0));
            //spawnPoints.Add(new CharacterSpawns("s1", "Семья", familyName == "None" ? "" : familyName, (familyHose != null) ? (familyHose.Position.X) : 0, (familyHose != null) ? (familyHose.Position.Y) : 0));
            spawnPoints.Add(new CharacterSpawns("s2", "Место выхода", "", SpawnPos.X, SpawnPos.Y, 1));
            House house = HouseManager.GetHouse(player);
            if(house != null) spawnPoints.Add(new CharacterSpawns("s3", "Личный дом", house.ID.ToString(), house.Position.X, house.Position.Y, 2));
            //spawnPoints.Add(new CharacterSpawns("s3", "Личный дом", house == null ? "" : house.ID.ToString(), (house != null) ? house.Position.X : 0, (house != null) ? house.Position.Y : 0));
            if(FractionID > 0) 
            {
                Vector3 position = Manager.getFracPos(FractionID);
                spawnPoints.Add(new CharacterSpawns("s4", "Организация", Manager.getName(FractionID), position.X, position.Y, 3));
            }
            // spawnPoints.Add(new CharacterSpawns("s4", "Организация", (FractionID < 1) ? "" : Manager.getName(FractionID), (FractionID > 0) ? Manager.getFracPos(FractionID).X : 0, (FractionID > 0) ? Manager.getFracPos(FractionID).Y : 0));

            // points.Add(familyName == "None" ? "" : familyName); // family
            // points.Add(JsonConvert.SerializeObject(SpawnPos)); // last spawn
            // var house = HouseManager.GetHouse(player);
            // points.Add(house == null ? "" : house.ID.ToString()); // house
            // var frac = FractionID < 1 ? "" : Manager.getName(FractionID);
            // points.Add(frac); // frac

            // var spawnPoints = new List<CharacterSpawns>();
            //spawnPoints.Add(new CharacterSpawns("s1", "Тест", "Тест2", 1000, 1000));
            /*
                new CharacterSpawns("s1", "Тест", "Тест2", 1000, 1000),
                new CharacterSpawns("s2", "Тест2", "Тест22", 1500, 1000),
                new CharacterSpawns("s3", "Тест3", "Тест23", 1000, 1500),
                new CharacterSpawns("s4", "Тест4", "Тест24", 2000, 2000)
            */
            SafeTrigger.ClientEvent(player,"auth:spawn:select", spawnPoints, index);
        }

        //arrest timer
        public void SetArrestTimer(ExtPlayer player)
        {
            ResetArrestTimer(player);
            if (ArrestDate <= DateTime.Now) return;

            int period = Convert.ToInt32((ArrestDate - DateTime.Now).TotalMilliseconds);
            SafeTrigger.ClientEvent(player,"hud:arrest:timer:update", period, "test reason");
            SafeTrigger.SetData(player, "ARREST_TIMER", Timers.StartOnce(period, () => ReleaseArrest(player)));
        }

        public void ResetArrestTimer(ExtPlayer player)
        {
            if (player.HasData("ARREST_TIMER"))
            {
                Timers.Stop(player.GetData<string>("ARREST_TIMER")); // still not fixed
                player.ResetData("ARREST_TIMER");
            }
        }
        private void ReleaseArrest(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            if (ArrestDate > DateTime.Now.AddMinutes(3))
                SetArrestTimer(player);
            else
                PoliceArrests.ReleasePlayer(player, null, 0);
        }

        public void UpdateData(ExtPlayer player)
        {            
            Health = player.Health;
            if (IsSpawned && !IsAlive)
            {
                SpawnPos = Ems.GetRandomSpawnPointAfterDeath();
                Health = 20;
            }
            else if(BusinessInsideId > -1)
            {
                Vector3 position = BusinessManager.BizList[BusinessInsideId].EnterPoint;
                SpawnPos = position + new Vector3(0, 0, 1.12);
            }
            else if (ExteriorPos != null)
            {
                Vector3 position = ExteriorPos;
                SpawnPos = position + new Vector3(0, 0, 1.12);
            }
            else if (InsideGarageID != -1)
            {
                var garage = GarageManager.Garages[InsideGarageID];
                SpawnPos = garage.Position + new Vector3(0, 0, 1.12);
            }else if (InsideHouseID != -1)
            {
                House house = HouseManager.Houses.FirstOrDefault(h => h.ID == InsideHouseID);
                if (house != null)
                    SpawnPos = house.Position + new Vector3(0, 0, 1.12);
            } else if (MP.RoyalBattle.RoyalBattleService.IsInBattle(player))
            {
                SpawnPos = MP.RoyalBattle.Configs.Configurations.ExitPosition + new Vector3(0, 0, 0.5);
            }
            else
            {
                SpawnPos = (player.IsInVehicle)
                ? player.Vehicle.Position + new Vector3(0, 0, 0.5)
                : player.Position;
            }
        }

        public void Save()
        {
            try
            {
                //Customization.SaveCharacter(UUID);

                List<int> vehicles = VehicleManager.getAllHolderVehicles(UUID, OwnerType.Personal);
                foreach (int veh in vehicles) VehicleManager.Vehicles[veh].Save();
                bool gender = Customization == null ? true : Customization.Gender;
                Main.PlayerSlotsInfo[UUID] = new SlotInfo(LVL, EXP, FractionID, Money, gender, LifeActivity.Hunger.Level, LifeActivity.Thirst.Level);
                int period = Convert.ToInt32((ArrestDate - DateTime.Now).TotalMinutes);
                int arrestTime = period > 0 ? period : 0;
                MySQL.Query(
                    "UPDATE characters SET pos = @prop1, health = @prop2, lvl = @prop3, exp = @prop4, money = @prop5, " +
                    "banknew = @prop6, work = @prop7, arrest = @prop8, wanted = @prop9, adminlvl = @prop10, " +
                    "licenses = @prop11, unwarn = @prop12, unmutedate = @prop13, warns = @prop14, onduty = @prop15, lasthour = @prop16, lastmin = @prop43, demorgan = @prop17, friends = @prop18, " +
                    "mulct = @prop19, arrestiligalTime = @prop20, arrestID = @prop21, timerMiss = @prop22, courttime = @prop23, lastdayplayedhours = @prop24, chips = @prop25, " +
                    "partner = @prop26, hungerlevel = @prop27, thirstlevel = @prop28, imp_job_state = @prop29, numberofratings = @prop30, " +
                    "amountofratings = @prop31, numberofadminratings = @prop32, amountofadminratings = @prop33, queststage = @prop34, arena_points = @prop35, media = @prop36, " +
                    "usedTips = @prop37, mediahelper = @prop38, iconoverhead = @prop39, bonusPoints = @prop40, respectPoints = @prop41, primeLeftFraction = @prop42 WHERE uuid = @prop0",
                    UUID, JsonConvert.SerializeObject(SpawnPos), Health, LVL, EXP, Money, BankNew, WorkID >= 15 ? 0 : WorkID, arrestTime, JsonConvert.SerializeObject(WantedLVL),
                    AdminLVL, JsonConvert.SerializeObject(Licenses, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd" }), MySQL.ConvertTime(Unwarn), MySQL.ConvertTime(UnmuteDate), Warns, OnDuty, LastHour,
                    DemorganTime, JsonConvert.SerializeObject(Friends), Mulct, ArrestiligalTime, ArrestID, MySQL.ConvertTime(TimerMiss), CourtTime, PlayedHoursInLastDay, JsonConvert.SerializeObject(CasinoChips), Partner,
                    LifeActivity.Hunger.Level, LifeActivity.Thirst.Level, JsonConvert.SerializeObject(ImprovableJobStates), NumberOfRatings, AmountOfRatings, NumberOfAdminRatings, AmountOfAdminRatings, (int)QuestStage, ArenaPoints, Media,
                    JsonConvert.SerializeObject(UsedTips), MediaHelper, JsonConvert.SerializeObject(IconOverHead), BonusPoints, RespectPoints, LastUsedPrimeLeave, LastHourMin);
                EventModel.Save(UUID);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Save:\n{e}");//оно выдазит когда ты заходишь в выбор персонажа?
            }
        }


        private int GenerateUUID()
        {            
            return ++_lastUUID;
        }
        
        public static Dictionary<string, string> toChange = new Dictionary<string, string>();      

        //public static void changeName(string oldName)
        //{
        //    try
        //    {
        //        if (!toChange.ContainsKey(oldName)) return;

        //        string newName = toChange[oldName];

        //        //int UUID = Main.PlayerNames.FirstOrDefault(u => u.Value == oldName).Key;
        //        int Uuid = Main.PlayerUUIDs.GetValueOrDefault(oldName);
        //        if (Uuid <= 0)
        //        {
        //            _logger.WriteWarning($"Cant'find UUID of player [{oldName}]");
        //            return;
        //        }

        //        string[] split = newName.Split("_");

        //        Main.PlayerNames[Uuid] = newName;
        //        Main.PlayerUUIDs.Remove(oldName);
        //        Main.PlayerUUIDs.Add(newName, Uuid);

        //        MySQL.Query("UPDATE `characters` SET `firstname` = @prop0, `lastname` = @prop1 WHERE `uuid` = @prop2", split[0], split[1], Uuid);

        //        VehicleManager.changeOwner(Uuid, newName);

        //        _logger.WriteDebug("Nickname has been changed!");
        //        toChange.Remove(oldName);
        //        GameLog.Name(Uuid, oldName, newName);
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.WriteError($"changeName:\n{e}");
        //    }
        //}
        public static bool NameIsCorrect(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            if (!name.Contains('_')) return false;

            string[] split = name.Split("_");
            if (split.Length != 2) return false;
            return NameIsCorrect(split[0], split[1]);
        }

        public static bool NameIsCorrect(string firstName, string lastName)
        {
            if (!regex.IsMatch(firstName)) return false;
            if (!regex.IsMatch(lastName)) return false;

            return true;
        }
        public static ChangeNameResult ChangeName(string currentName, string newName)
        {

            if (Main.PlayerUUIDs.ContainsKey(newName)) return ChangeNameResult.NewNameIsExist;
            if (!Main.PlayerUUIDs.ContainsKey(currentName)) return ChangeNameResult.BadCurrentName;
            if (!NameIsCorrect(newName)) return ChangeNameResult.IncorrectNewName;

            string[] split = newName.Split("_");
            int uuid = Main.PlayerUUIDs[currentName];
            Main.PlayerNames[uuid] = newName;
            Main.PlayerUUIDs.Remove(currentName);
            Main.PlayerUUIDs.Add(newName, uuid);
            MySQL.QuerySync("UPDATE `characters` SET `firstname` = @prop0, `lastname` = @prop1 WHERE `uuid` = @prop2", split[0], split[1], uuid);
            MySQL.QuerySync("UPDATE `characters` SET `firstname` = @prop0, `lastname` = @prop1 WHERE `uuid` = @prop2", split[0], split[1], uuid);

            ExtPlayer target = Trigger.GetPlayerByName(currentName);
            if(target != null) 
            {
                target.Name = newName;
                if (target.Character != null)
                {
                    target.Character.FirstName = split[0];
                    target.Character.LastName = split[1];
                }
            }

            VehicleManager.changeOwner(uuid, newName);
            GameLog.Name(uuid, currentName, newName);
            return ChangeNameResult.Success;
        }

        public string FullName => $"{FirstName}_{LastName}";
        public string GetPartnerName()
        {
            if (Main.PlayerNames.ContainsKey(Partner)) return Main.PlayerNames[Partner];
            if (Customization.Gender) return "Свободен";
            return "Свободна";
        }


        internal void AddExp(ExtPlayer player, bool prime)
        {
            int value = prime ? 1 + DonateService.PrimeAccount.BonusExp : 1;
            EXP += value;
            if (EXP >= 3 + LVL * 3)
            {
                EXP = EXP - (3 + LVL * 3);
                LVL += 1;
                if (LVL == 1) SafeTrigger.ClientEvent(player, "disabledmg", false);
                if (LVL == 5 && !player.Account.PromoReceived && !string.IsNullOrEmpty(player.Account.PromoUsed)) PromoCodesService.GiveReward(player);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы достигли ({LVL.ToString()}) уровня!", 3000);
                Main.OnPlayerLevelUp?.Invoke(player);
                SafeTrigger.SetSharedData(player, "C_LVL", LVL);
            }
            player.SendExpUpdate();
        }

        public void SubscribeToSystem(ExtPlayer player)
        {
            if (Inventory != null) Inventory.Subscribe(player);
            if (Equip != null) Equip.Subscribe(player);
            if (DonateInventory != null) DonateInventory.Subscribe(player);
            player.Name = FirstName + "_" + LastName;
            player.UpdateCoins();
            LifeSystem.LifeActivity.Subscribe(player);
            //player.Account.InitBonus();

            ServerGo.Casino.Business.Casino casino = CasinoManager.FindFirstCasino();
            foreach (int chip in CasinoChips)
            {
                if (casino == null) continue;
                if (chip <= 0) continue;

                casino.AddGambler(player, CasinoChips);
                break;
            }
            SafeTrigger.ClientEvent(player,"setUUID", UUID);

            Player = player;
        }
    }
}
