using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.PlayerEffects.Configs.Models
{
    public class HealTimer
    {
        public HealTimer(ExtPlayer player, int seconds, int amount)
        {
            _player = player;
            _amount = amount;
            _expiried = DateTime.Now.AddSeconds(seconds);
            _timerId = Timers.Start(2500, Handle);
        }

        private string _timerId;
        private ExtPlayer _player;
        private int _amount;
        private DateTime _expiried;

        public void ChangeEffect(int seconds, int amount)
        {
            _amount = amount;
            _expiried = DateTime.Now.AddSeconds(seconds);
        }

        internal void Destroy()
        {
            Timers.Stop(_timerId);
            _player?.ResetData("effect:heal");
        }

        private void Handle()
        {
            if(!_player.IsLogged() || _expiried < DateTime.Now)
                Destroy();
            else if(_player.Health + _amount < 100)
                _player.Health += _amount;
        }
    }
}
