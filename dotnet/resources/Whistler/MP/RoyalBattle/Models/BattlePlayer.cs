using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.Inventory;
using Whistler.Inventory.Models;

namespace Whistler.MP.RoyalBattle.Models
{
    class BattlePlayer
    {
        public int Kills { get; set; }
        public ExtPlayer _player { get; set; }
        public BattlePlayer(ExtPlayer player)
        {
            Kills = 0;
            _player = player;
        }
    }
}
