using AutoMapper.Internal;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Whistler.Common;
using Whistler.Common.Interfaces;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Families.FamilyMenu;
using Whistler.Families.FamilyWars;
using Whistler.Families.Models.DTO;
using Whistler.Fractions;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.Houses;
using Whistler.MoneySystem;
using Whistler.MoneySystem.DTO;
using Whistler.MoneySystem.Interface;
using Whistler.PersonalEvents;
using Whistler.PersonalEvents.Contracts;
using Whistler.PersonalEvents.Contracts.Models;
using Whistler.SDK;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.Families.Models
{
    internal class Family : IBankAccount, IOrganization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Owner { get; set; }
        public long Money { get; private set; }
        public string Nation { get; set; }
        public string Biography { get; set; }
        public Dictionary<int, FamilyRank> Ranks { get; set; }
        public int ChatIcon { get; set; }
        public string ChatColor { get; set; }
        public int EloRating { get; set; }
        public int CountBattles { get; set; }
        public List<string> Taboo { get; set; }
        public List<string> Rules { get; set; }
        public Vector3 ClothesPoint { get; set; }
        public uint ClothesDimension { get; set; }
        public int MoneyLimit { get; private set; }
        public OrgActivityType OrgActiveType { get; set; }
        public int Points { get; protected set; }

        //Not saved to family table
        private InteractShape _clothesShape { get; set; }
        public int Rating { get; set; }
        public Dictionary<int, FamilyMember> Members { get; set; }
        public Dictionary<int, ExtPlayer> OnlineMembers { get; set; }
        public string OwnerName { get { return Main.PlayerNames.GetOrDefault(Owner) ?? "None"; } }
        public FamilyEloParams EloParams { get { return new FamilyEloParams(Id, Name, OwnerName, CountBattles, CountBusiness, Members.Count, GlobalRating, Rating); } }
        public DateTime NextBizWarTime = DateTime.Now;
        public DateTime ProtectBizWarDate = DateTime.Now;
        public int MoneyForEnterprise = 0;
        public int CountBusiness => BusinessManager.BizList.Values.Where(item => item.FamilyPatronage == Id).Count();       
        public int GlobalRating => EloRating + Points + (int)(Members.Count * 0.2);
        public long IMoneyBalance => Money;
        public TypeMoneyAcc ITypeMoneyAcc => TypeMoneyAcc.Family;
        public int IOwnerID => Id;
        public TypeBankAccount ITypeBank => TypeBankAccount.Family;
        OrganizationType IOrganization.TypeOrganization => OrganizationType.Family;

        public List<OrgPaymentDTO> PaymentHystory { get; set; }

        private bool _editing = false;

        public ContractInfo FamilyContracts;
        public int RespectPoints { get; set; }

        public Family(string name, int owner)
        {
            Name = name;
            Owner = owner;
            Money = 0;
            Biography = "";
            Nation = "";
            ChatIcon = 1;
            ChatColor = "#FF41E0";
            InitStandartRank();
            Taboo = new List<string>();
            Rules = new List<string>();
            Members = new Dictionary<int, FamilyMember>();
            ClothesPoint = null;
            ClothesDimension = 0;
            EloRating = 1000;
            CountBattles = 0;
            MoneyLimit = 0;
            OrgActiveType = OrgActivityType.Neutral;
            Points = 0;
            var dataQuery = MySQL.QueryRead("INSERT INTO `families`(`f_name`, `f_owner`, `f_money`, `f_nation`, `f_biography`, `f_ranks`, `f_chaticon`, `f_chatcolor`, `f_elo`, `f_cntbattles`, `f_typefam`, `f_points` ) " +
                "VALUES (@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6, @prop7, @prop8, @prop9, @prop10, @prop11); SELECT @@identity;",
                Name, Owner, Money, Nation, Biography, JsonConvert.SerializeObject(Ranks), ChatIcon, ChatColor, EloRating, CountBattles, (int)OrgActiveType, Points);
            Id = Convert.ToInt32(dataQuery.Rows[0][0]);
            Rating = Id;
            OnlineMembers = new Dictionary<int, ExtPlayer>();
            FamilyContracts = new ContractInfo();
            NewMember(owner, 0);
        }

        public Family(DataRow row)
        {
            Id = Convert.ToInt32(row["f_id"]);
            Name = Convert.ToString(row["f_name"]);
            Owner = Convert.ToInt32(row["f_owner"]);
            Money = Convert.ToInt32(row["f_money"]);
            Nation = Convert.ToString(row["f_nation"]);
            Biography = Convert.ToString(row["f_biography"]);
            ChatIcon = Convert.ToInt32(row["f_chaticon"]);
            ChatColor = Convert.ToString(row["f_chatcolor"]);
            Taboo = JsonConvert.DeserializeObject<List<string>>(Convert.ToString(row["f_taboo"]));
            Rules = JsonConvert.DeserializeObject<List<string>>(Convert.ToString(row["f_rules"]));
            ClothesPoint = JsonConvert.DeserializeObject<Vector3>(Convert.ToString(row["f_clothespoint"]));
            ClothesDimension = Convert.ToUInt32(row["f_clothesdim"]);
            EloRating = Convert.ToInt32(row["f_elo"]);
            CountBattles = Convert.ToInt32(row["f_cntbattles"]);
            MoneyLimit = Convert.ToInt32(row["f_moneylimit"]);
            OrgActiveType = (OrgActivityType)Convert.ToInt32(row["f_typefam"]);
            Points = Convert.ToInt32(row["f_points"]);
            RespectPoints = Convert.ToInt32(row["f_respectPoints"]);

            try
            {
                Ranks = JsonConvert.DeserializeObject<Dictionary<int, FamilyRank>>(Convert.ToString(row["f_ranks"]));
                Ranks[0].AccessMemberManagement = true;
            }
            catch
            {
                InitStandartRank();
                _editing = true;
            }
            OnlineMembers = new Dictionary<int, ExtPlayer>();

            Members = new Dictionary<int, FamilyMember>();
            var result = MySQL.QueryRead(
                "SELECT `uuid`, `familylvl`, `familypoints`, `familypointsadd`, `familypointssub`, `familypointlastupdate` " +
                "FROM `characters` " +
                "WHERE `family` = @prop0 and `deleted` = false", 
                Id);
            if (result != null)
            {
                foreach (DataRow Row in result.Rows)
                {
                    int uuid = Convert.ToInt32(Row["uuid"]);
                    int familylvl = Convert.ToInt32(Row["familylvl"]);
                    int points = Convert.ToInt32(Row["familypoints"]);
                    int pointsAdd = Convert.ToInt32(Row["familypointsadd"]);
                    int pointsSub = Convert.ToInt32(Row["familypointssub"]);
                    DateTime pointsUpdate = Row["familypointlastupdate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(Row["familypointlastupdate"]);
                    Members.Add(uuid, new FamilyMember(uuid, familylvl, points, pointsAdd, pointsSub, pointsUpdate));
                }
            }

            FamilyContracts = EventManager.ContractLoadOrCreate(Id, ContractTypes.Family);
            UpdateClothesPoint();
        }

        public void Save()
        {
            foreach (var member in Members.Values)
            {
                member.Save();
            }
            FamilyContracts.Save(ContractTypes.Family, Id);
            if (!_editing)
                return;
            MySQL.Query("UPDATE `families` SET" +
                " `f_nation` = @prop1," +
                " `f_biography` = @prop2," +
                " `f_ranks` = @prop3," +
                " `f_chaticon` = @prop4," +
                " `f_chatcolor` = @prop5," +
                " `f_elo` = @prop6," +
                " `f_cntbattles` = @prop7," +
                " `f_taboo` = @prop8," +
                " `f_rules` = @prop9," +
                " `f_points` = @prop10," +
                " `f_respectPoints` = @prop11" +
                " WHERE `f_id` = @prop0", 
                Id, 
                Nation,
                Biography,
                JsonConvert.SerializeObject(Ranks),
                ChatIcon,
                ChatColor,
                EloRating,
                CountBattles,
                JsonConvert.SerializeObject(Taboo),
                JsonConvert.SerializeObject(Rules),
                Points,
                RespectPoints);
            _editing = false;
        }
        private void InitStandartRank()
        {
            Ranks = new Dictionary<int, FamilyRank> { { 0, new FamilyRank(0) }, { 1, new FamilyRank(1) } };
            Ranks[0].Name = "Leader";
            Ranks[0].AccessHouse = FamilyHouseAccess.FullAccess;
            Ranks[0].AccessFurniture = FamilyFurnitureAccess.ManagementFurniture;
            Ranks[0].AccessClothes = true;
            Ranks[0].AccessBizWar = true;
            Ranks[0].AccessMemberManagement = true;
        }

        private void UpdateClothesPoint()
        {
            if (_clothesShape != null)
                _clothesShape.Destroy();
            if (ClothesPoint == null)
                return;
            _clothesShape = InteractShape.Create(ClothesPoint, 1, 2, ClothesDimension)
                .AddInteraction(SkinManager.OpenFamilySkinMenu, "Чтобы переодеться")
                .AddEnterPredicate(EnterPredicate)
                .AddDefaultMarker();

        }

        public void SetClothesPoint(Vector3 point, uint dim)
        {
            ClothesPoint = point;
            ClothesDimension = dim;
            MySQL.Query("UPDATE `families` SET" +
                " `f_clothespoint` = @prop1, `f_clothesdim` = @prop2" +
                " WHERE `f_id` = @prop0",
                Id,
                JsonConvert.SerializeObject(ClothesPoint),
                ClothesDimension);
            UpdateClothesPoint();
        }
        public void SetFamilyType(OrgActivityType type)
        {
            if (OrgActiveType != type)
            {
                OrgActiveType = type;
                MySQL.Query("UPDATE `families` SET" +
                    " `f_typefam` = @prop1 " +
                    " WHERE `f_id` = @prop0",
                    Id,
                    (int)OrgActiveType);
            }
        }

        private bool EnterPredicate(ColShape shape, ExtPlayer player)
        {
            if (!player.IsLogged())
                return false;
            return CanAccessToClothes(player);
        }

        public void PreSave()
        {
            _editing = true;
        }

        public bool IsOwner(ExtPlayer player)
        {
            return IsOwner(player?.Character?.UUID ?? -1);
        }

        public bool IsOwner(int playerUUID)
        {
            return Owner == playerUUID;
        }

        public bool IsLeader(ExtPlayer player)
        {
            return IsLeader(player?.Character?.UUID ?? -1);
        }

        public bool IsLeader(int playerUUID)
        {
            return Owner == playerUUID || Members.ContainsKey(playerUUID) && Members[playerUUID].Rank == 0;
        }
        #region Money
        public bool MoneyAdd(int amount)
        {
            if (amount <= 0)
                return false;
            ChangeMoney(amount);
            return true;
        }

        public bool MoneySub(int amount, bool limit = false)
        {
            if (amount <= 0)
                return false;
            if (Money - amount < 0)
                return false;
            ChangeMoney(-amount);
            if (limit)
                ChangeMoneyLimit(MoneyLimit + amount);
            return true;
        }
        private void ChangeMoney(int amount)
        {
            Money += amount;
            MySQL.Query("UPDATE `families` SET" +
                " `f_money` = @prop1" +
                " WHERE `f_id` = @prop0",
                Id, Money);
        }
        public void ChangeMoneyLimit(int value)
        {
            MoneyLimit = value;
            MySQL.Query("UPDATE `families` SET" +
                " `f_moneylimit` = @prop1" +
                " WHERE `f_id` = @prop0",
                Id, MoneyLimit);
        }
        #endregion

        #region Access
        private bool CheckPlayer(ExtPlayer player)
        {
            var character = player.Character;
            if (character == null)
                return false;
            if (!Members.ContainsKey(character.UUID))
                return false;
            var member = Members[character.UUID];
            if (!Ranks.ContainsKey(member.Rank) && member.Rank > 0)
                member.ChangeRank(this, 1);
            return true;
        }
        public bool CanAccessToHouse(ExtPlayer player, FamilyHouseAccess houseAccess)
        {
            if (!CheckPlayer(player))
                return false;
            return IsLeader(player) || Ranks[Members[player.Character.UUID].Rank].AccessHouse >= houseAccess;
        }

        public bool CanAccessToFurniture(ExtPlayer player, FamilyFurnitureAccess furnAccess)
        {
            if (!CheckPlayer(player))
                return false;
            return IsLeader(player) || Ranks[Members[player.Character.UUID].Rank].AccessFurniture >= furnAccess;
        }

        public bool CanAccessToVehicle(ExtPlayer player, int vehicleID, FamilyVehicleAccess vehicleAccess)
        {
            if (!CheckPlayer(player))
                return false;
            if (Owner == player.Character.UUID)
                return true;
            return IsLeader(player) || Ranks[Members[player.Character.UUID].Rank].AccessVehicles.GetValueOrDefault(vehicleID, FamilyVehicleAccess.None) >= vehicleAccess;
        }

        public bool CanAccessToClothes(ExtPlayer player)
        {
            if (!CheckPlayer(player))
                return false;
            return IsLeader(player) || Ranks[Members[player.Character.UUID].Rank].AccessClothes;
        }

        public bool CanAccessToBizWar(ExtPlayer player)
        {
            if (!CheckPlayer(player)) return false;
            return IsLeader(player) || Ranks[Members[player.Character.UUID].Rank].AccessBizWar;
        }
        public bool CanAccessToMemberManagement(ExtPlayer player)
        {
            if (!CheckPlayer(player))
                return false;
            return IsLeader(player) || Ranks[Members[player.Character.UUID].Rank].AccessMemberManagement;
        }
        #endregion

        public bool DeleteMember(int uuid)
        {
            if (!Members.ContainsKey(uuid))
                return false;
            if (IsOwner(uuid))
            {
                var member = GetNewOwnerId();
                if (member > 0)
                {
                    if (!IsLeader(member))
                        Members.GetValueOrDefault(member)?.ChangeRank(this, 0);
                }
                SetOwner(member);
            }
            Members.Remove(uuid);
            SkinManager.TakePlayerCostumes(uuid, Inventory.Enums.ClothesOwn.Family);
            var player = Trigger.GetPlayerByUuid(uuid);
            MySQL.Query("UPDATE characters SET family = 0, familylvl = 0 WHERE uuid = @prop0", uuid);
            if (player != null)
            {
                if (player.Character.FamilyID == Id)
                {
                    player.Character.FamilyID = 0;
                    player.Character.FamilyLVL = 0;
                }
                DisconnectedMember(uuid);
                FamilyMenuManager.UnloadMenuData(player);
                SubscribeSystem.UnsubscribeMember(player, Id);
                SafeTrigger.SetSharedData(player, "familyuuid", 0);
                SafeTrigger.SetSharedData(player, "familyname", "None");
                MainMenu.SendStats(player);
            }
            FamilyMenuManager.RemoveMemberFromFamily(this, uuid);
            FamilyMenuManager.UpdateFamilyRatingData(this);
            return true;
        }

        public void SetOwner(ExtPlayer player)
        {
            SetOwner(player.Character.UUID);
        }
        public void SetOwner(int uuid)
        {
            if (uuid > 0 && !IsLeader(uuid) )
                return;
            int oldMember = Owner;
            Owner = uuid;
            MySQL.Query("UPDATE `families` SET `f_owner` = @prop0 WHERE `f_id` = @prop1", Owner, Id);
            FamilyMenuManager.UpdateMemberToFamily(this, oldMember, OnlineMembers.ContainsKey(oldMember));
            FamilyMenuManager.UpdateMemberToFamily(this, Owner, OnlineMembers.ContainsKey(Owner));
        }

        public int GetNewOwnerId()
        {
            var member = Members.FirstOrDefault(item => item.Value.Rank == 0 && item.Key != Owner);
            if (member.Value != null)
                return member.Key;
            member = Members.FirstOrDefault(item => item.Key != Owner);
            if (member.Value != null)
                return member.Key;
            else
                return -1;
        }

        public void ConnectedMember(int uuid, ExtPlayer player)
        {
            if (OnlineMembers.ContainsKey(uuid))
                OnlineMembers[uuid] = player;
            else
                OnlineMembers.Add(uuid, player);
        }

        public void DisconnectedMember(int uuid)
        {
            if (OnlineMembers.ContainsKey(uuid))
                OnlineMembers.Remove(uuid);
        }

        public void NewMember(int uuid, int rank)
        {
            if (Members.ContainsKey(uuid)) return;

            Members.Add(uuid, new FamilyMember(this, uuid, rank));
            FamilyMenuManager.UpdateFamilyRatingData(this);
        }
        #region RankManager
        public FamilyRank CreateRank()
        {
            if (Ranks.Count >= FamilyConstants.MAX_RANK_IN_FAMILY)
                return null;
            int id = Ranks.Keys.Max() + 1;
            Ranks.Add(id, new FamilyRank(id));
            return Ranks[id];
        }

        public FamilyRank UpdateRank(int rankId, string rankName, int accessHouse, int accessFurniture, bool accessClothes, bool accessWar, bool accessMemberManagement, string accessCarsJson)
        {
            if (!Ranks.ContainsKey(rankId)) return null;

            Ranks[rankId].Name = rankName;
            Ranks[rankId].AccessHouse = (FamilyHouseAccess)accessHouse;
            Ranks[rankId].AccessFurniture = (FamilyFurnitureAccess)accessFurniture;
            Ranks[rankId].AccessClothes = accessClothes;
            Ranks[rankId].AccessBizWar = accessWar;
            Ranks[rankId].AccessMemberManagement = accessMemberManagement;
            Dictionary<int, int> accessCars = JsonConvert.DeserializeObject<Dictionary<int, int>>(accessCarsJson);
            foreach (var car in accessCars)
            {
                Ranks[rankId].SetVehicleAccess(car.Key, car.Value);
            }
            return Ranks[rankId];
        }

        public FamilyRank UpdateRankName(int rankId, string rankName)
        {
            if (!Ranks.ContainsKey(rankId))
                return null;
            Ranks[rankId].Name = rankName;
            return Ranks[rankId];
        }

        public bool DeleteRank(int rankId)
        {
            if (!Ranks.ContainsKey(rankId) || rankId < 2)
                return false;
            Ranks.Remove(rankId);
            foreach (var item in Members.Where(item => item.Value.Rank == rankId))
            {
                item.Value.ChangeRank(this, 1);
            }
            return true;
        }
        #endregion

        #region ChangeInfo
        public void ChangeNation(string nation)
        {
            Nation = nation;
            PreSave();
            SubscribeSystem.TriggerEventToSubscribe(Id, "family:newNation", nation);
        }
        public void ChangeBiography(string biography)
        {
            Biography = biography;
            PreSave();
            SubscribeSystem.TriggerEventToSubscribe(Id, "family:newBiography", biography);
        }
        public void ChangeRules(List<string> taboo, List<string> rules)
        {
            Taboo = taboo;
            Rules = rules;
            PreSave();
            SubscribeSystem.TriggerEventToSubscribe(Id, "family:newRules", JsonConvert.SerializeObject(taboo), JsonConvert.SerializeObject(rules));
        }
        #endregion
        public void AddVehicle(PersonalBaseVehicle vehData)
        {
            FamilyMenuManager.UpdateVehicleToFamily(this, vehData);
        }
        public void RemoveVehicle(PersonalBaseVehicle vehData)
        {
            foreach (var rank in Ranks)
                rank.Value.DeleteVehicleAccess(vehData.ID);
            FamilyMenuManager.RemoveVehicleFromFamily(this, vehData.ID);
            PreSave();
        }

        internal List<FamilyRankDto> GetRanksDto()
        {
            List<FamilyRankDto> list = new List<FamilyRankDto>();
            foreach (var rank in Ranks)
            {
                list.Add(new FamilyRankDto(rank.Value));
            }
            return list;
        }

        internal House GetHouse()
        {
            return HouseManager.GetHouse(Id, OwnerType.Family);
        }

        internal void AddFamilyBattle(int eloChanging)
        {
            CountBattles++;
            EloRating += eloChanging;
            if (EloRating <= 0)
                EloRating = 0;
            PreSave();
            Save();
            FamilyMenuManager.UpdateFamilyRatingData(this);
        }

        public int GetMembersInBattle(BattleLocation id)
        {
            return OnlineMembers.Where(item => item.Value.IsLogged() && item.Value.Character.IsAlive && item.Value.Character.WarZone == id).Count();
        }

        string IOrganization.GetName()
        {
            return Name;
        }
        public void ChangePoints(int points)
        {
            if (Points + points < 0)
                Points = 0;
            else
                Points += points;
            PreSave();
        }
        public void ClearPoints()
        {
            Points = 0;
            EloRating = 1000;
            CountBattles = 0;
            PreSave();
            FamilyMenuManager.UpdateFamilyRatingData(this);
        }
        public List<OrgPaymentDTO> GetMemberPayments()
        {
            if (PaymentHystory == null) PaymentHystory = BankManager.GetMemberPayments(Members.Values, this, "Премия (семья)", 30);
            return PaymentHystory;
        }
        public void UpdatePayments(OrgPaymentDTO payment)
        {
            if (PaymentHystory == null) PaymentHystory = BankManager.GetMemberPayments(Members.Values, this, "Премия (семья)", 30);
            OrgPaymentDTO existPayment = PaymentHystory.FirstOrDefault(item => item.uuid == payment.uuid);
            if (existPayment != null) PaymentHystory.Remove(existPayment);
            PaymentHystory.Add(payment);
        }

        public bool ChangeRespectPoints(int value)
        {
            if (RespectPoints + value < 0)
                return false;
            RespectPoints += value;
            PreSave();
            return true;
        }
    }
    class CompFamilyByRating<T> : IComparer<T> //sort by val asc
        where T : Family
    {
        public int Compare(T x, T y)
        {
            if (x.GlobalRating > y.GlobalRating)  //sorted desc
                return -1;
            else if (x.GlobalRating < y.GlobalRating)
                return 1;
            else if (x.CountBattles > y.CountBattles)
                return -1;
            else if (x.CountBattles < y.CountBattles)
                return 1;
            else if (x.CountBusiness > y.CountBusiness)
                return -1;
            else if (x.CountBusiness < y.CountBusiness)
                return 1;
            else if (x.Members.Count > y.Members.Count)
                return -1;
            else if (x.Members.Count < y.Members.Count)
                return 1;
            else if (x.Money > y.Money)
                return -1;
            else if (x.Money < y.Money)
                return 1;
            return 0;
        }
    }
}
