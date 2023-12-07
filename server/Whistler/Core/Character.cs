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
        public static Action<Player> OnPlayerSpawned;
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

        private void CreateNewDonateInventroy(PlayerGo player)
        {
            DonateInventory = DonateService.CrateInventory();
            MySQL.Query("UPDATE `characters` SET `donateInventoryId`=@prop0 WHERE `uuid`=@prop1", DonateInventory.Id, UUID);
            DonateInventory.Subscribe(player);
        }

        public void LoadSpawnPoints(PlayerGo player)
        {
            var points = new List<string>();
            var familyHose = HouseManager.GetHouse(FamilyID, OwnerType.Family);
            var familyName = familyHose == null ? "None" : FamilyManager.GetFamilyName(FamilyID);
            points.Add(familyName == "None" ? "" : familyName);
            points.Add(JsonConvert.SerializeObject(SpawnPos));
            var house = HouseManager.GetHouse(player);
            points.Add(house == null ? "" : house.ID.ToString());
            var frac = FractionID < 1 ? "" : Manager.getName(FractionID);
            points.Add(frac);
            player.TriggerEvent("auth:spawn:select", points);
        }

        //arrest timer
        public void SetArrestTimer(PlayerGo player)
        {
            ResetArrestTimer(player);
            if (ArrestDate <= DateTime.UtcNow) return;
            var period = Convert.ToInt32((ArrestDate - DateTime.UtcNow).TotalMilliseconds);
            player.TriggerEvent("hud:arrest:timer:update", ArrestDate.ToString("yyyy-MM-dd HH:mm:ss"), "test reason");
            player.SetData("ARREST_TIMER", Timers.StartOnce(period, () => ReleaseArrest(player)));
        }

        public void ResetArrestTimer(PlayerGo player)
        {
            if (player.HasData("ARREST_TIMER"))
            {
                Timers.Stop(player.GetData<string>("ARREST_TIMER")); // still not fixed
                player.ResetData("ARREST_TIMER");
            }
        }
        private void ReleaseArrest(PlayerGo player)
        {
            if (!player.IsLogged()) return;
            if (ArrestDate > DateTime.UtcNow.AddMinutes(3))
                SetArrestTimer(player);
            else
                PoliceArrests.ReleasePlayer(player, null, 0);
        }

        public void UpdateData(PlayerGo player)
        {            
            Health = player.Health;
            if (IsSpawned && !IsAlive)
            {
                SpawnPos = Ems.GetRandomSpawnPointAfterDeath();
                Health = 20;
            }else if(BusinessInsideId > -1)
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
                
                var vehicles = VehicleManager.getAllHolderVehicles(UUID, OwnerType.Personal);
                foreach (var veh in vehicles)
                     VehicleManager.Vehicles[veh].Save();

                var gender = Customization == null ? true : Customization.Gender;

                Main.PlayerSlotsInfo[UUID] = new SlotInfo(LVL, EXP, FractionID, Money, gender, LifeActivity.Hunger.Level, LifeActivity.Thirst.Level, LifeActivity.Rest.Level, LifeActivity.Thirst.Level);
                var period = Convert.ToInt32((ArrestDate - DateTime.UtcNow).TotalMinutes);
                var arrestTime = period > 0 ? period : 0;
                MySQL.Query(
                    "UPDATE characters SET pos = @prop1, health = @prop2, lvl = @prop3, exp = @prop4, money = @prop5, " +
                    "banknew = @prop6, work = @prop7, arrest = @prop8, wanted = @prop9, adminlvl = @prop10, " +
                    "licenses = @prop11, unwarn = @prop12, unmutedate = @prop13, warns = @prop14, onduty = @prop15, lasthour = @prop16, demorgan = @prop17, friends = @prop18, " +
                    "mulct = @prop19, arrestiligalTime = @prop20, arrestID = @prop21, timerMiss = @prop22, courttime = @prop23, lastdayplayedhours = @prop24, chips = @prop25, " +
                    "partner = @prop26, hungerlevel = @prop27, thirstlevel = @prop28, restlevel = @prop29, joylevel = @prop30, imp_job_state = @prop31, numberofratings = @prop32, " +
                    "amountofratings = @prop33, numberofadminratings = @prop34, amountofadminratings = @prop35, queststage = @prop36, arena_points = @prop37, media = @prop38, " +
                    "usedTips = @prop39, mediahelper = @prop40, iconoverhead = @prop41, bonusPoints = @prop42, respectPoints = @prop43 WHERE uuid = @prop0",
                    UUID, JsonConvert.SerializeObject(SpawnPos), Health, LVL, EXP, Money, BankNew, WorkID >= 15 ? 0 : WorkID, arrestTime, JsonConvert.SerializeObject(WantedLVL),
                    AdminLVL, JsonConvert.SerializeObject(Licenses, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd" }), MySQL.ConvertTime(Unwarn), MySQL.ConvertTime(UnmuteDate), Warns, OnDuty, LastHourMin,
                    DemorganTime, JsonConvert.SerializeObject(Friends), Mulct, ArrestiligalTime, ArrestID, MySQL.ConvertTime(TimerMiss), CourtTime, PlayedHoursInLastDay, JsonConvert.SerializeObject(CasinoChips), Partner,
                    LifeActivity.Hunger.Level, LifeActivity.Thirst.Level, LifeActivity.Rest.Level, LifeActivity.Joy.Level, JsonConvert.SerializeObject(ImprovableJobStates), NumberOfRatings, AmountOfRatings, NumberOfAdminRatings, AmountOfAdminRatings, (int)QuestStage, ArenaPoints, Media,
                    JsonConvert.SerializeObject(UsedTips), MediaHelper, JsonConvert.SerializeObject(IconOverHead), BonusPoints, RespectPoints);
                EventModel.Save(UUID);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Save:\n{e}");
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
            List<string> split = name.Split("_").ToList();
            if (split.Count != 2)
                return false;
            return NameIsCorrect(split[0], split[1]);
        }
        public static bool NameIsCorrect(string firstName, string lastName)
        {
            var regex = new Regex(@"^[A-Za-z]{3,25}$");
            if (!regex.IsMatch(firstName))
                return false;
            if (!regex.IsMatch(lastName))
                return false;
            return true;
        }
        public static ChangeNameResult ChangeName(string currentName, string newName)
        {

            if (Main.PlayerUUIDs.ContainsKey(newName))
            {
                return ChangeNameResult.NewNameIsExist;
            }
            if (!Main.PlayerUUIDs.ContainsKey(currentName))
            {
                return ChangeNameResult.BadCurrentName;
            }

            List<string> split = newName.Split("_").ToList();
            if (!NameIsCorrect(newName))
                return ChangeNameResult.IncorrectNewName;

            int uuid = Main.PlayerUUIDs[currentName];
            Main.PlayerNames[uuid] = newName;
            Main.PlayerUUIDs.Remove(currentName);
            Main.PlayerUUIDs.Add(newName, uuid);
            MySQL.QuerySync("UPDATE `characters` SET `firstname` = @prop0, `lastname` = @prop1 WHERE `uuid` = @prop2", split[0], split[1], uuid);

            var character = Main.GetCharacterByUUID(uuid);
            if(character != null)
            {
                character.FirstName = split[0];
                character.LastName = split[1];
            }         

            Player target = NAPI.Player.GetPlayerFromName(currentName);
            if(target != null)
                target.Name = newName;

            VehicleManager.changeOwner(uuid, newName);

            GameLog.Name(uuid, currentName, newName);
            return ChangeNameResult.Success;
        }

        public string FullName => $"{FirstName}_{LastName}";
        public string GetPartnerName()
        {
            if (Main.PlayerNames.ContainsKey(Partner))
                return Main.PlayerNames[Partner];
            if (Customization.Gender)
                return "mmain:stats:info:freem";
            else
                return "mmain:stats:info:freef";
        }


        internal void AddExp(PlayerGo player, bool prime)
        {
            var value = prime ? 1 + DonateService.PrimeAccount.BonusExp : 1;
            EXP += value;
            if (EXP >= 3 + LVL * 3)
            {
                EXP = EXP - (3 + LVL * 3);
                LVL += 1;
                if (LVL == 1)
                {
                   Trigger.ClientEvent(player, "disabledmg", false);
                }
                Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Main_8".Translate(LVL.ToString()), 3000);
                Main.OnPlayerLevelUp?.Invoke(player);
            }
            player.SendExpUpdate();
        }

        public void SubscribeToSystem(PlayerGo player)
        {
            Inventory.Subscribe(player);
            Equip.Subscribe(player);
            DonateInventory.Subscribe(player);
            player.Name = FirstName + "_" + LastName;
            player.UpdateCoins();
            LifeActivity.Subscribe(player);
            player.GetAccount().InitBonus();

            foreach (var chip in CasinoChips)
            {
                if (chip > 0)
                {
                    CasinoManager.FindFirstCasino()?.AddGambler(player, CasinoChips);
                    break;
                }
            }
            player.TriggerEvent("setUUID", UUID);

            Player = player;
        }
    }
}
