using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Whistler.Entities;
using Whistler.GUI.Documents.Enums;
using Whistler.Helpers;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.NewDonateShop.Enums;
using Whistler.NewDonateShop.Models;
using Whistler.PriceSystem;
using Whistler.ServerConfiguration;
using Whistler.VehicleSystem;

namespace Whistler.NewDonateShop.Configs
{
    public class ItemsConfig
    {
        Dictionary<int, ItemModel> All { get; } = new Dictionary<int, ItemModel>();
        private Dictionary<ItemRarities, List<ItemModel>> _cache = new Dictionary<ItemRarities, List<ItemModel>>();
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ItemsConfig));
        private List<ItemModel> _sortedByPrice = null;
        internal List<ItemModel> SortedByPrice
        {
            get
            {
                if (_sortedByPrice == null)
                {
                    _sortedByPrice = All.Values.OrderBy(i => i.Price).ToList();
                }
                return _sortedByPrice;
            }
        }
        public ItemsConfig()
        {
            // AddVehicle(1000, "carsFrame347", "sabregt2", false);

            // #region Prime Accounts (VIP)
            // Add(1000, "Frame639", "1 day", 100, ItemRarities.Base, ItemCategories.Others, true, new PrimeDonateItem(1), weight: 30);
            // Add(1001, "Frame640", "2 days", 120, ItemRarities.Base, ItemCategories.Others, true, new PrimeDonateItem(2), weight: 20);
            // Add(1002, "Frame641", "3 days", 150, ItemRarities.Medium, ItemCategories.Others, true, new PrimeDonateItem(3), weight: 10);
            // Add(1003, "Frame642", "5 days", 250, ItemRarities.Medium, ItemCategories.Others, true, new PrimeDonateItem(5));
            // Add(1004, "Frame643", "8 days", 350, ItemRarities.Hight, ItemCategories.Others, true, new PrimeDonateItem(8));
            // Add(1005, "Frame644", "10 days", 500, ItemRarities.Hight, ItemCategories.Others, true, new PrimeDonateItem(10));
            // Add(1006, "Frame645", "15 days", 750, ItemRarities.Legend, ItemCategories.Others, true, new PrimeDonateItem(15));
            // Add(1007, "Frame772", "30 days", 1500, ItemRarities.Epic, ItemCategories.Others, true, new PrimeDonateItem(30));
            // Add(1008, "Frame646", "100 days", 5000, ItemRarities.NotDrop, ItemCategories.Others, true, new PrimeDonateItem(100));
            // #endregion

            // #region Experience (EXP)
            // //Exp
            // Add(1500, "Frame634", "1 EXP", 30, ItemRarities.Base, ItemCategories.Others, true, new ExpirianceDonateItem(1), weight: 20);
            // Add(1501, "Frame635", "3 EXP", 50, ItemRarities.Base, ItemCategories.Others, true, new ExpirianceDonateItem(3), weight: 10);
            // Add(1502, "Frame636", "6 EXP", 70, ItemRarities.Base, ItemCategories.Others, true, new ExpirianceDonateItem(6));
            // Add(1503, "Frame637", "9 EXP", 80, ItemRarities.Base, ItemCategories.Others, true, new ExpirianceDonateItem(9));
            // Add(1504, "Frame638", "12 EXP", 100, ItemRarities.Medium, ItemCategories.Others, true, new ExpirianceDonateItem(12));
            // #endregion

            // #region Money
            // //Money
            // AddMoney(2000, 5000, 10000, true, weight: 15);
            // AddMoney(2001, 10000, 25000, true, weight: 10);
            // AddMoney(2002, 25000, 50000, true, weight: 8);
            // AddMoney(2003, 50000, 75000, true, weight: 7);
            // AddMoney(2004, 75000, 100000, true, weight: 5);
            // AddMoney(2005, 100000, 200000, true, weight: 4);
            // AddMoney(2006, 200000, 400000, true, weight: 2);
            // AddMoney(2007, 400000, 1000000, true);
            // AddMoney(2008, 1000000, 2000000, true);
            // AddMoney(2008, 1000000, 2000000, false);
            // //AddMoney(2009, 2000000, 4000000, true);

            // #endregion

            // #region GoCoin
            // AddCoins(2500, 50, 150, true, weight: 4);
            // AddCoins(2501, 150, 250, true, weight: 3);
            // AddCoins(2502, 250, 500, true, weight: 2);
            // AddCoins(2503, 500, 1000, true);
            // AddCoins(2504, 1000, 2000, true);
            // AddCoins(2505, 2000, 4000, true);
            // AddCoins(2506, 5000, 10000, true);
            // #endregion

            // #region Bag
            // Add(4000, "bagsFrame547", "GUCCI Blue Bag", 3000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 509, 0, true));
            // Add(4001, "bagsFrame531", "GUCCI Silver Bag", 3000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 509, 1, true));
            // Add(4002, "bagsFrame623", "Nike Big Bag", 2500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 509, 6, true));
            // Add(4003, "bagsFrame603", "Nike Military Bag", 2700, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 509, 7, true));
            // Add(4004, "bagsFrame535", "Georgia Big Bag", 1000, ItemRarities.Hight, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLight, 509, 11, true));
            // Add(4005, "bagsFrame686", "Georgia Small Bag", 500, ItemRarities.Hight, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLight, 500, 3, true));
            // Add(4006, "bagsFrame702", "Russian Small Bag", 500, ItemRarities.Hight, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLight, 500, 4, true));
            // Add(4007, "bagsFrame762", "Simpsons Small Bag", 1500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 500, 8, true));
            // Add(4008, "bagsFrame778", "Pony Small Bag", 800, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 500, 9, true));
            // Add(4009, "bagsFrame646", "Dora Medium Bag", 1000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 502, 5, true));
            // Add(4010, "bagsFrame662", "Adidas Silver Bag", 1000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 502, 6, true));
            // Add(4011, "bagsFrame678", "Adidas Black Bag", 1000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 502, 7, true));
            // Add(4012, "bagsFrame742", "Naruto Bag", 1000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 502, 11, true));
            // Add(4013, "bagsFrame754", "Stone Island Bag", 1500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 502, 12, true));
            // Add(4014, "bagsFrame539", "Georgian Bag", 2000, ItemRarities.Hight, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLight, 510, 1, true));
            // Add(4015, "bagsFrame734", "3 Cat Bag", 900, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 500, 6, true));
            // Add(4016, "bagsFrame750", "Heart Small Bag", 900, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 500, 7, true));
            // Add(4017, "bagsFrame658", "Cat Small Bag", 700, ItemRarities.Hight, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackMedium, 500, 11, true));
            // Add(4018, "bagsFrame483", "Nike Green Bag", 1300, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 509, 4, true));
            // Add(4019, "bagsFrame466", "Nike Blue Bag", 1300, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 509, 5, true));
            // Add(4020, "bagsFrame519", "BMW Blue Bag", 1500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 509, 12, true));
            // Add(4021, "bagsFrame503", "BMW Red Bag", 1500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 509, 13, true));
            // Add(4022, "bagsFrame487", "BMW White Bag", 1500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 509, 14, true));
            // Add(4023, "bagsFrame471", "Military Green Bag", 700, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 509, 15, true));
            // Add(4024, "bagsFrame627", "Military White Bag", 700, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 509, 16, true));
            // Add(4025, "bagsFrame607", "Military Brown Bag", 700, ItemRarities.Hight, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackMedium, 509, 17, true));
            // Add(4026, "bagsFrame587", "Military Silver Bag", 700, ItemRarities.Hight, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackMedium, 509, 18, true));
            // Add(4027, "bagsFrame766", "Just Do It Bag", 1200, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 502, 3, true));
            // Add(4028, "bagsFrame782", "Dora White Bag", 1000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 502, 4, true));
            // Add(4029, "bagsFrame694", "Square Bag", 500, ItemRarities.Hight, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackMedium, 502, 8, true));
            // Add(4030, "bagsFrame710", "Alcoste Bag", 1500, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLarge, 502, 9, true));
            // Add(4031, "bagsFrame726", "SUPREME Purple Bag", 2500, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 502, 10, true));
            // Add(4032, "bagsFrame770", "GTA V Police Bag", 1000, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLarge, 502, 13, true));
            // Add(4033, "bagsFrame786", "GTA V Dog Bag", 1000, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLarge, 502, 14, true));
            // Add(4034, "bagsFrame718", "Graffiti Small Bag", 1000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 500, 5, true));
            // Add(4035, "bagsFrame599", "Cat Green Bag", 800, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLarge, 500, 12, true));
            // Add(4036, "bagsFrame642", "Cat Pink Bag", 800, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLarge, 500, 10, true));
            // Add(4037, "bagsFrame638", "THRASHER Silver Bag", 1500, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 500, 0, true));
            // Add(4038, "bagsFrame654", "THRASHER Blue Bag", 1500, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 500, 1, true));
            // Add(4039, "bagsFrame670", "THRASHER Purple Bag", 1500, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLarge, 500, 2, true));
            // Add(4040, "bagsFrame583", "SUPREME White Big Bag", 3500, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 509, 8, true));
            // Add(4041, "bagsFrame567", "SUPREME Black Big Bag", 3500, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 509, 9, true));
            // Add(4042, "bagsFrame551", "SUPREME Silver Big Bag", 3500, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 509, 10, true));
            // Add(4043, "bagsFrame515", "GUCCI Green Big Bag", 3000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 509, 2, true));
            // Add(4044, "bagsFrame499", "GUCCI Red Big Bag", 3000, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLarge, 509, 3, true));
            // Add(4045, "bagsFrame674", "SUPREME Heart Pink Bag", 10000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 500, 13, true));
            // Add(4046, "bagsFrame690", "SUPREME Heart Blue Bag", 10000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 500, 14, true));
            // Add(4047, "bagsFrame722", "GUCCI Tiger Bag", 25000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 502, 0, true));
            // Add(4048, "bagsFrame738", "GUCCI Snake Bag", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 502, 1, true));
            // Add(4049, "bagsFrame555", "GUCCI Classic White Bag", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 510, 0, true));
            // Add(4050, "bagsFrame525", "Classic Blue Bag", 3500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 510, 2, true));
            // Add(4051, "bagsFrame507", "Classic Rainbow Bag", 3700, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 510, 3, true));
            // Add(4052, "bagsFrame491", "Classic Snake Bag", 3600, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 510, 4, true));
            // Add(4053, "bagsFrame475", "Classic Blue Star Bag", 3800, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 510, 5, true));
            // Add(4054, "bagsFrame631", "Classic Purple Star Bag", 3800, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 510, 6, true));
            // Add(4055, "bagsFrame611", "Classic Colorful Bag", 3800, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 510, 7, true));
            // Add(4056, "bagsFrame591", "Classic Rain Bag", 3500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 510, 8, true));
            // Add(4057, "bagsFrame575", "Classic Skull Bag", 3600, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 510, 9, true));
            // Add(4058, "bagsFrame559", "Classic Blue And White Bag", 3500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 510, 10, true));
            // Add(4059, "bagsFrame543", "Classic Flower Blue Bag", 3500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 510, 11, true));
            // Add(4060, "bagsFrame523", "Classic Flower Pink Bag", 3500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 510, 12, true));
            // Add(4061, "bagsFrame511", "Classic Sand Bag", 3500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 510, 13, true));
            // Add(4062, "bagsFrame495", "Classic Graffiti Bag", 3600, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 510, 14, true));
            // Add(4063, "bagsFrame479", "Classic Graffiti V2 Bag", 3600, ItemRarities.Epic, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLarge, 510, 15, true));
            // Add(4064, "bagsFrame635", "GUCCI Millionaire GOLD Bag", 18000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 511, 1, true));
            // Add(4065, "bagsFrame615", "HELLO KITTY Bag", 18000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 511, 2, true));
            // Add(4066, "bagsFrame593", "GUCCI Millionaire SILVER Bag", 18000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 511, 3, true));
            // Add(4067, "bagsFrame650", "Knight Bag", 10000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 503, 0, true));
            // Add(4068, "bagsFrame706", "Camping Bag", 10000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 501, 0, true));
            // Add(4069, "bagsFrame547", "GUCCI Blue Bag", 3000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 502, 0, false));
            // Add(4070, "bagsFrame531", "GUCCI Silver Bag", 3000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 502, 1, false));
            // Add(4071, "bagsFrame623", "Nike Big Bag", 2500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 502, 6, false));
            // Add(4072, "bagsFrame603", "Nike Military Bag", 2700, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 502, 7, false));
            // Add(4073, "bagsFrame686", "Georgia Small Bag", 500, ItemRarities.Hight, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLight, 508, 3, false));
            // Add(4074, "bagsFrame702", "Russian Small Bag", 500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 508, 4, false));
            // Add(4075, "bagsFrame535", "Georgia Big Bag", 1000, ItemRarities.Hight, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLight, 502, 11, false));
            // Add(4076, "bagsFrame762", "Simpsons Small Bag", 1500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 508, 8, false));
            // Add(4077, "bagsFrame778", "Pony Small Bag", 800, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 508, 9, false));
            // Add(4078, "bagsFrame646", "Dora Medium Bag", 1000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 501, 5, false));
            // Add(4079, "bagsFrame662", "Adidas Silver Bag", 1000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 501, 6, false));
            // Add(4080, "bagsFrame678", "Adidas Black Bag", 1000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 501, 7, false));
            // Add(4081, "bagsFrame742", "Naruto Bag", 1000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 501, 11, false));
            // Add(4082, "bagsFrame754", "Stone Island Bag", 1500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLight, 501, 12, false));
            // Add(4083, "bagsFrame539", "Georgian Bag", 2000, ItemRarities.Hight, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLight, 503, 1, false));
            // Add(4084, "bagsFrame483", "Nike Green Bag", 1300, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 502, 4, false));
            // Add(4085, "bagsFrame466", "Nike Blue Bag", 1300, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 502, 5, false));
            // Add(4086, "bagsFrame519", "BMW Blue Bag", 1500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 502, 12, false));
            // Add(4087, "bagsFrame503", "BMW Red Bag", 1500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 502, 13, false));
            // Add(4088, "bagsFrame487", "BMW White Bag", 1500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 502, 14, false));
            // Add(4089, "bagsFrame471", "Military Green Bag", 700, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 502, 15, false));
            // Add(4090, "bagsFrame627", "Military White Bag", 700, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 502, 16, false));
            // Add(4091, "bagsFrame607", "Military Brown Bag", 700, ItemRarities.Hight, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackMedium, 502, 17, false));
            // Add(4092, "bagsFrame587", "Military Silver Bag", 700, ItemRarities.Hight, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackMedium, 502, 18, false));
            // Add(4093, "bagsFrame734", "3 Cat Bag", 900, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 508, 6, false));
            // Add(4094, "bagsFrame750", "Heart Small Bag", 900, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 508, 7, false));
            // Add(4095, "bagsFrame658", "Cat Small Bag", 700, ItemRarities.Hight, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackMedium, 508, 11, false));
            // Add(4096, "bagsFrame766", "Just Do It Bag", 1200, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 501, 3, false));
            // Add(4097, "bagsFrame782", "Dora White Bag", 1000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackMedium, 501, 4, false));
            // Add(4098, "bagsFrame694", "Square Bag", 500, ItemRarities.Hight, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackMedium, 501, 8, false));
            // Add(4099, "bagsFrame515", "GUCCI Green Big Bag", 3000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 502, 2, false));
            // Add(4100, "bagsFrame499", "GUCCI Red Big Bag", 3000, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLarge, 502, 3, false));
            // Add(4101, "bagsFrame583", "SUPREME White Big Bag", 3500, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 502, 8, false));
            // Add(4102, "bagsFrame567", "SUPREME Black Big Bag", 3500, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 502, 9, false));
            // Add(4103, "bagsFrame551", "SUPREME Silver Big Bag", 3500, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 502, 10, false));
            // Add(4104, "bagsFrame638", "THRASHER Silver Bag", 1500, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 508, 0, false));
            // Add(4105, "bagsFrame654", "THRASHER Blue Bag", 1500, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 508, 1, false));
            // Add(4106, "bagsFrame670", "THRASHER Purple Bag", 1500, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLarge, 508, 2, false));
            // Add(4107, "bagsFrame718", "Graffiti Small Bag", 1000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 508, 5, false));
            // Add(4108, "bagsFrame642", "Cat Pink Bag", 800, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLarge, 508, 10, false));
            // Add(4109, "bagsFrame710", "Alcoste Bag", 1500, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLarge, 501, 9, false));
            // Add(4110, "bagsFrame726", "SUPREME Purple Bag", 2500, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 501, 10, false));
            // Add(4111, "bagsFrame599", "Cat Green Bag", 800, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLarge, 508, 12, false));
            // Add(4112, "bagsFrame770", "GTA V Police Bag", 1000, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLarge, 501, 13, false));
            // Add(4113, "bagsFrame786", "GTA V Dog Bag", 1000, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BackpackLarge, 501, 14, false));
            // Add(4114, "bagsFrame674", "SUPREME Heart Pink Bag", 10000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 508, 13, false));
            // Add(4115, "bagsFrame690", "SUPREME Heart Blue Bag", 10000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 508, 14, false));
            // Add(4116, "bagsFrame722", "GUCCI Tiger Bag", 25000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 501, 0, false));
            // Add(4117, "bagsFrame738", "GUCCI Snake Bag", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 501, 1, false));
            // Add(4118, "bagsFrame555", "GUCCI Classic White Bag", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 503, 0, false));
            // Add(4119, "bagsFrame525", "Classic Blue Bag", 3500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 503, 2, false));
            // Add(4120, "bagsFrame507", "Classic Rainbow Bag", 3700, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 503, 3, false));
            // Add(4121, "bagsFrame491", "Classic Snake Bag", 3600, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 503, 4, false));
            // Add(4122, "bagsFrame475", "Classic Blue Star Bag", 3800, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 503, 5, false));
            // Add(4123, "bagsFrame631", "Classic Purple Star Bag", 3800, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 503, 6, false));
            // Add(4124, "bagsFrame611", "Classic Colorful Bag", 3800, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 503, 7, false));
            // Add(4125, "bagsFrame591", "Classic Rain Bag", 3500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 503, 8, false));
            // Add(4126, "bagsFrame575", "Classic Skull Bag", 3600, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 503, 9, false));
            // Add(4127, "bagsFrame559", "Classic Blue And White Bag", 3500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 503, 10, false));
            // Add(4128, "bagsFrame543", "Classic Flower Blue Bag", 3500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 503, 11, false));
            // Add(4129, "bagsFrame523", "Classic Flower Pink Bag", 3500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 503, 12, false));
            // Add(4130, "bagsFrame511", "Classic Sand Bag", 3500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 503, 13, false));
            // Add(4131, "bagsFrame495", "Classic Graffiti Bag", 3600, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 503, 14, false));
            // Add(4132, "bagsFrame479", "Classic Graffiti V2 Bag", 3600, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 503, 15, false));
            // Add(4133, "bagsFrame635", "GUCCI Millionaire GOLD Bag", 18000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 504, 1, false));
            // Add(4134, "bagsFrame615", "HELLO KITTY Bag", 18000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 504, 2, false));
            // Add(4135, "bagsFrame593", "GUCCI Millionaire SILVER Bag", 18000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 504, 3, false));
            // Add(4136, "bagsFrame650", "Knight Bag", 10000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.BackpackLarge, 500, 0, false));
            // #endregion

            // #region BodyArmor
            // Add(4500, "Frame760", "1 Body armor", 80, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BodyArmor, 1, 0, true), 1, weight: 20);
            // Add(4501, "Frame761", "2 Body armor", 100, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BodyArmor, 1, 0, true), 2, weight: 15);
            // Add(4502, "Frame762", "3 Body armor", 150, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BodyArmor, 1, 0, true), 3, weight: 10);
            // Add(4503, "Frame763", "5 Body armor", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BodyArmor, 1, 0, true), 5, weight: 5);
            // Add(4504, "Frame764", "10 Body armor", 600, ItemRarities.Medium, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BodyArmor, 1, 0, true), 10, weight: 5);
            // Add(4505, "Frame765", "50 Body armor", 3000, ItemRarities.Hight, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BodyArmor, 1, 0, true), 50);
            // Add(4506, "Frame766", "100 Body armor", 5500, ItemRarities.Epic, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BodyArmor, 1, 0, true), 100);
            // Add(4507, "Frame760", "1 Body armor", 80, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BodyArmor, 1, 0, false), 1, weight: 20);
            // Add(4508, "Frame761", "2 Body armor", 100, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BodyArmor, 1, 0, false), 2, weight: 15);
            // Add(4509, "Frame762", "3 Body armor", 150, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BodyArmor, 1, 0, false), 3, weight: 10);
            // Add(4510, "Frame763", "5 Body armor", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BodyArmor, 1, 0, false), 5, weight: 5);
            // Add(4511, "Frame764", "10 Body armor", 600, ItemRarities.Medium, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BodyArmor, 1, 0, false), 10, weight: 5);
            // Add(4512, "Frame765", "50 Body armor", 3000, ItemRarities.Hight, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BodyArmor, 1, 0, false), 50);
            // Add(4513, "Frame766", "100 Body armor", 5500, ItemRarities.Epic, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.BodyArmor, 1, 0, false), 100);
            // #endregion

            // #region Accessory
            // Add(5050, "accessoriesFrame643", "Russian Chain", 50, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Accessories, 500, 8, true));
            // Add(5051, "accessoriesFrame631", "Wolf Chain", 150, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Accessories, 500, 9, true));
            // Add(5052, "accessoriesFrame634", "Ghetto Chain", 400, ItemRarities.Base, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Accessories, 500, 3, true));
            // Add(5053, "accessoriesFrame647", "Gunster Chain", 400, ItemRarities.Base, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Accessories, 500, 2, true));
            // Add(5054, "accessoriesFrame655", "Georgian Chain", 100, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Accessories, 500, 7, true));
            // Add(5055, "accessoriesFrame663", "Good Chain", 50, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Accessories, 500, 11, true));
            // Add(5056, "accessoriesFrame667", "Families Chain", 100, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Accessories, 500, 6, true));
            // Add(5057, "accessoriesFrame675", "Mountain Chain", 50, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Accessories, 500, 10, true));
            // Add(5058, "accessoriesFrame623", "Marabunta Chain", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Accessories, 500, 4, true));
            // Add(5059, "accessoriesFrame659", "First Of Fidelity Chain", 500, ItemRarities.Base, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Accessories, 500, 1, true));
            // Add(5060, "accessoriesFrame671", "2PAC Chain", 300, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Accessories, 500, 0, true));
            // Add(5061, "accessoriesFrame679", "The Families Chain", 100, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Accessories, 500, 5, true));
            // Add(5062, "accessoriesFrame627", "GoPro HERO 7", 1500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Accessories, 502, 0, true));
            // //Add(5063, "accessoriesFrame639", "Police Badge", 3000, ItemRarities.Epic, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Accessories, 501, 1, true));
            // //Add(5064, "accessoriesFrame651", "FIB Badge", 3000, ItemRarities.Epic, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Accessories, 501, 0, true));
            // #endregion

            // #region Mask
            // Add(5500, "maskFrame622", "Best Cat Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 0, false));
            // Add(5501, "maskFrame646", "Cat Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 1, false));
            // Add(5502, "maskFrame674", "Dog Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 2, false));
            // Add(5503, "maskFrame702", "Keep Calm Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 3, false));
            // Add(5504, "maskFrame730", "Pew Pew Mask", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 4, false));
            // Add(5505, "maskFrame486", "Pew Madafaka Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 5, false));
            // Add(5506, "maskFrame514", "Fire Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 6, false));
            // Add(5507, "maskFrame542", "Hacker Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 7, false));
            // Add(5508, "maskFrame570", "Pika Mask", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 8, false));
            // Add(5509, "maskFrame598", "Adidas Mask", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 9, false));
            // Add(5510, "maskFrame626", "Coffe Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 10, false));
            // Add(5511, "maskFrame650", "Spider Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 11, false));
            // Add(5512, "maskFrame678", "Senshot Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 12, false));
            // Add(5513, "maskFrame706", "Kitty Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 13, false));
            // Add(5514, "maskFrame734", "Mole Mask", 600, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 14, false));
            // Add(5515, "maskFrame490", "Moon Cat Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 15, false));
            // Add(5516, "maskFrame518", "Naruto Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 16, false));
            // Add(5517, "maskFrame546", "Naruto V2 Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 17, false));
            // Add(5518, "maskFrame574", "Wave Mask", 350, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 18, false));
            // Add(5519, "maskFrame602", "Tokyo Mask", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 19, false));
            // Add(5520, "maskFrame630", "Creation Of Adam Mask", 350, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 20, false));
            // Add(5521, "maskFrame654", "Touch Me Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 21, false));
            // Add(5522, "maskFrame682", "Rawr Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 22, false));
            // Add(5523, "maskFrame710", "Black Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 23, false));
            // Add(5524, "maskFrame738", "Gangser Cat Mask", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 23, false));
            // Add(5525, "maskFrame502", "Iron Man Mask", 1500, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 2, false));
            // Add(5526, "maskFrame530", "Louis Vitton White Mask", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 3, false));
            // Add(5527, "maskFrame558", "Louis Vitton Black Mask", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 4, false));
            // Add(5528, "maskFrame586", "LV x SUPREME Red Mask", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 5, false));
            // Add(5529, "maskFrame614", "LV x SUPREME White Mask", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 6, false));
            // Add(5530, "maskFrame638", "LV x SUPREME Burgundy Mask", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 7, false));
            // Add(5531, "maskFrame666", "LV x SUPREME Green Mask", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 8, false));
            // Add(5532, "maskFrame694", "GUCCI Black Mask", 10000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 9, false));
            // Add(5533, "maskFrame722", "GUCCI White Mask", 10000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 10, false));
            // Add(5534, "maskFrame478", "Space Mask", 1000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 11, false));
            // Add(5535, "maskFrame506", "Blue Mask", 500, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 12, false));
            // Add(5536, "maskFrame534", "Flower Mask", 700, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 13, false));
            // Add(5537, "maskFrame562", "FC Barcelona Mask", 2500, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 14, false));
            // Add(5538, "maskFrame590", "AC Milan Mask", 2500, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 15, false));
            // Add(5539, "maskFrame618", "A.C.A.B Black Mask", 700, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 16, false));
            // Add(5540, "maskFrame642", "A.C.A.B Fuck The Police Mask", 700, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 17, false));
            // Add(5541, "maskFrame670", "A.C.A.B White Mask", 700, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 18, false));
            // Add(5542, "maskFrame698", "Five Finger Mask", 500, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 19, false));
            // Add(5543, "maskFrame726", "Grafitti Mask", 800, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 20, false));
            // Add(5544, "maskFrame482", "Grafitti V2 Mask", 800, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 21, false));
            // Add(5545, "maskFrame510", "Grafitty V3 Mask", 400, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 22, false));
            // Add(5546, "maskFrame538", "Grafitty V4 Mask", 800, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 23, false));
            // Add(5547, "maskFrame594", "Russia Mask", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 25, false));
            // Add(5548, "maskFrame466", "Beard Dark Brown", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 500, 0, false));
            // Add(5549, "maskFrame606", "Mustache Blonde", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 501, 2, false));
            // Add(5550, "maskFrame610", "Beard Black", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 505, 0, false));
            // Add(5551, "maskFrame470", "Cowboy Mustache Brown", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 503, 1, false));
            // Add(5552, "maskFrame714", "Cowboy Mustache Grey", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 503, 0, false));
            // Add(5553, "maskFrame474", "Solid Beard White", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 506, 2, false));
            // Add(5554, "maskFrame522", "Old Man Beard Yelow", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 500, 2, false));
            // Add(5555, "maskFrame526", "Long Beard Grey", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 504, 0, false));
            // Add(5556, "maskFrame718", "Beard Solid Brown", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 506, 1, false));
            // Add(5557, "maskFrame686", "Biker Mustache White", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 502, 2, false));
            // Add(5558, "maskFrame690", "Solid Beard Black", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 506, 0, false));
            // Add(5559, "maskFrame634", "Biker Mustache Grey", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 502, 0, false));
            // Add(5560, "maskFrame494", "Beard Brown", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 500, 1, false));
            // Add(5561, "maskFrame498", "Cowboy Mustache White", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 503, 2, false));
            // Add(5562, "maskFrame550", "Small Mustache Grey", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 501, 0, false));
            // Add(5563, "maskFrame554", "Long Beard Brown", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 504, 1, false));
            // Add(5564, "maskFrame658", "Biker Mustache Brown", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 502, 1, false));
            // Add(5565, "maskFrame662", "Royal Beard", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 505, 2, false));
            // Add(5566, "maskFrame578", "Small Mustache Brown", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 501, 1, false));
            // Add(5567, "maskFrame582", "Old Man Beard White", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 504, 2, false));
            // Add(5568, "maskFrame622", "Best Cat Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 0, true));
            // Add(5569, "maskFrame646", "Cat Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 1, true));
            // Add(5570, "maskFrame674", "Dog Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 2, true));
            // Add(5571, "maskFrame702", "Keep Calm Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 3, true));
            // Add(5572, "maskFrame730", "Pew Pew Mask", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 4, true));
            // Add(5573, "maskFrame486", "Pew Madafaka Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 5, true));
            // Add(5574, "maskFrame514", "Fire Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 6, true));
            // Add(5575, "maskFrame542", "Hacker Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 7, true));
            // Add(5576, "maskFrame570", "Pika Mask", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 8, true));
            // Add(5577, "maskFrame598", "Adidas Mask", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 9, true));
            // Add(5578, "maskFrame626", "Coffe Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 10, true));
            // Add(5579, "maskFrame650", "Spider Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 11, true));
            // Add(5580, "maskFrame678", "Senshot Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 12, true));
            // Add(5581, "maskFrame706", "Kitty Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 13, true));
            // Add(5582, "maskFrame734", "Mole Mask", 600, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 14, true));
            // Add(5583, "maskFrame490", "Moon Cat Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 15, true));
            // Add(5584, "maskFrame518", "Naruto Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 16, true));
            // Add(5585, "maskFrame546", "Naruto V2 Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 17, true));
            // Add(5586, "maskFrame574", "Wave Mask", 350, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 18, true));
            // Add(5587, "maskFrame602", "Tokyo Mask", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 19, true));
            // Add(5588, "maskFrame630", "Creation Of Adam Mask", 350, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 20, true));
            // Add(5589, "maskFrame654", "Touch Me Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 21, true));
            // Add(5590, "maskFrame682", "Rawr Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 22, true));
            // Add(5591, "maskFrame710", "Black Mask", 200, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 23, true));
            // Add(5592, "maskFrame738", "Gangser Cat Mask", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 509, 23, true));
            // Add(5593, "maskFrame502", "Iron Man Mask", 1500, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 2, true));
            // Add(5594, "maskFrame530", "Louis Vitton White Mask", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 3, true));
            // Add(5595, "maskFrame558", "Louis Vitton Black Mask", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 4, true));
            // Add(5596, "maskFrame586", "LV x SUPREME Red Mask", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 5, true));
            // Add(5597, "maskFrame614", "LV x SUPREME White Mask", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 6, true));
            // Add(5598, "maskFrame638", "LV x SUPREME Burgundy Mask", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 7, true));
            // Add(5599, "maskFrame666", "LV x SUPREME Green Mask", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 8, true));
            // Add(5600, "maskFrame694", "GUCCI Black Mask", 10000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 9, true));
            // Add(5601, "maskFrame722", "GUCCI White Mask", 10000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 10, true));
            // Add(5602, "maskFrame478", "Space Mask", 1000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 11, true));
            // Add(5603, "maskFrame506", "Blue Mask", 500, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 12, true));
            // Add(5604, "maskFrame534", "Flower Mask", 700, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 13, true));
            // Add(5605, "maskFrame562", "FC Barcelona Mask", 2500, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 14, true));
            // Add(5606, "maskFrame590", "AC Milan Mask", 2500, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 507, 15, true));
            // Add(5607, "maskFrame618", "A.C.A.B Black Mask", 700, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 16, true));
            // Add(5608, "maskFrame642", "A.C.A.B Fuck The Police Mask", 700, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 17, true));
            // Add(5609, "maskFrame670", "A.C.A.B White Mask", 700, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 18, true));
            // Add(5610, "maskFrame698", "Five Finger Mask", 500, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 19, true));
            // Add(5611, "maskFrame726", "Grafitti Mask", 800, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 20, true));
            // Add(5612, "maskFrame482", "Grafitti V2 Mask", 800, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 21, true));
            // Add(5613, "maskFrame510", "Grafitty V3 Mask", 400, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 22, true));
            // Add(5614, "maskFrame538", "Grafitty V4 Mask", 800, ItemRarities.Legend, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 23, true));
            // Add(5615, "maskFrame594", "Russia Mask", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 507, 25, true));
            // Add(5616, "maskFrame466", "Beard Dark Brown", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 500, 0, true));
            // Add(5617, "maskFrame606", "Mustache Blonde", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 501, 2, true));
            // Add(5618, "maskFrame610", "Beard Black", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 505, 0, true));
            // Add(5619, "maskFrame470", "Cowboy Mustache Brown", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 503, 1, true));
            // Add(5620, "maskFrame714", "Cowboy Mustache Grey", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 503, 0, true));
            // Add(5621, "maskFrame474", "Solid Beard White", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 506, 2, true));
            // Add(5622, "maskFrame522", "Old Man Beard Yelow", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 500, 2, true));
            // Add(5623, "maskFrame526", "Long Beard Grey", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 504, 0, true));
            // Add(5624, "maskFrame718", "Beard Solid Brown", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 506, 1, true));
            // Add(5625, "maskFrame686", "Biker Mustache White", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 502, 2, true));
            // Add(5626, "maskFrame690", "Solid Beard Black", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 506, 0, true));
            // Add(5627, "maskFrame634", "Biker Mustache Grey", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 502, 0, true));
            // Add(5628, "maskFrame494", "Beard Brown", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 500, 1, true));
            // Add(5629, "maskFrame498", "Cowboy Mustache White", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 503, 2, true));
            // Add(5630, "maskFrame550", "Small Mustache Grey", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 501, 0, true));
            // Add(5631, "maskFrame554", "Long Beard Brown", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 504, 1, true));
            // Add(5632, "maskFrame658", "Biker Mustache Brown", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 502, 1, true));
            // Add(5633, "maskFrame662", "Royal Beard", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 505, 2, true));
            // //Add(5634, "maskFrame578", "Small Mustache Brown", 0, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 501, 1, true));
            // Add(5635, "maskFrame582", "Old Man Beard White", 20000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Mask, 504, 2, true));

            // //v2.0
            // Add(5636, "pack_Frame680", "Zubenko", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 45, 0, true));
            // Add(5637, "pack_Frame680", "Zubenko", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 45, 0, false));
            // Add(5638, "pack_Frame679", "Night", 150, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 51, 0, true));
            // Add(5639, "pack_Frame679", "Night", 150, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 51, 0, false));
            // Add(5640, "pack_Frame681", "Bigness", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 101, 9, true));
            // Add(5641, "pack_Frame681", "Bigness", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 0, 0, false));
            // Add(5642, "pack_Frame683", "Anime", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 43, 0, true));
            // Add(5643, "pack_Frame683", "Anime", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 43, 0, false));
            // Add(5644, "pack_Frame682", "Crooked Mouth", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 44, 0, true));
            // Add(5645, "pack_Frame682", "Crooked Mouth", 300, ItemRarities.Low, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Mask, 44, 0, false));


            // #endregion

            // #region Top Male
            // Add(6000, "topFrame522", "The Dragon Top", 1000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 0, true));
            // Add(6001, "topFrame498", "Landscape Top", 2500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 1, true));
            // Add(6002, "topFrame510", "Spring Landscape Top", 1500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 2, true));
            // Add(6003, "topFrame554", "Tiger Top", 1500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 3, true));
            // Add(6004, "topFrame562", "Naruto Top", 1500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 4, true));
            // Add(6005, "topFrame486", "11-12-13 Top", 1000, ItemRarities.Medium, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 510, 5, true));
            // Add(6006, "topFrame474", "Criminal White Top", 900, ItemRarities.Medium, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 510, 6, true));
            // Add(6007, "topFrame466", "Criminal Black Top", 900, ItemRarities.Medium, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 510, 7, true));
            // Add(6008, "topFrame558", "Off-White White Top", 6000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 8, true));
            // Add(6009, "topFrame478", "Interstellar Top", 1000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 9, true));
            // Add(6010, "topFrame550", "Taboo", 1000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 10, true));
            // Add(6011, "topFrame538", "Where Is My Money Top", 1000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 11, true));
            // Add(6012, "topFrame542", "Audi Top", 1000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 12, true));
            // Add(6013, "topFrame490", "Belisigaga Top", 900, ItemRarities.Medium, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 510, 13, true));
            // Add(6014, "topFrame502", "BMW Top", 1000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 14, true));
            // Add(6015, "topFrame526", "XXXTentacion", 1000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 15, true));
            // Add(6016, "topFrame514", "Pusha T Black Top", 2500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 16, true));
            // Add(6017, "topFrame518", "Pusha T White Top", 2500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 17, true));
            // Add(6018, "topFrame546", "Drake Top", 1500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 18, true));
            // Add(6019, "topFrame482", "Kendrick Lamar Top", 1500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 19, true));
            // Add(6020, "topFrame470", "Kendrick Lamar Pride Top", 10000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 20, true));
            // Add(6021, "topFrame494", "XXXTentacion Black Top", 1500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 21, true));
            // Add(6022, "topFrame506", "GUCCI Space Top", 15000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 22, true));
            // Add(6023, "topFrame530", "Bape Top", 7000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 510, 23, true));
            // Add(6024, "topFrame534", "Crowned Rose Top", 10000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 514, 0, true));
            // Add(6025, "topFrame572", "Achor Top", 2500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 514, 1, true));
            // Add(6026, "topFrame564", "Royal Rose", 3500, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 514, 2, true));
            // Add(6027, "topFrame570", "Bird Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 514, 3, true));
            // Add(6028, "topFrame571", "Bone Top", 5000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 514, 4, true));
            // Add(6029, "topFrame576", "King Tiger Top", 3500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 514, 5, true));
            // //Add(6030, "-", "Top", 0, ItemRarities., ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 514, 6, true));
            // //Add(6031, "-", "Top", 0, ItemRarities., ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 514, 7, true));
            // Add(6032, "topFrame567", "Cringe Top", 5000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 514, 8, true));
            // Add(6033, "topFrame582", "Hustler Top", 2000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 514, 9, true));
            // Add(6034, "topFrame580", "King And Queen Top", 3000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 514, 10, true));
            // Add(6035, "topFrame585", "Bonnie And Clyde Top", 3500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 514, 11, true));
            // Add(6036, "topFrame586", "Legendary Top", 2000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 514, 12, true));
            // Add(6037, "topFrame579", "Lenin Punk Top", 1500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 514, 13, true));
            // Add(6038, "topFrame573", "Stickers Bomb Top", 2000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 514, 14, true));
            // Add(6039, "topFrame584", "One Of Us Will Die Top", 2000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 515, 0, true));
            // Add(6040, "topFrame581", "Anime V1 Top", 2000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 515, 1, true));
            // Add(6041, "topFrame583", "Anime V2 Top", 2300, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 515, 2, true));
            // Add(6042, "topFrame565", "Anime V3 Top", 2000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 515, 3, true));
            // Add(6043, "topFrame574", "Anime V4 Top", 2000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 515, 4, true));
            // Add(6044, "topFrame578", "Anime V5 Top", 2000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 515, 5, true));
            // Add(6045, "topFrame575", "Anime V6 Top", 2300, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 515, 6, true));
            // Add(6046, "topFrame563", "Anime V7 Top", 2000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 515, 7, true));
            // Add(6047, "topFrame568", "Anime V8 Top", 2000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 515, 8, true));
            // Add(6048, "topFrame566", "Anime V9 Top", 2500, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 515, 9, true));
            // Add(6049, "topFrame569", "Anime V10 Top", 2000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 515, 10, true));
            // Add(6050, "topFrame577", "Anime V11 Top", 2000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 515, 11, true));
            // #endregion

            // #region Top Female
            // Add(6500, "female_topFrame547", "Vogue Classic Top", 1000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 538, 0, false));
            // Add(6501, "female_topFrame546", "Vogue Aqua Top", 1000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 538, 1, false));
            // Add(6502, "female_topFrame548", "Vogue Ice Queen Top", 1000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 538, 2, false));
            // Add(6503, "female_topFrame549", "Vogue Red Star Top", 1000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 538, 3, false));
            // Add(6504, "female_topFrame552", "Vogue Dream Top", 1000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 538, 4, false));
            // Add(6505, "female_topFrame553", "Vogue Purple Top", 1000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 538, 5, false));
            // Add(6506, "female_topFrame551", "Vogue Snow White Top", 1000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 538, 6, false));
            // Add(6507, "female_topFrame550", "Vogue Brown Top", 1000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 538, 7, false));
            // Add(6508, "female_topFrame616", "Business High End Top", 500, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 502, 0, false));
            // Add(6509, "female_topFrame614", "Business Blue Top", 500, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 502, 1, false));
            // Add(6510, "female_topFrame617", "Business Classic Top", 500, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 502, 2, false));
            // Add(6511, "female_topFrame618", "Business Dark Top", 500, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 502, 3, false));
            // Add(6512, "female_topFrame620", "Business Silver Top", 500, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 502, 4, false));
            // Add(6513, "female_topFrame621", "Business Top", 500, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 502, 5, false));
            // Add(6514, "female_topFrame619", "Business Dark Grey Top", 500, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 502, 6, false));
            // Add(6515, "female_topFrame615", "Business Blue Metallic Top", 500, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 502, 7, false));
            // Add(6516, "female_topFrame607", "Business Styled Top", 500, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 502, 8, false));
            // Add(6517, "female_topFrame608", "Business Grey Top", 500, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 502, 9, false));
            // Add(6518, "female_topFrame610", "Business Matte Top", 500, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 502, 10, false));
            // Add(6519, "female_topFrame611", "Business Light Silver Top", 500, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 502, 11, false));
            // Add(6520, "female_topFrame609", "Business Black Styled Top", 500, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 502, 12, false));
            // Add(6521, "female_topFrame612", "Business Dark Blue Top", 500, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 502, 13, false));
            // Add(6522, "female_topFrame613", "Business White Top", 500, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 502, 14, false));
            // Add(6523, "female_topFrame558", "Ballet NonColor Top", 5000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 541, 0, false));
            // Add(6524, "female_topFrame559", "Ballet Classic Top", 5000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 541, 1, false));
            // Add(6525, "female_topFrame561", "Ballet Sky Top", 5000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 541, 2, false));
            // Add(6526, "female_topFrame560", "Ballet Light Red Top", 5000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 541, 3, false));
            // Add(6527, "female_topFrame557", "Ballet Silver Top", 5000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 541, 4, false));
            // Add(6528, "female_topFrame556", "Ballet Black Top", 5000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 541, 5, false));
            // Add(6529, "female_topFrame554", "Ballet Light Green Top", 5000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 541, 6, false));
            // Add(6530, "female_topFrame555", "Ballet Light Brown Top", 5000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 541, 7, false));
            // Add(6531, "female_topFrame543", "Ballet Red Cosmic Top", 5000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 541, 8, false));
            // Add(6532, "female_topFrame540", "Ballet Default Top", 5000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 541, 9, false));
            // Add(6533, "female_topFrame541", "Ballet Black Cosmic Top", 5000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 541, 10, false));
            // Add(6534, "female_topFrame542", "Ballet Ultra Red Top", 5000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 541, 11, false));
            // Add(6535, "female_topFrame545", "Ballet Blood Red Top", 5000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 541, 12, false));
            // Add(6536, "female_topFrame544", "Ballet Violet Top", 5000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 541, 13, false));
            // Add(6537, "female_topFrame539", "Ballet Grass Top", 5000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 541, 14, false));
            // Add(6538, "female_topFrame538", "Ballet Galaxy Top", 5000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 541, 15, false));
            // Add(6539, "female_topFrame623", "DG Brown Top", 1000, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 518, 0, false));
            // Add(6540, "female_topFrame622", "DG Classic Top", 1000, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Top, 518, 1, false));
            // Add(6541, "female_topFrame602", "DG Splatter Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 2, false));
            // Add(6542, "female_topFrame603", "DG Aqua Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 3, false));
            // Add(6543, "female_topFrame600", "DG Styled Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 4, false));
            // Add(6544, "female_topFrame597", "DG Violet Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 5, false));
            // Add(6545, "female_topFrame592", "DG Art Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 6, false));
            // Add(6546, "female_topFrame594", "DG White Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 7, false));
            // Add(6547, "female_topFrame587", "DG Zebra Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 8, false));
            // Add(6548, "female_topFrame588", "DG Tiger Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 9, false));
            // Add(6549, "female_topFrame604", "DG Solid Black Top", 5000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 10, false));
            // Add(6550, "female_topFrame595", "DG Tiger Star Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 11, false));
            // Add(6551, "female_topFrame605", "DG RGB Top", 5000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 12, false));
            // Add(6552, "female_topFrame596", "DG B&R Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 13, false));
            // Add(6553, "female_topFrame598", "DG R&B Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 14, false));
            // Add(6554, "female_topFrame606", "DG Black and Yellow Top", 5000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 15, false));
            // Add(6555, "female_topFrame601", "DG Choose Me Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 16, false));
            // Add(6556, "female_topFrame599", "DG Choose Me Black Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 17, false));
            // Add(6557, "female_topFrame584", "DG Choose Me Green Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 18, false));
            // Add(6558, "female_topFrame585", "DG White Flower Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 19, false));
            // Add(6559, "female_topFrame589", "DG Brown Flower Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 20, false));
            // Add(6560, "female_topFrame586", "DG In Style Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 21, false));
            // Add(6561, "female_topFrame590", "DG Red Blocks Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 22, false));
            // Add(6562, "female_topFrame591", "DG Grey Top", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 518, 23, false));
            // Add(6563, "female_topFrame582", "Short Pink Top", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 543, 0, false));
            // Add(6564, "female_topFrame577", "Chanel Pink Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 543, 1, false));
            // Add(6565, "female_topFrame578", "Chanel Light Pink Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 543, 2, false));
            // Add(6566, "female_topFrame581", "Short Green Top", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 543, 3, false));
            // Add(6567, "female_topFrame580", "Short Lime Top", 5000, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 543, 4, false));
            // Add(6568, "female_topFrame575", "Chanel Lime Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 543, 5, false));
            // Add(6569, "female_topFrame574", "Chanel Green Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 543, 6, false));
            // Add(6570, "female_topFrame576", "Chanel Orange Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 543, 7, false));
            // Add(6571, "female_topFrame583", "Short Red Top", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 543, 8, false));
            // Add(6572, "female_topFrame579", "Chanel Red Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 543, 9, false));
            // Add(6573, "female_topFrame569", "Light Purple Set Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 544, 0, false));
            // Add(6574, "female_topFrame568", "Light Pink Set Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 544, 1, false));
            // Add(6575, "female_topFrame566", "Light Green Set Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 544, 2, false));
            // Add(6576, "female_topFrame567", "Yellow Set Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 544, 3, false));
            // Add(6577, "female_topFrame570", "Pink Set Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 544, 4, false));
            // Add(6578, "female_topFrame571", "Blue Set Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 544, 5, false));
            // Add(6579, "female_topFrame573", "Orange Set Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 544, 6, false));
            // Add(6580, "female_topFrame572", "Sky Blue Set Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 544, 7, false));
            // Add(6581, "female_topFrame562", "Classic Set Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 544, 8, false));
            // Add(6582, "female_topFrame563", "White Set Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 544, 9, false));
            // Add(6583, "female_topFrame564", "Black Set Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 544, 10, false));
            // Add(6584, "female_topFrame565", "Red Set Top", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 544, 11, false));

            // //V2.0
            // Add(6585, "topFrame522", "The Dragon Top", 1000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Top, 524, 0, false));

            // #endregion

            // #region Leg Male
            // Add(7000, "male_trousersFrame549", "Business Styled Leg", 500, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Leg, 500, 9, true));
            // Add(7001, "male_trousersFrame550", "Shorts Leg", 500, ItemRarities.Base, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 505, 0, true));
            // Add(7002, "male_trousersFrame558", "Sport Leg", 1000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 515, 0, true));
            // Add(7003, "male_trousersFrame556", "Sport Colored Leg", 1000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 515, 2, true));
            // Add(7004, "male_trousersFrame557", "Sport Moon Leg", 1000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 515, 3, true));
            // Add(7005, "male_trousersFrame552", "Sport White Leg", 2000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 515, 4, true));
            // Add(7006, "male_trousersFrame553", "Sport Orange Leg", 1000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 515, 5, true));
            // Add(7007, "male_trousersFrame555", "Sport Aqua Leg", 1000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 515, 6, true));
            // Add(7008, "male_trousersFrame554", "Sport Numeral Leg", 1000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 515, 7, true));
            // Add(7009, "male_trousersFrame547", "Gucci Leg", 2000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 504, 8, true));
            // Add(7010, "male_trousersFrame548", "Whiteoff Leg", 2000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 503, 2, true));
            // #endregion

            // #region Leg Female
            // Add(7500, "female_trousersFrame575", "Sport Graffiti Leg", 1500, ItemRarities.Base, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 0, false));
            // Add(7501, "female_trousersFrame574", "Sport Graffiti V2 Leg", 1500, ItemRarities.Base, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 1, false));
            // Add(7502, "female_trousersFrame565", "Sport Blue Leg", 1500, ItemRarities.Base, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 2, false));
            // Add(7503, "female_trousersFrame569", "Sport Grey Leg", 2000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 3, false));
            // Add(7504, "female_trousersFrame564", "Sport White Leg", 2000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 4, false));
            // Add(7505, "female_trousersFrame568", "Sport Silver Leg", 2000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 5, false));
            // Add(7506, "female_trousersFrame566", "Sport SkyBlue Leg", 2000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 6, false));
            // Add(7507, "female_trousersFrame567", "Sport Grey Red Leg", 2000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 7, false));
            // Add(7508, "female_trousersFrame571", "Sport Green Red Leg", 1500, ItemRarities.Base, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 8, false));
            // Add(7509, "female_trousersFrame570", "Sport Red Leg", 1500, ItemRarities.Base, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 9, false));
            // Add(7510, "female_trousersFrame596", "Sport Supreme Red Leg", 2000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 10, false));
            // Add(7511, "female_trousersFrame573", "Sport Jungle Leg", 1500, ItemRarities.Base, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 11, false));
            // Add(7512, "female_trousersFrame572", "Sport Classic Leg", 1500, ItemRarities.Base, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 12, false));
            // Add(7513, "female_trousersFrame588", "Sport Graffiti V3 Leg", 1500, ItemRarities.Base, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 13, false));
            // Add(7514, "female_trousersFrame586", "Sport Aqua Leg", 2000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 14, false));
            // Add(7515, "female_trousersFrame587", "Sport Supreme Flame White Leg", 3500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 15, false));
            // Add(7516, "female_trousersFrame592", "Sport Supreme Flame Black Leg", 3500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 16, false));
            // Add(7517, "female_trousersFrame591", "Sport Flame Leg", 3000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 17, false));
            // Add(7518, "female_trousersFrame590", "Sport Naruto Leg", 2000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 18, false));
            // Add(7519, "female_trousersFrame593", "Sport Sharingan Leg", 3000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 19, false));
            // Add(7520, "female_trousersFrame597", "Sport Galaxy Leg", 1500, ItemRarities.Base, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Leg, 509, 20, false));
            // Add(7521, "female_trousersFrame595", "Sport Blue Roses Leg", 3500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 21, false));
            // Add(7522, "female_trousersFrame584", "Sport White Roses Leg", 3000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 22, false));
            // Add(7523, "female_trousersFrame585", "Sport Unicorn Leg", 2000, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 23, false));
            // Add(7524, "female_trousersFrame589", "Sport Unicorn V2 Leg", 1500, ItemRarities.Base, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 24, false));
            // Add(7525, "female_trousersFrame594", "Sport Crew Leg", 5000, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 509, 25, false));
            // Add(7526, "female_trousersFrame551", "Glamorous Shorts Silver Leg", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 516, 0, false));
            // Add(7527, "female_trousersFrame552", "Glamorous Shorts Green Leg", 2500, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 516, 1, false));
            // Add(7528, "female_trousersFrame553", "Glamorous Shorts Yellow Leg", 2500, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 516, 2, false));
            // Add(7529, "female_trousersFrame554", "Glamorous Shorts Leg", 7500, ItemRarities.Epic, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 516, 3, false));
            // Add(7530, "female_trousersFrame555", "Glamorous Shorts Pink Leg", 3500, ItemRarities.Hight, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 516, 4, false));
            // Add(7531, "female_trousersFrame556", "Glamorous Shorts Green Belt Leg", 2500, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 516, 5, false));
            // Add(7532, "female_trousersFrame582", "Black Dress Leg", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 515, 0, false));
            // Add(7533, "female_trousersFrame581", "White Dress Leg", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 515, 1, false));
            // Add(7534, "female_trousersFrame583", "Red Dress Leg", 2500, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 515, 2, false));
            // Add(7535, "female_trousersFrame580", "Grey Dress Leg", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 515, 3, false));
            // Add(7536, "female_trousersFrame578", "Army Dress Leg", 2500, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 515, 4, false));
            // Add(7537, "female_trousersFrame579", "Army Dress V2 Leg", 2500, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 515, 5, false));
            // Add(7538, "female_trousersFrame577", "School Dress Leg", 2500, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 515, 6, false));
            // Add(7539, "female_trousersFrame576", "School Dress V2 Leg", 2500, ItemRarities.Low, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 515, 7, false));
            // Add(7540, "female_trousersFrame547", "Jeans Leg", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 513, 0, false));
            // Add(7541, "female_trousersFrame549", "Jeans Astralis Leg", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 522, 2, false));
            // Add(7542, "female_trousersFrame550", "DG Jeans Leg", 5000, ItemRarities.Legend, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 520, 0, false));

            // //V2.0
            // Add(7543, "male_trousersFrame557", "Sport Moon Leg", 1000, ItemRarities.Medium, ItemCategories.Clothes, false, new ClothesDonateItem(ItemNames.Leg, 512, 3, false));

            // #endregion

            // #region Auto Armor
            // Add(8000, "Frame697", "Car armor20%", 500, ItemRarities.Low, ItemCategories.Others, true, new VehicleTiningDonateItem(ModTypes.Armor, 0));
            // Add(8001, "Frame698", "Car armor 40%", 800, ItemRarities.Medium, ItemCategories.Others, true, new VehicleTiningDonateItem(ModTypes.Armor, 1));
            // Add(8002, "Frame699", "Car armor 60%", 1250, ItemRarities.Hight, ItemCategories.Others, true, new VehicleTiningDonateItem(ModTypes.Armor, 2));
            // Add(8003, "Frame700", "Car armor 80%", 1750, ItemRarities.Legend, ItemCategories.Others, true, new VehicleTiningDonateItem(ModTypes.Armor, 3));
            // Add(8004, "Frame701", "Car armor 100%", 3000, ItemRarities.Epic, ItemCategories.Others, true, new VehicleTiningDonateItem(ModTypes.Armor, 4));
            // #endregion

            // #region Inventory weight
            // Add(8500, "Frame662", "Inventory weight 1kg", 700, ItemRarities.Base, ItemCategories.Others, true, new InventoryWeightDonateItem(1, 1));
            // Add(8501, "Frame663", "Inventory weight 2kg", 1000, ItemRarities.Low, ItemCategories.Others, true, new InventoryWeightDonateItem(2, 2));
            // Add(8502, "Frame664", "Inventory weight 3kg", 1500, ItemRarities.Low, ItemCategories.Others, true, new InventoryWeightDonateItem(3, 3));
            // Add(8503, "Frame665", "Inventory weight 4kg", 2000, ItemRarities.Medium, ItemCategories.Others, true, new InventoryWeightDonateItem(4, 4));
            // Add(8504, "Frame666", "Inventory weight 5kg", 2500, ItemRarities.Medium, ItemCategories.Others, true, new InventoryWeightDonateItem(5, 5));
            // Add(8505, "Frame667", "Inventory weight 6kg", 3000, ItemRarities.Medium, ItemCategories.Others, true, new InventoryWeightDonateItem(6, 6));
            // Add(8506, "Frame668", "Inventory weight 7kg", 3500, ItemRarities.Hight, ItemCategories.Others, true, new InventoryWeightDonateItem(7, 7));
            // Add(8507, "Frame669", "Inventory weight 8kg", 4000, ItemRarities.Hight, ItemCategories.Others, true, new InventoryWeightDonateItem(8, 8));
            // Add(8508, "Frame670", "Inventory weight 9kg", 4500, ItemRarities.Hight, ItemCategories.Others, true, new InventoryWeightDonateItem(9, 9));
            // Add(8509, "Frame671", "Inventory weight 10kg", 5000, ItemRarities.Epic, ItemCategories.Others, true, new InventoryWeightDonateItem(10, 10));
            // //Add(8509, "Frame672", "Inventory weight 11kg", 0, ItemRarities.Epic, ItemCategories.Others, true, new InventoryWeightDonateItem(11, 11));
            // //Add(8509, "Frame673", "Inventory weight 12kg", 0, ItemRarities.Epic, ItemCategories.Others, true, new InventoryWeightDonateItem(12, 12));
            // //Add(8509, "Frame674", "Inventory weight 13kg", 0, ItemRarities.Epic, ItemCategories.Others, true, new InventoryWeightDonateItem(13, 13));
            // //Add(8509, "Frame675", "Inventory weight 14kg", 0, ItemRarities.Epic, ItemCategories.Others, true, new InventoryWeightDonateItem(14, 14));
            // //Add(8509, "Frame676", "Inventory weight 15kg", 0, ItemRarities.Epic, ItemCategories.Others, true, new InventoryWeightDonateItem(15, 15));
            // //Add(8509, "Frame677", "Inventory weight 16kg", 0, ItemRarities.NotDrop, ItemCategories.Others, true, new InventoryWeightDonateItem(16, 16));
            // //Add(8509, "Frame678", "Inventory weight 20kg", 0, ItemRarities.NotDrop, ItemCategories.Others, true, new InventoryWeightDonateItem(20, 20));
            // //Add(8509, "Frame679", "Inventory weight 30kg", 0, ItemRarities.NotDrop, ItemCategories.Others, true, new InventoryWeightDonateItem(30, 30));
            // //Add(8509, "Frame680", "Inventory weight 50kg", 0, ItemRarities.NotDrop, ItemCategories.Others, true, new InventoryWeightDonateItem(50, 50));
            // #endregion

            // #region Car Boost
            // Add(9001, "Frame686", "Car boost 10%", 650, ItemRarities.Legend, ItemCategories.Others, true, new VehBoostDonateItem(10));
            // Add(9002, "Frame687", "Car boost 20%", 1300, ItemRarities.Epic, ItemCategories.Others, true, new VehBoostDonateItem(20));
            // Add(9003, "Frame688", "Car boost 30%", 2050, ItemRarities.NotDrop, ItemCategories.Others, true, new VehBoostDonateItem(30));
            // #endregion

            // #region Vip Number
            // //Add(9500, "Frame702", "4 identical digits", 550, ItemRarities.Hight, ItemCategories.Others, true, new PhoneNumberDonateItem(10, 4));
            // //Add(9501, "Frame703", "5 identical digits", 750, ItemRarities.Hight, ItemCategories.Others, true, new PhoneNumberDonateItem(10, 5));
            // //Add(9502, "Frame704", "6 identical digits", 950, ItemRarities.Legend, ItemCategories.Others, true, new PhoneNumberDonateItem(10, 6));
            // //Add(9503, "Frame705", "7 identical digits", 1250, ItemRarities.Legend, ItemCategories.Others, true, new PhoneNumberDonateItem(10, 7));
            // //Add(9504, "Frame706", "8 identical digits", 1555, ItemRarities.Epic, ItemCategories.Others, true, new PhoneNumberDonateItem(10, 8));
            // //Add(9505, "Frame707", "9 identical digits", 1750, ItemRarities.Epic, ItemCategories.Others, true, new PhoneNumberDonateItem(10, 9));
            // //Add(9506, "Frame708", "10 identical digits", 2250, ItemRarities.NotDrop, ItemCategories.Others, true, new PhoneNumberDonateItem(10, 10));
            // #endregion

            // #region Car Tuning
            // //Add(10000, "Frame725", "Turbo 1", 750, ItemRarities.Hight, ItemCategories.Others, true, new VehicleTiningDonateItem(VehicleSystem.ModTypes.Turbo, 0));
            // //Add(10001, "Frame726", "Engine 3", 2500, ItemRarities.Hight, ItemCategories.Others, true, new VehicleTiningDonateItem(VehicleSystem.ModTypes.Engine, 2));
            // //Add(10002, "Frame727", "Transmission 3", 2000, ItemRarities.Hight, ItemCategories.Others, true, new VehicleTiningDonateItem(VehicleSystem.ModTypes.Transmission, 2));
            // //Add(10003, "Frame728", "Brakes 2", 1000, ItemRarities.Hight, ItemCategories.Others, true, new VehicleTiningDonateItem(VehicleSystem.ModTypes.Brakes, 1));
            // //Add(10004, "Frame729", "Full tunning", 5500, ItemRarities.Epic, ItemCategories.Others, true, new FullVehicleTuning());
            // #endregion

            // #region Car Neon
            // //Add(10500, "0", "Neon 3 colors", 250, ItemRarities.Legend, ItemCategories.Others, true, new ????? (3));
            // //Add(10501, "0", "Neon 6 colors", 500, ItemRarities.Epic, ItemCategories.Others, true, new ????? (6));
            // //Add(10502, "0", "Neon 12 colors", 750, ItemRarities.NotDrop, ItemCategories.Others, true, new ????? (12));
            // #endregion

            // #region TruckerLVL
            // Add(11000, "Frame659", "Trucker 10 EXP", 500, ItemRarities.Medium, ItemCategories.Others, true, new TruckExpDonateItem(10), weight: 10);
            // Add(11001, "Frame660", "Trucker 20 EXP", 800, ItemRarities.Hight, ItemCategories.Others, true, new TruckExpDonateItem(20), weight: 5);
            // Add(11002, "Frame661", "Trucker 30 EXP", 1000, ItemRarities.Legend, ItemCategories.Others, true, new TruckExpDonateItem(30));
            // #endregion

            // #region Licenses
            // /*
            //     "A",
            //     "B",
            //     "C",
            //     "D",
            //     "E",
            //     "F",
            //     "G",
            //     "MED",
            //     "mID"             
            // */
            // //Add(11500, "0", "Paramedic license ", 150, ItemRarities.Base, ItemCategories.Others, true, new LicenseDonateItem());
            // Add(11501, "pack_Frame658", "Taxi driver's license", 300, ItemRarities.Low, ItemCategories.Others, true, new LicenseDonateItem(LicenseName.Taxi));
            // //Add(11502, "0", "Metal smelting license ", 150, ItemRarities.Base, ItemCategories.Others, true, new LicenseDonateItem());
            // //Add(11503, "0", "Weapon assembly license ", 150, ItemRarities.Base, ItemCategories.Others, true, new LicenseDonateItem());
            // //Add(11504, "0", "Hunting license ", 150, ItemRarities.Base, ItemCategories.Others, true, new LicenseDonateItem());
            // //Add(11505, "0", "Fishing license ", 150, ItemRarities.Base, ItemCategories.Others, true, new LicenseDonateItem());
            // //Add(11506, "Frame693", "Military ID", 2000, ItemRarities.Low, ItemCategories.Others, true, new LicenseDonateItem(LicenseName.Military));
            // //Add(11507, "Frame694", "Medical card", 500, ItemRarities.Low, ItemCategories.Others, true, new LicenseDonateItem(LicenseName.Medical));
            // Add(11508, "Frame695", "Driver's license", 200, ItemRarities.Base, ItemCategories.Others, false, new LicenseDonateItem(LicenseName.Moto, LicenseName.Truck, LicenseName.Auto));
            // Add(11509, "Frame696", "Weapon license", 750, ItemRarities.Medium, ItemCategories.Others, true, new LicenseDonateItem(LicenseName.Weapon));
            // //Add(11509, "0", "Full stack licenses", 0, ItemRarities.Legend, ItemCategories.Others, false, new LicenseDonateItem());

            // //V2.0
            // Add(11510, "pack_Frame659", "B License", 100, ItemRarities.Base, ItemCategories.Others, true, new LicenseDonateItem(LicenseName.Auto));
            // Add(11511, "pack_Frame668", "D License", 100, ItemRarities.Base, ItemCategories.Others, true, new LicenseDonateItem(LicenseName.Truck));
            // Add(11512, "Frame810", "Taxi/Weapon license", 100, ItemRarities.Medium, ItemCategories.Others, true, new LicenseDonateItem(LicenseName.Weapon, LicenseName.Taxi));

            // #endregion

            // #region Business
            // Add(12000, "Frame690", "Business arms factory", 500000, ItemRarities.NotDrop, ItemCategories.Others, true, new BusiessDonateItem());
            // Add(12001, "Frame691", "Business furniture factory", 350000, ItemRarities.NotDrop, ItemCategories.Others, true, new BusiessDonateItem());
            // Add(12002, "Frame736", "Business casino", 1000000, ItemRarities.NotDrop, ItemCategories.Others, true, new BusiessDonateItem());
            // Add(12003, "Frame730", "Bugatti salon", 500000, ItemRarities.NotDrop, ItemCategories.Others, true, new BusiessDonateItem());
            // Add(12004, "Frame731", "BMW salon", 450000, ItemRarities.NotDrop, ItemCategories.Others, true, new BusiessDonateItem());
            // Add(12005, "Frame732", "Rolls Royce salon", 500000, ItemRarities.NotDrop, ItemCategories.Others, true, new BusiessDonateItem());
            // Add(12006, "Frame733", "Ferrari salon", 500000, ItemRarities.NotDrop, ItemCategories.Others, true, new BusiessDonateItem());
            // Add(12007, "Frame734", "Lamborghini salon ", 500000, ItemRarities.NotDrop, ItemCategories.Others, true, new BusiessDonateItem());
            // Add(12008, "Frame735", "Moto salon", 500000, ItemRarities.NotDrop, ItemCategories.Others, true, new BusiessDonateItem());
            // #endregion

            // #region Default Car
            // //DefaultCar
            // AddVehicle(12500, "carsFrame347", "faction2", true);
            // AddVehicle(12501, "carsFrame493", "impaler", true);
            // AddVehicle(12502, "carsFrame435", "baller", true);
            // AddVehicle(12503, "carsFrame497", "tampa", true);
            // AddVehicle(12504, "carsFrame464", "zion", true);
            // AddVehicle(12505, "carsFrame495", "vamos", true);
            // AddVehicle(12506, "carsFrame479", "sentinel2", true);
            // AddVehicle(12507, "carsFrame355", "patriot", true);
            // AddVehicle(12508, "carsFrame483", "zion2", true);
            // AddVehicle(12509, "carsFrame363", "voodoo", true);
            // AddVehicle(12510, "carsFrame351", "sentinel", true);
            // AddVehicle(12511, "carsFrame359", "asea", true);
            // AddVehicle(12512, "carsFrame331", "bmx", true);
            // AddVehicle(12513, "carsFrame275", "exemplar", true);
            // AddVehicle(12514, "carsFrame279", "carbonizzare", true);
            // AddVehicle(12515, "carsFrame339", "dominator2", true);
            // AddVehicle(12516, "carsFrame391", "surano", true);
            // AddVehicle(12517, "carsFrame247", "asbo", true);
            // AddVehicle(12518, "carsFrame251", "blista", true);
            // AddVehicle(12519, "carsFrame255", "brioso", true);
            // AddVehicle(12520, "carsFrame259", "dilettante", true);
            // AddVehicle(12521, "carsFrame263", "issi2", true);
            // AddVehicle(12522, "carsFrame267", "panto", true);
            // AddVehicle(12523, "carsFrame516", "manchez", true);
            // AddVehicle(12524, "carsFrame532", "esskey", true);
            // AddVehicle(12525, "carsFrame534", "sanchez", true);
            // AddVehicle(12526, "carsFrame535", "akuma", true);
            // AddVehicle(12527, "carsFrame367", "carbonizzare", true);
            // AddVehicle(12528, "carsFrame271", "cogcabrio", true);
            // AddVehicle(12529, "carsFrame291", "feltzer2", true);
            // AddVehicle(12530, "carsFrame315", "jester2", true);
            // //AddVehicle(12531, "carsFrame399", "massacro2",  true);
            // AddVehicle(12532, "carsFrame403", "gp1", true);
            // AddVehicle(12533, "carsFrame287", "elegy2", true);
            // AddVehicle(12534, "carsFrame447", "rebla", true);
            // AddVehicle(12535, "carsFrame505", "moonbeam", true);
            // AddVehicle(12536, "carsFrame512", "sabregt", true);
            // AddVehicle(12537, "carsFrame508", "rocoto", true);
            // AddVehicle(12538, "carsFrame529", "carbonrs", true);
            // AddVehicle(12539, "carsFrame530", "bf400", true);
            // AddVehicle(12540, "carsFrame531", "bati", true);
            // AddVehicle(12541, "carsFrame513", "lectro", true);
            // AddVehicle(12542, "carsFrame515", "nemesis", true);
            // AddVehicle(12543, "carsFrame455", "xa21", true);
            // AddVehicle(12544, "carsFrame417", "emerus", true);
            // AddVehicle(12545, "carsFrame395", "toros", true);
            // AddVehicle(12546, "carsFrame523", "kamacho", true);
            // AddVehicle(12547, "carsFrame525", "blazer2", true);
            // AddVehicle(12548, "carsFrame465", "sentinel3", true);
            // AddVehicle(12549, "carsFrame480", "seven70", true);
            // AddVehicle(12550, "carsFrame466", "specter2", true);
            // AddVehicle(12551, "carsFrame307", "gt500", true);
            // AddVehicle(12552, "carsFrame303", "tyrus", true);
            // AddVehicle(12553, "carsFrame335", "monroe", true);
            // AddVehicle(12554, "carsFrame323", "windsor", true);
            // AddVehicle(12555, "carsFrame375", "caracara2", true);
            // AddVehicle(12556, "carsFrame379", "dubsta2", true);
            // AddVehicle(12557, "carsFrame383", "entityxf", true);
            // AddVehicle(12558, "carsFrame387", "sandking2", true);
            // AddVehicle(12559, "carsFrame419", "neon", true);
            // AddVehicle(12560, "carsFrame423", "baller4", true);
            // AddVehicle(12561, "carsFrame451", "penetrator", true);
            // AddVehicle(12562, "carsFrame494", "ninef2", true);
            // AddVehicle(12563, "carsFrame463", "infernus", true);
            // AddVehicle(12564, "carsFrame283", "comet3", true);
            // AddVehicle(12565, "carsFrame299", "flashgt", true);
            // AddVehicle(12566, "carsFrame527", "blazer", true);
            // AddVehicle(12567, "carsFrame528", "double", true);
            // AddVehicle(12568, "carsFrame538", "daemon2", true);
            // AddVehicle(12569, "carsFrame539", "bagger", true);
            // AddVehicle(12570, "carsFrame459", "osiris", true);
            // AddVehicle(12571, "carsFrame311", "schlagen", true);
            // AddVehicle(12572, "carsFrame431", "nero", true);
            // AddVehicle(12573, "carsFrame319", "neo", true);
            // AddVehicle(12574, "carsFrame411", "adder", true);
            // AddVehicle(12575, "carsFrame427", "t20", true);
            // AddVehicle(12576, "carsFrame371", "dubsta3", true);
            // AddVehicle(12577, "carsFrame407", "italigtb", true);
            // AddVehicle(12578, "carsFrame522", "shotaro", true);
            // AddVehicle(12579, "carsFrame526", "marshall", true);
            // AddVehicle(12580, "carsFrame514", "deathbike", true);
            // AddVehicle(12581, "carsFrame327", "mamba", true);
            // AddVehicle(12582, "carsFrame417", "emerus", true);
            // AddVehicle(12583, "carsFrame524", "monster", true);
            // AddVehicle(12584, "carsFrame517", "sanctus", true);
            // AddVehicle(12585, "carsFrame343", "ztype", true);
            // AddVehicle(12586, "carsFrame443", "prototipo", true);
            // //AddVehicle(12587, "carsFrame519", "frogger",  false);
            // //AddVehicle(12588, "carsFrame520", "thruster",  false);
            // //AddVehicle(12589, "carsFrame521", "buzzard2",  false);

            // //V2.0
            // AddVehicle(12590, "pack_Frame669", "tornado3", true);

            // #endregion

            // #region Mod Car
            // //ModCar
            // AddVehicle(13001, "carsFrame491", "gogolf7s", false);
            // AddVehicle(13002, "carsFrame478", "goz350", false);
            // AddVehicle(13004, "carsFrame511", "bmwm5", false);
            // AddVehicle(13007, "carsFrame498", "gozl1", false);
            // //AddVehicle(13008, "carsFrame510", "bmwx5me70", false);
            // //AddVehicle(13010, "carsFrame504", "gtrr35",  true);
            // //AddVehicle(13011, "carsFrame507", "panamera17turbo",  true);
            // AddVehicle(13012, "carsFrame482", "bmw21m5", false);
            // AddVehicle(13013, "carsFrame469", "gocharger69", false);
            // //AddVehicle(13014, "carsFrame484", "gosupra",  false);
            // //AddVehicle(13015, "carsFrame476", "gobentley1",  true);
            // AddVehicle(13016, "carsFrame472", "gogtr50", false);
            // AddVehicle(13017, "carsFrame486", "gomartin", false);
            // //AddVehicle(13018, "carsFrame470", "gobentleygt",  true);
            // AddVehicle(13019, "carsFrame468", "gomayb900", false);
            // //AddVehicle(13020, "carsFrame485", "golp670",  true);
            // //AddVehicle(13021, "carsFrame501", "g65go",  false);
            // //AddVehicle(13022, "carsFrame506", "bentaygav8",  true);
            // //AddVehicle(13023, "carsFrame487", "g63amg6x6",  false);
            // //AddVehicle(13024, "carsFrame474", "gomonstergt",  true);
            // //AddVehicle(13025, "carsFrame488", "goamggt",  false);
            // //AddVehicle(13026, "carsFrame481", "gomonsterr34",  true);
            // //AddVehicle(13027, "carsFrame467", "gosianr",  true);
            // //AddVehicle(13028, "carsFrame490", "gobolide",  true);
            // //AddVehicle(13029, "carsFrame500", "gobacalar",  true);
            // AddVehicle(13030, "carsFrame502", "goveneno", true);
            // AddVehicle(13031, "carsFrame489", "go812", false);
            // //AddVehicle(13032, "carsFrame477", "gosian",  true);
            // //AddVehicle(13033, "carsFrame503", "gomonster",  true);
            // AddVehicle(13034, "carsFrame540", "goniva", false);
            // #endregion

            // #region Weapon
            // //Add(13500, "weapons_Frame557", "Carabine Rifle", 100, ItemRarities.Base, ItemCategories.Weapons, true, new WeaponDonateItem(ItemNames.CarbineRifle), weight: 10);
            // //Add(13501, "weapons_Frame563", "AssaultRifle", 25, ItemRarities.Base, ItemCategories.Weapons, false, new WeaponDonateItem(ItemNames.AssaultRifle), weight: 10);
            // //Add(13502, "weapons_Frame559", "Revolver", 100, ItemRarities.Low, ItemCategories.Weapons, false, new WeaponDonateItem(ItemNames.Revolver), weight: 5);
            // //Add(13503, "weapons_Frame558", "Golf Club", 700, ItemRarities.Hight, ItemCategories.Weapons, false, new WeaponDonateItem(ItemNames.GolfClub), weight: 7);
            // //Add(13504, "weapons_Frame555", "Broken Bottle", 1000, ItemRarities.Legend, ItemCategories.Weapons, false, new WeaponDonateItem(ItemNames.Bottle), weight: 7);
            // //Add(13505, "weapons_Frame562", "Baseball Bat", 1000, ItemRarities.Legend, ItemCategories.Weapons, false, new WeaponDonateItem(ItemNames.Bat), weight: 7);
            // //Add(13506, "weapons_Frame564", "SniperRifle", 2500, ItemRarities.Epic, ItemCategories.Weapons, false, new WeaponDonateItem(ItemNames.SniperRifle), weight: 2);
            // //Add(13507, "weapons_Frame561", "Heavy Shotgun", 3000, ItemRarities.Epic, ItemCategories.Weapons, true, new WeaponDonateItem(ItemNames.HeavyShotgun), weight: 3);
            // //Add(13508, "weapons_Frame552", "Flare Gun", 5000, ItemRarities.Epic, ItemCategories.Weapons, false, new WeaponDonateItem(ItemNames.FlareGun), weight: 2);
            // //Add(13509, "weapons_Frame554", "Combat MG", 5000, ItemRarities.Epic, ItemCategories.Weapons, false, new WeaponDonateItem(ItemNames.CombatMG), weight: 3);
            // //Add(13510, "weapons_Frame553", "Heavy Sniper", 10000, ItemRarities.NotDrop, ItemCategories.Weapons, false, new WeaponDonateItem(ItemNames.HeavySniper));

            // ////V2.0
            // //Add(13511, "pack_Frame625", "Wrench", 10, ItemRarities.Base, ItemCategories.Weapons, true, new WeaponDonateItem(ItemNames.Wrench), weight: 20);
            // //Add(13512, "pack_Frame643", "SMG", 100, ItemRarities.Low, ItemCategories.Weapons, true, new WeaponDonateItem(ItemNames.SMG), weight: 10);
            // //Add(13513, "pack_Frame646", "Crowbar", 5, ItemRarities.Base, ItemCategories.Weapons, true, new WeaponDonateItem(ItemNames.Crowbar), weight: 20);
            // //Add(13514, "pack_Frame645", "Hammer", 5, ItemRarities.Base, ItemCategories.Weapons, true, new WeaponDonateItem(ItemNames.Hammer), weight: 20);
            // //Add(13515, "pack_Frame652", "Pistol", 50, ItemRarities.Base, ItemCategories.Weapons, true, new WeaponDonateItem(ItemNames.Pistol), weight: 15);


            // #endregion

            // #region MedicineChest
            // Add(14000, "Frame755", "Medicine chest 5", 150, ItemRarities.Base, ItemCategories.Others, true, new MedicamentsDonateItem(ItemNames.HealthKit), 5, weight: 7);
            // Add(14001, "Frame756", "Medicine chest 10", 300, ItemRarities.Base, ItemCategories.Others, true, new MedicamentsDonateItem(ItemNames.HealthKit), 10, weight: 5);
            // Add(14002, "Frame757", "Medicine chest 15", 450, ItemRarities.Low, ItemCategories.Others, true, new MedicamentsDonateItem(ItemNames.HealthKit), 15);
            // Add(14003, "Frame758", "Medicine chest 20", 700, ItemRarities.Medium, ItemCategories.Others, true, new MedicamentsDonateItem(ItemNames.HealthKit), 20);
            // Add(14004, "Frame759", "Medicine chest 50", 1450, ItemRarities.Hight, ItemCategories.Others, true, new MedicamentsDonateItem(ItemNames.HealthKit), 50);

            // //V2.0
            // Add(14005, "pack_Frame662", "Medicine chest", 30, ItemRarities.Base, ItemCategories.Others, true, new MedicamentsDonateItem(ItemNames.HealthKit));

            // #endregion

            // #region Food
            // //Add(14500, "Frame746", "Burger 30", 75, ItemRarities.Medium, ItemCategories.Others, true, new FoodDonateItem(ItemNames.Burger), 30, weight: 20);
            // //Add(14501, "Frame747", "Burger 60", 150, ItemRarities.Medium, ItemCategories.Others, true, new FoodDonateItem(ItemNames.Burger), 100, weight: 15);
            // //Add(14502, "Frame748", "Burger 90", 200, ItemRarities.Medium, ItemCategories.Others, true, new FoodDonateItem(ItemNames.Burger), 150, weight: 7);
            // //Add(14503, "Frame749", "Burger 150", 300, ItemRarities.Medium, ItemCategories.Others, true, new FoodDonateItem(ItemNames.Burger), 200, weight: 2);
            // //Add(14504, "Frame750", "Burger 250", 500, ItemRarities.Hight, ItemCategories.Others, true, new FoodDonateItem(ItemNames.Burger), 250);

            // //V2.0
            // //Add(14505, "pack_Frame656", "Burger 10", 20, ItemRarities.Base, ItemCategories.Others, true, new FoodDonateItem(ItemNames.Burger), 10);
            // //Add(14506, "pack_Frame656", "Burgers 5", 10, ItemRarities.Base, ItemCategories.Others, true, new FoodDonateItem(ItemNames.Burger), 5);
            // //Add(14507, "pack_Frame672", "Pizza", 5, ItemRarities.Base, ItemCategories.Others, true, new FoodDonateItem(ItemNames.Pizza));

            // #endregion

            // #region Drink
            // //Add(15000, "Frame751", "Cola 10", 50, ItemRarities.Base, ItemCategories.Others, true, new DrinkDonateItem(ItemNames.eCola), 10, weight: 25);
            // //Add(15001, "Frame752", "Cola 30", 70, ItemRarities.Base, ItemCategories.Others, true, new DrinkDonateItem(ItemNames.eCola), 40, weight: 15);
            // //Add(15002, "Frame753", "Cola 50", 100, ItemRarities.Base, ItemCategories.Others, true, new DrinkDonateItem(ItemNames.eCola), 60, weight: 10);
            // //Add(15003, "Frame754", "Cola 70", 125, ItemRarities.Base, ItemCategories.Others, true, new DrinkDonateItem(ItemNames.eCola), 80, weight: 5);

            // //V2.0
            // //Add(15004, "pack_Frame671", "3 Cola", 5, ItemRarities.Base, ItemCategories.Others, true, new DrinkDonateItem(ItemNames.eCola), 3);
            // //Add(15005, "pack_Frame671", "Cola", 2, ItemRarities.Base, ItemCategories.Others, true, new DrinkDonateItem(ItemNames.eCola));
            // #endregion

            // #region CasinoChips
            // Add(15500, "Frame716", "Casino Chips 50k", 250, ItemRarities.Base, ItemCategories.Others, true, new CasinoChipsDonateItem(50000));
            // Add(15501, "Frame717", "Casino Chips 100k", 500, ItemRarities.Base, ItemCategories.Others, true, new CasinoChipsDonateItem(100000));
            // Add(15502, "Frame718", "Casino Chips 150k", 750, ItemRarities.Medium, ItemCategories.Others, true, new CasinoChipsDonateItem(150000));
            // Add(15503, "Frame719", "Casino Chips 200k", 1000, ItemRarities.Medium, ItemCategories.Others, true, new CasinoChipsDonateItem(200000));
            // Add(15504, "Frame720", "Casino Chips 250k", 1250, ItemRarities.Medium, ItemCategories.Others, true, new CasinoChipsDonateItem(250000));
            // Add(15505, "Frame721", "Casino Chips 300k", 1500, ItemRarities.Hight, ItemCategories.Others, true, new CasinoChipsDonateItem(300000));
            // Add(15506, "Frame722", "Casino Chips 500k", 2000, ItemRarities.Hight, ItemCategories.Others, true, new CasinoChipsDonateItem(500000));
            // Add(15507, "Frame723", "Casino Chips 1kk", 2500, ItemRarities.Legend, ItemCategories.Others, true, new CasinoChipsDonateItem(1000000));
            // //Add(15508, "Frame724", "Casino Chips 2kk", 2750, ItemRarities.Epic, ItemCategories.Others, true, new CasinoChipsDonateItem(2000000));
            // #endregion

            // #region Garage
            // Add(16000, "Frame681", "6 parking spaces", 6000, ItemRarities.Legend, ItemCategories.Others, true, new ParkingDonateItem(6));
            // Add(16001, "Frame682", "10 parking spaces", 10000, ItemRarities.Epic, ItemCategories.Others, true, new ParkingDonateItem(10));
            // //Add(16002, "Frame683", "30 parking spaces", 30000, ItemRarities.Epic, ItemCategories.Others, true, new ParkingDonateItem(30));
            // //Add(16003, "Frame684", "50 parking spaces", 50000, ItemRarities.NotDrop, ItemCategories.Others, true, new ParkingDonateItem(50));
            // //Add(16004, "Frame685", "52 parking spaces", 52000, ItemRarities.NotDrop, ItemCategories.Others, true, new ParkingDonateItem(52));
            // #endregion

            // #region Salary Boost (Zarplata)
            // //Add(16579, "x2 for 1 day ", 0, ItemRarities.Base, ItemCategories.Others, false, new ?????(2,1));
            // //Add(16580, "x2 for 2 day ", 0, ItemRarities.Low, ItemCategories.Others, false, new ?????(2,2));
            // //Add(16581, "x2 for 3 day ", 0, ItemRarities.Medium, ItemCategories.Others, false, new ?????(2,3));
            // //Add(16582, "x3 for 1 day ", 0, ItemRarities.Medium, ItemCategories.Others, false, new ?????(3,1));
            // //Add(16583, "x3 for 2 day ", 0, ItemRarities.Hight, ItemCategories.Others, false, new ?????(3,2));
            // //Add(16584, "x3 for 3 day ", 0, ItemRarities.Legend, ItemCategories.Others, false, new ?????(3,3));
            // //Add(16585, "x5 for 1 day ", 0, ItemRarities.Epic, ItemCategories.Others, false, new ?????(5,1));
            // #endregion

            // #region Payday Boost
            // //Add(17086, "x2 payday for 1 day ", 0, ItemRarities.Base, ItemCategories.Others, false, new ?????(2,1));
            // //Add(17087, "x2 payday for 2 day ", 0, ItemRarities.Low, ItemCategories.Others, false, new ?????(2,2));
            // //Add(17088, "x2 payday for 3 day ", 0, ItemRarities.Medium, ItemCategories.Others, false, new ?????(2,3));
            // #endregion

            // #region GoCoin Boost
            // //Add(17589, "x3 free coins for 6 hours for 1 day ", 0, ItemRarities.Low, ItemCategories.Others, false, new ?????(3,1));
            // //Add(17590, "x3 free coins for 6 hours for 2 day ", 0, ItemRarities.Medium, ItemCategories.Others, false, new ?????(3,2));
            // //Add(17591, "x3 free coins for 6 hours for 3 day ", 0, ItemRarities.Hight, ItemCategories.Others, false, new ?????(3,3));
            // #endregion

            // #region Family Boost 
            // //Add(18092, "Family boost + 10", 0, ItemRarities.Hight, ItemCategories.Others, false, new ?????(10));
            // //Add(18093, "Family boost + 20", 0, ItemRarities.Legend, ItemCategories.Others, false, new ?????(20));
            // //Add(18094, "Family boost + 50", 0, ItemRarities.Epic, ItemCategories.Others, false, new ?????(30));
            // #endregion

            // #region Other Items
            // //Add(18500, "Frame770", "Guitar", 100, ItemRarities.Base, ItemCategories.Others, true, new OtherDonateItem(ItemNames.Guitar), weight: 10);
            // //Add(18501, "0", "Radio", 0, ItemRarities.Base, ItemCategories.Others, false, new OtherDonateItem(ItemNames.Radio));
            // //Add(18502, "Frame771", "Video camera", 100, ItemRarities.Base, ItemCategories.Others, true, new OtherDonateItem(ItemNames.Camera), weight: 10);
            // //Add(18503, "Frame769", "10 gas can", 100, ItemRarities.Base, ItemCategories.Others, true, new GasCanDonateItem(ItemNames.GasCan), 10, weight: 5);


            // //V 2.0
            // Add(18507, "pack_Frame623", "3 Canisters of Gasoline", 10, ItemRarities.Base, ItemCategories.Others, true, new GasCanDonateItem(ItemNames.GasCan), 3, weight: 10);
            // Add(18508, "pack_Frame628", "5 Cuffs", 30, ItemRarities.Base, ItemCategories.Others, true, new OtherDonateItem(ItemNames.Cuffs), 5, weight: 10);
            // Add(18509, "pack_Frame637", "Microphone", 100, ItemRarities.Low, ItemCategories.Others, true, new OtherDonateItem(ItemNames.Microphone), weight: 10);
            // Add(18510, "pack_Frame639", "Tablet", 650, ItemRarities.Hight, ItemCategories.Others, true, new OtherDonateItem(ItemNames.Pocket));
            // Add(18511, "pack_Frame641", "Walkie-talkie", 200, ItemRarities.Low, ItemCategories.Others, true, new OtherDonateItem(ItemNames.Radio));
            // Add(18512, "pack_Frame673", "Cigarettes", 5, ItemRarities.Base, ItemCategories.Others, true, new OtherDonateItem(ItemNames.Cigarettes), weight: 20);
            // Add(18513, "pack_Frame628", "3 Cuffs", 20, ItemRarities.Base, ItemCategories.Others, true, new OtherDonateItem(ItemNames.Cuffs), 3, weight: 20);
            // Add(18514, "pack_Frame628", "10 Cuffs", 60, ItemRarities.Base, ItemCategories.Others, true, new OtherDonateItem(ItemNames.Cuffs), 10, weight: 5);
            // Add(18515, "pack_Frame653", "10 Pocket", 40, ItemRarities.Base, ItemCategories.Others, true, new OtherDonateItem(ItemNames.Pocket), 10, weight: 5);


            // #endregion

            // #region Dogs
            // Add(19000, "petsFrame538", "Husky", 10000, ItemRarities.NotDrop, ItemCategories.Animals, false, new PetDonateItem(1318032802));
            // Add(19001, "petsFrame539", "Chop", 10000, ItemRarities.NotDrop, ItemCategories.Animals, false, new PetDonateItem(351016938));
            // Add(19002, "petsFrame540", "Shepard", 10000, ItemRarities.NotDrop, ItemCategories.Animals, false, new PetDonateItem(1126154828));
            // Add(19003, "petsFrame541", "Westy", 10000, ItemRarities.NotDrop, ItemCategories.Animals, false, new PetDonateItem(-1384627013));
            // Add(19004, "petsFrame542", "Poodle", 10000, ItemRarities.NotDrop, ItemCategories.Animals, false, new PetDonateItem(1125994524));
            // Add(19005, "petsFrame543", "Pug", 10000, ItemRarities.NotDrop, ItemCategories.Animals, false, new PetDonateItem(1832265812));
            // #endregion

            // #region Temporary car
            // //Add(19500, "Frame737", "Super car for 3 days", 0, ItemRarities.Medium, ItemCategories.Others, false, new ?????? (&& &));
            // //Add(19501, "Frame738", "Super car for 5 days", 0, ItemRarities.Hight, ItemCategories.Others, false, new ?????? (&& &));
            // //Add(19502, "Frame739", "Super car for 10 days", 0, ItemRarities.Legend, ItemCategories.Others, false, new ?????? (&& &));
            // #endregion

            // #region Additional Features 
            // //Add(20000, "Frame709", "Unwarn", 1000, ItemRarities.Base, ItemCategories.Others, false, new UnwarnDonateItem(), weight: 3);
            // //Add(20001, "Frame710", "Release from demorgan", 500, ItemRarities.Base, ItemCategories.Others, false, new UndemorganDonateItem(), weight: 3);
            // //Add(20002, "Frame711", "Release from prison", 500, ItemRarities.Base, ItemCategories.Others, false, new PrisonReleaseDonateItem(), weight: 3);
            // Add(20003, "Frame712", "Change face", 500, ItemRarities.Base, ItemCategories.Others, false, new CharacterCreatorDonateItem(), weight: 2);
            // //Add(20004, "Frame713", "Change name", 500, ItemRarities.Base, ItemCategories.Others, false, new ChangeNameDonateItem(), weight: 3);
            // Add(20005, "Frame714", "Icon under nickname", 15000, ItemRarities.Epic, ItemCategories.Others, false, new IconUnderNicknameDonateItem());
            // Add(20006, "Frame715", "Create organiation", 2000, ItemRarities.Legend, ItemCategories.Others, false, new OrganizqtionDonateItem());
            // //Add(20007, "Frame740", "Dream party", 10000, ItemRarities.Epic, ItemCategories.Others, false, new DreamEventDonateItem());
            // Add(20008, "Frame741", "Romantic on submarine", 10000, ItemRarities.Legend, ItemCategories.Others, false, new RomanticDonateItem());
            // //Add(20009, "Frame742", "Apartments in Arcadius", 25000, ItemRarities.Epic, ItemCategories.Others, false, new HouseDonateItem());
            // //Add(20010, "Frame743", "Dream house", 10000, ItemRarities.NotDrop, ItemCategories.Others, true, new HouseDonateItem());
            // //Add(20011, "0", "2 free speen", 0, ItemRarities.Low, ItemCategories.Others, false, new ?????? (&& &));
            // //Add(20012, "Frame744", "Donate x2 (10k - 30k)", 0, ItemRarities.Legend, ItemCategories.Others, false, new ?????? (&& &));
            // //Add(20013, "Frame745", "Donate x3 (10k- 30k)", 0, ItemRarities.Epic, ItemCategories.Others, false, new ?????? (&& &));
            // //Add(20013, "Frame689", "Prime weapon for month", 0, ItemRarities.NotDrop, ItemCategories.Others, false, new ?????? (&& &));
            // #endregion

            // #region Hats Male
            // Add(21000, "pack_Frame624", "Yellow helmet", 500, ItemRarities.Medium, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Hat, 145, 0, true));
            // #endregion

            // #region Hats Female
            // Add(21500, "pack_Frame624", "Yellow helmet", 500, ItemRarities.Medium, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Hat, 144, 0, false));
            // #endregion

            // #region Feets Male

            // //V2.0
            // Add(22000, "pack_Frame685", "Converse Black Feet", 1000, ItemRarities.Medium, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Feet, 504, 5, true));

            // #endregion

            // #region Feets Female

            // //V2.0
            // Add(22500, "pack_Frame685", "Converse Black Feet", 1000, ItemRarities.Medium, ItemCategories.Clothes, true, new ClothesDonateItem(ItemNames.Feet, 504, 5, false));

            // #endregion

            // #region Costume Male

            // //V2.0
            // //Add(23000, "pack_Frame674", "Military suit", 2000, ItemRarities.Epic, ItemCategories.Clothes, true, new CostumeDonateItem(CostumeNames.MFR14Camo1, true));
            // //Add(23001, "pack_Frame676", "Medical outfit", 2000, ItemRarities.Epic, ItemCategories.Clothes, true, new CostumeDonateItem(CostumeNames.MFR8Work, true));
            // #endregion

            // #region Costume Female

            // //V2.0
            // //Add(23500, "pack_Frame674", "Military suit", 2000, ItemRarities.Epic, ItemCategories.Clothes, true, new CostumeDonateItem(CostumeNames.FFR14Camo1, false));
            // //Add(23501, "pack_Frame676", "Medical outfit", 2000, ItemRarities.Epic, ItemCategories.Clothes, true, new CostumeDonateItem(CostumeNames.FFR8Work, false));
            // #endregion

            // #region Ammo

            // //V2.0
            // Add(24000, "pack_Frame627", "200 Rounds", 50, ItemRarities.Base, ItemCategories.Others, true, new AmmoDonateItem(ItemNames.RiflesAmmo), 200, weight: 10);
            // Add(24001, "pack_Frame627", "100 Rounds", 25, ItemRarities.Base, ItemCategories.Others, true, new AmmoDonateItem(ItemNames.PistolAmmo), 100, weight: 10);
            // Add(24002, "pack_Frame627", "200 Rounds", 50, ItemRarities.Base, ItemCategories.Others, true, new AmmoDonateItem(ItemNames.SMGAmmo), 200, weight: 10);

            // #endregion

            // #region Alcohol

            // //V2.0
            // Add(24500, "pack_Frame670", "Beer", 5, ItemRarities.Base, ItemCategories.Others, true, new AlcoholDonateItem(ItemNames.Beer));

            // #endregion

            // #region Fishing

            // //V2.0
            // Add(25000, "pack_Frame648", "Middle Rod", 150, ItemRarities.Low, ItemCategories.Others, true, new RodDonateItem(ItemNames.MiddleRod), weight: 5);
            // Add(25001, "pack_Frame650", "Middle Fishing Cage", 50, ItemRarities.Low, ItemCategories.Others, true, new CageDonateItem(ItemNames.MiddleFishingCage), weight: 5);
            // Add(25002, "pack_Frame649", "30 FishingBait", 15, ItemRarities.Base, ItemCategories.Others, true, new OtherDonateItem(ItemNames.FishingBait), 30, weight: 15);
            // #endregion

            // #region Narcotic

            // //V2.0
            // Add(25500, "pack_Frame661", "5 Drugs", 20, ItemRarities.Base, ItemCategories.Others, true, new NarcoticDonateItem(ItemNames.Marijuana), 5);
            // #endregion

            // #region Complects
            // //Complects
            // //V2.0
            // Add(26000, "Frame794", "Mechanick pack", -1, ItemRarities.Low, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { 18507,/* 13511,*/ 21000 }, true), weight: 10);
            // Add(26001, "Frame794", "Mechanick pack", -1, ItemRarities.Low, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { 18507, /*13511,*/ 21500 }, false), weight: 10);
            // Add(26002, "Frame795", "Mafia pack", -1, ItemRarities.Medium, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { /*13501,*/ 5636, 24000, 18508 }, true), weight: 7);
            // Add(26003, "Frame795", "Mafia pack", -1, ItemRarities.Medium, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { /*13501,*/ 5637, 24000, 18508 }, false), weight: 7);
            // Add(26004, "Frame796", "Gangster pack", -1, ItemRarities.Medium, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { /*13502,*/ 5638, 24001, 18508 }, true), weight: 17);
            // Add(26005, "Frame796", "Gangster pack", -1, ItemRarities.Medium, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { /*13502,*/ 5639, 24001, 18508 }, false), weight: 7);
            // //Add(26006, "Frame797", "Police pack", -1, ItemRarities.Low, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { /*13500,*/ 4500, 24000 }, true), weight: 10);
            // //Add(26007, "Frame797", "Police pack", -1, ItemRarities.Low, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { /*13500,*/ 4507, 24000 }, false), weight: 10);
            // Add(26008, "Frame799", "Journalist pack", -1, ItemRarities.Medium, ItemCategories.Others, true, new ComplectDonateItems(10, new List<int> { 18502, 18509 }), weight: 7);
            // Add(26009, "Frame800", "Gamer pack", -1, ItemRarities.Medium, ItemCategories.Others, true, new ComplectDonateItems(10, new List<int> { 18510, 14506, 15004 }), weight: 7);
            // Add(26010, "Frame801", "Music pack", -1, ItemRarities.Medium, ItemCategories.Others, true, new ComplectDonateItems(10, new List<int> { 18500, 18509 }), weight: 7);
            // //Add(26011, "Frame802", "Agent pack", -1, ItemRarities.Medium, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { 18511, 5640, /*13512,*/ 24002}, true), weight: 7);
            // //Add(26012, "Frame802", "Agent pack", -1, ItemRarities.Medium, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { 18511, 5641, /*13512,*/ 24002}, false), weight: 7);
            // Add(26013, "Frame803", "Homeless pack", -1, ItemRarities.Low, ItemCategories.Others, true, new ComplectDonateItems(10, new List<int> { 24500, 18512, 14507, 15005, 12590 }), weight: 10);
            // Add(26014, "Frame804", "Theif pack", -1, ItemRarities.Base, ItemCategories.Others, true, new ComplectDonateItems(10, new List<int> { /*13513, 13514,*/ 18513 }), weight: 12);
            // Add(26015, "Frame805", "Fisher pack", -1, ItemRarities.Medium, ItemCategories.Others, true, new ComplectDonateItems(10, new List<int> { 25000, 25001, 25002 }), weight: 7);
            // Add(26016, "Frame806", "Robber pack", -1, ItemRarities.Base, ItemCategories.Others, true, new ComplectDonateItems(10, new List<int> { /*13515,*/ 18514, 18515 }), weight: 12);
            // //Add(26017, "Frame808", "Bloger food pack", -1, ItemRarities.Epic, ItemCategories.Others, true, new ComplectDonateItems(10, new List<int> { 5062, 18509, 14505 }), weight: 3);
            // Add(26018, "Frame812", "Capter pack", -1, ItemRarities.Medium, ItemCategories.Others, true, new ComplectDonateItems(10, new List<int> { /*13502,*/ 25500, 14005, 24001 }), weight: 7);
            // Add(26019, "Frame813", "Bizwar pack", -1, ItemRarities.Medium, ItemCategories.Others, true, new ComplectDonateItems(10, new List<int> { /*13501,*/ 24000, 14005, 25500 }), weight: 7);
            // Add(26020, "Frame807", "Elegant pack", -1, ItemRarities.Epic, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { 6000, 7004, 22000 }, true), weight: 3);
            // Add(26021, "Frame807", "Elegant pack", -1, ItemRarities.Epic, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { 6585, 7543, 22500 }, false), weight: 3);
            // Add(26022, "Frame809", "Carnaval pack", -1, ItemRarities.Hight, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { 5642, 5644 }, true), weight: 5);
            // Add(26023, "Frame809", "Carnaval pack", -1, ItemRarities.Hight, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { 5643, 5645 }, false), weight: 5);
            // //Add(26026, "Frame814", "Military pack", -1, ItemRarities.Epic, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { 11506, 23000 }, true), weight: 3);
            // //Add(26027, "Frame814", "Military pack", -1, ItemRarities.Epic, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { 11506, 23500 }, false), weight: 3);
            // //Add(26028, "Frame815", "Doctor pack", -1, ItemRarities.Epic, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { 11507, 23001 }, true), weight: 3);
            // //Add(26029, "Frame815", "Doctor pack", -1, ItemRarities.Epic, ItemCategories.Others, true, new ComplectGenderDonateItem(10, new List<int> { 11507, 23501 }, false), weight: 3);


            // #endregion
            // //var prices = All.OrderBy(i => i.Value.Price).Select(i=>i.Value.Price).ToList();
            // //Console.WriteLine(JsonConvert.SerializeObject(prices));
        }

        private void Add(int id, string image, string name, int price, ItemRarities rarity, ItemCategories category, bool exclusive, BaseDonateItem item = null, int count = 1, int weight = 1)
        {
            if(item is ComplectGenderDonateItem)
            {
                var countRemoved = ((ComplectGenderDonateItem)item).Items.RemoveAll(i => !All.ContainsKey(i));
                if(countRemoved > 0)
                    Console.WriteLine($"removed from {id}: {countRemoved}");
            }
            if (item is ComplectDonateItems)
            {
                var countRemoved = ((ComplectDonateItems)item).Items.RemoveAll(i => !All.ContainsKey(i));
                if(countRemoved > 0)
                    Console.WriteLine($"removed from {id}: {countRemoved}");
            }


            All.Add(id, new ItemModel(id, image, name, price, RouletteConfig.RarityPrice.FirstOrDefault(r => r.Value.MinPrice <= price && price < r.Value.MaxPrice).Key, category, exclusive, item, count, weight));
        }

        private void AddVehicle(int id, string image, string name, bool exclusive)
        {
            var price = PriceManager.GetPrice(TypePrice.Car, name);
            if (price < 1)
            {
                _logger.WriteWarning("no price config for vehicle: " + name);
                return;
            }
            All.Add(id, new ItemModel(id, image, name, price, RouletteConfig.RarityPrice.FirstOrDefault(r => r.Value.MinPrice <= price && price < r.Value.MaxPrice).Key, ItemCategories.Vehicles, exclusive, new VehicleDonateItem(name), 1, 1));
        }

        private void AddMoney(int id, int from, int to, bool exclusive, int count = 1, int weight = 1)
        {
            var price = (from + to) / 2 / Main.ServerConfig.DonateConfig.CoinToVirtual;
            var rarity = RouletteConfig.RarityPrice.FirstOrDefault(r => r.Value.MinPrice <= price && price < r.Value.MaxPrice).Key;
            var image = rarity == ItemRarities.Hight ? "Frame774" : $"Frame{647 + rarity}";
            All.Add(id, new ItemModel(id, image, $"Money from {Math.Round(from / 1000.0, 1)}k to {Math.Round(to / 1000.0, 1)}k", price, rarity, ItemCategories.Others, exclusive, new MoneyDonateItem(from, to), count, weight));
        }
        private void AddCoins(int id, int from, int to, bool exclusive, int count = 1, int weight = 1)
        {
            var price = (from + to) / 2;
            var rarity = RouletteConfig.RarityPrice.FirstOrDefault(r => r.Value.MinPrice <= price && price < r.Value.MaxPrice).Key;
            var image = rarity == ItemRarities.Legend ? "Frame773" : $"Frame{653 + rarity}";
            All.Add(id, new ItemModel(id, image, $"Coins from {from} to {to}", price, rarity, ItemCategories.Others, exclusive, new CoinDonateItem(from, to), count, weight));
        }

        internal List<ItemModel> this[ItemRarities rarity]
        {
            get
            {
                if (!_cache.ContainsKey(rarity)) _cache.Add(rarity, All.Values.Where(i => i.Rarity == rarity).ToList());
                return _cache[rarity];
            }
        }
        internal ItemModel this[int id]
        {
            get
            {
                return All[id];
            }
        }
        public void ParseConfigs()
        {
            var _allIds = All.Values.Select(i => i.Id).ToList();
            using var r1 = new StreamWriter("ParsedConfigs/allItemsId.json");
            r1.Write(JsonConvert.SerializeObject(_allIds));
            using var r2 = new StreamWriter("interfaces/gui/src/configs/newDonateShop/items.js");
            r2.Write($"export default {JsonConvert.SerializeObject(All)}");
        }

        //public List<(int, int)> GetVehiclePrices()
        //{
        //    return All.Where(item => item.Value.Data is VehicleDonateItem).Select(item => (item.Value.Id, item.Value.Price)).ToList();
        //}

        internal List<ItemModel> GetItemsByPriceRange(int min, int max)
        {
            return All.Where(i => i.Value.Price >= min && i.Value.Price <= max).Select(i => i.Value).ToList();
        }

        private string _pricesDataCache;
        internal void SetUpdatedPrices(ExtPlayer player)
        {
            if (_pricesDataCache == null)
                UpdatePricesCacheData();
            player.TriggerCefEvent("newDonateShop/setUpdatedPrices", _pricesDataCache);
        }

        private void UpdatePricesCacheData()
        {
            var _data = All.Where(i => i.Value.Data is MoneyDonateItem || i.Value.Data is CoinDonateItem || i.Value.Data is VehicleDonateItem).Select(i => new int[] { i.Key, i.Value.Price }).ToList();
            _pricesDataCache = JsonConvert.SerializeObject(_data);
            //Console.WriteLine(_pricesDataCache);
        }

        internal void UpdateVehiclePrice(string name, int priceInCoins, int priceInDollar)
        {
            var car = All.FirstOrDefault(item => (item.Value.Data as VehicleDonateItem)?.Model == name);
            if (car.Value != null)
            {
                car.Value.Price = priceInCoins;
                var data = JsonConvert.SerializeObject(new int[] { car.Key, car.Value.Price });
                UpdatePricesCacheData();
                Main.ForEachAllPlayer((player) => player.TriggerCefEvent("newDonateShop/updateUpdatedPrices", data));
            }
        }
    }
}
