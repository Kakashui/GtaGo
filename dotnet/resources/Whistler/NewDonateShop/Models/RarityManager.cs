using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.NewDonateShop.Enums;
using Newtonsoft.Json;

namespace Whistler.NewDonateShop.Models
{
    class RarityManager
    { 
        private Dictionary<ItemRarities, RarityModel> _rarityChance;

        private Dictionary<int, ItemRarities> _range = new Dictionary<int, ItemRarities>();
        private Random _rand = new Random();
        private int _rangeValue = 0;
        public RarityManager() {
            _rarityChance = new Dictionary<ItemRarities, RarityModel>();
        }
        public RarityManager(string data)
        {
            _rarityChance = JsonConvert.DeserializeObject<Dictionary<ItemRarities, RarityModel>>(data);
            Update();
        }
        public void SetChance(ItemRarities rarity, int chance, bool update = false)
        {
            if (_rarityChance.ContainsKey(rarity))
            {
                _rarityChance[rarity].Chance = chance;                
            }
            else _rarityChance.Add(rarity, new RarityModel(chance));

            if(update)
                Update();
        }

        public ItemRarities GetRandom()
        {
            var rand = _rand.Next(0, _rangeValue);
            foreach (var item in _range)
            {
                if (item.Key > rand) return item.Value;
            }
            return ItemRarities.Base;
        }
        private void Update()
        {
            _rangeValue = 0;
            _range.Clear();
            foreach (var item in _rarityChance.OrderBy(i=>i.Value.Chance))
            {
                _rangeValue += item.Value.Chance;
                _range.Add(_rangeValue, item.Key);
            }
            _rangeValue++;
        }

        public string GetChanceData()
        {
            return JsonConvert.SerializeObject(_rarityChance);
        }
    }
}
