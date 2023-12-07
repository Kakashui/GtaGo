using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers.Interfaces;

namespace Whistler.MP.RoyalBattle.Models
{
    class PlayerRatingDTO : ISortedWithPlace
    {
        public string nickname { get; set; }
        public int kills { get; set; }
        public int place { get; set; }

        public int SortedValue => kills;

        public int Place { set => place = value; }

        public PlayerRatingDTO(string nickname, int kills, int place)
        {
            this.nickname = nickname;
            this.kills = kills;
            this.place = place;
        }
    }
}
