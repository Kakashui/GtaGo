using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper.Internal;

namespace Whistler.Families.Models.DTO
{
    internal class FamilyMemberDTO
    {
        public int id { get; set; }
        public string nickname { get; set; }
        public int rank { get; set; }
        public bool online { get; set; }
        public int up { get; set; }
        public int down { get; set; }
        public int rating { get; set; }

        public FamilyMemberDTO(FamilyMember member, bool online)
        {
            id = member.PlayerUUID;
            nickname = Main.PlayerNames.GetOrDefault(member.PlayerUUID);
            rank = member.Rank;
            this.online = online;
            up = member.PointsAdd;
            down = member.PointsSub;
            rating = member.Points;
        }
    }
}
