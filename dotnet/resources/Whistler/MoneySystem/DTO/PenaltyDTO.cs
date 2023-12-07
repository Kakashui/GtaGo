using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.MoneySystem.DTO
{
    class PenaltyDTO
    {
        public int id { get; set; }
        public string reason { get; set; }
        public int cost { get; set; }
        public string date { get; set; }
        public PenaltyDTO(int id, string reason, int cost, string date)
        {
            this.id = id;
            this.reason = reason;
            this.cost = cost;
            this.date = date;
        }
    }
}
