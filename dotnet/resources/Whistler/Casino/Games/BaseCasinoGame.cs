using System;
using GTANetworkAPI;
using Whistler.Core;
using ServerGo.Casino.Business;
using ServerGo.Casino.Gamblers;
using ServerGo.Casino.Games.Poker;
using ServerGo.Casino.Games.Roulette;
using Whistler.SDK;

namespace ServerGo.Casino.Games
{
    /// <summary>
    /// Abstract class represents API for different casino games
    /// </summary>
    public abstract class BaseCasinoGame
    {
        public int Id { get; set; }
        public CasinoGameType Game { get; }
        
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public uint Dimension { get; set; }

        protected BaseCasinoGame(CasinoGameType gameType)
        {
            Game = gameType;
        }
        protected void OnPlayerEnterGame(Gambler gambler)
        {
            gambler.StartGame(this);
        }

        protected void OnPlayerExitGame(Gambler gambler)
        {
            gambler.CancelGame();
        }
    }

    public enum CasinoGameType
    {
        Roulette,
        Poker
    }

    internal class CasinoGameFactory
    {
        private int _lastId;
        private int _bizId;
        public CasinoGameFactory(int bizId)
        {
            _bizId = bizId;
        }
        public BaseCasinoGame Create(CasinoGameType gameType, Vector3 position, uint dimension, Vector3 rotation = null, int smallBlind = 0, int startBalance = 0)
        {
            BaseCasinoGame game = null;
            switch (gameType)
            {
                case CasinoGameType.Roulette:
                    game = new RouletteGame(null, _bizId);
                    break;
                case CasinoGameType.Poker:
                    game = new PokerGame(position, smallBlind, startBalance);
                    break;
            }
            if (game == null) return null;
            game.Id = _lastId;
            game.Position = position;
            game.Rotation = rotation;
            game.Dimension = dimension;


            CreateColShape(game.Id, game.Position, dimension);
            _lastId++;
            return game;
        }

        private void CreateColShape(int gameId, Vector3 position, uint dimension)
        {
            InteractShape.Create(position, 3, 3, dimension)
                .AddInteraction((player) =>
                {
                    SafeTrigger.SetData(player, "CASINOGAME_ID", gameId);

                    CasinoManager.FindCasinoByBizId(_bizId)
                        .OnPlayerGamePressed(player);
                });
        }

        internal BaseCasinoGame Create(CasinoGameType roulette, object ninthRouletteTablePoint, uint dimension, Vector3 vector3)
        {
            throw new NotImplementedException();
        }
    }
}