using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Whistler.SDK;
using Whistler.Core;
using Whistler.VehicleSystem;
using Whistler.Businesses.Manager;
using Whistler.PriceSystem;
using Whistler.Helpers;

namespace Whistler.Businesses
{
    class BusinessesSettings
    {
        //public static Dictionary<int, List<ProductSettings>> BusinessSettings = new Dictionary<int, List<ProductSettings>>();

        public static Dictionary<int, BusinessTypeModel> BusinessTypes = new Dictionary<int, BusinessTypeModel>();

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(BusinessesSettings));

        public static void Load()
        {
            var result = MySQL.QueryRead("SELECT * FROM bizsettings");
            if (result == null || result.Rows.Count == 0)
            {
                _logger.WriteError("DB bizsettings return null result.");
                return;
            }
            foreach (DataRow Row in result.Rows)
            {
                //int biztype = Convert.ToInt32(Row["biztype"]);
                //List<ProductSettings> settings = JsonConvert.DeserializeObject<List<ProductSettings>>(Row["settings"].ToString());
                //BusinessSettings.Add(biztype, settings);
                //var bizList = BusinessManager.BizList.Values.Where(b=>b.Type == biztype).ToList();
                //foreach (var biz in bizList)
                //biz?.Orders.RemoveAll(o => o.Products.Any(p => settings.All(s => s.Name != p.Item1)));


                var bizSetts = new BusinessTypeModel(Row);
                BusinessTypes.Add(bizSetts.BizType, bizSetts);
            }
            
            //MakeNormalProductNames();
        }

        public static List<ProductSettings> GetProductSettings(int bizType)
        {
            if (BusinessTypes.ContainsKey(bizType))
                return BusinessTypes[bizType].Products;
            else
                return new List<ProductSettings>();
        }

        public static BusinessTypeModel GetBusinessSettings(int bizType)
        {
            return BusinessTypes.ContainsKey(bizType) ? BusinessTypes[bizType] : null;
        }

        private static void Save(int type)
        {
            MySQL.Query("UPDATE bizsettings SET settings=@prop0 WHERE biztype=@prop1", JsonConvert.SerializeObject(GetProductSettings(type)), type);
        }

        public static bool ChangeOrderPrice(int biztype, string productName, int newValue)
        {
            if (BusinessManager.AutoroomsBizTypes.Contains(biztype))
                return false;
            GetProductSettings(biztype).Find(s => s.Name == productName).OrderPrice = newValue;
            Save(biztype);
            return true;
        }

        public static bool ChangeMaxPrice(int biztype, string productName, int newValue)
        {
            var pSettings = GetProductSettings(biztype).Find(s => s.Name == productName);
            if (newValue <= pSettings.MinPrice)
                return false;

            pSettings.MaxPrice = newValue;
            UpdateBusinessProductPrice(biztype, productName, pSettings);

            Save(biztype);
            return true;
        }

        public static bool ChangeMinPrice(int biztype, string productName, int newValue)
        {
            var pSettings = GetProductSettings(biztype).Find(s => s.Name == productName);
            if (newValue >= pSettings.MaxPrice)
                return false;

            pSettings.MinPrice = newValue;
            UpdateBusinessProductPrice(biztype, productName, pSettings);

            Save(biztype);
            return true;
        }

        public static void ChangeStockCapacity(int biztype, string productName, int newValue)
        {
            GetProductSettings(biztype).Find(s => s.Name == productName).StockCapacity = newValue;
            Save(biztype);
        }

        public static bool DeleteProduct(int biztype, string name)
        {
            var pSetts = GetProductSettings(biztype).FirstOrDefault(s => s.Name == name);

            if (pSetts != null)
                GetProductSettings(biztype).Remove(pSetts);


            Save(biztype);

            foreach (var biz in BusinessManager.BizList.Where(b => b.Value.Type == biztype))
            {
                var product = biz.Value.Products.FirstOrDefault(p => p.Name == name);

                if (product == null)
                    continue;

                biz.Value.Products.Remove(product);
                biz.Value.Save();
            }

            return true;
        }

        public static bool AddNewProduct(int biztype, string name, string maxMinType, int orderPrice = 100000, int maxPrice = 150000, int minPrice = 90000, int stockCapacity = 1)
        {
            if (GetProductSettings(biztype).Any(s => s.Name == name))
                return false;

            if (BusinessManager.AutoroomsBizTypes.Contains(biztype))
            {
                name = name.ToLower();
                orderPrice = PriceManager.GetPriceInDollars(TypePrice.Car, name, orderPrice);
                maxMinType = "%";
                maxPrice = 140;
                minPrice = 70;
            }
            ProductSettings setts = new ProductSettings
            {
                Name = name,
                OrderPrice = orderPrice,
                MaxMinType = maxMinType,
                MaxPrice = maxPrice,
                MinPrice = minPrice,
                StockCapacity = stockCapacity
            };

            GetProductSettings(biztype).Add(setts);
            if (VehicleManager.CarRoomCustom.Contains(biztype))
            {
                var hash = NAPI.Util.GetHashKey(setts.Name.ToLower());
                if (!VehicleManager.CustomModelNames.ContainsKey(hash))
                    VehicleManager.CustomModelNames.Add(hash, setts.Name.ToLower());
            }

            Save(biztype);

            foreach (var biz in BusinessManager.BizList.Where(b => b.Value.Type == biztype))
            {
                if (biz.Value.Products.Any(p => p.Name == name)) continue;

                biz.Value.Products.Add(new Product(setts.MaxMinType == "%" ? 100 : setts.MaxPrice, 0, setts.Name));
                biz.Value.Save();
            }

            return true;
        }

        private static void UpdateBusinessProductPrice(int biztype, string productName, ProductSettings newSetting)
        {
            Product product;
            foreach (var biz in BusinessManager.BizList.Where(b => b.Value.Type == biztype))
            {
                product = biz.Value.Products.FirstOrDefault(p => p.Name == productName);
                if (product == null) continue;

                if (product.Price > newSetting.MaxPrice)
                {
                    product.Price = newSetting.MaxPrice;
                    biz.Value.Save();
                    continue;
                }
                if (product.Price < newSetting.MinPrice)
                {
                    product.Price = newSetting.MinPrice;
                    biz.Value.Save();
                    continue;
                }

            }
        }

        public static void UpdateBusinessProductSetts(int biztype, string productName, ProductSettings newSetting)
        {
            if (!BusinessManager.BizList.Any()) return;

            foreach (Business biz in BusinessManager.BizList.Values.Where(b => b.Type == biztype))
            {
                Product product = biz.Products.FirstOrDefault(p => p.Name == productName);
                if (product == null) continue;

                if (newSetting.MaxMinType == "%") product.Price = 100;
                else product.Price = newSetting.MaxPrice;
            }
        }

    }
}
