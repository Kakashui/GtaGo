using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GTANetworkAPI;
using Whistler.Casino.Dtos;
using Newtonsoft.Json;
using Whistler.SDK;
using ServerGo.Casino.ChipModels;
using ServerGo.Casino.Gamblers;
using ServerGo.Casino.Job;
using Object = GTANetworkAPI.Object;
using Trigger = Whistler.Trigger;
using Whistler;

namespace ServerGo.Casino.Games.Roulette
{
    /// <summary>
    /// The roulette game manager class
    /// </summary>
    internal class RouletteGame : BaseCasinoGame
    {
        public Object Template { get; set; }

        private const int MaxPlayers = 4;
        private Croupier _croupier;
        private BoardState _currentState;
        private Dictionary<Player, RoulettePlayer> _gameMembers;
        private int _bizId;
        private string _timer;
        public static int MaxWin { get; set; } = 5000000;
        private static int _maxIterations { get; set; } = 30;
        private static int _iterations { get; set; } = 0;


        public RouletteGame(Croupier croupier, int bizId) : base(CasinoGameType.Roulette)
        {
            if (croupier != null)
                _croupier = croupier;
            _bizId = bizId;
            _currentState = BoardState.WaitingForGamblers;
            _gameMembers = new Dictionary<Player, RoulettePlayer>();
        }

        private void Start()
        {
            //WhistlerTask.Run(TimerCallback, Constants.PlacingBetsTime * 1000);
            _timer = Timers.StartOnceTask(_bizId + "RouletteTimer" + Id, Constants.PlacingBetsTime * 1000, TimerCallback);
            _currentState = BoardState.WaitingForBets;
            foreach (var (player, roulettePlayer) in _gameMembers)
            {
                Trigger.ClientEvent(player, "player:startPlacingBets");
                Trigger.ClientEvent(player, "server:updateTimer", JsonConvert.SerializeObject(new BoardStateDto
                {
                    Name = _currentState.GetAttribute<DisplayAttribute>().Name,
                    Seconds = Constants.PlacingBetsTime
                }));
            }
        }

        private void End()
        {
            _currentState = BoardState.WaitingForGamblers;
            Timers.Stop(_timer);
        }

        public void OnPlayerDisconnect(Player player)
        {
            if (_gameMembers.ContainsKey(player))
                _gameMembers.Remove(player);
        }

        /// <summary>
        /// Every timer tick
        /// </summary>
        private void TimerCallback()
        {
            switch (_currentState)
            {
                case BoardState.WaitingForBets:
                    _currentState = BoardState.CalculatingWinner;
                    var winNumber = CalculateResult();
                    Timers.Stop(_timer);
                    _timer = Timers.StartOnceTask(_bizId + "RouletteTimer" + Id,
                        Constants.CalculatingResultTime * 1000, TimerCallback);
                    foreach (var (player, roulettePlayer) in _gameMembers)
                    {
                        Trigger.ClientEvent(player, "roulette:wheelAnim", true, winNumber);
                        Trigger.ClientEvent(player, "player:endPlacingBets");
                        Trigger.ClientEvent(player, "server:updateTimer", JsonConvert.SerializeObject(
                        new BoardStateDto
                        {
                            Name = _currentState.GetAttribute<DisplayAttribute>().Name,
                            Seconds = Constants.CalculatingResultTime
                        }));
                    }

                    return;
                case BoardState.CalculatingWinner:
                    _currentState = BoardState.ShowingResult;
                    Timers.Stop(_timer);
                    foreach (var (client, roulettePlayer) in _gameMembers)
                        roulettePlayer.NotifyAboutWinning(client);
                    _timer = Timers.StartOnceTask(_bizId + "RouletteTimer" + Id,
                        Constants.ShowingResultTime * 1000, TimerCallback);
                    foreach (var (client, roulettePlayer) in _gameMembers)
                    {
                        Trigger.ClientEvent(client, "roulette:wheelAnim", false);
                        Trigger.ClientEvent(client, "server:updateTimer", JsonConvert.SerializeObject(
                            new BoardStateDto
                            {
                                Name = _currentState.GetAttribute<DisplayAttribute>().Name,
                                Seconds = Constants.ShowingResultTime
                            }));
                        var bank = _gameMembers[client].Gambler.Bank;
                        Trigger.ClientEvent(client, "roulette:updatePlayerBank", bank.TotalValue,
                            JsonConvert.SerializeObject(CashBoxDto.CreateDto(bank.Chips)));
                    }

                    return;
                case BoardState.ShowingResult:
                    _currentState = BoardState.WaitingForBets;
                    Timers.Stop(_timer);

                    _timer = Timers.StartOnceTask(_bizId + "RouletteTimer" + Id,
                        Constants.PlacingBetsTime * 1000, TimerCallback);
                    foreach (var (player, roulettePlayer) in _gameMembers)
                    {
                        //Trigger.ClientEvent(player, "roulette:clearStats");
                        //foreach (var bet in roulettePlayer.Gambler.RouletteStats)
                        //    Trigger.ClientEvent(player, "roulette:sentStats",
                        //        JsonConvert.SerializeObject(bet)); //todo:fix
                        UpdateStats(player, roulettePlayer.Gambler.RouletteStats);
                        ClearBoard();
                        Trigger.ClientEvent(player, "player:startPlacingBets");
                        Trigger.ClientEvent(player, "server:updateTimer", JsonConvert.SerializeObject(
                            new BoardStateDto
                            {
                                Name = _currentState.GetAttribute<DisplayAttribute>().Name,
                                Seconds = Constants.PlacingBetsTime
                            }));
                        Trigger.ClientEvent(player, "roulette:updatePlayerBank",
                            _gameMembers[player].Gambler.Bank.TotalValue,
                            JsonConvert.SerializeObject(
                                CashBoxDto.CreateDto(_gameMembers[player].Gambler.Bank.Chips)));
                    }

                    return;
            }
        }

        private void UpdateStats(Player player, List<BetsStat> stats)
        {
            player.TriggerEvent("roulettes:stats:update", stats.TakeLast(6));
        }

        private int CalculateResult()
        {
            var winNumber = new Random().Next(0, 37);
            var helper = new RouletteHelper();
            int _maxWin = 0;
            foreach (var (player, roulettePlayer) in _gameMembers)
            {
                var totalWin = 0;
                foreach (var bet in roulettePlayer.Bets)
                {
                    var win = helper.GetWinning(winNumber, bet);
                    if (win.Chips.Count > 0)
                    {
                        totalWin += roulettePlayer.Gambler.Bank.Check(win.Chips);
                    }
                    if (_maxWin < totalWin) _maxWin = totalWin;
                }
            }
            if (_maxWin > MaxWin && _iterations < _maxIterations)  {
                _iterations++;
                return CalculateResult();
            }
            else
            {
                _iterations = 0;
                foreach (var (player, roulettePlayer) in _gameMembers)
                {
                    var winInDollars = 0;
                    foreach (var bet in roulettePlayer.Bets)
                    {
                        var win = helper.GetWinning(winNumber, bet);
                        if (win.Chips.Count > 0)
                        {
                            var betWin = roulettePlayer.Gambler.Bank.Charge(win.Chips);
                            winInDollars += betWin;
                        }
                    }
                    roulettePlayer.Winning = winInDollars;
                    if (roulettePlayer.Bets.Count == 0)
                        continue;// roulettePlayer.Gambler.RouletteStats.Add(new BetsStat(winNumber, "pass", 0));
                    else
                    {
                        if (winInDollars > 0)
                            roulettePlayer.Gambler.RouletteStats.Add(new BetsStat(winNumber, "win", (uint)winInDollars));
                        else roulettePlayer.Gambler.RouletteStats.Add(new BetsStat(winNumber, "lose", 0));
                    }
                }
            }

            return winNumber;
        }

        private void ClearBoard()
        {
            foreach (var gameMember in _gameMembers)
                gameMember.Value.ClearBets();
        }
        /// <summary>
        /// This method starts when player trying to sit at the table
        /// </summary>
        public void OnPlayerEnterGame(Player player, Gambler gambler)
        {
            if (_gameMembers.Count >= MaxPlayers || _gameMembers.ContainsKey(player)) return;
            _gameMembers.Add(player, new RoulettePlayer(gambler, FindFreeChair()));
            OnPlayerEnterGame(gambler);
            player.TriggerEvent("player:enterRoulette", Id, /*gambler.Bank.TotalValue,*/ _gameMembers[player].ChairIndex);
            if (_currentState == BoardState.WaitingForGamblers) Start();
            else Trigger.ClientEvent(_gameMembers.FirstOrDefault(g => g.Key != player).Key, 
                "roulette:requestTimerInfo");
            //Trigger.ClientEvent(player, "roulette:clearStats");
            //foreach (var bet in gambler.RouletteStats)
            //    Trigger.ClientEvent(player, "roulette:sentStats", JsonConvert.SerializeObject(bet));//todo:fix
            UpdateStats(player, gambler.RouletteStats);
        }

        private int FindFreeChair()
        {
            if (_gameMembers.Count == 0)// if sit first
                return 1;
            var occupied = new List<int>();
            foreach (var roulettePlayer in _gameMembers)
                occupied.Add(roulettePlayer.Value.ChairIndex);
            for (var i = 1; i <= 4; i++) 
                if (!occupied.Contains(i))//first
                    return i;
            
            return 1;
        }
        
        public void OnPlayerGetTimerInfo(int seconds)
        {
            foreach (var (client, roulettePlayer) in _gameMembers)
            {
                Trigger.ClientEvent(client, "server:updateTimer", JsonConvert.SerializeObject(new BoardStateDto
                {
                    Name = _currentState.GetAttribute<DisplayAttribute>().Name,
                    Seconds = seconds
                }));    
            }
        }
        
        /// <summary>
        /// This method starts when player exit table
        /// </summary>
        public void OnPlayerExitGame(Player player)
        {
            base.OnPlayerExitGame(_gameMembers[player].Gambler);
            _gameMembers.Remove(player);
            if (!_gameMembers.Any()) //if the last player exit
                End();
        }

        public void OnPlayerPlacedBets(Player player, string bet, IEnumerable<Chip> chips)
        {
            _gameMembers[player].MakeBet(bet, chips);
        }

        public void OnPlayerCanceledBet(Player player)
        {
            _gameMembers[player].CancelBet();
        }
    }
}