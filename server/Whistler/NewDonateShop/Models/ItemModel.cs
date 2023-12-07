using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Models;
using Whistler.NewDonateShop.Enums;
using Whistler.PriceSystem;

namespace Whistler.NewDonateShop.Models
{
    class ItemModel
    {
        public ItemModel(int id, string image, string name, int price, ItemRarities rarity, ItemCategories category, bool exclusive, BaseDonateItem data, int count, int weight)
        {
            Id = id;
            Name = name;
            Price = price;
            Rarity = rarity;
            Category = category;
            Exclusive = exclusive;
            Data = data;
            Image = image;
            Count = count;
            Weight = weight;
        }
        private int _price;

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "weight")]
        public int Weight { get; set; }
        [JsonProperty(PropertyName = "price")]
        public int Price { 
            get {
                if (Data is ComplectDonateItems)
                    return GetComplectPrice(((ComplectDonateItems)Data).Items);
                else if(Data is ComplectGenderDonateItem)
                    return GetComplectPrice(((ComplectGenderDonateItem)Data).Items);
                else if (Data is VehicleDonateItem)
                    return PriceManager.GetPrice(TypePrice.Car, (Data as VehicleDonateItem).Model, 500000);
                else 
                    return _price;
            }
            set {
                _price = value;
            } 
        }

        private int GetComplectPrice(List<int> items)
        {
            var result = 0;
            items.ForEach(i =>
            {
                result += DonateService.Items[i].Price;
            });
            return result;// * data.Discount / data.Discount;
        }
        [JsonProperty(PropertyName = "rarity")]
        public ItemRarities Rarity { get; set; }
        [JsonProperty(PropertyName = "category")]
        public ItemCategories Category { get; set; }
        public bool Exclusive { get; set; }
        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "data")]
        public BaseDonateItem Data { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
    }
}
