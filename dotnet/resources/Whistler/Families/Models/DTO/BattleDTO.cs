using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Families.Models.DTO
{
    class BattleDTO
    {
        public int id { get; set; }
        public int bizId { get; set; }
        public bool completed { get; set; }
        public int status { get; set; }
        public bool modal { get; set; }
        public bool incoming { get; set; }
        public bool won { get; set; }
        public string bizName { get; set; }
        public string enemyName { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string weaponName { get; set; }
        public string placeName { get; set; }
        public string comment { get; set; }

        public BattleDTO(BattleModel battle, int familyId)
        {
            id = battle.Id;
            bizId = battle.BizId;
            completed = battle.Status > BattleStatus.Confirm;
            status = (int)battle.Status;
            modal = battle.Status == BattleStatus.WaitConfirm && familyId == battle.FamilyDef;
            incoming = battle.FamilyDef == familyId;
            won = battle.FamilyWinner == familyId;
            bizName = Core.BusinessManager.GetBusiness(battle.BizId)?.GetBusinessName() ?? "Unknown";
            enemyName = FamilyManager.GetFamilyName(battle.FamilyAttack == familyId ? battle.FamilyDef : battle.FamilyAttack);
            date = battle.Date.ToString("d");
            time = battle.Date.ToString("t");
            weaponName = battle.Weapon.ToString();
            placeName = battle.Location.ToString();
            comment = battle.Comment;
        }
    }
}
