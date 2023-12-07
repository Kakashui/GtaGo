using AutoMapper.Internal;
using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Common;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Families.FamilyMP;
using Whistler.Families.FamilyMP.DTO;
using Whistler.Families.FamilyMP.Models;
using Whistler.Families.FamilyWars;
using Whistler.Families.Models;
using Whistler.Families.WarForCompany;
using Whistler.Families.WarForCompany.DTO;
using Whistler.Families.WarForCompany.Models;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.Houses;
using Whistler.MoneySystem;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.Families.FamilyMenu
{
    class FamilyMenuManager : Script
    {
        public static event Action<PlayerGo, Family> FamilyLoad;
        public static void LoadMenuData(PlayerGo player, Family fam)
        {
            var house = HouseManager.GetHouse(fam.Id, OwnerType.Family);
            var data = new
            {
                familyId = fam.Id,
                leaders = fam.Members.Where(memb => memb.Value.Rank == 0).Select(memb => new { id = memb.Value.PlayerUUID, name = Main.PlayerNames.GetOrDefault(memb.Value.PlayerUUID) }),
                nation = fam.Nation,
                houseType = house?.TypeHouse?.Name ?? "None",
                bankBalance = fam.Money.ToString(),
                biography = fam.Biography,
                familyRank = fam.Rating,
                tabooList = fam.Taboo,
                rulesList = fam.Rules,
            };
            var members = fam.Members.Select(memb => memb.Value.GetFamilyMemberDTO(fam.OnlineMembers.ContainsKey(memb.Key))).ToList();
            var cars = VehicleManager.getAllHolderVehicles(fam.Id, OwnerType.Family).Select(item => new { model = VehicleManager.Vehicles[item].ModelName, number = VehicleManager.Vehicles[item].Number, key = item });
            var ranks = fam.GetRanksDto();
            var familyRating = FamilyManager.GetFamilyRatings();
            var bizz = BusinessManager.BizList.Values
                .Where(item => item.FamilyPatronage == fam.Id)
                .Select(item => item.GetFamilyData());
            var allBizz = BusinessManager.BizList.Values
                .Select(item => item.GetFamilyData());
            var battles = WarManager.GetFamilyBattles(fam.Id).Select(item => item.GetBattleDTO(fam.Id));

            Trigger.ClientEvent(player, "family:loadData", JsonConvert.SerializeObject(data), player.GetCharacter().FamilyLVL, fam.ChatIcon, fam.ChatColor, JsonConvert.SerializeObject(members), fam.Name, JsonConvert.SerializeObject(bizz), JsonConvert.SerializeObject(allBizz), player.GetCharacter().UUID);
            Trigger.ClientEvent(player, "family:loadCarsAndRanks", JsonConvert.SerializeObject(cars), JsonConvert.SerializeObject(ranks));
            Trigger.ClientEvent(player, "family:loadFamilyRatings", JsonConvert.SerializeObject(familyRating));
            Trigger.ClientEvent(player, "family:loadFamilyBattles", JsonConvert.SerializeObject(battles));
            FamilyLoad?.Invoke(player, fam);
        }
        
        public static void UnloadMenuData(Player player)
        {
            Trigger.ClientEvent(player, "family:unloadData");
        }

        [RemoteEvent("family:openFamilyMenu")]
        public static void OpenFamilyMenu(Player player)
        {
            if (!player.IsLogged())
                return;
            Family family = player.GetFamily();
            if (family == null)
                return;
            Trigger.ClientEvent(player, "family:openMenu", family.Money.ToString());
        }

        #region Money

        [RemoteEvent("family:takeMoney")]
        public static bool RemoteEvent_tryTakeMoney(Player player, int amount)
        {
            if (!player.IsLogged())
                return false;
            Family family = player.GetFamily();
            if (amount < 0)
                return false;
            if (family == null)
                return false;
            if (!family.IsLeader(player))
                return false;
            if (player.GetCharacter().AdminLVL < 8 && amount + family.MoneyLimit > FamilyConstants.MAX_MONEY_SUB_IN_DAY)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Fam_43".Translate(FamilyConstants.MAX_MONEY_SUB_IN_DAY, FamilyConstants.MAX_MONEY_SUB_IN_DAY - family.MoneyLimit), 3000);
                return false;
            }
            if (Wallet.TransferMoney(family, player.GetBankAccount(), amount, 0, "Money_TakeFamMoney", player.GetCharacter().AdminLVL < 8))
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Fam_32".Translate(amount), 3000);
                Chat.SendToAdmins(5, "Fam_41".Translate(player.Name, family.Name, amount));
                Chat.CMD_FamilyRadio(player, "Fam_42".Translate(amount));
                if (amount >= 1000000)
                    FamilyManager.ChangePoints(player, FamilyActions.SubMoney);
                return true;
            }
            else
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Fam_34", 3000);
            }
            Trigger.ClientEvent(player, "family:updateMoney", family.Money.ToString());
            return false;
        }

        [RemoteEvent("family:putMoney")]
        public static bool RemoteEvent_tryPutMoney(Player player, int amount)
        {
            if (!player.IsLogged())
                return false;
            Family family = player.GetFamily();
            if (amount < 0)
                return false;
            if (family == null)
                return false;
            if (Wallet.TransferMoney(player.GetBankAccount(), family, amount, 0, "Money_PutFamMoney"))
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Fam_33".Translate(amount), 3000);
                Chat.SendToAdmins(5, "Fam_39".Translate(player.Name, family.Name, amount));
                Chat.CMD_FamilyRadio(player, "Fam_40".Translate(amount));
                if (amount >= 1000000)
                    FamilyManager.ChangePoints(player, FamilyActions.AddMoney);
                return true;
            }
            else
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Fam_34", 3000);
            }            
            Trigger.ClientEvent(player, "family:updateMoney", family.Money.ToString());
            return false;
        }
        #endregion

        #region Change Family Info
        [RemoteEvent("family:tryChangeName")]
        public static void RemoteEvent_tryChangeName(Player player, string name)
        {
            if (!player.IsLogged())
                return;
            Family family = player.GetFamily();
            if (family == null)
                return;
            if (!family.IsLeader(player))
                return;
            if (FamilyManager.CheckFamilyName(name))
            {
                Trigger.ClientEvent(player, "family:errorName");
                return;
            }
            MySQL.Query("UPDATE `families` SET `f_name` = @prop1 WHERE `f_id` = @prop0", family.Id, name); //changed after restart
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Fam_29", 3000);
        }

        [RemoteEvent("family:tryChangeBiography")]
        public static void RemoteEvent_SetFamilyBiography(Player player, string biography)
        {
            if (!player.IsLogged())
                return;
            Family family = player.GetFamily();
            if (family == null)
                return;
            if (!family.IsLeader(player))
                return;
            family.ChangeBiography(biography);
        }

        [RemoteEvent("family:tryChangeNation")]
        public static void RemoteEvent_SetFamilyNation(Player player, string nation)
        {
            if (!player.IsLogged())
                return;
            Family family = player.GetFamily();
            if (family == null)
                return;
            if (!family.IsLeader(player))
                return;
            family.ChangeNation(nation);
        }

        [RemoteEvent("family:tryChangeRules")]
        public static void RemoteEvent_TryChangeRules(Player player, string taboo, string rules)
        {
            if (!player.IsLogged())
                return;
            Family family = player.GetFamily();
            if (family == null)
                return;
            if (!family.IsLeader(player))
                return;
            family.ChangeRules(JsonConvert.DeserializeObject<List<string>>(taboo), JsonConvert.DeserializeObject<List<string>>(rules));
        }

        [RemoteEvent("family:trySetChatOptions")]
        public static void RemoteEvent_SetChatOptions(Player player, int icon, string color)
        {
            if (!player.IsLogged())
                return;
            Family family = player.GetFamily();
            if (family == null)
                return;
            if (!family.IsLeader(player))
                return;
            family.ChatColor = color;
            family.ChatIcon = icon;
            family.PreSave();
            SubscribeSystem.TriggerEventToSubscribe(family.Id, "family:updateChat", family.ChatIcon, family.ChatColor);
        }
        #endregion

        #region Rank
        [RemoteEvent("family:tryCreateRank")]
        public static void RemoteEvent_CreateRank(Player player)
        {
            if (!player.IsLogged())
                return;
            Family family = player.GetFamily();
            if (family == null)
                return;
            if (!family.IsLeader(player))
                return;
            var rank = family.CreateRank();
            if (rank != null)
            {
                family.PreSave();
                SubscribeSystem.TriggerEventToSubscribe(family.Id, "family:updateRank", rank.GetJsonDTO());
            }
        }

        [RemoteEvent("family:tryDeleteRank")]
        public static void RemoteEvent_DeleteRank(Player player, int rankId)
        {
            if (!player.IsLogged())
                return;
            Family family = player.GetFamily();
            if (family == null)
                return;
            if (!family.IsLeader(player))
                return;
            if (family.DeleteRank(rankId))
            {
                family.PreSave();
                SubscribeSystem.TriggerEventToSubscribe(family.Id, "family:deleteRank", rankId);
            }
        }

        [RemoteEvent("family:trySetRank")]
        public static void RemoteEvent_SaveRank(Player player, int rankId, string rankName, int accessHouse, int accessFurniture, bool accessClothes, bool accessWar, bool accessMemberManagement, string accessCars)
        {
            if (!player.IsLogged())
                return;
            Family family = player.GetFamily();
            if (family == null)
                return;
            if (!family.IsLeader(player))
                return;
            FamilyRank rank;
            if (rankId < 2)
                rank = family.UpdateRankName(rankId, rankName);
            else
                rank = family.UpdateRank(rankId, rankName, accessHouse, accessFurniture, accessClothes, accessWar, accessMemberManagement, accessCars);
            if (rank != null)
            {
                family.PreSave();
                SubscribeSystem.TriggerEventToSubscribe(family.Id, "family:updateRank", rank.GetJsonDTO());
            }
        }
        #endregion

        #region Member Management
        [RemoteEvent("family:trySetCurrentRank")]
        public static void RemoteEvent_TrySetCurrentRank(Player player, int memberUuid, int rankId)
        {
            if (!player.IsLogged())
                return;
            Family family = player.GetFamily();
            if (family == null)
                return;
            if (!family.CanAccessToMemberManagement(player))
                return;
            if (rankId == 0 || family.IsLeader(memberUuid))
            {
                if (family.IsOwner(memberUuid) || !family.IsOwner(player)) //not change for owner | only owner
                    return;
            }
            family.Members.GetValueOrDefault(memberUuid)?.ChangeRank(family, rankId);
        }

        [RemoteEvent("family:tryKickMember")]
        public static void RemoteEvent_TryKickMember(Player player, int memberUuid)
        {
            if (!player.IsLogged())
                return;
            Family family = player.GetFamily();
            if (family == null)
                return;
            if (!family.CanAccessToMemberManagement(player))
                return;
            if (family.IsLeader(memberUuid))
            {
                if (family.IsOwner(memberUuid) || !family.IsOwner(player))
                    return;
            }
            family.DeleteMember(memberUuid);
        }
        [RemoteEvent("family:leaveFromOrganization")]
        public static void RemoteEvent_LeaveFromOrganization(Player player)
        {
            if (!player.IsLogged())
                return;
            player.GetFamily()?.DeleteMember(player.GetCharacter().UUID);
        }

        #endregion

        #region Vehicle Management
        [RemoteEvent("family:repairCar")]
        public void RepairCar(Player player, int vehId)
        {
            if (!player.IsLogged()) return;
            if (!VehicleManager.Vehicles.ContainsKey(vehId)) return;
            var vData = VehicleManager.Vehicles[vehId];
            if (!vData.IsDeath)
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "House_91", 3000);
                return;
            }
            if (!MoneySystem.Wallet.MoneySub(player.GetCharacter(), 100, "Money_RecoveryCar"))
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "House_92", 3000);
                return;
            }
            vData.IsDeath = false;
            GarageManager.SendVehicleIntoGarage(vData);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "House_96", 3000);
        }

        [RemoteEvent("family:evacCar")]
        public void SendVehToGarage(Player player, int vehId)
        {
            if (!player.IsLogged()) return;
            if (!VehicleManager.Vehicles.ContainsKey(vehId)) return;
            var vData = VehicleManager.Vehicles[vehId];
            if (vData.IsDeath)
                return;
            GarageManager.SendVehicleIntoGarage(vData);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "House_96", 3000);
        }

        [RemoteEvent("family:sellCar")]
        public void SellVeh(Player player, int vehId)
        {
            VehicleOperations.SellVeh(player, vehId);
        }

        [RemoteEvent("family:transferCarToMe")]
        public void TransferFamily(Player player, int vehId)
        {

            if (!VehicleManager.Vehicles.ContainsKey(vehId)) return;
            var vData = VehicleManager.Vehicles[vehId] as PersonalBaseVehicle;
            if (vData.PropertyBuyStatus != PropBuyStatus.Bought)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_146", 3000);
                return;
            }
            if (!vData.CanAccessVehicle(player, AccessType.SellDollars))
                return;


            var vehicles = VehicleManager.getAllHolderVehicles(player.GetCharacter().UUID, OwnerType.Personal);
            if (vehicles.Count >= 50)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_141", 3000);
                return;
            }
            DialogUI.Open(player, "House_140".Translate( vData.ModelName, vData.Number), new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "dialog_0",
                    Icon = null,
                    Action = (p) =>
                    {
                        vData.DestroyVehicle();
                        PersonalVehicle vehDataNew = new PersonalVehicle(player.GetCharacter().UUID, vData);
                        VehicleManager.Vehicles[vehId] = vehDataNew;
                        vehDataNew.Save();
                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "House_139", 3000);
                        GarageManager.SendVehicleIntoGarage(vehDataNew);
                        MainMenu.SendProperty(player);
                    }
                },
                new DialogUI.ButtonSetting
                {
                    Name = "dialog_1",
                    Icon = null,
                    Action = { }
                }
            });
        }
        #endregion

        #region Battles

        [RemoteEvent("family:createBattle")]
        public static void CreateBattle(Player player, int place, int bizId, int weapon, string date, string time, string comment)
        {
            if (!player.IsLogged())
                return;
            Family family = player.GetFamily();
            if (family == null)
                return;
            if (!family.CanAccessToBizWar(player))
                return;
            if (!BusinessManager.BizList.ContainsKey(bizId))
                return;
            Business biz = BusinessManager.BizList[bizId];
            Family familyDef = FamilyManager.GetFamily(biz.FamilyPatronage);
            if (familyDef == null || familyDef.Id == 1)
                return;
            if (place < 0 || !Enum.IsDefined(typeof(BattleLocation), place))
                return;
            DateTime dateTime = Convert.ToDateTime($"{date} {time}");
            var result = WarManager.CreateBattle(bizId, familyDef.Id, family.Id, (BattleLocation)place, weapon, dateTime, comment);
            player.TriggerEvent("family:createBattleResponse", "createBattle_"+result.ToString(), result == CreateBattleResult.Success);
        }
        
        [RemoteEvent("family:acceptBattle")]
        public static void AcceptBattle(Player player, int battleId, bool accepted)
        {
            if (!player.IsLogged())
                return;
            Family family = player.GetFamily();
            if (family == null)
                return;
            if (!family.CanAccessToBizWar(player))
                return;
            var battle = WarManager.GetBattle(battleId);
            if (battle == null)
                return;
            if (battle.FamilyDef != family.Id)
                return;
            battle.ConfirmBattle(accepted);
        }
        #endregion

        #region MP


        #endregion

        [RemoteEvent("family:setOnGps")]
        public static void GiveBizGPS(Player player, int bizId)
        {
            if (!player.IsLogged()) return;
            var biz = BusinessManager.BizList.GetOrDefault(bizId);
            if (biz != null)
            {
                Trigger.ClientEvent(player, "createWaypoint", biz.EnterPoint.X, biz.EnterPoint.Y);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Fam_31".Translate(biz.ID), 3000);
            }
        }

        [RemoteEvent("family:deleteOrganization")]
        public static void RemoteEvent_DeleteOrganization(Player player)
        {
            if (!player.IsLogged())
                return;
            Family family = player.GetFamily();
            if (family == null)
                return;
            if (!family.IsOwner(player))
                return;
            FamilyManager.DeleteFamily(player, family);
        }

        public static void UpdateBattleInfo(BattleModel battle)
        {
            SubscribeSystem.TriggerEventToSubscribe(battle.FamilyAttack, "family:updateBattleData", JsonConvert.SerializeObject(battle.GetBattleDTO(battle.FamilyAttack)));
            SubscribeSystem.TriggerEventToSubscribe(battle.FamilyDef, "family:updateBattleData", JsonConvert.SerializeObject(battle.GetBattleDTO(battle.FamilyDef)));
        }


        public static void UpdateVehicleToFamily(Family family, PersonalBaseVehicle vehData)
        {
            var car = new { model = vehData.DisplayName, number = vehData.Number, key = vehData.ID };
            SubscribeSystem.TriggerEventToSubscribe(family.Id, "family:updateVehicle", JsonConvert.SerializeObject(car));
        }
        public static void RemoveVehicleFromFamily(Family family, int vehId)
        {
            SubscribeSystem.TriggerEventToSubscribe(family.Id, "family:removeVehicle", vehId);
        }

        public static void UpdateMemberToFamily(Family family, int uuid, bool online)
        {
            if (!family.Members.ContainsKey(uuid))
                return;
            var playerData = family.Members[uuid].GetFamilyMemberDTO(online);
            SubscribeSystem.TriggerEventToSubscribe(family.Id, "family:updateMember", JsonConvert.SerializeObject(playerData));
        }
        public static void RemoveMemberFromFamily(Family family, int uuid)
        {
            SubscribeSystem.TriggerEventToSubscribe(family.Id, "family:removeMember", uuid);
        }

        public static void UpdateBusinessToFamily(Family family, Business biz)
        {
            SubscribeSystem.TriggerEventToSubscribe(family.Id, "family:updateBusiness", JsonConvert.SerializeObject(biz.GetFamilyData()));
            UpdateFamilyRatingData(family);
        }
        public static void RemoveBusinessFromFamily(Family family, int bizId)
        {
            SubscribeSystem.TriggerEventToSubscribe(family.Id, "family:removeBusiness", bizId);
            UpdateFamilyRatingData(family);
        }
        public static void UpdateBusinessFamilyPatronage(Business biz, int familyId)
        {
            SubscribeSystem.TriggerEventToSubscribeAllFamily("family:updateBusinessFamPatronage", biz.ID, FamilyManager.GetFamilyName(familyId));
        }
        public static void UpdateFamilyRatingData(Family family)
        {
            SubscribeSystem.TriggerEventToSubscribeAllFamily("family:updateFamilyRating", family.Id, family.Name, family.OwnerName, family.CountBattles, family.CountBusiness, family.Members.Count, family.GlobalRating, family.Rating);
        }
        public static void UpdateFamilyMP(FamilyMPModel familyMPModel)
        {
            SubscribeSystem.TriggerEventToSubscribeAllFamily("family:updateFamilyMP", JsonConvert.SerializeObject(new FamilyMPModelDTO(familyMPModel)));
        }
        public static void UpdateContract(Family family, string contractDTO)
        {
            SubscribeSystem.TriggerEventToSubscribe(family.Id, "personalEvents:updateFamilyContracts", contractDTO);
        }


    }
}