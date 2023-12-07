using System;
using System.Collections.Generic;

namespace ServerGo.Casino.Games.Poker
{
    internal class DealingRound
    {
        private Dictionary<PokerPlayer, Dictionary<BidRoundType, int>> Bets = new Dictionary<PokerPlayer, Dictionary<BidRoundType, int>>();

        public List<int> BankList;
        private int currNumberBank;
        public BidRoundType currRound;

        public DealingRound(IEnumerable<PokerPlayer> players)
        {
            currRound = BidRoundType.Preflop;
            foreach (var player in players)
            {
                Bets.Add(player, new Dictionary<BidRoundType, int>());
                Bets[player].Add(currRound, 0);
            }
            BankList = new List<int>();
            BankList.Add(0);
            currNumberBank = 0;
        }

        public int getAllBank()
        {
            try
            {
                int result = 0;
                if (BankList != null)
                    foreach (int subBank in BankList)
                        result += subBank;
                return result;
            }
            catch
            {
                return 0;
            }

        }

        public void NextRound()
        {
            int currBet = 0;
            bool flag = false;
            foreach (var play in Bets.Values)
            {
                if (play[currRound] > currBet)
                    currBet = play[currRound];

            }
            foreach (var player in Bets)
            {
                if (player.Key.IsOnDealing && player.Value[currRound] == 0 && player.Key.allIn)
                {
                    flag = true;
                }
            }
            int amountSubBet = 0;
            while (currBet > 0)
            {
                if (flag)
                {
                    currNumberBank++;
                    BankList.Add(0);
                    flag = false;
                    foreach (var player in Bets)
                        player.Key.allIn = false;
                }
                if (amountSubBet > 0)
                {
                    foreach (var player in Bets)
                        player.Key.allIn = false;
                }

                int minBet = currBet;
                foreach (var player in Bets)
                {
                    if (player.Key.IsOnDealing && player.Value[currRound] - amountSubBet < minBet && player.Value[currRound] > amountSubBet)
                    {
                        minBet = player.Value[currRound] - amountSubBet;
                    }
                }
                int amountBank = 0;
                foreach (var player in Bets)
                {
                    if (player.Value[currRound] >= minBet + amountSubBet)
                    {
                        amountBank += minBet;
                    }
                    else if ((player.Value[currRound] < minBet + amountSubBet) && player.Value[currRound] > amountSubBet)
                        amountBank += (player.Value[currRound] - amountSubBet);
                    if (player.Value[currRound] == minBet + amountSubBet && player.Key.Balance == 0) 
                    {
                        player.Key.allIn = true;
                    }
                }
                BankList[currNumberBank] += amountBank;
                currBet -= minBet;
                amountSubBet += minBet;
                if (currBet > 0)
                {
                    currNumberBank++;
                    BankList.Add(0);
                }
            }
            currRound++;
            if (currRound != BidRoundType.OpenCard)
            {
                InitBetsCurrRound(currRound);
            }
        }

        private void InitBetsCurrRound(BidRoundType round)
        {
            foreach (var key in Bets.Keys)
            {
                Bets[key].Add(round, 0);
            }
        }

        public int GiveBetPlayer(PokerPlayer player)
        {
            if (Bets.ContainsKey(player) && Bets[player].ContainsKey(currRound))
                return Bets[player][currRound];
            else return 0;
        }

        public void SetBetPlayer(PokerPlayer player, int amount)
        {
            if (player.Balance >= amount)
            {
                Bets[player][currRound] += amount;
                player.Bets += amount;
                player.currBet = GiveBetPlayer(player);
                player.ChangeBalance(-amount);
            }
        }

        public int GiveCurrentBank(int bet)
        {
            int summ = 0;
            foreach (var player in Bets)
            {
                int b = bet;
                BidRoundType r = BidRoundType.Preflop;
                while (b > 0 && r < BidRoundType.OpenCard)
                {
                    if (player.Value[r] < b)
                    {
                        summ += player.Value[r];
                        b -= player.Value[r];
                        player.Value[r] = 0;
                    }
                    else
                    {
                        summ += b;
                        player.Value[r] -= b;
                        b = 0;
                    }
                    r++;
                }
            }
            return summ;
        }

        public int returnBets()
        {
            int max = 0;
            int submax = 0;
            foreach (var player in Bets)
            {
                if (player.Value[currRound] > submax)
                    submax = player.Value[currRound];
                if (submax > max)
                {
                    int k = max;
                    max = submax;
                    submax = k;
                }
            }
            if (max != submax)
                foreach (var player in Bets)
                {
                    if (player.Value[currRound] == max)
                    {
                        SetBetPlayer(player.Key, submax - max);
                        return player.Key._place;
                    }
                }
            return -1;
        }
    }
}