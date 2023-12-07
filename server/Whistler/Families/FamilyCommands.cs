using GTANetworkAPI;
using Whistler.SDK;
using Whistler.Core.Character;
using Whistler.Core;
using Whistler.Helpers;
using System.Collections.Generic;
using Whistler.Families.Models;
using System;
using System.Linq;
using Whistler.Families.FamilyMenu;
using Whistler.Common;
using Whistler.Entities;
using Whistler.Houses;

namespace Whistler.Families
{
    class FamilyCommands : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(FamilyCommands));

        [Command("families")]
        public static void CMD_Families(Player player, string name = "")
        {
            if (!Group.CanUseAdminCommand(player, "families"))
                return;
            FamilyManager.ViewFamilyList(player, name);
        }
        [Command("famkick")]
        public static void CMD_Families(Player player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "famkick"))
                return;
            Player target = Main.GetPlayerByID(id);
            if (target.IsLogged())
            {
                if (target.GetFamily()?.DeleteMember(target.GetCharacter().UUID) ?? false)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "famkick", 3000);
                    GameLog.Admin(player.Name, "famkick", target.Name);
                }
            }
            else
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_281", 3000);
        }
        [Command("setfamtype", "USAGE: /setfamtype [familyId] [type] Unknown = 0, Neutral = 1, Crime = 2, Government = 3")]
        public static void CMD_SetFamilyType(Player player, int familyId, int type)
        {
            if (!Group.CanUseAdminCommand(player, "setfamtype"))
                return;
            if (!Enum.IsDefined(typeof(OrgActivityType), type))
                return;
            FamilyManager.GetFamily(familyId)?.SetFamilyType((OrgActivityType)type);            
        }
        [Command("psto")]
        public static void CMD_SetFamilyClothesPoint(Player player, int familyId)
        {
            if (!Group.CanUseAdminCommand(player, "psto"))
                return;
            FamilyManager.GetFamily(familyId)?.SetClothesPoint(player.Position - new Vector3(0, 0, 1), player.Dimension);
        }
        [Command("kickallmembers")]
        public static void CMD_DeleteAllFamilyMembers(Player player, int familyId)
        {
            if (!Group.CanUseAdminCommand(player, "delallmembers"))
                return;
            if (familyId == 1)
                return;
            var family = FamilyManager.GetFamily(familyId);
            if (family == null)
                return;
            var members = family.Members.Keys.ToList();
            foreach (var uuid in members)
            {
                if (uuid == family.Owner)
                    continue;
                family.DeleteMember(uuid);
            }
            GameLog.Admin(player.Name, $"kickallmembers({familyId})", $"{family.Name}");
        }
        [Command("setfamleader")]
        public static void CMD_SetFamilyToPlayer(PlayerGo player, int id, int familyId)
        {
            if (!Group.CanUseAdminCommand(player, "setfamleader"))
                return;
            if (familyId == 1 && player.GetCharacter().AdminLVL < 10)
                return;
            var target = Main.GetPlayerByID(id);
            if (!target.IsLogged())
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_281", 3000);
                return;
            }
            var family = FamilyManager.GetFamily(familyId);
            if (family == null)
                return;
            var targetFamily = target.GetFamily();
            if (targetFamily != null)
            {
                if (family.Id != targetFamily.Id)
                {
                    if (!targetFamily.DeleteMember(target.GetCharacter().UUID))
                        return;
                }
            }
            FamilyManager.InvitePlayerToFamily(target, family, 0);
            GameLog.Admin(player.Name, $"setfamleader {id}", target.Name);
        }
        [Command("setfamowner")]
        public static void CMD_SetFamilyToPlayer(Player player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "setfamowner"))
                return;
            var target = Main.GetPlayerByID(id);
            if (!target.IsLogged())
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_281", 3000);
                return;
            }
            Family family = target.GetFamily();
            if (family == null)
                return;
            if (family.Id == 1 && player.GetCharacter().AdminLVL < 10)
                return;
            family.SetOwner(player);
            family.PreSave();
            family.Save();
            GameLog.Admin(player.Name, $"setfamowner {id}", target.Name);
        }

        [Command("fambiz")]
        public static void CMD_setBizMafia(Player player, int famId)
        {
            if (!player.IsLogged()) return;
            if (!Group.CanUseAdminCommand(player, "fambiz")) return;
            if (player.GetData<int>("BIZ_ID") == -1) return;


            Business biz = BusinessManager.BizList[player.GetData<int>("BIZ_ID")];
            biz.SetPatronageFamily(famId);
            GameLog.Admin($"{player.Name}", $"setBizFam({biz.ID},{famId})", $"");
            Family family = FamilyManager.GetFamily(famId);
            if (family != null)
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_226".Translate(family.Name, biz.ID), 3000);
        }

        [Command("setfambiz")]
        public static void CMD_setBizMafia(Player player, int bizID, int famId)
        {
            if (!player.IsLogged()) return;
            if (!Group.CanUseAdminCommand(player, "setfambiz")) return;


            Business biz = BusinessManager.BizList[bizID];
            biz.SetPatronageFamily(famId);
            GameLog.Admin($"{player.Name}", $"setBizFam({biz.ID},{famId})", $"");
            Family family = FamilyManager.GetFamily(famId);
            if (family != null)
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Core_226".Translate(family.Name, biz.ID), 3000);
        }

        [Command("changefampoints")]
        public static void ChangeFamilyPoins(Player player, int famId, int points)
        {
            if (!player.IsLogged()) return;
            if (!Group.CanUseAdminCommand(player, "changefampoints")) return;
            var family = FamilyManager.GetFamily(famId);
            family.ChangePoints(points);
            FamilyMenuManager.UpdateFamilyRatingData(family);
        }

        [Command("clearfamilypoints")]
        public static void ClearFamilyPoints(Player player)
        {
            if (!player.IsLogged()) return;
            if (player.GetCharacter().UUID != 529132) return;
            FamilyManager.ClearPoints();
        }

        [Command("spawnfamcar")]
        public static void SpawnFamCar(Player player)
        {
            if (!player.IsLogged()) return;
            var fam = player.GetFamily();
            if (fam == null)
                return;
            var house = HouseManager.GetHouseFamily(player);
            if (house == null)
                return;
            if (!fam.IsLeader(player))
                return;
            house.HouseGarage.DestroyCars(true);
            WhistlerTask.Run(() =>
            {
                house.HouseGarage.RespawnCars();
            }, 1000);
        }

        //[Command("takebiz")]
        //public static void CMD_takeBiz(Player player)
        //{
        //    if (!FamilyManager.CanAccessToBizWar(player)) return;
        //    if (player.GetData<int>("BIZ_ID") == -1)
        //    {
        //        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_437", 3000);
        //        return;
        //    }
        //    Business biz = BusinessManager.BizList[player.GetData<int>("BIZ_ID")];
        //    if (FamilyManager.GetFamily(biz.FamilyPatronage) != null)
        //    {
        //        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_544", 3000);
        //        return;
        //    }
        //    var family = player.GetFamily();
        //    if (family == null)
        //        return;
        //    biz.SetPatronageFamily(family.Id);
        //}
    }
}
