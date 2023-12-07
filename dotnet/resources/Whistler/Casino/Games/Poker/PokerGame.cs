using System.Collections.Generic;
using System;
using GTANetworkAPI;
using Whistler;
using ServerGo.Casino.Gamblers;
using Whistler.SDK;
using Newtonsoft.Json;
using Whistler.Casino.Dtos;
using System.Linq;
using Whistler.Helpers;
using Whistler.Entities;

namespace ServerGo.Casino.Games.Poker
{
    internal class PokerGame : BaseCasinoGame
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(PokerGame));
        private const int MaxPlayers = 6;
        private const int MinPlayers = 2;
        internal int SmallBlind;
        internal int MinBalanceStartGame;

        private Dictionary<ExtPlayer, PokerPlayer> _gameMembers;
        private Dealing _dealing;
        public PokerGame(Vector3 pos, int smallBlind, int startBalance) : base(CasinoGameType.Poker)
        {
            _gameMembers = new Dictionary<ExtPlayer, PokerPlayer>();
            SmallBlind = smallBlind;
            MinBalanceStartGame = startBalance;
            _dealing = new Dealing(SmallBlind, MinPlayers, MaxPlayers);
        }

        /// <summary>
        /// This method starts when player trying to sit at the table
        /// </summary>
        public void OnPlayerEnterGame(ExtPlayer player, Gambler gambler)//todo: withdraw this chips
        {
            try
            {
                int clearplace = getClearPlace();
                if (clearplace < 0 || _gameMembers.ContainsKey(player)) return;
                if (gambler.Bank.TotalValue < MinBalanceStartGame)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter,
                        "Недостаточно фишек для игры за этим столом!", 4000);
                    return;
                }
                base.OnPlayerEnterGame(gambler);
                _gameMembers.Add(player, new PokerPlayer(player, gambler, clearplace, MinBalanceStartGame));
                SafeTrigger.ClientEvent(player, "poker:enterPoker", Id, _gameMembers[player].FullBank, JsonConvert.SerializeObject(_gameMembers.Values), _gameMembers[player]._place, SmallBlind);
                foreach (var play in _gameMembers.Keys)
                {
                    if (play != null && play != player) //себе не отправляем
                        SafeTrigger.ClientEvent(play, "poker:newPlayer", JsonConvert.SerializeObject(_gameMembers[player]));
                }
                SafeTrigger.ClientEvent(player, "poker:TableCard", JsonConvert.SerializeObject(_dealing._tableCards));
                _dealing._players.Add(_gameMembers[player]);
                _dealing.startTimerNextGame();
            }
            catch (Exception e)
            {
                _logger.WriteError($"PokerGame | OnPlayerEnterGame: " + e.ToString());
            }
        }

        /// <summary>
        /// This method starts when player exit table
        /// </summary>
        public void OnPlayerExitGame(ExtPlayer player, Gambler gambler)
        {
            try
            {
                var pokerPlayer = _gameMembers[player];
                gambler.Bank.Reset();
                int ostatok = gambler.Bank.ConvertMoneyToChips(pokerPlayer.FullBank + pokerPlayer.Balance);
                var bank = gambler.Bank;
                SafeTrigger.ClientEvent(player, "roulette:updatePlayerBank", bank.TotalValue, JsonConvert.SerializeObject(CashBoxDto.CreateDto(bank.Chips)));

                _dealing.RemovePlayer(pokerPlayer);

                base.OnPlayerExitGame(pokerPlayer.Gambler);
                _gameMembers.Remove(player);

            }
            catch (Exception e)
            {
                _logger.WriteError($"PokerGame | OnPlayerExitGame: " + e.ToString());
            }
        }

        public void OnPlayerPlacedBets(ExtPlayer player, string bet, int amount)
        {
            try
            {
                if (!_gameMembers.ContainsKey(player)) return;
                switch (bet)
                {
                    case nameof(BetType.Call):
                        _dealing.OnPlayerMadeBet(_gameMembers[player], BetType.Call);
                        break;
                    case nameof(BetType.Check):
                        _dealing.OnPlayerMadeBet(_gameMembers[player], BetType.Check);
                        break;
                    case nameof(BetType.Raise):
                        _dealing.OnPlayerMadeBet(_gameMembers[player], BetType.Raise, amount);
                        break;
                    case nameof(BetType.Fold):
                        _dealing.OnPlayerMadeBet(_gameMembers[player], BetType.Fold);
                        break;
                    case nameof(BetType.AllIn):
                        _dealing.OnPlayerMadeBet(_gameMembers[player], BetType.AllIn, amount);
                        break;
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"PokerGame | OnPlayerPlacedBets: " + e.ToString());
            }
        }

        public void OnPlayerBuyChips(ExtPlayer player, int amount)
        {
            try
            {
                if (!_gameMembers.ContainsKey(player)) return;
                if (amount <= 0) return;
                var target = _gameMembers[player];
                if (target.FullBank >= amount)
                {
                    target.ChangeBalance(amount);
                    target.ChangeFullBank(-amount);
                    _dealing.SendEventToMembers("poker:changeBank", target._place, target.Balance, target.FullBank, target.currBet);
                    _dealing.startTimerNextGame();
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"PokerGame | OnPlayerPlacedBets: " + e.ToString());
            }
        }

        private int getClearPlace()
        {
            for (int i = 0; i < MaxPlayers; i++)
            {
                if (_gameMembers.Values.FirstOrDefault(c => c._place == i) == null)
                    return i;
            }
            return -1;
        }
        public void OnPlayerChangeImage(ExtPlayer player, string image)
        {
            try
            {
                if (!_gameMembers.ContainsKey(player)) return;
                _dealing.SendEventToMembers("poker:changeImage", _gameMembers[player]._place, image);
            }
            catch (Exception e)
            {
                _logger.WriteError($"PokerGame | OnPlayerPlacedBets: " + e.ToString());
            }
        }

    }
}