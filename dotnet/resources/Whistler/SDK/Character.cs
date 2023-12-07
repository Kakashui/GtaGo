using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using Whistler.Core.LifeSystem;
using Whistler.Inventory.Models;
using Whistler.Jobs.ImpovableJobs;
using Whistler.Families;
using Whistler.NewDonateShop.Models;
using Whistler.GUI.Documents.Models;
using Whistler.VehicleSystem;
using System.Data;
using Newtonsoft.Json;
using Whistler.Inventory;
using Whistler.NewDonateShop;
using Whistler.Helpers;
using Whistler.Inventory.Enums;
using ServerGo.Casino.Business;
using Whistler.Customization.Models;
using Whistler.Customization;
using Whistler.MoneySystem;
using Whistler.MoneySystem.Interface;
using Whistler.MoneySystem.Models;
using Whistler.PersonalEvents.Models;
using Whistler.PersonalEvents;
using Whistler.StartQuest;
using Whistler.Core;
using Whistler.Entities;
using Whistler.VehicleSystem.Models;
using Whistler.Core.Models;

namespace Whistler.SDK
{
    public class CharacterData : IMoneyOwner
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(CharacterData));
        public int UUID { get; set; } = -1;
        public Vector3 SpawnPos { get; set; } = new Vector3(0, 0, 0);
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime BirthDate { get; set; } = DateTime.Now;
        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;
        public DateTime VipDate { get; protected set; } = DateTime.UtcNow;

        public int Health { get; set; } = 100;
        public int LVL { get; set; } = 1;
        public int EXP { get; set; } = 0;
        public long Money { get; set; } = 3500;
        public int BankNew { get; set; } = 0;
        public int WorkID { get; set; } = 0;
        public int FractionID { get; set; } = 0;
        public int FractionLVL { get; set; } = 0;
        public int ArrestiligalTime { get; set; } = 0;
        public int CourtTime { get; set; } = 0;
        public int ArrestID { get; set; } = 0;
        public DateTime TimerMiss { get; set; } = DateTime.Now;
        public int FamilyID { get; set; } = 0;
        public int FamilyLVL { get; set; } = 0;

        public DateTime ArrestDate { get; set; } = DateTime.Now;
        public int DemorganTime { get; set; } = 0;
        public WantedLevel WantedLVL { get; set; } = null;
        public int AdminLVL { get; set; } = 0;
        public List<License> Licenses { get; set; } = new List<License>();
        public DateTime Unwarn { get; set; } = DateTime.Now;
        //public int Unmute { get; set; } = 0;
        public DateTime UnmuteDate { get; set; } = DateTime.Now;
        public int Warns { get; set; } = 0;
        public bool OnDuty { get; set; } = false;
        public int LastHour { get; set; } = 0;
        public int LastHourMin { get; set; } = 0;
        public int[] CasinoChips = new int[5];
        public List<PlayerFriend> Friends { get; set; } = new List<PlayerFriend>();
        public int Mulct { get; set; } = 0;
        public bool VoiceMuted = false;
        public int PlayedHoursInLastDay { get; set; } = 0;
        public bool ReportNotification { get; set; } = true;
        public int Partner { get; set; } = -1;
        public LifeActivity LifeActivity { get; set; }
        public Equip Equip { get; set; }
        public InventoryModel Inventory { get; set; }
        public DonateInventoryModel DonateInventory { get; set; }
        public StartQuestNames QuestStage { get; set; }
        
        public int ArenaPoints { get; set; }
        
        public List<string> UsedTips { get; set; } = new List<string>();
       
        public List<ImprovableJobState> ImprovableJobStates = new List<ImprovableJobState>();
        public IReadOnlyDictionary<ImprovableJobType, ImprovableJobState> ImprovableJobs =>
            ImprovableJobStates.ToDictionary(key => key.JobType);

        public int NumberOfRatings { get; set; } = 0;
        public int AmountOfRatings { get; set; } = 0;
        public int NumberOfAdminRatings { get; set; } = 0;
        public int AmountOfAdminRatings { get; set; } = 0;

        public int Media { get; set; } = 0;
        public int MediaHelper { get; set; } = 0;
        public bool MediaMuted { get; set; } = false;
        public PlayerIconOverHead IconOverHead { get; set; }
        public int LastVote { get; set; } = -1;
        public string ImageLink { get; set; }
        internal EventModel EventModel { get; set; }
        public int BonusPoints { get; set; }
        public int RespectPoints { get; set; }

        public string Promocode { get; set; }
        public int PromocodeLevel { get; set; }
        public int PromocodeActivatedCount { get; set; }
        public int PromocodeUsedCount { get; set; }
        public DateTime? LastUsedPrimeLeave { get; set; }

        // temperory data
        public Player Player { get; set; } = null;
        public Equip TempEquip { get; set; } = null;
        public InventoryModel TempInventory { get; set; } = null;

        public int InsideHouseID = -1;
        public int InsideGarageID = -1;
        public Vector3 ExteriorPos = null;
        public int InsideHotelID = -1;
        public int BusinessInsideId = -1;
        public bool IsAlive = false;
        public bool IsSpawned = false;
        public int ParkingSpawnCar = -1;
        public int OpenedReport = -1;
        public bool Cuffed = false;
        public bool CuffedCop = false;
        public bool CuffedGang = false;
        public ExtPlayer Follower = null;
        public ExtPlayer Following = null;

        public int InSaveZone = -1;
        public BattleLocation WarZone = BattleLocation.None;

        public bool IsAFK = false;
        public int AfkMinuteInHours = 0;
        public DateTime LastTriggerAFK = DateTime.Now;
        public int HouseTarget = -1;

        public Dictionary<VehicleAccess, ExtVehicle> TempVehicles = new Dictionary<VehicleAccess, ExtVehicle>();

        public int CustomizationId;
        public CustomizationModel Customization 
        { 
            get
            {
                return CustomizationService.GetById(CustomizationId);
            }
        }
        internal CheckingAccount BankModel
        {
            get
            {
                return BankManager.GetAccount(BankNew);
            }
        }

        public long IMoneyBalance => Money;
        public TypeMoneyAcc ITypeMoneyAcc { get { return TypeMoneyAcc.Player; } }
        public int IOwnerID => UUID;

        protected static int _lastUUID = 0;
        public CharacterData(string firstName, string lastName, int accountId, int customizationId, ClothesDTO clothes)
        {

            UUID = GenerateUUID();

            CustomizationId = customizationId;

            FirstName = firstName;
            LastName = lastName;

            BankNew = BankManager.CreateAccount(TypeBankAccount.Player, 0, UUID).ID;

            Licenses = new List<License>();

            SpawnPos = new Vector3(-3029.699, 72.52473, 12.902239);

            LifeActivity = new LifeActivity();

            Main.UUIDs.Add(UUID);
            Main.PlayerSlotsInfo.Add(UUID, new SlotInfo(LVL, EXP, FractionID, Money, Customization.Gender, LifeActivity.Hunger.Level, LifeActivity.Thirst.Level));
            Main.PlayerUUIDs.Add($"{firstName}_{lastName}", UUID);
            Main.PlayerNames.Add(UUID, $"{firstName}_{lastName}");

            QuestStage = StartQuestNames.Stage1IncomingToState;
            Equip = new Equip(clothes, Customization.Gender);
            Inventory = new InventoryModel(InventoryService.BasePlayerWeight, InventoryService.BasePlayerSize, InventoryTypes.Personal);

            Promocode = PromoCodesService.GenerateReferalCode();
            PromoCodesService.AddReferalCode(Promocode, UUID, 0, 0, 0);
            PromocodeLevel = 0;
            PromocodeActivatedCount = 0;
            PromocodeUsedCount = 0;


            Console.WriteLine("123123131231");//че за франкенштейн а не сборка
            MySQL.Query(@"INSERT INTO `characters`(`uuid`,`firstname`,`lastname`,`gender`,`health`,`lvl`,`exp`,`money`,`banknew`,`work`,`fraction`,`fractionlvl`,`arrest`,`demorgan`,`wanted`,
                `adminlvl`,`licenses`,`unwarn`,`unmutedate`,`warns`,`onduty`,`lasthour`,`lastmin`,`pos`,`createdate`, `friends`, `arrestiligalTime`, `arrestID`,
                `timerMiss`, `courttime`, `mulct`,`partner`,`equipId`,`inventoryId`, `queststage`, `owner`, `customizationid`, `promocode`, `vipdate`)
                VALUES(@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6, @prop7, @prop8, @prop9, @prop10, @prop11, @prop12, @prop13, @prop14, @prop15, @prop16, @prop17,
                @prop18, @prop19, @prop20, @prop21, @prop22, @prop23, @prop24, @prop25, @prop26, @prop27, @prop28, @prop29, @prop30, @prop31, @prop32, @prop33, @prop34, @prop35, @prop36, @prop37, @prop38)",

                UUID, FirstName, LastName, Customization.Gender, Health, LVL, EXP, Money, BankNew, WorkID, FractionID, FractionLVL, 0, DemorganTime,
                JsonConvert.SerializeObject(WantedLVL), AdminLVL, JsonConvert.SerializeObject(Licenses, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd" }), MySQL.ConvertTime(Unwarn),
                MySQL.ConvertTime(UnmuteDate), Warns, OnDuty, LastHour, LastHourMin, JsonConvert.SerializeObject(SpawnPos), MySQL.ConvertTime(CreateDate), JsonConvert.SerializeObject(Friends),
                ArrestiligalTime, ArrestID, MySQL.ConvertTime(TimerMiss), CourtTime, Mulct, Partner, Equip.Id, Inventory.Id, (int)QuestStage, accountId, CustomizationId, Promocode, MySQL.ConvertTime(VipDate));
            EventModel = new EventModel();
            CreateNewDonateInventroy();
        }
        public CharacterData(DataRow row)
        {

            UUID = Convert.ToInt32(row["uuid"]);
            FirstName = Convert.ToString(row["firstname"]);
            LastName = Convert.ToString(row["lastname"]);
            CustomizationId = Convert.ToInt32(row["customizationid"]);
            Health = Convert.ToInt32(row["health"]);
            LVL = Convert.ToInt32(row["lvl"]);
            EXP = Convert.ToInt32(row["exp"]);
            Money = Convert.ToInt64(row["money"]);
            BankNew = Convert.ToInt32(row["banknew"]);
            if (BankModel == null)
            {
                BankNew = BankManager.CreateAccount(TypeBankAccount.Player, 0, UUID).ID;
                MySQL.Query("UPDATE `characters` SET `banknew` = @prop0 WHERE `uuid` = @prop1", BankNew, UUID);
            }
            WorkID = Convert.ToInt32(row["work"]);
            FractionID = Convert.ToInt32(row["fraction"]);
            FractionLVL = Convert.ToInt32(row["fractionlvl"]);

            ArrestiligalTime = Convert.ToInt32(row["arrestiligalTime"]);
            CourtTime = Convert.ToInt32(row["courttime"]);
            ArrestID = Convert.ToInt32(row["arrestID"]);
            TimerMiss = ((DateTime)row["timerMiss"]);
            ArenaPoints = Convert.ToInt32(row["arena_points"]);

            UsedTips = JsonConvert.DeserializeObject<List<string>>(row["usedTips"].ToString()) ?? new List<string>();
            CasinoChips = row["chips"] != DBNull.Value ? JsonConvert.DeserializeObject<int[]>(row["chips"].ToString()) : new int[5];

            FamilyID = Convert.ToInt32(row["family"]);
            FamilyLVL = Convert.ToInt32(row["familylvl"]);

            ImageLink = row["imagelink"].ToString();


            ArrestDate = DateTime.Now.AddMinutes(Convert.ToInt32(row["arrest"]));
            DemorganTime = Convert.ToInt32(row["demorgan"]);
            WantedLVL = JsonConvert.DeserializeObject<WantedLevel>(row["wanted"].ToString());
            AdminLVL = Convert.ToInt32(row["adminlvl"]);
            Media = Convert.ToInt32(row["media"]);
            MediaHelper = Convert.ToInt32(row["mediahelper"]);
            try
            {
                Licenses = JsonConvert.DeserializeObject<List<License>>(row["licenses"].ToString());
                Licenses.RemoveAll(item => !item.IsActive);
            }
            catch
            {
                Licenses = new List<License>();
                var lic = JsonConvert.DeserializeObject<List<bool>>(row["licenses"].ToString());
                for (int i = 0; i < lic.Count; i++)
                {
                    if (lic[i])
                        Licenses.Add(new License((GUI.Documents.Enums.LicenseName)i));
                }
                MySQL.Query("UPDATE `characters` SET `licenses`=@prop0 WHERE `uuid`=@prop1", JsonConvert.SerializeObject(Licenses), UUID);
            }

            Unwarn = ((DateTime)row["unwarn"]);
            UnmuteDate = ((DateTime)row["unmutedate"]);
            Warns = Convert.ToInt32(row["warns"]);
            OnDuty = Convert.ToBoolean(row["onduty"]);
            LastHour = Convert.ToInt32(row["lasthour"]);
            LastHourMin = Convert.ToInt32(row["lastmin"]);
            Friends = JsonConvert.DeserializeObject<List<PlayerFriend>>(row["friends"].ToString()) ?? new List<PlayerFriend>();
            Mulct = Convert.ToInt32(row["mulct"]);
            Partner = Convert.ToInt32(row["partner"]);

            CreateDate = ((DateTime)row["createdate"]);
            ImprovableJobStates = row["imp_job_state"] == DBNull.Value ? new List<ImprovableJobState>()
                : JsonConvert.DeserializeObject<List<ImprovableJobState>>(row["imp_job_state"].ToString());

            PlayedHoursInLastDay = Convert.ToInt32(row["lastdayplayedhours"]);

            NumberOfRatings = Convert.ToInt32(row["numberofratings"]);
            AmountOfRatings = Convert.ToInt32(row["amountofratings"]);
            NumberOfAdminRatings = Convert.ToInt32(row["numberofadminratings"]);
            AmountOfAdminRatings = Convert.ToInt32(row["amountofadminratings"]);
            QuestStage = (StartQuestNames)Convert.ToInt32(row["queststage"]);
            LastVote = Convert.ToInt32(row["lastvote"]);

            BonusPoints = Convert.ToInt32(row["bonusPoints"]);
            RespectPoints = Convert.ToInt32(row["respectPoints"]);

            EventModel = EventManager.EventModelCreateOrLoad(UUID);

            LifeActivity = new LifeActivity(row);
            IconOverHead = (row["iconoverhead"] == DBNull.Value) ? null : JsonConvert.DeserializeObject<PlayerIconOverHead>(row["iconoverhead"].ToString());
            Equip = EquipService.GetById(Convert.ToInt32(row["equipId"]));
            Inventory = InventoryService.GetById(Convert.ToInt32(row["inventoryId"]));
            if (Equip == null)
            {
                Equip = new Equip();
                MySQL.Query("UPDATE `characters` SET `equipId`=@prop0 WHERE `uuid`=@prop1", Equip.Id, UUID);
            }
            if (Inventory == null)
            {
                Inventory = new InventoryModel(InventoryService.BasePlayerWeight, InventoryService.BasePlayerSize, InventoryTypes.Personal);
                MySQL.Query("UPDATE `characters` SET `inventoryId`=@prop0 WHERE `uuid`=@prop1", Inventory.Id, UUID);
            }
            Inventory.DeleteEmptyArmor();
            Equip.DeleteEmptyArmor();

            if (row.IsNull("donateInventoryId"))
                CreateNewDonateInventroy();
            else
            {
                DonateInventory = DonateService.GetInventoryById(Convert.ToInt32(row["donateInventoryId"]));
                if (DonateInventory == null)
                    CreateNewDonateInventroy();
            }
            if (row["pos"].ToString().Contains("NaN"))
            {
                _logger.WriteDebug("Detected wrong coordinates!");
                if (LVL <= 1)
                    SpawnPos = new Vector3(-3029.699, 72.52473, 12.902239); // On newbe spawn  -1037.248 -2731.725 13.75665
                else
                    SpawnPos = new Vector3(-388.5015, -190.0172, 36.19771); // near goverment
            }
            else SpawnPos = JsonConvert.DeserializeObject<Vector3>(row["pos"].ToString());

            Promocode = DBNull.Value == row["promocode"] ? null : row["promocode"].ToString();
            if (string.IsNullOrEmpty(Promocode))
            {
                Promocode = PromoCodesService.GenerateReferalCode();
                PromoCodesService.AddReferalCode(Promocode, UUID, 0, 0, 0);
                MySQL.QuerySync("UPDATE characters SET promocode = @prop0 WHERE uuid = @prop1", Promocode, UUID);
            }
            PromocodeLevel = Convert.ToInt32(row["promocodeLevel"]);
            PromocodeActivatedCount = Convert.ToInt32(row["promocodeActivated"]);
            PromocodeUsedCount = Convert.ToInt32(row["promocodeUsed"]);
            object primeLeftFraction = row["primeLeftFraction"];
            LastUsedPrimeLeave = primeLeftFraction == DBNull.Value ? null : (DateTime?)primeLeftFraction;
            VipDate = (DateTime)row["vipdate"];
        }


        private void CreateNewDonateInventroy()
        {
            DonateInventory = DonateService.CrateInventory();
            MySQL.Query("UPDATE `characters` SET `donateInventoryId`=@prop0 WHERE `uuid`=@prop1", DonateInventory.Id, UUID);
        }
        private int GenerateUUID()
        {
            return ++_lastUUID;
        }
        public void ClearTempFields()
        {
            Player = null;
            InsideHouseID = -1;
            InsideGarageID = -1;
            ExteriorPos = null;
            BusinessInsideId = -1;
            IsAlive = false;
            IsSpawned = false;
            ParkingSpawnCar = -1;
            OpenedReport = -1;
            Cuffed = false;
            CuffedCop = false;
            CuffedGang = false;
            Follower = null;
            Following = null;
            InSaveZone = -1;
            WarZone = BattleLocation.None;
            IsAFK = false;
            AfkMinuteInHours = 0;
            LastTriggerAFK = DateTime.Now;
            TempVehicles = new Dictionary<VehicleAccess, ExtVehicle>();
        }
        public void SetImage(string image)
        {
            ImageLink = image;
            MySQL.Query("UPDATE `characters` SET `imagelink`=@prop0 WHERE `uuid`=@prop1", ImageLink, UUID);
        }

        public void UpdateFriends(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player, "setFriendList", JsonConvert.SerializeObject(Friends));
        }

        public void UpdateReferal(ExtPlayer player, bool all)
        {
            if (player == null) return;

            PromoCode promocodeData = PromoCodesService.GetPromocodeDataByOwner(player);
            if (all)
            {
                player.TriggerCefEvent("optionsMenu/updateReferalsData", JsonConvert.SerializeObject(new { key = "total", value = promocodeData == null ? PromocodeActivatedCount : promocodeData.ActivatedCount }));
                player.TriggerCefEvent("optionsMenu/updateReferalsData", JsonConvert.SerializeObject(new { key = "code", value = promocodeData == null ? Promocode : promocodeData.Name }));
            }
            else
            {
                player.TriggerCefEvent("optionsMenu/updateReferalsData", JsonConvert.SerializeObject(new { key = "total", value = promocodeData == null ? PromocodeActivatedCount : promocodeData.ActivatedCount }));
            }
        }

        public bool AddFriend(string name)
        {
            if (Friends.FirstOrDefault(item => item.Nickname == name) == null)
            {
                Friends.Add(new PlayerFriend(name));
                MySQL.Query("UPDATE characters SET `friends` = @prop0 WHERE uuid = @prop1", JsonConvert.SerializeObject(Friends), UUID);
                return true;
            }
            return false;
        }

        public bool MoneyAdd(int amount)
        {
            if (amount <= 0) return false;
            return Change(amount);
        }

        public bool MoneySub(int amount, bool limit = false)
        {
            if (amount <= 0) return false;
            return Change(-amount);
        }
        private bool Change(int amount)
        {
            if (Money + amount < 0) return false;
            Money += amount;
            SafeTrigger.ClientEvent((ExtPlayer)Player, "UpdateMoney", Money.ToString(), Convert.ToString(amount));
            return true;
        }
        public void SetPartner(ExtPlayer player, ExtPlayer target)
        {
            var targetUuid = target.Character.UUID;
            Partner = targetUuid;
            MySQL.Query("UPDATE characters SET partner = @prop1 WHERE uuid = @prop0", targetUuid, UUID);
            SafeTrigger.ClientEvent(player, "marriage:complete", target.Name.Replace('_', ' '));
        }

        // public bool ChangeBonusPoint(int value)
        // {
        //     if (BonusPoints + value < 0)
        //         return false;
        //     BonusPoints += value;
        //     return true;
        // }

        public static void AddOfflineBonusPoint(int uuid, int value)
        {
            if (value <= 0)
                return;
            MySQL.Query("UPDATE characters SET bonusPoints = bonusPoints + @prop0 WHERE uuid = @prop1", value, uuid);
            // Main.GetCharacterByUUID(uuid)?.ChangeBonusPoint(value);
        }

        public bool ChangeRespectPoint(int value)
        {
            if (RespectPoints + value < 0)
                return false;
            RespectPoints += value;
            return true;
        }

        public static void AddOfflineRespectPoint(int uuid, int value)
        {
            if (value <= 0) return;
            MySQL.Query("UPDATE characters SET respectPoints = respectPoints + @prop0 WHERE uuid = @prop1", value, uuid);

            ExtPlayer target = Trigger.GetPlayerByUuid(uuid);
            if (target == null) return;

            target.Character.ChangeRespectPoint(value);
        }

    }


    public class PlayerFriend
    {
        public string Nickname;
        public PlayerFriend(string nickname)
        {
            Nickname = nickname;
        }
    }

    public class WantedLevel
    {
        public int Level { get; set; }
        public string WhoGive { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }

        public WantedLevel(int level, string whoGive, DateTime date, string reason)
        {
            Level = level;
            WhoGive = whoGive;
            Date = date;
            Reason = reason;
        }
    }

    public class PlayerIconOverHead
    {
        public string Dictionary { get; set; }
        public string Name { get; set; }
        public int Color { get; set; }
        public PlayerIconOverHead(string dictionary, string name, int color)
        {
            Dictionary = dictionary;
            Name = name;
            Color = color;
        }
        public PlayerIconOverHead()
        {
            Dictionary = "none";
            Name = "none";
            Color = -1;
        }
        public void UpdateSharedData(ExtPlayer player)
        {
            if (Dictionary != "none")
            {
                SafeTrigger.SetSharedData(player, "playerIcon:dict", Dictionary);
                SafeTrigger.SetSharedData(player, "playerIcon:name", Name);
                SafeTrigger.SetSharedData(player, "playerIcon:color", Color);
            }
            else
            {
                SafeTrigger.ResetSharedData(player, "playerIcon:dict");
                SafeTrigger.ResetSharedData(player, "playerIcon:name");
                SafeTrigger.ResetSharedData(player, "playerIcon:color");
            }
        }
    }
}
