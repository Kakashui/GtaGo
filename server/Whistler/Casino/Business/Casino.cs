using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using GTANetworkAPI;
using Whistler;
using Whistler.Casino.Dtos;
using Whistler.Core.Character;
using Whistler.MoneySystem;
using Newtonsoft.Json;
using ServerGo.Casino.ChipModels;
using ServerGo.Casino.Games;
using ServerGo.Casino.Gamblers;
using ServerGo.Casino.Games.Poker;
using ServerGo.Casino.Games.Roulette;
using Whistler.Core;
using Whistler.SDK;
using Player = GTANetworkAPI.Player;
using Trigger = Whistler.SDK.Trigger;
using Whistler.Helpers;

namespace ServerGo.Casino.Business
{
    /// <summary>
    /// Whistler business system casino
    /// </summary>
    internal class Casino
    {
        public int BizId => _bizData.ID;

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Casino));
        private Whistler.Core.Business _bizData;
        private CasinoRoom _room;
        private Dictionary<Player, Gambler> _gamblers;
        private List<BaseCasinoGame> _games;
        private CasinoGameFactory _factory;
        public CashBox CashBox { get; set; }


        public Casino(Whistler.Core.Business biz)
        {
            _bizData = biz;
            _room = new CasinoRoom(BizId);
            _gamblers = new Dictionary<Player, Gambler>();
            CashBox = new CashBox(0);
            _games = new List<BaseCasinoGame>();
            _factory = new CasinoGameFactory(BizId);            
            InitGames();
            LoadTaxesFromDb();
            NAPI.TextLabel.CreateTextLabel(Main.StringToU16("~g~Press E to buy chips"), new Vector3(CasinoManager.CashBoxPoint.X, CasinoManager.CashBoxPoint.Y, CasinoManager.CashBoxPoint.Z + 1), 5F, 0.3F, 0, new Color(255, 255, 255));
            Init();
        }

        private void Init()
        {
             InteractShape.Create(new Vector3(928.6951, 39.64806,79.97574), 2, 3)
                 .AddDefaultMarker()
                 .AddInteraction(OnPlayerTriedEnter);
            
             InteractShape.Create(CasinoManager.RoomExitPoint, 2, 3)
                 .AddDefaultMarker()
                 .AddInteraction(OnPlayerTriedExit);
        }

        private void LoadTaxesFromDb()
        {
            var table = MySQL.QueryRead($"SELECT * FROM casino WHERE id = 0");
            foreach (DataRow Row in table.Rows)
            {
                CasinoManager.StateShare = Convert.ToDouble(Row["stateTax"]) / 100;
                CasinoManager.CasinoShare = Convert.ToDouble(Row["casinoTax"]) / 100;
            }
        }

     
        private void InitGames()
        {
            foreach (var table in CasinoManager.PokerTablePoint)
                _games.Add(_factory.Create(CasinoGameType.Poker, table.Item1, _room.Dimension, table.Item2, table.Item3, table.Item4));
            foreach (var table in CasinoManager.RouletteTablePoint)
                _games.Add(_factory.Create(CasinoGameType.Roulette, table.Item1, _room.Dimension, table.Item2, table.Item3, table.Item4));
            foreach (var table in CasinoManager.PrivatePokerTablePoint)
                _games.Add(_factory.Create(CasinoGameType.Poker, table.Item1, table.Item5, table.Item2, table.Item3, table.Item4));
            foreach (var table in CasinoManager.PrivateRouletteTablePoint)
                _games.Add(_factory.Create(CasinoGameType.Roulette, table.Item1, table.Item5, table.Item2, table.Item3, table.Item4));
        }

        public void OnPlayerTriedEnter(Player player)
        {
            if (player.GetCharacter().LVL < CasinoManager.MinPlayerLvlToEnter)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter,
                    "Cas_2".Translate(CasinoManager.MinPlayerLvlToEnter), 3000);
                return;
            }

            if (player.GetCharacter().Money < CasinoManager.MinPlayerBalanceToEnter)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter,
                    "Cas_3", 3000);
                return;
            }
            _room.LetPlayerIn(player);
            if (_gamblers.TryGetValue(player, out var gambler))
                Trigger.ClientEvent(player, "showCasinoHud",
                    JsonConvert.SerializeObject(CashBoxDto.CreateDto(gambler.Bank.Chips)));
        }

        public void LoadBoardObjects(Player player)
        {
            Trigger.ClientEvent(player, "player:loadedRoulleteBoard", JsonConvert.SerializeObject(_games.Select(game => new { id = game.Id, position = game.Position, rotation = game.Rotation, message = GetMessage(game), dimension = game.Dimension, type = game.Game.ToString() })));
        }

        private string GetMessage(BaseCasinoGame casino)
        {
            if (casino.Game == CasinoGameType.Poker)
                return $"~g~Blind {(casino as PokerGame).SmallBlind}/{(casino as PokerGame).SmallBlind * 2}, Min balance: {(casino as PokerGame).MinBalanceStartGame}";
            return "";
        }

        public void OnPlayerTriedExit(Player player)
        {
            //player is gambler and has unsold chips
            //if (_gamblers.TryGetValue(player, out var gambler))
            //{
            //    Trigger.ClientEvent(player, "openDialog",
            //        "CASINO_EXIT", "Cas_4");
            //}
            //else
            Trigger.ClientEvent(player, "hideCasinoHud");
            _room.LetPlayerOut(player);
        }

        public void AddGambler(Player player, int[] casinoChips)
        {   
            var chipList = new List<Chip>();
            for (var i = 0; i < 5; i++)
                for (var j = 0; j < casinoChips[i]; j++)
                {
                    chipList.Add(ChipFactory.Create((ChipType)i));
                }
            lock (_gamblers)
            {
                if(!_gamblers.ContainsKey(player)) _gamblers.Add(player, new Gambler(player, chipList));
            }            

            Trigger.ClientEvent(player, "showCasinoHud",
                JsonConvert.SerializeObject(CashBoxDto.CreateDto(_gamblers[player].Bank.Chips)));
            
        }
        public void OnGamblerExit(Player player)
        {
            _gamblers.Remove(player);
            Trigger.ClientEvent(player, "hideCasinoHud");
            _room.LetPlayerOut(player);
        }

        public void AddChips(Player player, List<Chip> chips)
        {
            if (!_gamblers.ContainsKey(player))
            {   //if owner first time playing in this casino
                _gamblers.Add(player, new Gambler(player));
                player.GetCharacter().CasinoChips = new int[5];
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter,"Cas_5".Translate(_gamblers[player].Bank.TotalValue), 3000);
            }
            _gamblers[player].Bank.Charge(chips); 
            player.TriggerEvent("showCasinoHud", JsonConvert.SerializeObject(CashBoxDto.CreateDto(_gamblers[player].Bank.Chips)));
        }

        public void OnPlayerBoughtChips(Player player, CashBoxDto data)
        {
            var totalCost = 0;
            foreach (var chip in data.CreateChipList())
                totalCost += chip.Value;
            if (player.GetCharacter().Money < totalCost) return;
            if (!_gamblers.ContainsKey(player) && data.TotalCount > 0)
            {   //if owner first time playing in this casino
                _gamblers.Add(player, new Gambler(player));                
                player.GetCharacter().CasinoChips = new int[5];
                totalCost = _gamblers[player].Bank.Charge(data.CreateChipList());
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Cas_5".Translate( _gamblers[player].Bank.TotalValue), 3000);
                //Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter,
                  //  "Cas_6", 4000);
            }
            else
                totalCost = _gamblers[player].Bank.Charge(data.CreateChipList());
            Wallet.MoneySub(player.GetCharacter(), totalCost, "Money_BuyChips");
            CashBox.Charge(totalCost, player);

            Trigger.ClientEvent(player, "showCasinoHud", JsonConvert.SerializeObject(CashBoxDto.CreateDto(_gamblers[player].Bank.Chips)));
        }

        public void OnPlayerSoldChips(Player player)
        {
            if (!_gamblers.TryGetValue(player, out var gambler)) return;

            Wallet.MoneyAdd(player.GetCharacter(), gambler.Bank.SoldValue, "Money_SoldChips");
            CashBox.Withdraw(gambler.Bank.SoldValue, player);
            gambler.Bank.Reset();

            if (CashBox.Amount < 0)
            {   //if the owner online
                if (_bizData.OwnerID > 0 && Main.GetPlayerByUUID(_bizData.OwnerID) != null)
                    Notify.Send(Main.GetPlayerByUUID(_bizData.OwnerID), NotifyType.Info, NotifyPosition.BottomCenter, "Cas_7", 3000);
            }

            Trigger.ClientEvent(player, "hideCasinoHud");
        }

        #region RageInteractions
        public void OnPlayerCashBoxPressed(Player player)
        {
            OnPlayerLeftGame(player);
            var playerBank = 0;
            if (_gamblers.TryGetValue(player, out var gambler))
                playerBank = gambler.Bank.SoldValue;
            Trigger.ClientEvent(player, "openCashbox", playerBank);
        }

        public void OnPlayerGamePressed(Player player)
        {
            if (!_gamblers.TryGetValue(player, out var gambler))
            {
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter,
                    "Cas_8", 3000);
                return;
            }
            var gameId = player.GetData<int>("CASINOGAME_ID");
            (_games[gameId] as RouletteGame)?.OnPlayerEnterGame(player, gambler);
            (_games[gameId] as PokerGame)?.OnPlayerEnterGame(player, gambler);
        }

        public void OnPlayerLeftGame(Player player)
        {
            if (!_gamblers.TryGetValue(player, out var gambler)) return;
            if (gambler.ActingGame == null) return;
            switch (_games[gambler.ActingGame.Id].Game)
            {
                case CasinoGameType.Roulette:
                    (_games[gambler.ActingGame.Id] as RouletteGame)?.OnPlayerExitGame(player);
                    return;
                case CasinoGameType.Poker:
                    (_games[gambler.ActingGame.Id] as PokerGame)?.OnPlayerExitGame(player, gambler);
                    return;
            }
        }

        public void OnPlayerDisconnected(Player player)
        {
            _gamblers.Remove(player);
            foreach (var game in _games)
                (game as RouletteGame)?.OnPlayerDisconnect(player);
        }
        public void OnPlayerPlacedBets(Player player, string bet, IEnumerable<Chip> chips)
        {
            if (!_gamblers.TryGetValue(player, out var gambler)) return;
            
            if (!_gamblers[player].Bank.Verify(chips)) return;
            _gamblers[player].Bank.Withdraw(chips);
            
            switch (_games[gambler.ActingGame.Id].Game)
            {
                case CasinoGameType.Roulette:
                    (_games[gambler.ActingGame.Id] as RouletteGame)?.OnPlayerPlacedBets(player, bet, chips);
                    return;
                case CasinoGameType.Poker:
                    (_games[gambler.ActingGame.Id] as PokerGame)?.OnPlayerPlacedBets(player, bet, 0);
                    return;
            }
            Trigger.ClientEvent(player, "roulette:updatePlayerBank", gambler.Bank.TotalValue, JsonConvert.SerializeObject(CashBoxDto.CreateDto(gambler.Bank.Chips)));
        }

        public void OnPlayerPokerPlacedBets(Player player, string bet, int amount)
        {
            try
            {
                if (!_gamblers.TryGetValue(player, out var gambler)) return;
                switch (_games[gambler.ActingGame.Id].Game)
                {
                    case CasinoGameType.Poker:
                        (_games[gambler.ActingGame.Id] as PokerGame)?.OnPlayerPlacedBets(player, bet, amount);
                        return;
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | OnPlayerPokerPlacedBets: " + e.ToString());
            }
        }
        public void OnPlayerPokerBuyChips(Player player, int amount)
        {
            try
            {
                if (!_gamblers.TryGetValue(player, out var gambler)) return;
                switch (_games[gambler.ActingGame.Id].Game)
                {
                    case CasinoGameType.Poker:
                        (_games[gambler.ActingGame.Id] as PokerGame)?.OnPlayerBuyChips(player, amount);
                        return;
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | OnPlayerPokerPlacedBets: " + e.ToString());
            }
        }

        public void OnPlayerSentTimerInfo(Player player, TimerDto dto)
        {
            if (!_gamblers.TryGetValue(player, out var gambler)) return;
            switch (_games[gambler.ActingGame.Id].Game)
            {
                case CasinoGameType.Roulette:
                    (_games[gambler.ActingGame.Id] as RouletteGame)?.OnPlayerGetTimerInfo(dto.Seconds);
                    return;
            }
        }
        #endregion
        public void OnOwnerLostBusiness()
        {
            _bizData.TakeBusinessFromOwner(Convert.ToInt32(_bizData.SellPrice * 0.8), "Money_BizWithdrawalCasino");
            CashBox.Reset();
        }

        public void OnPlayerCanceledBet(Player player)
        {
            if (!_gamblers.TryGetValue(player, out var gambler)) return;
            switch (_games[gambler.ActingGame.Id].Game)
            {
                case CasinoGameType.Roulette:
                    (_games[gambler.ActingGame.Id] as RouletteGame)?.OnPlayerCanceledBet(player);
                    return;
                case CasinoGameType.Poker:
                    
                    return;
            }
            Trigger.ClientEvent(player, "roulette:updatePlayerBank", gambler.Bank.TotalValue, JsonConvert.SerializeObject(CashBoxDto.CreateDto(gambler.Bank.Chips)));
        }

        public void OnPlayerRequestedBank(Player player)
        {
            if (!_gamblers.TryGetValue(player, out var gambler)) return;
            player.TriggerEvent("roulette:updatePlayerBank", gambler.Bank.TotalValue, JsonConvert.SerializeObject(CashBoxDto.CreateDto(gambler.Bank.Chips)));
        }
        public void OnPlayerChangeImage(Player player, string image)
        {
            try
            {
                if (!_gamblers.TryGetValue(player, out var gambler)) return;
                switch (_games[gambler.ActingGame.Id].Game)
                {
                    case CasinoGameType.Poker:
                        (_games[gambler.ActingGame.Id] as PokerGame)?.OnPlayerChangeImage(player, image);
                        return;
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"Casino | OnPlayerPokerPlacedBets: " + e.ToString());
            }
        }
    }
}