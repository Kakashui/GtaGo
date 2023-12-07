using GTANetworkAPI;
using Whistler.Fishing.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Fishing
{
    class FishingTraders
    {      
        public static List<Trader> FishShops { get; set; } = new List<Trader>
        {
            new Trader(PedHash.OldMan2, "Antonio", "Торговец рыбой", 371 , 25, "Скупка рыбы", new Vector3(1466.631, 6551.961, 13.96011), new Vector3(0, 0, 73.18819))
        };
    }
}
