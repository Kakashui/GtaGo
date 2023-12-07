using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Fractions.GOV.Models;

namespace Whistler.Fractions.GOV.Config
{
    class Consignments
    {
        public static DateTime FinishVoteDate = new DateTime(2021, 2, 28, 6, 0, 0);
        public static List<Vector3> VotePositions = new List<Vector3>
        {
            new Vector3(-539.9066, -191.0166, 37.21965),
            new Vector3(-559.0994, -201.9234, 37.21966),
            new Vector3(-538.1046, -181.8093, 41.70978),
            new Vector3(-567.8675, -199.0310, 41.70978),
        };

        public static Dictionary<int, ConsignmentVote> ConsignmentVotes = new Dictionary<int, ConsignmentVote>
        {
           
        };
    }
}
