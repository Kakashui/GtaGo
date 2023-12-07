using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Common.Interfaces;
using Whistler.Families.Configs;
using Whistler.Families.Models.DTO;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Families.Models
{
    internal class FamilyMember : IMember
    {
        public int PlayerUUID { get; set; }
        public int Rank { get; set; }
        public int Points { get; set; }
        public int PointsAdd { get; set; }
        public int PointsSub { get; set; }
        public DateTime PointsUpdate { get; set; }
        private bool _editing { get; set; }
        public FamilyMember(Family family, int playerUUID, int rank)
        {
            PlayerUUID = playerUUID;
            Rank = rank;
            Points = 0;
            PointsAdd = 0;
            PointsSub = 0;
            _editing = false;
            MySQL.Query(
                "UPDATE `characters` " +
                "SET `family` = @prop1, `familylvl` = @prop2, `familypoints` = @prop3, `familypointsadd` = @prop4, `familypointssub` = @prop5, `familypointlastupdate` = @prop6 " +
                "WHERE `uuid` = @prop0", 
                PlayerUUID, family.Id, Rank, Points, PointsAdd, PointsSub, MySQL.ConvertTime(PointsUpdate));
            var player = Trigger.GetPlayerByUuid(PlayerUUID);
            if (player != null)
            {
                player.Character.FamilyID = family.Id;
                player.Character.FamilyLVL = Rank;
            }
            FamilyMenu.FamilyMenuManager.UpdateMemberToFamily(family, PlayerUUID, family.OnlineMembers.ContainsKey(PlayerUUID));
        }
        public FamilyMember(int playerUUID, int rank, int points, int pointsAdd, int pointsSub, DateTime pointsUpdate)
        {
            PlayerUUID = playerUUID;
            Rank = rank;
            _editing = false;
            Points = points;
            PointsAdd = pointsAdd;
            PointsSub = pointsSub;
            PointsUpdate = pointsUpdate;
            _editing = false;
        }
        public void ChangeRank(Family family, int newRank)
        {
            Rank = newRank;
            MySQL.Query("UPDATE `characters` SET `familylvl` = @prop0 WHERE `uuid` = @prop1", Rank, PlayerUUID);
            var player = Trigger.GetPlayerByUuid(PlayerUUID);
            if (player != null)
            {
                player.Character.FamilyLVL = Rank;
                SafeTrigger.ClientEvent(player, "family:updateMemberRank", Rank);
            }
            FamilyMenu.FamilyMenuManager.UpdateMemberToFamily(family, PlayerUUID, family.OnlineMembers.ContainsKey(PlayerUUID));
        }
        public FamilyMemberDTO GetFamilyMemberDTO(bool online)
        {
            return new FamilyMemberDTO(this, online);
        }
        public void ChangePoints(Family family, FamilyActions action)
        {
            return;//TODO
            if (!PointsForAction.ListPoints.ContainsKey(action))
                return;
            _editing = true;
            var value = PointsForAction.ListPoints[action];
            Points += value;
            if (PointsUpdate.Day != DateTime.Now.Day || PointsUpdate.Month != DateTime.Now.Month)
            {
                PointsAdd = 0;
                PointsSub = 0;
            }
            if (value > 0)
                PointsAdd += value;
            else
                PointsSub += value;
            PointsUpdate = DateTime.Now;
            FamilyMenu.FamilyMenuManager.UpdateMemberToFamily(family, PlayerUUID, family.OnlineMembers.ContainsKey(PlayerUUID));
        }
        public void Save()
        {
            if (!_editing)
                return;
            _editing = false;
            MySQL.Query("UPDATE `characters` SET `familypoints` = @prop1, `familypointsadd` = @prop2, `familypointssub` = @prop3, `familypointlastupdate` = @prop4 WHERE `uuid` = @prop0", PlayerUUID, Points, PointsAdd, PointsSub, MySQL.ConvertTime(PointsUpdate));
        }
    }
}
