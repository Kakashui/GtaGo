using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Whistler.GUI;
using Whistler.MoneySystem;
using Whistler.SDK;
using Whistler.Businesses;
using ServerGo.Casino.Business;
using System.Text.RegularExpressions;
using Whistler.VehicleSystem;
using Whistler.Helpers;
using Whistler.MoneySystem.Interface;
using Whistler.Businesses.Models;
using Whistler.Fractions;
using Whistler.Common;
using Whistler.PriceSystem;
using Whistler.Entities;
using Whistler.VehicleSystem.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Whistler.Core
{
    partial class BusinessManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(BusinessManager));

        public static int BusinessTakeInterval = 24 * 5; //hours
        public const int TaxPayedHours = 48;
        public const float BIZ_PROFIT_TAX = 0.1F;

        public static readonly List<int> AutoroomsBizTypes = new List<int>() { 2, 3, 4, 5, 15, 20, 21, 22, 23, 24, 25, 26, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38 };

        public static readonly List<int> BadPedsModel = new List<int>
        {
            848542878,
            442429178,
            22,
            12,
            1
        };
        public static void Init()
        {
            try
            {
                BusinessesSettings.Load();

                DataTable result = MySQL.QueryRead("SELECT * FROM businesses");
                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow Row in result.Rows)
                    {
                        Business data = new Business(Row);
                        BizList.Add(data.ID, data);
                    }
                } 
                else _logger.WriteError("DB biz return null result.");

                InteractionPressed += OnFurnitureStoreInteractionPressed;
                Main.Payday += () =>
                {
                    if (!BizList.Any()) return;

                    foreach (var biz in BizList.Values.ToList())
                    {
                        biz.ComplyOrdered();
                    }
                };
                //Main.OnPlayerReady += Main_OnPlayerReady;
                Fractions.TransportAlcohol.TransportManager.InitPoints(BizList.Where(item => item.Value.Type == 0 && item.Value.UnloadPoint != null).Select(item => item.Value.UnloadPoint).ToList());
                Whistler.Houses.Furnitures.FurnitureSettings.InitFurnitureSettings();
                FixPrices();
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT \"BUSINESSES\":\n" + e.ToString());
            }
        }

        [ServerEvent(Event.PlayerConnected)]
        public void OnPplayerConnected(ExtPlayer player)
        {
            Main_OnPlayerReady(player);
        }

        private static void FixPrices(bool updateDb = false)
        {
            if (!BizList.Any()) return;

            Product product;
            Businesses.Manager.BusinessTypeModel settings;
            foreach (Business business in BizList.Values)
            {
                if (business.OwnerID >= 0) continue;

                settings = BusinessesSettings.GetBusinessSettings(business.Type);
                if (settings == null) continue;

                foreach (ProductSettings prod in settings.Products)
                {
                    if (prod.MaxMinType == "%") 
                    {
                        product = business.Products.FirstOrDefault(p => p.Name == prod.Name);
                        if (product != null) product.Price = 100;
                    }
                    else
                    {
                        if (AutoroomsBizTypes.Contains(business.Type))
                        {
                            prod.MaxMinType = "%";
                            prod.MaxPrice = 140;
                            prod.MinPrice = 70;
                            product = business.Products.FirstOrDefault(p => p.Name == prod.Name);
                            if (product != null) product.Price = 100;
                            continue;
                        }

                        product = business.Products.FirstOrDefault(p => p.Name == prod.Name);
                        if (product != null && product.Price > prod.MaxPrice) product.Price = prod.MaxPrice;
                    }
                }
                if (!updateDb) continue;
                
                MySQL.Query("UPDATE `businesses` SET `products` = @prop0 WHERE `id`=@prop1", JsonConvert.SerializeObject(business.Products), business.ID);
            }
        }

        public static Business GetBusinessByOwner(int ownerUUID)
        {
            return BizList.FirstOrDefault(item => item.Value.OwnerID == ownerUUID).Value;
        }

        public static Business GetBusinessByOwner(ExtPlayer player)
        {
            if (!player.IsLogged()) return null;
            if (!BizList.Any()) return null;
            return BizList.Values.FirstOrDefault(item => item.OwnerID == player.Character.UUID);
        }       

        private static void Main_OnPlayerReady(ExtPlayer player)
        {
            try
            {
                // Setting peds in businesses.
                var peds = BizList.SelectMany(b => b.Value.Peds);
                SafeTrigger.ClientEvent(player,"businesses:setPeds", JsonConvert.SerializeObject(peds));
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT \"Main_OnPlayerReady\":\n" + e.ToString());
            }
        }

        public static void SavingBusiness()
        {
            if (!BizList.Any()) return;

            foreach (Business biz in BizList.Values)
            {
                if (biz == null) continue;

                biz.Save();
            }
        }

        [ServerEvent(Event.ResourceStop)]
        public void OnResourceStop()
        {
            try
            {
                SavingBusiness();
            }
            catch (Exception e) { _logger.WriteError("ResourceStop: " + e.ToString()); }
        }

        public static Dictionary<int, Business> BizList = new Dictionary<int, Business>();


        public enum BusinessType : int
        {
            Shop247 = 0,
            PetrolStation = 1,
            PremiumCarRoom = 2,
            SportCarRoom = 3,
            MiddleCarRoom = 4,
            MotoCarRoom = 5,
            GunShop = 6,
            ClothesShop = 7,
            BurgerShop = 8,
            TattooSalon = 9,
            BarberShop = 10,
            MasksShop = 11,
            LSCustoms = 12,
            Carwash = 13,
            PetShop = 14,
            SuperCarRoom = 15,
            Autorepair = 16,
            RentCar = 17,
            CarTrade = 18,
            Casino = 19,
            RetroCarRoom = 20,
            JDMCarRoom = 21,
            NewCarRoom1 = 22,
            NewCarRoom2 = 23,
            NewCarRoom3 = 24,
            NewCarRoom4 = 25,
            NewCarRoom5 = 26,
            FurnitureStore = 27,
            IllegalShop = 28,
            MercedesPage = 29,
            BmwPage = 30,
            RollsPage = 31,
            BentleyPage = 32,
            FerrariPage = 33,
            DiamondPage = 34,
            LuxePage = 35,
            JeepPage = 36,
            CasualPage = 37,
            GaragePage = 38,
            FarmHouse = 39,
            HandlingModShop = 40,
        }

        public static Business GetBusiness(int bizId) => BizList.GetValueOrDefault(bizId);

        public static List<Product> fillProductList(int type)
        {
            List<Product> products_list = new List<Product>();

            List<ProductSettings> productSettingsList = BusinessesSettings.GetProductSettings(type);
            foreach (ProductSettings prodSet in productSettingsList)
            {
                products_list.Add(new Product(prodSet.MaxMinType == "$" ? prodSet.OrderPrice : 100, 0, prodSet.Name));
            }
            
            return products_list;
        }

        [RemoteEvent("businesses::buyBusiness")]
        public static void InfoPanel_BuyBusinessEvent(ExtPlayer player, bool cashPay)
        {
            try
            {
                buyBusinessCommand(player, cashPay);
                GetBusinessInformation(player);
            }
            catch (Exception e) { _logger.WriteError($"Error catch on businesses::buyBusiness event: {e.ToString()}"); }
        }

        [Command("infob")]
        [RemoteEvent("businesses::openinfopanel")]
        public static void GetBusinessInformation(ExtPlayer player)
        {
            try
            {
                if (player.GetData<int>("BIZ_ID") == -1) return;

                Business biz = BizList[player.GetData<int>("BIZ_ID")];
                var dto = biz.GetInfoDTO();
                SafeTrigger.ClientEvent(player,"businesses:openInfoPanel", JsonConvert.SerializeObject(dto), JsonConvert.SerializeObject(new { money = player.GetMoneyPayment(PaymentsType.Cash).IMoneyBalance, bank = player.GetMoneyPayment(PaymentsType.Card).IMoneyBalance }));
            }
            catch (Exception e) { _logger.WriteError($"Error catch on infob cmd: {e}"); }
        }

        public static event Action<ExtPlayer, Business> InteractionPressed;
        public static void interactionPressed(ExtPlayer player)
        {
            if (player.GetData<int>("BIZ_ID") == -1) return;
            if (player.Character.Following != null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вас кто-то тащит за собой", 3000);
                return;
            }
            if (player.Character.Follower != null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Отпустить человека", 3000);
                return;
            }
            Business biz = BizList[player.GetData<int>("BIZ_ID")];
            InteractionPressed?.Invoke(player, biz);
            if (biz.OwnerID > 0 && !Main.PlayerNames.ContainsKey(biz.OwnerID))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Этот бизнес {biz.TypeModel.TypeName} пока-что не работает", 3000);
                return;
            }

            ExtVehicle playerVehicle = player.IsInVehicle ? player.Vehicle as ExtVehicle : null;
            if (AutoroomsBizTypes.Contains(biz.Type))
                OpenCarromMenu(player, biz.ID);
            else switch (biz.Type)
            {
                case 0:
                    OpenBizShopMenu(player);
                    break;
                case 1:
                    if (!player.IsInVehicle) return;
                    if (player.VehicleSeat != VehicleConstants.DriverSeat) return;
                    OpenPetrolMenu(player);
                    break;
                case 6:
                    SafeTrigger.SetData(player, "GUNSHOP", biz.ID);
                    WeaponShops.Open(player, biz);
                    break;
                case 7:
                    if (player.Character.OnDuty || player.Session.OnWork)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы должны закончить рабочий день", 3000);
                        return;
                    }

                    SafeTrigger.SetData(player, "CLOTHES_SHOP", biz.ID);
                    player.Character.ExteriorPos = player.Position;
                    SafeTrigger.UpdateDimension(player,  Dimensions.RequestPrivateDimension());
                    player.ChangePosition(new Vector3(-168.8666, -298.4844, 39.8));
                    player.Rotation = new Vector3(0, 0, 256.533);

                    SafeTrigger.ClientEvent(player, "openClothes", biz.Products[0].Price, new Vector3(-168.8666, -298.4844, 39.8), JsonConvert.SerializeObject(new { money = player.GetMoneyPayment(PaymentsType.Cash).IMoneyBalance, bank = player.GetMoneyPayment(PaymentsType.Card).IMoneyBalance }));
                    player.PlayAnimation("amb@world_human_guard_patrol@male@base", "base", 1);
                    break;
                case 8:
                    OpenBurgerShot(player, biz.ID);
                    break;
                case 9:
                    if (player.Character.OnDuty || player.Session.OnWork)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы должны закончить рабочий день", 3000);
                        return;
                    }
                    SafeTrigger.SetData(player, "BODY_SHOP", biz.ID);
                    player.Character.ExteriorPos = player.Position;
                    SafeTrigger.UpdateDimension(player,  Dimensions.RequestPrivateDimension());
                    player.ChangePosition(new Vector3(1864.089, 3747.348, 33.3));
                    player.Rotation = new Vector3(0, 0, 0);

                    SafeTrigger.ClientEvent(player, "tattoo:open", biz.Products[0].Price);
                    break;
                case 10:
                    OpenBarberShop(player, biz);
                    break;
                case 11:
                    if (player.Character.OnDuty || player.Session.OnWork)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы должны закончить рабочий день", 3000);
                        return;
                    }
                    SafeTrigger.SetData(player, "MASKS_SHOP", biz.ID);
                    SafeTrigger.ClientEvent(player, "openMasks",
                        biz.Products[0].Price,
                        JsonConvert.SerializeObject(new { money = player.GetMoneyPayment(PaymentsType.Cash).IMoneyBalance, bank = player.GetMoneyPayment(PaymentsType.Card).IMoneyBalance })
                    );
                    player.PlayAnimation("amb@world_human_guard_patrol@male@base", "base", 1);
                    player.Character.Customization.SetMaskFace(player, true);
                    break;
                case 12:
                    LsCustomOpen(player);
                    break;
                case 13:
                    CarWash.OpenCarWashMenu(player, biz);
                    break;
                case 14:
                    return;
                case 16:
                    if (!player.IsInVehicle)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Вы должны находиться в машине", 3000);
                        return;
                    }
                    if (playerVehicle.IsWearable())
                        Autorepair.OpenRepairCarMenu(player);
                    else
                        SafeTrigger.ClientEvent(player, "openDialog", "REPAIR_PAY", $"Вы хотите починить машину за {biz.Products[0].Price}$?");
                    break;
                case 18:
                    //TODO: download new system treading
                    break;
                case 19:
                    //CasinoManager.FindCasinoByBizId(player.GetData<int>("BIZ_ID")).OnPlayerTriedEnter(player);
                    break;
                case 28:
                    IllegalShop.OpenMenu(player, biz);
                    break;
                case 39:
                    FarmHouse.OpenFarmMenu(player, biz);
                    break;
                case 40:
                    HandlingModShop.OpenShop(player, false);
                    break;
            }
        }

        private static bool NotifyTakeProduct(ExtPlayer player, TakeProductResponse result, string messageDontFunc)
        {
            switch (result)
            {
                case TakeProductResponse.AllBought:
                    Notify.SendSuccess(player, "Вы купили все товары");
                    return true;
                case TakeProductResponse.PartBuyDontProduct:
                    Notify.SendInfo(player, "Вы купили не все товары: недостаточно товара на складе");
                    return true;
                case TakeProductResponse.PartBuyDontMoney:
                    Notify.SendInfo(player, "Вы купили не все товары: недостаточно денежных средств");
                    return true;
                case TakeProductResponse.PartBuyDontFunc:
                    if (messageDontFunc != null)
                        Notify.SendInfo(player, $"Вы купили не все товары: {messageDontFunc}");
                    return true;
                case TakeProductResponse.DontProduct:
                    Notify.SendError(player, "Недостаточно товара на складе");
                    return false;
                case TakeProductResponse.DontMoney:
                    Notify.SendError(player, "Недостаточно денежных средств");
                    return false;
                case TakeProductResponse.DontFunc:
                    if (messageDontFunc != null)
                        Notify.SendError(player, messageDontFunc);
                    return false;
            }
            return false;
        }
        public static bool TakeProd(ExtPlayer player, Business biz, IMoneyOwner from, List<BuyModel> products, string desc, string messageDontFunc)
        {
            return NotifyTakeProduct(player, TakeProd(biz, from, products, desc), messageDontFunc);
        }
        public static bool TakeProd(ExtPlayer player, Business biz, IMoneyOwner from, BuyModel product, string desc, string messageDontFunc)
        {
            return NotifyTakeProduct(player, TakeProd(biz, from, new List<BuyModel> { product }, desc), messageDontFunc);
        }

        private static TakeProductResponse TakeProd(Business biz, IMoneyOwner from, List<BuyModel> products, string desc)
        {
            TakeProductResponse result = TakeProductResponse.AllBought;
            int amount = 0;
            int profit = 0;
            IMoneyOwner to = biz.BankAccountModel ?? (IMoneyOwner)MoneyManager.ServerMoney;
            foreach (var itemProd in products)
            {
                var price = biz.GetPriceByProductName(itemProd.ProductName);
                if (price == null)
                {
                    // NAPI.Util.ConsoleOutput("PRICE CYKA NULL");
                    if (result == TakeProductResponse.AllBought)
                        result = TakeProductResponse.PartBuyDontProduct;
                    continue;
                }
                int count = itemProd.CountProduct;
                if (price.Lefts < count && biz.GetOwnerName() != "Government")
                {
                    // NAPI.Util.ConsoleOutput($"PRICE CYKA LEFTS < COUNT ({price.Lefts}<{count})");
                    if (result == TakeProductResponse.AllBought)
                        result = TakeProductResponse.PartBuyDontProduct;
                    if (itemProd.OnlyAllProduct || price.Lefts <= 0)
                    {
                        continue;
                    }
                    else
                        count = price.Lefts;
                }
                if (from.IMoneyBalance < amount + count * price.CurrentPrice)
                {
                    if (itemProd.OnlyAllProduct)
                    {
                        if (result == TakeProductResponse.AllBought)
                            result = TakeProductResponse.PartBuyDontMoney;
                        continue;
                    }
                    count = (int)(from.IMoneyBalance - amount) / price.CurrentPrice;
                    if (count <= 0)
                    {
                        if (result == TakeProductResponse.AllBought)
                            result = TakeProductResponse.PartBuyDontMoney;
                        continue;
                    }
                }
                var newCount = itemProd.BuyFunc(count);
                if (newCount == 0 || itemProd.OnlyAllProduct && newCount != count)
                    result = TakeProductResponse.PartBuyDontFunc;
                else
                {
                    amount += newCount * price.CurrentPrice;
                    if (price.OrderPrice < price.CurrentPrice)
                        profit += newCount * (price.CurrentPrice - price.OrderPrice);
                    if (newCount != count)
                        result = TakeProductResponse.PartBuyDontFunc;
                    var prod = biz.Products.FirstOrDefault(item => item.Name == itemProd.ProductName);
                    if (prod != null)
                        prod.Lefts -= newCount;
                }
            }
            int tax = (int)(profit * BIZ_PROFIT_TAX);
            int income = amount - tax;
            if (Wallet.TransferMoney(from, new List<(IMoneyOwner, int)>
            {
                (to, income),
                (Manager.GetFraction(6), tax),
            }, desc)) 
            {
                biz.ProfitData.NewIncome(income);
                return result;
            }
            else
            {
                switch (result)
                {
                    case TakeProductResponse.PartBuyDontProduct:
                        result = TakeProductResponse.DontProduct;
                        break;
                    case TakeProductResponse.PartBuyDontMoney:
                        result = TakeProductResponse.DontMoney;
                        break;
                    case TakeProductResponse.PartBuyDontFunc:
                        result = TakeProductResponse.DontFunc;
                        break;
                    default:
                        result = TakeProductResponse.DontMoney;
                        break;
                }
            }
            return result;
        }

        public static bool BusinessDepositMoney(ExtPlayer player, int amount)
        {
            var business = player.GetBusiness();
            if (business == null)
                return false;
            return Wallet.TransferMoney(player.Character.BankModel, business.BankAccountModel, amount, 0, "Перевод на счет бизнеса");
        }
        public static bool BusinessWithdrawMoney(ExtPlayer player, int amount)
        {
            var business = player.GetBusiness();
            if (business == null)
                return false;
            return Wallet.TransferMoney(business.BankAccountModel, player.Character.BankModel, amount, 0, "Перевод со счета бизнеса");
        }

        public static void BizNewPrice(ExtPlayer player, int price, int BizID, string prodName)
        {
            Business biz = GetBusiness(BizID);
            if (biz.OwnerID != player.Character.UUID) return;

            ProductSettings productSettings = null;

            if (biz.Type != (int)BusinessManager.BusinessType.RentCar)
            {
                productSettings = biz.TypeModel.Products.First(s => s.Name == prodName);
                if (price < productSettings.MinPrice || price > productSettings.MaxPrice)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Цена должна быть {productSettings.MinPrice} - {productSettings.MaxPrice}{productSettings.MaxMinType}", 3000);
                    return;
                }
            }
            else
            {
                if (price < 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Невернaя цена!", 3000);
                    return;
                }
            }

            Product product = biz.Products.FirstOrDefault(x => x.Name == prodName);
            if (product != null)
            {
                var vehHash = NAPI.Util.GetHashKey(prodName);
                product.Price = price;
                if (biz.Type == (int)BusinessManager.BusinessType.RentCar)
                {
                    var cars = VehicleManager.getAllHolderVehicles(biz.ID, OwnerType.Rent);
                    foreach (var rentCar in cars)
                    {
                        if (VehicleManager.Vehicles[rentCar].ModelName == prodName)
                            VehicleManager.Vehicles[rentCar].Price = price;
                    }
                }

                var mintype = productSettings == null ? "$" : productSettings.MaxMinType;
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Теперь {product.Name} стоит {price}{mintype}", 3000);
            }
        }

        public static void buyBusinessCommand(ExtPlayer player, bool cashPay)
        {
            if (!player.HasData("BIZ_ID") || player.GetData<int>("BIZ_ID") == -1)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы должны находиться около бизнеса", 3000);
                return;
            }
            int id = player.GetData<int>("BIZ_ID");
            Business biz = BusinessManager.BizList[id];
            if (player.GetBusiness() != null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Вы не можете приобрести больше 1 бизнеса", 3000);
                return;
            }
            if (biz.OwnerID > -1 && Main.PlayerNames.ContainsKey(biz.OwnerID))
            {
                if (biz.OwnerID == player.Character.UUID)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Этот бизнес принадлежит Вам", 3000);
                    return;
                }
                else
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Этот бизнес принадлежит другому игроку", 3000);
                    return;
                }
            }
            IMoneyOwner paymentFrom = player.GetMoneyPayment(cashPay ? PaymentsType.Cash : PaymentsType.Card);
            if (!Wallet.MoneySub(paymentFrom, biz.SellPrice, $"Покупка бизнеса #{biz.ID}"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно денежных средств", 3000);
                return;
            }
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Поздравляем! Вы купили {biz.TypeModel.TypeName}, не забудьте внести налог за него в банкомате", 3000);
            biz.SetOwner(player.Character.UUID);

            player.SendTip("Купив бизнес вовремя его оплачивайте и заказывайте товары в порту для ваших клиентов");
        }

        public static void UpdateBusinessCommand(int businessType, bool withPrice)
        {
            List<Product> products_list = BusinessManager.fillProductList(businessType);
            foreach (var biz in BizList.Values)
            {
                if (biz.Type == businessType)
                {
                    var newList = new List<Product>();
                    products_list.ForEach(product =>
                    {
                        var index = biz.Products.FindIndex(p => p.Name == product.Name);
                        if (index < 0)
                        {
                            newList.Add(product);
                        }
                        else
                        {
                            if (withPrice) 
                                biz.Products[index].Price = product.Price;
                            newList.Add(biz.Products[index]);
                        };
                    });
                    biz.Products = newList;
                    MySQL.Query("UPDATE `businesses` SET `products` = @prop0 WHERE `id`=@prop1", JsonConvert.SerializeObject(biz.Products), biz.ID);
                };
            }
        }

        private static Random _random = new Random();
        private static int GetNewID()
        {
            // var newId = 10000;
            // while (BizList.ContainsKey(newId))
            //     newId = _random.Next(10000, 15000);
            var newId = BizList.Values.Last().ID + 1;

            return newId;
        }

        [Command("setbizdim")]
        public static void HandleSetBusinessDimension(ExtPlayer player, int bizId, uint dimension)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "setbizdim")) return;

                var business = BizList[bizId];
                business.Dimension = dimension;
                business.ChangeEnterPoint(business.EnterPoint);
                BusinessManager.BizList[bizId].Save();
            }
            catch (Exception e) { _logger.WriteError("Unhandled exception catched on setbizdim: " + e.ToString()); }
        }

        [Command("setbizblippos")]
        public static void HandleSetBusinessBlipPosition(ExtPlayer player, int bizId)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "setbizblippos")) return;

                var business = BizList[bizId];
                business.BlipPosition = player.Position;
                business.CreateBlip();
                BusinessManager.BizList[bizId].Save();
            }
            catch (Exception e) { _logger.WriteError("Unhandled exception catched on setbizblippos: " + e.ToString()); }
        }

        public static void createBusinessCommand(ExtPlayer player, int govPrice, int type)
        {
            if (!Group.CanUseAdminCommand(player, "createbusiness")) return;
            if (BusinessesSettings.GetBusinessSettings(type) == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Бизнес с таким ID не существует", 3000);
                return;
            }
            var pos = player.Position;
            pos.Z -= 1.12F;
            List<Product> products_list = BusinessManager.fillProductList(type);

            var id = GetNewID();

            Business biz = new Business(id, govPrice, type, products_list, pos);
            BizList.Add(id, biz);

            if (type == 19)
            {
                MySQL.Query("INSERT INTO `casino`(`id`, `bizId`,`amount`, `stateTax`, `casinoTax`, `maxWinOfBet`) VALUES(@prop0, @prop1, @prop2, @prop3, @prop4, @prop5)", 0, biz.ID, 1000010, CasinoManager.StateShare, CasinoManager.CasinoShare, 100000);
            }
            
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы создали бизнес {biz.TypeModel.TypeName}", 3000);
            GameLog.Admin($"{player.Name}", $"createBiz({type}, {govPrice})", "");
        }
    
        public static void createBusinessUnloadpoint(ExtPlayer player, int bizid)
        {
            if (!Group.CanUseAdminCommand(player, "createunloadpoint")) return;
            var pos = player.Position;
            BizList[bizid].UnloadPoint = pos;
            MySQL.Query("UPDATE businesses SET unloadpoint=@prop0 WHERE id=@prop1", JsonConvert.SerializeObject(pos), bizid);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Успешно создана точка разгрузки для бизнеса ID: {bizid}", 3000);
        }

        public static void deleteBusinessCommand(ExtPlayer player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "deletebusiness")) return;
            Business biz = BusinessManager.GetBusiness(id);
            if (biz == null)
                return;
            biz.TakeBusinessFromOwner(biz.SellPrice, $"Продажа бизнеса {biz.ID}");
            MySQL.Query("DELETE FROM businesses WHERE id=@prop0", id);
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Вы удалили бизнес", 3000);
                        
            if (biz.Type == 19)
            {
                MySQL.Query("DELETE FROM `casino` WHERE bizId=@prop0", id);
            }
            biz.Destroy();
            BizList.Remove(biz.ID);
            GameLog.Admin(player.Name, $"deleteBiz({id})", "");
        }

        public static void sellBusinessCommand(ExtPlayer player, ExtPlayer target, int price)
        {
            if (!player.IsLogged() || !target.IsLogged()) return;

            if (player.Position.DistanceTo(target.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок далеко", 3000);
                return;
            }

            var biz = player.GetBusiness();
            if (biz == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У Вас нет бизнеса", 3000);
                return;
            }
            if (biz.Pledged)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Бизнес находится под залогом!", 3000);
                return;
            }

            if (target.GetBusiness() != null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок купил максимум бизнесов", 3000);
                return;
            }

            if (price < biz.SellPrice / 2 || price > biz.SellPrice * 3)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Невозможно продать бизнес за такую цену. Укажите цену от {biz.SellPrice / 2}$ до {biz.SellPrice * 3}$", 3000);
                return;
            }

            if (target.Character.Money < price)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У игрока недостаточно денег", 3000);
                return;
            }

            DialogUI.Open(target, $"{player.Name} предложил купить вам бизнес {biz.TypeModel.TypeName} за {price}$", new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "Да",// yes
                    Icon = "confirm",
                    Action = p =>
                    {
                        acceptBuyBusiness(p, player, price, biz);
                    }
                },

                new DialogUI.ButtonSetting
                {
                    Name = "Нет",// no
                    Icon = "cancel",
                    Action = p => {}
                }
            });
            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы предложили игроку {target.Character.UUID} купить ваш бизнес за {price}$", 3000);
        }

        public static void acceptBuyBusiness(ExtPlayer player, ExtPlayer seller, int price, Business biz)
        {
            if (!seller.IsLogged() || !player.IsLogged()) return;

            if (player.Position.DistanceTo(seller.Position) > 2)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Игрок далеко", 3000);
                return;
            }
            if (biz.OwnerID != seller.Character.UUID)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Этот бизнес больше не принадлежит игроку", 3000);
                return;
            }

            if (player.GetBusiness() != null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "У вас уже максимальное количество бизнесов", 3000);
                return;
            }
            if (!Wallet.TransferMoney(player.Character, seller.Character, price, 0, $"Покупка бизнеса #{biz.ID}"))
            {
                Notify.Send(seller, NotifyType.Error, NotifyPosition.BottomCenter, $"У игрока недостаточно денег", 3000);
                return;
            }

            biz.SetOwner(player.Character.UUID);

            MainMenu.SendProperty(player);
            MainMenu.SendProperty(seller);

            Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вы купили у {seller.Name.Replace('_', ' ')} бизнес {biz.TypeModel.TypeName} за {price}$", 3000);
            Notify.Send(seller, NotifyType.Info, NotifyPosition.BottomCenter, $"{player.Name.Replace('_', ' ')} купил ваш бизнес {biz.TypeModel.TypeName} за {price}$", 3000);
        }

        public static void SetNewBusinessName(ExtPlayer player, int bizId, string text)
        {
            var validText = Regex.Replace(text, @"[^0-9a-zA-Z\s]+", "");

            BizList[bizId].Name = validText;
            BizList[bizId].Save();
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы успешно изменили имя бизнеса на {validText}", 3000);
        }
    }

    public class Order
    {
        public List<(string, int)> Products { get; set; }

        [JsonIgnore]
        public Business Customer { get; set; }
        
        [JsonIgnore]
        public bool Taked { get; set; }

        [JsonIgnore]
        public int UID { get; set; }

        public DateTime OrderTime { get; set; }

        [JsonConstructor]
        public Order(List<(string, int)> products, string orderTime)
        {
            Products = products;
            if (!DateTime.TryParse(orderTime, out DateTime orderDate))
            {
                OrderTime = DateTime.Now;
                return;
            }

            OrderTime = orderDate;
        }

        public Order(List<(string, int)> products, Business customer, bool taked = false)
        {
            Products = products;
            Taked = taked;
            OrderTime = DateTime.Now;
            Customer = customer;
        }

        public int GetOrderedProducts(string name)
        {
            int count = 0;
            foreach (var prod in Products)
            {                
                if (string.Equals(prod.Item1, name, StringComparison.CurrentCultureIgnoreCase))
                    count += prod.Item2;
            }
            return count;
        }
    }

    public class Product
    {
        public Product(int price, int left, string name)
        {
            Price = price;
            Lefts = left;
            Name = name;
        }

        public int Price { get; set; }
        public int Lefts { get; set; }
        public string Name { get; set; }
    }
}
