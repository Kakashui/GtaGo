using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using ServerGo.Casino.Business;
using Whistler.Businesses;
using Whistler.Common;
using Whistler.Core;
using Whistler.Core.Character;
using Whistler.Core.CustomSync;
using Whistler.Core.CustomSync.Attachments;
using Whistler.Core.LifeSystem;
using Whistler.Core.Models;
using Whistler.Core.nAccount;
using Whistler.Core.OldPets;
using Whistler.Core.ReportSystem;
using Whistler.Customization;
using Whistler.DoorsSystem;
using Whistler.Families;
using Whistler.Families.FamilyMP;
using Whistler.Families.WarForCompany;
using Whistler.Fractions;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.Houses;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.MoneySystem;
using Whistler.MP.OrgBattle;
using Whistler.NewDonateShop;
using Whistler.ParkingSystem;
using Whistler.PersonalEvents;
using Whistler.PersonalEvents.Contracts.Models;
using Whistler.Phone;
using Whistler.Phone.Forbes;
using Whistler.Phone.Messenger.Accounts;
using Whistler.ReferralSystem;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;

namespace Whistler.Entities
{
    public class ExtPlayer : Player
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ExtPlayer));
        #region Temporary fields
        public int DATA_INTERACT_ID = -1;
        public string MUTE_TIMER = null;
        public bool IsMuted => MUTE_TIMER != null || (Character?.UnmuteDate ?? DateTime.Now) > DateTime.Now;
        public object ContainerInHand = null;
        public DateTime LastSavePlayingTime = DateTime.MaxValue;
        public int StartQuestTempParam = 0;
        public ItemNames MetallPlantOre;
        public int OreVeinID = -1;
        #endregion

        public PSession Session { get; set; } = null;
        public Account Account { get; set; }
        public Character Character { get; set; }
        public ReferralModel Referrals { get; set; }
        public bool Logged() => Character != null;
        public ExtPlayer(NetHandle handle) : base(handle)
        {
            Account = null;
            Character = null;
        }
        public void LoadAccount(Account account)
        {
            Account = account;
        }
        public LoginEvent LoadAccount(DataRow row)
        {
            ulong socialClubId = (ulong)row["socialclubid"];
            string login = row["login"].ToString();
            if (socialClubId == 0)
            {
                if (row.IsNull("socialclub")) return LoginEvent.SclubError;

                string socialClub = row["socialclub"].ToString();
                if (socialClub != this.SocialClubName) return LoginEvent.SclubError;

                ExtPlayer exitsExtPlayer = Main.AllPlayers.FirstOrDefault(pl => pl.SocialClubName == socialClub && pl != this);
                if (exitsExtPlayer == null)
                {
                    SaveSocialClubID(login, this.SocialClubId);
                    LoadAccountData(row);
                    return LoginEvent.Authorized;
                }
                else
                {
                    KickIfPlayerExist(login, exitsExtPlayer);
                    return LoginEvent.Already;
                }
            }
            else
            {
                if (socialClubId != this.SocialClubId) return LoginEvent.SclubError;

                ExtPlayer exitsExtPlayer = Main.AllPlayers.FirstOrDefault(pl => pl.SocialClubId == socialClubId && pl != this);
                if (exitsExtPlayer == null)
                {
                    LoadAccountData(row);
                    return LoginEvent.Authorized;
                }
                else
                {
                    KickIfPlayerExist(login, exitsExtPlayer);
                    return LoginEvent.Already;
                }
            }
        }

        private void LoadAccountData(DataRow row)
        {
            Account = new Account(row, Session.HWID, this.Address);
        }
        private void KickIfPlayerExist(string login, ExtPlayer exist)
        {
            _logger.WriteWarning($"loggiden Current login:{login} social:{this?.SocialClubId} hwid:{this?.Serial}");
            _logger.WriteWarning($"loggiden Exists login:{exist?.Account?.Login} social:{exist?.SocialClubId} hwid:{exist?.Serial}");
            SafeTrigger.ClientEvent(exist, "kick", "Вы были исключены потому что кто то пытался зайти в игру с вашими учетными данными");
            SafeTrigger.ClientEvent(this, "kick", "Вы были исключены потому что аккаунт с такими учетными  данными уже авторизован");
        }
        private void SaveSocialClubID(string login, ulong scID)
        {
            MySQL.QuerySync("UPDATE accounts SET `socialclubid` = @prop0 WHERE `login` = @prop1", scID, login);
        }
        public bool LoadCharacterData(int uuid)//кароче я заебался, давай зхавтрна
        {
            Console.WriteLine($"LoadCharacterData: {uuid}");
            if (!Account.Characters.Contains(uuid)) return false;

            DataTable result = MySQL.QueryRead("SELECT * FROM `characters` WHERE uuid = @prop0", uuid);
            if (result == null || result.Rows.Count == 0) return false;
            Console.WriteLine($"LoadCharacterData: {JsonConvert.SerializeObject(result.Rows[0])}");
            Character = new Character(result.Rows[0]);
            Character.SubscribeToSystem(this);
            return true;
        }

        public void CreateCharacter(Character character)
        {
            Character = character;
            Character.SubscribeToSystem(this);
        }

        public string isFriend(ExtPlayer target)
        {
            var friend = Character.Friends.Find(f => f.Nickname == target.Character.FullName);
            if(friend == null){
                return $"Незнакомец #{target.Character.UUID}";
            }else{
                return target.Character.FullName.Replace('_', ' ');
            }
        }

        public void ActionInvoke(Action<ExtPlayer> action)
        {
            if (Logged())
                action?.Invoke(this);
        }

        public bool CheckPredicate(Func<ExtPlayer, bool> func)
        {
            if (Logged())
                return func.Invoke(this);
            return false;
        }

        private void InitializeSession()
        {
            Session.FisherData = new Fishing.Models.FisherData(this);
        }

        public void Spawn(int index)
        {
            InitializeSession();
            this.TriggerCefEvent("optionsMenu/setCanUsePromo", string.IsNullOrEmpty(Account.PromoUsed));
            PromoCodesService.Subscribe(this);

            NewDonateShop.Configs.DonateConfig cfg = Main.ServerConfig.DonateConfig;
            if (cfg != null)
            {
                SafeTrigger.ClientEvent(this, "dshop:cources:update",
                new List<int> { cfg.CoinToVirtual, cfg.RubToCoin },
                cfg.Currency,
                JsonConvert.SerializeObject(cfg.CoinKits));
            }

            if (Character.Customization == null)
            {
                if (!CustomizationService.ConvertOldCustomization(this))
                    CustomizationService.SendToCreator(this, -1);
            }
            else
            {
                Character.Customization.Apply(this);
            }
            SafeTrigger.SetSharedData(this, "IS_MASK", false);
            SafeTrigger.ClientEvent(this, "setadminlvl", Character.AdminLVL);
            SafeTrigger.ClientEvent(this, "UpdateMoney", Character.Money.ToString());
            SafeTrigger.ClientEvent(this, "UpdateBank", Character.BankModel.Balance.ToString());
            SafeTrigger.ClientEvent(this, "initPhone");

            FamilyManager.PlayerLoadFamily(this);
            Manager.LoadFraction(this);
            Jobs.WorkManager.load(this);

            this.Health = (Character.Health > 5) ? Character.Health : 5;
            SafeTrigger.SetSharedData(this, "REMOTE_ID", this.Value);
            SafeTrigger.SetSharedData(this, "C_ID", Character.UUID);
            SafeTrigger.SetSharedData(this, "C_LVL", Character.LVL);
            SafeTrigger.SetSharedData(this, "Ping", this.Ping);
            SafeTrigger.SetSharedData(this, "IS_MEDIA", Character.Media > 0);
            SafeTrigger.SetSharedData(this, "IS_MEDIAHELPER", Character.MediaHelper > 0);
            Character.IconOverHead?.UpdateSharedData(this);

            Voice.Voice.PlayerJoin(this);

            SafeTrigger.SetSharedData(this, "voipmode", -1);

            if (Character.AdminLVL > 0)
                GangsCapture.LoadBlips(this);

            if (Character.WantedLVL != null)
                SafeTrigger.ClientEvent(this, "setWanted", Character.WantedLVL.Level);

            SafeTrigger.SetData(this, "RESIST_STAGE", 0);
            SafeTrigger.SetData(this, "RESIST_TIME", 0);
            if (Character.AdminLVL > 0)
            {
                SafeTrigger.SetSharedData(this, "ALVL", Character.AdminLVL);
            }

            // MainMenu.SendStats(this);
            // MainMenu.SendProperty(this);

            if (this.Character.LVL == 0)
                SafeTrigger.ClientEvent(this, "disabledmg", true);



            //
            // var phoneAccount = Character.PhoneTemporary.Phone?.Account;

            // if (phoneAccount == null)
            // {
            //     NAPI.Task.Run(() =>
            //     {
            //         CreateAccountHandler.HandleCreateAccount(this, Character.FirstName, Character.LastName);
            //     });
            //     // // HandleCreateAccount(this, Character.FirstName, Character.LastName);
            //     // CreateAccountHandler.HandleCreateAccount(this, Character.FirstName, Character.LastName);
            //     return;
            // }
            //HandleCreateAccount
            //
            House house = HouseManager.GetHouse(this);
            if (house != null)
            {
                Roommate roommateData = house.GetRoommate(Character.UUID);
                if (roommateData != null) roommateData.SetCharacter(this);

                this.CreateClientBlip(HouseManager.PERSONAL_HOUSE_BLIP_ID, 40, "House", house.Position, 0.6F, 73, 0);
                if (house.HouseGarage != null)
                {
                    this.CreateClientMarker(333, 42, house.HouseGarage.Position - new Vector3(0, 0, 0.5), 2, NAPI.GlobalDimension, new Color(182, 211, 0), new Vector3(90, 90, 90));
                    SafeTrigger.ClientEvent(this, "createGarageBlip", house.HouseGarage.Position);
                }
            }

            var house2 = HouseManager.GetHouseFamily(this);

            if (house2 != null)
            {
                this.CreateClientMarker(334, 42, house2.HouseGarage.Position - new Vector3(0, 0, 0.5), 2, NAPI.GlobalDimension, new Color(220, 220, 0), new Vector3(90, 90, 90));
            }

            switch (index)
            {
                case 0:
                    var familyHose = HouseManager.GetHouse(Character.FamilyID, OwnerType.Family);
                    if (familyHose != null) Character.SpawnPos = familyHose.Position + new Vector3(0, 0, 1.2);
                    break;
                case 2:
                    if (house != null) Character.SpawnPos = house.Position + new Vector3(0, 0, 1.2);
                    break;
                case 3:
                    if (Character.FractionID != 0) Character.SpawnPos = Manager.FractionSpawns[Character.FractionID];
                    break;
                default:
                    break;
            }
            this.SyncInventoryId();
            if (Character.FractionID > 0) Manager.UpdateFracData(this);
            Character.OnPlayerSpawned?.Invoke(this);

            SafeTrigger.SetData(this, "LOGGED_IN", true);
            SafeTrigger.ClientEvent(this, "auth:doSpawn");

            if (Character.Warns > 0 && DateTime.Now > Character.Unwarn)
            {
                Character.Warns--;

                if (Character.Warns > 0)
                    Character.Unwarn = DateTime.Now.AddDays(14);
                Notify.Send(this, NotifyType.Warning, NotifyPosition.BottomCenter, $"Одно предупреждение было снято. У Вас осталось {Character.Warns.ToString()}", 3000);
            }

            if (Character.AdminLVL > 0)
                ReportManager.OnAdminLoad(this);
            Character.UpdateFriends(this);
            Character.UpdateReferal(this, true);

            DoorsService.SyncDoorStateForPlayer(this);
            SafeTrigger.ClientEvent(this, "exp:init", Character.EXP, Character.LVL);
            this.UpdatePrime();
            MoneyManager.SubscribePlayerToBankAccounts(this, Character);
            ForbesHandler.PlayerLoadForbesList(this);
            InventoryService.OnPlayerSpawn(this);


            uint dimension = this.Dimension;
            Character.IsSpawned = true;
            Character.IsAlive = true;

            if (Character.UnmuteDate > DateTime.Now)
            {
                var time = (Character.UnmuteDate - DateTime.Now).Milliseconds;
                this.MutePlayer(time, false);
            }
            if (Character.DemorganTime != 0)
            {
                if (!this.HasData("ARREST_TIMER"))
                {
                    SafeTrigger.SetData(this, "ARREST_TIMER", Timers.StartTask(1000, () => Admin.timer_demorgan(this)));
                    Core.Weapons.RemoveAll(this, true);
                    Admin.RemoveMasks(this);
                    this.SendTODemorgan();
                    SafeTrigger.UpdateDimension(this, 1337);
                }
                else _logger.WriteWarning($"ClientSpawn ArrestTime (DEMORGAN) worked avoid");
            }
            else if (Character.ArrestDate > DateTime.Now)
            {
                if (!this.HasData("ARREST_TIMER"))
                {
                    Character.SetArrestTimer(this);
                    this.ChangePosition(Police.policeCheckpoints[4]);
                }
                else _logger.WriteWarning($"ClientSpawn ArrestTime (KPZ) worked avoid");
            }
            else if (Character.CourtTime != 0)
            {
                if (!this.HasData("PRISON_TIME"))
                {
                    SafeTrigger.SetData(this, "PRISON_TIME", Timers.StartTask(1000, () => PrisonFib.timer_prisFib(this)));
                    this.ChangePosition(PrisonFib.randomPrisonpointFib());
                    SafeTrigger.UpdateDimension(this, (uint)Character.ArrestID);
                    SafeTrigger.ClientEvent(this, "Client_CheckIsInJail");
                    //Client_CheckIsInJail
                }
                else _logger.WriteWarning($"ClientSpawn ArrestTime (PRISON) worked avoid");
            }
            else
            {
                this.ChangePosition(Character.SpawnPos);

            }
            Core.Pets.Controller.InitializePlayerPet(this);
            HouseManager.GetHouse(this)?.ConnectedPlayer();
            SafeTrigger.ClientEvent(this, "ready", Character.UUID, Account.Login);
            SafeTrigger.SetSharedData(this, "InDeath", false);
            Main.InvokePlayerReady(this, Character);
            // MainMenu.UpdateBonusPoints(this);
            MainMenu.SendStats(this);
            MainMenu.LoadDonateList(this);
            MainMenu.SendProperty(this);
            NAPI.Task.Run(() => 
            {
                if (this.IsLogged() && dimension == this.Dimension) SafeTrigger.UpdateDimension(this, 0);
            }, 4000);
            LastSavePlayingTime = DateTime.Now;

            if (Character.LifeActivity != null)
            {
                if (Character.LifeActivity.Hunger != null) this.TriggerEventSafe("lifesystem:setStatsByKey", "hungerLevel", Character.LifeActivity.Hunger.Level);
                if (Character.LifeActivity.Thirst != null) this.TriggerEventSafe("lifesystem:setStatsByKey", "thirstLevel", Character.LifeActivity.Thirst.Level);
            }
        }
        public void DisconnectedPlayer(DisconnectionType type, string reason)
        {
            if (!Logged())
            {
                Account = null;
                Character = null;
                return;
            }
            try
            {
                Main.PlayerPreDisconnect?.Invoke(this);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Event_OnPlayerDisconnected 1:\n{e}");
            }
            Core.Pets.Controller.UnloadPlayerPet(this);
            LifeActivity.Destroy(this);
            PromoCodesService.UnSubscribe(this);
            FamilyManager.PlayerUnloadFamily(this, Character);
            ManagerMP.OnPlayerDisconnected(this);
            OrgBattleManager.OnPlayerDisconnected(this);
            WarCompanyManager.DisconnectedPlayer(this);
            VehicleManager.WarpPlayerOutOfVehicle(this);
            try
            {
                if (Character.Cuffed && Character.CuffedCop && Character.DemorganTime <= 0)
                {
                    Character.DemorganTime = 7200;
                    Core.Weapons.RemoveAll(this, true);
                    Admin.RemoveMasks(this);
                    Chat.AdminToAll($"{this.Name.Replace('_', ' ')} вышел из игры в наручниках, за что получает 2 часа demorgan!");
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Event_OnPlayerDisconnected 3:\n{e}");
            }

            try
            {
                House house = HouseManager.GetHouse(this);
                if (house != null) house.DisconnectedPlayer(Character.UUID);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Event_OnPlayerDisconnected 4:\n{e}");
            }


            MP.RoyalBattle.RoyalBattleService.OnPlayerDisconnected(this);
            MoneyManager.UnsubscribePlayerFromBankAccounts(Character);

            ParkingManager.DeleteParkingVehicle(this);

            Ems.onPlayerDisconnectedhandler(this, type, reason);
            Police.onPlayerDisconnectedhandler(this, type, reason);
            Fractions.PDA.PersonalDigitalAssistant.OnPlayerDisconnectedhandler(this, type, reason);
            LSNewsManager.OnPlayerDisconnectedhandler(this, type, reason);

            HouseManager.Event_OnPlayerDisconnected(this, type, reason);
            Manager.UnloadFraction(this);
            BusinessManager.TestDrive_PlayerDisconnected(this);

            PhoneLoader.PhoneDisconnect?.Invoke(Character);

            if (CasinoManager.Casinos.Any())
            {
                CasinoManager.FindFirstCasino().OnPlayerLeftGame(this);
                CasinoManager.FindFirstCasino().OnPlayerDisconnected(this);
            }

            try
            {
                if (Character.ParkingSpawnCar > 0)
                {
                    SafeTrigger.GetVehicleByDataId(Character.ParkingSpawnCar)?.CustomDelete();
                    Character.ParkingSpawnCar = -1;
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Event_OnPlayerDisconnected 5:\n{e}");
            }
            Voice.Voice.PlayerQuit(this);

            try
            {
                foreach (var veh in Character.TempVehicles)
                {
                    veh.Value?.CustomDelete();
                }
                Character.TempVehicles = new Dictionary<VehicleAccess, ExtVehicle>();
            }
            catch (Exception e)
            {
                _logger.WriteError($"Event_OnPlayerDisconnected Character.TempVehicles.CustomDelete:\n{e}");
            }
            AddPlayingTime();
            LastSavePlayingTime = DateTime.MaxValue;
            ClearMuteTimer();
            GameLog.Disconnected(Character.UUID);
            Character.DonateInventory.Unsubscribe(this);
            Character.UpdateData(this);
            //Account.CheckBonus(this, false);
            Character.Player = null;
            Character.Save();

            Character.Inventory?.Save();
            Character.Equip?.Save();

            Account = null;
            Character = null;
        }
        public void AddPrime(int days)
        {
            Character.AddPrime(days);
            this?.UpdatePrime();
        }
        public bool SubMCoins(int count)
        {
            if (Account.SubMCoins(count))
            {
                SendMCoinsInfoToMainMenu();
                return true;
            }
            return false;
        }
        public bool AddMCoins(int count)
        {
            if (Account.AddMCoins(count))
            {
                SendMCoinsInfoToMainMenu();
                return true;
            }
            return false;
        }
        public void SendMCoinsInfoToMainMenu()
        {
            if (this == null) return;

            SafeTrigger.ClientEvent(this, "dshop:coins:update", Account.MCoins);
        }
        internal void CreatePlayerAction(PlayerActions action, int progress)
        {
            if (Logged())
                EventManager.InvokeEvent(action, this, progress);
        }

        // public bool ChangeBonusPoints(int value)
        // {
        //     if (Character.ChangeBonusPoint(value))
        //     {
        //         MainMenu.UpdateBonusPoints(this);
        //         return true;
        //     }
        //     return false;
        // }

        public void AddPlayingTime()
        {
            DateTime now = DateTime.Now;
            if (LastSavePlayingTime > now) return;

            int currPlaying = (int)(now - LastSavePlayingTime).TotalMinutes;
            LastSavePlayingTime = now;
            CreatePlayerAction(PlayerActions.PlayingOnServer, currPlaying);
        }
        #region Methods on temp fields


        public void MutePlayer(int time, bool updateCharacter = true)
        {
            ClearMuteTimer();
            MUTE_TIMER = Timers.StartOnce(time * 1000 * 60, TimerMute);
            SafeTrigger.SetSharedData(this, "voice.muted", true);
            SafeTrigger.ClientEvent(this, "voice.mute");
            if (updateCharacter)
            {
                Character.UnmuteDate = DateTime.Now.AddMinutes(time);
                Character.VoiceMuted = true;
            }
        }
        public void Unmute()
        {
            NAPI.Task.Run(() =>
            {
                if (MUTE_TIMER == null) return;
                ClearMuteTimer();
                Character.UnmuteDate = DateTime.Now;
                Character.VoiceMuted = false;
                SafeTrigger.SetSharedData(this, "voice.muted", false);
            });
        }
        public void ClearMuteTimer()
        {
            if (MUTE_TIMER != null)
            {
                Timers.Stop(MUTE_TIMER);
                MUTE_TIMER = null;
            }
        }
        public void TimerMute()
        {
            Unmute();
            Notify.Send(this, NotifyType.Warning, NotifyPosition.BottomCenter, "Mute был снят, не нарушайте больше!", 3000);
        }

        #region Container In Hand

        public bool IsPlayerHaveContainer()
        {
            return ContainerInHand != null;
        }

        public void GiveContainerToPlayer(BaseItem item, AttachId attachId)
        {
            if (this == null) return;

            SafeTrigger.ClientEvent(this, "materialsSupply:pickContainer");
            AttachmentSync.AddAttachment(this, attachId);

            Main.OnAntiAnim(this);
            this.PlayAnimGo("anim@heists@box_carry@", "idle", AnimFlag.Looped | AnimFlag.CanMove | AnimFlag.UpperBody);

            ContainerInHand = item;
            Session.Container = attachId;
        }

        internal void GiveContainerToPlayer(VehicleItemBase item, AttachId attachId)
        {
            if (this == null) return;

            SafeTrigger.ClientEvent(this, "materialsSupply:pickContainer");
            AttachmentSync.AddAttachment(this, attachId);

            Main.OnAntiAnim(this);
            this.PlayAnimGo("anim@heists@box_carry@", "idle", AnimFlag.Looped | AnimFlag.CanMove | AnimFlag.UpperBody);

            ContainerInHand = item;
            Session.Container = attachId;
        }
        public object GetLincContainerFromPlayer()
        {
            if (!IsPlayerHaveContainer()) return null;
            return ContainerInHand;
        }

        public object TakeContainerFromPlayer()
        {
            if (!IsPlayerHaveContainer()) return null;

            object item = ContainerInHand;
            ContainerInHand = null;
            SafeTrigger.ClientEvent(this, "materialsSupply:takeContainer");
            AttachmentSync.RemoveAttachment(this, Session.Container);
            Session.Container = AttachId.invalid;

            Main.OffAntiAnim(this);
            this.StopAnimGo();
            if (item != null && item is BaseItem)
                DropSystem.DropItem(item as BaseItem, this.Position, this.Dimension, false);
            return item;
        }
        #endregion

        #endregion
    }
}
