using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using Whistler.Businesses.Manager.DTOs;
using Whistler.GUI;
using Newtonsoft.Json;
using Whistler.SDK;
using ServerGo.Casino.Business;
using Whistler.Businesses;
using Whistler.Docks;
using Whistler.Families;
using Whistler.Businesses.Manager;
using Whistler.MoneySystem;
using System.Data;
using Whistler.Helpers;
using Whistler.Businesses.Models;
using Whistler.MoneySystem.Models;
using Whistler.Common.Interfaces;
using Whistler.Common;
using Whistler.Entities;
using System.Drawing;
using System.Xml.Linq;

namespace Whistler.Core
{
    public class Business : IWhistlerProperty
    {
        public static Action<ExtPlayer, Business> PlayerEnterUnloadShape;

        public int ID { get; set; }
        public int OwnerID { get; set; }
        public int SellPrice { get; set; }
        public int Type { get; set; }
        public string Address { get; set; }
        public List<Product> Products { get; set; }
        public int BankAccount { get; set; }
        public int BankNalog { get; set; }
        public Vector3 EnterPoint { get; private set; }
        public Vector3 UnloadPoint { get; set; }
        public int FamilyPatronage { get; set; }
        public DateTime TakeoverDate { get; set; }
        public DateTime DayWithoutProducts { get; set; }
        public int ColshapeRange { get; set; } = 1;
        public uint Dimension { get; set; } = 0;

        public List<Order> Orders { get; set; }
        public string Name { get; set; }
        public List<PedDTO> Peds { get; set; }
        public Vector3 BlipPosition { get; set; }
        public List<Vector3> AdditionalPositions { get; set; }
        public bool Pledged { get; set; }

        public Vector3 CamPosition { get; private set; }

        public BusinessTypeModel TypeModel { get { return BusinessesSettings.GetBusinessSettings(Type); } }

        [JsonIgnore]
        public int BizTax
        {
            get
            {
                return Convert.ToInt32(SellPrice * MoneyConstants.PayTaxCoeffBusinessForHour);
            }
        }
        [JsonIgnore]
        internal CheckingAccount BankNalogModel => BankManager.GetAccount(BankNalog);
        [JsonIgnore]
        internal CheckingAccount BankAccountModel => BankManager.GetAccount(BankAccount);

        [JsonIgnore]
        public BizProfitDTO ProfitData { get; private set; }

        public OwnerType OwnerType => OwnerType.Personal;

        public PropertyType PropertyType => PropertyType.Business;
        public int CurrentPrice => SellPrice;

        public string PropertyName => TypeModel?.TypeName;

        [JsonIgnore]
        private Blip blip = null;
        [JsonIgnore]
        private InteractShape shape = null;
        [JsonIgnore]
        private List<InteractShape> additionalShape = null;

        [JsonIgnore]
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Business));
        public Business(int id, int sellPrice, int type, List<Product> products, Vector3 enterPoint)
        {
            ID = id;
            OwnerID = -1;
            SellPrice = sellPrice;
            Type = type;
            Products = products;
            EnterPoint = enterPoint;
            UnloadPoint = new Vector3();
            CamPosition = new Vector3();
            BankNalog = BankManager.CreateAccount(TypeBankAccount.BusinessNalog).ID;
            BankAccount = BankManager.CreateAccount(TypeBankAccount.Business).ID;
            FamilyPatronage = -1;
            DateTime now = DateTime.Now;
            TakeoverDate = now;
            DayWithoutProducts = now;
            ProfitData = new BizProfitDTO(now);

            Orders = new List<Order>();
            ColshapeRange = 1;
            Dimension = 0;
            BlipPosition = null;
            AdditionalPositions = new List<Vector3>();
            Name = "";
            Peds = new List<PedDTO>();

            CreateEnterColshape();
            CreateBlip();
            MySQL.Query("INSERT INTO businesses (id, owneruuid, sellprice, type, products, enterpoint, unloadpoint, banknew, bankacc, mafia, orders, name, peds, daywithoutproducts, camposition, profitData) " +
                $"VALUES (@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6, @prop7, @prop8, @prop9, @prop10, @prop11, @Prop12, @Prop13, @prop14, @prop15)",
                ID,
                OwnerID,
                SellPrice,
                Type,
                JsonConvert.SerializeObject(Products),
                JsonConvert.SerializeObject(EnterPoint),
                JsonConvert.SerializeObject(UnloadPoint),
                BankNalog,
                BankAccount,
                FamilyPatronage,
                JsonConvert.SerializeObject(Orders),
                Name,
                JsonConvert.SerializeObject(Peds),
                MySQL.ConvertTime(DayWithoutProducts),
                JsonConvert.SerializeObject(CamPosition),
                JsonConvert.SerializeObject(ProfitData)
            );
        }

        public Business(DataRow row)
        {
            ID = Convert.ToInt32(row["id"]);
            OwnerID = Convert.ToInt32(row["owneruuid"]);
            if (OwnerID < -1)
            {
                OwnerID = Main.PlayerUUIDs.GetValueOrDefault(row["owner"].ToString(), -1);
                UpdateOwner();
            }
            SellPrice = Convert.ToInt32(row["sellprice"]);
            Type = Convert.ToInt32(row["type"]);
            Products = JsonConvert.DeserializeObject<List<Product>>(row["products"].ToString());
            EnterPoint = JsonConvert.DeserializeObject<Vector3>(row["enterpoint"].ToString());
            CamPosition = JsonConvert.DeserializeObject<Vector3>(row["camposition"].ToString());
            UnloadPoint = JsonConvert.DeserializeObject<Vector3>(row["unloadpoint"].ToString());
            BankNalog = Convert.ToInt32(row["banknew"]);
            BankAccount = Convert.ToInt32(row["bankacc"]);
            Pledged = Convert.ToBoolean(row["pledged"]);
            if (BankNalogModel == null)
            {
                BankNalog = BankManager.CreateAccount(TypeBankAccount.BusinessNalog).ID;
                MySQL.Query("UPDATE `businesses` SET banknew = @prop1 WHERE id = @prop0", ID, BankNalog);
            }
            if (BankAccountModel == null)
            {
                BankAccount = BankManager.CreateAccount(TypeBankAccount.Business).ID;
                MySQL.Query("UPDATE `businesses` SET bankacc = @prop1 WHERE id = @prop0", ID, BankAccount);
            }
            FamilyPatronage = Convert.ToInt32(row["family"]);
            DateTime now = DateTime.Now;
            TakeoverDate = row["takeoverdate"] == DBNull.Value ? now : Convert.ToDateTime(row["takeoverdate"]);
            DayWithoutProducts = row["daywithoutproducts"] == DBNull.Value ? now : Convert.ToDateTime(row["daywithoutproducts"]);
            ProfitData = row["profitData"] == DBNull.Value ? new BizProfitDTO(now) : JsonConvert.DeserializeObject<BizProfitDTO>(row["profitData"].ToString());
            if (ProfitData.RecordSince.Year != now.Year || ProfitData.RecordSince.Month != now.Month) ProfitData.SetupRecordDate(now, true);
            Orders = new List<Order>();
            List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(row["orders"].ToString());
            if (orders != null && orders.Any())
            {
                foreach (Order order in orders) 
                {
                    if (order == null) continue;

                    order.Customer = this;
                    order.Taked = false;

                    Dock.CreateOrder(order);
                }
            }
            ColshapeRange = Convert.ToInt32(row["colshapeRange"]);
            Dimension = Convert.ToUInt32(row["dimension"]);
            BlipPosition = JsonConvert.DeserializeObject<Vector3>(row["blipPosition"].ToString());
            if (row["additionalpos"] != DBNull.Value)
                AdditionalPositions = JsonConvert.DeserializeObject<List<Vector3>>(row["additionalpos"].ToString());
            else
                AdditionalPositions = new List<Vector3>();
            Name = row["name"].ToString();
            Peds = JsonConvert.DeserializeObject<List<PedDTO>>(row["peds"].ToString()).Where(item => !BusinessManager.BadPedsModel.Contains(item.Model)).ToList();

            CreateEnterColshape();
            CreateBlip();
        }
        public void Save()
        {
            MySQL.Query("UPDATE `businesses` SET sellprice = @prop1, products = @prop2, family = @prop3, orders = @prop4, name = @prop5," +
                "peds = @prop6, colshapeRange = @prop7, enterpoint = @prop8, blipPosition = @prop9, dimension = @prop10, takeoverdate = @prop11, camposition = @prop12, profitData = @prop13 WHERE id = @prop0",
                this.ID, this.SellPrice, JsonConvert.SerializeObject(this.Products), this.FamilyPatronage, JsonConvert.SerializeObject(this.Orders), this.Name,
                JsonConvert.SerializeObject(this.Peds), this.ColshapeRange, JsonConvert.SerializeObject(this.EnterPoint), JsonConvert.SerializeObject(this.BlipPosition),
                this.Dimension, MySQL.ConvertTime(TakeoverDate), JsonConvert.SerializeObject(this.CamPosition), JsonConvert.SerializeObject(this.ProfitData));
        }
        public void UpdateOwner()
        {
            MySQL.Query("UPDATE `businesses` SET `owneruuid` = @prop0 WHERE `id` = @prop1", OwnerID, ID);
        }
        public void UpdateDayWithoutProducts()
        {
            DayWithoutProducts = DateTime.Now;
            MySQL.Query("UPDATE `businesses` SET `daywithoutproducts` = @prop0 WHERE `id` = @prop1", MySQL.ConvertTime(DayWithoutProducts), ID);
        }

        #region orders
        public void ComplyOrdered()
        {
            Task.Run(() =>
            {
                try
                {
                    var outOfDateOrders = Orders.Where(o => DateTime.Now.Subtract(o.OrderTime).TotalHours > 3 && o.Taked == false);
                    foreach (var order in outOfDateOrders.ToList()) ComplyOrder(order);
                }
                catch (Exception ex)
                {
                    _logger.WriteError($"ComplyOrdered:\n{ex}");
                }

            });
        }
        public void ComplyOrder(Order order)
        {
            if (!Orders.Contains(order)) return;
            Orders.Remove(order);
            if (Dock.CurrentOrders.Contains(order)) Dock.CurrentOrders.Remove(order);
            if (order.Products == null) return;

            foreach (var (orderedProductName, orderedProductAmount) in order.Products)
            {
                var product = Products.FirstOrDefault(p => p.Name == orderedProductName);
                if (product != null) product.Lefts += orderedProductAmount;
            }
            UpdateBlip();
        }

        public void UpdateBlip()
        {
            if (Type != 1) return;

            NAPI.Task.Run(() =>
            {
                byte color = 24;
                foreach (Product prod in Products)
                {
                    if (prod.Lefts >= 200) continue;

                    color = 49;
                    break;
                }
                if (blip.Color == color) return;

                blip?.Delete();
                blip = NAPI.Blip.CreateBlip(Convert.ToUInt32(TypeModel.BlipType), BlipPosition ?? EnterPoint, 1f, color, TypeModel.TypeName, 255, 0, true);
            });
        }
        #endregion

        #region Interact & Blip
        public void CreateBlip()
        {
            float blipScale;
            int bColor = TypeModel.BlipColor;
            bool bChanged = false;
            string bName = TypeModel.TypeName;
            switch (Type)
            {
                case 1:
                    {
                        bool changeColor = true;
                        foreach (Product prod in Products)
                        {
                            if (prod.Lefts >= 200) continue;
                            
                            changeColor = false;
                            break;
                        }
                        if (changeColor) bColor = 24; // Зелёный
                        blipScale = 1;
                        bChanged = true;
                        break;
                    }
                case 16:
                    blipScale = 1.5f;
                    break;
                case 18:
                    blipScale = 1f;
                    break;
                case 19:
                    blipScale = 1f;
                    CasinoManager.Casinos.Add(new ServerGo.Casino.Business.Casino(this));
                    break;
                default:
                    blipScale = 1;
                    break;
            }
            var _blipPosition = BlipPosition ?? EnterPoint;

            blip?.Delete();

            if (bChanged)
            {
                blip = NAPI.Blip.CreateBlip(Convert.ToUInt32(TypeModel.BlipType), _blipPosition, blipScale,
                                Convert.ToByte(bColor), bName, 255, 0, true);
            }
            else
            {
                blip = NAPI.Blip.CreateBlip(Convert.ToUInt32(TypeModel.BlipType), _blipPosition, blipScale,
                                Convert.ToByte(TypeModel.BlipColor), TypeModel.TypeName, 255, 0, true);
            }
            
        }


        internal void CreateEnterColshape()
        {
            if (shape != null)
            {
                shape.Destroy();
            }

            shape = CreateEnterShape(EnterPoint);

            if (additionalShape != null)
            {
                foreach (var addShape in additionalShape)
                {
                    addShape.Destroy();
                }
            }
            additionalShape = new List<InteractShape>();
            foreach (var pos in AdditionalPositions)
            {
                var newShape = CreateEnterShape(pos);
                additionalShape.Add(newShape);
            }
        }

        private InteractShape CreateEnterShape(Vector3 position)
        {

            var interactshape = InteractShape.Create(position, ColshapeRange, 3, Dimension)
                .AddOnEnterColshapeExtraAction((s, entity) =>
                {
                    try
                    {
                        SafeTrigger.SetData(entity, "BIZ_ID", ID);
                    }
                    catch (Exception e)
                    {
                        NAPI.Util.ConsoleOutput("shape.OnEntityEnterColshape: " + e.ToString());
                    }
                })
                .AddOnExitColshapeExtraAction((s, entity) =>
                {
                    try
                    {
                        SafeTrigger.SetData(entity, "BIZ_ID", -1);
                    }
                    catch (Exception e)
                    {
                        NAPI.Util.ConsoleOutput("shape.OnEntityExitColshape: " + e.ToString());
                    }
                });
            if (Type != 19) 
                interactshape.AddInteraction(BusinessManager.interactionPressed, "Для покупки товаров");
            interactshape.AddInteraction(BusinessManager.GetBusinessInformation, "Чтобы открыть инфопанель", Key.VK_I);
            return interactshape;
        }
        #endregion

        public void ChangeEnterPoint(Vector3 newPoint)
        {
            EnterPoint = newPoint;
            CreateEnterColshape();
            blip.Position = EnterPoint;

            var admins =  Trigger.GetAllPlayers()
                .Where(p => p != null && p.IsLogged() && p.Character.AdminLVL > 0)
                .ToArray();
            SafeTrigger.ClientEventToPlayers(admins, "businesses:setMarker", this.ID, newPoint, this.ColshapeRange);
        }
        public void SetOwner(int uuid)
        {
            UpdateDayWithoutProducts();

            int oldOwnerID = OwnerID;
            OwnerID = uuid;
            if (oldOwnerID > 0)
            {
                ExtPlayer oldOwner = Trigger.GetPlayerByUuid(OwnerID);
                if (oldOwner.IsLogged())
                {
                    BankNalogModel.UnSubscribe();
                    MainMenu.SendProperty(oldOwner);
                }

                CheckingAccount bank = BankManager.GetAccountByUUID(oldOwnerID);
                if (bank != null) Wallet.TransferMoney(BankAccountModel, bank, (int)BankAccountModel.IMoneyBalance, 0, "Перевод со счета бизнеса");
            }

            Wallet.SetBankMoney(BankAccount, 0);
            ProfitData.FullReset();
            if (OwnerID <= 0)
            {
                BusinessTypeModel bizSettings = BusinessesSettings.GetBusinessSettings(Type);
                if (bizSettings != null)
                {
                    foreach (ProductSettings prod in bizSettings.Products)
                    {
                        Product product = Products.FirstOrDefault(p => p.Name == prod.Name);
                        if (product == null) continue;

                        product.Price = prod.MaxMinType == "%" ? 100 : prod.MaxPrice;
                    }
                }
                foreach (Product prod in Products)
                {
                    prod.Lefts = 99999;
                }
                Wallet.SetBankMoney(BankNalog, 0);
            }
            else
            {
                if (oldOwnerID <= 0)
                {
                    Wallet.SetBankMoney(BankNalog, BizTax * BusinessManager.TaxPayedHours);
                    foreach (var prod in Products)
                    {
                        prod.Lefts = 0;
                    }
                }    
                var player = Trigger.GetPlayerByUuid(OwnerID);
                if (player.IsLogged())
                {
                    BankNalogModel.Subscribe(player, SellPrice);
                    MainMenu.SendProperty(player);
                }
            }
            UpdateBlip();
            UpdateOwner();
            Save();
        }
        public void DeletePropertyFromMember() => SetOwner(-1);

        public string GetBusinessName()
        {
            if (TypeModel != null)
                return TypeModel.TypeName;
            else
                return "Unknown";
        }

        public string GetOwnerName()
        {
            return Main.PlayerNames.GetValueOrDefault(OwnerID, "Government");
        }

        public BizInfoDTO GetInfoDTO()
        {
            return new BizInfoDTO
            {
                Name = string.IsNullOrEmpty(Name) ? TypeModel.TypeName : Name,
                Description = TypeModel.TypeName,
                ID = ID,
                Price = SellPrice,
                Owner = GetOwnerName(),
                Overseer = FamilyManager.GetFamilyName(FamilyPatronage),
                Purchaseable = !Main.PlayerNames.ContainsKey(OwnerID),
                Type = Type,
                CamPositionX = CamPosition.X,
                CamPositionY = CamPosition.Y,
                CamPositionZ = CamPosition.Z
            };
        }

        public void Destroy()
        {
            NAPI.Task.Run(() =>
            {
                blip.Delete();
                shape.Destroy();
            });
        }


        public void SetCam(Vector3 campos)
        {
            CamPosition = campos;
            MySQL.Query("UPDATE `businesses` SET `camposition` = @prop0 WHERE `id` = @prop1", JsonConvert.SerializeObject(CamPosition), ID);
        }

        #region Family
        public void SetPatronageFamily(int familyId)
        {
            if (familyId == FamilyPatronage)
                return;
            var oldFamId = FamilyPatronage;
            FamilyPatronage = familyId;
            TakeoverDate = DateTime.Now;
            FamilyManager.RemoveBusiness(oldFamId, this);
            FamilyManager.NewBusiness(familyId, this);
            Families.FamilyMenu.FamilyMenuManager.UpdateBusinessFamilyPatronage(this, familyId);
        }

        public int GetFamilyTax()
        {
            return 1000;
        }

        public dynamic GetFamilyData()
        {
            return new
            {
                name = GetBusinessName(),
                id = ID,
                img = 0,
                stats = new List<dynamic>
                {
                    new { desc = "Дата захвата", value = TakeoverDate.ToString("d") },
                    new { desc = "Прибыль в час", value = GetFamilyTax() },
                },
                income = GetFamilyTax(),
                owner = GetOwnerName(),
                famOwner = FamilyManager.GetFamilyName(FamilyPatronage),
            };
        }
        #endregion

        private int GetOrderedProducts(string name)
        {
            int count = 0;
            foreach (var order in Orders)
            {
                count += order.GetOrderedProducts(name);
            }
            return count;
        }

        public bool CheckLowLevelProducts(out int currPercent)
        {
            currPercent = 0;
            if (Type == 1)
            {
                bool lowProducts = false;
                foreach (Product prod in Products)
                {
                    if (prod.Lefts >= 200) continue;

                    lowProducts = true;
                    break;
                }
                return lowProducts;
            }
            int totalPrice = 0;
            int currentPrice = 0;
            var prods = TypeModel.Products;
            if (prods.Count == 0) return false;
            foreach (var prod in Products)
            {
                var prodSetts = prods.FirstOrDefault(item => item.Name == prod.Name);
                if (prodSetts != null)
                {
                    totalPrice += prodSetts.OrderPrice * prodSetts.StockCapacity;
                    currentPrice += prodSetts.OrderPrice * (prod.Lefts + GetOrderedProducts(prod.Name));
                }
            }
            currPercent = (int)(currentPrice / (double)totalPrice * 100);
            return totalPrice == 0 ? false : (currentPrice / (double)totalPrice * 100) < TypeModel.MinimumPercentProduct;
        }

        public bool TakeBusinessFromOwner(int price, string reason, string message = null)
        {
            if (Main.PlayerNames.ContainsKey(OwnerID))
            {
                if (price > 0 && !Pledged)
                {
                    Wallet.TransferMoney(MoneyManager.ServerMoney, BankManager.GetAccountByUUID(OwnerID), price, 0, reason);
                }
                if (message != null)
                {
                    ExtPlayer player = Trigger.GetPlayerByUuid(OwnerID);
                    if (player != null)
                    {
                        Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, message, 3000);
                    }
                }
                SetOwner(-1);

                Save();
                return true;
            }
            return false;
        }

        public ProductPrice GetPriceByProduct(Product product)
        {
            if (product == null) return null;

            ProductSettings productType = TypeModel.Products.FirstOrDefault(item => item.Name == product.Name);
            if (productType == null) return null;

            if (productType.MaxMinType == "%") 
            {
                double multiplier = (double)product.Price / 100.0;
                int productRealPrice = (int)Math.Round(productType.OrderPrice * multiplier, MidpointRounding.AwayFromZero);
                return new ProductPrice(productType.OrderPrice, productRealPrice, product.Lefts);
            }
            return new ProductPrice(productType.OrderPrice, product.Price, product.Lefts);
        }

        public ProductPrice GetPriceByProductName(string name)
        {
            Product product = Products.FirstOrDefault(item => item.Name == name);
            return GetPriceByProduct(product);
        }
        public ProductPrice GetPriceByProductId(int id)
        {
            if (Products.Count <= id) return null;
            return GetPriceByProduct(Products[id]);
        }

        public BuyProductModel GetProductPrice(string name, int priceModule = 0)
        {
            var product = Products.FirstOrDefault(item => item.Name == name);
            if (product != null)
            {
                var productType = TypeModel.Products.FirstOrDefault(item => item.Name == name);
                if (productType != null)
                {
                    int count = priceModule <= 0 ? 1 : priceModule / productType.OrderPrice;
                    if (productType.MaxMinType == "%")
                        return new BuyProductModel((priceModule > 0 ? priceModule : 1) * product.Price / 100, count);
                    else
                        return new BuyProductModel(count * product.Price, count);
                }
                else
                    return new BuyProductModel(product.Price, 1);
            }
            return null;
        }
        public BuyProductModel GetProductPriceByProductId(int id, int priceModule = 0)
        {
            if (Products.Count > id)
                return GetProductPrice(Products[id].Name, priceModule);
            return null;
        }

        public void SetPledged(bool status)
        {
            Pledged = status;
            MySQL.Query("UPDATE `businesses` SET `pledged` = @prop0 WHERE `id` = @prop1", Pledged, ID);
        }
    }
}