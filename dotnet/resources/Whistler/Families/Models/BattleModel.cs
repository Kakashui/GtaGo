using System;
using System.Data;
using Whistler.Families.Models.DTO;
using Whistler.SDK;

namespace Whistler.Families.Models
{
    class BattleModel
    {
        public int Id { get; private set; }
        public int BizId { get; private set; }
        public int FamilyDef { get; private set; }
        public int FamilyAttack { get; private set; }
        public BattleLocation Location { get; private set; }
        public int Weapon { get; private set; }
        public DateTime Date { get; private set; }
        public string Comment { get; private set; }
        public BattleStatus Status { get; private set; }
        public int FamilyWinner { get; private set; }
        public int DefPoints { get; private set; }
        public int AttackPoints { get; private set; }

        public string Timer = null;

        public BattleModel(int bizId, int familyDef, int familyAttack, BattleLocation location, int weapon, DateTime date, string comment)
        {
            BizId = bizId;
            FamilyDef = familyDef;
            FamilyAttack = familyAttack;
            Location = location;
            Weapon = weapon;
            Date = date;
            Comment = comment;
            Status = BattleStatus.WaitConfirm;
            var dataQuery = MySQL.QueryRead("INSERT INTO `familybattles`(`business`, `familydef`, `familyattack`, `location`, `weapon`, `time`, `comment`, `status` ) " +
                "VALUES (@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6, @prop7); SELECT @@identity;",
                BizId, FamilyDef, FamilyAttack, (int)Location, Weapon, MySQL.ConvertTime(Date), Comment, (int)Status);
            Id = Convert.ToInt32(dataQuery.Rows[0][0]);
        }
        public BattleModel(DataRow row)
        {
            Id = Convert.ToInt32(row["id"]);
            BizId = Convert.ToInt32(row["business"]);
            FamilyDef = Convert.ToInt32(row["familydef"]);
            FamilyAttack = Convert.ToInt32(row["familyattack"]);
            Location = (BattleLocation)Convert.ToInt32(row["location"]);
            Weapon = Convert.ToInt32(row["weapon"]);
            Date = Convert.ToDateTime(row["time"]);
            Comment = Convert.ToString(row["comment"]);
            Status = (BattleStatus)Convert.ToInt32(row["status"]);
            FamilyWinner = Convert.ToInt32(row["familywinner"]);
        }

        public BattleDTO GetBattleDTO(int familyId)
        {
            return new BattleDTO(this, familyId);
        }
        private void ChangeStatus(BattleStatus newStatus)
        {
            Status = newStatus;
            MySQL.Query("UPDATE `familybattles` SET `status` = @prop1 WHERE `id` = @prop0", Id, (int)Status);
            FamilyMenu.FamilyMenuManager.UpdateBattleInfo(this);
        }

        public void ConfirmBattle(bool accepted)
        {
            if (Status != BattleStatus.WaitConfirm)
                return;
            if (Date <= DateTime.Now)
                return;
            if (accepted)
                ChangeStatus(BattleStatus.Confirm);
            else
                FinishBattle(BattleStatus.FinishedWithoutAFight, FamilyAttack);
        }

        public void FinishBattle(BattleStatus result, int familyWinner)
        {
            Family familyAttack = FamilyManager.GetFamily(FamilyAttack);
            Family familyDef = FamilyManager.GetFamily(FamilyDef);
            FamilyWinner = familyWinner;
            var biz = Core.BusinessManager.GetBusiness(BizId);
            switch (result)
            {
                case BattleStatus.FightIsOver:
                case BattleStatus.FinishedWithoutAFight:
                    if (familyWinner == FamilyAttack)
                    {
                        biz?.SetPatronageFamily(familyWinner);
                    }
                    UpdateRating(familyAttack, familyDef, FamilyWinner);
                    break;
            }
            ChangeStatus(result);
        }

        public void NotCarriedOut()
        {
            FamilyWinner = FamilyDef;
            MySQL.Query("UPDATE `familybattles` SET `familywinner` = @prop1 WHERE `id` = @prop0", Id, FamilyWinner);
            ChangeStatus(BattleStatus.NotCarriedOut);
        }

        private void UpdateRating(Family familyAttack, Family familyDefend, int familyWin)
        {
            if (familyAttack == null || familyDefend == null)
                return;
            AttackPoints = FamilyWars.WarManager.GetDiffRating(familyAttack, familyDefend.EloRating, familyAttack.Id == familyWin);
            DefPoints = FamilyWars.WarManager.GetDiffRating(familyDefend, familyAttack.EloRating, familyDefend.Id == familyWin);
            familyAttack.AddFamilyBattle(AttackPoints);
            familyDefend.AddFamilyBattle(DefPoints);
            MySQL.Query("UPDATE `familybattles` SET `famattackpoint` = @prop1, `famdefpoint` = @prop2, `familywinner` = @prop3 WHERE `id` = @prop0", Id, AttackPoints, DefPoints, FamilyWinner);
        }

    }
}
