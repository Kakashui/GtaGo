using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Whistler.Inventory.Models;
using Whistler.Inventory.Enums;
using Whistler.Inventory;

namespace Whistler.AlcoholBar.Configs
{
    class BarConfig
    {
        private Dictionary<int, AlcoItem> _config;
        public Dictionary<int, int> Discounts;
        private int _id = 0;
        private Random _random;
        public BarConfig()
        {
            _config = new Dictionary<int, AlcoItem>();
            Discounts = new Dictionary<int, int>();
            Add("Red wine", "redWine.png", 2000, ItemNames.RedWine, 10);
            Add("Negroni", "negroni.png", 500, ItemNames.Negroni);
            Add("Pinacolada", "pinacolada.png", 500, ItemNames.Pinacolada, 15);
            Add("Mojito", "mojito.png", 450, ItemNames.Mojito);
            Add("Daiquiri", "daiquiri.png", 350, ItemNames.Daiquiri);
            Add("Tequila Sunrise", "tequilaSunrise.png", 10000, ItemNames.TequilaSunrise, 20);
            Add("Cristal", "cristal.png", 35000, ItemNames.Cristal);
            Add("Lambrusco", "lambrusco.png", 699, ItemNames.Lambrusco);
            Add("Chacha", "chacha.png", 399, ItemNames.Chacha);
            Add("Margarita", "margarita.png", 299, ItemNames.Margarita);
            Add("Alexandra", "alexandra.png", 700, ItemNames.Alexandra);
            Add("Whiskey", "whiskey.png", 24990, ItemNames.Whiskey);
            Add("Tequila", "tequila.png", 15000, ItemNames.Tequila);
            Add("Rom", "rom.png", 10000, ItemNames.Rom);
            Add("Vodka", "vodka.png", 4000, ItemNames.Vodka);
            Add("White Wine", "whiteWine.png", 2000, ItemNames.WhiteWine);
            Add("Beer", "beer.png", 500, ItemNames.Beer);
            Add("Cognac", "cognac.png", 10000, ItemNames.Cognac);
            Add("LaurentPerrier", "laurentPerrier.png", 1000, ItemNames.LaurentPerrier);
            Add("Moonshine", "moonshine.png", 3000, ItemNames.Moonshine);
        }
        public AlcoItem this[int id]
        {
            get
            {
                return _config[id];
            }
        }

        public int GetItemIdByName(string name)
        {
            if (!_config.Any()) return -1;

            AlcoItem item = _config.Values.FirstOrDefault(x => x.Name == name);
            if (item == null) return -1;

            return item.Id;
        }

        public AlcoItem GetItemByName(string name)
        {
            if (!_config.Any()) return null;

            AlcoItem item = _config.Values.FirstOrDefault(x => x.Name == name);
            if (item == null) return null;

            return item;
        }

        public void SetRandomDoscount(int discount)
        {
            if (_random == null) _random = new Random();
            var keys = _config.Keys.ToList();
            var random = keys[_random.Next(0, keys.Count)];
            SetDiscount(random, discount);
        }
        public void SetDiscount(int id, int discount)
        {
            if (Discounts.ContainsKey(id))
            {
                if (discount == 0)
                    Discounts.Remove(discount);
                else
                    Discounts[id] = discount;
            }
            else if (discount != 0)
                Discounts.Add(id, discount);
        }
        internal void Parse()
        {
            if (Directory.Exists("interfaces/gui/src/configs/bar"))
            {
                using var r1 = new StreamWriter("interfaces/gui/src/configs/bar/products.js");
                r1.Write($"export default {JsonConvert.SerializeObject(_config.Values, Formatting.Indented)}");
            }
        }

        private void Add(string name, string img, int price, ItemNames itemName, int discount = 0)
        {

            var item = new AlcoItem
            {
                Id = _id++,
                Name = name,
                Image = img,
                Price = price,
                Discount = discount,
                Item = ItemsFabric.CreateAlcohol(itemName, 1, false, true)
            };
            _config.Add(item.Id, item);
        }
    }
}
