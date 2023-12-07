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
        public static event Action<ExtPlayer, Family> FamilyLoad;
        public static void LoadMenuData(ExtPlayer player, Family fam)
        {
            var house = HouseManager.GetHouse(fam.Id, OwnerType.Family);
            var data = new
            {
                familyId = fam.Id,
                leaders = fam.Members.Where(memb => memb.Value.Rank == 0).Select(memb => new { id = memb.Value.PlayerUUID, name = Main.PlayerNames.GetOrDefault(memb.Value.PlayerUUID) }),
                nation = fam.Nation,
                houseType = house?.TypeHouse?.Name ?? string.Empty,
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

            SafeTrigger.ClientEvent(player, "family:loadData", JsonConvert.SerializeObject(data), player.Character.FamilyLVL, fam.ChatIcon, fam.ChatColor, JsonConvert.SerializeObject(members), fam.Name, JsonConvert.SerializeObject(bizz), JsonConvert.SerializeObject(allBizz), player.Character.UUID);
            SafeTrigger.ClientEvent(player, "family:loadCarsAndRanks", JsonConvert.SerializeObject(cars), JsonConvert.SerializeObject(ranks));
            SafeTrigger.ClientEvent(player, "family:loadFamilyRatings", JsonConvert.SerializeObject(familyRating));
            SafeTrigger.ClientEvent(player, "family:loadFamilyBattles", JsonConvert.SerializeObject(battles));
            FamilyLoad?.Invoke(player, fam);
        }
        
        public static void UnloadMenuData(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player, "family:unloadData");
        }

        [RemoteEvent("family:openFamilyMenu")]
        public static void OpenFamilyMenu(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            Family family = player.GetFamily();
            if (family == null)
                return;
            SafeTrigger.ClientEvent(player, "family:openMenu", family.Money.ToString());
        }

        #region Money

        [RemoteEvent("family:takeMoney")]
        public static bool RemoteEvent_tryTakeMoney(ExtPlayer player, int amount)
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
            if (player.Character.AdminLVL < 8 && amount + family.MoneyLimit > FamilyConstants.MAX_MONEY_SUB_IN_DAY)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Лимит снятия со счета семьи ${FamilyConstants.MAX_MONEY_SUB_IN_DAY} в сутки, осталось сегодня: ${FamilyConstants.MAX_MONEY_SUB_IN_DAY - family.MoneyLimit}", 3000);
                return false;
            }
            if (Wallet.TransferMoney(family, player.Character.BankModel, amount, 0, "Перевод со счета семьи", player.Character.AdminLVL < 8))
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы сняли со счета семьи ${amount}", 3000);
                Chat.SendToAdmins(5, $"{player.Name} из семьи {family.Name} снял со счета ${amount}");
                Chat.CMD_FamilyRadio(player, $"Снял со счета ${amount}");
                if (amount >= 1000000)
                    FamilyManager.ChangePoints(player, FamilyActions.SubMoney);
                return true;
            }
            else
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно средств для выполнения операции", 3000);
            }
            SafeTrigger.ClientEvent(player, "family:updateMoney", family.Money.ToString());
            return false;
        }

        [RemoteEvent("family:putMoney")]
        public static bool RemoteEvent_tryPutMoney(ExtPlayer player, int amount)
        {
            if (!player.IsLogged())
                return false;
            Family family = player.GetFamily();
            if (amount < 0)
                return false;
            if (family == null)
                return false;
            if (Wallet.TransferMoney(player.Character.BankModel, family, amount, 0, "Перевод на счет семьи"))
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы положили на счет семьи ${amount}", 3000);
                Chat.SendToAdmins(5, $"{player.Name} из семьи {family.Name} положил на счет ${amount}");
                Chat.CMD_FamilyRadio(player, $"Положил на счет ${amount}");
                if (amount >= 1000000)
                    FamilyManager.ChangePoints(player, FamilyActions.AddMoney);
                return true;
            }
            else
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно средств для выполнения операции", 3000);
            }            
            SafeTrigger.ClientEvent(player, "family:updateMoney", family.Money.ToString());
            return false;
        }
        #endregion

        #region Change Family Info
        [RemoteEvent("family:tryChangeName")]
        public static void RemoteEvent_tryChangeName(ExtPlayer player, string name)
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
                SafeTrigger.ClientEvent(player, "family:errorName");
                return;
            }
            MySQL.Query("UPDATE `families` SET `f_name` = @prop1 WHERE `f_id` = @prop0", family.Id, name); //changed after restart
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Вы успешно сменили название семьи. Изменение вступит в силу после рестарта сервера", 3000);
        }

        [RemoteEvent("family:tryChangeBiography")]
        public static void RemoteEvent_SetFamilyBiography(ExtPlayer player, string biography)
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
        public static void RemoteEvent_SetFamilyNation(ExtPlayer player, string nation)
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
        public static void RemoteEvent_TryChangeRules(ExtPlayer player, string taboo, string rules)
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
        public static void RemoteEvent_SetChatOptions(ExtPlayer player, int icon, string color)
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
        public static void RemoteEvent_CreateRank(ExtPlayer player)
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
        public static void RemoteEvent_DeleteRank(ExtPlayer player, int rankId)
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
        public static void RemoteEvent_SaveRank(ExtPlayer player, int rankId, string rankName, int accessHouse, int accessFurniture, bool accessClothes, bool accessWar, bool accessMemberManagement, string accessCars)
        {
            if (!player.IsLogged()) return;

            Family family = player.GetFamily();
            if (family == null)return;
            if (!family.IsLeader(player)) return;

            FamilyRank rank;
            if (rankId < 1) rank = family.UpdateRankName(rankId, rankName);
            else rank = family.UpdateRank(rankId, rankName, accessHouse, accessFurniture, accessClothes, accessWar, accessMemberManagement, accessCars);
            if (rank == null) return;

            family.PreSave();
            SubscribeSystem.TriggerEventToSubscribe(family.Id, "family:updateRank", rank.GetJsonDTO());
        }
        #endregion

        #region Member Management
        [RemoteEvent("family:trySetCurrentRank")]
        public static void RemoteEvent_TrySetCurrentRank(ExtPlayer player, int memberUuid, int rankId)
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
        public static void RemoteEvent_TryKickMember(ExtPlayer player, int memberUuid)
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
        public static void RemoteEvent_LeaveFromOrganization(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            player.GetFamily()?.DeleteMember(player.Character.UUID);
        }

        #endregion

        #region Vehicle Management
        [RemoteEvent("family:repairCar")]
        public void RepairCar(ExtPlayer player, int vehId)
        {
            if (!player.IsLogged()) return;
            if (!VehicleManager.Vehicles.ContainsKey(vehId)) return;
            var vData = VehicleManager.Vehicles[vehId];
            if (!vData.IsDeath)
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Машину не нужно восстанавливать", 3000);
                return;
            }
            if (!MoneySystem.Wallet.MoneySub(player.Character, 100, "Восстановление авто"))
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "У вас не достаточно средств", 3000);
                return;
            }
            vData.IsDeath = false;
            GarageManager.SendVehicleIntoGarage(vData);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Ваша машина появилась в гараже", 3000);
        }

        [RemoteEvent("family:evacCar")]
        public void SendVehToGarage(ExtPlayer player, int vehId)
        {
            if (!player.IsLogged()) return;
            if (!VehicleManager.Vehicles.ContainsKey(vehId)) return;
            var vData = VehicleManager.Vehicles[vehId];
            if (vData.IsDeath)
                return;
            GarageManager.SendVehicleIntoGarage(vData);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Ваша машина появилась в гараже", 3000);
        }

        [RemoteEvent("family:sellCar")]
        public void SellVeh(ExtPlayer player, int vehId)
        {
            VehicleOperations.SellVeh(player, vehId);
        }

        [RemoteEvent("family:transferCarToMe")]
        public void TransferFamily(ExtPlayer player, int vehId)
        {

            if (!VehicleManager.Vehicles.ContainsKey(vehId)) return;
            var vData = VehicleManager.Vehicles[vehId] as PersonalBaseVehicle;
            if (vData.PropertyBuyStatus != PropBuyStatus.Bought)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Этот автомоболь нельзя передать", 3000);
                return;
            }
            if (!vData.CanAccessVehicle(player, AccessType.SellDollars))
                return;


            var vehicles = VehicleManager.getAllHolderVehicles(player.Character.UUID, OwnerType.Personal);
            if (vehicles.Count >= 50)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У Вас максимальное количество машин!", 3000);
                return;
            }
            DialogUI.Open(player, $"Вы действительно хотите забрать машину {vData.ModelName} - {vData.Number} из семьи?", new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "Подтвердить",
                    Icon = null,
                    Action = (p) =>
                    {
                        vData.DestroyVehicle();
                        PersonalVehicle vehDataNew = new PersonalVehicle(player.Character.UUID, vData);
                        VehicleManager.Vehicles[vehId] = vehDataNew;
                        vehDataNew.Save();
                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Машина успешно изъята из семьи", 3000);
                        GarageManager.SendVehicleIntoGarage(vehDataNew);
                        MainMenu.SendProperty(player);
                    }
                },
                new DialogUI.ButtonSetting
                {
                    Name = "Отменить",
                    Icon = null,
                    Action = { }
                }
            });
        }
        #endregion

        #region Battles

        [RemoteEvent("family:createBattle")]
        public static void CreateBattle(ExtPlayer player, int place, int bizId, int weapon, string date, string time, string comment)
        {
            if (!player.IsLogged()) return;

            Family family = player.GetFamily();
            if (family == null) return;

            if (!family.CanAccessToBizWar(player))
            {
                Notify.SendError(player, "У Вас нет доступа для объявления войны.");
                return;
            }

            if (place < 0 || !Enum.IsDefined(typeof(BattleLocation), place))
            {
                Notify.SendError(player, "Вы не выбрали место битвы.");
                return;
            }

            if (!BusinessManager.BizList.ContainsKey(bizId))
            {
                Notify.SendError(player, "Произошла ошибка #9252, пожалуйста, обратитесь к администрации проекта.");
                return;
            }

            Business biz = BusinessManager.BizList[bizId];
            if (biz == null) return;

            Family familyDef = FamilyManager.GetFamily(biz.FamilyPatronage);
            if (familyDef == null || familyDef.Id == 1)
            {
                Notify.SendError(player, "Нельзя объявить войну, так как бизнес никто не крышует.");
                return;
            }

            DateTime dateTime = Convert.ToDateTime($"{date} {time}");
            var result = WarManager.CreateBattle(bizId, familyDef.Id, family.Id, (BattleLocation)place, weapon, dateTime, comment);
            SafeTrigger.ClientEvent(player,"family:createBattleResponse", "createBattle_"+result.ToString(), result == CreateBattleResult.Success);
        }
        
        [RemoteEvent("family:acceptBattle")]
        public static void AcceptBattle(ExtPlayer player, int battleId, bool accepted)
        {
            if (!player.IsLogged()) return;

            Family family = player.GetFamily();
            if (family == null) return;
            if (!family.CanAccessToBizWar(player))
            {
                Notify.SendError(player, "У Вас нет доступа для объявления войны.");
                return;
            }

            BattleModel battle = WarManager.GetBattle(battleId);
            if (battle == null) return;
            if (battle.FamilyDef != family.Id)
            {
                Notify.SendError(player, "Вы не можете принять данную битву, так как она не относится к Вашей семье.");
                return;
            }

            battle.ConfirmBattle(accepted);
        }
        #endregion

        #region MP


        #endregion

        [RemoteEvent("family:setOnGps")]
        public static void GiveBizGPS(ExtPlayer player, int bizId)
        {
            if (!player.IsLogged()) return;
            var biz = BusinessManager.BizList.GetOrDefault(bizId);
            if (biz != null)
            {
                SafeTrigger.ClientEvent(player, "createWaypoint", biz.EnterPoint.X, biz.EnterPoint.Y);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы отметили меткой GPS бизнес #{biz.ID}", 3000);
            }
        }

        [RemoteEvent("family:deleteOrganization")]
        public static void RemoteEvent_DeleteOrganization(ExtPlayer player)
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