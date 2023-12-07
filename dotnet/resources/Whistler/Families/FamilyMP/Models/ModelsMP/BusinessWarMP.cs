using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Families.FamilyMenu;
using Whistler.Families.FamilyWars;
using Whistler.Helpers;
using Whistler.MP.OrgBattle;
using Whistler.SDK;

namespace Whistler.Families.FamilyMP.Models.ModelsMP
{
    class BusinessWarMP : FamilyMPModel
    {
        public int BusinessID { get; set; }
        public BusinessWarMP(DataRow row) : base(row)
        {
            BusinessID = Convert.ToInt32(row["business"].ToString());
        }
        public BusinessWarMP(DateTime date, BattleLocation location, int businessID) : base(date, location, FamilyMPType.BusinessWar)
        {
            BusinessID = businessID;
            var dataQuery = MySQL.QueryRead("INSERT INTO `familymp`(`date`, `location`, `winner`, `finished`, `type`, `kills`, `business`) " +
                "VALUES (@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6); SELECT @@identity;",
                MySQL.ConvertTime(Date), (int)Location, WinnerFamily, IsFinished, (int)Type, JsonConvert.SerializeObject(KillLog), BusinessID);
            ID = Convert.ToInt32(dataQuery.Rows[0][0]);
        }

        public override bool IsMember(ExtPlayer player)
        {
            return false;
        }


        public override bool PlayerDeath(ExtPlayer player, ExtPlayer killer, uint weapon)
        {
            return false;
        }

        public override void TryEndMP()
        {
        }
        private void StartBattleAction()
        {
            IsPlaying = true;
            Chat.AdminToAll($"Мероприятие \"{NameMP}\" началось!");
        }
        private void EndBattleAction(int orgId)
        {
            WinnerFamily = orgId;

            ManagerMP.CurrentMP = null;
            IsPlaying = false;
            WinnerFamilyName = FamilyManager.GetFamilyName(WinnerFamily);
            IsFinished = true;
            Save();

            var biz = BusinessManager.GetBusiness(BusinessID);
            biz?.SetPatronageFamily(WinnerFamily);
            FamilyMenuManager.UpdateFamilyMP(this);
            if (WinnerFamily > 0)
                Chat.AdminToAll($"Мероприятие \"{NameMP}\" завершено! Победила семья {WinnerFamilyName}");
            else
                Chat.AdminToAll($"Мероприятие \"{NameMP}\" завершено! Ни одной семье не удалось захватить бизнес");
        }
        public override bool TryStartMP()
        {
            if (IsFinished || IsPlaying)
                return false;
            OrgBattleManager.CreateOrgBattleWithPoints(Common.OrganizationType.Family, Location, Common.OrgActivityType.Crime)
                .AddStartAction(StartBattleAction)
                .AddEndAction(EndBattleAction)
                .BattleStart();
            return true;
        }
    }
}