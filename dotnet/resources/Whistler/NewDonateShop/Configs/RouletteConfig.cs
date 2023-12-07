using System;
using System.Collections.Generic;
using System.Text;
using Whistler.NewDonateShop.Enums;
using Whistler.NewDonateShop.Models;

namespace Whistler.NewDonateShop.Configs
{
    class RouletteConfig
    {
        public Dictionary<int, RouletteModel> Config { get; } = new Dictionary<int, RouletteModel>();
        public static Dictionary<ItemRarities, RarityConfigModel> RarityPrice { get; } = new Dictionary<ItemRarities, RarityConfigModel>
        {
            { ItemRarities.Base, new RarityConfigModel{ Price = 25, MinPrice = 0, MaxPrice = 500 } },
            { ItemRarities.Low, new RarityConfigModel{ Price = 50, MinPrice = 500, MaxPrice = 1500 } },
            { ItemRarities.Medium, new RarityConfigModel{ Price = 100, MinPrice = 1500, MaxPrice = 3000 } },
            { ItemRarities.Hight, new RarityConfigModel{ Price = 200, MinPrice = 3000, MaxPrice = 7000 } },
            { ItemRarities.Epic, new RarityConfigModel{ Price = 400, MinPrice = 7000, MaxPrice = 20000 } },
            { ItemRarities.Legend, new RarityConfigModel{ Price = 500, MinPrice = 20000, MaxPrice = 100000 } },
            { ItemRarities.NotDrop, new RarityConfigModel{ Price = 1000, MinPrice = 100000, MaxPrice = 10000000 } }
        };
        public RouletteConfig()
        {
            Add(0, "Game Demo", 500000, false, "rgba(255,255,255,0.3)", "blick", 0, 2000000);
            Add(1, "Standard", 200, true, "rgba(255,255,255,0.3)", "blick", 0, 2000000);
            Add(2, "Premium", 500, true, "#35F3FF", "blick", 200, 2000000);
            Add(3, "Luxe", 1500, true, "#EDCC6D", "blick", 1000, 2000000);
        }

        private void Add(int id, string name, int price, bool forCoins, string color, string effect, int minPrice, int maxPrice)
        {
            Config.Add(id, new RouletteModel(id, name, price, forCoins, color, effect, minPrice, maxPrice));
        }
    }
}
