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
        public PlayerGo _player { get; set; }
        public BattlePlayer(PlayerGo player)
        {
            Kills = 0;
            _player = player;
        }
    }
}
