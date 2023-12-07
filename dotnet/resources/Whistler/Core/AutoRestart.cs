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
        private WhistlerLogger _logger = new WhistlerLogger(typeof(AutoRestart));
        private static Timer _timer;
        private static int _messageCount = 2;
        private const int _interval = 5;
        public const int RestartInHour = 7;
        private const int RestartInMinute = 30;

        public AutoRestart()
        {
            DateTime now = DateTime.Now;
            DateTime restartTime = DateTime.Today.AddDays(1).AddHours(RestartInHour).AddMinutes(RestartInMinute);
            TimeSpan restartIn = restartTime - now;
            _logger.WriteInfo($"Next restart in {restartIn.Hours} hours and {restartIn.Minutes} minutes (at {restartTime.Hour}:{restartTime.Minute})");

            int interval = _interval * 60 * 1000;
            uint delay = Convert.ToUInt32(restartIn.TotalMilliseconds - interval * _messageCount);
             _timer = new Timer(Handle, null, delay, interval);
        }

        private void Handle(object param)
        {
            if(_messageCount == 0)
            {
                NAPI.Task.Run(() =>
                {
                    Chat.AdminToAll("Происходит автоматический рестарт сервера.");
                    _logger.WriteInfo("System restart triggered");
                    Admin.ServerRestart("System", "autorestart");
                });
            }
            else
                NAPI.Task.Run(() => Chat.AdminToAll($"Сервер перезапустится через {_messageCount * _interval} минут."));

            _messageCount--;
        }
    }
}
