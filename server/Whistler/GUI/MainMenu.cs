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

namespace Whistler.GUI
{
    class MainMenu : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(MainMenu));

        public static void UpdateBonusPoints(PlayerGo player)
        {
            player.TriggerEvent("mmenu:bp:update", player.Character.BonusPoints);
        }
        public static void SendProperty(Player player)
        {
            try
            {
                if (!player.IsLogged()) return;
                Core.Character.Character acc = player.GetCharacter();

                var home = HouseManager.GetHouse(player) != null;

                var biz = BusinessManager.GetBusinessByOwner(acc.UUID);
                var businessDTO = biz == null ? null
                    : new BusinessDTO()
                    {                       
                        number = biz.ID,
                        type = biz.Type,
                        name = biz.Name,
                        tax = biz.BizTax,
                        taxCount = (int)biz.BankNalogModel.Balance,
                        price = biz.SellPrice
                    };

                var vehs = new List<VehicleDTO>();
                VehicleManager.Vehicles.Where(v => v.Value.OwnerID == acc.UUID && v.Value.OwnerType == OwnerType.Personal).ToList().ForEach(v =>
                {
                    vehs.Add(new VehicleDTO
                    {
                        id = v.Value.ID,
                        name = v.Value.DisplayName,
                        numbers = v.Value.Number,
                        price = VehicleConstants.GetCorrectSalePrice(PriceManager.GetPriceInDollars(TypePrice.Car, v.Value.ModelName, 0), player.GetAccount().IsPrimeActive())
                    });
                });
                var data = new PropertyDTO
                {
                    house = home,
                    business = businessDTO,
                    transport = vehs,
                };

                string json = JsonConvert.SerializeObject(data);
                Trigger.ClientEvent(player, "mmenu:props:update", json);

            }
            catch (Exception e)
            {
                _logger.WriteError($"SendProperty:\n {e}");
            }
        }

        public static void SendStats(Player player)
        {
            try
            {
                if (!player.IsLogged()) return;
                Core.Character.Character acc = player.GetCharacter();

                long bank = acc.BankModel.Balance;

                string lic = player.GetLicenses();
                if (lic == "") lic = "Gui_33";

                string work = (acc.WorkID > 0) ? Jobs.WorkManager.JobStats[acc.WorkID - 1] : "Gui_34";
                string fraction = Configs.GetConfigOrDefault(acc.FractionID).Name;
                int fractionRank = acc.FractionLVL;

                string number = acc.PhoneTemporary?.Simcard?.Number.ToString() ?? "Gui_36";

                if (!player.HasSharedData("lvl") || player.GetSharedData<int>("lvl") != acc.LVL)
                    player.SetSharedData("lvl", acc.LVL);

                var info = Main.PlayerSlotsInfo[acc.UUID];
                var data = new StatsDTO
                {
                    username = acc.FullName,
                    level = acc.LVL,
                    exp = acc.EXP,
                    organization = fraction,
                    rank = fractionRank,
                    phoneNumber = number,
                    bans = 0,
                    warns = acc.Warns,
                    licenses = lic,
                    registrationDate = acc.CreateDate,
                    passportNumber = acc.UUID.ToString(),
                    bankCount = acc.BankModel.Number.ToString(),
                    work = work,
                    maritalStatus = new MaritalStatus(info.Gender, acc.Partner)
                };

                string json = JsonConvert.SerializeObject(data);
                player.TriggerEvent("mmenu:stats:update", json);

            }
            catch (Exception e)
            {
                _logger.WriteError($"SendStats:\n {e}");
            }
        }

        [RemoteEvent(Events.GET_STATS)]
        public static void GetStats(Player player) {
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
        public static void SellBizToGov(Player player, int bizId)
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
        public static void GetProducts(Player player, int bizId)
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
                    player.TriggerEvent("mmenu:products:update", JsonConvert.SerializeObject(data));
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError($"GetProducts:\n {ex}");
            }
            
        }

        [RemoteEvent("mmenu:product:price:set")]
        public static void SetProductPrice(Player player, int bizId, string pName, int price)
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
        public static void CellBusinessToPlayer(Player player, int id, int price)
        {
            try
            {
                if (Main.GetPlayerByID(id) == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_281", 3000);
                    return;
                }
                BusinessManager.sellBusinessCommand(player, Main.GetPlayerByID(id), price);
            }
            catch (Exception ex)
            {

                _logger.WriteError($"CellBusinessToPlayer:\n {ex}");
            }
        }

        [RemoteEvent("mmenu:biz:name:set")]
        public static void SetProductPrice(Player player, int bizId, string name)
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

        [RemoteEvent("mmenu:job:waypoint")]
        public void SetJobWayPoint(Player player, int id)
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
        public void SendVehToGarage(Player player, int vehId)
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
        public void SellVeh(Player player, int vehId)
        {
            VehicleOperations.SellVeh(player, vehId);
        }

        [RemoteEvent("mmenu:vehicle:makekey")]
        public void MakeCarKey(PlayerGo player, int vehId)
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
                    if (player.GetCharacter().InsideGarageID != garage.ID)
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
        public void ChangeCarKey(Player player, int vehId)
        {
            try
            {
                if (!VehicleManager.Vehicles.ContainsKey(vehId)) return;
                var vData = VehicleManager.Vehicles[vehId] as PersonalBaseVehicle;
                House house = HouseManager.GetHouse(player, true);
                if (house != null)
                {
                    var garage = GarageManager.Garages[house.GarageID];
                    if (player.GetCharacter().InsideGarageID != garage.ID)
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
        public void TransferFamily(Player player, int vehId)
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

                var vehicles = VehicleManager.getAllHolderVehicles(player.GetCharacter().FamilyID, OwnerType.Family);
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

                            FamilyVehicle vDataNew = new FamilyVehicle(p.GetCharacter().FamilyID, vData);
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
        public void ChangePwd(Player player, string oldPwd, string newPwd)
        {
            try
            {
                if (!player.IsLogged()) return;
                var acc = player.GetAccount();
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
        public void SellToPed(Player player, int carId, int playerId, int price)
        {
            try
            {
                var target = NAPI.Pools.GetAllPlayers().FirstOrDefault(p => p.Value == playerId);
                if (target == null || !target.IsLogged() || player.Position.DistanceTo(target.Position) > 3)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_16", 3000);
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
                if (VehicleManager.getAllHolderVehicles(target.GetCharacter().UUID, OwnerType.Personal).Count >= house.HouseGarage.GarageConfig.MaxCars)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Main_118", 3000);
                    return;
                }
                if (target.GetCharacter().Money < price)
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
                if (!VehicleOperations.CheckCorrectVehiclePrice(player, vData.ModelName, price))
                    return;

                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Main_120".Translate( target.Name, vData.ModelName, vData.Number, price), 3000);

                target.TriggerEvent("openDialog", "BUY_CAR", "Main_121".Translate( player.Name, vData.ModelName, vData.Number, price));
                target.SetData("CAR_SELLER", player);
                target.SetData("CAR_NUMBER", carId);
                target.SetData("CAR_PRICE", price);
            }
            catch (Exception e)
            {
                _logger.WriteError($"SellToPed:\n{e}");
            }

        }

        [RemoteEvent("player::changeimage")]
        public void ChangeAvatar(Player player, string image)
        {
            try
            {
                player?.GetCharacter()?.SetImage(image);
                CasinoManager.ChangeImage(player, image);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"ChangeAvatar:\n {ex}");
            }
        }
    }
}
