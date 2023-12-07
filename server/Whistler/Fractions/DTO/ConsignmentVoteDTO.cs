using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Fractions.GOV.Models;
using Whistler.Fractions.Models;

namespace Whistler.Fractions.DTO
{
    class ConsignmentVoteDTO
    {
        public int id { get; set; }
        public bool vote { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public int countVote { get; set; }
        public ConsignmentVoteDTO(ConsignmentVote consignmentVote)
        {
            id = consignmentVote.Id;
            vote = false;
            name = consignmentVote.Name;
            desc = consignmentVote.Description;
            countVote = consignmentVote.CountVote;
        }
    }
}
