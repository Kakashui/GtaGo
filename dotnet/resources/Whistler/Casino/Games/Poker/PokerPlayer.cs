using System.Collections.Generic;
using GTANetworkAPI;
using Whistler;
using Whistler.Casino.Dtos;
using ServerGo.Casino.Business;
using ServerGo.Casino.ChipModels;
using ServerGo.Casino.Gamblers;
using ServerGo.Casino.Games.Poker.Cards;
using Newtonsoft.Json;
using Whistler.Helpers;
using Whistler.Entities;
using Whistler.SDK;

namespace ServerGo.Casino.Games.Poker
{
    internal class PokerPlayer
    {
        [JsonIgnore]
        public List<Card> Cards { get; private set;}
        [JsonIgnore]
        public Gambler Gambler { get; }
        [JsonIgnore]
        public int FullBank {get; private set;}
        public int Balance { get; private set; }
        public bool IsOnDealing { get; private set; }
        [JsonIgnore]
        public int Bets { get; set; }
        public List<Card> openCard { get; set;}
        public string clientName;
        public string Image { get; set; }

        public int _place {get; set;}
        public string _move {get; set;} //последнее действие
        public int currBet {get; set;} //ставка в текущем раунде
        public bool allIn;

        [JsonIgnore]
        public ExtPlayer _client;
        public PokerPlayer(ExtPlayer player, Gambler gambler, int place, int minBalance)
        {
            FullBank = gambler.Bank.TotalValue;//calculate amount of the chips
            ChangeBalance(minBalance);
            ChangeFullBank(-minBalance);
            Gambler = gambler;
            Cards = new List<Card>(2);
            openCard = new List<Card>();
            _client = player;
            _place = place;
            clientName = _client.Name;
            allIn = false;
            Image = player.Character.ImageLink;
        }

        public void JoinDealing()
        {
            IsOnDealing = true;
        }

        public void LeaveDealing()
        {
            IsOnDealing = false;
            Cards = new List<Card>(2);
            openCard = new List<Card>();
        }

        public void RequestBet(int currentBet, int myBet)
        {
            SafeTrigger.ClientEvent(_client, "poker:requestMakingBet", currentBet, myBet);
        }

        public void SubBets(int amount)
        {
            if (amount > Bets)
                Bets = 0;
            else
                Bets -= amount;
        }

        public void ChangeBalance(int amount)
        {
            if (Balance + amount < 0)
                Balance = 0;
            else
                Balance += amount;
        }

        public void ChangeFullBank(int amount)
        {
            if (FullBank + amount < 0)
                FullBank = 0;
            else
                FullBank += amount;
        }

        public void ResetData() //сбрасываем данные прошлой игры
        {
            LeaveDealing();
            Bets = 0;
            _move = "";
            allIn = false;
        }

    }
    class CompPlayerByPlace<T> : IComparer<T> //sort by val asc
        where T : PokerPlayer
    {
        public int Compare(T x, T y)
        {
            if (x._place < y._place)
                return -1;
            else if (x._place > y._place)
                return 1;
            else      
                return 0;
        }
    }
}