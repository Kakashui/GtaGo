using Whistler.Core;
using Whistler.Fractions;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using Whistler.Helpers;
using Whistler.Families.Models;
using Whistler.Families.FamilyMenu;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models.VehiclesData;
using AutoMapper.Internal;
using GTANetworkAPI;
using System.Data;
using Whistler.Families.FamilyWars;
using Whistler.Houses;
using Whistler.Core.Character;
using Whistler.Common;
using Whistler.MoneySystem;
using Whistler.Entities;
using Whistler.GUI;
using Whistler.Inventory.Models;
using Whistler.MoneySystem.Models;

namespace Whistler.Families
{
    class FamilyManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(FamilyManager));

        private static Dictionary<int, Family> Families = new Dictionary<int, Family>();
        public FamilyManager()
        {
            VehicleManager.NewVehicleToHolder += (PersonalBaseVehicle vehData) => ChangeVehicleHolder(vehData, true);
            VehicleManager.RemoveVehicleFromHolder += (PersonalBaseVehicle vehData) => ChangeVehicleHolder(vehData, false);
            Main.Payday += UpdateRatings;
        }

        public static void ClearPoints()
        {
            foreach (var family in Families)
            {
                family.Value.ClearPoints();
            }
        }

        public static List<int> GetFamiliesKeys()
        {
            return Families.Keys.ToList();
        }

        public static void LoadFamilies()
        {

            var result = MySQL.QueryRead("SELECT * FROM `families`");

            if (result == null)
            {
                _logger.WriteError("DB `families` return null result");
            }
            else
            {
                foreach (DataRow Row in result.Rows)
                {
                    Family family = new Family(Row);
                    Families.Add(family.Id, family);
                }
            }
            UpdateRatings();
            WarManager.LoadBattles();
        }

        public static void SetAllFamiliesMoneyLimit()
        {
            if (Families == null) return;
            if (!Families.Any()) return;

            foreach (Family family in Families.Values)
            {
                if (family == null) continue;

                family.ChangeMoneyLimit(0);
            }
        }
        public static void PayFamilyMoneyForBusinessAndEnterprise()
        {
            foreach (var fam in Families)
            {
                var amount = BusinessManager.BizList.Where(item => item.Value.FamilyPatronage == fam.Key).Sum(item => item.Value.GetFamilyTax());
                if (amount > 0)
                    MoneySystem.Wallet.MoneyAdd(fam.Value, amount, "Прибыль за захват бизнеса");
                if (fam.Value.MoneyForEnterprise > 0)
                {
                    Wallet.MoneyAdd(fam.Value, fam.Value.MoneyForEnterprise, "Захват предприятий");
                    fam.Value.MoneyForEnterprise = 0;
                }
            }
        }

        public static void UpdateRatings()
        {
            CompFamilyByRating<Family> sortFamily = new CompFamilyByRating<Family>();
            List<Family> families = Families.Values.ToList();
            families.Sort(sortFamily);
            int maxRating = families.Count;
            int i = 1;
            foreach (var family in families)
            {
                if (family.Members.Count > 10)
                    family.Rating = i++;
                else
                    family.Rating = maxRating;
                FamilyMenuManager.UpdateFamilyRatingData(family);
            }
        }

        public static void ViewFamilyList(ExtPlayer player, string name)
        {
            if (Families.Count == 0)
            {
                Chat.SendTo(player, "No families were found in database!");
            }
            else
            {
                foreach (var family in Families)
                {
                    if (family.Value.Name.ToLower().Contains(name) || family.Value.OwnerName.ToLower().Contains(name) || family.Key.ToString() == name || name == "")
                        Chat.SendTo(player, $"#{family.Key} | {family.Value.Name} | Owner: { family.Value.OwnerName} | Members: {family.Value.OnlineMembers.Count}/{family.Value.Members.Count} | Type: {family.Value.OrgActiveType}");
                }
            }
        }

        public static List<FamilyEloParams> GetFamilyRatings()
        {
            return Families.Values.Select(item => item.EloParams).ToList();
        }

        public static void SavingFamilies()
        {
            if (!Families.Any()) return;

            foreach (Family family in Families.Values)
            {
                if (family == null) continue;

                family.Save();
            }
        }


        private static void ChangeVehicleHolder(PersonalBaseVehicle vehData, bool addCar)
        {
            if (vehData.OwnerType != OwnerType.Family)
                return;
            Family fam = GetFamily(vehData.OwnerID);
            if (fam == null)
                return;
            if (addCar)
                fam.AddVehicle(vehData);
            else
                fam.RemoveVehicle(vehData);
        }

        public static Family GetFamily(ExtPlayer player)
        {
            if (!player.IsLogged()) return null;
            return GetFamily(player.Character.FamilyID);
        }

        public static Family GetFamily(int famId)
        {
            if (famId <= 0) return null;
            if (Families.ContainsKey(famId)) return Families[famId];
            return null;
        }

        public static Family GetFamilyByUUID(int uuid)
        {
            return Families.FirstOrDefault(item => item.Value.Members.ContainsKey(uuid)).Value;
        }

        public static bool IsLeader(ExtPlayer player)
        {
            return player.GetFamily()?.IsLeader(player) ?? false;
        }

        public static bool CanAccessToHouse(ExtPlayer player, int targetFamily, FamilyHouseAccess houseAccess)
        {
            if (!player.IsLogged())
                return false;
            int famID = player.Character.FamilyID;
            if (targetFamily != famID)
                return false;
            if (!Families.ContainsKey(famID))
                return false;
            return Families[famID].CanAccessToHouse(player, houseAccess);
        }

        public static bool CanAccessToFurniture(ExtPlayer player, int targetFamily, FamilyFurnitureAccess furnAccess)
        {
            if (!player.IsLogged())
                return false;
            int famID = player.Character.FamilyID;
            if (targetFamily != famID)
                return false;
            if (!Families.ContainsKey(famID))
                return false;
            return Families[famID].CanAccessToFurniture(player, furnAccess);
        }

        public static bool CanAccessToVehicle(ExtPlayer player, int targetFamily, int vehicleID, FamilyVehicleAccess vehicleAccess)
        {
            if (!player.IsLogged())
                return false;
            int famID = player.Character.FamilyID;
            if (targetFamily != famID)
                return false;
            if (!Families.ContainsKey(famID))
                return false;
            return Families[famID].CanAccessToVehicle(player, vehicleID, vehicleAccess);
        }

        public static bool CanAccessToDoor(ExtPlayer player, string access)
        {
            string fam = access.Replace("family", "");
            if (int.TryParse(fam, out int familyId))
            {
                return CanAccessToHouse(player, familyId, FamilyHouseAccess.OpenDoors);
            }
            return false;
        }

        public static bool CanAccessToBizWar(ExtPlayer player)
        {
            if (!player.IsLogged())
                return false;
            int famID = player.Character.FamilyID;
            if (!Families.ContainsKey(famID))
                return false;
            return Families[famID].CanAccessToBizWar(player);
        }

        public static bool CanAccessToMemberManagement(ExtPlayer player)
        {
            if (!player.IsLogged())
                return false;
            int famID = player.Character.FamilyID;
            if (!Families.ContainsKey(famID))
                return false;
            return Families[famID].CanAccessToMemberManagement(player);
        }

        public static int CountFamilyMemberOnline(int familyID)
        {
            if (Families.ContainsKey(familyID))
                return Families[familyID].OnlineMembers.Values.Where(player => player.IsLogged()).Count();
            else
                return 0;
        }

        public static List<ExtPlayer> GetFamilyMembers(int familyID)
        {
            if (Families.ContainsKey(familyID))
                return Families[familyID].OnlineMembers.Values.Where(player => player.IsLogged()).ToList();
            else
                return new List<ExtPlayer>();
        }

        public static string GetFamilyName(int familyID)
        {
            if (Families.ContainsKey(familyID))
                return Families[familyID].Name;
            else
                return "None";
        }


        public static void PlayerLoadFamily(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            int famID = player.Character.FamilyID;
            if (Families.ContainsKey(famID))
            {
                Families[famID].ConnectedMember(player.Character.UUID, player);
                FamilyMenuManager.UpdateMemberToFamily(Families[famID], player.Character.UUID, true);
                FamilyMenuManager.LoadMenuData(player, Families[famID]);
                SubscribeSystem.SubscribeMember(player, famID);
                SafeTrigger.SetSharedData(player, "familyuuid", famID);
                SafeTrigger.SetSharedData(player, "familyname", Families[famID].Name);
                Families[famID].GetHouse()?.CreateFamilyMemberMarker(player);
            }
            else
            {
                SafeTrigger.SetSharedData(player, "familyuuid", 0);
                SafeTrigger.SetSharedData(player, "familyname", "-");
            }
        }
        public static void PlayerUnloadFamily(ExtPlayer player, Character character)
        {
            try
            {
                int famID = character.FamilyID;
                if (Families.ContainsKey(famID))
                {
                    Families[famID].DisconnectedMember(character.UUID);
                    SubscribeSystem.UnsubscribeMember(player, famID);
                    FamilyMenuManager.UpdateMemberToFamily(Families[famID], character.UUID, false);
                    Families[famID].GetHouse()?.DeleteFamilyMemberMarker(player);
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Event_OnPlayerDisconnected 2:\n{e}");
            }
        }

        public static void InvitePlayerToFamily(ExtPlayer player, Family family, int rank = 1)
        {
            if (!player.IsLogged()) return;
            family.NewMember(player.Character.UUID, rank);
            MainMenu.SendStats(player);
            player.SendTip("Вас приняли в организацию. Нажмите K чтобы открыть меню вашей семьи");
            PlayerLoadFamily(player);
        }


        [Command("createfamily")]
        public void REMOTE_AnswerFamily(ExtPlayer player, int id, string familyName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "createfamily"))
                    return;
                var target = Trigger.GetPlayerByUuid(id);
                if (target == null)
                    return;

                CreateFamily(target, familyName, 0, MoneySystem.PaymentsType.Cash);

                if (target.Character.FamilyID != 0)
                {
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Игрок успешно оплатил создание семьи", 3000);
                    Notify.Send(target, NotifyType.Success, NotifyPosition.BottomCenter, "Вы успешно оплатили создание семьи", 3000);
                }
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT \"REMOTE_FamilyBuy\": " + e.ToString());
            }
        }

        public static bool CheckFamilyName(string familyName)
        {
            return Families.Any(d => d.Value.Name.ToLower() == familyName.ToLower());
        }

        public static bool CreateFamily(ExtPlayer player, string familyName, int price, MoneySystem.PaymentsType paymentsType, Action<ExtPlayer, bool, string> notifyAction = null)
        {
            try
            {
                if (CheckFamilyName(familyName))
                {
                    if (notifyAction == null)
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Семья с таким именем уже существует", 3000);
                    else
                        notifyAction(player, false, "Семья с таким именем уже существует");
                    return false;
                }

                if (player.Character.FamilyID != 0)
                {
                    if (notifyAction == null)
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы должны выйти из семьи, чтобы создать свою", 3000);
                    else
                        notifyAction(player, false, "Вы должны выйти из семьи, чтобы создать свою");
                    return false;
                }
                if (price > 0)
                {
                    if (!MoneySystem.Wallet.TransferMoney(player.GetMoneyPayment(paymentsType), MoneyManager.ServerMoney, price, 0, "Создание семьи"))
                    {
                        if (notifyAction == null)
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно денег", 3000);
                        else
                            notifyAction(player, false, "Недостаточно денег");
                        return false;
                    }
                }


                Family family = new Family(familyName, player.Character.UUID);
                Families.Add(family.Id, family);

                player.SendTip("Нажмите K чтобы открыть меню семьи");
                PlayerLoadFamily(player);
                FamilyMenuManager.UpdateFamilyRatingData(family);
                MainMenu.SendStats(player);
                if (notifyAction == null)
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Вы успешно создали семью", 3000);
                else
                    notifyAction(player, true, "Вы успешно создали семью");
                return true;
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT \"CreateFamily\": " + e.ToString());
                return false;
            }
        }

        public static void ChangePoints(ExtPlayer player, FamilyActions action)
        {
            Family family = player.GetFamily();
            if (family == null)
                return;
            int uuid = player.Character.UUID;
            if (!family.Members.ContainsKey(uuid))
                return;
            family.Members[uuid].ChangePoints(family, action);
        }

        public static void DeleteFamily(ExtPlayer player, Family family)
        {
            if (family.Members.Count > 1)
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "В семье не должно быть других участников", 3000);
                return;
            }
            if (VehicleManager.getAllHolderVehicles(family.Id, OwnerType.Family).Count > 0)
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "В семье не должно быть автомобилей", 3000);
                return;
            }
            if (HouseManager.GetHouse(family.Id, OwnerType.Family) != null)
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "У семьи не должно быть дома", 3000);
                return;
            }
            foreach (var biz in BusinessManager.BizList.Values.Where(item => item.FamilyPatronage == family.Id))
            {
                biz.SetPatronageFamily(-1);
            }
            family.DeleteMember(family.Owner);
            MySQL.Query("DELETE FROM `families` WHERE `f_id` = @prop0", family.Id);
            Families.Remove(family.Id);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Семья успешно удалена", 3000);
        }

        public static void NewBusiness(int famId, Business biz)
        {
            Family family = GetFamily(famId);
            if (family == null)
                return;
            FamilyMenuManager.UpdateBusinessToFamily(family, biz);
        }

        public static void RemoveBusiness(int famId, Business biz)
        {
            Family family = GetFamily(famId);
            if (family == null)
                return;
            FamilyMenuManager.RemoveBusinessFromFamily(family, biz.ID);
        }

    }
}
