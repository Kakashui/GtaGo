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
            foreach (var fam in Families)
            {
                fam.Value.ChangeMoneyLimit(0);
            }
        }
        public static void PayFamilyMoneyForBusinessAndEnterprise()
        {
            foreach (var fam in Families)
            {
                var amount = BusinessManager.BizList.Where(item => item.Value.FamilyPatronage == fam.Key).Sum(item => item.Value.GetFamilyTax());
                if (amount > 0)
                    MoneySystem.Wallet.MoneyAdd(fam.Value, amount, "Money_BizwarProfit");
                if (fam.Value.MoneyForEnterprise > 0)
                {
                    Wallet.MoneyAdd(fam.Value, fam.Value.MoneyForEnterprise, "Money_CompanyOwn");
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

        public static void ViewFamilyList(Player player, string name)
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
            try
            {
                foreach (var family in Families.Values)
                {
                    family.Save();
                }
            }
            catch (Exception e) { _logger.WriteError("SaveFamilyCars: " + e.ToString()); }
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

        public static Family GetFamily(int famId)
        {
            return Families.GetValueOrDefault(famId);
        }

        public static Family GetFamilyByUUID(int uuid)
        {
            return Families.FirstOrDefault(item => item.Value.Members.ContainsKey(uuid)).Value;
        }

        public static bool IsLeader(Player player)
        {
            return player.GetFamily()?.IsLeader(player) ?? false;
        }

        public static bool CanAccessToHouse(Player player, int targetFamily, FamilyHouseAccess houseAccess)
        {
            if (!player.IsLogged())
                return false;
            int famID = player.GetCharacter().FamilyID;
            if (targetFamily != famID)
                return false;
            if (!Families.ContainsKey(famID))
                return false;
            return Families[famID].CanAccessToHouse(player, houseAccess);
        }

        public static bool CanAccessToFurniture(Player player, int targetFamily, FamilyFurnitureAccess furnAccess)
        {
            if (!player.IsLogged())
                return false;
            int famID = player.GetCharacter().FamilyID;
            if (targetFamily != famID)
                return false;
            if (!Families.ContainsKey(famID))
                return false;
            return Families[famID].CanAccessToFurniture(player, furnAccess);
        }

        public static bool CanAccessToVehicle(Player player, int targetFamily, int vehicleID, FamilyVehicleAccess vehicleAccess)
        {
            if (!player.IsLogged())
                return false;
            int famID = player.GetCharacter().FamilyID;
            if (targetFamily != famID)
                return false;
            if (!Families.ContainsKey(famID))
                return false;
            return Families[famID].CanAccessToVehicle(player, vehicleID, vehicleAccess);
        }

        public static bool CanAccessToDoor(Player player, string access)
        {
            string fam = access.Replace("family", "");
            if (int.TryParse(fam, out int familyId))
            {
                return CanAccessToHouse(player, familyId, FamilyHouseAccess.OpenDoors);
            }
            return false;
        }

        public static bool CanAccessToBizWar(Player player)
        {
            if (!player.IsLogged())
                return false;
            int famID = player.GetCharacter().FamilyID;
            if (!Families.ContainsKey(famID))
                return false;
            return Families[famID].CanAccessToBizWar(player);
        }

        public static bool CanAccessToMemberManagement(Player player)
        {
            if (!player.IsLogged())
                return false;
            int famID = player.GetCharacter().FamilyID;
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

        public static List<Player> GetFamilyMembers(int familyID)
        {
            if (Families.ContainsKey(familyID))
                return Families[familyID].OnlineMembers.Values.Where(player => player.IsLogged()).ToList();
            else
                return new List<Player>();
        }

        public static string GetFamilyName(int familyID)
        {
            if (Families.ContainsKey(familyID))
                return Families[familyID].Name;
            else
                return "None";
        }


        public static void PlayerLoadFamily(PlayerGo player)
        {
            if (!player.IsLogged())
                return;
            int famID = player.GetCharacter().FamilyID;
            if (Families.ContainsKey(famID))
            {
                Families[famID].ConnectedMember(player.GetCharacter().UUID, player);
                FamilyMenuManager.UpdateMemberToFamily(Families[famID], player.GetCharacter().UUID, true);
                FamilyMenuManager.LoadMenuData(player, Families[famID]);
                SubscribeSystem.SubscribeMember(player, famID);
                player.SetSharedData("familyuuid", famID);
                player.SetSharedData("familyname", Families[famID].Name);
                Families[famID].GetHouse()?.CreateFamilyMemberMarker(player);
            }
            else
            {
                player.SetSharedData("familyuuid", 0);
                player.SetSharedData("familyname", "-");
            }
        }
        public static void PlayerUnloadFamily(Player player, Character character)
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

        public static void InvitePlayerToFamily(PlayerGo player, Family family, int rank = 1)
        {
            if (!player.IsLogged())
                return;
            family.NewMember(player.GetCharacter().UUID, rank);
            player.SendTip("tip_family_invited");
            player.SendTip("tip_7");
            PlayerLoadFamily(player);
        }


        [Command("createfamily")]
        public void REMOTE_AnswerFamily(PlayerGo player, int id, string familyName)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "createfamily"))
                    return;
                var target = Main.GetPlayerByID(id);
                if (target == null)
                    return;

                CreateFamily(target, familyName, 0, MoneySystem.PaymentsType.Cash);

                if (target.GetCharacter().FamilyID != 0)
                {
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Fam_5".Translate(), 3000);
                    Notify.Send(target, NotifyType.Success, NotifyPosition.BottomCenter, "Fam_6", 3000);
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

        public static bool CreateFamily(PlayerGo player, string familyName, int price, MoneySystem.PaymentsType paymentsType, Action<Player, bool, string> notifyAction = null)
        {
            try
            {
                if (CheckFamilyName(familyName))
                {
                    if (notifyAction == null)
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Fam_7", 3000);
                    else
                        notifyAction(player, false, "Fam_7");
                    return false;
                }

                if (player.GetCharacter().FamilyID != 0)
                {
                    if (notifyAction == null)
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Fam_8", 3000);
                    else
                        notifyAction(player, false, "Fam_8");
                    return false;
                }
                if (price > 0)
                {
                    if (!MoneySystem.Wallet.TransferMoney(player.GetMoneyPayment(paymentsType), Manager.GetFraction(6), price, 0, "Money_FamilyCreate"))
                    {
                        if (notifyAction == null)
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "money:enouth", 3000);
                        else
                            notifyAction(player, false, "money:enouth");
                        return false;
                    }
                }


                Family family = new Family(familyName, player.GetCharacter().UUID);
                Families.Add(family.Id, family);

                player.SendTip("tip_7");
                PlayerLoadFamily(player);
                FamilyMenuManager.UpdateFamilyRatingData(family);
                if (notifyAction == null)
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Fam_9", 3000);
                else
                    notifyAction(player, true, "Fam_9");
                return true;
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT \"CreateFamily\": " + e.ToString());
                return false;
            }
        }

        public static void ChangePoints(Player player, FamilyActions action)
        {
            Family family = player.GetFamily();
            if (family == null)
                return;
            int uuid = player.GetCharacter().UUID;
            if (!family.Members.ContainsKey(uuid))
                return;
            family.Members[uuid].ChangePoints(family, action);
        }

        public static void DeleteFamily(Player player, Family family)
        {
            if (family.Members.Count > 1)
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Fam_35", 3000);
                return;
            }
            if (VehicleManager.getAllHolderVehicles(family.Id, OwnerType.Family).Count > 0)
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Fam_36", 3000);
                return;
            }
            if (HouseManager.GetHouse(family.Id, OwnerType.Family) != null)
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Fam_37", 3000);
                return;
            }
            foreach (var biz in BusinessManager.BizList.Values.Where(item => item.FamilyPatronage == family.Id))
            {
                biz.SetPatronageFamily(-1);
            }
            family.DeleteMember(family.Owner);
            MySQL.Query("DELETE FROM `families` WHERE `f_id` = @prop0", family.Id);
            Families.Remove(family.Id);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Fam_38", 3000);
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
