using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Fractions.DTO;
using Whistler.SDK;

namespace Whistler.Fractions.GOV.Models
{
    class ConsignmentVote
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CountVote { get; set; }
        public ConsignmentVote(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            var result = MySQL.QueryRead("SELECT count(*) as countVote FROM `characters` WHERE `lastvote` = @prop0 AND `deleted` = false", Id);
            if (result != null && result.Rows.Count > 0)
            {
                CountVote = Convert.ToInt32(result.Rows[0]["countVote"]);
            }
            else
                CountVote = 0;
        }
        public ConsignmentVoteDTO GetConsignmentVoteDTO()
        {
            return new ConsignmentVoteDTO(this);
        }
    }
}
