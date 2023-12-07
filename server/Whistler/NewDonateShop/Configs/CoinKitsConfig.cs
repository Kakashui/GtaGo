using System;
using System.Collections.Generic;
using System.Text;
using Whistler.NewDonateShop.Models;

namespace Whistler.NewDonateShop.Configs
{
    class CoinKitsConfig2
    {
        public Dictionary<int, CoinKitConfigModel> Config { get; } = new Dictionary<int, CoinKitConfigModel>();

        public CoinKitsConfig2()
        {
            Add(0, 5000, 5500);
            Add(1, 10000, 11000);
            Add(2, 20000, 23000);
            Add(3, 30000, 35000);
            Add(4, 50000, 57000);

            //Add(0, 5000, 10500);
            //Add(1, 10000, 21000);
            //Add(2, 20000, 43000);
            //Add(3, 30000, 65000);
            //Add(4, 50000, 107000);
        }

        private void Add(int id, int price, int coins)
        {
           // Config.Add(id, new CoinKitConfigModel(id, price, coins));
        }
    }
}
