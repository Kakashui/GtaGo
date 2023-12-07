using System.Collections.Generic;
using GTANetworkAPI;
using Whistler;
using ServerGo.Casino.ChipModels;
using ServerGo.Casino.Games;
using ServerGo.Casino.Games.Roulette;
using Whistler.Helpers;
using Whistler.Entities;

namespace ServerGo.Casino.Gamblers
{
    /// <summary>
    /// Represents the player with chips
    /// </summary>
    public class Gambler
    {
        public ChipBank Bank { get; }
        public BaseCasinoGame ActingGame { get; private set; }

        public List<BetsStat> RouletteStats { get; private set; }

        public Gambler(ExtPlayer player)
        {
            RouletteStats = new List<BetsStat>();
            Bank = new ChipBank(player.Character);
        }
        public Gambler(ExtPlayer player, IEnumerable<Chip> chips)
        {
            RouletteStats = new List<BetsStat>();
            Bank = new ChipBank(player.Character, chips);
        }

        public void StartGame(BaseCasinoGame game)
        {
            if (ActingGame != null) return;
            ActingGame = game;
        }

        public void CancelGame()
        {
            ActingGame = null;
        }

        public void ClearStats()
        {
            RouletteStats = new List<BetsStat>();
        }
    }
}