using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Core
{
    class Dimensions
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Dimensions));
        private static uint _dimensionPool = 10000000;
        public static uint RequestPrivateDimension()
        {          
            return --_dimensionPool;
        }
        public static uint RequestPrivateDimension(string log)
        {
            _logger.WriteDebug($"{log}: {_dimensionPool - 1}");
            return --_dimensionPool;
        }
    }
}
