using System.Collections.Generic;
using ServerGo.Casino.Games.Poker.Cards;
using System.Linq;
using Newtonsoft.Json;
using Whistler.SDK;
using System;
using GTANetworkAPI;
using Whistler;
using Whistler.Helpers;

namespace ServerGo.Casino.Games.Poker
{
    internal class Dealing
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Dealing));
        private Queue<PokerPlayer> _queue; //players with cards
        public List<PokerPlayer> _players; //player in game
        private SortedList<Combins, PokerPlayer> _winsPlayer;
        private int SmallBlind;
        private int minBalanceFromGame;
        private PokerPlayer _lastBet;

        private Deck _deck;
        private DealingRound _dealingRound;
        public List<Card> _tableCards;
        private int _currentBet;
        private int _dealer;
        private int _maxPlayers;
        private int _minPlayers;
        private bool tableState;
        private string _timer; //timer after end game
        private string _timerMove;
        private string _timerBetweenRound;
        private static int time_show_winners = 5;
        private static int time_wait_newRound = 4;

        private PokerPlayer waitPlayer;


        public Dealing(int minBetAmount, int minPlayers = 2, int maxPlayers = 6)
        {
            _players = new List<PokerPlayer>();
            SmallBlind = minBetAmount;
            _dealer = maxPlayers;
            _maxPlayers = maxPlayers;
            _minPlayers = minPlayers;
            tableState = false;
            minBalanceFromGame = SmallBlind * 2;
            _timer = null;
        }


        public void startTimerNextGame()
        {
            try
            {
                if (tableState) //game already runned
                    return;
                if (_timer == null)
                {
                    _timer = Timers.Start(1000 * time_wait_newRound, TimerWaitNextRound);
                }

            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | startTimerNextGame: " + e.ToString());
            }
        }


        public void NewGame()
        {
            try
            {
                if (tableState)
                    return;
                if (CntPlayersInBalance(minBalanceFromGame) < _minPlayers)
                    return;
                tableState = true;
                if (_timer != null)
                    Timers.Stop(_timer);
                _timer = null;
                clearTableData();
                CompPlayerByPlace<PokerPlayer> sortPlayers = new CompPlayerByPlace<PokerPlayer>();
                _players.Sort(sortPlayers);
                NextDealer();
                foreach (var player in _players.ToList())
                {
                    player.ResetData();
                    if (!player._client.IsLogged())
                    {
                        _players.Remove(player);
                        continue;
                    }
                    if (player.Balance >= minBalanceFromGame)
                        _queue.Enqueue(player);
                }
                if (_queue.FirstOrDefault(c => c._place == _dealer) != null) //dealer to head queue
                    while (_queue.Peek()._place != _dealer)
                    {
                        _queue.Enqueue(_queue.Dequeue());
                    }
                _dealingRound = new DealingRound(_queue);
                DistributeCards();
                StartBidRound(_dealingRound.currRound);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | NewGame: " + e.ToString());
            }
        }

        private void clearTableData()
        {
            try
            {
                _queue = new Queue<PokerPlayer>();
                _deck = new Deck();
                _tableCards = new List<Card>();
                _winsPlayer = null;
                if (_timerMove != null)
                {
                    Timers.Stop(_timerMove);
                    _timerMove = null;
                }
                waitPlayer = null;
                if (_timerBetweenRound != null)
                {
                    Timers.Stop(_timerBetweenRound);
                    _timerBetweenRound = null;
                }
                foreach (var player in _players)
                {
                    player.ResetData();
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | clearTableData: " + e.ToString());
            }
        }

        public void StartBidRound(BidRoundType round)
        {
            try
            {
                
                _currentBet = 0;
                foreach (var player in _players)
                    player.currBet = 0;
                if (_timerBetweenRound != null)
                {
                    Timers.Stop(_timerBetweenRound);
                    _timerBetweenRound = null;
                }
                SendEventToMembers("poker:NewRound");
                if (CntIsOnDealingPlayers() <= 1)//TODO
                {
                    EarlyEndRound();
                    return;
                }
                switch (round)
                {
                    case BidRoundType.Preflop:
                        StartPreflop();
                        return;
                    case BidRoundType.Flop:
                        StartNewRound(3);
                        return;
                    case BidRoundType.Tern:
                        StartNewRound();
                        return;
                    case BidRoundType.River:
                        StartNewRound();
                        return;
                    case BidRoundType.OpenCard:
                        openCards();
                        return;
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | StartBidRound: " + e.ToString());
            }
        }

        private void StartPreflop()
        {
            try
            {
                DoBlindsStep();
                RequestBet(_queue.Peek());
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | StartPreflop: " + e.ToString());
            }
        }

        private void StartNewRound(int tableCard = 1)
        {
            try
            {
                PutCardOnTable(tableCard);
                if (CntPlayersInBalanceInDealing() < 2)
                    NextRound();
                else
                    RequestBet(_queue.Peek());
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | StartFlop: " + e.ToString());
            }
        }
        private void DoBlindsStep()
        {
            try
            {
                _queue.Enqueue(_queue.Dequeue());
                _currentBet = SmallBlind * 2;
                _dealingRound.SetBetPlayer(_queue.Peek(), SmallBlind);
                _queue.Peek()._move = "Малый блайнд";
                SendMoveOnPlayers(_queue.Peek());
                _queue.Enqueue(_queue.Dequeue());
                _queue.Peek()._move = "Большой блайнд";
                _dealingRound.SetBetPlayer(_queue.Peek(), _currentBet);
                SendMoveOnPlayers(_queue.Peek());
                _queue.Enqueue(_queue.Dequeue());

                _lastBet = _queue.Peek();
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | DoBlindsStep: " + e.ToString());
            }

        }

        private void DistributeCards()
        {
            try
            {
                foreach (var pokerPlayer in _players)
                {
                    if (pokerPlayer.Balance > SmallBlind * 2)
                    {
                        pokerPlayer.JoinDealing();
                        GiveCardFromDeck(pokerPlayer);
                        GiveCardFromDeck(pokerPlayer);
                    }
                }
                SendHandCardToPlayers();
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | DistributeCards: " + e.ToString());
            }
        }

        private void GiveCardFromDeck(PokerPlayer pokerPlayer)
        {
            try
            {
                var card = _deck.ExtractCard(_deck.GetRandomCard());
                if (card != null)
                    pokerPlayer.Cards.Add(card);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | GiveCardFromDeck: " + e.ToString());
            }
        }

        private void PutCardOnTable(int cnt = 1)
        {
            try
            {
                for (int i = 0; i < cnt; i++)
                {
                    var card = _deck.ExtractCard(_deck.GetRandomCard());
                    if (card != null)
                        _tableCards.Add(card);
                }
                SendEventToMembers("poker:TableCard", JsonConvert.SerializeObject(_tableCards));
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | GiveCardFromDeck: " + e.ToString());
            }
        }

        private void nextMake()
        {
            try
            {
                _queue.Enqueue(_queue.Dequeue());
                if (_queue.Peek() != _lastBet)
                    RequestBet(_queue.Peek());
                else
                    NextRound();
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | nextMake: " + e.ToString());
            }

        }
        private void NextRound()
        {
            try
            {
                var excessBet = _dealingRound.returnBets();
                if (excessBet != -1)
                {
                    var target = _players.FirstOrDefault(c => c._place == excessBet);
                    if (target != null)
                        SendEventToMembers("poker:changeBank", target._place, target.Balance, target.FullBank, target.currBet);
                }
                _dealingRound.NextRound();
                SendEventToMembers("poker:EndRound", JsonConvert.SerializeObject(_dealingRound.BankList));
                _timerBetweenRound = Timers.Start(2000, TimerBetweenRound);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | NextRound: " + e.ToString());
            }

        }

        private void RequestBet(PokerPlayer player)
        {
            try
            {
                if (player.Balance > 0 && player.IsOnDealing)
                {
                    player._move = "Wait";
                    SendMoveOnPlayers(player);
                    waitPlayer = player;
                    if (_timerMove != null)
                    {
                        Timers.Stop(_timerMove);
                        _timerMove = null;
                    }
                    _timerMove = Timers.Start(60000, TimerWaitBetPlayer);
                    player.RequestBet(_currentBet, _dealingRound.GiveBetPlayer(player));
                }
                else
                    nextMake();
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | RequestBet: " + e.ToString());
            }
        }

        public void RemovePlayer(PokerPlayer player)
        {
            try
            {
                player.LeaveDealing();
                SendEventToMembers("poker:leavePlayer", player._place);
                lock (_players)
                {
                    _players.Remove(player);
                }
                if (_winsPlayer != null && _winsPlayer.ContainsValue(player))
                    _winsPlayer.Remove(_winsPlayer.FirstOrDefault(c => c.Value == player).Key);
                if (waitPlayer == player)
                {
                    OnPlayerMadeBet(player, BetType.Fold);
                }
                if (_players.Count == 0)
                {
                    clearTableData();
                    tableState = false;
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | RemovePlayer: " + e.ToString());
            }
        }
        private void EarlyEndRound()
        {
            try
            {
                if (_queue.Count > 0)
                {
                    var winner = _queue.FirstOrDefault(c => c.IsOnDealing == true);
                    if (winner != null && _dealingRound != null)
                    {
                        winner.ChangeBalance(_dealingRound.getAllBank());
                        SendEventToMembers("poker:AllFoldButPlayer", winner._place, _dealingRound.getAllBank());
                    }
                }
                _dealingRound.BankList = null;
                EndGame();
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | EarlyEndRound: " + e.ToString());
            }
        }

        public void OnPlayerMadeBet(PokerPlayer player, BetType bet, int amount = 0)
        {
            try
            {
                if (player == waitPlayer && waitPlayer != null)
                {
                    waitPlayer = null;
                    if (_timerMove != null)
                    {
                        Timers.Stop(_timerMove);
                        _timerMove = null;
                    }
                    int prev_currentbank = _currentBet;
                    switch (bet)
                    {
                        case BetType.Call:
                            int sum = _currentBet - _dealingRound.GiveBetPlayer(player);
                            if (player.Balance >= sum)
                            {
                                _dealingRound.SetBetPlayer(player, sum);
                                player._move = "Уравнял(а)";
                            }
                            else
                            {
                                _dealingRound.SetBetPlayer(player, player.Balance);
                                player._move = "ALL IN!";
                            }
                            break;
                        case BetType.Fold:
                            player.LeaveDealing();
                            player._move = "Сбросил(а)";
                            break;
                        case BetType.Check:
                            if (_dealingRound.GiveBetPlayer(player) >= _currentBet)
                            {
                                player._move = "Пропуск";
                            }
                            else
                            {
                                player.LeaveDealing();
                                player._move = "Сбросил(а)";
                            }
                            break;
                        case BetType.Raise:
                            _currentBet = _dealingRound.GiveBetPlayer(player) + amount;
                            if (player.Balance > amount)
                            {
                                _dealingRound.SetBetPlayer(player, amount);
                                player._move = "Повышает";
                            }
                            else if (player.Balance == amount)
                            {
                                _dealingRound.SetBetPlayer(player, amount);
                                player._move = "ВаБанк!";
                            }
                            else if (player.Balance < amount)
                            {
                                _dealingRound.SetBetPlayer(player, player.Balance);
                                player._move = "ВаБанк!".Translate();
                            }
                            break;
                        case BetType.AllIn:
                            _dealingRound.SetBetPlayer(player, player.Balance);
                            _currentBet = _dealingRound.GiveBetPlayer(player);
                            player._move = "ВаБанк!".Translate();
                            break;
                    }

                    if (_currentBet > prev_currentbank)
                        _lastBet = player;
                    SendMoveOnPlayers(player);
                    nextMake();
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | OnPlayerMadeBet: " + e.ToString());
            }
        }

        private void openCards()
        {
            try
            {
                _winsPlayer = new SortedList<Combins, PokerPlayer>();
                foreach (var player in _players)
                {
                    if (player.IsOnDealing)
                    {
                        _winsPlayer.Add(Combinations.GetComb(player, _tableCards), player);
                        player.openCard = new List<Card>(player.Cards);
                    }
                }
                SendHandCardToPlayers();
                SeparationBank();
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | openCards: " + e.ToString());
            }
        }

        private void SeparationBank()
        {
            try
            {
                if (_dealingRound.BankList != null && _dealingRound.BankList.Count > 0 && _dealingRound.BankList[0] > 0 && _winsPlayer.Count > 0)
                {
                    Combins winner = _winsPlayer.Keys[0];

                    foreach (Card card in _tableCards)
                        if (winner.Cards.FirstOrDefault(c => c.IsEqualTo(card)) != null)
                            card.Wins = true;
                        else
                            card.Wins = false;

                    int countWins = 0;
                    var minBet = _winsPlayer.Values[0].Bets;
                    foreach (var player in _winsPlayer)
                    {
                        if (player.Key == winner)
                        {
                            foreach (Card card in player.Value.openCard)
                                if (player.Key.Cards.FirstOrDefault(c => c.IsEqualTo(card)) != null)
                                    card.Wins = true;
                                else
                                    card.Wins = false;
                            if (player.Value.Bets < minBet)
                                minBet = player.Value.Bets;
                            countWins++;
                        }
                        else break;
                    }
                    int sumWins = _dealingRound.GiveCurrentBank(minBet) / countWins;
                    List<PokerPlayer> listWinsThisStep = new List<PokerPlayer>();
                    foreach (var key in _winsPlayer.Keys)
                    {
                        if (key == winner)
                        {
                            listWinsThisStep.Add(_winsPlayer[key]);
                            _winsPlayer[key].ChangeBalance(sumWins);
                        }
                        _winsPlayer[key].SubBets(minBet);
                    }
                    while (true)
                    {
                        var play = _winsPlayer.FirstOrDefault(c => c.Value.Bets == 0);
                        if (play.Value != null)
                            _winsPlayer.Remove(play.Key);
                        else
                            break;
                    }
                    _dealingRound.BankList.Remove(_dealingRound.BankList[0]);
                    SendEventToMembers("poker:WinCombins", JsonConvert.SerializeObject(_tableCards), JsonConvert.SerializeObject(listWinsThisStep), sumWins, JsonConvert.SerializeObject(_dealingRound.BankList));
                    
                }
                else if (_dealingRound.BankList != null)
                    _dealingRound.BankList.Remove(_dealingRound.BankList[0]);
                EndGame();
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | SeparationBank: " + e.ToString());
            }

        }

        private void EndGame()
        {
            try
            {
                if (_timer != null)
                {
                    Timers.Stop(_timer);
                    _timer = null;
                }
                _timer = Timers.Start(1000 * time_show_winners, TimerShowWinner);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | EndGame: " + e.ToString());
            }
        }

        private void WaitNextRound()
        {
            try
            {

                if (_timer != null)
                {
                    Timers.Stop(_timer);
                    _timer = null;
                }
                if (_dealingRound.BankList != null && _dealingRound.BankList.Count > 0 && _dealingRound.BankList[0] > 0 && _winsPlayer != null && _winsPlayer.Count > 0)
                {
                    SeparationBank();
                    return;
                }
                tableState = false;
                clearTableData();
                foreach (var player in _players)
                {
                    SafeTrigger.ClientEvent(player._client, "poker:NewDealing");
                }
                _timer = Timers.Start(1000 * time_wait_newRound, TimerWaitNextRound);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | SendHandCardToPlayers: " + e.ToString());
            }
        }


        #region SendMessage

        private void SendHandCardToPlayers()
        {
            try
            {
                foreach (var player in _players)
                {
                    SafeTrigger.ClientEvent(player._client, "poker:playersCard", JsonConvert.SerializeObject(_queue), JsonConvert.SerializeObject(player.Cards));
                    if (player._client.HasData("ALLSEEINEYE") && player._client.GetData<bool>("ALLSEEINEYE"))
                        foreach (var play in _players)
                            SafeTrigger.ClientEvent(player._client, "poker:giveMyCards", play._place, JsonConvert.SerializeObject(play.Cards)); //named giveMyCards for conspiration
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | SendHandCardToPlayers: " + e.ToString());
            }
        }

        private void SendMoveOnPlayers(PokerPlayer target)
        {
            SendEventToMembers("poker:playerMove", target._move, target._place, target.Balance, target.IsOnDealing, target.currBet);
        }

        public void SendEventToMembers(string eventName, params object[] args)
        {
            try
            {
                foreach (var play in _players)
                    SafeTrigger.ClientEvent(play._client, eventName, args);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | AllFoldButPlayer: " + e.ToString());
            }
        }

        #endregion

        #region Timers

        private void TimerShowWinner()
        {
            WaitNextRound();
        }
        private void TimerWaitNextRound()
        {
            NewGame();
        }
        private void TimerWaitBetPlayer()
        {
            OnPlayerMadeBet(waitPlayer, BetType.Fold);
        }
        private void TimerBetweenRound()
        {
            StartBidRound(_dealingRound.currRound);
        }


        #endregion

        #region Counts

        private int CntIsOnDealingPlayers()
        {
            return _queue.Where(player => player.IsOnDealing).Count();
        }
        private void NextDealer()
        {
            try
            {
                int check = _dealer + 1;
                while (check != _dealer)
                {
                    var player = _players.FirstOrDefault(c => c._place == check);
                    if (player != null && player.Balance >= minBalanceFromGame)
                        break;
                    check++;
                    if (check >= _maxPlayers)
                        check = 0;
                }
                _dealer = check;
            }
            catch (Exception e)
            {
                _logger.WriteError($"Dealing | nextDealer: " + e.ToString());
            }
        }
        private int CntPlayersInBalance(int balance)
        {
            return _players.Where(player => player.Balance >= balance).Count();
        }
        private int CntPlayersInBalanceInDealing()
        {
            return _players.Where(player => player.IsOnDealing && player.Balance > 0).Count();
        }
        #endregion
    }
}