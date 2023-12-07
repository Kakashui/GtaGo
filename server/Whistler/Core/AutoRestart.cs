using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Core
{
    class AutoRestart
    {
        WhistlerLogger _logger = new WhistlerLogger(typeof(AutoRestart));
        private static Timer _timer;
        private static int _messageCount = 3;
        private const int _interval = 15;
        public const int RestartInHour = 5;
        public AutoRestart()
        {
            DateTime now = DateTime.Now;
            var startTime = new DateTime(now.Year, now.Month, now.Day, RestartInHour, 0, 0, 0);
            if (now.Hour >= RestartInHour - 1)
                startTime = startTime.AddDays(1);
            var restartIn = startTime - now;

            _logger.WriteInfo($"Next restart in {restartIn.Hours} hours {restartIn.Minutes} minutes");

            var interval = _interval * 60 * 1000;
            var delay = Convert.ToUInt32(restartIn.TotalMilliseconds - interval * _messageCount);
             _timer = new Timer(Handle, null, delay, interval);
        }

        private void Handle(object param)
        {
            if(_messageCount == 0)
            {
                WhistlerTask.Run(() =>
                {
                    Chat.AdminToAll("sys:rest:now");
                    Admin.ServerRestart("System", "autorestart");
                });
            }
            else
                WhistlerTask.Run(() => Chat.AdminToAll("sys:rest:in".Translate(_messageCount * _interval)));

            _messageCount--;
        }
    }
}
