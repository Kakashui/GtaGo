using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.SDK;
using Whistler.MoneySystem;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Linq;
using Whistler.Houses;
using Whistler.ClothesCustom;
using Whistler.VehicleSystem;
using Whistler.Businesses;
using Whistler.Helpers;
using Whistler.VehicleSystem.Models.VehiclesData;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Core.nAccount;
using Whistler.NewDonateShop;
using Whistler.Fractions;
using ServerGo.Casino.Business;
using Whistler.Common;
using Whistler.PriceSystem;
using Whistler.Entities;
using Whistler.Jobs.SteelMaking;
using Whistler.Families.Models;
using Whistler.Families;
using System.ComponentModel;
using System.Reflection;
using Whistler.Houses.Models;

namespace Whistler.GUI
{
    class MainMenu : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(MainMenu));

        // public static void UpdateBonusPoints(ExtPlayer player)
        // {
        //     SafeTrigger.ClientEvent(player,"mmenu:bp:update", player.Character.BonusPoints);
        // }
        public static void SendProperty(ExtPlayer player)
        {
            try
            {
                if (!player.IsLogged()) return;

                Core.Character.Character acc = player.Character;

                House house = HouseManager.GetHouse(player);
                Garage garage = null;
                if (house != null && GarageManager.Garages.ContainsKey(house.GarageID)) garage = GarageManager.Garages[house.GarageID];
                HomeDTO homeData = house == null ? null : new HomeDTO()
                {
                    Number = house.ID,
                    Style = HouseManager.HouseTypeList[house.Type].Name,
                    GarageSpace = garage == null ? 0 : garage.GarageConfig.MaxCars
                };

                Business biz = BusinessManager.GetBusinessByOwner(acc.UUID);
                BusinessDTO businessDTO = biz == null ? null : new BusinessDTO()
                {                       
                    number = biz.ID,
                    type = biz.Type,
                    name = biz.Name,
                    tax = biz.BizTax,
                    taxCount = (int)biz.BankNalogModel.Balance,
                    price = biz.SellPrice
                };

                List<VehicleDTO> vehs = new List<VehicleDTO>();
                VehicleManager.Vehicles.Where(v => v.Value.OwnerID == acc.UUID && v.Value.OwnerType == OwnerType.Personal).ToList().ForEach(v =>
                {
                    vehs.Add(new VehicleDTO
                    {
                        id = v.Value.ID,
                        name = v.Value.DisplayName,
                        numbers = v.Value.Number,
                        price = VehicleConstants.GetCorrectSalePrice(PriceManager.GetPriceInDollars(TypePrice.Car, v.Value.ModelName, 0), false)
                    });
                });
                PropertyDTO data = new PropertyDTO
                {
                    house = homeData,
                    business = businessDTO,
                    transport = vehs,
                };

                string json = JsonConvert.SerializeObject(data);
                SafeTrigger.ClientEvent(player, "mmenu:props:update", json);

            }
            catch (Exception e)
            {
                _logger.WriteError($"SendProperty:\n {e}");
            }
        }

        public static void SendStats(ExtPlayer player)
        {
            try
            {
                if (!player.IsLogged()) return;

                Core.Character.Character acc = player.Character;
                string lic = player.GetLicenses();
                if (lic == "") lic = "Gui_33";
                
                string work = "Gui_34";
                if (acc.WorkID > 0)
                {
                    int workid = acc.WorkID-1;
                    string workName = Jobs.WorkManager.JobStats.ElementAtOrDefault(workid);
                    if (workName != null) work = workName;
                }
                string fraction = null;
                if(acc.FractionID > 0) fraction = Configs.GetConfigOrDefault(acc.FractionID).Name;
                int fractionRank = acc.FractionLVL;
                string fractionRankName = "none";
                if(acc.FractionID > 0)
                {
                    Fractions.Models.Fraction fr = Manager.GetFraction(player);
                    if (fr != null)
                    {
                        Fractions.Models.FractionRank frRank = fr.GetRank(player);
                        if (frRank != null) fractionRankName = frRank.RankName;
                    } 
                }
                string number = "-";
                if (acc.PhoneTemporary.Phone != null) number = acc.PhoneTemporary.Phone.SimCard.Number.ToString();
                string fam = null;
                if (acc.FamilyID > 0)
                {
                    Family family = FamilyManager.GetFamily(acc.FamilyID);
                    if (family != null) fam = family.Name;
                }

                SafeTrigger.SetSharedData(player, "lvl", acc.LVL);
                SlotInfo info = null;
                if (Main.PlayerSlotsInfo.ContainsKey(acc.UUID)) info = Main.PlayerSlotsInfo[acc.UUID];
                StatsDTO data = new StatsDTO
                {
                    username = acc.FullName,
                    level = acc.LVL,
                    exp = acc.EXP,
                    organization = fraction,
                    rank = fractionRank,
                    rankName = fractionRankName,
                    phoneNumber = number,
                    bans = 0,
                    warns = acc.Warns,
                    licenses = lic,
                    registrationDate = acc.CreateDate,
                    passportNumber = acc.UUID.ToString(),
                    bankCount = acc.BankModel == null ? "0" : acc.BankModel.Number.ToString(),
                    work = work,
                    maritalStatus = new MaritalStatus(info != null ? info.Gender : acc.Customization.Gender, acc.Partner),
                    family = fam
                };

                string json = JsonConvert.SerializeObject(data);
                SafeTrigger.ClientEvent(player, "mmenu:stats:update", json);

            }
            catch (Exception e)
            {
                _logger.WriteError($"SendStats:\n {e}");
            }
        }

        private static int GetImgIdByMoneyAmount(int amount)
        {
            return amount switch
            {
                int n when n <= 100000 => 1,
                int n when n > 100000 && n <= 500000 => 2,
                int n when n > 500000 && n <= 1000000 => 3,
                int n when n > 1000000 && n <= 3000000 => 4,
                int n when n > 3000000 && n <= 5000000 => 5,
                int n when n > 5000000 => 6,
                _ => 1,
            };
        }

        public static void LoadDonateList(ExtPlayer player)
        {
            try
            {
                if (!player.IsLogged()) return;

                List<MoneyDonate> dataMoney = new List<MoneyDonate>();
                //List<DonateDTO> dataDonate = new List<DonateDTO>();
                int premiumPrice = 0;

                foreach(NewDonateShop.Models.Donate dt in DonateService.DonateList)
                {
                    if (dt == null) continue;

                    if (dt.Type == 1) 
                    {
                        premiumPrice = dt.Price;
                        continue;
                    }
                    if (dt.Type == 2)
                    {
                        dataMoney.Add(new MoneyDonate() 
                        { 
                            id = dt.Id, 
                            name = dt.Name, 
                            price = dt.Price, 
                            imgid = GetImgIdByMoneyAmount(dt.Count)
                        });
                        continue;
                    }
                    if (dt.Type == 3)
                    {
                        SafeTrigger.ClientEvent(player,"mmenu:donateExclusive:update", dt.Price, dt.Count, dt.MaxCount);
                        continue;
                    }
                }

                string jsonMoney = JsonConvert.SerializeObject(dataMoney);
                SafeTrigger.ClientEvent(player,"mmenu:donate:update", premiumPrice, jsonMoney);

            }
            catch (Exception e)
            {
                _logger.WriteError($"SendStats:\n {e}");
            }
        }

        [RemoteEvent(Events.GET_STATS)]
        public static void GetStats(ExtPlayer player) {
            try
            {
                SendStats(player);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"SendStats:\n {ex}");
            }
        }

        [RemoteEvent("mmenu:biz:sell")]
        public static void SellBizToGov(ExtPlayer player, int bizId)
        {
            try
            {
                
                if (!BusinessManager.BizList.ContainsKey(bizId))
                    SendProperty(player);
                else
                {
                    var biz = player.GetBusiness();
                    if (biz == null)
                        return;
                    if (biz.Pledged)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Biz_182", 3000);
                        return;
                    }
                    var price = biz.SellPrice / 100 * 70;
                    
                    DialogUI.Open(player, "Biz_96_1".Translate( price), new List<DialogUI.ButtonSetting>
                    {
                        new DialogUI.ButtonSetting
                        {
                            Name = "House_58",
                            Icon = null,
                            Action = p =>
                            {
                                biz.TakeBusinessFromOwner(price, "Money_SellBiz".Translate(biz.ID));
                                SendProperty(player);
                                Notify.Send(p, NotifyType.Success, NotifyPosition.BottomCenter, "Biz_72".Translate(price), 3000);
                            } 
                        },

                        new DialogUI.ButtonSetting
                        {
                            Name = "House_59",
                            Icon = null,
                            Action = p => {}
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError($"SellBizToGov:\n {ex}");
            }
        }

        [RemoteEvent("mmenu:products:get")]
        public static void GetProducts(ExtPlayer player, int bizId)
        {
            try
            {
                if (!BusinessManager.BizList.ContainsKey(bizId))
                    SendProperty(player);
                else
                {
                    var biz = BusinessManager.BizList[bizId];
                    var data = new List<ProductDTO>();
                    biz.Products.ForEach(p =>
                    {
                        var setts = biz.TypeModel.Products.FirstOrDefault(s => s.Name == p.Name);
                        data.Add(new ProductDTO
                        {
                            title = p.Name,
                            price = p.Price,
                            curCount = p.Lefts,
                            maxCount = setts == null ? 0 : setts.StockCapacity
                        });
                    });
                    SafeTrigger.ClientEvent(player,"mmenu:products:update", JsonConvert.SerializeObject(data));
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError($"GetProducts:\n {ex}");
            }
            
        }

        [RemoteEvent("mmenu:product:price:set")]
        public static void SetProductPrice(ExtPlayer player, int bizId, string pName, int price)
        {
            try
            {
                BusinessManager.BizNewPrice(player, price, bizId, pName);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"SetProductPrice:\n {ex}");
            }
        }

        [RemoteEvent("mmenu:biz:sell:player")]
        public static void CellBusinessToPlayer(ExtPlayer player, int id, int price)
        {
            try
            {
                if (Trigger.GetPlayerByUuid(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_281", 3000);
                    return;
                }
                BusinessManager.sellBusinessCommand(player, Trigger.GetPlayerByUuid(id), price);
            }
            catch (Exception ex)
            {

                _logger.WriteError($"CellBusinessToPlayer:\n {ex}");
            }
        }

        [RemoteEvent("mmenu:biz:name:set")]
        public static void SetProductPrice(ExtPlayer player, int bizId, string name)
        {
            try
            {
                var biz = BusinessManager.GetBusinessByOwner(player);
                if (biz == null || biz.GetOwnerName() != player.Name) return;
                BusinessManager.SetNewBusinessName(player, bizId, name);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"SetProductPrice:\n {ex}");
            }
        }

        private static Dictionary<JobWaypoints, Vector3> _wayPoints = new Dictionary<JobWaypoints, Vector3> {
            {JobWaypoints.BusDriver, new Vector3(435.2562, -645.7712, 32.26001) },
            {JobWaypoints.Electric, new Vector3(2756.224, 3480.598, 54.49258) },
            {JobWaypoints.Loader, new Vector3(1196.618, -3253.16, 5.975182) },
            {JobWaypoints.Fermer, new Vector3(1904.276, 4909.896, 47.62975) },
            {JobWaypoints.CarTheif, new Vector3(1417.386, 6343.156, 22.88101) },
            {JobWaypoints.Hunter, new Vector3(-2195.411, 4277.419, 48.05609) },
            {JobWaypoints.Pylot, new Vector3(-1187.935, -2936.729, 12.82468) },
            {JobWaypoints.Trucker, new Vector3(847.11, -902.2193, 24.13147) },
            {JobWaypoints.Taxi, new Vector3() },
            {JobWaypoints.Sawmill, new Vector3() },
            {JobWaypoints.Miner, OreMiningSettings.OreVeinBlips.GetRandomElement() },
            {JobWaypoints.WeaponMaster, new Vector3(-262.8346, -2660.437, 6.449978) },
            {JobWaypoints.MetalMakePoint, OreMiningSettings.PointMetal.Position }
        };
        private static int id;
        private static string name;

        [RemoteEvent("mmenu:job:waypoint")]
        public void SetJobWayPoint(ExtPlayer player, int id)
        {
            try
            {
                var point = (JobWaypoints)id;
                if (!_wayPoints.ContainsKey(point)) return;
                player.CreateWaypoint(_wayPoints[point]);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "mmain:wp:jobpoint", 3000);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"SetJobWayPoint:\n {ex}");
            }
        }

        [RemoteEvent("mmenu:vehicle:togarage")]
        public void SendVehToGarage(ExtPlayer player, int vehId)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!VehicleManager.Vehicles.ContainsKey(vehId)) return;
                var vData = VehicleManager.Vehicles[vehId];
                if (vData.IsDeath == true)
                    vData.IsDeath = false;
                GarageManager.SendVehicleIntoGarage(vData);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "House_96", 3000);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"SendVehToGarage:\n {ex}");
            }
        }

        [RemoteEvent("mmenu:vehicle:sell")]
        public void SellVeh(ExtPlayer player, int vehId)
        {
            VehicleOperations.SellVeh(player, vehId);
        }

        [RemoteEvent("mmenu:vehicle:makekey")]
        public void MakeCarKey(ExtPlayer player, int vehId)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!VehicleManager.Vehicles.ContainsKey(vehId)) return;
                var vData = VehicleManager.Vehicles[vehId] as PersonalBaseVehicle;
                House house = HouseManager.GetHouse(player, true);
                if (house != null)
                {
                    var garage = GarageManager.Garages[house.GarageID];
                    if (player.Character.InsideGarageID != garage.ID)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_99", 3000);
                        return;
                    }
                }
                var key = ItemsFabric.CreateCarKey(ItemNames.CarKey, vehId, vData.KeyNum, false);
                if (key == null)
                {
                    return;
                }
                if (!player.GetInventory().AddItem(key))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_100", 3000);
                    return;
                }
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "House_101".Translate( vData.Number), 3000);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"MakeCarKey:\n {ex}");
            }
        }

        [RemoteEvent("mmenu:vehicle:changekey")]
        public void ChangeCarKey(ExtPlayer player, int vehId)
        {
            try
            {
                if (!VehicleManager.Vehicles.ContainsKey(vehId)) return;
                var vData = VehicleManager.Vehicles[vehId] as PersonalBaseVehicle;
                House house = HouseManager.GetHouse(player, true);
                if (house != null)
                {
                    var garage = GarageManager.Garages[house.GarageID];
                    if (player.Character.InsideGarageID != garage.ID)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_99", 3000);
                        return;
                    }
                }
                vData.KeyNum++;
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "House_104".Translate( vData.Number), 3000);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"ChangeCarKey:\n {ex}");
            }
            
        }

        [RemoteEvent("mmenu:vehicle:transferfamily")]
        public void TransferFamily(ExtPlayer player, int vehId)
        {
            try
            {
                if (!VehicleManager.Vehicles.ContainsKey(vehId)) return;
                var vData = VehicleManager.Vehicles[vehId] as PersonalBaseVehicle;
                if (vData.PropertyBuyStatus != PropBuyStatus.Bought)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_146", 3000);
                    return;
                }
                if (vData.Pledged) return;
                if (player.GetFamily() == null)
                    return;

                var vehicles = VehicleManager.getAllHolderVehicles(player.Character.FamilyID, OwnerType.Family);
                if (vehicles.Count >= 50)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_137", 3000);
                    return;
                }

                DialogUI.Open(player, "House_136".Translate(vData.ModelName, vData.Number), new List<DialogUI.ButtonSetting>
                {
                    new DialogUI.ButtonSetting
                    {
                        Name = "dialog_0",
                        Icon = null,
                        Action = (p) =>
                        {
                            vData.DestroyVehicle();

                            FamilyVehicle vDataNew = new FamilyVehicle(p.Character.FamilyID, vData);
                            VehicleManager.Vehicles[vData.ID] = vDataNew;
                            vDataNew.Save();

                            Notify.Send(p, NotifyType.Success, NotifyPosition.BottomCenter, "House_135", 3000);
                            GarageManager.SendVehicleIntoGarage(vDataNew);
                        }
                    },
                    new DialogUI.ButtonSetting
                    {
                        Name = "dialog_1",
                        Icon = null,
                        Action = { }
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.WriteError($"TransferFamily:\n {ex}");
            }
        }

        [RemoteEvent("mmain:pwd:change")]
        public void ChangePwd(ExtPlayer player, string oldPwd, string newPwd)
        {
            try
            {
                if (!player.IsLogged()) return;
                var acc = player.Account;
                var oldPass = Account.GetSha256(oldPwd);
                if(oldPass != acc.Password)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "mmain:pwd:wrong", 3000);
                    return;
                }
                acc.changePassword(newPwd);
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "mmain:pwd:success", 3000);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"ChangePwd:\n {ex}");
            }
        }

        [RemoteEvent("mmenu:cars:sell:toplayer")]
        public void SellToPed(ExtPlayer player, int carId, int playerId, int price)
        {
            try
            {
                var target = Trigger.GetPlayerByUuid(playerId);
                if (target == null || !target.IsLogged() || player.Position.DistanceTo(target.Position) > 1.5)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_16", 3000);
                    return;
                }

                if (target.Session.VehicleSeller != null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Подождите, этому игроку уже предлагают купить машину.", 3000);
                    return;
                }

                House house = HouseManager.GetHouse(target, true);
                if (house == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_116", 3000);
                    return;
                }
                if (house.GarageID == 0)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_117", 3000);
                    return;
                }
                if (VehicleManager.getAllHolderVehicles(target.Character.UUID, OwnerType.Personal).Count >= house.HouseGarage.GarageConfig.MaxCars)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_118", 3000);
                    return;
                }
                if (target.Character.Money < price)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Biz_56", 3000);
                    return;
                }


                if (!VehicleManager.Vehicles.ContainsKey(carId))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_119", 3000);
                    return;
                }
                var vData = VehicleManager.Vehicles[carId] as PersonalVehicle;

                if (!vData.CanAccessVehicle(player, AccessType.SellDollars) && !vData.CanAccessVehicle(player, AccessType.SellRouletteCar))
                {
                    Notify.Send(player, NotifyType.Info, NotifyPosition.Bottom, "House_89", 3000);
                    return;
                }
                if (price < 0) return;
                //if (!VehicleOperations.CheckCorrectVehiclePrice(player, vData.ModelName, price)) return;

                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Main_120".Translate( target.Name, vData.ModelName, vData.Number, price), 3000);

                SafeTrigger.ClientEvent(target, "openDialog", "BUY_CAR", $"{player.Name} ({player.Character.UUID}) предложил вам купить автомобиль {vData.ModelName} ({vData.Number}) за ${price}");
                target.Session.VehicleSeller = player;
                target.Session.VehicleNumber = carId;
                target.Session.VehiclePrice = price;
            }
            catch (Exception e)
            {
                _logger.WriteError($"SellToPed:\n{e}");
            }

        }

        [RemoteEvent("player::changeimage")]
        public void ChangeAvatar(ExtPlayer player, string image)
        {
            try
            {
                player?.Character?.SetImage(image);
                CasinoManager.ChangeImage(player, image);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"ChangeAvatar:\n {ex}");
            }
        }
    }
}
