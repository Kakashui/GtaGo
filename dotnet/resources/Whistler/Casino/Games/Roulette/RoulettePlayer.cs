using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Whistler.Casino.Dtos;
using Whistler.SDK;
using Whistler;
using ServerGo.Casino.Business;
using ServerGo.Casino.ChipModels;
using ServerGo.Casino.Gamblers;
using Whistler.Entities;

namespace ServerGo.Casino.Games.Roulette
{
    internal class RoulettePlayer
    {
        public Gambler Gambler { get; }
        public Queue<Bet> Bets { get; private set; }
        
        public int Winning { get; set; }
        public int ChairIndex { get; }

        public RoulettePlayer(Gambler gambler, int chairIndex)
        {
            Gambler = gambler;
            Bets = new Queue<Bet>();
            ChairIndex = chairIndex;
        }

        public void MakeBet(string bet, IEnumerable<Chip> chips)
        {
            Bets.Enqueue(new Bet(bet, chips));
        }

        public void ClearBets()
        {
            Bets = new Queue<Bet>();
            Winning = 0;
        }

        public void CancelBet()
        {
            var lastBet = Bets.Dequeue();
            Gambler.Bank.Charge(lastBet.Cost.Chips);
        }

        public void NotifyAboutWinning(ExtPlayer client)
        {
            if (Winning > 0)
                Notify.Send(client, NotifyType.Success, NotifyPosition.Center, $"Вы выиграли {Winning}$!", 3000);
        }
    }
}