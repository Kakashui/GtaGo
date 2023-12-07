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
        public static void CMD_Families(ExtPlayer player, string name = "")
        {
            if (!Group.CanUseAdminCommand(player, "families"))
                return;
            FamilyManager.ViewFamilyList(player, name);
        }
        [Command("famkick")]
        public static void CMD_Families(ExtPlayer player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "famkick"))
                return;
            ExtPlayer target = Trigger.GetPlayerByUuid(id);
            if (target.IsLogged())
            {
                if (target.GetFamily()?.DeleteMember(target.Character.UUID) ?? false)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок кикнут из семьи", 3000);
                    GameLog.Admin(player.Name, "famkick", target.Name);
                }
            }
            else
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Игрок с таким ID не найден", 3000);
        }
        [Command("setfamtype", "USAGE: /setfamtype [familyId] [type] Unknown = 0, Neutral = 1, Crime = 2, Government = 3")]
        public static void CMD_SetFamilyType(ExtPlayer player, int familyId, int type)
        {
            if (!Group.CanUseAdminCommand(player, "setfamtype"))
                return;
            if (!Enum.IsDefined(typeof(OrgActivityType), type))
                return;
            FamilyManager.GetFamily(familyId)?.SetFamilyType((OrgActivityType)type);            
        }
        [Command("psto")]
        public static void CMD_SetFamilyClothesPoint(ExtPlayer player, int familyId)
        {
            if (!Group.CanUseAdminCommand(player, "psto"))
                return;
            FamilyManager.GetFamily(familyId)?.SetClothesPoint(player.Position - new Vector3(0, 0, 1), player.Dimension);
        }
        [Command("kickallmembers")]
        public static void CMD_DeleteAllFamilyMembers(ExtPlayer player, int familyId)
        {
            if (!Group.CanUseAdminCommand(player, "delallmembers"))return;

            Family family = FamilyManager.GetFamily(familyId);
            if (family == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Семьи с таким ID не найдено.", 3000);
                return;
            }
            if (family.Members == null || family.Members.Any())
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"В семье нет игроков.", 3000);
                return;
            }

            List<int> members = family.Members.Keys.ToList();
            foreach (var uuid in members)
            {
                if (uuid == family.Owner) continue;

                family.DeleteMember(uuid);
            }
            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы успешно очистили семью {family.Name}", 3000);
            GameLog.Admin(player.Name, $"kickallmembers({familyId})", $"{family.Name}");
        }
        [Command("setfamleader")]
        public static void CMD_SetFamilyToPlayer(ExtPlayer player, int id, int familyId)
        {
            if (!Group.CanUseAdminCommand(player, "setfamleader")) return;

            ExtPlayer target = Trigger.GetPlayerByUuid(id);
            if (!target.IsLogged())
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Игрок с таким ID не найден", 3000);
                return;
            }

            Family family = FamilyManager.GetFamily(familyId);
            if (family == null) 
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Семьи с таким ID не найдено.", 3000);
                return;
            }

            Family targetFamily = target.GetFamily();
            if (targetFamily != null && family.Id != targetFamily.Id && !targetFamily.DeleteMember(target.Character.UUID)) return;

            FamilyManager.InvitePlayerToFamily(target, family, 0);
            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы успешно установили {target.Name} лидером семьи.", 3000);
            GameLog.Admin(player.Name, $"setfamleader {id}", target.Name);
        }
        [Command("setfamowner")]
        public static void CMD_SetFamilyToPlayer(ExtPlayer player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "setfamowner")) return;

            ExtPlayer target = Trigger.GetPlayerByUuid(id);
            if (!target.IsLogged())
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Игрок с таким ID не найден", 3000);
                return;
            }

            Family family = target.GetFamily();
            if (family == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Игрок не состоит в семье", 3000);
                return;
            }

            family.SetOwner(player);
            family.PreSave();
            family.Save();
            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы успешно установили {target.Name} владельцем семьи.", 3000);
            GameLog.Admin(player.Name, $"setfamowner {id}", target.Name);
        }

        [Command("fambiz")]
        public static void CMD_setBizMafia(ExtPlayer player, int famId)
        {
            if (!player.IsLogged()) return;
            if (!Group.CanUseAdminCommand(player, "fambiz")) return;

            int bizId = player.GetData<int>("BIZ_ID");
            if (bizId == -1)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы должны стоять на точке бизнеса.", 3000);
                return;
            }

            Family family = FamilyManager.GetFamily(famId);
            if (family == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Семьи с таким ID не найдено.", 3000);
                return;
            }

            if (!BusinessManager.BizList.ContainsKey(bizId))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"В системе не найден бизнес с ID {bizId}.", 3000);
                return;
            }

            Business biz = BusinessManager.BizList[bizId];
            biz.SetPatronageFamily(famId);
            GameLog.Admin($"{player.Name}", $"setBizFam({biz.ID},{famId})", $"");
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"{family.Name} теперь владеет бизнесом #{biz.ID}", 3000);
        }

        [Command("setfambiz")]
        public static void CMD_setBizMafia(ExtPlayer player, int bizID, int famId)
        {
            if (!player.IsLogged()) return;
            if (!Group.CanUseAdminCommand(player, "setfambiz")) return;

            Family family = FamilyManager.GetFamily(famId);
            if (family == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Семьи с таким ID не найдено.", 3000);
                return;
            }

            if (!BusinessManager.BizList.ContainsKey(bizID))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"В системе не найден бизнес с ID {bizID}.", 3000);
                return;
            }

            Business biz = BusinessManager.BizList[bizID];
            biz.SetPatronageFamily(famId);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"{family.Name} теперь владеет бизнесом #{biz.ID}", 3000);
            GameLog.Admin($"{player.Name}", $"setBizFam({biz.ID},{famId})", $"");
        }

        [Command("changefampoints")]
        public static void ChangeFamilyPoins(ExtPlayer player, int famId, int points)
        {
            if (!player.IsLogged()) return;
            if (!Group.CanUseAdminCommand(player, "changefampoints")) return;
            Family family = FamilyManager.GetFamily(famId);
            if (family == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Семьи с таким ID не найдено.", 3000);
                return;
            }

            family.ChangePoints(points);
            FamilyMenuManager.UpdateFamilyRatingData(family);
        }

        //[Command("clearfamilypoints")]
        public static void ClearFamilyPoints(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            //if (player.Character.UUID != 529132) return;
            FamilyManager.ClearPoints();
        }

        [Command("spawnfamcar")]
        public static void SpawnFamCar(ExtPlayer player)
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
            NAPI.Task.Run(() =>
            {
                house.HouseGarage.RespawnCars();
            }, 1000);
        }

        //[Command("takebiz")]
        //public static void CMD_takeBiz(ExtPlayer player)
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
