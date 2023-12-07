using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;

namespace Whistler
{
    static class WhistlerTask
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(WhistlerTask));
        public static void Run(Action action, int time = 0)
        {
            NAPI.Task.Run(() =>
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception e)
                {
                    _logger.WriteError($"WhistlerTask.Run: ${e}");
                }
            }, time);
        }
    }
}
