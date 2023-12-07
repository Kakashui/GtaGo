using GTANetworkAPI;
using Whistler.Fishing.Models;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.Helpers;
using Whistler.Entities;

namespace Whistler.Fishing
{
    public class MiniGame
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(MiniGame));
        public MiniGame(ExtPlayer client)
        {
            _client = client;
            Active = false;
        }
        internal bool Active { get; private set; }
        private readonly ExtPlayer _client;
        internal int Fish { get; private set; }
        private int _difficult;


        internal void Start(FishingSpot spot, FisherData fisher, Rod rod)
        {
            Active = true;
            if (!spot.InSea) _client.Rotation = spot.Rotation;
            _client.SetSharedData(Const.DATA_ACTIVE, 1);
            //_client.PlayAnimation(Const.ANIM_DICT, Const.ANIM_NAME, 38);
            GenerateNewGame(fisher, rod, spot.InSea);
        }

        internal void Stop()
        {
            Active = false;
            _client.StopAnimation();
            _client.SetSharedData(Const.DATA_ACTIVE, 0);
        }

        private void GenerateNewGame(FisherData fisher, Rod rod, bool inSea)
        {
            try
            {
                var delay = FishingAPI.Random.Next(Const.TIME_WAIT_FISH_MIN, Const.TIME_WAIT_FISH_MAX);

                Fish = FishingAPI.Random.Next(0 + fisher.Lvl / 2, 13 + fisher.Lvl); //<< important max not should be more than enum Fish
                _difficult = Math.Min(Math.Max(0, Fish - rod.GetPower() - fisher.Lvl / 2), 11);
                NAPI.Task.Run(() =>
                {
                    if (!Active) return;
                    SafeTrigger.ClientEvent(_client, Const.CLIENT_EVENT_MINI_GAME_START, _difficult);
                    SafeTrigger.SetSharedData(_client, Const.DATA_ACTIVE, 2);
                }, delay * 1000);
            }
            catch (Exception e) { _logger.WriteError("GenerateNewGame: " + e.ToString()); }
        }
    }
}
