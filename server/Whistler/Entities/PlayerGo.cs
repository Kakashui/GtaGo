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
using Whistler.Core.nAccount;
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
using Whistler.PersonalEvents;
using Whistler.PersonalEvents.Contracts.Models;
using Whistler.Phone;
using Whistler.Phone.Forbes;
using Whistler.ReferralSystem;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;

namespace Whistler.Entities
{
    public class PlayerGo : Player
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(PlayerGo));
        #region Temporary fields
        public int DATA_INTERACT_ID = -1;
        public string MUTE_TIMER = null;
        public bool IsMuted => MUTE_TIMER != null || (Character?.UnmuteDate ?? DateTime.Now) > DateTime.Now;
        public object ContainerInHand = null;
        public AttachId AttachIdContainer = AttachId.invalid;
        public DateTime LastSavePlayingTime = DateTime.MaxValue;
        public int StartQuestTempParam = 0;
        public ItemNames MetallPlantOre;
        public int OreVeinID = -1;
        #endregion

        public Account Account { get; set; }
        public Character Character { get; set; }
        public ReferralModel Referrals { get; set; }
        public bool Logged() => Character != null;
        public PlayerGo(NetHandle handle) : base(handle)
        {
            Account = null;
            Character = null;
        }
        public void LoadAccount(Account account)
        {
            Account = account;
            Main.CashAccounts.Add(account.Login, account);
        }
        public LoginEvent LoadAccount(DataRow row)
        {
            var socialClubId = Convert.ToUInt64(row["socialclubid"].ToString());
            var login = row["login"].ToString();
            if (socialClubId == 0)
            {
                var socialClub = row["socialclub"].ToString();
                if (socialClub != this.SocialClubName) 
                    return LoginEvent.SclubError;
                var exitsPlayerGo = NAPI.Pools.GetAllPlayers().FirstOrDefault(pl => pl.SocialClubName == socialClub && (pl as PlayerGo) != this) as PlayerGo;
                if (exitsPlayerGo == null)
                {
                    SaveSocialClubID(login, this.SocialClubId);
                    LoadAccountOrGetInCash(row, login);
                    return LoginEvent.Authorized;
                }
                else
                {
                    KickIfPlayerExist(login, exitsPlayerGo);
                    return LoginEvent.Already;
                }
            }
            else
            {
                if (socialClubId != this.SocialClubId)
                    return LoginEvent.SclubError;
                var exitsPlayerGo = NAPI.Pools.GetAllPlayers().FirstOrDefault(pl => pl.SocialClubId == socialClubId && (pl as PlayerGo) != this) as PlayerGo;
                if (exitsPlayerGo == null)
                {
                    LoadAccountOrGetInCash(row, login);
                    return LoginEvent.Authorized;
                }
                else
                {
                    KickIfPlayerExist(login, exitsPlayerGo);
                    return LoginEvent.Already;
                }
            }
        }

        private void LoadAccountOrGetInCash(DataRow row, string login)
        {
            if (Main.CashAccounts.ContainsKey(login))
                Account = Main.CashAccounts[login];
            else
            {
                Account = new Account(row, this.GetData<string>("RealHWID"), this.Address);
                Main.CashAccounts.Add(login, Account);
            }
        }
        private void KickIfPlayerExist(string login, PlayerGo exist)
        {
            _logger.WriteWarning($"loggiden Current login:{login} social:{this?.SocialClubId} hwid:{this?.Serial}");
            _logger.WriteWarning($"loggiden Exists login:{exist?.Account?.Login} social:{exist?.SocialClubId} hwid:{exist?.Serial}");
            exist?.TriggerEvent("kick", "logged:already:1");
            this?.TriggerEvent("kick", "logged:already:2");
        }
        private void SaveSocialClubID(string login, ulong scID)
        {
            MySQL.QuerySync("UPDATE accounts SET `socialclubid` = @prop0 WHERE `login` = @prop1", scID, login);
        }
        public bool LoadCharacter(int uuid)
        {
            if (!Account.Characters.Contains(uuid))
                return false;
            if (Main.CashCharacters.ContainsKey(uuid))
            {
                Character = Main.CashCharacters[uuid];
            }
            else
            {
                DataTable result = MySQL.QueryRead("SELECT * FROM `characters` WHERE uuid = @prop0", uuid);
                if (result == null || result.Rows.Count == 0)
                {
                    return false;
                }
                Character = new Character(result.Rows[0]);
                Main.CashCharacters.Add(Character.UUID, Character);
            }
            Character.SubscribeToSystem(this);
            return true;
        }

        public void CreateCharacter(Character character)
        {
            Character = character;
            Character.SubscribeToSystem(this);
            Main.CashCharacters.Add(Character.UUID, Character);
        }

        public string isFriend(PlayerGo target)
        {
            var friend = Character.Friends.Find(f => f.Nickname == target.Character.FullName);
            if(friend == null){
                return $"Незнакомец #{target.Character.UUID}";
            }else{
                return target.Character.FullName.Replace('_', ' ');
                //.Replace('_', ' ') NAPI.Util.ConsoleOutput($"{friend.Nickname}");
            }
        }

        public void ActionInvoke(Action<PlayerGo> action)
        {
            if (Logged())
                action?.Invoke(this);
        }

        public bool CheckPredicate(Func<PlayerGo, bool> func)
        {
            if (Logged())
                return func.Invoke(this);
            return false;
        }

        public void Spawn(int index)
        {
            this.TriggerEvent("dshop:cources:update",
                new List<int> { Main.ServerConfig.DonateConfig.CoinToVirtual, Main.ServerConfig.DonateConfig.RubToCoin },
                Main.ServerConfig.DonateConfig.Currency,
                JsonConvert.SerializeObject(Main.ServerConfig.DonateConfig.CoinKits));
            if (Character.Customization == null)
            {
                if (!CustomizationService.ConvertOldCustomization(this))
                    CustomizationService.SendToCreator(this, -1);
            }
            else
            {
                Character.Customization.Apply(this);
            }
            this.SetSharedData("IS_MASK", false);
            this.TriggerEvent("setadminlvl", Character.AdminLVL);
            this.TriggerEvent("UpdateMoney", Character.Money.ToString());
            this.TriggerEvent("UpdateBank", Character.BankModel.Balance.ToString());
            this.TriggerEvent("initPhone");

            FamilyManager.PlayerLoadFamily(this);
            Manager.LoadFraction(this);
            Jobs.WorkManager.load(this);

            this.Health = (Health > 5) ? Health : 5;

            this.SetSharedData("REMOTE_ID", this.Value);
            this.SetSharedData("C_ID", Character.UUID);
            this.SetSharedData("IS_MEDIA", Character.Media > 0);
            this.SetSharedData("IS_MEDIAHELPER", Character.MediaHelper > 0);
            Character.IconOverHead?.UpdateSharedData(this);

            Voice.Voice.PlayerJoin(this);

            this.SetSharedData("voipmode", -1);

            if (Character.AdminLVL > 0)
                GangsCapture.LoadBlips(this);

            if (Character.WantedLVL != null)
                this.TriggerEvent("setWanted", Character.WantedLVL.Level);

            this.SetData("RESIST_STAGE", 0);
            this.SetData("RESIST_TIME", 0);
            if (Character.AdminLVL > 0)
            {
                this.SetSharedData("ALVL", Character.AdminLVL);
            }

            MainMenu.SendStats(this);
            MainMenu.SendProperty(this);

            if (this.GetCharacter().LVL == 0)
                this.TriggerEvent("disabledmg", true);

            House house = HouseManager.GetHouse(this);
            if (house != null)
            {
                this.CreateClientBlip(HouseManager.PERSONAL_HOUSE_BLIP_ID, 40, "House", house.Position, 0.6F, 73, 0);
                if (house.HouseGarage != null)
                {
                    this.CreateClientMarker(333, 42, house.HouseGarage.Position - new Vector3(0, 0, 0.5), 2, NAPI.GlobalDimension, new Color(182, 211, 0), new Vector3(90, 90, 90));
                    this.TriggerEvent("createGarageBlip", house.HouseGarage.Position);
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

            this.SetData("LOGGED_IN", true);
            this.TriggerEvent("auth:doSpawn");

            if (Character.Warns > 0 && DateTime.Now > Character.Unwarn)
            {
                Character.Warns--;

                if (Character.Warns > 0)
                    Character.Unwarn = DateTime.Now.AddDays(14);
                Notify.Send(this, NotifyType.Warning, NotifyPosition.BottomCenter, $"Core_78".Translate(Character.Warns.ToString()), 3000);
            }

            if (Character.AdminLVL > 0)
                ReportManager.OnAdminLoad(this);
            Character.UpdateFriends(this);
            Character.UpdateReferal(this, true);

            DoorsService.SyncDoorStateForPlayer(this);
            Trigger.ClientEvent(this, "exp:init", Character.EXP, Character.LVL);
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
                    this.SetData("ARREST_TIMER", Timers.StartTask(1000, () => Admin.timer_demorgan(this)));
                    Core.Weapons.RemoveAll(this, true);
                    Admin.RemoveMasks(this);
                    this.SendTODemorgan();
                    this.Dimension = 1337;
                }
                else _logger.WriteWarning($"ClientSpawn ArrestTime (DEMORGAN) worked avoid");
            }
            else if (Character.ArrestDate > DateTime.UtcNow)
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
                    this.SetData("PRISON_TIME", Timers.StartTask(1000, () => PrisonFib.timer_prisFib(this)));
                    this.ChangePosition(PrisonFib.randomPrisonpointFib());
                    this.Dimension = (uint)Character.ArrestID;
                }
                else _logger.WriteWarning($"ClientSpawn ArrestTime (PRISON) worked avoid");
            }
            else
            {
                this.ChangePosition(Character.SpawnPos);

            }
            HouseManager.GetHouse(this)?.ConnectedPlayer();
            Trigger.ClientEvent(this, "ready");
            this.SetData("spmode", false);
            this.SetSharedData("InDeath", false);
            Main.InvokePlayerReady(this, Character);
            MainMenu.UpdateBonusPoints(this);
            WhistlerTask.Run(() => {
                if (this.IsLogged() && dimension == this.Dimension) this.Dimension = 0;
            }, 4000);
            LastSavePlayingTime = DateTime.Now;
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
                    Chat.AdminToAll("Com_145".Translate(this.Name.Replace('_', ' ')));
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Event_OnPlayerDisconnected 3:\n{e}");
            }

            try
            {
                HouseManager.GetHouse(this)?.DisconnectedPlayer(Character.UUID);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Event_OnPlayerDisconnected 4:\n{e}");
            }


            MP.RoyalBattle.RoyalBattleService.OnPlayerDisconnected(this);
            MoneyManager.UnsubscribePlayerFromBankAccounts(Character);


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
                    VehicleManager.GetVehicleByUUID(Character.ParkingSpawnCar)?.CustomDelete();
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
                Character.TempVehicles = new Dictionary<VehicleAccess, Vehicle>();
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
            Account.CheckBonus(this, false);
            Character.Player = null;
            Character.Save();

            Character.Inventory?.Save();
            Character.Equip?.Save();

            Account = null;
            Character = null;
        }
        public void AddPrime(int days)
        {
            Account.AddPrime(days);
            this?.UpdatePrime();
        }
        public bool SubGoCoins(int count)
        {
            if (Account.SubGoCoins(count))
            {
                SendGoCoinsInfoToMainMenu();
                return true;
            }
            return false;
        }
        public bool AddGoCoins(int count)
        {
            if (Account.AddGoCoins(count))
            {
                SendGoCoinsInfoToMainMenu();
                return true;
            }
            return false;
        }
        public void SendGoCoinsInfoToMainMenu()
        {
            this?.TriggerEvent("dshop:coins:update", Account.GoCoins);
        }
        internal void CreatePlayerAction(PlayerActions action, int progress)
        {
            if (Logged())
                EventManager.InvokeEvent(action, this, progress);
        }

        public bool ChangeBonusPoints(int value)
        {
            if (Character.ChangeBonusPoint(value))
            {
                MainMenu.UpdateBonusPoints(this);
                return true;
            }
            return false;
        }

        public void AddPlayingTime()
        {
            if (LastSavePlayingTime > DateTime.Now)
                return;
            int currPlaying = (int)(DateTime.Now - LastSavePlayingTime).TotalMinutes;
            LastSavePlayingTime = DateTime.Now;
            CreatePlayerAction(PlayerActions.PlayingOnServer, currPlaying);
        }
        #region Methods on temp fields


        public void MutePlayer(int time, bool updateCharacter = true)
        {
            ClearMuteTimer();
            MUTE_TIMER = Timers.StartOnce(time * 1000 * 60, TimerMute);
            this.SetSharedData("voice.muted", true);
            Trigger.ClientEvent(this, "voice.mute");
            if (updateCharacter)
            {
                Character.UnmuteDate = DateTime.Now.AddMinutes(time);
                Character.VoiceMuted = true;
            }
        }
        public void Unmute()
        {
            WhistlerTask.Run(() =>
            {
                if (MUTE_TIMER == null) return;
                ClearMuteTimer();
                Character.UnmuteDate = DateTime.Now;
                Character.VoiceMuted = false;
                this.SetSharedData("voice.muted", false);
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
            Notify.Send(this, NotifyType.Warning, NotifyPosition.BottomCenter, "local_110", 3000);
        }

        #region Container In Hand

        public bool IsPlayerHaveContainer()
        {
            return ContainerInHand != null;
        }

        public void GiveContainerToPlayer(BaseItem item, AttachId attachId)
        {
            this.TriggerEvent("materialsSupply:pickContainer");
            AttachmentSync.AddAttachment(this, attachId);

            Main.OnAntiAnim(this);
            this.PlayAnimGo("anim@heists@box_carry@", "idle", AnimFlag.Looped | AnimFlag.CanMove | AnimFlag.UpperBody);

            ContainerInHand = item;
            AttachIdContainer = attachId;
        }

        internal void GiveContainerToPlayer(VehicleItemBase item, AttachId attachId)
        {
            this.TriggerEvent("materialsSupply:pickContainer");
            AttachmentSync.AddAttachment(this, attachId);

            Main.OnAntiAnim(this);
            this.PlayAnimGo("anim@heists@box_carry@", "idle", AnimFlag.Looped | AnimFlag.CanMove | AnimFlag.UpperBody);

            ContainerInHand = item;
            AttachIdContainer = attachId;
        }
        public object GetLincContainerFromPlayer()
        {
            if (!IsPlayerHaveContainer())
                return null;
            return ContainerInHand;
        }

        public object TakeContainerFromPlayer()
        {
            if (!IsPlayerHaveContainer())
                return null;

            var item = ContainerInHand;
            ContainerInHand = null;

            this.TriggerEvent("materialsSupply:takeContainer");
            AttachmentSync.RemoveAttachment(this, AttachIdContainer);

            AttachIdContainer = AttachId.invalid;

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
